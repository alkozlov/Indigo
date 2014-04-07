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
    public class AuthorizedViewModel : CommonPartialViewModel
    {
        /// <summary>
        /// Initializes a new instance of the AuthorizedViewModel class.
        /// </summary>
        public AuthorizedViewModel()
        {
        }

        public override ApplicationView ViewType
        {
            get { return ApplicationView.AuthorizedCommandPanel; }
        }
    }
}