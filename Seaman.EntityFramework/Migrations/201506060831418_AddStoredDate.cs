namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoredDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sample", "DateStored", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sample", "DateStored");
        }
    }
}
