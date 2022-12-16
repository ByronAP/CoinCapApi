// ***********************************************************************
// Assembly         : CoinCapApi
// Author           : ByronAP
// Created          : 12-16-2022
//
// Last Modified By : ByronAP
// Last Modified On : 12-16-2022
// ***********************************************************************
// <copyright file="CoinCapTradesWSClient.cs" company="ByronAP">
//     Copyright © 2022 ByronAP, CoinCap. All rights reserved.
// </copyright>
// ***********************************************************************
using ByronAP.Net.WebSockets;
using CoinCapApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CoinCapApi
{
    /// <summary>
    /// The CoinCap trade websocket streams trades from other cryptocurrency exchange websockets. Users must select a specific exchange. In the /exchanges endpoint users can determine if an exchange has a socket available by noting response 'socket':true/false. See an example in the /exchanges endpoint documentation above. The trades websocket is the only way to receive individual trade data through CoinCap.
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso href="https://docs.coincap.io/#ed9ed517-dd00-4d1d-98e4-772643117d9e"/>
    /// <seealso cref="IDisposable" />
    public class CoinCapTradesWSClient : IDisposable
    {
        /// <summary>
        /// Occurs when trade data is received.
        /// </summary>
        public event OnTrade OnTradeEvent;

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

        private readonly ILogger<CoinCapTradesWSClient> _logger;
        private readonly string _exchangeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinCapTradesWSClient"/> class.
        /// </summary>
        /// <param name="exchangeId">The exchange you wish to subscribe to.</param>
        /// <param name="options">The WebSocketClient options.</param>
        public CoinCapTradesWSClient(string exchangeId, WebSocketOptions options)
        {
            _exchangeId = exchangeId;

            if (options.Logger != null) { _logger = (ILogger<CoinCapTradesWSClient>)options.Logger; }

            if (string.IsNullOrEmpty(options.Url) || string.IsNullOrWhiteSpace(options.Url)) { options.Url = $"{Constants.API_WS_BASE_URL}/trades/{_exchangeId}"; }

            CCWebSocket = new WebSocketClient(options);

            AttachEventHandlers();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinCapTradesWSClient"/> class.
        /// </summary>
        /// <param name="exchangeId">The exchange you wish to subscribe to.</param>
        /// <param name="logger">The logger.</param>
        public CoinCapTradesWSClient(string exchangeId, ILogger<CoinCapTradesWSClient> logger = null)
        {
            _exchangeId = exchangeId;

            _logger = logger;

            var options = new WebSocketOptions($"{Constants.API_WS_BASE_URL}/trades/{_exchangeId}");
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
                var trade = JsonConvert.DeserializeObject<WSTrade>(message);

                OnTradeEvent?.Invoke(this, InstanceID, _exchangeId, trade);
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
