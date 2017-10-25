namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingMembership : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Member_MemberID", "dbo.Members");
            DropIndex("dbo.AspNetUsers", new[] { "Member_MemberID" });
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateofBirth = c.DateTime(nullable: false),
                        MembershipEnrollment = c.DateTime(nullable: false),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        LicenseNumber = c.String(),
                        IsOwner = c.Boolean(nullable: false),
                        IsTrainer = c.Boolean(nullable: false),
                        IsOwnerAndTrainer = c.Boolean(nullable: false),
                        AgreedToTerms = c.Boolean(nullable: false),
                        Signature = c.String(),
                        Affiliation = c.String(),
                        ManagingPartner = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "MemberID", c => c.Int(nullable: false));
            //AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            //AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Membership_ID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Membership_ID");
            AddForeignKey("dbo.AspNetUsers", "Membership_ID", "dbo.Memberships", "ID");
            DropColumn("dbo.AspNetUsers", "Member_MemberID");
            DropTable("dbo.Members");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateofBirth = c.DateTime(nullable: false),
                        MembershipEnrollment = c.DateTime(nullable: false),
                        Income = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        LicenseNumber = c.String(),
                        IsOwner = c.Boolean(nullable: false),
                        IsTrainer = c.Boolean(nullable: false),
                        IsOwnerAndTrainer = c.Boolean(nullable: false),
                        AgreedToTerms = c.Boolean(nullable: false),
                        Signature = c.String(),
                        Affiliation = c.String(),
                        ManagingPartner = c.String(),
                    })
                .PrimaryKey(t => t.MemberID);
            
            AddColumn("dbo.AspNetUsers", "Member_MemberID", c => c.Int());
            DropForeignKey("dbo.AspNetUsers", "Membership_ID", "dbo.Memberships");
            DropIndex("dbo.AspNetUsers", new[] { "Membership_ID" });
            DropColumn("dbo.AspNetUsers", "Membership_ID");
            //DropColumn("dbo.AspNetUsers", "LastName");
            //DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "MemberID");
            DropTable("dbo.Memberships");
            CreateIndex("dbo.AspNetUsers", "Member_MemberID");
            AddForeignKey("dbo.AspNetUsers", "Member_MemberID", "dbo.Members", "MemberID");
        }
    }
}
