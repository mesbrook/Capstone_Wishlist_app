using System.Collections.Generic;
using Capstone_Wishlist_app.Services;
using System.ComponentModel.DataAnnotations;

namespace Capstone_Wishlist_app.Models {
    public static class ItemViewExtensions {
        public static string GetFormattedAgeRange(this Item item) {
            if (item.MinAgeMonths == 0 && item.MaxAgeMonths == 0) {
                return "all ages";
            }
            if (item.MinAgeMonths == 0) {
                return "up to " + FormatAge(item.MaxAgeMonths);
            }
            if (item.MaxAgeMonths == 0) {
                return FormatAge(item.MinAgeMonths) + " and up";
            }

            return FormatAge(item.MinAgeMonths) + " to " + FormatAge(item.MaxAgeMonths);
        }

        private static string FormatAge(int ageMonths) {
            if (ageMonths < 24) {
                return string.Format("{0} months");
            }

            if (ageMonths % 12 != 0) {
                return string.Format("{0} years {1} months", ageMonths / 12, ageMonths % 12);
            }

            return string.Format("{0} years", ageMonths / 12);
        }
    }

    public class FindGiftsViewModel {
        public int WishlistId { get; set; }
        public string ChildFirstName { get; set; }
    }

    public class FindGiftsResultsViewModel {
        public int WishlistId { get; set; }
        public IList<Item> Results { get; set; }
        public ICollection<string> ExistingItemIds { get; set; }
    }

    public class DonorListViewModel
    {
        public int ChildId { get; set; }
        public int FamilyId { get; set; }
        public List<Wishlist>  Wishlists { get; set; }

        [Display(Name = "Name")]
        public string FirstName { get; set; }

        public int Age { get; set; }
        public char Gender { get; set; }

        [Display(Name = "Short Biography")]
        public string Biographies { get; set; }

        public IList<Item> items { get; set; }
    }
}