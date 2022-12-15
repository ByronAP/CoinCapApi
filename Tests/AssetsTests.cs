namespace Tests
{
    public class AssetsTests
    {
        [Test]
        public async Task GetAssetsTest()
        {
            var requestResult = await Helpers.GetApiClient().Assets.GetAssetsAsync();

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);

            requestResult = await Helpers.GetApiClient().Assets.GetAssetsAsync();

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
        }

        [Test]
        public async Task GetAssetTest()
        {
            var requestResult = await Helpers.GetApiClient().Assets.GetAssetAsync("bitcoin");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data.Id, Is.EqualTo("bitcoin"));

            requestResult = await Helpers.GetApiClient().Assets.GetAssetAsync("ethereum");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data.Id, Is.EqualTo("ethereum"));
        }

        [Test]
        public async Task GetAssetHistoryTest()
        {
            var requestResult = await Helpers.GetApiClient().Assets.GetAssetHistoryAsync("bitcoin", CoinCapApi.Types.TimeInterval.M1);

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);

            requestResult = await Helpers.GetApiClient().Assets.GetAssetHistoryAsync("ethereum", CoinCapApi.Types.TimeInterval.D1, Convert.ToUInt64(DateTimeOffset.UtcNow.AddYears(-1).AddDays(-60).ToUnixTimeMilliseconds()), Convert.ToUInt64(DateTimeOffset.UtcNow.AddYears(-1).ToUnixTimeMilliseconds()));

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
        }

        [Test]
        public async Task GetAssetMarketsTest()
        {
            var requestResult = await Helpers.GetApiClient().Assets.GetAssetMarketsAsync("bitcoin");

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);

            requestResult = await Helpers.GetApiClient().Assets.GetAssetMarketsAsync("ethereum", offset: 100);

            Assert.That(requestResult, Is.Not.Null);
            Assert.That(requestResult.Data, Is.Not.Empty);
        }
    }
}
