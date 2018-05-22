using System.Web.Mvc;
using StockTicker.Web.Attributes;
using StockTicker.Web.Helpers;
using StockTicker.Web.StockServiceReference;

namespace StockTicker.Web.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {
        private readonly StockWebServiceSoapClient _client;

        public HomeController()
        {
            _client = new StockWebServiceSoapClient();
        }

        //
        // GET: /Home/Index
        public ActionResult Index()
        {
            var credentials = new UserCredentials
            {
                UserName = MembershipHelper.User.UserName,
                Password = MembershipHelper.User.Password,
            };
            var model = _client.GetList(credentials);

            if (model.Success)
            {
                return View(model.Result);
            }
            else
            {
                ViewBag.Error = model.Error;
            }

            return View();
        }
    }
}