namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cane",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CanisterId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Canister", t => t.CanisterId)
                .Index(t => t.CanisterId, name: "IX_Canister_Id");
            
            CreateTable(
                "dbo.Canister",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TankId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tank", t => t.TankId)
                .Index(t => t.TankId, name: "IX_Tank_Id");
            
            CreateTable(
                "dbo.Tank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Available = c.Boolean(nullable: false),
                        UniqName = c.String(maxLength: 50),
                        Name = c.String(),
                        CaneId = c.Int(),
                        SampleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cane", t => t.CaneId)
                .ForeignKey("dbo.Sample", t => t.SampleId)
                .Index(t => t.CaneId, name: "IX_Cane_Id")
                .Index(t => t.SampleId, name: "IX_Sample_Id");
            
            CreateTable(
                "dbo.CollectionMethod",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sample",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepositorFirstName = c.String(maxLength: 50),
                        DepositorLastName = c.String(maxLength: 50),
                        DepositorDob = c.String(maxLength: 50),
                        DepositorSsn = c.String(maxLength: 20),
                        PartnerFirstName = c.String(maxLength: 50),
                        PartnerLastName = c.String(maxLength: 50),
                        PartnerDob = c.String(maxLength: 50),
                        PartnerSsn = c.String(maxLength: 20),
                        Autologous = c.Boolean(nullable: false),
                        Refreeze = c.Boolean(nullable: false),
                        TestingOnFile = c.Boolean(nullable: false),
                        CryobankPurchased = c.Boolean(nullable: false),
                        CryobankName = c.String(maxLength: 100),
                        CryobankVialId = c.String(maxLength: 20),
                        DirectedDonor = c.Boolean(nullable: false),
                        DirectedDonorId = c.String(maxLength: 20),
                        DirectedDonorLastName = c.String(maxLength: 50),
                        DirectedDonorFirstName = c.String(maxLength: 50),
                        AnonymousDonor = c.Boolean(nullable: false),
                        AnonymousDonorId = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedByUserId = c.Int(),
                        PhysicianId = c.Int(),
                        CollectionMethodId = c.Int(),
                        CommentId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CollectionMethod", t => t.CollectionMethodId)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.ModifiedByUserId)
                .ForeignKey("dbo.Physician", t => t.PhysicianId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.ModifiedByUserId)
                .Index(t => t.PhysicianId)
                .Index(t => t.CollectionMethodId)
                .Index(t => t.CommentId)
                .Index(t => t.UserId, name: "IX_User_Id");
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 100),
                        MiddleName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 50),
                        Email = c.String(maxLength: 100),
                        UserName = c.String(nullable: false, maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                        LockedUntil = c.DateTime(),
                        Password = c.String(),
                        PasswordType = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, name: "IX_User_UserName");
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_Role_Name");
            
            CreateTable(
                "dbo.Physician",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId, name: "IX_User_Id")
                .Index(t => t.RoleId, name: "IX_Role_Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sample", "PhysicianId", "dbo.Physician");
            DropForeignKey("dbo.Sample", "ModifiedByUserId", "dbo.User");
            DropForeignKey("dbo.Location", "SampleId", "dbo.Sample");
            DropForeignKey("dbo.Sample", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.Sample", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.Sample", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Sample", "CollectionMethodId", "dbo.CollectionMethod");
            DropForeignKey("dbo.Location", "CaneId", "dbo.Cane");
            DropForeignKey("dbo.Canister", "TankId", "dbo.Tank");
            DropForeignKey("dbo.Cane", "CanisterId", "dbo.Canister");
            DropIndex("dbo.UserRole", "IX_Role_Id");
            DropIndex("dbo.UserRole", "IX_User_Id");
            DropIndex("dbo.Role", "IX_Role_Name");
            DropIndex("dbo.User", "IX_User_UserName");
            DropIndex("dbo.Sample", "IX_User_Id");
            DropIndex("dbo.Sample", new[] { "CommentId" });
            DropIndex("dbo.Sample", new[] { "CollectionMethodId" });
            DropIndex("dbo.Sample", new[] { "PhysicianId" });
            DropIndex("dbo.Sample", new[] { "ModifiedByUserId" });
            DropIndex("dbo.Sample", new[] { "CreatedByUserId" });
            DropIndex("dbo.Location", "IX_Sample_Id");
            DropIndex("dbo.Location", "IX_Cane_Id");
            DropIndex("dbo.Canister", "IX_Tank_Id");
            DropIndex("dbo.Cane", "IX_Canister_Id");
            DropTable("dbo.UserRole");
            DropTable("dbo.Physician");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.Sample");
            DropTable("dbo.CollectionMethod");
            DropTable("dbo.Location");
            DropTable("dbo.Tank");
            DropTable("dbo.Canister");
            DropTable("dbo.Cane");
        }
    }
}
