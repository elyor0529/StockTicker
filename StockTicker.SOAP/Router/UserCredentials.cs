using System.Web;
using System.Web.Services.Protocols;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StockTicker.Lib.Common.Memberships;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Soap.Router
{
    public class UserCredentials : SoapHeader
    {
        #region fields

        private UserManager _userManager;
        private User _user;

        #endregion

        #region members


        public bool IsValid()
        {
            _user = UserManager.Find(UserName, Password);

            return _user != null;
        } 

        #endregion

        #region props

        public string UserName { get; set; }
        public string Password { get; set; }

        private UserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }

        #endregion

        #region ctors

        public UserCredentials()
        {
        }

        public UserCredentials(UserManager userManager)
        {
            UserManager = userManager;
        }

        #endregion
    }
}