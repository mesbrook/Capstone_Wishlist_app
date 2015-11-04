using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Models;
using Capstone_Wishlist_app.ViewModels;
using System.Net;
using System.Data.Entity;


namespace Capstone_Wishlist_app.Controllers
{
    public class CreateFamilyController : Controller
    {
        private WishlistContext db = new WishlistContext();

        // GET: CreateFamily
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFamily()
        {
            var viewModel = new CreateFamilyProfile();
            return View();
        }

        [HttpPost]
        public ActionResult CreateFamily(CreateFamilyProfile viewModel){

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Error. Object is not valid";
                return View(viewModel);
            }

            var family = new Family()
            {
                ParentFirstName = viewModel.ParentFirstName,
                ParentLastName = viewModel.ParentLastName,
                Shipping_address = viewModel.Shipping_address,
                Shipping_city = viewModel.Shipping_city,
                Shipping_state = viewModel.Shipping_state,
                Shipping_zipCode = viewModel.Shipping_zipCode,
                Phone = viewModel.Phone,
                Email = viewModel.Email
            };

            var child = new Child()
            {
                Family_ID = viewModel.Family_ID,
                Child_FirstName = viewModel.Child_FirstName,
                Child_LastName = viewModel.Child_LastName,
                Age = viewModel.Age,
                Gender = viewModel.Gender
            };


            db.Families.Add(family);
            db.Children.Add(child);
            db.SaveChanges();
            TempData["Message"] = "Family and Child added successfully";
                return RedirectToAction("CreateFamily");
        }
    }
}