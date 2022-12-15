using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class RateResponse
    {
        [JsonProperty("data")]
        public RateData Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
