namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMimeTypeAndPhotoAndPhotoContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PhotoContent", c => c.Binary());
            AddColumn("dbo.Posts", "MimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "MimeType");
            DropColumn("dbo.Posts", "PhotoContent");
        }
    }
}
