namespace Indigo.BusinessLogicLayer.Account
{
    using System;
    using System.Security.Principal;

    public class IndigoUserIdentity : IIdentity
    {
        public IndigoUserIdentity(UserAccount user)
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
    }
}