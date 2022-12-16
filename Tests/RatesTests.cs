namespace Tests
{
    public class RatesTests
    {
        [Test]
        public async Task GetRatesTest()
        {
            var requestResult = await Helpers.GetApiClient().Rates.GetRatesAsync();

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
        }

        [Test]
        public async Task GetRateTest()
        {
            var requestResult = await Helpers.GetApiClient().Rates.GetRateAsync("bitcoin");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data.Id, Is.EqualTo("bitcoin"));

            requestResult = await Helpers.GetApiClient().Rates.GetRateAsync("ethereum");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data.Id, Is.EqualTo("ethereum"));
        }
    }
}
