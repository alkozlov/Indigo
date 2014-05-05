using System.Text;
using Indigo.DataAccessLayer.IRepositories;
using Indigo.DataAccessLayer.Repositories;

namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Document;

    public class ShingleList : ReadOnlyCollection<ShingleList.ShingleItem>
    {
        public class ShingleItem
        {
            public long? ShingleId { get; set; }

            public List<String> Words { get; private set; }

            public byte Size
            {
                get { return (byte) this.Words.Count; }
            }

            private String _asString;
            public String AsString
            {
                get
                {
                    if (String.IsNullOrEmpty(this._asString))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (String word in this.Words)
                        {
                            stringBuilder.Append(String.Format(" {0}", word));
                        }

                        this._asString = stringBuilder.ToString().Trim();
                    }

                    return this._asString;
                }
            }

            public long? CheckSum { get; set; }

            public ShingleItem()
            {
                this.Words = new List<String>();
            }

            public ShingleItem(List<String> words)
            {
                this.Words = words;
            }
        }

        public Int32 ShingleSize { get; private set; }

        public Int32? DocumentId { get; private set; }

        private ShingleList(IList<ShingleItem> list, Int32 shingleSize, Int32? documentId) : base(list)
        {
            this.ShingleSize = shingleSize;
            this.DocumentId = documentId;
        }

        public static async Task<ShingleList> CreateAsync(List<String> words, List<String> stopWords, Int32 shingleSize, Int32? documentid = null)
        {
            ShingleList shingleList = await Task.Run(() =>
            {
                List<String> modifiedWords = words.Select(x => x.ToLower()).ToList();

                // Remove stop-words
                foreach (String stopWord in stopWords)
                {
                    modifiedWords.RemoveAll(word => String.Equals(word, stopWord, StringComparison.CurrentCultureIgnoreCase));
                }

                // Split list to shingles
                Int32 shinglesCount = modifiedWords.Count - shingleSize + 1;
                List<ShingleItem> shingles = new List<ShingleItem>(shinglesCount);
                for (int i = 0; i < shinglesCount; i++)
                {
                    ShingleItem shingle = new ShingleItem(modifiedWords.Skip(i).Take(shingleSize).ToList());
                    shingles.Add(shingle);
                }

                return new ShingleList(shingles, shingleSize, documentid);
            });

            return shingleList;
        }

        public static async Task<ShingleList> GetAsync(Int32 documentId, AnalysisAccuracy analysisAccuracy)
        {
            List<ShingleItem> shingles = new List<ShingleItem>();

            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                var dataShingles = (await shinglesRepository.GetShinglesAsync(documentId, (byte) analysisAccuracy)).ToList();
                if (dataShingles.Any())
                {
                    shingles.AddRange(dataShingles.Select(dataShingle => new ShingleItem
                    {
                        ShingleId = dataShingle.ShingleId,
                        CheckSum = dataShingle.CheckSum
                    }));
                }
            }

            ShingleList shingleList = new ShingleList(shingles, (byte) analysisAccuracy, documentId);
            return shingleList;
        }

        public static async Task<Int32> GetShinglesCountAsync(Int32 documentId, byte shingleSize)
        {
            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                Int32 shinglesCount = await shinglesRepository.GetShinglesCountAsync(documentId, shingleSize);

                return shinglesCount;
            }
        }

        public static async Task DeleteDocumentShingles(Int32 documentId)
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must be greater then 0.", "documentId");
            }

            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                await shinglesRepository.DeleteAsync(documentId);
            }
        }

        public static async Task DeleteDocumentShingles(Int32 documentId, byte shingleSize)
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must be greater then 0.", "documentId");
            }

            if (shingleSize < 3 || shingleSize > 10)
            {
                throw new ArgumentException("Shingle size must be between 3 and 10.", "shingleSize");
            }

            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                await shinglesRepository.DeleteAsync(documentId, shingleSize);
            }
        }

        public async Task DeleteAllAsync()
        {
            List<long> shingleIds = this.Where(x => x.ShingleId.HasValue).Select(x => x.ShingleId.Value).ToList();

            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                await shinglesRepository.DeleteAllAsync(shingleIds);
            }
        }
    }
}