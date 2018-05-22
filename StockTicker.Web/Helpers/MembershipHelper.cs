using System.Web;
using StockTicker.Lib.Common.Memberships;
using StockTicker.Web.AccountServiceReference;

namespace StockTicker.Web.Helpers
{
    public static class MembershipHelper
    {
        private const string USER_KEY = "_USER_";
         
        public static UserModel User
        {
            get { return (UserModel)HttpContext.Current.Session[USER_KEY]; }
        }

        public static void SignIn(UserModel dataModel)
        {
            HttpContext.Current.Session[USER_KEY] = dataModel;
        }

        public static void SignOut()
        {
            HttpContext.Current.Session[USER_KEY] = null;
        }

        public static bool IsAuthorize()
        {
            return HttpContext.Current.Session[USER_KEY] != null;
        }

    }
}