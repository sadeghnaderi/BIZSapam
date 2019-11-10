namespace BizSapam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixProblem1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_AccessLevels",
                c => new
                {
                    Id = c.Byte(nullable: false),
                    AccessType = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Tbl_InvoiceItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InvoiceId = c.Int(nullable: false),
                    ProductId = c.Int(nullable: false),
                    Price = c.String(nullable: false, maxLength: 10),
                    Qty = c.Short(nullable: false),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tbl_Invoices", t => t.InvoiceId, cascadeDelete: true)
                .ForeignKey("dbo.Tbl_Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.ProductId);

            CreateTable(
                "dbo.Tbl_Invoices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserID = c.Int(nullable: false),
                    SellerID = c.Int(nullable: false),
                    DateTime = c.String(nullable: false, maxLength: 25),
                    InvoiceStatesID = c.Byte(nullable: false),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tbl_User", t => t.SellerID, cascadeDelete: true)
                .ForeignKey("dbo.Tbl_InvoiceStates", t => t.InvoiceStatesID, cascadeDelete: true)
                .ForeignKey("dbo.Tbl_User", t => t.UserID, cascadeDelete: false)
                .Index(t => t.UserID)
                .Index(t => t.SellerID)
                .Index(t => t.InvoiceStatesID);

            CreateTable(
                "dbo.Tbl_User",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50),
                    BIZCode = c.String(nullable: false, maxLength: 30),
                    Username = c.String(maxLength: 25),
                    Password = c.String(maxLength: 25),
                    AccessLevelID = c.Byte(nullable: false),
                    Picture = c.String(),
                    Active = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tbl_AccessLevels", t => t.AccessLevelID, cascadeDelete: true)
                .Index(t => t.AccessLevelID);

            CreateTable(
                "dbo.Tbl_InvoiceStates",
                c => new
                {
                    Id = c.Byte(nullable: false),
                    State = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Tbl_Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Barcode = c.String(maxLength: 50),
                    ProductName = c.String(nullable: false, maxLength: 50),
                    Price = c.String(maxLength: 10),
                    Qty = c.Short(nullable: false),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Tbl_LogTypes",
                c => new
                {
                    Id = c.Byte(nullable: false),
                    Type = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Tbl_PurchaseTypes",
                c => new
                {
                    Id = c.Byte(nullable: false),
                    Type = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Tbl_SirjanPurchase",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    PurchaseId = c.Byte(nullable: false),
                    Price = c.String(nullable: false, maxLength: 10),
                    DateTime = c.String(nullable: false, maxLength: 25),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tbl_PurchaseTypes", t => t.PurchaseId, cascadeDelete: true)
                .ForeignKey("dbo.Tbl_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PurchaseId);

            CreateTable(
                "dbo.Tbl_TehranPurchase",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Price = c.String(nullable: false, maxLength: 10),
                    DateTime = c.String(nullable: false, maxLength: 25),
                    Description = c.String(),
                    User_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tbl_User", t => t.User_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.Tbl_UserLogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateTime = c.String(nullable: false, maxLength: 25),
                    LogTypes_Id = c.Byte(),
                    User_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tbl_LogTypes", t => t.LogTypes_Id)
                .ForeignKey("dbo.Tbl_User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.LogTypes_Id)
                .Index(t => t.User_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Tbl_UserLogs", "User_Id", "dbo.Tbl_User");
            DropForeignKey("dbo.Tbl_UserLogs", "LogTypes_Id", "dbo.Tbl_LogTypes");
            DropForeignKey("dbo.Tbl_TehranPurchase", "User_Id", "dbo.Tbl_User");
            DropForeignKey("dbo.Tbl_SirjanPurchase", "UserId", "dbo.Tbl_User");
            DropForeignKey("dbo.Tbl_SirjanPurchase", "PurchaseId", "dbo.Tbl_PurchaseTypes");
            DropForeignKey("dbo.Tbl_InvoiceItems", "ProductId", "dbo.Tbl_Products");
            DropForeignKey("dbo.Tbl_InvoiceItems", "InvoiceId", "dbo.Tbl_Invoices");
            DropForeignKey("dbo.Tbl_Invoices", "UserID", "dbo.Tbl_User");
            DropForeignKey("dbo.Tbl_Invoices", "InvoiceStatesID", "dbo.Tbl_InvoiceStates");
            DropForeignKey("dbo.Tbl_Invoices", "SellerID", "dbo.Tbl_User");
            DropForeignKey("dbo.Tbl_User", "AccessLevelID", "dbo.Tbl_AccessLevels");
            DropIndex("dbo.Tbl_UserLogs", new[] { "User_Id" });
            DropIndex("dbo.Tbl_UserLogs", new[] { "LogTypes_Id" });
            DropIndex("dbo.Tbl_TehranPurchase", new[] { "User_Id" });
            DropIndex("dbo.Tbl_SirjanPurchase", new[] { "PurchaseId" });
            DropIndex("dbo.Tbl_SirjanPurchase", new[] { "UserId" });
            DropIndex("dbo.Tbl_User", new[] { "AccessLevelID" });
            DropIndex("dbo.Tbl_Invoices", new[] { "InvoiceStatesID" });
            DropIndex("dbo.Tbl_Invoices", new[] { "SellerID" });
            DropIndex("dbo.Tbl_Invoices", new[] { "UserID" });
            DropIndex("dbo.Tbl_InvoiceItems", new[] { "ProductId" });
            DropIndex("dbo.Tbl_InvoiceItems", new[] { "InvoiceId" });
            DropTable("dbo.Tbl_UserLogs");
            DropTable("dbo.Tbl_TehranPurchase");
            DropTable("dbo.Tbl_SirjanPurchase");
            DropTable("dbo.Tbl_PurchaseTypes");
            DropTable("dbo.Tbl_LogTypes");
            DropTable("dbo.Tbl_Products");
            DropTable("dbo.Tbl_InvoiceStates");
            DropTable("dbo.Tbl_User");
            DropTable("dbo.Tbl_Invoices");
            DropTable("dbo.Tbl_InvoiceItems");
            DropTable("dbo.Tbl_AccessLevels");
        }
    }
}
