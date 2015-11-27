using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        public int WishlistId { get; set; }

        public string ItemId { get; set; }

        [ForeignKey("WishlistId")]
        public virtual Wishlist Wishlist { get; set; }
        
        //The order status of the item
        public WishlistItemStatus Status { get; set; }
    }

    //The possible order statuses for all items
    public enum WishlistItemStatus {
        Unapproved,
        Avaliable,
        Ordered
    };
}