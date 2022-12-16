namespace Tests
{
    internal static class Helpers
    {
        private static CoinCapClient? _apiClient = null;

        internal static CoinCapClient GetApiClient()
        {
            if (_apiClient == null)
            {
                var factory = LoggerFactory.Create(x =>
                {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Debug);
                });
                var logger = factory.CreateLogger<CoinCapClient>();

                _apiClient = new CoinCapClient(logger: logger);
            }

            return _apiClient;
        }
    }
}
