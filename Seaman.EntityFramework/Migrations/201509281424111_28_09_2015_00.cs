namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28_09_2015_00 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sample", "DepositorSsnType", c => c.Int(nullable: false));
            AddColumn("dbo.Sample", "PartnerSsnType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sample", "PartnerSsnType");
            DropColumn("dbo.Sample", "DepositorSsnType");
        }
    }
}
