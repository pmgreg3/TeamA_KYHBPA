namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDocumentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        DocumentName = c.String(),
                        DocumentDescription = c.String(),
                        DocumentContent = c.Binary(),
                        DocumentUploadDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Documents");
        }
    }
}
