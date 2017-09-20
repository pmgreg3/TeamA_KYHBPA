namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPhotoTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Members");
            DropColumn("dbo.Members", "ID");
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        PhotoTitle = c.String(),
                        PhotoDesc = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoID);
            
            AddColumn("dbo.Members", "MemberID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Members", "MemberID");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "ID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Members");
            DropColumn("dbo.Members", "MemberID");
            DropTable("dbo.Photos");
            AddPrimaryKey("dbo.Members", "ID");
        }
    }
}
