using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class CandlesResponse
    {
        [JsonProperty("data")]
        public CandleData[] Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
