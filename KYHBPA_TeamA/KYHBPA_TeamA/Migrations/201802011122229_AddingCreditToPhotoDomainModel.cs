namespace KYHBPA_TeamA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCreditToPhotoDomainModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Credit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Credit");
        }
    }
}
