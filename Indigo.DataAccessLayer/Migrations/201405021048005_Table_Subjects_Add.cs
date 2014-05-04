namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_Subjects_Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectsId = c.Int(nullable: false, identity: true),
                        SubjectHeader = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SubjectsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subjects");
        }
    }
}
