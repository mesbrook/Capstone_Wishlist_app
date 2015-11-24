using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models {
    public class AddToCartViewModel {
        public int WishlistItemId { get; set; }
        public decimal ListPrice { get; set; }
        public string Title { get; set; }
    }

    public class CartCountViewModel {
        public int DonorId { get; set; }
        public int Count { get; set; }
    }
}