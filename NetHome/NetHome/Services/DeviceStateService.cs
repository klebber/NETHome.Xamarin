using System.Text.Json;
using System.Threading.Tasks;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class DeviceStateService : IDeviceStateService
    {
        private readonly IDeviceManager _deviceManager;
        public DeviceStateService()
        {
            _deviceManager = DependencyService.Get<IDeviceManager>();
        }

        public async Task ChangeDeviceState(DeviceModel device)
        {
            var json = JsonSerializer.Serialize(device, JsonHelper.GetOptions());
            var response = await HttpRequestHelper.PostAsync("api/state/change", json);
            if (!response.IsSuccessStatusCode)
            {
                throw new BadResponseException(response);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            var newValue = await JsonSerializer.DeserializeAsync<DeviceModel>(stream, JsonHelper.GetOptions());
            _deviceManager.Updated(newValue);
        }

        public void DeviceStateChanged(DeviceModel device)
        {
            _deviceManager.Updated(device);
        }
    }
}
