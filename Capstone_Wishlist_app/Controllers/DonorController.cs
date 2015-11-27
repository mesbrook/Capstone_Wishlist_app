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
using System.Data.Entity.Infrastructure;
using System.Web.SessionState;
using Capstone_Wishlist_app.Services;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Capstone_Wishlist_app.Controllers {
    [SessionState(SessionStateBehavior.Required)]
    public class DonorController : Controller {
        private const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

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

        public DonorController()
            : base() {
            _db = new WishlistContext();
        }

        // GET: Donor
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignInAnonymously() {
            var donor = new Donor();
            _db.Donors.Add(donor);

            var cart = new Cart { Donor = donor, ModifiedDate = DateTime.Now };
            _db.Carts.Add(cart);

            await _db.SaveChangesAsync();

            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            var anonymousIdentifier = Guid.NewGuid().ToString();
            identity.AddClaim(new Claim("Donor", donor.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, anonymousIdentifier));
            identity.AddClaim(new Claim(IdentityProviderClaimType, anonymousIdentifier));
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [DonorAuthorize]
        public ActionResult ViewCart(int id) {
            var cart = _db.Carts.Where(c => c.DonorId == id)
                .Include(c => c.Donor)
                .Include(c => c.Items.Select(ci => ci.Item.Wishlist.Child))
                .Single();
            var items = cart.Items.Select(ci => new CartItemViewModel {
                CartId = ci.CartId,
                WishlistItemId = ci.WishlistItemId,
                ItemId = ci.Item.ItemId,
                ChildName = ci.Item.Wishlist.Child.FirstName,
                Price = ci.Price,
                Title = ci.Title
            }).ToList();

            return View(new CartViewModel {
                DonorId = id,
                Items = items
            });
        }

        [HttpPost]
        [DonorAuthorize]
        public async Task<ActionResult> AddItemToCart(int id, int wishlistItemId) {
            var isInCart = await _db.CartItems.AnyAsync(ci => ci.CartId == id && ci.Item.Id == wishlistItemId);

            if (isInCart) {
                return Json(new { WasInCart = true });
            }

            var item = await _db.WishlistItems.FindAsync(wishlistItemId);
            var retailer = new AmazonRetailer(AmazonAssociateTag, AmazonAccessKey, "AWSECommerceServicePort");
            var retailItem = (await retailer.LookupItemsAsync(new[] { item.ItemId })).FirstOrDefault();
            var cart = await _db.Carts.FindAsync(id);

            var cartItem = new CartItem {
                Cart = cart,
                Item = item,
                Price = retailItem.ListPrice,
                Title = retailItem.Title
            };

            _db.CartItems.Add(cartItem);
            await _db.SaveChangesAsync();

            return Json(new { IsInCart = true });
        }

        [HttpPost]
        [DonorAuthorize]
        public async Task<ActionResult> RemoveItemFromCart(int id, int wishlistItemId) {
            var item = await _db.CartItems.FindAsync(id, wishlistItemId);
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();

            var items = await GetViewableItemsFromCart(id);

            return PartialView("_CartItems", new CartViewModel {
                DonorId = id,
                Items = items
            });
        }

        public async Task<IList<CartItemViewModel>> GetViewableItemsFromCart(int donorId) {
            var cart = await _db.Carts.Where(c => c.DonorId == donorId)
                .Include(c => c.Donor)
                .Include(c => c.Items.Select(ci => ci.Item.Wishlist.Child))
                .SingleAsync();
            return cart.Items.Select(ci => new CartItemViewModel {
                CartId = ci.CartId,
                WishlistItemId = ci.WishlistItemId,
                ItemId = ci.Item.ItemId,
                ChildName = ci.Item.Wishlist.Child.FirstName,
                Price = ci.Price,
                Title = ci.Title
            }).ToList();
        }

        [HttpGet]
        [DonorAuthorize]
        public ActionResult CountItemsInCart(int id) {
            var count = _db.CartItems.Count(ci => ci.CartId == id);

            return PartialView("_CartCount", new CartCountViewModel {
                DonorId = id,
                Count = count
            });
        }

        [HttpGet]
        [DonorAuthorize]
        public async Task<ActionResult> PurchaseCart(int id) {
            var items = await _db.CartItems.Where(ci => ci.CartId == id)
                .Include(ci => ci.Item.Wishlist.Child)
                .ToListAsync();

            return View(new PurchaseCartViewModel {
                DonorId = id,
                BillingAddress = new CreateAddressModel(),
                Items = ToViewableItems(items)
            });
        }

        private async Task RemoveUnavailableItems(IList<CartItem> items) {
            foreach (var ci in items) {
                _db.CartItems.Remove(ci);
            }

            await _db.SaveChangesAsync();
        }

        private IList<CartItemViewModel> ToViewableItems(IList<CartItem> items) {
            return items.Select(ci => new CartItemViewModel {
                CartId = ci.CartId,
                WishlistItemId = ci.WishlistItemId,
                ItemId = ci.Item.ItemId,
                ChildName = ci.Item.Wishlist.Child.FirstName,
                Price = ci.Price,
                Title = ci.Title
            }).ToList();
        }

        [HttpPost]
        [DonorAuthorize]
        public async Task<ActionResult> PurchaseCart(int id, PurchaseCartViewModel purchase) {
            var items = await _db.CartItems.Where(ci => ci.CartId == id)
                .Include(ci => ci.Item.Wishlist.Child)
                .ToListAsync();
            var availableItems = items.Where(ci => ci.Item.Status == WishlistItemStatus.Avaliable)
                .ToList();
            var unavailableItems = items.Where(ci => ci.Item.Status != WishlistItemStatus.Avaliable)
                .ToList();

            if (unavailableItems.Any()) {
                await RemoveUnavailableItems(unavailableItems);
                TempData["unavailableItems"] = ToViewableItems(unavailableItems);
            }

            Session["order"] = BuildOrderModel(id, items);

            return RedirectToAction("ConfirmOrder", new { id = id });
        }

        private OrderViewModel BuildOrderModel(int donorId, IList<CartItem> items) {
            var orderItems = items.Select(ci => new OrderItemViewModel {
                WishlistItemId = ci.Item.Id,
                ItemId = ci.Item.ItemId,
                Price = ci.Price,
                Title = ci.Title
            }).ToList();

            var subtotal = items.Sum(oi => oi.Price);
            var shipping = subtotal * (decimal) (0.05 + new Random().NextDouble() * 0.2);
            var tax = (subtotal + shipping) * 0.05m;
            var total = subtotal + shipping + tax;

            var order = new OrderViewModel {
                DonorId = donorId,
                OrderId = Guid.NewGuid().ToString(),
                Subtotal = subtotal,
                Shipping = shipping,
                SalesTax = tax,
                Total = total,
                Items = orderItems
            };

            return order;
        }

        [HttpGet]
        [DonorAuthorize]
        public ActionResult ConfirmOrder(int id) {
            var order = Session["order"] as OrderViewModel;

            return View(order);
        }

        [HttpPost]
        [DonorAuthorize]
        public async Task<ActionResult> CompleteOrder(int id) {
            var order = TakeOrderFromSession();

            var orderedItemIds = order.Items.Select(oi => oi.WishlistItemId);
            var wishlistItems = await _db.WishlistItems.Where(wi => orderedItemIds.Contains(wi.Id))
                .ToListAsync();

            var hasReserved = await ReserveItemsForDonation(wishlistItems);

            if (!hasReserved) {
                return RedirectToAction("PurchaseCart", new { id = id });
            }

            var donationId = await BuildDonationModel(order, wishlistItems);

            await ClearCart(wishlistItems);

            return RedirectToAction("ThankYou", new { id = id, donationId = donationId });
        }

        private OrderViewModel TakeOrderFromSession() {
            var order = Session["order"] as OrderViewModel;
            Session.Remove("order");

            return order;
        }

        private async Task ClearCart(List<WishlistItem> wishlistItems) {
            var donatedIds = wishlistItems.Select(wi => wi.Id);
            var clearItems = await _db.CartItems.Where(ci => donatedIds.Contains(ci.WishlistItemId))
                .ToListAsync();

            foreach(var ci in clearItems) {
                _db.CartItems.Remove(ci);
            }

            await _db.SaveChangesAsync();
        }

        private async Task<int> BuildDonationModel(OrderViewModel order, IList<WishlistItem> wishlistItems) {
            var donation = new Donation {
                DonorId = order.DonorId,
                OrderId = order.OrderId,
                Date = DateTime.Now,
                Subtotal = order.Subtotal,
                SalesTax = order.SalesTax,
                Total = order.Total
            };

            _db.Donations.Add(donation);

            var donatedItems = order.Items.Join(wishlistItems, oi => oi.WishlistItemId, wi => wi.Id,
                (oi, wi) => new DonatedItem {
                    Donation = donation,
                    Item = wi,
                    PurchasePrice = oi.Price,
                    Title = oi.Title,
                });

            foreach (var di in donatedItems) {
                _db.DonatedItems.Add(di);
            }

            await _db.SaveChangesAsync();

            return donation.Id;
        }

        private async Task<bool> ReserveItemsForDonation(IList<WishlistItem> items) {
            foreach (var wi in items) {
                wi.Status = WishlistItemStatus.Ordered;
            }

            try {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) {
                return false;
            }
        }

        [HttpGet]
        [DonorAuthorize]
        public async Task<ActionResult> ThankYou(int id, int donationId) {
            var donation = await _db.Donations.Where(dn => dn.Id == donationId && dn.DonorId == id)
                .Include(dn => dn.Donor)
                .Include(dn => dn.Items.Select(di => di.Item.Wishlist.Child))
                .FirstAsync();
            var childNames = donation.Items.Select(di => di.Item.Wishlist.Child.FirstName)
                .Distinct()
                .ToList();

            return View(new ThankYouViewModel {
                DonorId = id,
                DonationId = donation.Id,
                Total = donation.Total,
                ChildNames = childNames
            });
        }
    }
}