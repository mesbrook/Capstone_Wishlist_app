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

namespace Capstone_Wishlist_app.Controllers
{
    public class DonorController : Controller
    {
        private WishlistContext _db;

        public DonorController() : base() {
            _db = new WishlistContext();
        }

        // GET: Donor
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewCart(int id) {
            var cart = _db.Carts.Where(c => c.DonorId == id)
                .Include(c => c.Donor)
                .Include(c => c.Items.Select(ci => ci.Item))
                .First();

            return View(cart);
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToCart(int id, AddToCartViewModel addition) {
            var isInCart = await _db.CartItems.AnyAsync(ci => ci.CartId == id && ci.Item.Id == addition.WishlistItemId);

            if (isInCart) {
                return Json(new { WasInCart = true });
            }

            var cart = await _db.Carts.Where(c => c.DonorId == id)
                .FirstAsync();
            var item = await _db.WishlistItems.FindAsync(addition.WishlistItemId);
            var cartItem = new CartItem {
                Cart = cart,
                Item = item,
                Price = addition.ListPrice,
                Title = addition.Title
            };

            _db.CartItems.Add(cartItem);
            await _db.SaveChangesAsync();

            return Json(new { IsInCart = true });
        }
    }
}