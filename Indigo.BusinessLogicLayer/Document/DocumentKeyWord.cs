namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class DocumentKeyWord
    {
        public Int64 DocumentKeyWordId { get; private set; }
        public Int32 DocumentId { get; private set; }
        public String Word { get; private set; }
        public Int32 Usages { get; private set; }

        private DocumentKeyWord(Int32 documentKeyWordId, Int32 documentId, String word)
        {
            this.DocumentKeyWordId = documentKeyWordId;
            this.DocumentId = documentId;
            this.Word = word;
        }

        public static async Task<DocumentKeyWord> CreateAsync(Int32 documentId, String word, Int32 usages)
        {
            if (String.IsNullOrEmpty(word) || String.IsNullOrEmpty(word.Trim()))
            {
                throw new ArgumentException("Document word can't be null or empty.", "word");
            }

            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must more then zero.", "documentId");
            }

            Document document = await Document.GetAsync(documentId);
            if (document == null)
            {
                String message = String.Format("Document {0} not found.", documentId);
                throw new NullReferenceException(message);
            }

            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                var dataEntity = await documentKeyWordsRepository.GetByDocumentIdAsync(documentId, word);
                if (dataEntity != null)
                {
                    throw new DuplicateNameException("Document Key Word already exists.");
                }
            }

            DataModels.DocumentKeyWord dataDocumentKeyWord = new DataModels.DocumentKeyWord
            {
                DocumentId = documentId,
                Word = word,
                Usages = usages
            };

            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                dataDocumentKeyWord = await documentKeyWordsRepository.CreateAsync(dataDocumentKeyWord);
                DocumentKeyWord documentKeyWord = dataDocumentKeyWord != null ? ConvertToBusinessObject(dataDocumentKeyWord) : null;

                return documentKeyWord;
            }
        }

        public static async Task<DocumentKeyWord> GetByIdAsync(Int32 documentKeyWordId)
        {
            if (documentKeyWordId <= 0)
            {
                throw new ArgumentException("Document Key Word ID must more then zero.", "documentKeyWordId");
            }

            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                DataModels.DocumentKeyWord dataDocumentKeyWord = await documentKeyWordsRepository.GetAsync(documentKeyWordId);
                DocumentKeyWord documentKeyWord = dataDocumentKeyWord != null ? ConvertToBusinessObject(dataDocumentKeyWord) : null;

                return documentKeyWord;
            }
        }

        public static async Task<DocumentKeyWord> GetByDocumentIdAsync(Int32 documentId, String word)
        {
            if (String.IsNullOrEmpty(word) || String.IsNullOrEmpty(word.Trim()))
            {
                throw new ArgumentException("Document word can't be null or empty.", "word");
            }

            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must more then zero.", "documentId");
            }

            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                DataModels.DocumentKeyWord dataDocumentKeyWord = await documentKeyWordsRepository.GetByDocumentIdAsync(documentId, word);
                DocumentKeyWord documentKeyWord = dataDocumentKeyWord != null ? ConvertToBusinessObject(dataDocumentKeyWord) : null;

                return documentKeyWord;
            }
        }

        public async Task DeleteAsync()
        {
            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                await documentKeyWordsRepository.DeleteByIdAsync(this.DocumentKeyWordId);
            }
        }

        #region Helpers

        private static DocumentKeyWord ConvertToBusinessObject(DataModels.DocumentKeyWord dataDocumentKeyWord)
        {
            Mapper.CreateMap<DataModels.DocumentKeyWord, DocumentKeyWord>();
            DocumentKeyWord documentKeyWord = Mapper.Map<DataModels.DocumentKeyWord, DocumentKeyWord>(dataDocumentKeyWord);

            return documentKeyWord;
        }

        #endregion
    }
}