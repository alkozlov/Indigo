namespace Indigo.BusinessLogicLayer.UserAccount
{
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public class IndigoUserIdentity : IIdentity
    {
        protected IndigoUserIdentity(UserAccount user)
        {
            this.User = user;
        }

        public UserAccount User { get; private set; }

        public String Name
        {
            get { return this.IsAuthenticated ? this.User.Login : String.Empty; }
        }

        public String AuthenticationType
        {
            get { return "Indigo"; }
        }

        public Boolean IsAuthenticated
        {
            get { return this.User != null; }
        }

        public static async Task<IndigoUserIdentity> GetIdentityAsync(UserAccount user)
        {
            await Task.Delay(100);

            return null;
        }
    }
}