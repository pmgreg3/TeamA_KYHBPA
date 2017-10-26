namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingDateTimesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Memberships", "DateofBirth", c => c.DateTime());
            AlterColumn("dbo.Memberships", "MembershipEnrollment", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Memberships", "MembershipEnrollment", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Memberships", "DateofBirth", c => c.DateTime(nullable: false));
        }
    }
}
