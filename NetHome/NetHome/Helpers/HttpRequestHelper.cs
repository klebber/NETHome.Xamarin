using NetHome.Models.User;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetHome.Helpers
{
    public static class HttpRequestHelper
    {
        internal static async Task<HttpResponseMessage> LoginAsync(LoginModel loginModel)
        {
            var json = JsonSerializer.Serialize(loginModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://192.168.0.100:34674/api/user/login";
            using var client = new HttpClient();

            return await client.PostAsync(url, data);
        }

        internal static async Task<bool> ValidateAsync(string token)
        {
            string url = "http://192.168.0.100:34674/api/user/validate";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
