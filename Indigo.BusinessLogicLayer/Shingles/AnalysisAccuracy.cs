namespace Indigo.BusinessLogicLayer.Shingles
{
    using System.ComponentModel;

    /// <summary>
    /// Set shingles size.
    /// </summary>
    public enum AnalysisAccuracy : byte
    {
        /// <summary>
        /// Equals shingle size 3.
        /// </summary>
        [Description("Equals shingle size 3.")]
        Level3 = 3,

        /// <summary>
        /// Equals shingle size 4.
        /// </summary>
        [Description("Equals shingle size 4.")]
        Level4 = 4,

        /// <summary>
        /// Equals shingle size 5.
        /// </summary>
        [Description("Equals shingle size 5.")]
        Level5 = 5,

        /// <summary>
        /// Equals shingle size 6.
        /// </summary>
        [Description("Equals shingle size 6.")]
        Level6 = 6,

        /// <summary>
        /// Equals shingle size 7.
        /// </summary>
        [Description("Equals shingle size 7.")]
        Level7 = 7,

        /// <summary>
        /// Equals shingle size 8.
        /// </summary>
        [Description("Equals shingle size 8.")]
        Level8 = 8,

        /// <summary>
        /// Equals shingle size 9.
        /// </summary>
        [Description("Equals shingle size 9.")]
        Level9 = 9,

        /// <summary>
        /// Equals shingle size 10.
        /// </summary>
        [Description("Equals shingle size 10.")]
        Level10 = 10
    }
}