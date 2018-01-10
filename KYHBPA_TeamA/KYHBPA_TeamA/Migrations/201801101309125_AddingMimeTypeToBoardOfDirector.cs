namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMimeTypeToBoardOfDirector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardOfDirectors", "MimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoardOfDirectors", "MimeType");
        }
    }
}
