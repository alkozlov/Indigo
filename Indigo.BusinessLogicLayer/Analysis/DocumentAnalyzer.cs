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
    using Tools = Indigo.Tools.Parsers;

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

        public async Task<CompareResult> FindSimilarDocumentsAsync(AnalysisTargetDocument targetDocument, DocumentAnalysisSettings settings)
        {
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(this.TempDirectory, "\\", tempTextFileName);
            String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName, DefaultTempLematizationFilePostfix);
            String tempLematizationFileFullName = String.Concat(this.TempDirectory, "\\", tempLematizationFileName);

            ShinglesResult shinglesResult = null;
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
                    var parserWords = (await Tools.ParserTool.Current.ParseDocumentAsync(tempLematizationFileFullName)).ToList();
                    List<DocumentWord> documentWords = ConvertToBusinessDocumentWords(parserWords);

                    // Parse origignal document to words
                    var parserOriginalWords = (await Tools.ParserTool.Current.ParseDocumentAsync(tempTextFileFullName)).ToList();
                    List<DocumentWord> originalDocumentWords = ConvertToBusinessDocumentWords(parserOriginalWords);
                    documentWords = MergeDocumentWordsPositions(originalDocumentWords, documentWords);

                    // Remove stop-words from origin text
                    List<DocumentWord> modifiedWords = await StopWordFilter.FilterAsync(documentWords);

                    #region Shingles algorithm

                    ShingleList shingleList = await ShingleList.CreateAsync(modifiedWords, settings.ShingleSize);
                    CheckSumCollection checkSumCollection = await CheckSumCollection.CreateAsync(shingleList, HashAlgorithmType.SHA256);

                    // TODO: Add caching checksum

                    DocumentList documentList = await DocumentList.GetAllDocumentsAsync();
                    var asyncTasks = documentList.Select(document => Task<ShinglesResultSet>.Factory.StartNew(() =>
                    {
                        ShingleList documentShingleList = ShingleList.GetAsync(document.DocumentId, settings.ShingleSize).Result;
                        CheckSumCollection documentCheckSumCollection = CheckSumCollection.Create(documentShingleList);
                        float matchingCoefficient = CheckSumComparer.CalculateMatchingPercentage(checkSumCollection, documentCheckSumCollection);
                        List<Shingle> similarShingles = CheckSumComparer.GetSimilarShingles(checkSumCollection, documentCheckSumCollection);

                        ShinglesResultSet shinglesResultSet =
                            new ShinglesResultSet(document.DocumentId, matchingCoefficient, similarShingles);
                        return shinglesResultSet;
                    }));

                    List<ShinglesResultSet> shinglesResultSets = (await Task.WhenAll(asyncTasks)).ToList();
                    List<ShinglesResultSet> filterShinglesResultSets =
                        shinglesResultSets.Where(x => x.MatchingCoefficient >= settings.MinimalSimilarityLevel).ToList();
                    shinglesResult = new ShinglesResult(settings.ShingleSize, filterShinglesResultSets);

                    #endregion

                    #region LSA

                    List<DocumentWord> modifiedWordsUsageFilter = UsageFilter.Filter(modifiedWords, MinimalWordsUsageInDocument);
                    DocumentVector documentVector = DocumentVectorization.CreateVector(modifiedWordsUsageFilter);

                    List<DocumentVector> documentsVectors = new List<DocumentVector> {documentVector};
                    foreach (var item in documentList)
                    {
                        DocumentKeyWordList documentKeyWordList = await DocumentKeyWordList.GetAsync(item.DocumentId);
                        DocumentVector itemVector = DocumentVectorization.CreateVector(documentKeyWordList);
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
                            Y = Convert.ToSingle(vt[1, i]),
                            Z = Convert.ToSingle(vt[2, i])
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
            CompareResult compareResult = new CompareResult(shinglesResult, lsaResult);
            return compareResult;
        }

        public async Task<AnalysisResult> AnalyzeDocumentAsync(AnalysisTargetDocument targetDocument)
        {
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(this.TempDirectory, "\\", tempTextFileName);
            String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName, DefaultTempLematizationFilePostfix);
            String tempLematizationFileFullName = String.Concat(this.TempDirectory, "\\", tempLematizationFileName);

            AnalysisResult analysisResult = null;

            try
            {
                // 1. Convert original document to txt format and create copy in temp folder
                using (IDocumentConverter converter = new MsWordDocumentConverter())
                {
                    await converter.ConvertDocumentToTextFileAsync(targetDocument.FilePath, tempTextFileFullName);
                }

                try
                {
                    // 2. Lematization// 3. Lematization
                    await LematizationTool.Current.ProcessDocumntAsync(tempTextFileFullName, tempLematizationFileFullName);

                    // 3. Parse temp documents to words
                    var parserWords = (await Tools.ParserTool.Current.ParseDocumentAsync(tempLematizationFileFullName)).ToList();
                    List<DocumentWord> documentWords = ConvertToBusinessDocumentWords(parserWords);

                    // 4. Remove stop-words from origin text
                    List<DocumentWord> modifiedWords = await StopWordFilter.FilterAsync(documentWords);

                    // 5. Vectorization document words for get usage value for each word
                    DocumentVector documentVector = DocumentVectorization.CreateVector(modifiedWords);

                    analysisResult = new AnalysisResult();
                    analysisResult.DocumentKeyWords = DocumentKeyWordList.Create(null, documentVector); ;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Remove temp files
                    File.Delete(tempTextFileFullName);
                    File.Delete(tempLematizationFileFullName);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return analysisResult;
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

        private List<DocumentWord> ConvertToBusinessDocumentWords(List<Tools.DocumentWord> toolDocumentWords)
        {
            List<DocumentWord> documentWords = toolDocumentWords.Select(x => new DocumentWord
            {
                Word = x.Word,
                StartIndex = x.StartIndex
            }).ToList();

            return documentWords;
        }

        public List<DocumentWord> MergeDocumentWordsPositions(List<DocumentWord> originalDocumentWords,
            List<DocumentWord> processedDocumentWords)
        {
            List<DocumentWord> mergedDocumentWords = processedDocumentWords.Select((x, i) => new DocumentWord
            {
                Word = x.Word,
                StartIndex = originalDocumentWords[i].StartIndex
            }).ToList();

            return mergedDocumentWords;
        }

        #endregion
    }
}