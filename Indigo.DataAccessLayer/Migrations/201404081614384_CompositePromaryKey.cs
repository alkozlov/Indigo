namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompositePromaryKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AccountPermissions");
            AddPrimaryKey("dbo.AccountPermissions", new[] { "AccountType", "PermissionType", "AccessType" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AccountPermissions");
            AddPrimaryKey("dbo.AccountPermissions", "AccessType");
        }
    }
}
