namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedchildtoDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Child",
                c => new
                    {
                        Child_ID = c.Int(nullable: false, identity: true),
                        Family_ID = c.Int(nullable: false),
                        Child_FirstName = c.String(nullable: false),
                        Child_LastName = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Child_ID)
                .ForeignKey("dbo.Family", t => t.Family_ID, cascadeDelete: true)
                .Index(t => t.Family_ID);
            
            CreateTable(
                "dbo.Family",
                c => new
                    {
                        Family_ID = c.Int(nullable: false, identity: true),
                        ParentFirstName = c.String(nullable: false),
                        ParentLastName = c.String(nullable: false),
                        Shipping_address = c.String(nullable: false),
                        Shipping_city = c.String(nullable: false),
                        Shipping_state = c.String(nullable: false),
                        Shipping_zipCode = c.String(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Family_ID);
            
            CreateTable(
                "dbo.Wishlist",
                c => new
                    {
                        whishlist_id = c.Int(nullable: false, identity: true),
                        ASIN = c.String(nullable: false),
                        Child_Id = c.Int(nullable: false),
                        Filled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.whishlist_id)
                .ForeignKey("dbo.Child", t => t.Child_Id, cascadeDelete: true)
                .Index(t => t.Child_Id);
            
            CreateTable(
                "dbo.WishlistWishItem",
                c => new
                    {
                        Wishlist_whishlist_id = c.Int(nullable: false),
                        WishItem_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Wishlist_whishlist_id, t.WishItem_ID })
                .ForeignKey("dbo.Wishlist", t => t.Wishlist_whishlist_id, cascadeDelete: true)
                .ForeignKey("dbo.WishItem", t => t.WishItem_ID, cascadeDelete: true)
                .Index(t => t.Wishlist_whishlist_id)
                .Index(t => t.WishItem_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishlistWishItem", "WishItem_ID", "dbo.WishItem");
            DropForeignKey("dbo.WishlistWishItem", "Wishlist_whishlist_id", "dbo.Wishlist");
            DropForeignKey("dbo.Wishlist", "Child_Id", "dbo.Child");
            DropForeignKey("dbo.Child", "Family_ID", "dbo.Family");
            DropIndex("dbo.WishlistWishItem", new[] { "WishItem_ID" });
            DropIndex("dbo.WishlistWishItem", new[] { "Wishlist_whishlist_id" });
            DropIndex("dbo.Wishlist", new[] { "Child_Id" });
            DropIndex("dbo.Child", new[] { "Family_ID" });
            DropTable("dbo.WishlistWishItem");
            DropTable("dbo.Wishlist");
            DropTable("dbo.Family");
            DropTable("dbo.Child");
        }
    }
}
