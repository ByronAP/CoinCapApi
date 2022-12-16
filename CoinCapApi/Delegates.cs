using CoinCapApi.Models;
using System;
using System.Collections.Generic;

namespace CoinCapApi
{
    public delegate void OnTrade(object sender, Guid instanceId, string exchange, WSTrade trade);
    public delegate void OnPrices(object sender, Guid instanceId, Dictionary<string, decimal> prices);
}
