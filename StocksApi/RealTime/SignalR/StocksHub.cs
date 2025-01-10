using Microsoft.AspNetCore.SignalR;

namespace StocksApi.RealTime.SignalR;

public class StocksHub : Hub<IStocksClient>
{
    public override async Task OnConnectedAsync()
    {
        List<string> defaultSymbols = new() { "MSFT", "AAPL", "NVDA" };
        foreach (string symbol in defaultSymbols)
        {
            await AddToGroupAsync(symbol);
        }
    }

    public async Task AddToGroupAsync(string symbol)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, symbol.ToUpper());
    }
}