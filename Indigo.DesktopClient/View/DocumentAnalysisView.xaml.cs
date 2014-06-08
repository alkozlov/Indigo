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
                String defaultTextStyleName = "DefaultText";
                var similarTextStyle = this.RichEditControl1.Document.CharacterStyles[similarTextStyleName];
                var defaultTextStyle = this.RichEditControl1.Document.CharacterStyles[defaultTextStyleName];
                if (similarTextStyle == null)
                {
                    similarTextStyle = this.RichEditControl1.Document.CharacterStyles.CreateNew();
                    similarTextStyle.Name = similarTextStyleName;
                    similarTextStyle.Parent = this.RichEditControl1.Document.CharacterStyles["Default Paragraph Font"];
                    similarTextStyle.BackColor = Color.FromArgb(135, Color.Tomato);
                    this.RichEditControl1.Document.CharacterStyles.Add(similarTextStyle);
                }

                if (defaultTextStyle == null)
                {
                    defaultTextStyle = this.RichEditControl1.Document.CharacterStyles.CreateNew();
                    defaultTextStyle.Name = similarTextStyleName;
                    defaultTextStyle.Parent = this.RichEditControl1.Document.CharacterStyles["Default Paragraph Font"];
                    defaultTextStyle.BackColor = Color.FromArgb(135, Color.White);
                    this.RichEditControl1.Document.CharacterStyles.Add(defaultTextStyle);
                }

                // Setup default format before selecting similar text parts
                CharacterProperties defaultCharacterProperties =
                    this.RichEditControl1.Document.BeginUpdateCharacters(0, this.RichEditControl1.Document.Length);
                defaultCharacterProperties.Style = defaultTextStyle;
                this.RichEditControl1.Document.EndUpdateCharacters(defaultCharacterProperties);

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