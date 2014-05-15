namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Indigo.BusinessLogicLayer.Document;

    public class DocumentVector : Dictionary<DocumentWord, Int32>
    {
        public Int32? DocumentId { get; private set; }

        public DocumentVector(int? documentId) : base()
        {
            this.DocumentId = documentId;
        }

        public DocumentVector(int? documentId, IDictionary<DocumentWord, Int32> dictionary)
            : base(dictionary)
        {
            this.DocumentId = documentId;
        }
    }

    public class DocumentVectorization
    {
        public static DocumentVector Vectorisation(List<DocumentWord> documentWords)
        {
            DocumentVector vector = new DocumentVector(null);
            foreach (var documentWord in documentWords)
            {
                if (!vector.Any(x => x.Key.Word == documentWord.Word && x.Key.StartIndex == documentWord.StartIndex))
                {
                    vector.Add(documentWord, 1);
                }
                else
                {
                    vector[documentWord]++;
                }
            }

            return vector;
        }

        public static DocumentVector Vectorisation(DocumentKeyWordList documentKeyWordList)
        {
            DocumentVector vector = new DocumentVector(documentKeyWordList.DocumentId);
            foreach (var documentKeyWord in documentKeyWordList)
            {
                DocumentWord documentWord = new DocumentWord
                {
                    Word = documentKeyWord.Word,
                    StartIndex = -1
                };
                vector.Add(documentWord, documentKeyWord.Usages);
            }

            return vector;
        }
    }
}