using NetHome.Common.Models;
using NetHome.Helpers;
using NetHome.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Exceptions
{
    public class DeviceStateService : IDeviceStateService
    {
        public async Task ChangeDeviceState(DeviceModel device)
        {
            var token = await SecureStorage.GetAsync("AuthorizationToken");
            if (token is null) throw new InvalidTokenException("Token not found!");
            var json = JsonSerializer.Serialize(device, JsonHelper.GetOptions());
            var response = await HttpRequestHelper.PostAsync("api/state/change", json, token);
            var stream = await response.Content.ReadAsStreamAsync();
            var newValue = await JsonSerializer.DeserializeAsync<DeviceModel>(stream, JsonHelper.GetOptions());
            DeviceManager.Updated(newValue);
        }
    }
}
