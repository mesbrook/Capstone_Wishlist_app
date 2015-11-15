using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Security.Claims;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Models;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

namespace Capstone_Wishlist_app.Controllers {
    public class FamilyController : Controller {
        private WishlistContext _db = new WishlistContext();

        // GET: Family
        public ActionResult Index() {
            return View(_db.Families.ToList());
        }

        // GET: Family/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = _db.Families.Find(id);
            if (family == null) {
                return HttpNotFound();
            }
            return View(family);
        }

        // GET: Family/Create
        public ActionResult Create() {
            //List<Child> ci = new List<Child> { new Child { Child_ID = 0, Child_FirstName = "", Child_LastName = "", Age = 0 } };
            return View();
        }

        // POST: Family/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Family_ID,ParentFirstName,ParentLastName,Shipping_address,Shipping_city,Shipping_state,Shipping_zipCode,Phone,Email")] Family family) {
            if (ModelState.IsValid) {
                _db.Families.Add(family);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family);
        }

        // GET: Family/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = _db.Families.Find(id);
            if (family == null) {
                return HttpNotFound();
            }
            return View(family);
        }

        // POST: Family/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Family_ID,ParentFirstName,ParentLastName,Shipping_address,Shipping_city,Shipping_state,Shipping_zipCode,Phone,Email")] Family family) {
            if (ModelState.IsValid) {
                _db.Entry(family).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(family);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Register() {
            return View(new RegisterFamilyModel { ShippingAddress = new CreateAddressModel { } });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterFamilyModel registration) {
            if (!ModelState.IsValid) {
                return View(registration);
            }

            var family = await CreateFamilyModel(registration);
            var familyCredentials = await CreateFamilyAccount(family);

            TempData["firstTimeRegistration"] = true;
            TempData["familyCredentials"] = familyCredentials;

            return RedirectToAction("RegisterChild", new { id = family.Id });
        }

        private async Task<Family> CreateFamilyModel(RegisterFamilyModel registration) {
            var family = new Family {
                ParentFirstName = registration.ParentFirstName,
                ParentLastName = registration.ParentLastName,
                Phone = registration.Phone,
                Email = registration.Email
            };

            if (!registration.IsShippingToCharity) {
                family.ShippingAddress = new Address {
                    LineOne = registration.ShippingAddress.LineOne,
                    LineTwo = registration.ShippingAddress.LineTwo,
                    City = registration.ShippingAddress.City,
                    State = registration.ShippingAddress.State,
                    PostalCode = registration.ShippingAddress.PostalCode
                };
            }

            _db.Families.Add(family);
            await _db.SaveChangesAsync();
            return family;
        }

        private async Task<FamilyCredentials> CreateFamilyAccount(Family family) {
            var userNameChars = family.ParentLastName.ToLowerInvariant()
                .ToCharArray()
                .Where(c => char.IsLetter(c))
                .ToArray();
            var userName = new string(userNameChars);
            var password = GenerateRandomPassword(8);
            var userStore = new UserStore<WishlistUser>(_db);
            var userManager = new WishlistUserManager(userStore);
            await userManager.CreateAsync(new WishlistUser {
                UserName = userName,
                Email = family.Email,
                PhoneNumber = family.Phone
            }, password);

            var createdUser = await userManager.FindByNameAsync(userName);
            await userManager.AddToRoleAsync(createdUser.Id, "Family");
            await userManager.AddClaimAsync(createdUser.Id, new Claim("Family", family.Id.ToString()));

            return new FamilyCredentials {
                Username = userName,
                Password = password
            };
        }

        private static string GenerateRandomPassword(int maxLength) {
            var cryptoProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[(maxLength / 4) * 3];
            cryptoProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        [HttpGet]
        public ActionResult Administer(int id) {
            var family = _db.Families.Where(f => f.Id == id)
                .Include(f => f.Children)
                .Single();

            return View(family);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(int id) {
            var familyUser = await _db.Users.Where(
                u => u.Claims.Any(c => c.ClaimType == "Family" && c.ClaimValue == id.ToString())
                ).FirstAsync();

            using (var userStore = new UserStore<WishlistUser>(_db))
            using (var userManager = new WishlistUserManager(userStore)) {
                var password = GenerateRandomPassword(8);
                var hashedPassword = userManager.PasswordHasher.HashPassword(password);
                await userStore.SetPasswordHashAsync(familyUser, hashedPassword);
                await userStore.UpdateAsync(familyUser);

                TempData["familyCredentials"] = new FamilyCredentials {
                    Username = familyUser.UserName,
                    Password = password
                };
                return RedirectToAction("Administer", new { id = id });
            }
        }

        [HttpGet]
        [FamilyAuthorize(Entity = "Family")]
        public async Task<ActionResult> RegisterChild(int id) {
            var family = await _db.Families.FindAsync(id);

            return View(new RegisterChildModel {
                FamilyId = id,
                FamilyName = family.ParentLastName
            });
        }

        [HttpPost]
        [FamilyAuthorize(Entity = "Family")]
        public async Task<ActionResult> RegisterChild(int id, RegisterChildModel registration) {
            if (!ModelState.IsValid) {
                return View(registration);
            }

            var child = new Child {
                FamilyId = registration.FamilyId,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Age = registration.Age,
                Gender = registration.Gender
            };

            _db.Children.Add(child);

            var bio = new ChildBiography {
                Child = child,
                CreationDate = DateTime.Now,
                Text = registration.BiographyText
            };

            _db.Biographies.Add(bio);

            var wishlist = new Wishlist {
                Child = child
            };

            _db.WishLists.Add(wishlist);
            await _db.SaveChangesAsync();
            await AuthorizeChildAndWishlistForFamilyUser(child, wishlist);

            TempData["registeredChild"] = child;
            return RedirectToAction("RegisterChild", new { id = registration.FamilyId });
        }

        private async Task AuthorizeChildAndWishlistForFamilyUser(Child child, Wishlist wishlist) {
            var familyUser = await _db.Users.Where(
                u => u.Claims.Any(c => c.ClaimType == "Family" && c.ClaimValue == child.FamilyId.ToString())
                ).FirstAsync();

            using (var userStore = new UserStore<WishlistUser>(_db))
            using (var userManager = new WishlistUserManager(userStore)) {
                await userManager.AddClaimAsync(familyUser.Id, new Claim("Child", child.Id.ToString()));
                await userManager.AddClaimAsync(familyUser.Id, new Claim("Wishlist", wishlist.Id.ToString()));

                if (User.Identity.GetUserId() == familyUser.Id) {
                    var claimsIdenity = (ClaimsIdentity) User.Identity;
                    claimsIdenity.AddClaim(new Claim("Child", child.Id.ToString()));
                    claimsIdenity.AddClaim(new Claim("Wishlist", wishlist.Id.ToString()));
                    HttpContext.GetOwinContext().Authentication.SignIn(claimsIdenity);
                }
            }
        }

        [HttpGet]
        [FamilyAuthorize(Entity = "Family")]
        public ActionResult ViewWishlists(int id) {
            var wishlists = (
                from w in _db.WishLists
                where w.Child.FamilyId == id
                select w).ToList();

            return View(wishlists.Select(w => new FamilyWishlistViewModel {
                WishlistId = w.Id,
                ChildId = w.ChildId,
                ChildFirstName = w.Child.FirstName,
                Items = new List<WishlistItem>(w.Items)
            }));
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
