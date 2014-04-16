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
    public class ReferencesViewModel : CommonPartialViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ReferencesViewModel class.
        /// </summary>
        public ReferencesViewModel()
        {
        }

        #endregion

        #region Overrides

        public override ApplicationView ViewType
        {
            get { return ApplicationView.References; }
        }

        #endregion
    }
}