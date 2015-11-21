using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models
{
    public class ShoppingCartItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal ListPrice { get; set; }
        public string ImageUrl { get; set; }
        public string ListingUrl { get; set; }
        public int MinAgeMonths { get; set; }
        public int MaxAgeMonths { get; set; }
    }

    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ItemId { get; set; }
    }
}
    