using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models {
    public class Cart {
        [Key]
        public int DonorId { get; set; }

        [Required]
        [ForeignKey("DonorId")]
        public virtual Donor Donor { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }

    public class CartItem {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int WishlistItemId { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }

        [Required]
        [ForeignKey("WishlistItemId")]
        public virtual WishlistItem Item { get; set; }
    }
}