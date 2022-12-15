using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class MarketsResponse
    {
        [JsonProperty("data")]
        public MarketData[] Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
