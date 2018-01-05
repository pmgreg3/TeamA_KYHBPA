namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Link");
        }
    }
}
