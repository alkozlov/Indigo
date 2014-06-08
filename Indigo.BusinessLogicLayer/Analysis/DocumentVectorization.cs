namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Indigo.BusinessLogicLayer.Document;

    public class DocumentVector : Dictionary<String, Int32>
    {
        public Int32? DocumentId { get; private set; }

        public DocumentVector(int? documentId) : base()
        {
            this.DocumentId = documentId;
        }

        public DocumentVector(int? documentId, IDictionary<String, Int32> dictionary)
            : base(dictionary)
        {
            this.DocumentId = documentId;
        }

        public Int32 TotalWords
        {
            get
            {
                Int32 totalWordsCount = this.Sum(entity => entity.Value);
                return totalWordsCount;
            }
        }
    }

    public class DocumentVectorization
    {
        public static DocumentVector CreateVector(List<DocumentWord> documentWords)
        {
            DocumentVector vector = new DocumentVector(null);
            foreach (var documentWord in documentWords)
            {
                //if (!vector.Any(x => x.Key.Word.Equals(documentWord.Word) && x.Key.StartIndex == documentWord.StartIndex))
                if (vector.All(x => x.Key != documentWord.Word))
                {
                    vector.Add(documentWord.Word, 1);
                }
                else
                {
                    vector[documentWord.Word]++;
                }
            }

            return vector;
        }

        public static DocumentVector CreateVector(DocumentKeyWordList documentKeyWordList)
        {
            DocumentVector vector = new DocumentVector(documentKeyWordList.DocumentId);
            foreach (var documentKeyWord in documentKeyWordList)
            {
                DocumentWord documentWord = new DocumentWord
                {
                    Word = documentKeyWord.Word,
                    StartIndex = -1
                };
                vector.Add(documentWord.Word, documentKeyWord.Usages);
            }

            return vector;
        }
    }
}