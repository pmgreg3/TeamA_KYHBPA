namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedphotoswithpartners : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "IsPartnerOrg", c => c.Boolean(nullable: false));
            DropColumn("dbo.Photos", "InPartnerOrgCarousel");
            DropColumn("dbo.Photos", "PartnerHyperLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "PartnerHyperLink", c => c.String());
            AddColumn("dbo.Photos", "InPartnerOrgCarousel", c => c.Boolean(nullable: false));
            DropColumn("dbo.Photos", "IsPartnerOrg");
        }
    }
}
