namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shingles_Table_Add : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shingles", "DocumentId", "dbo.Documents");
            DropIndex("dbo.Shingles", new[] { "DocumentId" });
            DropTable("dbo.Shingles");
        }
    }
}
