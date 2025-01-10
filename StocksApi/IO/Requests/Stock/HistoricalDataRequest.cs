
namespace StocksApi.IO.Requests.Stock;

public class HistoricalDataRequest
{
    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }
}