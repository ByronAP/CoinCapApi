namespace CoinCapApi.Types
{
    public enum TimeInterval
    {
        /// <summary>
        /// 1 minute
        /// </summary>
        M1 = 1,
        /// <summary>
        /// 5 minutes
        /// </summary>
        M5 = 2,
        /// <summary>
        /// 15 minutes
        /// </summary>
        M15 = 4,
        /// <summary>
        /// 30 minutes
        /// </summary>
        M30 = 8,
        /// <summary>
        /// 1 hour
        /// </summary>
        H1 = 16,
        /// <summary>
        /// 2 hours
        /// </summary>
        H2 = 32,
        /// <summary>
        /// 6 hours
        /// </summary>
        H6 = 64,
        /// <summary>
        /// 12 hours
        /// </summary>
        H12 = 128,
        /// <summary>
        /// 1 day (24 hours)
        /// </summary>
        D1 = 256
    }
}
