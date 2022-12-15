using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class ExchangeData
    {
        /// <summary>
        /// The unique identifier for the exchange.
        /// </summary>
        [JsonProperty("exchangeId")]
        public string ExchangeId { get; set; }

        /// <summary>
        /// The proper name of the exchange.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The rank is in ascending order - this number is directly associated with the total exchange volume whereas the highest volume exchange receives rank 1.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// The amount of daily volume a single exchange transacts in relation to total daily volume of all exchanges.
        /// </summary>
        [JsonProperty("percentTotalVolume")]
        public decimal? PercentTotalVolume { get; set; }

        /// <summary>
        /// The exchanges daily volume represented in USD.
        /// </summary>
        [JsonProperty("volumeUsd")]
        public double? VolumeUsd { get; set; }

        /// <summary>
        /// The number of trading pairs (or markets) offered by exchange.
        /// </summary>
        [JsonProperty("tradingPairs")]
        public int TradingPairs { get; set; }

        /// <summary>
        /// Is trade data for this exchange available via websocket.
        /// <para>true/false, true = trade socket available, false = trade socket unavailable</para>
        /// </summary>
        [JsonProperty("socket")]
        public bool? Socket { get; set; }

        /// <summary>
        /// The web address of the exchange.
        /// </summary>
        [JsonProperty("exchangeUrl")]
        public string ExchangeUrl { get; set; }

        /// <summary>
        /// UNIX timestamp (milliseconds) since information was received from this exchange.
        /// </summary>
        [JsonProperty("updated")]
        public long Updated { get; set; }
    }
}
