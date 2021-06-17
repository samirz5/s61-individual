using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Tweet_Service.Service
{
    public class TrendService
    {
        private readonly HttpClient _client = new();
        private readonly string _urlTrendService;

        public TrendService(IConfiguration config)
        {
            _urlTrendService = config.GetValue<string>("Url:TrendService");
        }

        public async Task<List<Guid>> GetTweetIdsOfTrend(string trend)
        {
            string endPoint = "tweetIdsByTrend/";
            List<Guid> tweetIds = await _client.GetFromJsonAsync<List<Guid>>(_urlTrendService + endPoint + trend);
            return tweetIds;
        }
    }
}
