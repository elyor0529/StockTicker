using System;

namespace StockTicker.Lib.DAL.EF
{
    public interface IUnitOfWork : IDisposable
    {

        IDbContext DbContext { get; }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();
    }
}