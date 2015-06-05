namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DobToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sample", "DepositorDob", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sample", "PartnerDob", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sample", "PartnerDob", c => c.String(maxLength: 50));
            AlterColumn("dbo.Sample", "DepositorDob", c => c.String(maxLength: 50));
        }
    }
}
