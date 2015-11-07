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
        public int Id { get; set; }

        [Required]
        public int ChildId { get; set; }

        [ForeignKey("ChildId")]
        public virtual Child Child { get; set; }

        public virtual ICollection<WishlistItem> Items { get; set; }
    }
}