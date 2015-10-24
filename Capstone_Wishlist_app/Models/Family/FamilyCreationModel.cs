using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Capstone_Wishlist_app.Models.Shared;

namespace Capstone_Wishlist_app.Models.Family {
    public class FamilyCreationModel {
        [Required]
        [Display(Name="Family Name", ShortName="Name")]
        public string FamilyName { get; set; }

        public AddressCreationModel HomeAddress { get; set; }

        [Required]
        public AddressCreationModel ShippingAddress { get; set; }

        [Display(Name="Phone Number", ShortName="Phone")]
        public string Phone { get; set; }

        [Display(Name="Email Address", ShortName="Email")]
        public string Email { get; set; }
    }
}