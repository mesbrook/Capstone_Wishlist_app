namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedmaxlengthtoASIN : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Item", "ASIN", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Item", "ASIN", unique: true, name: "IX_FirstAndSecond");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Item", "IX_FirstAndSecond");
            AlterColumn("dbo.Item", "ASIN", c => c.String(nullable: false));
        }
    }
}
