using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Models;


namespace Capstone_Wishlist_app.Migrations {
    internal sealed class Configuration : DbMigrationsConfiguration<WishlistContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

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

            context.Families.AddOrUpdate(family);

            var child = new Child {
                Id = 1,
                FamilyId = 1,
                FirstName = "Tim",
                LastName = "Cratchet",
                Age = 7,
                Gender = 'M'
            };

            context.Children.AddOrUpdate(child);

            var wishlist = new Wishlist {
                Id = 1,
                ChildId = 1
            };

            context.WishLists.AddOrUpdate(wishlist);
            context.SaveChanges();

            SeedRoles(context);
            SeedUserAccounts(context);
        }

        private static void SeedRoles(WishlistContext context) {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            roleManager.Create(new IdentityRole {
                Name = "Admin"
            });
        }

        private static void SeedUserAccounts(WishlistContext context) {
            var userStore = new UserStore<WishlistUser>(context);
            var userManager = new WishlistUserManager(userStore);

            userManager.Create(new WishlistUser {
                UserName = "eoneill",
                Email = "eoneillspsu@gmail.com",
                EmailConfirmed = true,
                Name = "Eric",
            }, "OweBahama14");

            var ericUser = userManager.FindByName("eoneill");
            userManager.AddToRoles(ericUser.Id, "Admin");
        }
    }
}
