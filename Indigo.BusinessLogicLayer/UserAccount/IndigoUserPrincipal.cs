using System.Threading.Tasks;

namespace Indigo.BusinessLogicLayer.UserAccount
{
    using System;
    using System.Security.Principal;

    public class IndigoUserPrincipal : IPrincipal
    {
        private IndigoUserIdentity _identity;

        public IndigoUserIdentity Identity
        {
            get { return this._identity ?? new AnonymousIdentity(); }
            set { this._identity = value; }
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

        public static async Task<UserAccount> LoginAsync(String email, String passeord)
        {
            await Task.Delay(100);

            return null;
        }
    }
}