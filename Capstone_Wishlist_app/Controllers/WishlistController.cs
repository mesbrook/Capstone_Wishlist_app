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

        private WishlistContext _db;
        private IRetailer _retailer;

        public WishlistController()
            : base() {
            _db = new WishlistContext();
            _retailer = new AmazonRetailer(AmazonAssociateTag, AmazonAccessKey, "AWSECommerceServicePort");
        }

        public async Task<ActionResult> Index() {
            var results = _db.WishLists.Include(c => c.Child).Include(i => i.Items).Include(w => w.Child.Biographies).ToList();
                                 
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
                   // Gender = item.Child.Gender,
                    Biographies = item.Child.Biographies.OrderBy(b => b.CreationDate).Select( b => b.Text).FirstOrDefault(),                   
                    retailItems = items.ToList()                    
                });
            }
            return View(WLlist);
        }


        [HttpGet]
        [FamilyAuthorize(Entity="Wishlist")]

        public ActionResult FindGifts(int id) {
            var wishlist = _db.WishLists.Find(id);

            return View(new FindGiftsViewModel {
                WishlistId = id,
                ChildFirstName = wishlist.Child.FirstName
            });
        }

        [HttpGet]
        [FamilyAuthorize(Entity="Wishlist")]
        public async Task<ActionResult> SearchItems(int id, ItemCategory category, string keywords) {
            var existingItemIds = await (
                from wi in _db.WishlistItems
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
        [FamilyAuthorize(Entity="Wishlist")]
        public async Task<ActionResult> AddItem(int id, string itemId) {
            var isItemOnWishlist = await (
                from wi in _db.WishlistItems
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

            _db.WishlistItems.Add(wishItem);
            await _db.SaveChangesAsync();

            return Json(new { IsOnWishlist = true });
        }

        [HttpPost]
        [FamilyAuthorize(Entity="Wishlist")]
        public async Task<ActionResult> RemoveItem(int id, int itemId) {
            var wishItem = await _db.WishlistItems.Where(wi => wi.Id == itemId)
                .FirstOrDefaultAsync();

            if (wishItem != null) {
                _db.WishlistItems.Remove(wishItem);
                await _db.SaveChangesAsync();
                TempData["itemRemoved"] = wishItem.ItemId;
            }
            
            var wishlist = await _db.WishLists.Where(w => w.Id == id)
                .Include(w => w.Items)
                .FirstAsync();
            var items = await GetViewableItems(wishlist);

            return PartialView("_OwnItems", items);
        }

        [HttpGet]
        [FamilyAuthorize(Entity="Wishlist")]
        public async Task<ActionResult> ViewOwn(int id) {
            var wishlist = _db.WishLists.Find(id);
            var items = await GetViewableItems(wishlist);

            return View(new OwnWishlistViewModel {
                WishlistId = wishlist.Id,
                ChildId = wishlist.ChildId,
                FamilyId = wishlist.Child.FamilyId,
                ChildFirstName = wishlist.Child.FirstName,
                ChildLastName = wishlist.Child.LastName,
                Items = items
            });
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Approve(int id) {
            var wishlist = await _db.WishLists.Where(w => w.Id == id)
                .Include(w => w.Items)
                .Include(w => w.Child)
                .FirstOrDefaultAsync();
            var items = wishlist.Items.Select(wi => new ApproveItemViewModel {
                Id = wi.Id,
                WishlistId = wi.WishlistId,
                ItemId = wi.ItemId,
                Status = wi.Status,
                IsSelected = false
            }).ToList();

            await AddRetailerItemProperties(items);

            return View(new ApproveWishlistViewModel {
                WishlistId = wishlist.Id,
                ChildId = wishlist.ChildId,
                FamilyId = wishlist.Child.FamilyId,
                ChildFirstName = wishlist.Child.FirstName,
                ChildLastName = wishlist.Child.LastName,
                Items = items
            });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Approve(int id, ApproveWishlistViewModel approval) {
            var items = await _db.WishlistItems.Where(wi => wi.WishlistId == id)
                .ToListAsync();
            var itemApprovals = items.Join(approval.Items, wi => wi.Id, ai => ai.Id,
                (wi, ai) => new { Item = wi, IsApproved = ai.IsSelected });

            foreach (var ia in itemApprovals) {
                if (ia.IsApproved && ia.Item.Status == WishlistItemStatus.Unapproved) {
                    ia.Item.Status = WishlistItemStatus.Avaliable;
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Approve", new { id = id });
        }

        private async Task AddRetailerItemProperties(IList<ApproveItemViewModel> items) {
            var itemIds = items.Select(i => i.ItemId).ToArray();
            var retailItems = await _retailer.LookupItemsAsync(itemIds);
            var itemMatches = items.Join(retailItems, wi => wi.ItemId, ri => ri.Id,
                (wi, ri) => new{ Item = wi, RetailItem = ri});

            foreach (var match in itemMatches) {
                var wi = match.Item;
                var ri = match.RetailItem;
                wi.Title = ri.Title;
                wi.ListingUrl = ri.ListingUrl;
                wi.ImageUrl = ri.ImageUrl;
                wi.ListPrice = ri.ListPrice;
                wi.MinAgeMonths = ri.MinAgeMonths;
                wi.MaxAgeMonths = ri.MaxAgeMonths;
            }
        }
        private async Task<IList<WishlistItemViewModel>> GetViewableItems(Wishlist wishlist) {
            var itemIds = wishlist.Items.Select(i => i.ItemId).ToArray();
            var retailItems = await _retailer.LookupItemsAsync(itemIds);

            return wishlist.Items.Join(retailItems, wi => wi.ItemId, ri => ri.Id,
                (wi, ri) => new WishlistItemViewModel {
                    Id = wi.Id,
                    WishlistId = wi.WishlistId,
                    ItemId = wi.ItemId,
                    Status = wi.Status,
                    Title = ri.Title,
                    ListingUrl = ri.ListingUrl,
                    ImageUrl = ri.ImageUrl,
                    ListPrice = ri.ListPrice,
                    MinAgeMonths = ri.MinAgeMonths,
                    MaxAgeMonths = ri.MaxAgeMonths
                }).ToList();
        }
    }
}