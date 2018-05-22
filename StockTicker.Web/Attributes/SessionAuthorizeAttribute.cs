using System.Web.Mvc;
using System.Web.Routing;
using StockTicker.Web.Helpers;

namespace StockTicker.Web.Attributes
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            // Check for authorization
            if (MembershipHelper.IsAuthorize())
                return;

            var routeValues = new RouteValueDictionary(new
            {
                action = "Login",
                controller = "Account",
                redirectUrl = filterContext.RequestContext.HttpContext.Request["redirectUrl"]
            });

            filterContext.Result = new RedirectToRouteResult(routeValues);
        }
    }
}