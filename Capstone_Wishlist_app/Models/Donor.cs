using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models {
    public class Donor {
        [Key]
        public int Id { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }
    }

    public class Donation {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DonorId { get; set; }

        public DateTime Date { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SalesTax { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("DonorId")]
        public virtual Donor Donor { get; set; }

        public virtual ICollection<DonatedItem> Items { get; set; }
    }

    public class DonatedItem {
        [Key]
        [Column(Order = 1)]
        public int DonationId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int WishlistItemId { get; set; }

        public string Title { get; set; }
        public decimal PurchasePrice { get; set; }

        [ForeignKey("WishlistItemId")]
        public virtual WishlistItem Item { get; set; }

        [ForeignKey("DonationId")]
        public virtual Donation Donation { get; set; }
    }
}