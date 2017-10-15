namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardOfDirectors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Email = c.String(),
                        Description = c.String(),
                        PhotoContent = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BoardOfDirectors");
        }
    }
}
