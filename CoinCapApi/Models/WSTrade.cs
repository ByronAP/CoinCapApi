using Newtonsoft.Json;
using System;

namespace CoinCapApi.Models
{
    public class WSTrade
    {
        public Guid WSInstanceId { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("quote")]
        public string Quote { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("priceUsd")]
        public decimal PriceUsd { get; set; }
    }
}
