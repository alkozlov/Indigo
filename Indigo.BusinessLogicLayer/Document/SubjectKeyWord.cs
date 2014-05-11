namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class SubjectKeyWord
    {
        public Int32 SubjectKeyWordId { get; private set; }
        public Int32 SubjectId { get; private set; }
        public String Word { get; private set; }

        private SubjectKeyWord(Int32 subjectKeyWordId, Int32 subjectId, String word)
        {
            this.SubjectKeyWordId = subjectKeyWordId;
            this.SubjectId = subjectId;
            this.Word = word;
        }

        public static async Task<SubjectKeyWord> CreateAsync(Int32 subjectId, String word)
        {
            if (String.IsNullOrEmpty(word) || String.IsNullOrEmpty(word.Trim()))
            {
                throw new ArgumentException("Subject word can't be null or empty.", "word");
            }

            if (subjectId <= 0)
            {
                throw new ArgumentException("Subject ID must more then zero.", "subjectId");
            }

            Subject subject = await Subject.GetByIdAsync(subjectId);
            if (subject == null)
            {
                String message = String.Format("Subject {0} not found.", subjectId);
                throw new NullReferenceException(message);
            }

            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                var dataEntity = await subjectKeyWordsRepository.GetBySubjectIdAsync(subjectId, word);
                if (dataEntity != null)
                {
                    throw new DuplicateNameException("Subject Key Word already exists.");
                }
            }

            DataModels.SubjectKeyWord dataSubjectKeyWord = new DataModels.SubjectKeyWord
            {
                SubjectId = subjectId,
                Word = word
            };

            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                dataSubjectKeyWord = await subjectKeyWordsRepository.CreateAsync(dataSubjectKeyWord);
                SubjectKeyWord subjectKeyWord = dataSubjectKeyWord != null ? ConvertToBusinessObject(dataSubjectKeyWord) : null;

                return subjectKeyWord;
            }
        }

        public static async Task<SubjectKeyWord> GetByIdAsync(Int32 subjectKeyWordId)
        {
            if (subjectKeyWordId <= 0)
            {
                throw new ArgumentException("Subject Key Word ID must more then zero.", "subjectKeyWordId");
            }

            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                DataModels.SubjectKeyWord dataSubjectKeyWord = await subjectKeyWordsRepository.GetAsync(subjectKeyWordId);
                SubjectKeyWord subjectKeyWord = dataSubjectKeyWord != null ? ConvertToBusinessObject(dataSubjectKeyWord) : null;

                return subjectKeyWord;
            }
        }

        public static async Task<SubjectKeyWord> GetBySubjectIdAsync(Int32 subjectId, String word)
        {
            if (String.IsNullOrEmpty(word) || String.IsNullOrEmpty(word.Trim()))
            {
                throw new ArgumentException("Subject word can't be null or empty.", "word");
            }

            if (subjectId <= 0)
            {
                throw new ArgumentException("Subject ID must more then zero.", "subjectId");
            }

            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                DataModels.SubjectKeyWord dataSubjectKeyWord = await subjectKeyWordsRepository.GetBySubjectIdAsync(subjectId, word);
                SubjectKeyWord subjectKeyWord = dataSubjectKeyWord != null ? ConvertToBusinessObject(dataSubjectKeyWord) : null;

                return subjectKeyWord;
            }
        }

        public async Task DeleteAsync()
        {
            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                await subjectKeyWordsRepository.DeleteByIdAsync(this.SubjectKeyWordId);
            }
        }

        #region Helpers

        private static SubjectKeyWord ConvertToBusinessObject(DataModels.SubjectKeyWord dataSubjectKeyWord)
        {
            Mapper.CreateMap<DataModels.SubjectKeyWord, SubjectKeyWord>();
            SubjectKeyWord subjectKeyWord = Mapper.Map<DataModels.SubjectKeyWord, SubjectKeyWord>(dataSubjectKeyWord);

            return subjectKeyWord;
        }

        #endregion
    }
}