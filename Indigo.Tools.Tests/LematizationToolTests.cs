namespace Indigo.Tools.Tests
{
    using System;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class LematizationToolTests
    {
        [Test]
        public void OutputDirectoryTest()
        {
            String currentAssemblyDirectory =AppDomain.CurrentDomain.BaseDirectory;
            String outputDirectory = LematizationTool.Instance.OutputDirectory;
            
            Assert.IsTrue(outputDirectory.StartsWith(currentAssemblyDirectory));
        }

        [Test]
        public async Task SimpleLemmatizationTest()
        {
            await LematizationTool.Instance.ProcessDocumntAsync("D:\\TestLemmatization_1.txt", "D:\\TestLemmatization_2.txt");
        }
    }
}