using System;
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
    public class ProfileViewModel : CommonPartialViewModel
    {
        #region Overrides

        public override ApplicationView ViewType
        {
            get { return ApplicationView.Profile; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ProfileViewModel class.
        /// </summary>
        public ProfileViewModel()
        {
        }

        #endregion
    }
}