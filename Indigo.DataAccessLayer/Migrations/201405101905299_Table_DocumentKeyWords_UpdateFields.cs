namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_DocumentKeyWords_UpdateFields : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DocumentKeyWords");
            AddColumn("dbo.DocumentKeyWords", "Usages", c => c.Int(nullable: false));
            AlterColumn("dbo.DocumentKeyWords", "DocumentKeyWordId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.DocumentKeyWords", "DocumentKeyWordId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DocumentKeyWords");
            AlterColumn("dbo.DocumentKeyWords", "DocumentKeyWordId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.DocumentKeyWords", "Usages");
            AddPrimaryKey("dbo.DocumentKeyWords", "DocumentKeyWordId");
        }
    }
}
