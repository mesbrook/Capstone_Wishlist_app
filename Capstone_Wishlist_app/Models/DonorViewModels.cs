using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class CartViewModel {
        public int DonorId { get; set; }
        public IList<CartItemViewModel> Items { get; set; }
    }

    public class CartItemViewModel {
        public int CartId { get; set; }
        public int WishlistItemId { get; set; }
        public int WishlistId { get; set; }
        public string ItemId { get; set; }
        public string Title { get; set; }

        [DisplayFormat(DataFormatString="{0:C}")]
        public decimal Price { get; set; }
        public string ChildName { get; set; }
    }

    public class PurchaseCartViewModel {
        public int DonorId { get; set; }

        [Display(Name = "Credit Card Number")]
        [RegularExpression(@"\d{16}", ErrorMessage="Card number must be 16 digits.")]
        public string Number { get; set; }

        [Display(Name = "Name On Card")]
        public string Name { get; set; }

        [Display(Name = "Verification Code")]
        public string VerficationCode { get; set; }

        [Display(Name = "Month")]
        [Range(1, 12)]
        public int ExpirationMonth { get; set; }

        [Display(Name = "Year")]
        [Range(0, int.MaxValue)]
        public int ExpirationYear { get; set; }

        public CreateAddressModel BillingAddress { get; set; }

        public IList<CartItemViewModel> Items { get; set; }
    }

    public class OrderViewModel {
        public int DonorId;
        public string OrderId;

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Subtotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Shipping { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SalesTax { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public IList<OrderItemViewModel> Items { get; set; }
    }

    public class OrderItemViewModel {
        public int WishlistItemId { get; set; }
        public string ItemId { get; set; }
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
    }

    public class ThankYouViewModel {
        public int DonorId { get; set; }
        public int DonationId { get; set; }
        public decimal Total { get; set; }
        public ICollection<string> ChildNames { get; set; }
    }
}