using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.ViewModels
{
    public class CreateFamilyProfile
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
        [Display(Name = "Shipping Street Address")]
        public string Shipping_address { get; set; }

        [Required]
        [Display(Name = "Shipping City")]
        public string Shipping_city { get; set; }

        [Required]
        [Display(Name = "Shipping State")]
        public string Shipping_state { get; set; }

        [Required]
        [Display(Name = "Shipping ZipCode")]
        public string Shipping_zipCode { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Child's First Name")]
        public string Child_FirstName { get; set; }

        [Required]
        [Display(Name = "Child's Last Name")]
        public string Child_LastName { get; set; }

        public int Age { get; set; }

        public char Gender { get; set; }

    }
}