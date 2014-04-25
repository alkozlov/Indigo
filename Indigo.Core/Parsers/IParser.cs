using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Indigo.Core.Parsers
{
    public interface IParser
    {
        Task<IEnumerable<String>> ExtractDocumentWordsAsync(String filePath);
    }
}