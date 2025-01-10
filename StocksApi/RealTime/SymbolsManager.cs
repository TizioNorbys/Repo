
namespace StocksApi.RealTime;

public class SymbolsManager
{
	private readonly HashSet<string> activeSymbols = new() { "MSFT", "AAPL", "NVDA" };
	
	public void AddSymbol(string symbol)
	{
        activeSymbols.Add(symbol);
    }

	public IReadOnlyCollection<string> GetAllSymbols()
	{
		return activeSymbols.ToList();
	}
}