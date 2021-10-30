using NetHome.Common.JsonConverters;
using NetHome.Common.Models;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Services
{
    public class DeviceService : IDeviceService
    {
        public async Task<ICollection<DeviceModel>> GetAll()
        {
            string token = await SecureStorage.GetAsync("AuthorizationToken");
            if (token is null) throw new Exception(); // TODO napravi exception koji pokriva ovaj i slicne slucajeve
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/devices/getall", token);
            Stream stream = await response.Content.ReadAsStreamAsync();
            string rez = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
                }
                catch (JsonException)
                {
                    message = "Unable to load devices from server.";
                }
                throw new ServerException(reason, message);
            }
            return await JsonSerializer.DeserializeAsync<List<DeviceModel>>(stream, JsonHelper.GetOptions());
        }
    }
}
