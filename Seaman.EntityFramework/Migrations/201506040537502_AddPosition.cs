namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPosition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Location", "PositionId", c => c.Int());
            CreateIndex("dbo.Location", "PositionId", name: "IX_Position_Id");
            AddForeignKey("dbo.Location", "PositionId", "dbo.Position", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Location", "PositionId", "dbo.Position");
            DropIndex("dbo.Location", "IX_Position_Id");
            DropColumn("dbo.Location", "PositionId");
            DropTable("dbo.Position");
        }
    }
}
