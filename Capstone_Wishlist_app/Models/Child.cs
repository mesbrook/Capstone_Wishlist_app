using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Wishlist_app.Models {
    public class Child {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FamilyId { get; set; }

        [Required]
        [Display(Name = "Child's First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Child's Last Name")]
        public string LastName { get; set; }

        public int Age { get; set; }

        public char Gender { get; set; }

        [ForeignKey("FamilyId")]
        public virtual Family Family { get; set; }

        public virtual ICollection<Wishlist> Wishlists { get; set; }

        public virtual ICollection<ChildBiography> Biographies { get; set; }
    }
}