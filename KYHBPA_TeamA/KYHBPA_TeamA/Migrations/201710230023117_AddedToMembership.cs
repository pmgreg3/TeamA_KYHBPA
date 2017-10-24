namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedToMembership : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AppliedForMembership", c => c.Boolean(nullable: false));
            DropColumn("dbo.Memberships", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Memberships", "Email", c => c.String());
            DropColumn("dbo.AspNetUsers", "AppliedForMembership");
        }
    }
}
