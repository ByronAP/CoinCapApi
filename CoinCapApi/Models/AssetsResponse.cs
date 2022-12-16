using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class AssetsResponse
    {
        [JsonProperty("data")]
        public AssetData[] Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
