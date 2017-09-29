namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInImageGalleryPropToPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "InPhotoGallery", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "InPhotoGallery");
        }
    }
}
