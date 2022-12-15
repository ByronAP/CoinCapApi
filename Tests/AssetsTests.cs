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
    }
}
