using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Indigo.BusinessLogicLayer.Account;
using Indigo.WinClient.Model;
using Indigo.WinClient.ViewModel;

namespace Indigo.WinClient.ViewModel.Partial
{
    using System;

    using GalaSoft.MvvmLight;

    using Indigo.WinClient.View;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProfileViewModel : CommonPartialViewModel
    {
        public delegate void LoadViewModelHandler(UserAccount user);

        public event LoadViewModelHandler LoadViewModel;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ProfileViewModel class.
        /// </summary>
        public ProfileViewModel()
        {
            this.LoadViewModel += async user =>
            {
                UserAccount currentUser = await UserAccount.GetUserAsync(user.Login);
                if (currentUser != null)
                {
                    this.ProfileModel = new ProfileModel
                    {
                        Login = currentUser.Login,
                        Email = currentUser.Email,
                        CreatedDateUtc = currentUser.CreatedDateUtc
                    };
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить данные профиля!");
                }
            };

            this.LoadViewModel(IndigoUserPrincipal.Current.Identity.User);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="ProfileModel" /> property's name.
        /// </summary>
        public const string ProfileModelPropertyName = "ProfileModel";

        private ProfileModel _profileModel;

        /// <summary>
        /// Sets and gets the ProfileModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ProfileModel ProfileModel
        {
            get
            {
                return this._profileModel;
            }

            set
            {
                if (this._profileModel == value)
                {
                    return;
                }

                this._profileModel = value;
                base.RaisePropertyChanged(ProfileModelPropertyName);
            }
        }

        #endregion
    }
}