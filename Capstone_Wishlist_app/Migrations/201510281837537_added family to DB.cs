namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedfamilytoDB : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Item", newName: "WishItem");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.WishItem", newName: "Item");
        }
    }
}
