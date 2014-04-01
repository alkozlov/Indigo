namespace Indigo.Core.Lemmatization
{
    using System;
    using System.Threading.Tasks;

    public interface ILemmatizer
    {
        Task LemmatizationDocumentAsync(String origoinalDocument, String outputDocument);
    }
}