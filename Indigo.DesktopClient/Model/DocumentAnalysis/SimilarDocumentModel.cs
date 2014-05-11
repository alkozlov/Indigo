namespace Indigo.DesktopClient.Model.DocumentAnalysis
{
    using System;

    using Indigo.DesktopClient.Model.PenthouseModels;

    public class SimilarDocumentModel
    {
        public Int32 DocumentId { get; set; }

        public String DocumentName { get; set; }

        public DocumentType DocumentType { get; set; }

        public Int32 SimilarityValue { get; set; }
    }
}