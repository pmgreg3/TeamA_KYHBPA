namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingThumbnailToPhotoDomainModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "ThumbnailPhotoContent", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "ThumbnailPhotoContent");
        }
    }
}
