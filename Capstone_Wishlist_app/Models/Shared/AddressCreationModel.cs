using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models.Shared {
    public class AddressCreationModel {
        [Required]
        [Display(Name="Address Line 1", ShortName="Line 1")]
        public string LineOne { get; set; }

        [Display(Name="Address Line 2", ShortName="Line 2")]
        public string LineTwo { get; set; }

        [Display(Name="City or Locality", ShortName="City")]
        public string City { get; set; }

        [Required]
        [Display(Name="State or Territory", ShortName="State")]
        public string State { get; set; }

        [Display(Name="Postal Code", ShortName="Zip")]
        public string PostalCode { get; set; }
    }
}