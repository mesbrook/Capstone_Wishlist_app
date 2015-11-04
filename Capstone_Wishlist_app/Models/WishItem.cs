using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models
{
    public class WishItem
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]        
        [Index("IX_FirstAndSecond", IsUnique = true)]
        public string ASIN { get; set; }

        public string Title { get; set; }

        public bool Approved { get; set; }

        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}