using Capstone_Wishlist_app.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.ServiceModel.Web;
using Capstone_Wishlist_app.Services;
using System.Security.Claims;

namespace Capstone_Wishlist_app.Controllers {
    [InjectDonorIdentity]
    public class HomeController : Controller {

        public ActionResult Index() {
            if (User.IsInRole("Family")) {
                ViewBag.FamilyId = GetFamilyIdFromUserIdentity();
            }

            return View();
        }

        [Authorize]
        public ActionResult About() {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Unauthorized() {
            return View();
        }

        private int GetFamilyIdFromUserIdentity() {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var familyId = claimsIdentity.FindFirstValue("Family");
            return int.Parse(familyId);
        }
    }
}