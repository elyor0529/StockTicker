using System.Linq;
using StockTicker.Lib.BLL.Repository;
using StockTicker.Lib.DAL.EF;
using StockTicker.Lib.DAL.Models;

namespace StockTicker.Lib.BLL.Service
{
    public class StockService : EntityService<Stock>
    {
        private readonly IEntityRepository<Stock> _repository;

        public StockService(IUnitOfWork unitOfWork, IEntityRepository<Stock> repository) : base(unitOfWork, repository)
        {
            _repository = repository;
        }

        public Stock FindBySymbol(string symbol)
        {
            return _repository.FindBy(a => a.Symbol == symbol).FirstOrDefault();
        } 

    }
}
