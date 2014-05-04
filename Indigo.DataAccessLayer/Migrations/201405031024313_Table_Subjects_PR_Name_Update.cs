namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_Subjects_PR_Name_Update : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Subjects", "SubjectsId", "SubjectId");
        }

        public override void Down()
        {
            RenameColumn("dbo.Subjects", "SubjectId", "SubjectsId");
        }
    }
}
