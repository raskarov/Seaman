namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConsentFormUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sample", "ConsentFormUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sample", "ConsentFormUrl");
        }
    }
}
