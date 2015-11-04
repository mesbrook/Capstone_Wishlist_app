namespace Capstone_Wishlist_app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedfamilyprofileswithchild : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateFamilyProfile",
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
                        Child_FirstName = c.String(nullable: false),
                        Child_LastName = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Family_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CreateFamilyProfile");
        }
    }
}
