using NetHome.Exceptions;
using NetHome.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Helpers
{
    public static class HttpRequestHelper
    {
        public static async Task<HttpResponseMessage> GetAsync(string route)
        {
            string uri = Preferences.Get("ServerAddress", null);
            if (uri is null) throw new ServerCommunicationException("Server adress not found!", "Please enter valid server adress and try again.");
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(15);
            var token = await UserService.GetAuthorizationToken();
            if (token is null) throw new ServerAuthorizationException("Authorization token not found!");
            client.DefaultRequestHeaders.Add("Authorization", token);
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(uri).AbsoluteUri + route);
                return CheckResponse(response);
            }
            catch (HttpRequestException)
            {
                throw new ServerCommunicationException("Server unreachable!",
                    "Check if server address is correct and if server is running.");
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string route, string json)
        {
            string uri = Preferences.Get("ServerAddress", null);
            if (uri is null) throw new ServerCommunicationException("Server adress not found!", "Please enter valid server adress and try again.");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(15);
            var token = await UserService.GetAuthorizationToken();
            if (token is null) throw new ServerAuthorizationException("Authorization token not found!");
            client.DefaultRequestHeaders.Add("Authorization", token);
            try
            {
                HttpResponseMessage response = await client.PostAsync(new Uri(uri).AbsoluteUri + route, data);
                return CheckResponse(response);
            }
            catch (HttpRequestException)
            {
                throw new ServerCommunicationException("Server unreachable!",
                    "Http request has timed out. Check if server address is correct and if server is running.");
            }
        }

        public static async Task<HttpResponseMessage> PostUnauthorizedAsync(string route, string json)
        {
            string uri = Preferences.Get("ServerAddress", null);
            if (uri is null) throw new ServerCommunicationException("Server adress not found!", "Please enter valid server adress and try again.");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(15);
            try
            {
                HttpResponseMessage response = await client.PostAsync(new Uri(uri).AbsoluteUri + route, data); 
                return CheckResponse(response);
            }
            catch (HttpRequestException)
            {
                throw new ServerCommunicationException("Server unreachable!",
                    "Http request has timed out. Check if server address is correct and if server is running.");
            }
        }

        private static HttpResponseMessage CheckResponse(HttpResponseMessage response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK => response,
                HttpStatusCode.Unauthorized => throw new ServerAuthorizationException("Server returned error 401:Unauthorized!"),
                _ => throw new BadResponseException(response)
            };
        }
    }
}
