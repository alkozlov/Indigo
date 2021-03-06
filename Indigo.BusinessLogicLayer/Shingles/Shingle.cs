﻿namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Indigo.BusinessLogicLayer.Document;

    public class Shingle
    {
        public long? ShingleId { get; set; }

        public List<DocumentWord> Words { get; private set; }

        public byte Size
        {
            get { return (byte)this.Words.Count; }
        }

        private String _asString;
        public String AsString
        {
            get
            {
                if (String.IsNullOrEmpty(this._asString))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (DocumentWord documentWord in this.Words)
                    {
                        stringBuilder.Append(String.Format(" {0}", documentWord.Word));
                    }

                    this._asString = stringBuilder.ToString().Trim();
                }

                return this._asString;
            }
        }

        public long? CheckSum { get; set; }

        public Int32 StartIndex
        {
            get { return this.Words.First().StartIndex; }
        }

        public Int32 EndIndex 
        {
            get
            {
                DocumentWord lastWordInShingle = this.Words.Last();
                return lastWordInShingle.StartIndex + lastWordInShingle.Word.Length;
            }
        }

        public Shingle()
        {
            this.Words = new List<DocumentWord>();
        }

        public Shingle(List<DocumentWord> words)
        {
            this.Words = words;
        }
    }
}
