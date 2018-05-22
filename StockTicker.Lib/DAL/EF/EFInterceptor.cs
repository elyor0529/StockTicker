using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using StockTicker.Lib.Common.Utils;

namespace StockTicker.Lib.DAL.EF
{
    public class EFInterceptor : IDbCommandInterceptor
    {
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            WriteLog(string.Format(" IsAsync: {0}\r\n Command Text: {1}", interceptionContext.IsAsync, command.CommandText));
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            WriteLog(string.Format(" IsAsync: {0}\r\n Command Text: {1}", interceptionContext.IsAsync, command.CommandText));
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            WriteLog(string.Format(" IsAsync: {0} \r\n Command Text: {1}", interceptionContext.IsAsync, command.CommandText));
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            WriteLog(string.Format(" IsAsync: {0}\r\n Command Text: {1}", interceptionContext.IsAsync, command.CommandText));
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            WriteLog(string.Format(" IsAsync: {0}\r\n Command Text: {1}", interceptionContext.IsAsync, command.CommandText));
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            WriteLog(string.Format(" IsAsync: {0}\r\n Command Text: {1}", interceptionContext.IsAsync, command.CommandText));
        }

        private static void WriteLog(string command)
        {
            Logging.Info(command);
        }
    }
}
