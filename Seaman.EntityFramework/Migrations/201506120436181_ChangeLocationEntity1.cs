namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLocationEntity1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "Extracted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "Extracted");
        }
    }
}
