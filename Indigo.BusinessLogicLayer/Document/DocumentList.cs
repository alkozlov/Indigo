namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.BusinessLogicLayer.Account;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class DocumentList : ReadOnlyCollection<DocumentList.Item>
    {
         public class Item
         {
             public Int32 DocumentId { get; set; }
             public Guid DocumentGuid { get; set; }
             public String OriginalFileName { get; set; }
             public String StoredFileName { get; set; }
             public UserAccount AddedByUser { get; set; }
             public DateTime CreatedDateUtc { get; set; }
         }

        private DocumentList(IList<Item> list) : base(list)
        {
        }

        public static async Task<DocumentList> GetAllDocumentsAsync()
        {
            using (IDocumentsRepository documentsRepository = new DocumentsRepository())
            {
                List<DataModels.Document> dataDocuments = (await documentsRepository.GetAllDocumentsAsync()).ToList();
                List<Item> documentItems = new List<Item>();
                if (dataDocuments.Count > 0)
                {
                    foreach (var dataDocument in dataDocuments)
                    {
                        UserAccount documentOwner = await UserAccount.GetUserAsync(dataDocument.AddedUserId);
                        String originalFileName = String.Concat(dataDocument.OriginalName, dataDocument.FileExtension);
                        String storedFileName = String.Concat(dataDocument.StoredName, dataDocument.FileExtension);
                        Item documentItem = new Item
                        {
                            DocumentId = dataDocument.DocumentId,
                            DocumentGuid = dataDocument.DocumentGuid,
                            OriginalFileName = originalFileName,
                            StoredFileName = storedFileName,
                            AddedByUser = documentOwner,
                            CreatedDateUtc = dataDocument.CreateDateUtc
                        };

                        documentItems.Add(documentItem);
                    }
                }
                
                DocumentList documentList = new DocumentList(documentItems);
                return documentList;
            }
        }

        #region Helpers

        

        #endregion
    }
}