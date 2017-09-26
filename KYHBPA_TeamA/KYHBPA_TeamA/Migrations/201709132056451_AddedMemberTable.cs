namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMemberTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.ID);
                    
        }
        
        public override void Down()
        {
            DropTable("dbo.Members");
        }
    }
}
