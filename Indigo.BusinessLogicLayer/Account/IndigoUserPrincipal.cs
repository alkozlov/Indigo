namespace Indigo.BusinessLogicLayer.Account
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndigoUserPrincipal : IPrincipal
    {
        public IndigoUserIdentity Identity { get; private set; }

        public static IndigoUserPrincipal Current
        {
            get
            {
                IPrincipal currentThreadPrincipal = Thread.CurrentPrincipal;

                return currentThreadPrincipal as IndigoUserPrincipal;
            }
        }

        #region Overrides

        Boolean IPrincipal.IsInRole(String role)
        {
            return false;
        }

        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        #endregion

        public static async Task<IndigoUserPrincipal> LoginAsync(String emailOrLogin, String passeord)
        {
            UserAccount userAccount = await UserAccount.GetUserAsync(emailOrLogin);

            if (userAccount == null)
            {
                throw new LoginException(LoginExceptionReason.EmailOrPasswordInvalid,
                    String.Format("Неверные логин или пароль для пользователя {0}", emailOrLogin));
            }

            if (!userAccount.IsAcive)
            {
                throw new LoginException(LoginExceptionReason.AccountDisabled,
                    String.Format("Пользователь {0} заблокирован", emailOrLogin));
            }

            if (!userAccount.ValidatePassword(passeord))
            {
                throw new LoginException(LoginExceptionReason.EmailOrPasswordInvalid,
                    String.Format("Неверные логин или пароль для пользователя {0}", emailOrLogin));
            }

            IndigoUserIdentity identity = new IndigoUserIdentity(userAccount);
            IndigoUserPrincipal principal = new IndigoUserPrincipal(identity);

            return principal;
        }

        #region Helpers

        public void AssignPrincipalToCurrentContext()
        {
            Thread.CurrentPrincipal = this;
        }

        #endregion

        #region Constructors

        public IndigoUserPrincipal()
        {
            this.Identity = new AnonymousIdentity();
        }

        public IndigoUserPrincipal(IndigoUserIdentity identity)
        {
            this.Identity = identity;
        }

        #endregion
    }
}