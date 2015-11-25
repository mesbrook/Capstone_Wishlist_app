namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialsetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineOne = c.String(),
                        LineTwo = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChildBiography",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChildId = c.Int(nullable: false),
                        Text = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Child", t => t.ChildId, cascadeDelete: true)
                .Index(t => t.ChildId);
            
            CreateTable(
                "dbo.Child",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FamilyId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Family", t => t.FamilyId, cascadeDelete: true)
                .Index(t => t.FamilyId);
            
            CreateTable(
                "dbo.Family",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentFirstName = c.String(nullable: false),
                        ParentLastName = c.String(nullable: false),
                        ShippingAddressId = c.Int(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.ShippingAddressId)
                .Index(t => t.ShippingAddressId);
            
            CreateTable(
                "dbo.Wishlist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Child", t => t.ChildId, cascadeDelete: true)
                .Index(t => t.ChildId);
            
            CreateTable(
                "dbo.WishlistItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WishlistId = c.Int(nullable: false),
                        ItemId = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wishlist", t => t.WishlistId, cascadeDelete: true)
                .Index(t => t.WishlistId);
            
            CreateTable(
                "dbo.CartItem",
                c => new
                    {
                        CartId = c.Int(nullable: false),
                        WishlistItemId = c.Int(nullable: false),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.CartId, t.WishlistItemId })
                .ForeignKey("dbo.Cart", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.WishlistItem", t => t.WishlistItemId, cascadeDelete: true)
                .Index(t => t.CartId)
                .Index(t => t.WishlistItemId);
            
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        DonorId = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DonorId)
                .ForeignKey("dbo.Donor", t => t.DonorId)
                .Index(t => t.DonorId);
            
            CreateTable(
                "dbo.Donor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Donation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DonorId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Donor", t => t.DonorId, cascadeDelete: true)
                .Index(t => t.DonorId);
            
            CreateTable(
                "dbo.DonatedItem",
                c => new
                    {
                        DonationId = c.Int(nullable: false),
                        WishlistItemId = c.Int(nullable: false),
                        Title = c.String(),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.DonationId, t.WishlistItemId })
                .ForeignKey("dbo.Donation", t => t.DonationId, cascadeDelete: true)
                .ForeignKey("dbo.WishlistItem", t => t.WishlistItemId, cascadeDelete: true)
                .Index(t => t.DonationId)
                .Index(t => t.WishlistItemId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CartItem", "WishlistItemId", "dbo.WishlistItem");
            DropForeignKey("dbo.CartItem", "CartId", "dbo.Cart");
            DropForeignKey("dbo.Cart", "DonorId", "dbo.Donor");
            DropForeignKey("dbo.DonatedItem", "WishlistItemId", "dbo.WishlistItem");
            DropForeignKey("dbo.DonatedItem", "DonationId", "dbo.Donation");
            DropForeignKey("dbo.Donation", "DonorId", "dbo.Donor");
            DropForeignKey("dbo.WishlistItem", "WishlistId", "dbo.Wishlist");
            DropForeignKey("dbo.Wishlist", "ChildId", "dbo.Child");
            DropForeignKey("dbo.Family", "ShippingAddressId", "dbo.Address");
            DropForeignKey("dbo.Child", "FamilyId", "dbo.Family");
            DropForeignKey("dbo.ChildBiography", "ChildId", "dbo.Child");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DonatedItem", new[] { "WishlistItemId" });
            DropIndex("dbo.DonatedItem", new[] { "DonationId" });
            DropIndex("dbo.Donation", new[] { "DonorId" });
            DropIndex("dbo.Cart", new[] { "DonorId" });
            DropIndex("dbo.CartItem", new[] { "WishlistItemId" });
            DropIndex("dbo.CartItem", new[] { "CartId" });
            DropIndex("dbo.WishlistItem", new[] { "WishlistId" });
            DropIndex("dbo.Wishlist", new[] { "ChildId" });
            DropIndex("dbo.Family", new[] { "ShippingAddressId" });
            DropIndex("dbo.Child", new[] { "FamilyId" });
            DropIndex("dbo.ChildBiography", new[] { "ChildId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DonatedItem");
            DropTable("dbo.Donation");
            DropTable("dbo.Donor");
            DropTable("dbo.Cart");
            DropTable("dbo.CartItem");
            DropTable("dbo.WishlistItem");
            DropTable("dbo.Wishlist");
            DropTable("dbo.Family");
            DropTable("dbo.Child");
            DropTable("dbo.ChildBiography");
            DropTable("dbo.Address");
        }
    }
}
