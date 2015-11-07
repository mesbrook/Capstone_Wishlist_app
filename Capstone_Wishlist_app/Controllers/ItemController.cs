using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;
using Capstone_Wishlist_app.DAL;
using Capstone_Wishlist_app.Models;
using Capstone_Wishlist_app.Services;

namespace Capstone_Wishlist_app.Controllers
{
    public class ItemController : Controller
    {
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

        private IRetailer _retailer;

        public ItemController(): base() {
            _retailer = new AmazonRetailer(AmazonAssociateTag, AmazonAccessKey, "AWSECommerceServicePort");
        }

        [HttpGet]
        public async Task<ActionResult> Search(ItemCategory category, string keywords) {
            var items = await _retailer.FindItemsAsync(category, keywords);
            return PartialView("_SearchResults", items);
        }
    }
}
