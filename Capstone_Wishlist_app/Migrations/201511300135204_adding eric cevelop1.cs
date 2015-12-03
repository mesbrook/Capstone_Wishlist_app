namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingericcevelop1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donation", "OrderId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donation", "OrderId");
        }
    }
}
