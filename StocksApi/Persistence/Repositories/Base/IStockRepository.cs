using StocksApi.Persistence.Entities;

namespace StocksApi.Persistence.Repositories.Base;

public interface IStockRepository
{
    void Add(Stock stock);

    Task<Dictionary<DateTime, double>> GetDataBetween(string symbol, DateTime startDate, DateTime endDate);

    Task SaveChangesAsync();
}