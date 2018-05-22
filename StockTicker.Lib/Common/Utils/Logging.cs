using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using log4net;
using log4net.Config;

namespace StockTicker.Lib.Common.Utils
{
    public static class Logging
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Setup(string path)
        {
            if (File.Exists(path))
                XmlConfigurator.Configure(new FileInfo(path));
        }

        private static void AddRequestHeaders(bool isError = false)
        {
            Logger.Info("-----------------------------------------------------------------------");


            if (HttpContext.Current == null)
                return;

            var request = HttpContext.Current.Request;
            var agent = request.UserAgent;
            var host = request.UserHostAddress;

            var log = string.Format("Agent:{0}r\n Host:{1}", agent, host);

            if (isError)
                Logger.Error(log);
            else
                Logger.Info(log);
        }

        public static void Info(string info, bool includeHttpHeaders = true, [CallerMemberName] string memberName = "",[CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (includeHttpHeaders)
                AddRequestHeaders();

            Logger.Info("Info:\t" + info);

            Logger.Info("Member name:\t" + memberName);
            Logger.Info("Source file path:\t" + sourceFilePath);
            Logger.Info("Source line number:\t" + sourceLineNumber);
        }

        public static void Error(string error, bool includeHttpHeaders = true, [CallerMemberName] string memberName = "",[CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (includeHttpHeaders)
                AddRequestHeaders(true);

            Logger.Info("Error:\t" + error);

            Logger.Info("Member name:\t" + memberName);
            Logger.Info("Source file path:\t" + sourceFilePath);
            Logger.Info("Source line number:\t" + sourceLineNumber);
        }
    }
}