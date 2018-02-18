namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFeatureArticleAttributeToPosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "FrontPageFeature", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "FrontPageFeature");
        }
    }
}
