namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class SubjectKeyWordList : ReadOnlyCollection<SubjectKeyWordList.Item>
    {
        public Int32 SubjectId { get; private set; }

        public class Item
        {
            public Int32 SubjectKeyWordId { get; set; }
            public String Word { get; set; }
        }

        private SubjectKeyWordList(int subjectId, IList<Item> list) : base(list)
        {
            this.SubjectId = subjectId;
        }

        public static async Task<SubjectKeyWordList> GetAsync(Int32 subjectId)
        {
            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                var dataSubjectKeyWords = await subjectKeyWordsRepository.GetBySubjectIdAsync(subjectId);
                List<Item> items = dataSubjectKeyWords.Select(x => new Item
                {
                    SubjectKeyWordId = x.SubjectKeyWordId,
                    Word = x.Word
                }).ToList();

                SubjectKeyWordList subjectKeyWordList = new SubjectKeyWordList(subjectId, items);
                return subjectKeyWordList;
            }
        }

        public static async Task<SubjectKeyWordList> GetAllAsync(Int32 subjectId)
        {
            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                var dataSubjectKeyWords = await subjectKeyWordsRepository.GetAllAsync();
                List<Item> items = dataSubjectKeyWords.Select(x => new Item
                {
                    SubjectKeyWordId = x.SubjectKeyWordId,
                    Word = x.Word
                }).ToList();

                SubjectKeyWordList subjectKeyWordList = new SubjectKeyWordList(subjectId, items);
                return subjectKeyWordList;
            }
        }

        public async Task DeleteAsync()
        {
            var subjectKeyWordIds = this.Select(x => x.SubjectKeyWordId);

            using (ISubjectKeyWordsRepository subjectKeyWordsRepository = new SubjectKeyWordsRepository())
            {
                await subjectKeyWordsRepository.DeleteRangeAsync(subjectKeyWordIds);
            }
        }
    }
}