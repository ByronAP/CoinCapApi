using Newtonsoft.Json;

namespace CoinCapApi.Models
{
    public class CandleData
    {
        /// <summary>
        /// The price (quote) at which the first transaction was completed in a given time period.
        /// </summary>
        [JsonProperty("open")]
        public decimal Open { get; set; }

        /// <summary>
        /// The highest price (quote) at which the base was traded during the time period.
        /// </summary>
        [JsonProperty("high")]
        public decimal High { get; set; }

        /// <summary>
        /// The lowest price (quote) at which the base was traded during the time period.
        /// </summary>
        [JsonProperty("low")]
        public decimal Low { get; set; }

        /// <summary>
        /// The price (quote) at which the last transaction was completed in a given time period.
        /// </summary>
        [JsonProperty("close")]
        public decimal Close { get; set; }

        /// <summary>
        /// The amount of base asset traded in the given time period.
        /// </summary>
        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// The timestamp for starting of the time period (bucket), represented in UNIX milliseconds.
        /// </summary>
        [JsonProperty("period")]
        public long Period { get; set; }
    }
}
