namespace Indigo.DesktopClient.Model.PenthouseModels
{
    using System;
    using Indigo.BusinessLogicLayer.Account;

    public class UserModel
    {
        public Int32 UserAccountId { get; set; }

        public String Login { get; set; }

        public String Email { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public UserAccountType AccountType { get; set; }
    }
}