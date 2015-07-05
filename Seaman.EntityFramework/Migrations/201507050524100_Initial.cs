namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectionMethod",
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
                        Extracted = c.Boolean(nullable: false),
                        Tank = c.String(maxLength: 10),
                        Canister = c.Int(nullable: false),
                        Cane = c.String(maxLength: 20),
                        Position = c.Int(nullable: false),
                        UniqName = c.String(maxLength: 50),
                        DateStored = c.DateTime(),
                        DateExtracted = c.DateTime(),
                        CollectionMethodId = c.Int(),
                        Name = c.String(),
                        SampleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CollectionMethod", t => t.CollectionMethodId)
                .ForeignKey("dbo.Sample", t => t.SampleId)
                .Index(t => t.CollectionMethodId)
                .Index(t => t.SampleId, name: "IX_Sample_Id");
            
            CreateTable(
                "dbo.Sample",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepositorFirstName = c.String(maxLength: 50),
                        DepositorLastName = c.String(maxLength: 50),
                        DepositorDob = c.DateTime(nullable: false),
                        DepositorSsn = c.String(maxLength: 20),
                        PartnerFirstName = c.String(maxLength: 50),
                        PartnerLastName = c.String(maxLength: 50),
                        PartnerDob = c.DateTime(),
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
                        DirectedDonorDob = c.DateTime(),
                        AnonymousDonor = c.Boolean(nullable: false),
                        AnonymousDonorId = c.String(maxLength: 20),
                        ConsentFormUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedByUserId = c.Int(),
                        PhysicianId = c.Int(),
                        Comment = c.String(maxLength: 1000),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.ModifiedByUserId)
                .ForeignKey("dbo.Physician", t => t.PhysicianId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.ModifiedByUserId)
                .Index(t => t.PhysicianId)
                .Index(t => t.UserId, name: "IX_User_Id");
            
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
                "dbo.ExtractReason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CanistersCount = c.Int(nullable: false),
                        CanesCount = c.Int(nullable: false),
                        PositionsCount = c.Int(nullable: false),
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
            DropForeignKey("dbo.Location", "CollectionMethodId", "dbo.CollectionMethod");
            DropIndex("dbo.UserRole", "IX_Role_Id");
            DropIndex("dbo.UserRole", "IX_User_Id");
            DropIndex("dbo.Role", "IX_Role_Name");
            DropIndex("dbo.User", "IX_User_UserName");
            DropIndex("dbo.Sample", "IX_User_Id");
            DropIndex("dbo.Sample", new[] { "PhysicianId" });
            DropIndex("dbo.Sample", new[] { "ModifiedByUserId" });
            DropIndex("dbo.Sample", new[] { "CreatedByUserId" });
            DropIndex("dbo.Location", "IX_Sample_Id");
            DropIndex("dbo.Location", new[] { "CollectionMethodId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.Tank");
            DropTable("dbo.ExtractReason");
            DropTable("dbo.Physician");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.Sample");
            DropTable("dbo.Location");
            DropTable("dbo.CollectionMethod");
        }
    }
}
