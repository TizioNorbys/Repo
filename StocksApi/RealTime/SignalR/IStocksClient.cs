namespace StocksApi.RealTime.SignalR;

public interface IStocksClient
{
    Task ReceiveUpdatedPrice(StockPriceUpdate updatedPrice);
}