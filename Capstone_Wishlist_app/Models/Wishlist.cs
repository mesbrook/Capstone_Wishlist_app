using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Wishlist_app.Models
{
    public class Wishlist
    {
        [Key]
        public int whishlist_id { get; set; }

       // [ForeignKey("WishItem")]
        [Required]
        public string ASIN { get; set; }

        [ForeignKey("Child")]
        [Required]
        public int Child_Id { get; set; }

        public bool Filled { get; set; }


        public virtual Child Child { get; set; }

        public virtual ICollection<WishItem> WishItems { get; set; }

        public Wishlist()
        {
            Filled = false;
        }

        //The possible order statuses for a wishlist
        public enum ItemStatus { Unapproved, Avaliable, Ordered, Shipping, Delivered };

        //The order status of the wishlist, obtained from the status of the WishItem(s) in the wishlist
        //The possible statuses are Unapproved, Avaliable, Ordered, Shipping, Delivered
        public ItemStatus Status { get; set; }
    }
}