namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLocationEntity : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sample", new[] { "CollectionMethodId" });
            AddColumn("dbo.Location", "DateStored", c => c.DateTime(nullable: false));
            AddColumn("dbo.Location", "CollectionMethodId", c => c.Int());
            CreateIndex("dbo.Location", "CollectionMethodId");
            DropColumn("dbo.Sample", "DateStored");
            DropColumn("dbo.Sample", "CollectionMethodId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sample", "CollectionMethodId", c => c.Int());
            AddColumn("dbo.Sample", "DateStored", c => c.DateTime(nullable: false));
            DropIndex("dbo.Location", new[] { "CollectionMethodId" });
            DropColumn("dbo.Location", "CollectionMethodId");
            DropColumn("dbo.Location", "DateStored");
            CreateIndex("dbo.Sample", "CollectionMethodId");
        }
    }
}
