using StockTicker.Lib.DAL.EF;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.BLL.Repository
{
    public class StockRepository : EntityRepository<Stock>
    {
        public StockRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
