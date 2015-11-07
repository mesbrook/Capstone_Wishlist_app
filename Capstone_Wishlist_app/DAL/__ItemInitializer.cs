using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Capstone_Wishlist_app.Models;

namespace Capstone_Wishlist_app.DAL
{
    public class ItemInitializer : System.Data.Entity. DropCreateDatabaseIfModelChanges<WishlistContext>
    {
        protected override void Seed(WishlistContext context){

            var items = new List<Item>
            {
            new Item{ASIN="B00NW2Q6ZG" ,Title="LEGO Minecraft The Cave 21113 Playset" ,Gender="boy" ,AgeGroup=84 ,Amount=1999 ,FormattedPrice="$19.99",ImageUrlSmall="http://ecx.images-amazon.com/images/I/51to-VgwoVL._SL75_.jpg" ,ImageUrlMed="http://ecx.images-amazon.com/images/I/51to-VgwoVL._SL160_.jpg" ,Url="http://www.amazon.com/LEGO-Minecraft-Cave-21113-Playset/dp/B00NW2Q6ZG%3FSubscriptionId%3DAKIAIIQWRMQEQPV4UNPQ%26tag%3Dsenicapsproj-20%26linkCode%3Dxm2%26camp%3D2025%26creative%3D165953%26creativeASIN%3DB00NW2Q6ZG" ,Features="Includes 2 minifigures with assorted accessories: Steve and a zombie, plus a spider" },
            new Item{ASIN="B00GSPFDX0" ,Title="LEGO Superheroes 76011 Batman: Man-Bat Attack" ,Gender="boy" ,AgeGroup=72 ,Amount=1594 ,FormattedPrice="$15.94" ,ImageUrlSmall="http://ecx.images-amazon.com/images/I/519qZ8Cxj4L._SL75_.jpg" ,ImageUrlMed="http://ecx.images-amazon.com/images/I/519qZ8Cxj4L._SL160_.jpg" ,Url="http://www.amazon.com/LEGO-Superheroes-76011-Batman-Man-Bat/dp/B00GSPFDX0%3FSubscriptionId%3DAKIAIIQWRMQEQPV4UNPQ%26tag%3Dsenicapsproj-20%26linkCode%3Dxm2%26camp%3D2025%26creative%3D165953%26creativeASIN%3DB00GSPFDX0" ,Features="Includes Batman, Nightwing and Man-Bat minifigures with assorted weapons" },
            new Item{ASIN="B00NHQGE04" ,Title="LEGO Disney Princess Elsa's Sparkling Ice Castle" ,Gender="girl" ,AgeGroup=72 ,Amount=3199 ,FormattedPrice="$31.99" ,ImageUrlSmall="http://ecx.images-amazon.com/images/I/61XvyoMpS1L._SL75_.jpg" ,ImageUrlMed="http://ecx.images-amazon.com/images/I/61XvyoMpS1L._SL160_.jpg" ,Url="http://www.amazon.com/Disney-Princess-Elsas-Sparkling-Castle/dp/B00NHQGE04%3Fpsc%3D1%26SubscriptionId%3DAKIAIIQWRMQEQPV4UNPQ%26tag%3Dsenicapsproj-20%26linkCode%3Dxm2%26camp%3D2025%26creative%3D165953%26creativeASIN%3DB00NHQGE04" ,Features="Features a castle with icicle tree, sleigh, secret staircase, ice cream bar, bed and an ice hill" },
            new Item{ASIN="B00UN1Q87U" ,Title="Shopkins S3 Food Fair Themed Packs Cool And Creamy Collection" ,Gender="girl" ,AgeGroup=60 ,Amount=1297 ,FormattedPrice="$12.97" ,ImageUrlSmall="http://ecx.images-amazon.com/images/I/61OnGZoVQnL._SL75_.jpg" ,ImageUrlMed="http://ecx.images-amazon.com/images/I/61OnGZoVQnL._SL160_.jpg" ,Url="http://www.amazon.com/Shopkins-Themed-Packs-Creamy-Collection/dp/B00UN1Q87U%3FSubscriptionId%3DAKIAIIQWRMQEQPV4UNPQ%26tag%3Dsenicapsproj-20%26linkCode%3Dxm2%26camp%3D2025%26creative%3D165953%26creativeASIN%3DB00UN1Q87U" ,Features="Cool & Creamy Theme" },
            //new Item{ASIN= ,Title= ,Gender= ,AgeGroup= ,Amount= ,FormattedPrice= ,ImageUrlSmall= ,ImageUrlMed= ,Url= ,Features= },
            };

            items.ForEach(i => context.Items.Add(i));
            context.SaveChanges();
        }
    }
}