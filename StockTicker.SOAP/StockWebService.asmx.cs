using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StockTicker.Lib.Common.Memberships;
using StockTicker.Lib.Common.Utils;
using StockTicker.Soap.Models;
using StockTicker.Soap.Router;

namespace StockTicker.Soap
{
    /// <summary>
    ///     Summary description for StockWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class StockWebService : WebService
    {
        #region props

        private UserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }

        #endregion

        #region fields

        public UserCredentials Credentials;
        private UserManager _userManager;

        #endregion

        #region ctors

        public StockWebService()
        {
        }

        public StockWebService(UserManager userManager)
        {
            UserManager = userManager;
        }

        #endregion

        #region members 

        [WebMethod(Description = "Returns all the stock symbols")]
        [SoapHeader("Credentials", Required = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml, XmlSerializeString = true)]
        public ResultModel<StockModel[]> GetList()
        {
            if (Credentials == null)
                return ResultModel<StockModel[]>.ThrowIfError("Authorization is required");

            if (!Credentials.IsValid())
                return ResultModel<StockModel[]>.ThrowIfError("Invalid Username or Password");

            var user = UserManager.Find(Credentials.UserName,Credentials.Password);
            var model = user.Stocks.Select(s => new StockModel(s.Symbol, s.Name, s.Price)).ToArray();

            return ResultModel<StockModel[]>.CreateInstance(model);
        }

        [WebMethod(Description = "Returns all the stock symbols")]
        [SoapHeader("Credentials", Required = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml, XmlSerializeString = true)]
        public ResultModel<StockModel[]> UpdatePrices()
        {
            if (Credentials == null)
                return ResultModel<StockModel[]>.ThrowIfError("Authorization is required");

            if (!Credentials.IsValid())
                return ResultModel<StockModel[]>.ThrowIfError("Invalid Username or Password");

            var user = UserManager.Find(Credentials.UserName, Credentials.Password);
            var stocks = user.Stocks.Select(s => new StockModel(s.Symbol, s.Name, s.Price));

            foreach (var stock in stocks)
            {
                stock.Price = RandomHelper.GetNumber();
            }

            UserManager.Update(user);

            var model = user.Stocks.Select(s => new StockModel(s.Symbol, s.Name, s.Price)).ToArray();
            return ResultModel<StockModel[]>.CreateInstance(model);
        }

        #endregion
    }
}