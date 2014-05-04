using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo.BusinessLogicLayer.Document;
using NUnit.Framework;

namespace Indigo.BusinessLogicLayer.Tests
{
    [TestFixture]
    class SubjectTests
    {
        [Test]
        public async void CreateCustomSybjectTest()
        {
            String customHeader = Guid.NewGuid().ToString("N");
            Subject customSubject = await Subject.CreateAsync(customHeader);

            Assert.IsNotNull(customSubject);
            Assert.AreEqual(customHeader, customSubject.SubjectHeader);
        }

        [Test]
        public async void Create10CustomSybjectsTest()
        {
            // Generate subject headers
            const Int32 customHeadersCount = 10;
            List<String> customHeaders = new List<String>();
            for (int i = 0; i < customHeadersCount; i++)
            {
                customHeaders.Add(Guid.NewGuid().ToString("N"));
            }

            // Validation
            CollectionAssert.IsNotEmpty(customHeaders);
            Assert.AreEqual(customHeadersCount, customHeaders.Count);

            // Create subjects
            List<Subject> customSubjects = new List<Subject>();
            foreach (var customHeader in customHeaders)
            {
                Subject customSubject = await Subject.CreateAsync(customHeader);
                customSubjects.Add(customSubject);
            }

            // Validation
            CollectionAssert.IsNotEmpty(customSubjects);
            Assert.AreEqual(customHeadersCount, customSubjects.Count);
            foreach (Subject customSubject in customSubjects)
            {
                Assert.IsTrue(customHeaders.Any(x => x.Equals(customSubject.SubjectHeader)));
            }

            // Get subjects
            SubjectList subjectList = await SubjectList.GetSubjectsAsync();
            
            // Validation
            CollectionAssert.IsNotEmpty(subjectList);
            foreach (Subject customSubject in customSubjects)
            {
                Assert.IsTrue(subjectList.Any(x => x.SubjectHeader.Equals(customSubject.SubjectHeader)),
                    "Subject '{0}' not present in collection.", customSubject.SubjectHeader);
            }
        }

        [Test]
        public async void Create100CustomSybjectsTest()
        {
            const Int32 customHeadersCount = 100;
            List<String> customHeaders = new List<String>();
            for (int i = 0; i < customHeadersCount; i++)
            {
                customHeaders.Add(Guid.NewGuid().ToString("N"));
            }
            
            CollectionAssert.IsNotEmpty(customHeaders);
            Assert.AreEqual(customHeadersCount, customHeaders.Count);

            List<Subject> customSubjects = new List<Subject>();
            foreach (var customHeader in customHeaders)
            {
                Subject customSubject = await Subject.CreateAsync(customHeader);
                customSubjects.Add(customSubject);
            }

            CollectionAssert.IsNotEmpty(customSubjects);
            Assert.AreEqual(customHeadersCount, customSubjects.Count);
            foreach (Subject customSubject in customSubjects)
            {
                Assert.IsTrue(customHeaders.Any(x => x.Equals(customSubject.SubjectHeader)));
            }

            SubjectList subjectList = await SubjectList.GetSubjectsAsync();

            // Validation
            CollectionAssert.IsNotEmpty(subjectList);
            foreach (Subject customSubject in customSubjects)
            {
                Assert.IsTrue(subjectList.Any(x => x.SubjectHeader.Equals(customSubject.SubjectHeader)),
                    "Subject '{0}' not present in collection.", customSubject.SubjectHeader);
            }
        }
    }
}
