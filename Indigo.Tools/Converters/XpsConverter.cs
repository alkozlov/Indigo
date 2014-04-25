using System.Collections;
using System.Linq;

namespace Indigo.Tools.Converters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Xps.Packaging;
    using MsWord = Microsoft.Office.Interop.Word;

    public class XpsConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentName">Full path to document.</param>
        /// <param name="outputDirectory">Output directory for .xps file.</param>
        /// <returns>Full XPS file name.</returns>
        public static async Task<String> ConvertToXps(String documentName, String outputDirectory)
        {
            String xpsFileName = await Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(documentName);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("Can't find document for convertation.", documentName);
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(outputDirectory);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                String newXpsDocumentName = String.Concat(outputDirectory, "\\", Guid.NewGuid().ToString("N"), ".xps");

                // Create a WordApplication and add Document to it
                MsWord.Application wordApplication = new MsWord.Application();
                wordApplication.Documents.Add(documentName);

                MsWord.Document doc = wordApplication.ActiveDocument;
                // You must ensure you have Microsoft.Office.Interop.Word.Dll version 12.
                // Version 11 or previous versions do not have WdSaveFormat.wdFormatXPS option
                doc.SaveAs(newXpsDocumentName, MsWord.WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();

                return newXpsDocumentName;
            });

            return xpsFileName;
        }
    }
}