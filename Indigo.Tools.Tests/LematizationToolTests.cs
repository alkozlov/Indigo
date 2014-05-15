using System.Collections.Generic;
using System.Linq;
using Indigo.Tools.Converters;
using Indigo.Tools.Parsers;

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
            IDocumentConverter documentConverter = new MsWordDocumentConverter();
            await documentConverter.ConvertDocumentToTextFileAsync("D:\\Temp\\IndigoTestDocuments\\Самовоспитание_2.docx", "D:\\Temp\\Test\\Doc.txt");

            await LematizationTool.Current.ProcessDocumntAsync("D:\\Temp\\Test\\Doc.txt", "D:\\Temp\\Test\\Doc_LEM.txt");

            List<DocumentWord> words1 = (await ParserTool.Current.ParseDocumentAsync("D:\\Temp\\Test\\Doc.txt")).ToList();

            List<DocumentWord> words2 = (await ParserTool.Current.ParseDocumentAsync("D:\\Temp\\Test\\Doc_LEM.txt")).ToList();
        }
    }
}