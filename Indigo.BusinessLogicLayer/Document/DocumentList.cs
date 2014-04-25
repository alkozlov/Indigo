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
             public Boolean IsCorrupted { get; set; }
         }

        private DocumentList(IList<Item> list) : base(list)
        {
        }

        public static async Task<DocumentList> GetAllDocumentsAsync(String storagePath)
        {
            using (IDocumentsRepository documentsRepository = new DocumentsRepository())
            {
                List<DataModels.Document> dataDocuments = (await documentsRepository.GetAllDocumentsAsync()).ToList();
                if (dataDocuments.Count == 0)
                {
                    return new DocumentList(new List<Item>());
                }
                else
                {
                    ConcurrentBag<Item> documents = new ConcurrentBag<Item>();
                    dataDocuments.AsParallel().ForAll(async x =>
                    {
                        UserAccount documentOwner = await UserAccount.GetUserAsync(x.AddedUserId);
                        String originalFileName = String.Concat(x.OriginalName, x.FileExtension);
                        String storedFileName = String.Concat(x.StoredName, x.FileExtension);
                        Item item = new Item
                        {
                            DocumentId = x.DocumentId,
                            DocumentGuid = x.DocumentGuid,
                            OriginalFileName = originalFileName,
                            StoredFileName = storedFileName,
                            AddedByUser = documentOwner,
                            CreatedDateUtc = x.CreateDateUtc,
                            IsCorrupted = CheckFileIntegrity(String.Concat(storagePath, "\\", storedFileName))
                        };

                        documents.Add(item);
                    });

                    return new DocumentList(documents.ToList());
                }
            }
        }

        #region Helpers

        private static Boolean CheckFileIntegrity(String fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            return fileInfo.Exists;
        }

        #endregion
    }
}