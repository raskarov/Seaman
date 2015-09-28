namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28_09_2015_01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sample", "DepositorAddress", c => c.String(maxLength: 100));
            AddColumn("dbo.Sample", "DepositorCity", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "DepositorState", c => c.String(maxLength: 20));
            AddColumn("dbo.Sample", "DepositorZip", c => c.Long());
            AddColumn("dbo.Sample", "DepositorHomePhone", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "DepositorCellPhone", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "DepositorEmail", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "PartnerAddress", c => c.String(maxLength: 100));
            AddColumn("dbo.Sample", "PartnerCity", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "PartnerState", c => c.String(maxLength: 20));
            AddColumn("dbo.Sample", "PartnerZip", c => c.Long());
            AddColumn("dbo.Sample", "PartnerHomePhone", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "PartnerCellPhone", c => c.String(maxLength: 50));
            AddColumn("dbo.Sample", "PartnerEmail", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sample", "PartnerEmail");
            DropColumn("dbo.Sample", "PartnerCellPhone");
            DropColumn("dbo.Sample", "PartnerHomePhone");
            DropColumn("dbo.Sample", "PartnerZip");
            DropColumn("dbo.Sample", "PartnerState");
            DropColumn("dbo.Sample", "PartnerCity");
            DropColumn("dbo.Sample", "PartnerAddress");
            DropColumn("dbo.Sample", "DepositorEmail");
            DropColumn("dbo.Sample", "DepositorCellPhone");
            DropColumn("dbo.Sample", "DepositorHomePhone");
            DropColumn("dbo.Sample", "DepositorZip");
            DropColumn("dbo.Sample", "DepositorState");
            DropColumn("dbo.Sample", "DepositorCity");
            DropColumn("dbo.Sample", "DepositorAddress");
        }
    }
}
