namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_Documents_FK_UserAccount : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Documents", "AddedUserId");
            AddForeignKey("dbo.Documents", "AddedUserId", "dbo.UserAccounts", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "AddedUserId", "dbo.UserAccounts");
            DropIndex("dbo.Documents", new[] { "AddedUserId" });
        }
    }
}
