using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indigo.Core.Parsers;

namespace Indigo.Tools.Parsers
{
    public class ParserTool
    {
        // Singleton pattern
        private static ParserTool _current;

        public static ParserTool Current
        {
            get { return _current ?? (_current = new ParserTool()); }
        }

        #region Fields

        private readonly IParser _parser;

        #endregion

        #region Methods

        public async Task<IEnumerable<String>> ParseFileAsync(String filePath)
        {
            var words = await this._parser.ExtractDocumentWordsAsync(filePath);
            return words;
        }

        public async Task<IEnumerable<DocumentWord>> ParseDocumentAsync(String documentPath)
        {
            var textWords = await this._parser.ExtractDocumentWordsWithPositionsAsync(documentPath);
            var documentWords = textWords.Select(x => new DocumentWord
            {
                Word = x.Word,
                StartIndex = x.StartIndex
            });

            return documentWords;
        }

        #endregion

        #region Constructors

        private ParserTool()
        {
            //this._parser = new MyStemParser();
            this._parser = new IntelligentParser();
        }

        #endregion
    }
}