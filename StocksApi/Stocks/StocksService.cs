using FluentResults;
using FluentValidation;
using StocksApi.DTOs;
using StocksApi.Errors;
using StocksApi.Persistence.Entities;
using StocksApi.Persistence.Repositories.Base;
using StocksApi.RealTime;

namespace StocksApi.Stocks;

public class StocksService
{
    private readonly StocksClient _stocksClient;
    private readonly SymbolsManager _symbolsManager;
    private readonly IStockRepository _stockRepository;
    private readonly IValidator<HistoricalDataDto> _historicalDataValidator;

    public StocksService(
        StocksClient stocksClient,
        SymbolsManager symbolsManager,
        IStockRepository stockRepository,
        IValidator<HistoricalDataDto> historicalDataValidator)
    {
        _stocksClient = stocksClient;
        _symbolsManager = symbolsManager;
        _stockRepository = stockRepository;
        _historicalDataValidator = historicalDataValidator;
    }

    public async Task<Result<StockPriceResponse>> GetCurrentPrice(string symbol)
    {
        var stockData = await _stocksClient.GetStockData(symbol.ToUpper());
        if (stockData is null || !IsValidSymbol(stockData))
        {
            return Result.Fail(StocksErrors.InvalidSymbol(symbol));
        }

        // add the symbol to the set of active symbols
        _symbolsManager.AddSymbol(symbol.ToUpper());

        // save the price to the database
        await SaveData(symbol, stockData);

        StockPriceResponse stockPrice = new(symbol.ToUpper(), stockData.CurrentPrice);
        return Result.Ok(stockPrice);
    }

    public async Task SaveData(string symbol, FinnHubData stockData)
    {
        var stockDateTime = DateTimeOffset.FromUnixTimeSeconds(stockData.UnixTimeStamp).DateTime;
        Stock stock = new()
        {
            Symbol = symbol.ToUpper(),
            Price = stockData.CurrentPrice,
            DateTime = stockDateTime
        };

        _stockRepository.Add(stock);
        await _stockRepository.SaveChangesAsync();
    }

    public async Task<Result<Dictionary<DateTime, double>>> GetHistoricalData(string symbol, HistoricalDataDto request)
    {
        var valResult = await _historicalDataValidator.ValidateAsync(request);
        
        if (!valResult.IsValid)
            return Result.Fail(StocksErrors.Validation(valResult.Errors));

        DateTime start = request.StartDate.ToDateTime(TimeOnly.MinValue);
        DateTime end = request.EndDate.ToDateTime(TimeOnly.MaxValue);

        var data = await _stockRepository.GetDataBetween(symbol.ToUpper(), start, end);
        if (data.Count == 0)
            return Result.Fail(StocksErrors.HistoricalSearch(symbol.ToUpper(), request.StartDate, request.EndDate));

        var sortedValues = data.OrderByDescending(x => x.Key).ToDictionary();
        return Result.Ok(sortedValues);
    }

    private static bool IsValidSymbol(FinnHubData stockData)
    {
        return stockData.Change != null && stockData.PercentChange != null;
    }
}