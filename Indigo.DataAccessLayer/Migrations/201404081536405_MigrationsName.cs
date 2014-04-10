namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationsName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserAccounts", newName: "UserAccount");
            CreateTable(
                "dbo.AccountPermission",
                c => new
                    {
                        AccessType = c.Byte(nullable: false),
                        AccountType = c.Byte(nullable: false),
                        PermissionType = c.Byte(nullable: false),
                        LastModifiedDateUtc = c.DateTime(nullable: false),
                        ModifiedByUserId = c.Int(),
                    })
                .PrimaryKey(t => t.AccessType)
                .ForeignKey("dbo.UserAccount", t => t.ModifiedByUserId)
                .Index(t => t.ModifiedByUserId);
            
            CreateTable(
                "dbo.PermissionAccessType",
                c => new
                    {
                        AccessType = c.Byte(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.AccessType);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        PermissionType = c.Byte(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PermissionType);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountPermission", "ModifiedByUserId", "dbo.UserAccount");
            DropIndex("dbo.AccountPermission", new[] { "ModifiedByUserId" });
            DropTable("dbo.Permission");
            DropTable("dbo.PermissionAccessType");
            DropTable("dbo.AccountPermission");
            RenameTable(name: "dbo.UserAccount", newName: "UserAccounts");
        }
    }
}
