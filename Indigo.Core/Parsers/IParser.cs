namespace Indigo.Core.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IParser
    {
        Task<IEnumerable<String>> ExtractDocumentWordsAsync(String filePath);

        Task<IEnumerable<TextWord>> ExtractDocumentWordsWithPositionsAsync(String filePath);
    }
}