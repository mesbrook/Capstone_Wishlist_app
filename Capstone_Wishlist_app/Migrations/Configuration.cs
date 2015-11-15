using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Models;
using System.Security.Claims;


namespace Capstone_Wishlist_app.Migrations {
    internal sealed class Configuration : DbMigrationsConfiguration<WishlistContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WishlistContext context) {
            var address = new Address {
                Id = 1,
                LineOne = "1100 South Marietta Pkwy",
                LineTwo = "",
                City = "Marietta",
                State = "Georgia",
                PostalCode = "30064"
            };

            context.Addresses.AddOrUpdate(address);
            var family = new Family {
                Id = 1,
                ParentFirstName = "Robert",
                ParentLastName = "Cratchet",
                ShippingAddressId = 1
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
            base.Seed(context);
        }

        private static void SeedRoles(WishlistContext context) {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            roleManager.Create(new IdentityRole {
                Name = "Admin"
            });
            roleManager.Create(new IdentityRole {
                Name = "Family"
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

            userManager.Create(new WishlistUser {
                UserName = "cratchet",
                Email = "rcratchet@example.com",
                EmailConfirmed = true,
                Name = "Robert"
            }, "SwazyDoze14");

            var ericUser = userManager.FindByName("eoneill");
            userManager.AddToRoles(ericUser.Id, "Admin");

            var bobUser = userManager.FindByName("cratchet");
            userManager.AddToRole(bobUser.Id, "Family");
            userManager.AddClaim(bobUser.Id, new Claim("Family", (1).ToString()));
        }
    }
}
