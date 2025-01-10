using System.Text.Json;

namespace StocksApi.Stocks
{
	public class StocksClient
	{
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public StocksClient(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        public async Task<FinnHubData?> GetStockData(string symbol)
        {
            var json = await _client.GetStringAsync($"/quote?symbol={symbol}&token={_configuration["Stocks:ApiKey"]}");
            var stockData = JsonSerializer.Deserialize<FinnHubData>(json);

            return stockData;
        }
    }
}

