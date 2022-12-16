using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class MarketData
    {
        /// <summary>
        /// The unique identifier for the exchange.
        /// </summary>
        [JsonProperty("exchangeId")]
        public string ExchangeId { get; set; }

        /// <summary>
        /// The rank is in ascending order - this number represents the amount of volume transacted by this market in relation to other markets on that exchange.
        /// </summary>
        [JsonProperty("rank")]
        public int? Rank { get; set; }

        /// <summary>
        /// The most common symbol used to identify the base asset, base is asset purchased.
        /// </summary>
        [JsonProperty("baseSymbol")]
        public string BaseSymbol { get; set; }

        /// <summary>
        /// The unique identifier for the base asset, base is asset purchased.
        /// </summary>
        [JsonProperty("baseId")]
        public string BaseId { get; set; }

        /// <summary>
        /// The most common symbol used to identify the quote asset, quote is asset used to purchase base.
        /// </summary>
        [JsonProperty("quoteSymbol")]
        public string QuoteSymbol { get; set; }

        /// <summary>
        /// The unique identifier for the quote asset, quote is asset used to purchase base.
        /// </summary>
        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        /// <summary>
        /// The amount of quote asset traded for one unit of base asset.
        /// </summary>
        [JsonProperty("priceQuote")]
        public decimal? PriceQuote { get; set; }

        /// <summary>
        /// The quote price translated to USD.
        /// </summary>
        [JsonProperty("priceUsd")]
        public decimal? PriceUsd { get; set; }

        /// <summary>
        /// The volume transacted on this market in last 24 hours.
        /// </summary>
        [JsonProperty("volumeUsd24Hr")]
        public double? VolumeUsd24Hr { get; set; }

        /// <summary>
        /// The amount of daily volume a single market transacts in relation to total daily volume of all markets on the exchange.
        /// </summary>
        [JsonProperty("percentExchangeVolume")]
        public decimal? PercentExchangeVolume { get; set; }

        /// <summary>
        /// The number of trades on this market in the last 24 hours.
        /// </summary>
        [JsonProperty("tradesCount24Hr")]
        public int? TradesCount24Hr { get; set; }

        /// <summary>
        /// UNIX timestamp (milliseconds) since information was received from this particular market.
        /// </summary>
        [JsonProperty("updated")]
        public long? Updated { get; set; }
    }
}
