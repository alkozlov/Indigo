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
            get
            {
                String name = String.Empty;

                if (this.IsAuthenticated)
                {
                    name = !String.IsNullOrEmpty(this.User.Login) ? this.User.Login : this.User.Email;
                }
                
                return name;
            }
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