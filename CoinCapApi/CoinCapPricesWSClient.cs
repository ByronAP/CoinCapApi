// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-16-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-16-2022
// ***********************************************************************
// <copyright file="CoinCapPricesWSClient.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using ByronAP.Net.WebSockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinCapApi
{
    /// <summary>
    /// The CoinCap prices endpoint is the most accurate source of real-time changes to the global price of an asset. Each time the system receives data that moves the global price in one direction or another, this change is immediately published through the websocket. These prices correspond with the values shown in /assets - a value that may change several times per second based on market activity.
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#37dcec0b-1f7b-4d98-b152-0217a6798058"/>
    /// <seealso cref="IDisposable" />
    public class CoinCapPricesWSClient : IDisposable
    {
        /// <summary>
        /// Occurs when new price data arrives.
        /// </summary>
        public event OnPrices OnPricesEvent;

        /// <summary>
        /// Occurs when the connection state changes.
        /// </summary>
        public event OnConnectionStateChanged OnConnectionStateChangedEvent;

        /// <summary>
        /// Provides access to the underlying WebSocketClient instance.
        /// </summary>
        /// <value>The WebSocket.</value>
        public WebSocketClient CCWebSocket { get; }

        /// <summary>
        /// Each class instance is assigned a unique ID which allows you to keep track of multiple instances.
        /// </summary>
        /// <value>The instance identifier.</value>
        public Guid InstanceID => CCWebSocket.InstanceId;

        private readonly ILogger<CoinCapPricesWSClient> _logger;
        private readonly IEnumerable<string> _assets;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinCapPricesWSClient"/> class.
        /// </summary>
        /// <param name="assets">The assets you wish to subscribe to or ALL for all assets. Names are case sensitive.</param>
        /// <param name="options">The WebSocketClient options.</param>
        public CoinCapPricesWSClient(IEnumerable<string> assets, WebSocketOptions options)
        {
            _assets = assets;

            if (options.Logger != null) { _logger = (ILogger<CoinCapPricesWSClient>)options.Logger; }

            if (string.IsNullOrEmpty(options.Url) || string.IsNullOrWhiteSpace(options.Url)) { options.Url = $"{Constants.API_WS_BASE_URL}/prices?assets={string.Join(",", _assets)}"; }

            CCWebSocket = new WebSocketClient(options);

            AttachEventHandlers();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinCapPricesWSClient"/> class.
        /// </summary>
        /// <param name="assets">The assets you wish to subscribe to or ALL for all assets. Names are case sensitive.</param>
        /// <param name="logger">The logger.</param>
        public CoinCapPricesWSClient(IEnumerable<string> assets, ILogger<CoinCapPricesWSClient> logger = null)
        {
            _assets = assets;

            _logger = logger;

            var options = new WebSocketOptions($"{Constants.API_WS_BASE_URL}/prices?assets={string.Join(",", assets)}");
            if (logger != null) { options.Logger = _logger; }

            CCWebSocket = new WebSocketClient(options);

            AttachEventHandlers();
        }

        /// <summary>
        /// Connects this instance to the websocket server.
        /// </summary>
        /// <returns><c>true</c> if connection success, <c>false</c> otherwise.</returns>
        public async Task<bool> Connect()
        {
            try
            {
                var result = await CCWebSocket.ConnectAsync();
                if (result.Item1) { return true; }

                _logger?.LogError(result.Item2, "WebSocket {ID} connection failed.", InstanceID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "WebSocket {ID} connection error.", InstanceID);
            }

            return false;
        }

        /// <summary>
        /// Disconnects this instance from the websocket server.
        /// </summary>
        public async Task Disconnect()
        {
            try
            {
                await CCWebSocket.DisconnectAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "WebSocket {ID} error while closing.", InstanceID);
            }
        }

        private void AttachEventHandlers()
        {
            CCWebSocket.ConnectionStateChanged += CCWebSocket_ConnectionStateChanged;
            CCWebSocket.MessageReceived += CCWebSocket_MessageReceived;
        }

        private void CCWebSocket_MessageReceived(object sender, string message)
        {
            _logger?.LogDebug("WebSocket {ID} Received Message: {Message}.", CCWebSocket.InstanceId, message);
            try
            {
                var price = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(message);

                OnPricesEvent?.Invoke(this, InstanceID, price);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "WebSocket {ID} error processing message {Message}.", InstanceID, message);
            }
        }

        private void CCWebSocket_ConnectionStateChanged(object sender, System.Net.WebSockets.WebSocketState newWebSocketState, System.Net.WebSockets.WebSocketState oldWebSocketState)
        {
            OnConnectionStateChangedEvent?.Invoke(this, newWebSocketState, oldWebSocketState);

            _logger?.LogInformation("WebSocket {ID} connection state changed from {OldState} to {NewState}.", InstanceID, oldWebSocketState, newWebSocketState);

            if (newWebSocketState == System.Net.WebSockets.WebSocketState.Aborted)
            {
                _logger?.LogWarning("WebSocket {ID} connection was aborted.", InstanceID);
            }

            if (newWebSocketState == System.Net.WebSockets.WebSocketState.Closed)
            {
                _logger?.LogWarning("WebSocket {ID} connection was closed.", InstanceID);
            }
        }

        public void Dispose()
        {
            ((IDisposable)CCWebSocket).Dispose();
        }
    }
}
