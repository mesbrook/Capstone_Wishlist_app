using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Capstone_Wishlist_app.Models;

namespace Capstone_Wishlist_app.DAL {
    public class WishlistInitializer : DropCreateDatabaseIfModelChanges<WishlistContext> {
        protected override void Seed(WishlistContext context) {
            var family = new Family {
                Id = 1,
                ParentFirstName = "Robert",
                ParentLastName = "Cratchet",
                Shipping_address = "1100 South Marietta Pkwy",
                Shipping_city = "Marietta",
                Shipping_state = "Georgia",
                Shipping_zipCode = "30064"
            };

            context.Families.Add(family);

            var child = new Child {
                Id = 1,
                FamilyId = 1,
                FirstName = "Tim",
                LastName = "Cratchet",
                Age = 7,
                Gender = 'M'
            };

            context.Children.Add(child);

            var wishlist = new Wishlist {
                Id = 1,
                ChildId = 1
            };

            context.WishLists.Add(wishlist);
            context.SaveChanges();
        }
    }
}