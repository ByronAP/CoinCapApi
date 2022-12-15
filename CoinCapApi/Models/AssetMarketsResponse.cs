using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class AssetMarketsResponse
    {
        [JsonProperty("data")]
        public AssetMarketData[] Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
