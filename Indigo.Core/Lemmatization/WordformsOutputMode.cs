namespace Indigo.Core.Lemmatization
{
    /// <summary>
    /// Define word forms that will be presented in lemmatizar result
    /// </summary>
    public enum WordformsOutputMode : byte
    {
        /// <summary>
        /// Print all forms
        /// </summary>
        AllForms = 0,

        /// <summary>
        /// Print only lemmas and grammemes.
        /// </summary>
        OnlyLemmasAndGrammemes = 1
    }
}