namespace Tests
{
    public class MarketsTests
    {
        [Test]
        public async Task GetMarketsTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync();

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
        }

        [Test]
        public async Task GetMarketsByExchangeTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(exchangeId: "gdax");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
            Assert.That(requestResult.Data.First().ExchangeId.ToLowerInvariant(), Is.EqualTo("gdax"));
        }

        [Test]
        public async Task GetMarketsByBaseSymbolTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(baseSymbol: "eth");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
            Assert.That(requestResult.Data.First().BaseSymbol.ToLowerInvariant(), Is.EqualTo("eth"));
        }

        [Test]
        public async Task GetMarketsByQuoteSymbolTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(quoteSymbol: "eth");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
            Assert.That(requestResult.Data.First().QuoteSymbol.ToLowerInvariant(), Is.EqualTo("eth"));
        }

        [Test]
        public async Task GetMarketsByBaseIdTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(baseId: "ethereum");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
            Assert.That(requestResult.Data.First().BaseId.ToLowerInvariant(), Is.EqualTo("ethereum"));
        }


        [Test]
        public async Task GetMarketsByQuoteIdTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(quoteId: "ethereum");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
            Assert.That(requestResult.Data.First().QuoteId.ToLowerInvariant(), Is.EqualTo("ethereum"));
        }

        [Test]
        public async Task GetMarketsByAssetSymbolTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(assetSymbol: "eth");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);

            if (requestResult.Data.First().QuoteSymbol.Equals("eth", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.Pass();
            }

            if (requestResult.Data.First().BaseSymbol.Equals("eth", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public async Task GetMarketsByAssetIdTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(assetId: "ethereum");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);

            if (requestResult.Data.First().QuoteId.Equals("ethereum", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.Pass();
            }

            if (requestResult.Data.First().BaseId.Equals("ethereum", StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public async Task GetMarketsLimitTest()
        {
            var requestResult = await Helpers.GetApiClient().Markets.GetMarketsAsync(limit: 2);

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
            Assert.That(requestResult.Data.Length, Is.EqualTo(2));
        }
    }
}
