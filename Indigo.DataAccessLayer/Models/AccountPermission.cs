namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class AccountPermission
    {
        public Byte AccountType { get; set; }

        public Byte PermissionType { get; set; }

        public Byte AccessType { get; set; }

        public DateTime LastModifiedDateUtc { get; set; }

        public Int32? ModifiedByUserId { get; set; }

        public UserAccount UserAccount { get; set; }
    }
}