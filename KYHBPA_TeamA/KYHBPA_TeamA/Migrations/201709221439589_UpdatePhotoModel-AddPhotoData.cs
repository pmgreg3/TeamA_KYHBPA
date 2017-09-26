namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePhotoModelAddPhotoData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "PhotoData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "PhotoData");
        }
    }
}
