using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Capstone_Wishlist_app.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Wishlist_app.Models.Family {
    public class Family {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Address")]
        public virtual int ShippingAddressId { get; set; }

        [ForeignKey("Address")]
        public virtual int HomeAddressId { get; set; }
    }
}