namespace Tests
{
    public class ExchangesTests
    {
        [Test]
        public async Task GetExchangesTest()
        {
            var requestResult = await Helpers.GetApiClient().Exchanges.GetExchangesAsync();

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
        }

        [Test]
        public async Task GetExchangeTest()
        {
            var requestResult = await Helpers.GetApiClient().Exchanges.GetExchangeAsync("binance");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data.ExchangeId, Is.EqualTo("binance"));

            requestResult = await Helpers.GetApiClient().Exchanges.GetExchangeAsync("kraken");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data.ExchangeId, Is.EqualTo("kraken"));
        }
    }
}
