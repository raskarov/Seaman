namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVialIdToCryobank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cryobank", "VialId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cryobank", "VialId");
        }
    }
}
