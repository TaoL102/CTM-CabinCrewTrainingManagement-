namespace CTM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CabinCrews",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 100),
                        IsResigned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.EnglishTests",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CabinCrewID = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Grade = c.String(nullable: false),
                        CategoryID = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        UploadRecordID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CabinCrews", t => t.CabinCrewID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.UploadRecords", t => t.UploadRecordID)
                .Index(t => t.CabinCrewID)
                .Index(t => t.CategoryID)
                .Index(t => t.UploadRecordID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UploadRecords",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CategoryID = c.String(maxLength: 128),
                        ApplicationUserID = c.String(maxLength: 128),
                        Description = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        IsWithdrawn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .Index(t => t.CategoryID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RefresherTrainings",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CabinCrewID = c.String(nullable: false, maxLength: 128),
                        CategoryID = c.String(nullable: false, maxLength: 128),
                        Remark = c.String(),
                        Date = c.DateTime(nullable: false),
                        UploadRecordID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CabinCrews", t => t.CabinCrewID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.UploadRecords", t => t.UploadRecordID)
                .Index(t => t.CabinCrewID)
                .Index(t => t.CategoryID)
                .Index(t => t.UploadRecordID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EventType = c.Int(nullable: false),
                        TableName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        UserId = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RefresherTrainings", "UploadRecordID", "dbo.UploadRecords");
            DropForeignKey("dbo.RefresherTrainings", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.RefresherTrainings", "ID", "dbo.CabinCrews");
            DropForeignKey("dbo.EnglishTests", "UploadRecordID", "dbo.UploadRecords");
            DropForeignKey("dbo.UploadRecords", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.UploadRecords", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EnglishTests", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.EnglishTests", "ID", "dbo.CabinCrews");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RefresherTrainings", new[] { "UploadRecordID" });
            DropIndex("dbo.RefresherTrainings", new[] { "CategoryID" });
            DropIndex("dbo.RefresherTrainings", new[] { "ID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UploadRecords", new[] { "ApplicationUserID" });
            DropIndex("dbo.UploadRecords", new[] { "CategoryID" });
            DropIndex("dbo.EnglishTests", new[] { "UploadRecordID" });
            DropIndex("dbo.EnglishTests", new[] { "CategoryID" });
            DropIndex("dbo.EnglishTests", new[] { "ID" });
            DropIndex("dbo.CabinCrews", new[] { "Name" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Logs");
            DropTable("dbo.RefresherTrainings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UploadRecords");
            DropTable("dbo.Categories");
            DropTable("dbo.EnglishTests");
            DropTable("dbo.CabinCrews");
        }
    }
}
