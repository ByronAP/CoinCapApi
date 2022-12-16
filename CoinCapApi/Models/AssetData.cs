using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class AssetData
    {
        /// <summary>
        /// The unique identifier for this asset.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The rank is in ascending order - this number is directly associated with the marketcap whereas the highest marketcap receives rank 1.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// The most common symbol used to identify this asset on an exchange.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// The proper name for this asset.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The available supply for trading.
        /// </summary>
        [JsonProperty("supply")]
        public double? Supply { get; set; }

        /// <summary>
        /// The total quantity of asset issued.
        /// </summary>
        [JsonProperty("maxSupply")]
        public double? MaxSupply { get; set; }

        /// <summary>
        /// The total value of the asset in circulation (marketcap = supply x price) in USD.
        /// </summary>
        [JsonProperty("marketCapUsd")]
        public double? MarketCapUsd { get; set; }

        /// <summary>
        /// The quantity of trading volume represented in USD over the last 24 hours.
        /// </summary>
        [JsonProperty("volumeUsd24Hr")]
        public double? VolumeUsd24Hr { get; set; }

        /// <summary>
        /// The volume-weighted price based on real-time market data, translated to USD.
        /// </summary>
        [JsonProperty("priceUsd")]
        public decimal? PriceUsd { get; set; }

        /// <summary>
        /// The direction and value change in the last 24 hours.
        /// </summary>
        [JsonProperty("changePercent24Hr")]
        public decimal? ChangePercent24Hr { get; set; }

        /// <summary>
        /// The Volume Weighted Average Price in the last 24 hours.
        /// </summary>
        [JsonProperty("vwap24Hr")]
        public decimal? Vwap24Hr { get; set; }

        /// <summary>
        /// Address of the assets blockchain explorer.
        /// </summary>
        [JsonProperty("explorer")]
        public string Explorer { get; set; }
    }
}
