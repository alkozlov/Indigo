using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataModels = Indigo.DataAccessLayer.Models;
using Indigo.DataAccessLayer.IRepositories;
using Indigo.DataAccessLayer.Repositories;

namespace Indigo.BusinessLogicLayer.Document
{
    public class DocumentSubject
    {
        public Int32 DocumentId { get; private set; }
        public Int32 SubjectId { get; private set; }

        private DocumentSubject(Int32 documentId, Int32 subjectId)
        {
            this.DocumentId = documentId;
            this.SubjectId = subjectId;
        }

        public static async Task<DocumentSubject> CreateAsync(Int32 documentId, Int32 subjectId)
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must be more then zero.", "documentId");
            }

            if (subjectId <= 0)
            {
                throw new ArgumentException("Subject ID must be more then zero.", "subjectId");
            }

            Document document = await Document.GetAsync(documentId);
            if (document == null)
            {
                throw new NullReferenceException("Document not found.");
            }

            Subject subject = await Subject.GetByIdAsync(subjectId);
            if (subject == null)
            {
                throw new NullReferenceException("Subject not found.");
            }

            using (IDocumentSubjectsRepository documentSubjectsRepository = new DocumentSubjectsRepository())
            {
                var dataEntity = await documentSubjectsRepository.GetAsync(documentId, subjectId);
                if (dataEntity != null)
                {
                    throw new DuplicateNameException("This subject already exists for specified document.");
                }
                else
                {
                    DataModels.DocumentSubject dataDocumentSubject = new DataModels.DocumentSubject
                    {
                        DocumentId = documentId,
                        SubjectId = subjectId
                    };
                    dataDocumentSubject = await documentSubjectsRepository.CreateAsync(dataDocumentSubject);
                    DocumentSubject documentSubject = ConvertToBusinessObject(dataDocumentSubject);

                    return documentSubject;
                }
            }
        }

        public static async Task<DocumentSubject> GetAsync(Int32 documentId, Int32 subjectId)
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must be more then zero.", "documentId");
            }

            if (subjectId <= 0)
            {
                throw new ArgumentException("Subject ID must be more then zero.", "subjectId");
            }

            Document document = await Document.GetAsync(documentId);
            if (document == null)
            {
                throw new NullReferenceException("Document not found.");
            }

            Subject subject = await Subject.GetByIdAsync(subjectId);
            if (subject == null)
            {
                throw new NullReferenceException("Subject not found.");
            }

            using (IDocumentSubjectsRepository documentSubjectsRepository = new DocumentSubjectsRepository())
            {
                DataModels.DocumentSubject dataDocumentSubject = await documentSubjectsRepository.GetAsync(documentId, subjectId);
                DocumentSubject documentSubject = ConvertToBusinessObject(dataDocumentSubject);

                return documentSubject;
            }
        }

        public static async Task<List<DocumentSubject>> GetAsync(Int32 documentId)
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Document ID must be more then zero.", "documentId");
            }

            Document document = await Document.GetAsync(documentId);
            if (document == null)
            {
                throw new NullReferenceException("Document not found.");
            }

            using (IDocumentSubjectsRepository documentSubjectsRepository = new DocumentSubjectsRepository())
            {
                var dataDocumentSubjects = (await documentSubjectsRepository.GetAsync(documentId)).ToList();
                List<DocumentSubject> documentSubjects = new List<DocumentSubject>();
                if (dataDocumentSubjects.Any())
                {
                    documentSubjects.AddRange(dataDocumentSubjects.Select(ConvertToBusinessObject));
                }

                return documentSubjects;
            }
        }

        #region Helpers

        private static DocumentSubject ConvertToBusinessObject(DataModels.DocumentSubject dataDocumentSubject)
        {
            Mapper.CreateMap<DataModels.DocumentSubject, DocumentSubject>();
            DocumentSubject documentSubject = Mapper.Map<DataModels.DocumentSubject, DocumentSubject>(dataDocumentSubject);

            return documentSubject;
        }

        #endregion
    }
}