namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Analysis;
    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class DocumentKeyWordList : ReadOnlyCollection<DocumentKeyWordList.Item>
    {
         public Int32? DocumentId { get; private set; }

        public class Item
        {
            public Int64? DocumentKeyWordId { get; set; }
            public String Word { get; set; }
            public Int32 Usages { get; set; }
        }

        private DocumentKeyWordList(Int32? documentId, IList<Item> list) : base(list)
        {
            this.DocumentId = documentId;
        }

        public static DocumentKeyWordList Create(Int32? documentId, DocumentVector documentVector)
        {
            List<Item> items = documentVector.Select(x => new Item
            {
                DocumentKeyWordId = null,
                Word = x.Key,
                Usages = x.Value
            }).ToList();

            DocumentKeyWordList documentKeyWordList = new DocumentKeyWordList(documentId, items);
            return documentKeyWordList;
        }

        public static async Task<DocumentKeyWordList> CreateAndSaveAsync(Int32 documentId, DocumentVector documentWords)
        {
            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                var documentKeyWordsDictionary = documentWords.ToDictionary(x => x.Key, x => x.Value);
                var dataDocumentKeyWords = await documentKeyWordsRepository.CreateRangeAsync(documentId, documentKeyWordsDictionary);
                List<Item> items = dataDocumentKeyWords.Select(dataDocumentKeyWord => new Item
                {
                    DocumentKeyWordId = dataDocumentKeyWord.DocumentKeyWordId,
                    Word = dataDocumentKeyWord.Word
                }).ToList();

                DocumentKeyWordList documentKeyWordList = new DocumentKeyWordList(documentId, items);
                return documentKeyWordList;
            }
        }

        public static async Task<DocumentKeyWordList> GetAsync(Int32 documentId)
        {
            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                var dataDocumentKeyWords = await documentKeyWordsRepository.GetByDocumentIdAsync(documentId);
                List<Item> items = dataDocumentKeyWords.Select(x => new Item
                {
                    DocumentKeyWordId = x.DocumentKeyWordId,
                    Word = x.Word
                }).ToList();

                DocumentKeyWordList documentKeyWordList = new DocumentKeyWordList(documentId, items);
                return documentKeyWordList;
            }
        }

        public static async Task<DocumentKeyWordList> GetAllAsync(Int32 documentId)
        {
            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                var dataDocumentKeyWords = await documentKeyWordsRepository.GetAllAsync();
                List<Item> items = dataDocumentKeyWords.Select(x => new Item
                {
                    DocumentKeyWordId = x.DocumentKeyWordId,
                    Word = x.Word
                }).ToList();

                DocumentKeyWordList documentKeyWordList = new DocumentKeyWordList(documentId, items);
                return documentKeyWordList;
            }
        }

        public async Task DeleteAsync()
        {
            var documentKeyWordIds = this.Select(x => x.DocumentKeyWordId.Value);

            using (IDocumentKeyWordsRepository documentKeyWordsRepository = new DocumentKeyWordsRepository())
            {
                await documentKeyWordsRepository.DeleteRangeAsync(documentKeyWordIds);
            }
        }
    }
}