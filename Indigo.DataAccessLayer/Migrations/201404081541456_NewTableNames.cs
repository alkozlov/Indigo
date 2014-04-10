namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AccountPermission", newName: "AccountPermissions");
            RenameTable(name: "dbo.UserAccount", newName: "UserAccounts");
            RenameTable(name: "dbo.PermissionAccessType", newName: "PermissionAccessTypes");
            RenameTable(name: "dbo.Permission", newName: "Permissions");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Permissions", newName: "Permission");
            RenameTable(name: "dbo.PermissionAccessTypes", newName: "PermissionAccessType");
            RenameTable(name: "dbo.UserAccounts", newName: "UserAccount");
            RenameTable(name: "dbo.AccountPermissions", newName: "AccountPermission");
        }
    }
}
