namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountPermissions",
                c => new
                    {
                        AccountType = c.Byte(nullable: false),
                        PermissionType = c.Byte(nullable: false),
                        AccessType = c.Byte(nullable: false),
                        LastModifiedDateUtc = c.DateTime(nullable: false),
                        ModifiedByUserId = c.Int(),
                    })
                .PrimaryKey(t => new { t.AccountType, t.PermissionType, t.AccessType })
                .ForeignKey("dbo.UserAccounts", t => t.ModifiedByUserId)
                .Index(t => t.ModifiedByUserId);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserGuid = c.Guid(nullable: false),
                        Login = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        PasswordSalt = c.String(nullable: false, maxLength: 100),
                        CreatedDateUtc = c.DateTime(nullable: false),
                        LastLoginDateUtc = c.DateTime(),
                        RemovedDateUtc = c.DateTime(),
                        AccountType = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        DocumentGuid = c.Guid(nullable: false),
                        FileExtension = c.String(nullable: false, maxLength: 15),
                        OriginalName = c.String(nullable: false, maxLength: 256),
                        StoredName = c.String(nullable: false, maxLength: 256),
                        AddedUserId = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.UserAccounts", t => t.AddedUserId, cascadeDelete: true)
                .Index(t => t.AddedUserId);
            
            CreateTable(
                "dbo.PermissionAccessTypes",
                c => new
                    {
                        AccessType = c.Byte(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.AccessType);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        PermissionType = c.Byte(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PermissionType);
            
            CreateTable(
                "dbo.Shingles",
                c => new
                    {
                        ShingleId = c.Long(nullable: false, identity: true),
                        DocumentId = c.Int(nullable: false),
                        ShingleSize = c.Byte(nullable: false),
                        CheckSum = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ShingleId)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.StopWords",
                c => new
                    {
                        StopWordId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.StopWordId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectHeader = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shingles", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "AddedUserId", "dbo.UserAccounts");
            DropForeignKey("dbo.AccountPermissions", "ModifiedByUserId", "dbo.UserAccounts");
            DropIndex("dbo.Shingles", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "AddedUserId" });
            DropIndex("dbo.AccountPermissions", new[] { "ModifiedByUserId" });
            DropTable("dbo.Subjects");
            DropTable("dbo.StopWords");
            DropTable("dbo.Shingles");
            DropTable("dbo.Permissions");
            DropTable("dbo.PermissionAccessTypes");
            DropTable("dbo.Documents");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.AccountPermissions");
        }
    }
}
