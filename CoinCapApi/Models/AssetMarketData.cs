using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class AssetMarketData
    {
        /// <summary>
        /// The unique identifier for exchange.
        /// </summary>
        [JsonProperty("exchangeId")]
        public string ExchangeId { get; set; }

        /// <summary>
        /// The unique identifier for the base asset, base is asset purchased.
        /// </summary>
        [JsonProperty("baseId")]
        public string BaseId { get; set; }

        /// <summary>
        /// The unique identifier for the quote asset, quote is asset used to purchase base.
        /// </summary>
        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        /// <summary>
        /// The most common symbol used to identify the base asset, base is asset purchased.
        /// </summary>
        [JsonProperty("baseSymbol")]
        public string BaseSymbol { get; set; }

        /// <summary>
        /// The most common symbol used to identify the quote asset, quote is asset used to purchase base.
        /// </summary>
        [JsonProperty("quoteSymbol")]
        public string QuoteSymbol { get; set; }

        /// <summary>
        /// The volume transacted on this market in last 24 hours.
        /// </summary>
        [JsonProperty("volumeUsd24Hr")]
        public double? VolumeUsd24Hr { get; set; }

        /// <summary>
        /// The amount of quote asset traded for one unit of base asset.
        /// </summary>
        [JsonProperty("priceUsd")]
        public decimal? PriceUsd { get; set; }

        /// <summary>
        /// The percent of quote asset volume.
        /// </summary>
        [JsonProperty("volumePercent")]
        public decimal? VolumePercent { get; set; }
    }
}
