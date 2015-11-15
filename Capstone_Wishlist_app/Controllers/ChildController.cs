using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Models;

namespace Capstone_Wishlist_app.Controllers {
    public class ChildController : Controller {
        private WishlistContext _db = new WishlistContext();

        // GET: Child
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile(int id) {
            var child = await _db.Children.FindAsync(id);
            var bioText = await (
                from b in _db.Biographies
                where b.ChildId == id
                orderby b.CreationDate descending
                select b.Text).FirstOrDefaultAsync();

            return View(new EditChildProfileModel {
                Id = child.Id,
                FirstName = child.FirstName,
                LastName = child.LastName,
                BiographyText = bioText
            });
        }

        [HttpPost]
        public async Task<ActionResult> EditProfile(int id, EditChildProfileModel edit) {
            if (!ModelState.IsValid) {
                edit.Id = id;
                return View(edit);
            }

            var bio = new ChildBiography {
                ChildId = id,
                CreationDate = DateTime.Now,
                Text = edit.BiographyText
            };

            _db.Biographies.Add(bio);
            await _db.SaveChangesAsync();

            TempData["profileSaved"] = true;
            return RedirectToAction("EditProfile", new { id = id });
        }

        [HttpGet]
        public async Task<ActionResult> ViewProfile(int id) {
            var child = await _db.Children.FindAsync(id);

            var latestBio = child.Biographies.OrderByDescending(b => b.CreationDate)
                .Select(b => b.Text)
                .FirstOrDefault();

            var firstWishlist = child.Wishlists.FirstOrDefault();

            return View(new ChildViewModel {
                ChildId = child.Id,
                FamilyId = child.FamilyId,
                WishlistId = firstWishlist == null ? 0 : firstWishlist.Id,
                FirstName = child.FirstName,
                Age = child.Age,
                Gender = child.Gender,
                BiographyText = latestBio
            });
        }
    }
}