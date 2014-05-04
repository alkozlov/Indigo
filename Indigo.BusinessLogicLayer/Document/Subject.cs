namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class Subject
    {
        public Int32 SubjectId { get; set; }

        public String SubjectHeader { get; set; }

        public static async Task<Subject> CreateAsync(String subjectHeader)
        {
            if (String.IsNullOrEmpty(subjectHeader) || String.IsNullOrEmpty(subjectHeader.Trim()))
            {
                throw new ArgumentNullException("subjectHeader", "Subject header can't be null or empty.");
            }

            String convertedSubjectHeader = subjectHeader.Trim().ToLower();

            using (ISubjectsRepository subjectsRepository = new SubjectsRepository())
            {
                var dataExistingSubject = await subjectsRepository.GetAsync(convertedSubjectHeader);
                if (dataExistingSubject != null)
                {
                    throw new DuplicateNameException(String.Format("Subject '{0}' already exists.", convertedSubjectHeader));
                }

                DataModels.Subject dataSubject = new DataModels.Subject
                {
                    SubjectHeader = convertedSubjectHeader
                };

                dataSubject = await subjectsRepository.CreateAsync(dataSubject);
                Subject subject = ConvertToBusinessObject(dataSubject);

                return subject;
            }
        }

        public static async Task<Subject> GetByIdAsync(Int32 subjectId)
        {
            using (ISubjectsRepository subjectsRepository = new SubjectsRepository())
            {
                var dataSubject = await subjectsRepository.GetAsync(subjectId);

                Subject subject = null;
                if (dataSubject != null)
                {
                    subject = ConvertToBusinessObject(dataSubject);
                }

                return subject;
            }
        }

        public static async Task<Subject> GetByHeaderAsync(String subjectHeader)
        {
            using (ISubjectsRepository subjectsRepository = new SubjectsRepository())
            {
                var dataSubject = await subjectsRepository.GetAsync(subjectHeader);

                Subject subject = null;
                if (dataSubject != null)
                {
                    subject = ConvertToBusinessObject(dataSubject);
                }

                return subject;
            }
        }

        public async Task DeleteAsync()
        {
            using (ISubjectsRepository subjectsRepository = new SubjectsRepository())
            {
                await subjectsRepository.DeleteAsync(this.SubjectId);
            }
        }

        #region Constructors

        private Subject()
        {
            
        }

        #endregion

        #region Helpers

        public static Subject ConvertToBusinessObject(DataModels.Subject dataSubject)
        {
            Mapper.CreateMap<DataModels.Subject, Subject>();
            Subject subject = Mapper.Map<DataModels.Subject, Subject>(dataSubject);

            return subject;
        }

        #endregion
    }
}