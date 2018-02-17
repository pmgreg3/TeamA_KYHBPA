namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBlogThumbnail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ThumbnailPhotoContent", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "ThumbnailPhotoContent");
        }
    }
}
