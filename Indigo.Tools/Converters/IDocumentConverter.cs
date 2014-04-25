namespace Indigo.Tools.Converters
{
    using System;
    using System.Threading.Tasks;

    public interface IDocumentConverter : IDisposable
    {
        Task ConvertDocumentToTextFileAsync(String originDocumentName, String outputFileName);
    }
}