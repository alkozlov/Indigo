namespace Indigo.Core.Lemmatization
{
    /// <summary>
    /// Specifies the rule of inference lemmas in a new file with a new line or retaining the original text markup.
    /// </summary>
    public enum WordWrappingMode : byte
    {
        /// <summary>
        /// Enable word wrapping for each lemma. Punctuation marks also will be removed.
        /// </summary>
        Enabled = 0,

        /// <summary>
        /// Save original document markup and punctuation marks
        /// </summary>
        Disabled = 1
    }
}