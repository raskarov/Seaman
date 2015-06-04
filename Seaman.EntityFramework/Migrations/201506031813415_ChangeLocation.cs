namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cane", "CanisterId", "dbo.Canister");
            DropForeignKey("dbo.Canister", "TankId", "dbo.Tank");
            DropIndex("dbo.Cane", "IX_Canister_Id");
            DropIndex("dbo.Canister", "IX_Tank_Id");
            AddColumn("dbo.Location", "CanisterId", c => c.Int());
            AddColumn("dbo.Location", "TankId", c => c.Int());
            CreateIndex("dbo.Location", "CanisterId", name: "IX_Canister_Id");
            CreateIndex("dbo.Location", "TankId", name: "IX_Tank_Id");
            AddForeignKey("dbo.Location", "CanisterId", "dbo.Canister", "Id");
            AddForeignKey("dbo.Location", "TankId", "dbo.Tank", "Id");
            DropColumn("dbo.Cane", "CanisterId");
            DropColumn("dbo.Canister", "TankId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Canister", "TankId", c => c.Int());
            AddColumn("dbo.Cane", "CanisterId", c => c.Int());
            DropForeignKey("dbo.Location", "TankId", "dbo.Tank");
            DropForeignKey("dbo.Location", "CanisterId", "dbo.Canister");
            DropIndex("dbo.Location", "IX_Tank_Id");
            DropIndex("dbo.Location", "IX_Canister_Id");
            DropColumn("dbo.Location", "TankId");
            DropColumn("dbo.Location", "CanisterId");
            CreateIndex("dbo.Canister", "TankId", name: "IX_Tank_Id");
            CreateIndex("dbo.Cane", "CanisterId", name: "IX_Canister_Id");
            AddForeignKey("dbo.Canister", "TankId", "dbo.Tank", "Id");
            AddForeignKey("dbo.Cane", "CanisterId", "dbo.Canister", "Id");
        }
    }
}
