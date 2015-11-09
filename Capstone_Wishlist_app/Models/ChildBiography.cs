using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone_Wishlist_app.Models {
    public class ChildBiography {
        [Key]
        public int Id { get; set; }

        public int ChildId { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        [ForeignKey("ChildId")]
        public virtual Child Child { get; set; }
    }
}