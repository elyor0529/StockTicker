using System;
using System.Security.Principal;
using System.Web;

namespace StockTicker.Soap.Router
{
    public class WebServiceAuthenticationEvent : EventArgs
    {

        public WebServiceAuthenticationEvent()
        {
            //TODO:
        }

        public WebServiceAuthenticationEvent(HttpContext context)
        {
            Context = context;
        }

        public WebServiceAuthenticationEvent(HttpContext context, string user, string password)
        {
            Context = context;
            User = user;
            Password = password;
        }

        public HttpContext Context { get; }

        public IPrincipal Principal { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public bool HasCredentials
        {
            get
            {
                return User != null && Password != null;
            }
        }

        public void Authenticate()
        {
            var i = new GenericIdentity(User);

            Principal = new GenericPrincipal(i, new string[0]);
        } 

    }
}