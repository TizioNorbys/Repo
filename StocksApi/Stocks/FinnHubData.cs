using System.Text.Json.Serialization;

namespace StocksApi.Stocks
{
	public class FinnHubData
	{
        [JsonPropertyName("c")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("d")]
        public decimal? Change { get; set; }

        [JsonPropertyName("dp")]
        public decimal? PercentChange { get; set; }

        [JsonPropertyName("h")]
        public decimal High { get; set; }
        
        [JsonPropertyName("l")]
        public decimal Low { get; set; }

        [JsonPropertyName("o")]
        public decimal Open { get; set; }

        [JsonPropertyName("pc")]
        public decimal PreviousClose { get; set; }

        [JsonPropertyName("t")]
        public long UnixTimeStamp { get; set; }
    }
}

