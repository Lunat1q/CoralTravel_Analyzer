﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TiqUtils.Serialize;
using TiqUtils.Utils;

namespace CoralTravelAnalyzer.CoralTravelApi
{
    public abstract class ApiBase<T> : IDisposable, IWebRequestApi<T>
    {
        private const string BaseUrl = "http://www.coral.ru/";

        private readonly Uri _baseUri = new Uri(BaseUrl);

        private readonly HttpClient _client;

        private bool _requestInRun;
        private CancellationTokenSource _cts;

        protected ApiBase()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            _client = new HttpClient(handler) {BaseAddress = _baseUri};
            cookieContainer.Add(_baseUri, new Cookie("ClientInfo", "did=14398"));
        }

        private void AskStop()
        {
            if (_requestInRun)
            {
                _cts.Cancel();
            }
            _cts = new CancellationTokenSource();
        }

        public async Task<T> GetDataAsync(bool instant = false, int delaySec = 5)
        {
            var result = default(T);

            if (_requestInRun) return default(T);

            AskStop();
            _requestInRun = true;
            try
            {
                if (!instant)
                    await Task.Delay(delaySec * 1000);
                var resultMsg = await _client.GetAsync(GetRequestUrl(), _cts.Token);
                var resultContent = await resultMsg.Content.ReadAsStringAsync();
                result = Json.DeserializeDataFromString<T>(resultContent);
            }
            catch (OperationCanceledException)
            {
                //ignore
            }
            catch (Exception ex)
            {
                Logging.ErrorLog(ex.Message);
            }
            finally
            {
                _requestInRun = false;
            }
            return result;
        }

        protected abstract string GetRequestUrl();

        public void Dispose()
        {
            _client?.Dispose();
            
        }

        public abstract void SetRequestParameters(params string[] parameters);
    }
}