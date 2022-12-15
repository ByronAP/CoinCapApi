using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class ExchangeResponse
    {
        [JsonProperty("data")]
        public ExchangeData Data { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
