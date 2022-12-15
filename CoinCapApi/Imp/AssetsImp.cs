// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-14-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-15-2022
// ***********************************************************************
// <copyright file="AssetsImp.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using CoinCapApi.Models;
using CoinCapApi.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinCapApi.Imp
{
    /// <summary>
    /// <para>Implementation of the '/assets' API calls.</para>
    /// <para>Implementation classes do not have a public constructor
    /// and must be accessed through an instance of <see cref="CoinCapClient"/>.</para>
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#ee0c0be6-513f-4466-bbb0-2016add462e9"/>
    public class AssetsImp
    {
        private readonly RestClient _restClient;
        private readonly ILogger<CoinCapClient> _logger;
        private readonly MemCache _cache;

        internal AssetsImp(RestClient restClient, MemCache cache, ILogger<CoinCapClient> logger)
        {
            _logger = logger;
            _cache = cache;
            _restClient = restClient;
        }

        /// <summary>Get assets as an asynchronous operation.</summary>
        /// <param name="search">Search by asset id (bitcoin) or symbol (BTC).</param>
        /// <param name="ids">Query with multiple ids (bitcoin,ethereum,monero).</param>
        /// <param name="limit">The number of items to retrieve. Default: 100 Max: 2000.</param>
        /// <param name="offset">The number of items to skip (aka offset).</param>
        /// <seealso href="https://docs.coincap.io/#89deffa0-ab03-4e0a-8d92-637a857d2c91"/>
        /// <returns>A Task&lt;<see cref="AssetsResponse"/>&gt; representing the asynchronous operation.</returns>
        public async Task<AssetsResponse> GetAssetsAsync(string search = null, IEnumerable<string> ids = null, uint limit = 100, uint offset = 0)
        {
            if (limit > 2000) { limit = 2000; }

            var request = new RestRequest(CoinCapClient.BuildUrl("assets"));
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) { request.AddQueryParameter("search", search); }
            if (ids != null && ids.Any()) { request.AddQueryParameter("ids", string.Join(",", ids)); }
            request.AddQueryParameter("limit", limit);
            if (offset != 0) { request.AddQueryParameter("offset", offset); }


            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 60);

            return JsonConvert.DeserializeObject<AssetsResponse>(jsonStr);
        }

        /// <summary>Get an asset by id as an asynchronous operation.</summary>
        /// <param name="id">The unique identifier of the asset.</param>
        /// <seealso href="https://docs.coincap.io/#f8869879-171f-4240-adfd-dd2947506adc"/>
        /// <returns>A Task&lt;AssetResponse&gt; representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">id - Null or invalid value, id must be a valid asset id.</exception>
        public async Task<AssetResponse> GetAssetAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException(nameof(id), "Null or invalid value, id must be a valid asset id."); }

            var request = new RestRequest(CoinCapClient.BuildUrl("assets", id));

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 60);

            return JsonConvert.DeserializeObject<AssetResponse>(jsonStr);
        }


        /// <summary>
        /// Get asset history as an asynchronous operation.
        /// </summary>
        /// <param name="id">The unique identifier of the asset.</param>
        /// <param name="interval">The interval to bucket data.</param>
        /// <param name="start">The start time, UNIX timestamp in milliseconds (UTC).</param>
        /// <param name="end">The end time, UNIX timestamp in milliseconds (UTC).</param>
        /// <seealso href="https://docs.coincap.io/#61e708a8-8876-4fb2-a418-86f12f308978"/>
        /// <returns>A Task&lt;<see cref="AssetHistoryResponse"/>&gt; representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">id - Null or invalid value, id must be a valid asset id.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid start/end values, start and end must both be valid timestamps.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">end - Invalid value, end must be a valid timestamp greater than start.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">start - Invalid value, start must be in the past.</exception>
        public async Task<AssetHistoryResponse> GetAssetHistoryAsync(string id, TimeInterval interval, ulong start = 0, ulong end = 0)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException(nameof(id), "Null or invalid value, id must be a valid asset id."); }

            if (start > 0 && end == 0 || end > 0 && start == 0) { throw new ArgumentOutOfRangeException("Invalid start/end values, start and end must both be valid timestamps."); }
            if (start > 0 && end <= start) { throw new ArgumentOutOfRangeException(nameof(end), "Invalid value, end must be a valid timestamp greater than start."); }
            if (start > 0 && end > 0 & (long)start > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) { throw new ArgumentOutOfRangeException(nameof(start), "Invalid value, start must be in the past."); }
            var request = new RestRequest(CoinCapClient.BuildUrl("assets", id, "history"));
            request.AddQueryParameter("interval", interval.ToString().ToLowerInvariant());
            if (start > 0 && end > 0)
            {
                request.AddQueryParameter("start", start);
                request.AddQueryParameter("end", end);
            }

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 30);

            return JsonConvert.DeserializeObject<AssetHistoryResponse>(jsonStr);
        }

        /// <summary>
        /// Get asset markets as an asynchronous operation.
        /// </summary>
        /// <param name="id">The unique identifier of the asset.</param>
        /// <param name="limit">The number of items to retrieve. Default: 100 Max: 2000.</param>
        /// <param name="offset">The number of items to skip (aka offset).</param>
        /// <seealso href="https://docs.coincap.io/#7f727b78-5013-41ec-aa1c-351b836458d0"/>
        /// <returns>A Task&lt;<see cref="AssetMarketsResponse"/>&gt; representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">id - Null or invalid value, id must be a valid asset id.</exception>
        public async Task<AssetMarketsResponse> GetAssetMarketsAsync(string id, uint limit = 100, uint offset = 0)
        {
            if (limit > 2000) { limit = 2000; }

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException(nameof(id), "Null or invalid value, id must be a valid asset id."); }

            var request = new RestRequest(CoinCapClient.BuildUrl("assets", id, "markets"));
            request.AddQueryParameter("limit", limit);
            if (offset != 0) { request.AddQueryParameter("offset", offset); }

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 60);

            return JsonConvert.DeserializeObject<AssetMarketsResponse>(jsonStr);
        }

    }
}
