using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class RateData
    {
        /// <summary>
        /// The unique identifier for asset or fiat.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The most common symbol used to identify asset or fiat.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// The currency symbol used to identify asset or fiat.
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// The type of currency (fiat or crypto).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The rate conversion to USD.
        /// </summary>
        [JsonProperty("rateUsd")]
        public decimal? RateUsd { get; set; }
    }
}
