using System;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;
using StockTicker.Lib.Common.Utils;

namespace StockTicker.Soap.Router
{

    public sealed class WebServiceAuthenticationModule : IHttpModule
    {

        public WebServiceAuthenticationModule()
        {
            //TODO:
        }

        public string ModuleName
        {
            get { return "WebServiceAuthentication"; }
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.AuthenticateRequest += OnEnter;
        }

        private static void OnAuthenticate(WebServiceAuthenticationEvent e)
        {
            if (e.User == null)
                return;

            if (e.HasCredentials)
                e.Authenticate();

            e.Context.User = e.Principal;
        }

        private static void OnEnter(object source, EventArgs eventArgs)
        {
            var app = (HttpApplication)source;
            var context = app.Context;
            var httpStream = context.Request.InputStream;

            // Save the current position of stream.
            var posStream = httpStream.Position;

            // If the request contains an HTTP_SOAPACTION 
            // header, look at this message.
            if (context.Request.ServerVariables["HTTP_SOAPACTION"] == null)
                return;

            // Load the body of the HTTP message
            // into an XML document.
            var dom = new XmlDocument();
            var soapUser = String.Empty;
            var soapPassword = String.Empty;

            try
            {
                dom.Load(httpStream);

                // Reset the stream position.
                httpStream.Position = posStream;

                // Bind to the Authentication header.
                soapUser = dom.GetElementsByTagName("User")?.Item(0)?.InnerText;
                soapPassword = dom.GetElementsByTagName("Password")?.Item(0)?.InnerText;
            }
            catch (Exception e)
            {
                // Reset the position of stream.
                httpStream.Position = posStream;

                // Throw a SOAP exception.
                var name = new XmlQualifiedName("Load");
                var soapException = new SoapException("Unable to read SOAP request", name, e);

                Logging.Error(soapException.ToString());

                throw soapException;
            }

            // Raise the custom global.asax event.
            OnAuthenticate(new WebServiceAuthenticationEvent(context, soapUser, soapPassword));
        }
    }
}