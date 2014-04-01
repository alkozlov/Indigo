namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class UserAccount
    {
        public Int32 UserId { get; set; }

        public Guid UserGuid { get; set; }

        public String Login { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String PasswordSalt { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime? LastLoginDateUtc { get; set; }

        public DateTime? RemovedDateUtc { get; set; }

        public Byte AccountType { get; set; }

        public Boolean IsActive { get; set; }
    }
}