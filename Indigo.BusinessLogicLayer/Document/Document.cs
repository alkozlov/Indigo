using System.Linq;
using Indigo.BusinessLogicLayer.Shingles;

namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.BusinessLogicLayer.Account;
    using Indigo.BusinessLogicLayer.Storage;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class Document
    {
        public Int32 DocumentId { get; set; }
        public Guid DocumentGuid { get; set; }
        public String OriginalFileName { get; set; }
        public String StoredFileName { get; set; }
        public UserAccount AddedByUser { get; private set; }
        public DateTime CreatedDateUtc { get; set; }

        #region Constructors

        private Document()
        {
            
        }

        #endregion

        /// <summary>
        /// Create new document in database and return it include storage name
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <param name="addedByUser"></param>
        /// <returns></returns>
        public static async Task<Document> CreateAsync(String originalFileName, UserAccount addedByUser)
        {
            if (addedByUser == null)
            {
                throw new UnauthorizedAccessException("For this action requires authorization.");
            }

            DataModels.Document dataDocument = new DataModels.Document
                {
                    DocumentGuid = Guid.NewGuid(),
                    OriginalName = Path.GetFileNameWithoutExtension(originalFileName),
                    StoredName = Guid.NewGuid().ToString("N"),
                    FileExtension = Path.GetExtension(originalFileName),
                    AddedUserId = addedByUser.UserId,
                    CreateDateUtc = DateTime.UtcNow
                };

            using (IDocumentsRepository documentsRepository = new DocumentsRepository())
            {
                // 1. Add object to database
                dataDocument = await documentsRepository.CreateAsync(dataDocument);
                Document document = ConvertToBusinessObject(dataDocument);

                // 2. Add file to local and server storages
                // 2.1. Local storage
                using (StorageConnection localStorageConnection = StorageConnector.GetStorageConnection(StorageType.Local))
                {
                    if (localStorageConnection.IsAvailable)
                    {
                        localStorageConnection.UploadFile(originalFileName, document.StoredFileName);
                    }
                }

                // TODO: implement leter
                // 2.2. Remote storage
                //using (StorageConnection serverStorageConnection = StorageConnector.GetStorageConnection(StorageType.Server))
                //{
                //    if (serverStorageConnection.IsAvailable)
                //    {
                //        serverStorageConnection.UploadFile(originalFileName, document.StoredFileName);
                //    }
                //}

                return document;
            }
        }

        public static async Task<Document> GetAsync(Guid documentGuid)
        {
            if (documentGuid.Equals(Guid.Empty))
            {
                throw new ArgumentException("Empty document guid.", "documentGuid");
            }

            using (IDocumentsRepository documentsRepository = new DocumentsRepository())
            {
                DataModels.Document dataDocument = await documentsRepository.GetByGuid(documentGuid);
                Document document = dataDocument != null ? ConvertToBusinessObject(dataDocument) : null;

                return document;
            }
        }

        public async Task DeleteAsync()
        {
            // Delete shingles
            var availableShingleSizes = Enum.GetValues(typeof(AnalysisAccuracy)).Cast<AnalysisAccuracy>().ToArray();
            foreach (var availableShingleSize in availableShingleSizes)
            {
                ShingleList shingleList = await ShingleList.GetAsync(this.DocumentId, availableShingleSize);
                await shingleList.DeleteAllAsync();
            }

            // Delete document
            using (IDocumentsRepository documentsRepository = new DocumentsRepository())
            {
                await documentsRepository.DeleteDocumentAsync(this.DocumentId);
            }
        }

        #region Helpers

        private static Document ConvertToBusinessObject(DataModels.Document dataDocument)
        {
            Mapper.CreateMap<DataModels.Document, Document>()
                .ForMember(dest => dest.OriginalFileName, opt => opt.MapFrom(src => String.Concat(src.OriginalName, src.FileExtension)))
                .ForMember(dest => dest.StoredFileName, opt => opt.MapFrom(src => String.Concat(src.StoredName, src.FileExtension)));

            Document document = Mapper.Map<DataModels.Document, Document>(dataDocument);

            return document;
        }

        private static DataModels.Document ConvertToDataModelObject(Document document)
        {
            Mapper.CreateMap<Document, DataModels.Document>()
                .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(document.OriginalFileName)))
                .ForMember(dest => dest.StoredName, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(document.StoredFileName)))
                .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(document.OriginalFileName)))
                .ForMember(dest => dest.AddedUserId, opt => opt.MapFrom(src => document.AddedByUser.UserId));

            DataModels.Document dataDocument = Mapper.Map<Document, DataModels.Document>(document);

            return dataDocument;
        }

        #endregion
    }
}