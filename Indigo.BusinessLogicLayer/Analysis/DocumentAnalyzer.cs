using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Document;
    using Indigo.BusinessLogicLayer.Shingles;
    using Indigo.Tools;
    using Indigo.Tools.Converters;
    using Indigo.Tools.Parsers;

    public class DocumentAnalyzer
    {
        private const String DefaultTempApplicationFolder = "Indigo";
        private const String DefaultTempLematizationFilePostfix = "_LEM";
        private const Int32 MinimalWordsUsageInDocument = 2;

        #region Singltone
        
        private static DocumentAnalyzer _current;

        public static DocumentAnalyzer Current
        {
            get { return _current ?? (_current = new DocumentAnalyzer()); }
        }

        #endregion

        #region Properties

        public String TempDirectory { get; private set; }

        #endregion

        #region Methods

        public async Task<CompareResult> AnalyzeDocumentAsync(AnalysisTargetDocument targetDocument, DocumentAnalysisSettings settings)
        {
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(this.TempDirectory, "\\", tempTextFileName);
            String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName, DefaultTempLematizationFilePostfix);
            String tempLematizationFileFullName = String.Concat(this.TempDirectory, "\\", tempLematizationFileName);

            List<CompareResultSet> compareResultSets = new List<CompareResultSet>();

            try
            {
                // 1. Convert original document to txt format and create copy in temp folder
                using (IDocumentConverter converter = new MsWordDocumentConverter())
                {
                    await converter.ConvertDocumentToTextFileAsync(targetDocument.FilePath, tempTextFileFullName);
                }

                try
                {
                    // 2. Lematization
                    await LematizationTool.Current.ProcessDocumntAsync(tempTextFileFullName, tempLematizationFileFullName);
                    
                    // Parse temp documents to words
                    var words = (await ParserTool.Current.ParseFileAsync(tempLematizationFileFullName)).ToList();
                    StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();
                    
                    // Remove stop-words from origin text
                    List<String> modifiedWords = words.Select(x => x.ToLower()).ToList();
                    foreach (var stopWord in stopWordList)
                    {
                        modifiedWords.RemoveAll(word => String.Equals(word, stopWord.Content, StringComparison.CurrentCultureIgnoreCase));
                    }

                    #region Shingles algorithm

                    ShingleList shingleList = await ShingleList.CreateAsync(modifiedWords, settings.ShingleSize);
                    CheckSumCollection checkSumCollection = await CheckSumCollection.CreateAsync(shingleList, HashAlgorithmType.SHA256);

                    // TODO: Add caching checksum

                    DocumentList documentList = await DocumentList.GetAllDocumentsAsync();
                    var asyncTasks = documentList.Select(document => Task<CompareResultSet>.Factory.StartNew(() =>
                    {
                        ShingleList documentShingleList = ShingleList.GetAsync(document.DocumentId, settings.ShingleSize).Result;
                        CheckSumCollection documentCheckSumCollection = CheckSumCollection.Create(documentShingleList);
                        float matchingCoefficient = CheckSumComparer.CalculateMatchingPercentage(checkSumCollection, documentCheckSumCollection);
                        
                        ShinglesCompareResult shinglesCompareResult = new ShinglesCompareResult(settings.ShingleSize, matchingCoefficient);
                        CompareResultSet compareResultSet = new CompareResultSet(document.DocumentId, shinglesCompareResult);

                        return compareResultSet;
                    }));

                    compareResultSets = (await Task.WhenAll(asyncTasks)).ToList();

                    #endregion
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error{0}: {1}", e.HResult, e.Message);
                    throw;
                }
                finally
                {
                    // Remove temp files
                    File.Delete(tempTextFileFullName);
                    File.Delete(tempLematizationFileFullName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Filtration
            List<CompareResultSet> filterCompareResult =
                compareResultSets.Where(x => x.ShinglesCompareResult.CompareCoefficient >= settings.MinimalSimilarityLevel).ToList();

            CompareResult compareResult = new CompareResult(filterCompareResult, null);
            return compareResult;
        }

        public async Task<CompareResult> AnalyzeDocumentAsync_V2(AnalysisTargetDocument targetDocument, DocumentAnalysisSettings settings)
        {
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(this.TempDirectory, "\\", tempTextFileName);
            String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName, DefaultTempLematizationFilePostfix);
            String tempLematizationFileFullName = String.Concat(this.TempDirectory, "\\", tempLematizationFileName);

            List<CompareResultSet> compareResultSets = new List<CompareResultSet>();
            LsaResult lsaResult = null;

            try
            {
                // 1. Convert original document to txt format and create copy in temp folder
                using (IDocumentConverter converter = new MsWordDocumentConverter())
                {
                    await converter.ConvertDocumentToTextFileAsync(targetDocument.FilePath, tempTextFileFullName);
                }

                try
                {
                    // 2. Lematization
                    await LematizationTool.Current.ProcessDocumntAsync(tempTextFileFullName, tempLematizationFileFullName);

                    // Parse temp documents to words
                    var words = (await ParserTool.Current.ParseFileAsync(tempLematizationFileFullName)).ToList();
                    StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();

                    // Remove stop-words from origin text
                    List<String> modifiedWords = words.Select(x => x.ToLower()).ToList();
                    foreach (var stopWord in stopWordList)
                    {
                        modifiedWords.RemoveAll(word => String.Equals(word, stopWord.Content, StringComparison.CurrentCultureIgnoreCase));
                    }

                    #region Shingles algorithm

                    ShingleList shingleList = await ShingleList.CreateAsync(modifiedWords, settings.ShingleSize);
                    CheckSumCollection checkSumCollection = await CheckSumCollection.CreateAsync(shingleList, HashAlgorithmType.SHA256);

                    // TODO: Add caching checksum

                    DocumentList documentList = await DocumentList.GetAllDocumentsAsync();
                    var asyncTasks = documentList.Select(document => Task<CompareResultSet>.Factory.StartNew(() =>
                    {
                        ShingleList documentShingleList = ShingleList.GetAsync(document.DocumentId, settings.ShingleSize).Result;
                        CheckSumCollection documentCheckSumCollection = CheckSumCollection.Create(documentShingleList);
                        float matchingCoefficient = CheckSumComparer.CalculateMatchingPercentage(checkSumCollection, documentCheckSumCollection);

                        ShinglesCompareResult shinglesCompareResult = new ShinglesCompareResult(settings.ShingleSize, matchingCoefficient);
                        CompareResultSet compareResultSet = new CompareResultSet(document.DocumentId, shinglesCompareResult);

                        return compareResultSet;
                    }));

                    compareResultSets = (await Task.WhenAll(asyncTasks)).ToList();

                    #endregion

                    #region LSA

                    List<String> modifiedWordsUsageFilter = UsageFilter.Filter(modifiedWords, MinimalWordsUsageInDocument);
                    DocumentVector documentVector = DocumentVectorization.Vectorisation(modifiedWordsUsageFilter);

                    List<DocumentVector> documentsVectors = new List<DocumentVector>();
                    documentsVectors.Add(documentVector);
                    foreach (var item in documentList)
                    {
                        DocumentKeyWordList documentKeyWordList = await DocumentKeyWordList.GetAsync(item.DocumentId);
                        DocumentVector itemVector = DocumentVectorization.Vectorisation(documentKeyWordList);
                        documentsVectors.Add(itemVector);
                    }

                    LsaMatrix lsaMatrix = LsaMatrix.Create(documentsVectors.ToArray());
                    Svd svd = lsaMatrix.Svd(true);

                    var u = svd.U();
                    var w = svd.W();
                    var vt = svd.VT();

                    List<LsaResultSet> lsaResultSets = new List<LsaResultSet>();
                    for (int i = 0; i < vt.ColumnCount; i++)
                    {
                        LsaResultSet lsaResultSet = new LsaResultSet
                        {
                            DocumentId = documentsVectors[i].DocumentId,
                            X = Convert.ToSingle(vt[0, i]),
                            Y = Convert.ToSingle(vt[1, i])
                        };
                        lsaResultSets.Add(lsaResultSet);
                    }
                    lsaResult = LsaResult.Create(lsaResultSets);

                    #endregion
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error{0}: {1}", e.HResult, e.Message);
                    throw;
                }
                finally
                {
                    // Remove temp files
                    File.Delete(tempTextFileFullName);
                    File.Delete(tempLematizationFileFullName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Filtration
            List<CompareResultSet> filterCompareResult =
                compareResultSets.Where(x => x.ShinglesCompareResult.CompareCoefficient >= settings.MinimalSimilarityLevel).ToList();

            CompareResult compareResult = new CompareResult(filterCompareResult, lsaResult);
            return compareResult;
        }

        #endregion

        #region Constructors

        private DocumentAnalyzer()
        {
            this.TempDirectory = String.Concat(Path.GetTempPath(), "\\", DefaultTempApplicationFolder);
        }

        #endregion

        #region Helpers

        private List<List<DocumentList.Item>> SplitToSublist(DocumentList documentList, Int32 sublistSize)
        {
            List<List<DocumentList.Item>> sublists = documentList.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/sublistSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            return sublists;
        }

        #endregion
    }
}