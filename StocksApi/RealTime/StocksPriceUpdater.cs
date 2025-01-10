using Microsoft.AspNetCore.SignalR;
using StocksApi.RealTime.SignalR;
using StocksApi.Stocks;

namespace StocksApi.RealTime;

public class StocksPriceUpdater : BackgroundService
{
    private readonly SymbolsManager _symbolsManager;
    private readonly IHubContext<StocksHub, IStocksClient> _hubContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StocksPriceUpdater(
        SymbolsManager symbolsManager,
        IHubContext<StocksHub,IStocksClient> hubContext,
        IServiceScopeFactory serviceScopeFactory)
    {
        _symbolsManager = symbolsManager;
        _hubContext = hubContext;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdatePrices();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task UpdatePrices()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        StocksService stocksService = scope.ServiceProvider.GetRequiredService<StocksService>();

        foreach (string symbol in _symbolsManager.GetAllSymbols())
        {
            var result = await stocksService.GetCurrentPrice(symbol);
            var currentPrice = result.Value;

            StockPriceUpdate updatedPrice = new(symbol, currentPrice.Price);
            await _hubContext.Clients.Group(symbol).ReceiveUpdatedPrice(updatedPrice);
        }
    }
}