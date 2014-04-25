using GalaSoft.MvvmLight;
using Indigo.DesktopClient.View;

namespace Indigo.DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TextAnalysisViewModel : CommonViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextAnalysisViewModel class.
        /// </summary>
        public TextAnalysisViewModel()
        {
        }

        #endregion

        #region Overries

        public override ApplicationView ViewType
        {
            get { return ApplicationView.TextAnalisys; }
        }

        #endregion

        #region Commands
        #endregion
    }
}