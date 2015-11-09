using System.ComponentModel.DataAnnotations;

namespace Capstone_Wishlist_app.Models {
    public class EditChildProfileModel {
        [Required]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [Display(Name="Short Biography")]
        public string BiographyText { get; set; }
    }

    public class ChildViewModel {
        public int ChildId { get; set; }
        public int FamilyId { get; set; }
        public int WishlistId { get; set; }

        [Display(Name = "Name")]
        public string FirstName { get; set; }

        public int Age { get; set; }
        public char Gender { get; set; }

        [Display(Name = "Short Biography")]
        public string BiographyText { get; set; }
    }
}