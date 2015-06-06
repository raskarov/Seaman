namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePositionName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tank", "PositionsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Tank", "PositionCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tank", "PositionCount", c => c.Int(nullable: false));
            DropColumn("dbo.Tank", "PositionsCount");
        }
    }
}
