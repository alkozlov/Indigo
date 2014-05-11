namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;

    using Indigo.BusinessLogicLayer.Document;

    public class DocumentVector : Dictionary<String, Int32>
    {
        public Int32? DocumentId { get; private set; }

        public DocumentVector(int? documentId) : base()
        {
            this.DocumentId = documentId;
        }

        public DocumentVector(int? documentId, IDictionary<String, Int32> dictionary) : base(dictionary)
        {
            this.DocumentId = documentId;
        }
    }

    public class DocumentVectorization
    {
        public static DocumentVector Vectorisation(List<String> documentWords)
        {
            DocumentVector vector = new DocumentVector(null);
            foreach (var documentWord in documentWords)
            {
                if (!vector.ContainsKey(documentWord))
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
                vector.Add(documentKeyWord.Word, documentKeyWord.Usages);
            }

            return vector;
        }
    }
}