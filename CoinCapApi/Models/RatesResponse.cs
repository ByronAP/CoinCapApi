using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class RatesResponse
    {
        [JsonProperty("data")]
        public RateData[] Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
