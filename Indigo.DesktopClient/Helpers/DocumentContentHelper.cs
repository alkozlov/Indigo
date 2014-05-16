namespace Indigo.DesktopClient.Helpers
{
    using System;
    using System.Threading.Tasks;

    using Indigo.Tools.Converters;

    public class DocumentContentHelper
    {
        public static async Task<String> GetDocumentPlainText(String documentFile)
        {
            using (IDocumentConverter documentConverter = new MsWordDocumentConverter())
            {
                String documentPlanText = await documentConverter.ConvertDocumentToText(documentFile);

                return documentPlanText;
            }
        }
    }
}