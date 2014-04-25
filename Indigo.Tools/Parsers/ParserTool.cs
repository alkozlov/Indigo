using System;
using System.Collections;
using System.Collections.Generic;
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

        #endregion

        #region Constructors

        private ParserTool()
        {
            this._parser = new MyStemParser();
        }

        #endregion
    }
}