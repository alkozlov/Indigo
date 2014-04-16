namespace Indigo.BusinessLogicLayer.Account
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndigoUserPrincipal : IPrincipal
    {
        public IndigoUserIdentity Identity { get; private set; }

        private static IndigoUserPrincipal _current;

        public static IndigoUserPrincipal Current
        {
            get { return _current ?? (_current = new IndigoUserPrincipal()); }
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

        public static async Task<IndigoUserPrincipal> SigninAsync(String emailOrLogin, String passeord)
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

            userAccount.LastLoginDateUtc = DateTime.UtcNow;
            await userAccount.SaveAsync();

            IndigoUserIdentity identity = new IndigoUserIdentity(userAccount);
            IndigoUserPrincipal principal = new IndigoUserPrincipal(identity);
            _current = principal;

            return principal;
        }

        public async Task SignoutAsync()
        {
            await Task.Delay(1);
            this.Identity = new AnonymousIdentity();
            Thread.CurrentPrincipal = new IndigoUserPrincipal(this.Identity);
        }

        #region Constructors

        private IndigoUserPrincipal()
        {
            this.Identity = new AnonymousIdentity();
        }

        private IndigoUserPrincipal(IndigoUserIdentity identity)
        {
            this.Identity = identity;
        }

        #endregion
    }
}