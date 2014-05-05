namespace Indigo.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SP_DELETE_ShingleTable : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Shingle_Insert",
                p => new
                    {
                        DocumentId = p.Int(),
                        ShingleSize = p.Byte(),
                        CheckSum = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[Shingles]([DocumentId], [ShingleSize], [CheckSum])
                      VALUES (@DocumentId, @ShingleSize, @CheckSum)
                      
                      DECLARE @ShingleId bigint
                      SELECT @ShingleId = [ShingleId]
                      FROM [dbo].[Shingles]
                      WHERE @@ROWCOUNT > 0 AND [ShingleId] = scope_identity()
                      
                      SELECT t0.[ShingleId]
                      FROM [dbo].[Shingles] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ShingleId] = @ShingleId"
            );
            
            CreateStoredProcedure(
                "dbo.Shingle_Update",
                p => new
                    {
                        ShingleId = p.Long(),
                        DocumentId = p.Int(),
                        ShingleSize = p.Byte(),
                        CheckSum = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[Shingles]
                      SET [DocumentId] = @DocumentId, [ShingleSize] = @ShingleSize, [CheckSum] = @CheckSum
                      WHERE ([ShingleId] = @ShingleId)"
            );
            
            CreateStoredProcedure(
                "dbo.spShinglesDeleteByDocumentId",
                p => new
                    {
                        ShingleId = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Shingles]
                      WHERE ([ShingleId] = @ShingleId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.spShinglesDeleteByDocumentId");
            DropStoredProcedure("dbo.Shingle_Update");
            DropStoredProcedure("dbo.Shingle_Insert");
        }
    }
}
