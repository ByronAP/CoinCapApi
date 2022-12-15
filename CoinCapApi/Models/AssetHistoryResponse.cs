using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public partial class AssetHistoryResponse
    {
        [JsonProperty("data")]
        public AssetHistoryData[] Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
