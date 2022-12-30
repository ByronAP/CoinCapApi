using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoinCapApi
{
    public static class CoinCapServiceCollectionExtensions
    {
        public static IServiceCollection AddCoinCapApi(this IServiceCollection services)
           => services.AddSingleton<CoinCapClient>();

        public static IServiceCollection AddCoinCapApi(this IServiceCollection services, string apiKey)
            => services.AddSingleton<CoinCapClient>(new CoinCapClient(apiKey: apiKey));

        public static IServiceCollection AddCoinCapApi(this IServiceCollection services, ILogger<CoinCapClient> logger)
            => services.AddSingleton<CoinCapClient>(new CoinCapClient(logger: logger));

        public static IServiceCollection AddCoinCapApi(this IServiceCollection services, string apiKey, ILogger<CoinCapClient> logger)
            => services.AddSingleton<CoinCapClient>(new CoinCapClient(apiKey: apiKey, logger: logger));

        // don't add websocket classes since they ideally should be custom setup with event handlers
    }
}
