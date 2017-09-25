namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMemberToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Member_MemberID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Member_MemberID");
            AddForeignKey("dbo.AspNetUsers", "Member_MemberID", "dbo.Members", "MemberID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Member_MemberID", "dbo.Members");
            DropIndex("dbo.AspNetUsers", new[] { "Member_MemberID" });
            DropColumn("dbo.AspNetUsers", "Member_MemberID");
        }
    }
}
