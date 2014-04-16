using GalaSoft.MvvmLight;
using Indigo.DesktopClient.View;

namespace Indigo.DesktopClient.ViewModel.Partial
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocumentsViewModel : CommonPartialViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DocumentsViewModel class.
        /// </summary>
        public DocumentsViewModel()
        {
        }

        #endregion

        #region Overrides

        public override ApplicationView ViewType
        {
            get { return ApplicationView.DocumentsDatabase; }
        }

        #endregion
    }
}