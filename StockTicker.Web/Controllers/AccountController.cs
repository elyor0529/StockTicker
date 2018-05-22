using System.Threading.Tasks;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using StockTicker.Lib.Common.Exceptions;
using StockTicker.Lib.Common.Memberships;
using StockTicker.Web.AccountServiceReference;
using StockTicker.Web.Helpers;
using StockTicker.Web.Models;
using MemberRole = StockTicker.Web.AccountServiceReference.MemberRole;

namespace StockTicker.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountWebServiceSoapClient _client;

        public AccountController()
        {
            _client = new AccountWebServiceSoapClient();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);
             
            try
            {
                var result = await _client.LoginAsync(model.Email, model.Password);

                if (result.Body.LoginResult.Success)
                {
                    MembershipHelper.SignIn(result.Body.LoginResult.Result);

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", result.Body.LoginResult.Error);
                }
            }
            catch (StockException exception)
            {
                ModelState.AddModelError("", exception.Message);
            }

            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", ControlHelper.CAPTCHA_KEY, "Incorrect CAPTCHA code!")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
             
            try
            {
                var result = await _client.RegisterAsync(model.FullName, model.Email, model.Password);

                if (result.Body.RegisterResult.Success)
                {
                    MembershipHelper.SignIn(result.Body.RegisterResult.Result);

                    MvcCaptcha.ResetCaptcha(ControlHelper.CAPTCHA_KEY);

                    return RedirectToLocal();
                }
                else
                {
                    ModelState.AddModelError("", result.Body.RegisterResult.Error);
                }
            }
            catch (StockException exception)
            {
                ModelState.AddModelError("", exception.Message);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl = "/")
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            MembershipHelper.SignOut();

            return RedirectToLocal();
        }
    }
}