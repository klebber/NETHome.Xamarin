using NetHome.Common;
using NetHome.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceManager _deviceManager;
        public DeviceService()
        {
            _deviceManager = DependencyService.Get<IDeviceManager>();
        }

        public async Task FetchAllDevices()
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/devices/getall");
            Stream stream = await response.Content.ReadAsStreamAsync();
            var devices = await JsonSerializer.DeserializeAsync<List<DeviceModel>>(stream, JsonHelper.GetOptions());
            _deviceManager.SetList(devices);
        }
    }
}
