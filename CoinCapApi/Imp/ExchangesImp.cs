// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-15-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-15-2022
// ***********************************************************************
// <copyright file="ExchangesImp.cs" company="ByronAP">
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
    /// <para>Implementation of the '/exchanges' API calls.</para>
    /// <para>Implementation classes do not have a public constructor
    /// and must be accessed through an instance of <see cref="CoinCapClient"/>.</para>
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#aff336c8-9d06-4654-bc15-a56cef06a69e"/>
    public class ExchangesImp
    {
        private readonly RestClient _restClient;
        private readonly ILogger<CoinCapClient> _logger;
        private readonly MemCache _cache;

        internal ExchangesImp(RestClient restClient, MemCache cache, ILogger<CoinCapClient> logger)
        {
            _logger = logger;
            _cache = cache;
            _restClient = restClient;
        }

        /// <summary>
        /// Get exchanges as an asynchronous operation.
        /// </summary>
        /// <seealso href="https://docs.coincap.io/#e1c56fd0-d57a-40dd-8a24-4b0883b58cfb"/>
        /// <returns>A Task&lt;<see cref="ExchangesResponse"/>&gt; representing the asynchronous operation.</returns>
        public async Task<ExchangesResponse> GetExchangesAsync()
        {
            var request = new RestRequest(CoinCapClient.BuildUrl("exchanges"));

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 120);

            return JsonConvert.DeserializeObject<ExchangesResponse>(jsonStr);
        }

        /// <summary>
        /// Get exchange as an asynchronous operation.
        /// </summary>
        /// <param name="id">The unique identifier of the exchange to get.</param>
        /// <seealso href="https://docs.coincap.io/#217e6d7e-feb8-4b9f-81d5-c53cda839579"/>
        /// <returns>A Task&lt;<see cref="ExchangeResponse"/>&gt; representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">id - Null or invalid value, id must be a valid exchange id.</exception>
        public async Task<ExchangeResponse> GetExchangeAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException(nameof(id), "Null or invalid value, id must be a valid exchange id."); }

            var request = new RestRequest(CoinCapClient.BuildUrl("exchanges", id));

            var jsonStr = await CoinCapClient.GetStringResponseAsync(_restClient, request, _cache, _logger, 60);

            return JsonConvert.DeserializeObject<ExchangeResponse>(jsonStr);
        }

    }
}
