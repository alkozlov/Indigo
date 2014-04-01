namespace Indigo.Tools
{
    using System;
    using System.Threading.Tasks;

    using Indigo.Core.Lemmatization;

    public class LematizationTool
    {
        #region Defaults values

        private const String DefaultLematizationToolDocumentsDirectory = "\\LematizationToolDocuments";

        private const String DefaultMyStemLematizationTool = "\\tools\\mystem.exe";

        #endregion

        // Singleton pattern
        private static LematizationTool _instance;

        public static LematizationTool Instance
        {
            get { return _instance ?? (_instance = new LematizationTool()); }
        }

        #region Fields

        private readonly ILemmatizer _lemmatizer;

        #endregion

        #region Prpoerties

        public String OutputDirectory { get; private set; }

        public String LemmatizationTool { get; private set; }

        #endregion

        #region Methods

        public async Task ProcessDocumntAsync(String originalDocument, String outputDocument)
        {
            await this._lemmatizer.LemmatizationDocumentAsync(originalDocument, outputDocument);
        }

        public void ChangeOutputDirectory(String outputDirectory)
        {
            this.OutputDirectory = outputDirectory;
        }

        public void ChangeLemmatizationTool(String lemmatizationTool)
        {
            this.LemmatizationTool = lemmatizationTool;
        }

        #endregion

        #region Constructors

        private LematizationTool()
        {
            String applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.OutputDirectory = String.Concat(applicationDirectory, DefaultLematizationToolDocumentsDirectory);
            this.LemmatizationTool = String.Concat(applicationDirectory, DefaultMyStemLematizationTool);
            this._lemmatizer = new MyStemLemmatizer(this.LemmatizationTool);
        }

        #endregion
    }
}
