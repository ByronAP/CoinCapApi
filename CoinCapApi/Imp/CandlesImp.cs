// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-15-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-15-2022
// ***********************************************************************
// <copyright file="CandlesImp.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using CoinCapApi.Models;
using CoinCapApi.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace CoinCapApi.Imp
{
    /// <summary>
    /// <para>Implementation of the '/candles' API calls.</para>
    /// <para>Implementation classes do not have a public constructor
    /// and must be accessed through an instance of <see cref="CoinCapClient"/>.</para>
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#ab6ce4ff-3669-4b60-88bb-a5e7c12e6881"/>
    public class CandlesImp
    {
        private readonly RestClient _restClient;
        private readonly ILogger<CoinCapClient> _logger;
        private readonly MemCache _cache;

        internal CandlesImp(RestClient restClient, MemCache cache, ILogger<CoinCapClient> logger)
        {
            _logger = logger;
            _cache = cache;
            _restClient = restClient;
        }

        /// <summary>
        /// Get candle data (OHLCV) as an asynchronous operation.
        /// </summary>
        /// <param name="exchangeId">The unique identifier of the exchange.</param>
        /// <param name="interval">The interval to bucket data.</param>
        /// <param name="baseId">The base asset identifier.</param>
        /// <param name="quoteId">The quote asset identifier.</param>
        /// <param name="start">UNIX time in milliseconds. omitting will return the most recent candles.</param>
        /// <param name="end">UNIX time in milliseconds. omitting will return the most recent candles.</param>
        /// <returns>A Task&lt;<see cref="CandlesResponse"/>&gt; representing the asynchronous operation.</returns>
        /// <seealso href="https://docs.coincap.io/#51da64d7-b83b-4fac-824f-3f06b6c8d944"/>
        /// <exception cref="System.ArgumentNullException">exchangeId - Null or invalid value, exchangeId must be a valid exchange id.</exception>
        /// <exception cref="System.ArgumentNullException">baseId - Null or invalid value, baseId must be a valid asset id.</exception>
        /// <exception cref="System.ArgumentNullException">quoteId - Null or invalid value, quoteId must be a valid asset id.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid start/end values, start and end must both be valid timestamps.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">end - Invalid value, end must be a valid timestamp greater than start.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">start - Invalid value, start must be in the past.</exception>
        public async Task<CandlesResponse> GetCandlesAsync(string exchangeId, TimeInterval interval, string baseId, string quoteId, ulong start = 0, ulong end = 0)
        {
            if (string.IsNullOrEmpty(exchangeId) || string.IsNullOrWhiteSpace(exchangeId)) { throw new ArgumentNullException(nameof(exchangeId), "Null or invalid value, exchangeId must be a valid exchange id."); }
            if (string.IsNullOrEmpty(baseId) || string.IsNullOrWhiteSpace(baseId)) { throw new ArgumentNullException(nameof(baseId), "Null or invalid value, baseId must be a valid asset id."); }
            if (string.IsNullOrEmpty(quoteId) || string.IsNullOrWhiteSpace(quoteId)) { throw new ArgumentNullException(nameof(quoteId), "Null or invalid value, quoteId must be a valid asset id."); }

            if (start > 0 && end == 0 || end > 0 && start == 0) { throw new ArgumentOutOfRangeException("Invalid start/end values, start and end must both be valid timestamps."); }
            if (start > 0 && end <= start) { throw new ArgumentOutOfRangeException(nameof(end), "Invalid value, end must be a valid timestamp greater than start."); }
            if (start > 0 && end > 0 & (long)start > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) { throw new ArgumentOutOfRangeException(nameof(start), "Invalid value, start must be in the past."); }

            var request = new RestRequest(CoinCapClient.BuildUrl("candles"));
            request.AddQueryParameter("exchangeId", exchangeId);
            request.AddQueryParameter("interval", interval.ToString().ToLowerInvariant());
            request.AddQueryParameter("baseId", baseId);
            request.AddQueryParameter("quoteId", quoteId);
            if (start > 0) { request.AddQueryParameter("start", start); }
            if (end > 0) { request.AddQueryParameter("end", end); }

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 60);

            return JsonConvert.DeserializeObject<CandlesResponse>(jsonStr);
        }
    }
}
