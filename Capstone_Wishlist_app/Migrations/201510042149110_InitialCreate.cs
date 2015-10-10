namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ASIN = c.String(nullable: false),
                        Title = c.String(),
                        Gender = c.String(),
                        AgeGroup = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        FormattedPrice = c.String(),
                        ImageUrlSmall = c.String(),
                        ImageUrlMed = c.String(),
                        Url = c.String(),
                        Features = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Item");
        }
    }
}
