using NetHome.Common.JsonConverters;
using NetHome.Common.Models;
using NetHome.Exceptions;
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
        public async Task FetchAllDevices()
        {
            string token = await SecureStorage.GetAsync("AuthorizationToken");
            if (token is null) throw new InvalidTokenException("Token not found!");
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/devices/getall", token);
            Stream stream = await response.Content.ReadAsStreamAsync();
            var devices = await JsonSerializer.DeserializeAsync<List<DeviceModel>>(stream, JsonHelper.GetOptions());
            DeviceManager.SetList(devices);
        }
    }
}
