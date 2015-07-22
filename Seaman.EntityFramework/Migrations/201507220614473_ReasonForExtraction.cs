namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReasonForExtraction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "ReasonId", c => c.Int());
            AddColumn("dbo.Location", "ExtractReasonId", c => c.Int());
            CreateIndex("dbo.Location", "ExtractReasonId", name: "IX_ExtractReason_Id");
            AddForeignKey("dbo.Location", "ExtractReasonId", "dbo.ExtractReason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Location", "ExtractReasonId", "dbo.ExtractReason");
            DropIndex("dbo.Location", "IX_ExtractReason_Id");
            DropColumn("dbo.Location", "ExtractReasonId");
            DropColumn("dbo.Location", "ReasonId");
        }
    }
}
