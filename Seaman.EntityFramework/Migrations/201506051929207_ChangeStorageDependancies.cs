namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStorageDependancies : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Location", "CaneId", "dbo.Cane");
            DropForeignKey("dbo.Location", "CanisterId", "dbo.Canister");
            DropForeignKey("dbo.Location", "TankId", "dbo.Tank");
            DropIndex("dbo.Location", "IX_Cane_Id");
            DropIndex("dbo.Location", "IX_Canister_Id");
            DropIndex("dbo.Location", "IX_Tank_Id");
            AddColumn("dbo.Cane", "CanisterId", c => c.Int());
            AddColumn("dbo.Canister", "TankId", c => c.Int());
            AddColumn("dbo.Position", "CaneId", c => c.Int());
            CreateIndex("dbo.Cane", "CanisterId", name: "IX_Canister_Id");
            CreateIndex("dbo.Canister", "TankId", name: "IX_Tank_Id");
            CreateIndex("dbo.Position", "CaneId", name: "IX_Cane_Id");
            AddForeignKey("dbo.Cane", "CanisterId", "dbo.Canister", "Id");
            AddForeignKey("dbo.Canister", "TankId", "dbo.Tank", "Id");
            AddForeignKey("dbo.Position", "CaneId", "dbo.Cane", "Id");
            DropColumn("dbo.Location", "CaneId");
            DropColumn("dbo.Location", "CanisterId");
            DropColumn("dbo.Location", "TankId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "TankId", c => c.Int());
            AddColumn("dbo.Location", "CanisterId", c => c.Int());
            AddColumn("dbo.Location", "CaneId", c => c.Int());
            DropForeignKey("dbo.Position", "CaneId", "dbo.Cane");
            DropForeignKey("dbo.Canister", "TankId", "dbo.Tank");
            DropForeignKey("dbo.Cane", "CanisterId", "dbo.Canister");
            DropIndex("dbo.Position", "IX_Cane_Id");
            DropIndex("dbo.Canister", "IX_Tank_Id");
            DropIndex("dbo.Cane", "IX_Canister_Id");
            DropColumn("dbo.Position", "CaneId");
            DropColumn("dbo.Canister", "TankId");
            DropColumn("dbo.Cane", "CanisterId");
            CreateIndex("dbo.Location", "TankId", name: "IX_Tank_Id");
            CreateIndex("dbo.Location", "CanisterId", name: "IX_Canister_Id");
            CreateIndex("dbo.Location", "CaneId", name: "IX_Cane_Id");
            AddForeignKey("dbo.Location", "TankId", "dbo.Tank", "Id");
            AddForeignKey("dbo.Location", "CanisterId", "dbo.Canister", "Id");
            AddForeignKey("dbo.Location", "CaneId", "dbo.Cane", "Id");
        }
    }
}
