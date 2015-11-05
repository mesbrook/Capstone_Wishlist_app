using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone_Wishlist_app.Controllers
{
    public class WishlistController : Controller
    {
        // GET: Wishlist
        public ActionResult Index()
        {
            return View();
        }

        /**
         * Removed some SQL based methods
         
        //This method should update the status of a wishlist
        //The wishlist's status is the status of the least complete WishItem
        //Should be called whenever a WishItem's Status is updated and when a Wishlist is created if a default status is not chosen
        public void updateWishlistStatus(Wishlist wl)
        {
            int n = 5;
            foreach (WishItem z in wl.WishItems)
            {
                if (z.Status.ToString() == "Unapproved" && n > 1)
                    n = 1;
                if (z.Status.ToString() == "Avaliable" && n > 2)
                    n = 2;
                if (z.Status.ToString() == "Ordered" && n > 3)
                    n = 3;
                if (z.Status.ToString() == "Shipping" && n > 4)
                    n = 4;
                if (z.Status.ToString() == "Delivered" && n > 5)
                    n = 5;
            }

            switch (n)
            {
                case 1:
                    wl.Status = "Unapproved";
                    break;
                case 2:
                    wl.Status = "Avaliable";
                    break;
                case 3:
                    wl.Status = "Ordered";
                    break;
                case 4:
                    wl.Status = "Shipping";
                    break;
                case 5:
                    wl.Status = "Delivered";
                    break;
            }
        } 
         */
    }
}