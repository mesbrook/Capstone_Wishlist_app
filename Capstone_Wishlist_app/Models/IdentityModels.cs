using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Capstone_Wishlist_app.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class WishlistUser : IdentityUser
    {
        public string Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<WishlistUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public const string AdminRole = "Admin";
    }

    public class FamilyAuthorizeAttribute : AuthorizeAttribute {
        public string Entity { get; set; }

        public override void OnAuthorization(AuthorizationContext context) {
            var id = context.RequestContext.RouteData.Values["id"];
            var claimsUser = (ClaimsPrincipal)context.HttpContext.User;

            if (!claimsUser.HasClaim(Entity, id.ToString()) && !claimsUser.IsInRole(WishlistUser.AdminRole)) {
                HandleUnauthorizedRequest(context);
            }
            base.OnAuthorization(context);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context) {
            base.HandleUnauthorizedRequest(context);

            if (context.HttpContext.User.Identity.IsAuthenticated) {
                context.Result = new RedirectResult("~/Home/Unauthorized");
            }
        }
    }
}