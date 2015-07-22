namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeExtractReason : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.Location", name: "IX_ExtractReason_Id", newName: "IX_ExtractReasonId");
            DropColumn("dbo.Location", "ReasonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "ReasonId", c => c.Int());
            RenameIndex(table: "dbo.Location", name: "IX_ExtractReasonId", newName: "IX_ExtractReason_Id");
        }
    }
}
