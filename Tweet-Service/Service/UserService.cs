using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Tweet_Service.Models;

namespace Tweet_Service.Service
{
    public class UserService
    {
        private readonly HttpClient _client = new();
        private readonly string _urlUserService;

        public UserService(IConfiguration config)
        {
            _urlUserService = config.GetValue<string>("Url:UserService");
        }

        public async Task<List<string>> GetUserNamesFollowing(string userName)
        {
            string endPoint = "getFollowingUserNames/";
            List<string> userNames = await _client.GetFromJsonAsync<List<string>>(_urlUserService + endPoint + userName);
            return userNames;
        }

        public async Task<UserDTO> GetUserByUserName(string userName)
        {
            string endPoint = "getByUserName/";
            var content = await _client.GetStringAsync(_urlUserService + endPoint + userName);
            if (content == "")
            {
                return null;
            }
            return JsonSerializer.Deserialize<UserDTO>(content);

        }
    }
}
