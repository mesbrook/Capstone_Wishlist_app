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
            context.SaveChanges();

            var family = new Family {
                Id = 1,
                ParentFirstName = "Robert",
                ParentLastName = "Cratchet",
                ShippingAddressId = 1
            };
            context.Families.AddOrUpdate(family);
            context.SaveChanges();

            var child = new Child {
                Id = 1,
                FamilyId = 1,
                FirstName = "Tim",
                LastName = "Cratchet",
                Age = 7,
                Gender = Gender.Male
            };
            context.Children.AddOrUpdate(child);
            context.SaveChanges();

            var wishlist = new Wishlist {
                Id = 1,
                ChildId = 1
            };
            context.WishLists.AddOrUpdate(wishlist);
            context.SaveChanges();

            SeedDonor(context);
            SeedRoles(context);
            SeedUserAccounts(context);
            base.Seed(context);
        }

        private static void SeedDonor(WishlistContext context) {
            var donor = new Donor { Id = 1 };
            context.Donors.AddOrUpdate(donor);
            context.SaveChanges();

            var cart = new Cart {
                DonorId = 1,
                ModifiedDate = DateTime.Now
            };
            context.Carts.AddOrUpdate(cart);
            context.SaveChanges();
        }

        private static void SeedRoles(WishlistContext context) {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            roleManager.Create(new IdentityRole {
                Name = "Admin"
            });
            roleManager.Create(new IdentityRole
            {
                Name = "Moderator"
            });
            roleManager.Create(new IdentityRole {
                Name = "Family"
            });
            roleManager.Create(new IdentityRole
            {
                Name = "Donor"
            });
        }

        private static void SeedUserAccounts(WishlistContext context) {
            var userStore = new UserStore<WishlistUser>(context);
            var userManager = new WishlistUserManager(userStore);

            userManager.Create(new WishlistUser {
                UserName = "jmarley",
                Email = "jmarley@santawishlist.com",
                EmailConfirmed = true,
                Name = "Jacob Marley",
            }, "OweBahama14");

            var jakeUser = userManager.FindByName("jmarley");
            userManager.AddToRoles(jakeUser.Id, WishlistUser.AdminRole);

            userManager.Create(new WishlistUser {
                UserName = "rcratchet",
                Email = "rcratchet@example.com",
                EmailConfirmed = true,
                Name = "Robert"
            }, "SwazyDoze14");

            var bobUser = userManager.FindByName("rcratchet");
            userManager.AddToRole(bobUser.Id, "Family");
            userManager.AddClaim(bobUser.Id, new Claim("Family", (1).ToString()));
            userManager.AddClaim(bobUser.Id, new Claim("Child", (1).ToString()));
            userManager.AddClaim(bobUser.Id, new Claim("Wishlist", (1).ToString()));

            userManager.Create(new WishlistUser {
                UserName = "bscrooge@example.com",
                Email = "bscrooge@example.com",
                EmailConfirmed = true,
                Name = "Ben Scrooge"
            }, "ChristmasPast");

            var benUser = userManager.FindByName("bscrooge@example.com");
            userManager.AddToRole(benUser.Id, "Donor");
            userManager.AddClaim(benUser.Id, new Claim("Donor", (1).ToString()));
        }
    }
}
