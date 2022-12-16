using System.Diagnostics;

namespace Tests
{
    public class WSTests
    {
        [Test]
        public async Task TradesWSTest()
        {
            var factory = LoggerFactory.Create(x =>
            {
                x.AddConsole();
                x.SetMinimumLevel(LogLevel.Debug);
            });
            var logger = factory.CreateLogger<CoinCapTradesWSClient>();
            using var wsClient = new CoinCapTradesWSClient("binance", logger);

            bool tradeSeen = false;

            wsClient.OnTradeEvent += (sender, instanceId, exchangeId, trade) => { tradeSeen = true; };

            var stopWatch = new Stopwatch();

            await wsClient.Connect();

            stopWatch.Start();

            do
            {
                await Task.Delay(50);
            } while (!tradeSeen && stopWatch.Elapsed.TotalSeconds < 20);

            stopWatch.Stop();

            await wsClient.Disconnect();

            Assert.That(tradeSeen, Is.True);
        }

        [Test]
        public async Task PricesWSTest()
        {
            var factory = LoggerFactory.Create(x =>
            {
                x.AddConsole();
                x.SetMinimumLevel(LogLevel.Debug);
            });
            var logger = factory.CreateLogger<CoinCapPricesWSClient>();
            using var wsClient = new CoinCapPricesWSClient(new[] { "ALL" }, logger);

            bool priceSeen = false;

            wsClient.OnPricesEvent += (sender, instanceId, prices) => { priceSeen = true; };

            var stopWatch = new Stopwatch();

            await wsClient.Connect();

            stopWatch.Start();

            do
            {
                await Task.Delay(50);
            } while (!priceSeen && stopWatch.Elapsed.TotalSeconds < 20);

            stopWatch.Stop();

            await wsClient.Disconnect();

            Assert.That(priceSeen, Is.True);
        }
    }
}
