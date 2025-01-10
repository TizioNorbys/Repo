using Microsoft.EntityFrameworkCore;
using StocksApi.Persistence.Entities;
using StocksApi.Persistence.Repositories.Base;

namespace StocksApi.Persistence.Repositories;

public class StockRepository : IStockRepository
{
	private readonly AppDbContext _dbContext;

	public StockRepository(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void Add(Stock stock)
	{
		_dbContext.Stocks.Add(stock);
	}

	public async Task<Dictionary<DateTime, double>> GetDataBetween(string symbol, DateTime startDate, DateTime endDate)
	{
		return await (from stock in _dbContext.Stocks
				where stock.Symbol == symbol
				let date = stock.DateTime
				where date >= startDate && date <= endDate
				select new StockDetails(stock.DateTime, stock.Price)
				).ToDictionaryAsync(x => x.DateTime, x => x.Price);
	}

	public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}

public record StockDetails(DateTime DateTime, double Price);