namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImplementDocumentSubjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentKeyWords",
                c => new
                    {
                        DocumentKeyWordId = c.Int(nullable: false, identity: true),
                        Word = c.String(nullable: false, maxLength: 100),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentKeyWordId)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.DocumentSubjects",
                c => new
                    {
                        DocumentId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentId, t.SubjectId })
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.DocumentId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.SubjectKeyWords",
                c => new
                    {
                        SubjectKeyWordId = c.Int(nullable: false, identity: true),
                        Word = c.String(nullable: false, maxLength: 100),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectKeyWordId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubjectKeyWords", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.DocumentSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.DocumentSubjects", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.DocumentKeyWords", "DocumentId", "dbo.Documents");
            DropIndex("dbo.SubjectKeyWords", new[] { "SubjectId" });
            DropIndex("dbo.DocumentSubjects", new[] { "SubjectId" });
            DropIndex("dbo.DocumentSubjects", new[] { "DocumentId" });
            DropIndex("dbo.DocumentKeyWords", new[] { "DocumentId" });
            DropTable("dbo.SubjectKeyWords");
            DropTable("dbo.DocumentSubjects");
            DropTable("dbo.DocumentKeyWords");
        }
    }
}
