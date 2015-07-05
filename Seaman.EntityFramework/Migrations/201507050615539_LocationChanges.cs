namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "CaneLetter", c => c.String());
            AddColumn("dbo.Location", "CaneColor", c => c.String(maxLength: 20));
            AddColumn("dbo.Location", "SpecimenNumber", c => c.String(maxLength: 20));
            DropColumn("dbo.Location", "Cane");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "Cane", c => c.String(maxLength: 20));
            DropColumn("dbo.Location", "SpecimenNumber");
            DropColumn("dbo.Location", "CaneColor");
            DropColumn("dbo.Location", "CaneLetter");
        }
    }
}
