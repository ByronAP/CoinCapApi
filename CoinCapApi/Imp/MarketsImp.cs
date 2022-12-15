// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-15-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-15-2022
// ***********************************************************************
// <copyright file="MarketsImp.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using CoinCapApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace CoinCapApi.Imp
{
    /// <summary>
    /// <para>Implementation of the '/markets' API calls.</para>
    /// <para>Implementation classes do not have a public constructor
    /// and must be accessed through an instance of <see cref="CoinCapClient"/>.</para>
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#d4bac290-230a-48c6-a8eb-6804b2d137f3"/>
    public class MarketsImp
    {
        private readonly RestClient _restClient;
        private readonly ILogger<CoinCapClient> _logger;
        private readonly MemCache _cache;

        internal MarketsImp(RestClient restClient, MemCache cache, ILogger<CoinCapClient> logger)
        {
            _logger = logger;
            _cache = cache;
            _restClient = restClient;
        }

        /// <summary>
        /// Get markets as an asynchronous operation.
        /// </summary>
        /// <param name="exchangeId">Filters by exchange id.</param>
        /// <param name="baseSymbol">Filters by all markets containing the base symbol.</param>
        /// <param name="quoteSymbol">Filters by all markets containing the quote symbol.</param>
        /// <param name="baseId">Filters by all markets containing the base id.</param>
        /// <param name="quoteId">Filters by all markets containing the quote id.</param>
        /// <param name="assetSymbol">Filters by all markets containing symbol (base and quote).</param>
        /// <param name="assetId">Filters by all markets containing id (base and quote).</param>
        /// <param name="limit">The number of items to retrieve. Default: 100 Max: 2000.</param>
        /// <param name="offset">The number of items to skip (aka offset).</param>
        /// <returns>A Task&lt;<see cref="MarketsResponse"/>&gt; representing the asynchronous operation.</returns>
        public async Task<MarketsResponse> GetMarketsAsync(string exchangeId = null, string baseSymbol = null, string quoteSymbol = null, string baseId = null, string quoteId = null, string assetSymbol = null, string assetId = null, uint limit = 100, uint offset = 0)
        {
            if (limit > 2000) { limit = 2000; }

            var request = new RestRequest(CoinCapClient.BuildUrl("markets"));
            if (!string.IsNullOrEmpty(exchangeId) && !string.IsNullOrWhiteSpace(exchangeId)) { request.AddQueryParameter("exchangeId", exchangeId); }
            if (!string.IsNullOrEmpty(baseSymbol) && !string.IsNullOrWhiteSpace(baseSymbol)) { request.AddQueryParameter("baseSymbol", baseSymbol); }
            if (!string.IsNullOrEmpty(quoteSymbol) && !string.IsNullOrWhiteSpace(quoteSymbol)) { request.AddQueryParameter("quoteSymbol", quoteSymbol); }
            if (!string.IsNullOrEmpty(baseId) && !string.IsNullOrWhiteSpace(baseId)) { request.AddQueryParameter("baseId", baseId); }
            if (!string.IsNullOrEmpty(quoteId) && !string.IsNullOrWhiteSpace(quoteId)) { request.AddQueryParameter("quoteId", quoteId); }
            if (!string.IsNullOrEmpty(assetSymbol) && !string.IsNullOrWhiteSpace(assetSymbol)) { request.AddQueryParameter("assetSymbol", assetSymbol); }
            if (!string.IsNullOrEmpty(assetId) && !string.IsNullOrWhiteSpace(assetId)) { request.AddQueryParameter("assetId", assetId); }
            request.AddQueryParameter("limit", limit);
            request.AddQueryParameter("offset", offset);

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 60);

            return JsonConvert.DeserializeObject<MarketsResponse>(jsonStr);
        }
    }
}
