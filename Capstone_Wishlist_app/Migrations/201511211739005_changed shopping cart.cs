namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedshoppingcart : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ShoppingCartItems", newName: "ShoppingCartItem");
            AlterColumn("dbo.Cart", "ItemId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cart", "ItemId", c => c.Int(nullable: false));
            RenameTable(name: "dbo.ShoppingCartItem", newName: "ShoppingCartItems");
        }
    }
}
