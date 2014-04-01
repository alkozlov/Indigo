namespace Indigo.DesktopClient.ViewModel
{
    using System;
    using Indigo.DesktopClient.View;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SignInViewModel : CommonViewModel
    {
        #region Override

        public override ApplicationView ViewType
        {
            get { return ApplicationView.SignIn; }
        }

        #endregion

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private String _title = "Вход в личный кабинет";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String Title
        {
            get
            {
                return this._title;
            }

            set
            {
                if (this._title == value)
                {
                    return;
                }

                this._title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SignInViewModel class.
        /// </summary>
        public SignInViewModel()
        {
            
        }

        #endregion

    }
}