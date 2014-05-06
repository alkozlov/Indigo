namespace Indigo.DesktopClient.Model.PenthouseModels
{
    using System;

    public class NewUserModel
    {
        public String Email { get; set; }

        public String Login { get; set; }

        public AccountTypeModel AccountTypeModel { get; set; }

        public NewUserModel()
        {
            this.AccountTypeModel = new AccountTypeModel();
        }
    }
}