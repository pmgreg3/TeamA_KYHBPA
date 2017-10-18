namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingMemberID : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "MemberID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "MemberID", c => c.Int(nullable: false));
        }
    }
}
