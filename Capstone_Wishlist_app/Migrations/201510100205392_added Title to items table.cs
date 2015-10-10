namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTitletoitemstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "Title");
        }
    }
}
