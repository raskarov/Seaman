namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIntToStrInCryobank : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cryobank", "VialId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cryobank", "VialId", c => c.Int(nullable: false));
        }
    }
}
