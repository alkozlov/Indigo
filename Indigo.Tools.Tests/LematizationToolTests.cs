namespace Indigo.Tools.Tests
{
    using System;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class LematizationToolTests
    {
        [Test]
        public async Task SimpleLemmatizationTest()
        {
            await LematizationTool.Current.ProcessDocumntAsync("D:\\test.txt", "D:\\test_1.txt");
        }
    }
}