namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingPhotoGalleryPropToLandingPageCarouselAndPartnerOrgName : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Photos", "InPhotoGallery", "InLandingPageCarousel");
            RenameColumn("dbo.Photos", "IsPartnerOrg", "InPartnerOrgCarousel");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Photos", "InLandingPageCarousel", "InPhotoGallery");
            RenameColumn("dbo.Photos", "InPartnerOrgCarousel", "IsPartnerOrg");
        }
    }
}
