using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Capstone_Wishlist_app.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Capstone_Wishlist_app.DAL
{
    public class WishlistContext : DbContext
    {

        public WishlistContext() : base("WishlistContext") { }

        public DbSet<WishItem> Items { get; set; }
        public DbSet<Wishlist> WishLists { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Family> Families { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}