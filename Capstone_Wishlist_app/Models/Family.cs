using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Capstone_Wishlist_app.Models
{
    public class Family
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Parent's First Name")]
        public string ParentFirstName { get; set; }

        [Required]
        [Display(Name = "Parent's Last Name")]
        public string ParentLastName { get; set; }

        public int? ShippingAddressId { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [ForeignKey("ShippingAddressId")]
        public virtual Address ShippingAddress { get; set; }

        public virtual ICollection<Child> Children { get; set; }
    }
}