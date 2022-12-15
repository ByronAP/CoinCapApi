// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-14-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-14-2022
// ***********************************************************************
// <copyright file="CoinCapClient.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using CoinCapApi.Exceptions;
using CoinCapApi.Imp;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoinCapApi
{
    /// <summary>
    /// <para>Create an instance of this class to access the API methods.</para>
    /// <para>
    /// Methods and parameters are named as specified in the official
    /// CoinCap API documentation (Ex: API call '/rates/{id}' 
    /// translates to 'CoinCapClient.Rates.GetRateAsync({id})').
    /// </para>
    /// <para>By default response caching is enabled. To disable it set <see cref="IsCacheEnabled"/> to <c>false</c>.</para>
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class CoinCapClient : IDisposable
    {
        /// <summary>
        /// The RestSharp client instance used to make the API calls.
        /// This is exposed in case you wish to change options such as use a proxy.
        /// </summary>
        /// <value>The RestSharp client instance.</value>
        public RestClient CCRestClient { get; }

        /// <summary>
        /// <para>Provides access to the Assets API calls.</para>
        /// An instance of <see cref="AssetsImp"/>.
        /// </summary>
        /// <value>Assets API calls.</value>
        public AssetsImp Assets { get; }

        /// <summary>
        /// <para>Provides access to the Rates API calls.</para>
        /// An instance of <see cref="RatesImp"/>.
        /// </summary>
        /// <value>Rates API calls.</value>
        public RatesImp Rates { get; }

        /// <summary>
        /// <para>Provides access to the Exchanges API calls.</para>
        /// An instance of <see cref="ExchangesImp"/>.
        /// </summary>
        /// <value>Exchanges API calls.</value>
        public ExchangesImp Exchanges { get; }

        /// <summary>
        /// <para>Provides access to the Markets API calls.</para>
        /// An instance of <see cref="MarketsImp"/>.
        /// </summary>
        /// <value>Markets API calls.</value>
        public MarketsImp Markets { get; }

        /// <summary>
        /// <para>Provides access to the Candles API calls.</para>
        /// An instance of <see cref="CandlesImp"/>.
        /// </summary>
        /// <value>Candles API calls.</value>
        public CandlesImp Candles { get; }

        /// <summary>
        /// <para>Gets or sets whether this instance is using response caching.</para>
        /// <para>Caching is enabled by default.</para>
        /// </summary>
        /// <value><c>true</c> if this instances cache is enabled; otherwise, <c>false</c>.</value>
        public bool IsCacheEnabled { get { return _cache.Enabled; } set { _cache.Enabled = value; } }

        private readonly MemCache _cache;
        private bool _disposedValue;
        private readonly ILogger<CoinCapClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinCapClient"/> class.
        /// <para>Using an API key is optional but can be obtained for free from <see href="https://coincap.io/api-key"/></para>
        /// <para>Using a logger is optional but recommended. This can be supplied via dependency injection.</para>
        /// </summary>
        /// <param name="apiKey">Your API key.</param>
        /// <param name="logger">Your logger.</param>
        public CoinCapClient(string apiKey = null, ILogger<CoinCapClient> logger = null)
        {
            _logger = logger;
            _cache = new MemCache(_logger);

            CCRestClient = new RestClient(Constants.API_BASE_URL);
            if (!string.IsNullOrEmpty(apiKey) && !String.IsNullOrWhiteSpace(apiKey))
            {
                CCRestClient.AddDefaultHeader("Authorization", $"Bearer {apiKey}");
            }

            CCRestClient.AddDefaultHeader("Accept-Encoding", "gzip, deflate, br");
            CCRestClient.AddDefaultHeader("Accept", "application/json");
            CCRestClient.AddDefaultHeader("Connection", "keep-alive");
            CCRestClient.AddDefaultHeader("User-Agent", $"CoinCapApi .NET Client/{Assembly.GetExecutingAssembly().GetName().Version}");

            Assets = new AssetsImp(CCRestClient, _cache, _logger);
            Rates = new RatesImp(CCRestClient, _cache, _logger);
            Exchanges = new ExchangesImp(CCRestClient, _cache, _logger);
            Markets = new MarketsImp(CCRestClient, _cache, _logger);
            Candles = new CandlesImp(CCRestClient, _cache, _logger);
        }

        /// <summary>
        /// Clears the response cache.
        /// </summary>
        public void ClearCache() => _cache.Clear();

        internal static async Task<string> GetStringResponseAsync(RestClient client, RestRequest request, MemCache cache, ILogger logger, int cacheTime)
        {
            var fullUrl = client.BuildUri(request).ToString();

            try
            {
                if (cache.TryGet(fullUrl, out var cacheResponse))
                {
                    return (string)cacheResponse;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "");
            }

            try
            {
                var response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    cache.CacheRequest(fullUrl, response, cacheTime);

                    return response.Content;
                }

                if (response.ErrorException != null)
                {
                    logger?.LogError(response.ErrorException, "GetStringResponseAsync failed.");
                    throw response.ErrorException;
                }

                throw new UnknownException($"Unknown exception, http response code is not success, {response.StatusCode}.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "GetStringResponseAsync request failure.");
                throw;
            }
        }

        internal static string BuildUrl(params string[] parts)
        {
            if (parts.Length > 2)
            {
                var sb = new StringBuilder();
                sb.Append("/v").Append(Constants.API_VERSION);
                foreach (var part in parts)
                {
                    sb.Append('/');
                    sb.Append(part);
                }
                return sb.ToString();
            }
            else
            {
                var result = $"/v{Constants.API_VERSION}";
                foreach (var part in parts)
                {
                    result += $"/{part}";
                }
                return result;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_cache != null)
                    {
                        try
                        {
                            _cache.Dispose();
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
