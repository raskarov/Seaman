namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetPartnerDobNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sample", "PartnerDob", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sample", "PartnerDob", c => c.DateTime(nullable: false));
        }
    }
}
