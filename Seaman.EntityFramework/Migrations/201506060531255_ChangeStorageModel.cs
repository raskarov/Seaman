namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStorageModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tank", "CanistersCount", c => c.Int(nullable: false));
            AddColumn("dbo.Tank", "CanesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Tank", "PositionCount", c => c.Int(nullable: false));
            AddColumn("dbo.Location", "Tank", c => c.String(maxLength: 10));
            AddColumn("dbo.Location", "Canister", c => c.Int(nullable: false));
            AddColumn("dbo.Location", "Cane", c => c.String(maxLength: 20));
            AddColumn("dbo.Location", "Postion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "Postion");
            DropColumn("dbo.Location", "Cane");
            DropColumn("dbo.Location", "Canister");
            DropColumn("dbo.Location", "Tank");
            DropColumn("dbo.Tank", "PositionCount");
            DropColumn("dbo.Tank", "CanesCount");
            DropColumn("dbo.Tank", "CanistersCount");
        }
    }
}
