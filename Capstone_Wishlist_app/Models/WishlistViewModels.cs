﻿using System;
using System.Linq;
using System.Collections.Generic;
using Capstone_Wishlist_app.Services;

namespace Capstone_Wishlist_app.Models {
    public static class AgeRange {
        public static string FormatAgeRange(int minAgeMonths, int maxAgeMonths) {
            if (minAgeMonths == 0 && maxAgeMonths == 0) {
                return "all ages";
            }
            if (minAgeMonths == 0) {
                return "up to " + FormatAge(maxAgeMonths);
            }
            if (maxAgeMonths == 0) {
                return FormatAge(minAgeMonths) + " and up";
            }

            return FormatAge(minAgeMonths) + " to " + FormatAge(maxAgeMonths);
        }

        public static string FormatAge(int ageMonths) {
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

    public class OwnWishlistViewModel {
        public int WishlistId { get; set; }
        public int ChildId { get; set; }
        public int FamilyId { get; set; }
        public string ChildFirstName { get; set; }
        public string ChildLastName { get; set; }
        public IList<WishlistItemViewModel> Items { get; set; }
    }

    public class WishlistItemViewModel {
        public int Id { get; set; }
        public int WishlistId { get; set; }
        public string ItemId { get; set; }
        public string Title { get; set; }
        public decimal ListPrice { get; set; }
        public string ImageUrl { get; set; }
        public string ListingUrl { get; set; }
        public int MinAgeMonths { get; set; }
        public int MaxAgeMonths { get; set; }
        public WishlistItemStatus Status { get; set; }
    }

    public class ApproveWishlistViewModel {
        public int WishlistId { get; set; }
        public int ChildId { get; set; }
        public int FamilyId { get; set; }
        public string ChildFirstName { get; set; }
        public string ChildLastName { get; set; }
        public IList<ApproveItemViewModel> Items { get; set; }
    }

    public class ApproveItemViewModel : WishlistItemViewModel {
        public bool IsSelected { get; set; }
    }

    public static class WishlistItemViewExtensions {
        private static readonly IReadOnlyCollection<WishlistItemStatus> donatedStatuses = new[] {
            WishlistItemStatus.Ordered,
            WishlistItemStatus.Shipping,
            WishlistItemStatus.Delivered };

        public static int GetPercentDonated(this IList<WishlistItem> items) {
            var itemCount = items.Count;
            var donatedCount = items.GetCountDonated();
            return (int) ((float) donatedCount / Math.Max(itemCount, 1) * 100);
        }

        public static int GetCountDonated(this IList<WishlistItem> items) {
            return items.Count(i => donatedStatuses.Contains(i.Status));
        }
    }
}