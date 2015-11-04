using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace Capstone_Wishlist_app.Models
{
    public class Family
    {
        [Key]
        public int Family_ID { get; set; }

        [Required]
        [Display(Name = "Parent's First Name")]
        public string ParentFirstName { get; set; }

        [Required]
        [Display(Name = "Parent's Last Name")]
        public string ParentLastName { get; set; }

        [Required]
        public string Shipping_address { get; set; }

        [Required]
        public string Shipping_city { get; set; }

        [Required]
        public string Shipping_state { get; set; }

        [Required]
        public string Shipping_zipCode { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        public virtual ICollection<Child> Children { get; set; }
    }
}