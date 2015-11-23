namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Cart");
            DropTable("dbo.ShoppingCartItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShoppingCartItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.String(),
                        Title = c.String(),
                        ListPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        ListingUrl = c.String(),
                        MinAgeMonths = c.Int(nullable: false),
                        MaxAgeMonths = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ItemId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
