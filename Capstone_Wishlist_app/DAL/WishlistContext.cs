using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Capstone_Wishlist_app.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Capstone_Wishlist_app.DAL
{
    public class WishlistContext : IdentityDbContext<WishlistUser>
    {

        public WishlistContext() : base("WishlistContext", false) { }

        public DbSet<Wishlist> WishLists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Address> Addresses { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}