namespace Tests
{
    internal class DITests
    {
        [Test]
        public void CreateViaDITest()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddCoinCapApi();

            using var provider = services.BuildServiceProvider();

            var api = provider.GetService<CoinCapClient>();

            Assert.That(api, Is.Not.Null);
        }

        [Test]
        public void CreateViaDIWithApiKeyTest()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddCoinCapApi("FakeApiKey");

            using var provider = services.BuildServiceProvider();

            var api = provider.GetService<CoinCapClient>();

            Assert.That(api, Is.Not.Null);
        }
    }
}
