namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShingleList : ReadOnlyCollection<Shingle>
    {
        public Int32 ShingleSize { get; private set; }

        public Int32? DocumentId { get; private set; }

        private ShingleList(IList<Shingle> list, Int32 shingleSize, Int32? documentId) : base(list)
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
                List<Shingle> shingles = new List<Shingle>(shinglesCount);
                for (int i = 0; i < shinglesCount; i++)
                {
                    Shingle shingle = new Shingle(modifiedWords.Skip(i).Take(shingleSize).ToList());
                    shingles.Add(shingle);
                }

                return new ShingleList(shingles, shingleSize, documentid);
            });

            return shingleList;
        }
    }
}