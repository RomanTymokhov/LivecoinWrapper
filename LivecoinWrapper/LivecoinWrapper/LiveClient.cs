﻿using LivecoinWrapper.DataLayer;
using LivecoinWrapper.DataLayer.ReciveData;
using LivecoinWrapper.DataLayer.RequestData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LivecoinWrapper
{
    public class LiveClient
    {
        private HttpClient httpClient;

        private readonly string publicKey;
        private readonly string secretKey;

        private const string baseAddress = "https://api.livecoin.net";

        public LiveClient()
        {
            httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public LiveClient(string pubKey, string secKey)
        {
            publicKey = pubKey;
            secretKey = secKey;

            httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        protected async Task<T> JsonGetAsync<T>(string requestUri)
        {
            var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();         // throw if web request failed

            var json = await response.Content.ReadAsStringAsync();

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
        }

        #region Public Methods

        public async Task<List<Ticker>> GetTickerAsync()
        {
            return await JsonGetAsync<List<Ticker>>(LiveMethod.GetTickerUri());
        }

        public async Task<Ticker> GetTickerAsync(string marketPair)
        {
            return await JsonGetAsync<Ticker>(LiveMethod.GetTickerUri(marketPair));
        }


        public async Task<List<Trade>> GetLastTradesAsync(string marketPair, string ordType = "false", string minOrHour = "false")
        {
            // minOrHour = "true" - данные за последнюю минуту, "false"(по умолчанию) - данные за час
            // ordType = "BUY" or "SELL", "false"(по умолчанию)
            return await JsonGetAsync<List<Trade>>(LiveMethod.GetLasttradeUri(marketPair, ordType, minOrHour));
        }


        public async Task<object> GetOllOrderBookAsync()
        {
            return await JsonGetAsync<object>(LiveMethod.GetOllOrdebookUri());
        }

        public async Task<OrderBook> GetOrderBookAsync(string marketPair)
        {
            return await JsonGetAsync<OrderBook>(LiveMethod.GetOrderbookUri(marketPair));
        }

        #endregion


    }
}
