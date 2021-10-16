using NetHome.Common.Models;
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
        internal static async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
        {
            var json = JsonSerializer.Serialize(loginModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://192.168.0.100:58332/api/user/login";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);
            if (!response.IsSuccessStatusCode) return null;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            LoginResponseModel loginResponse = JsonSerializer.Deserialize<LoginResponseModel>(jsonResponse, options);
            return loginResponse;
        }

        internal static async Task<UserModel> ValidateAsync(string token)
        {
            string url = "http://192.168.0.100:58332/api/user/validate";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            UserModel userData = JsonSerializer.Deserialize<UserModel>(jsonResponse, options);
            return userData;
        }
        
        internal static async Task<HttpResponseMessage> RegisterAsync(RegisterModel registerModel)
        {
            var json = JsonSerializer.Serialize(registerModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://192.168.0.31:58332/api/user/register";
            using var client = new HttpClient();

            return await client.PostAsync(url, data);
        }
    }
}
