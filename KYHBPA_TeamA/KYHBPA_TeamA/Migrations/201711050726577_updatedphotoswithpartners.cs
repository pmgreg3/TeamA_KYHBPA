namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedphotoswithpartners : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "IsPartnerOrg", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "IsPartnerOrg");
        }
    }
}
