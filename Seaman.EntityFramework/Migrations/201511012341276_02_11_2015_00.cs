namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_11_2015_00 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "DateFrozen", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "DateFrozen");
        }
    }
}
