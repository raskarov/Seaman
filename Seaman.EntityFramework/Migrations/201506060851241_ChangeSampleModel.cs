namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSampleModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sample", "DirectedDonorDob", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sample", "DirectedDonorDob", c => c.DateTime(nullable: false));
        }
    }
}
