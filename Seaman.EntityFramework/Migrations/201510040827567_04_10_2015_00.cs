namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_10_2015_00 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tank", "TankDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tank", "TankDescription");
        }
    }
}
