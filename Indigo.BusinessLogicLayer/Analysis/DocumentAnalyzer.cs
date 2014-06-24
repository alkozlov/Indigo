namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Double.Factorization;

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

        public async Task<CompareResult> CompareDocumentsAsync(AnalysisTargetDocument targetDocument1, AnalysisTargetDocument targetDocument2)
        {
            DocumentAnalysisSettings settings = new DocumentAnalysisSettings(ShingleSize.Size3, 0.01f);

            String tempFile1OnlyName = Guid.NewGuid().ToString("N");
            String tempTextFile1Name = String.Format("{0}.txt", tempFile1OnlyName);
            String tempTextFile1FullName = String.Concat(this.TempDirectory, "\\", tempTextFile1Name);
            String tempLematizationFile1Name = String.Format("{0}{1}.txt", tempFile1OnlyName, DefaultTempLematizationFilePostfix);
            String tempLematizationFile1FullName = String.Concat(this.TempDirectory, "\\", tempLematizationFile1Name);

            String tempFile2OnlyName = Guid.NewGuid().ToString("N");
            String tempTextFile2Name = String.Format("{0}.txt", tempFile2OnlyName);
            String tempTextFile2FullName = String.Concat(this.TempDirectory, "\\", tempTextFile2Name);
            String tempLematizationFile2Name = String.Format("{0}{1}.txt", tempFile2OnlyName, DefaultTempLematizationFilePostfix);
            String tempLematizationFile2FullName = String.Concat(this.TempDirectory, "\\", tempLematizationFile2Name);

            ShinglesResult shinglesResult = null;
            LsaResult lsaResult = null;

            try
            {
                // 1. Convert original document to txt format and create copy in temp folder
                using (IDocumentConverter converter = new MsWordDocumentConverter())
                {
                    await converter.ConvertDocumentToTextFileAsync(targetDocument1.FilePath, tempTextFile1FullName);
                    await converter.ConvertDocumentToTextFileAsync(targetDocument2.FilePath, tempTextFile2FullName);
                }

                // 2. Lematization
                await LematizationTool.Current.ProcessDocumntAsync(tempTextFile1FullName, tempLematizationFile1FullName);
                await LematizationTool.Current.ProcessDocumntAsync(tempTextFile2FullName, tempLematizationFile2FullName);

                try
                {
                    // Parse temp documents to words
                    var parserWords1 = (await Tools.ParserTool.Current.ParseDocumentAsync(tempLematizationFile1FullName)).ToList();
                    List<DocumentWord> document1Words = ConvertToBusinessDocumentWords(parserWords1);
                    var parserWords2 = (await Tools.ParserTool.Current.ParseDocumentAsync(tempLematizationFile2FullName)).ToList();
                    List<DocumentWord> document2Words = ConvertToBusinessDocumentWords(parserWords2);

                    // Parse origignal document to words
                    var parserOriginalWords1 = (await Tools.ParserTool.Current.ParseDocumentAsync(tempTextFile1FullName)).ToList();
                    List<DocumentWord> originalDocument1Words = ConvertToBusinessDocumentWords(parserOriginalWords1);
                    document1Words = MergeDocumentWordsPositions(originalDocument1Words, document1Words);
                    var parserOriginalWords2 = (await Tools.ParserTool.Current.ParseDocumentAsync(tempTextFile2FullName)).ToList();
                    List<DocumentWord> originalDocument2Words = ConvertToBusinessDocumentWords(parserOriginalWords2);
                    document2Words = MergeDocumentWordsPositions(originalDocument2Words, document2Words);

                    // Remove stop-words from origin text
                    List<DocumentWord> modifiedWords1 = await StopWordFilter.FilterAsync(document1Words);
                    List<DocumentWord> modifiedWords2 = await StopWordFilter.FilterAsync(document2Words);

                    ShingleList shingleList1 = await ShingleList.CreateAsync(modifiedWords1, settings.ShingleSize);
                    CheckSumCollection checkSumCollection1 = await CheckSumCollection.CreateAsync(shingleList1, HashAlgorithmType.SHA256);
                    ShingleList shingleList2 = await ShingleList.CreateAsync(modifiedWords2, settings.ShingleSize);
                    CheckSumCollection checkSumCollection2 = await CheckSumCollection.CreateAsync(shingleList2, HashAlgorithmType.SHA256);

                    List<Shingle> similarShingles1 = CheckSumComparer.GetSimilarShingles(checkSumCollection1, checkSumCollection2);
                    float matchingCoefficient1 = CheckSumComparer.CalculateMatchingPercentage(checkSumCollection1, checkSumCollection2);
                    List<Shingle> similarShingles2 = CheckSumComparer.GetSimilarShingles(checkSumCollection2, checkSumCollection1);
                    float matchingCoefficient2 = CheckSumComparer.CalculateMatchingPercentage(checkSumCollection2, checkSumCollection1);

                    ShinglesResultSet shinglesResultSet1 = new ShinglesResultSet(-1, matchingCoefficient1, similarShingles1);
                    ShinglesResultSet shinglesResultSet2 = new ShinglesResultSet(-1, matchingCoefficient2, similarShingles2);
                    shinglesResult = new ShinglesResult(settings.ShingleSize, new List<ShinglesResultSet>
                    {
                        shinglesResultSet1, shinglesResultSet2
                    });
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error{0}: {1}", e.HResult, e.Message);
                    throw;
                }
                finally
                {
                    // Remove temp files
                    File.Delete(tempTextFile1FullName);
                    File.Delete(tempLematizationFile1FullName);
                    File.Delete(tempTextFile2FullName);
                    File.Delete(tempLematizationFile2FullName);
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