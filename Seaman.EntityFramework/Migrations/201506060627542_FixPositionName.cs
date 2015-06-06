namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPositionName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "Position", c => c.Int(nullable: false));
            DropColumn("dbo.Location", "Postion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "Postion", c => c.Int(nullable: false));
            DropColumn("dbo.Location", "Position");
        }
    }
}
