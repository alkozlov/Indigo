namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Document;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class ShingleList : ReadOnlyCollection<ShingleList.ShingleItem>
    {
        public class ShingleItem
        {
            public long? ShingleId { get; set; }

            public List<DocumentWord> Words { get; private set; }

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
                        foreach (DocumentWord documentWord in this.Words)
                        {
                            stringBuilder.Append(String.Format(" {0}", documentWord.Word));
                        }

                        this._asString = stringBuilder.ToString().Trim();
                    }

                    return this._asString;
                }
            }

            public Int32 StartIndex
            {
                get { return this.Words.First().StartIndex; }
            }

            public Int32 EndIndex
            {
                get { return this.Words.Last().StartIndex + this.Words.Last().Word.Length; }
            }

            public long? CheckSum { get; set; }

            public ShingleItem()
            {
                this.Words = new List<DocumentWord>();
            }

            public ShingleItem(List<DocumentWord> words)
            {
                this.Words = words;
            }
        }

        public ShingleSize ShingleSize { get; private set; }

        public Int32? DocumentId { get; private set; }

        private ShingleList(IList<ShingleItem> list, ShingleSize shingleSize, Int32? documentId) : base(list)
        {
            this.ShingleSize = shingleSize;
            this.DocumentId = documentId;
        }

        public static async Task<ShingleList> CreateAsync(List<DocumentWord> words, ShingleSize shingleSize, Int32? documentid = null)
        {
            ShingleList shingleList = await Task.Run(() =>
            {
                Int32 shingleSizeValue = (byte) shingleSize;
                
                // Split list to shingles
                Int32 shinglesCount = words.Count - shingleSizeValue + 1;
                List<ShingleItem> shingles = new List<ShingleItem>(shinglesCount);
                for (int i = 0; i < shinglesCount; i++)
                {
                    ShingleItem shingle = new ShingleItem(words.Skip(i).Take(shingleSizeValue).ToList());
                    shingles.Add(shingle);
                }

                return new ShingleList(shingles, shingleSize, documentid);
            });

            return shingleList;
        }

        public static async Task<ShingleList> GetAsync(Int32 documentId, ShingleSize shingleSize)
        {
            List<ShingleItem> shingles = new List<ShingleItem>();

            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                var dataShingles = (await shinglesRepository.GetShinglesAsync(documentId, (byte) shingleSize)).ToList();
                if (dataShingles.Any())
                {
                    shingles.AddRange(dataShingles.Select(dataShingle => new ShingleItem
                    {
                        ShingleId = dataShingle.ShingleId,
                        CheckSum = dataShingle.CheckSum
                    }));
                }
            }

            ShingleList shingleList = new ShingleList(shingles, shingleSize, documentId);
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