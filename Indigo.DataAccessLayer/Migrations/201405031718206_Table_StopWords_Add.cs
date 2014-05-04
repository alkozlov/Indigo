namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_StopWords_Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StopWords",
                c => new
                    {
                        StopWordId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.StopWordId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StopWords");
        }
    }
}
