namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_item_model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "Approved", c => c.Boolean(nullable: false));
            DropColumn("dbo.Item", "Title");
            DropColumn("dbo.Item", "Gender");
            DropColumn("dbo.Item", "AgeGroup");
            DropColumn("dbo.Item", "Amount");
            DropColumn("dbo.Item", "FormattedPrice");
            DropColumn("dbo.Item", "ImageUrlSmall");
            DropColumn("dbo.Item", "ImageUrlMed");
            DropColumn("dbo.Item", "Url");
            DropColumn("dbo.Item", "Features");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "Features", c => c.String());
            AddColumn("dbo.Item", "Url", c => c.String());
            AddColumn("dbo.Item", "ImageUrlMed", c => c.String());
            AddColumn("dbo.Item", "ImageUrlSmall", c => c.String());
            AddColumn("dbo.Item", "FormattedPrice", c => c.String());
            AddColumn("dbo.Item", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.Item", "AgeGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Item", "Gender", c => c.String());
            AddColumn("dbo.Item", "Title", c => c.String());
            DropColumn("dbo.Item", "Approved");
        }
    }
}
