namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class SubjectList : ReadOnlyCollection<SubjectList.SubjectItem>
    {
        public class SubjectItem
        {
            public Int32 SubjectId { get; set; }

            public String SubjectHeader { get; set; }
        }

        private SubjectList(IList<SubjectItem> list) : base(list)
        {
        }

        public static async Task<SubjectList> GetSubjectsAsync()
        {
            using (ISubjectsRepository subjectsRepository = new SubjectsRepository())
            {
                var dataSubjects = (await subjectsRepository.GetAllAsync()).ToList();
                List<SubjectItem> subjects = new List<SubjectItem>();
                if (dataSubjects.Any())
                {
                    subjects = dataSubjects.Select(x => new SubjectItem
                    {
                        SubjectId = x.SubjectId,
                        SubjectHeader = x.SubjectHeader
                    }).ToList();
                }

                SubjectList subjectList = new SubjectList(subjects);
                return subjectList;
            }
        }
    }
}