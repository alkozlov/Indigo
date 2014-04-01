namespace Indigo.Core.Lemmatization
{
    /// <summary>
    /// Enabled or disabled save punctuation in output document.
    /// </summary>
    public enum DocumentPunctuationMode : byte
    {
        /// <summary>
        /// Save document punctuation.
        /// </summary>
        Enabled = 0,

        /// <summary>
        /// Don't save document punctuation. Save only words to output file.
        /// </summary>
        Disabled = 1
    }
}