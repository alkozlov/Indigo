using System;
using System.Drawing;
using DevExpress.XtraRichEdit.API.Native;
using GalaSoft.MvvmLight.Messaging;
using Indigo.BusinessLogicLayer.Analysis;
using Indigo.BusinessLogicLayer.Shingles;
using Indigo.DesktopClient.Model.Notifications;

namespace Indigo.DesktopClient.View
{
    /// <summary>
    /// Description for DocumentAnalysisView.
    /// </summary>
    public partial class DocumentAnalysisView
    {
        /// <summary>
        /// Initializes a new instance of the DocumentAnalysisView class.
        /// </summary>
        public DocumentAnalysisView()
        {
            InitializeComponent();

            Messenger.Default.Register<SimilarDocumentsSearchNotification>(this, "1111", message =>
            {
                String similarTextStyleName = "SimilarText";
                var similarTextStyle = this.RichEditControl1.Document.CharacterStyles[similarTextStyleName];
                if (similarTextStyle == null)
                {
                    similarTextStyle = this.RichEditControl1.Document.CharacterStyles.CreateNew();
                    similarTextStyle.Name = similarTextStyleName;
                    similarTextStyle.Parent = this.RichEditControl1.Document.CharacterStyles["Default Paragraph Font"];
                    similarTextStyle.BackColor = Color.FromArgb(135, Color.Tomato);
                    this.RichEditControl1.Document.CharacterStyles.Add(similarTextStyle);
                }

                Int32 startIndex, length;
                foreach (Shingle similarShingle in message.ShinglesResultSet.SimilarShingles)
                {
                    startIndex = similarShingle.StartIndex;
                    length = similarShingle.EndIndex - similarShingle.StartIndex;

                    CharacterProperties characterProperties = this.RichEditControl1.Document.BeginUpdateCharacters(startIndex, length);
                    characterProperties.Style = similarTextStyle;
                    this.RichEditControl1.Document.EndUpdateCharacters(characterProperties);
                }
            });
        }
    }
}