using CoinCapApi.Properties;

namespace CoinCapApi
{
    public static class Constants
    {
        /// <summary>
        /// The display name of the API provider.
        /// </summary>
        public static readonly string API_NAME = "CoinCap";

        /// <summary>
        /// The base API URL.
        /// </summary>
        public static string API_BASE_URL = "https://api.coincap.io";

        /// <summary>
        /// The API version.
        /// </summary>
        public static readonly uint API_VERSION = 2;

        /// <summary>
        /// The API logo at 128 X 128 in PNG format.
        /// This is an embedded resource.
        /// </summary>
        public static readonly byte[] API_LOGO_128X128_PNG = Resources.coincap_logo;

        /// <summary>
        /// The API minimum cache time in milliseconds.
        /// <para>If you need to call the same endpoint more that this you are doing something wrong and should probably use websockets or a different service.</para>
        /// </summary>
        public static readonly uint API_MIN_CACHE_TIME_MS = 15000;
    }
}
