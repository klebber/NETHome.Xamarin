using NetHome.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Helpers
{
    public static class HttpRequestHelper
    {
        public static async Task<HttpResponseMessage> GetAsync(string route, string token = null)
        {
            try
            {
                string uri = Preferences.Get("ServerAddress", null);
                if (uri is null) throw new ServerCommunicationException("Server adress not found!", "Please enter valid server adress and try again.");
                using HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(15);
                if (token is not null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.GetAsync(new Uri(uri).AbsoluteUri + route);
                return response.IsSuccessStatusCode ? response : throw new ServerCommunicationException(response);
            }
            catch (Exception)
            {
                throw new ServerCommunicationException("Server unreachable!", 
                    "Check if server address is correct and if server is running.");
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string route, string json, string token = null)
        {
            try
            {
                string uri = Preferences.Get("ServerAddress", null);
                if (uri is null) throw new ServerCommunicationException("Server adress not found!", "Please enter valid server adress and try again.");
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(15);
                if (token is not null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.PostAsync(new Uri(uri).AbsoluteUri + route, data);
                return response.IsSuccessStatusCode ? response : throw new ServerCommunicationException(response);
            }
            catch (Exception)
            {
                throw new ServerCommunicationException("Server unreachable!",
                    "Http request has timed out. Check if server address is correct and if server is running.");
            }
        }
    }
}
