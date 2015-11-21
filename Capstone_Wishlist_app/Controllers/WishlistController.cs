using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using System.Threading.Tasks;
using Capstone_Wishlist_app.Models;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Services;

namespace Capstone_Wishlist_app.Controllers {
    public class WishlistController : Controller {
        private static string AmazonAccessKey {
            get {
                return ConfigurationManager.AppSettings["AWSAccessKeyId"];
            }
        }

        private static string AmazonAssociateTag {
            get {
                return ConfigurationManager.AppSettings["AWSAssociatesId"];
            }
        }

        private WishlistContext _context;
        private IRetailer _retailer;

        public WishlistController()
            : base() {
            _context = new WishlistContext();
            _retailer = new AmazonRetailer(AmazonAssociateTag, AmazonAccessKey, "AWSECommerceServicePort");
        }

        public async Task<ActionResult> Index() {
            var results = _context.WishLists.Include(c => c.Child).Include(i => i.Items).Include(w => w.Child.Biographies).ToList();
                                 
          var WLlist = new List<DonorListViewModel>();
            foreach (var item in results)
            {
                //string[] WLitems = new string[item.Items.Select(w => w.ItemId).Count()];
                string[] WLitems = item.Items.Select(w => w.ItemId).ToArray();
                var items = await _retailer.LookupItemsAsync(WLitems);
                WLlist.Add(new DonorListViewModel(){
                    ChildId = item.ChildId,
                    FamilyId = item.Child.FamilyId,
                    WishlistId = item.Id,
                    FirstName = item.Child.FirstName,
                    Age = item.Child.Age,
                    Gender = item.Child.Gender,
                    Biographies = item.Child.Biographies.OrderBy(b => b.CreationDate).First().Text,                   
                    retailItems = items.ToList()
                });
            }
            return View(WLlist);
        }

        

        public ActionResult FindGifts(int id) {
            var wishlist = _context.WishLists.Find(id);

            return View(new FindGiftsViewModel {
                WishlistId = id,
                ChildFirstName = wishlist.Child.FirstName
            });
        }

        [HttpGet]
        public async Task<ActionResult> SearchItems(int id, ItemCategory category, string keywords) {
            var existingItemIds = await (
                from wi in _context.WishlistItems
                where wi.WishlistId == id
                select wi.ItemId).ToListAsync();
            var items = await _retailer.FindItemsAsync(category, keywords);
            var viewModel = new FindGiftsResultsViewModel {
                WishlistId = id,
                Results = items.ToList(),
                ExistingItemIds = existingItemIds
            };

            return PartialView("_SearchResults", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddItem(int id, string itemId) {
            var isItemOnWishlist = await (
                from wi in _context.WishlistItems
                where wi.WishlistId == id && wi.ItemId == itemId
                select wi
                ).AnyAsync();

            if (isItemOnWishlist) {
                return Json(new { IsOnWishlist = true });
            }

            var wishItem = new WishlistItem {
                WishlistId = id,
                ItemId = itemId,
                Status = WishlistItemStatus.Unapproved,
            };

            _context.WishlistItems.Add(wishItem);
            await _context.SaveChangesAsync();

            return Json(new { IsOnWishlist = true });
        }
    }
}