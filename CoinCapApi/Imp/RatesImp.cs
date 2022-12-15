// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-15-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-15-2022
// ***********************************************************************
// <copyright file="RatesImp.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using CoinCapApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace CoinCapApi.Imp
{
    /// <summary>
    /// <para>Implementation of the '/rates' API calls.</para>
    /// <para>Implementation classes do not have a public constructor
    /// and must be accessed through an instance of <see cref="CoinCapClient"/>.</para>
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#d4bac290-230a-48c6-a8eb-6804b2d137f3"/>
    public class RatesImp
    {
        private readonly RestClient _restClient;
        private readonly ILogger<CoinCapClient> _logger;
        private readonly MemCache _cache;

        internal RatesImp(RestClient restClient, MemCache cache, ILogger<CoinCapClient> logger)
        {
            _logger = logger;
            _cache = cache;
            _restClient = restClient;
        }

        /// <summary>
        /// Get rates of assets as an asynchronous operation.
        /// </summary>
        /// <seealso href="https://docs.coincap.io/#2a87f3d4-f61f-42d3-97e0-3a9afa41c73b"/>
        /// <returns>A Task&lt;<see cref="RatesResponse"/>&gt; representing the asynchronous operation.</returns>
        public async Task<RatesResponse> GetRatesAsync()
        {
            var request = new RestRequest(CoinCapClient.BuildUrl("rates"));

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 15);

            return JsonConvert.DeserializeObject<RatesResponse>(jsonStr);
        }

        /// <summary>
        /// Get the rate of an asset as an asynchronous operation.
        /// </summary>
        /// <param name="id">The asset id to get the rate for.</param>
        /// <seealso href="https://docs.coincap.io/#0a8102a5-c338-4661-aa99-f1c57661b5b1"/>
        /// <returns>A Task&lt;<see cref="RateResponse"/>&gt; representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">id - Null or invalid value, id must be a valid asset id.</exception>
        public async Task<RateResponse> GetRateAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException(nameof(id), "Null or invalid value, id must be a valid asset id."); }

            var request = new RestRequest(CoinCapClient.BuildUrl("rates", id));

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 15);

            return JsonConvert.DeserializeObject<RateResponse>(jsonStr);
        }
    }
}
