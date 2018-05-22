using System.ComponentModel;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StockTicker.Lib.Common.Extensions;
using StockTicker.Lib.Common.Memberships;
using StockTicker.Lib.DAL.Models;
using StockTicker.Soap.Models;

namespace StockTicker.Soap
{
    /// <summary>
    ///     Summary description for StockWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class AccountWebService : WebService
    {
        #region fields

        private UserManager _userManager;
        private RoleManager _roleManager;

        #endregion

        #region props 

        private UserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }

        private RoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<RoleManager>(); }
            set { _roleManager = value; }
        }

        #endregion

        #region ctors

        public AccountWebService()
        {
        }

        public AccountWebService(UserManager userManager, RoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        #endregion

        #region members 

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml, XmlSerializeString = true)]
        public ResultModel<UserModel> Login(string user, string password)
        {
            var userEnt = UserManager.Find(user, password);

            if (userEnt == null)
                return ResultModel<UserModel>.ThrowIfError("User name or password is wrond");

            var roles = UserManager.GetRoles(userEnt.Id) ?? new string[] { };
            var roleEnt = RoleManager.FindByName(roles[0]);
            var model = new UserModel
            {
                UserName = user,
                Password = password,
                FullName = userEnt.FullName,
                Role = roleEnt.Code
            };

            return ResultModel<UserModel>.CreateInstance(model);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml, XmlSerializeString = true)]
        public ResultModel<UserModel> Register(string fullName, string email, string password)
        {
            var userEnt = UserManager.FindByEmail(email);

            if (userEnt != null)
                return ResultModel<UserModel>.ThrowIfError("This " + email + " user exist");

            var role = MemberRole.Client;

            //entity
            var user = new User
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            //user
            var result = UserManager.Create(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(";", result.Errors);

                return ResultModel<UserModel>.ThrowIfError(errors);
            }

            //role
            result = UserManager.AddToRole(user.Id, role.GetName());
            if (!result.Succeeded)
            {
                var errors = string.Join(";", result.Errors);

                return ResultModel<UserModel>.ThrowIfError(errors);
            }

            return ResultModel<UserModel>.CreateInstance(new UserModel
            {
                FullName = fullName,
                Password = password,
                UserName = email,
                Role =role
            });
        }

        #endregion
    }
}