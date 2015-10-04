using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models
{
    public class Item
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string ASIN { get; set; }

        public string Title { get; set; }

        public string Gender { get; set; }

        public int AgeGroup { get; set; }//set in months

        public int Amount { get; set; } 

        public string FormattedPrice { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrlSmall { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrlMed { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string Features { get; set; }
    }
}