using NetHome.Common;
using NetHome.Helpers;

namespace NetHome.ViewModels.Devices
{
    public class RgbLightViewModel : BaseDeviceViewModel
    {
        public RGBLightModel RgbLight { get => (RGBLightModel)DeviceClone; set => DeviceClone = value; }

        internal void OnAppearing(int deviceId)
        {
            DeviceClone = _deviceManager.GetDeviceById(deviceId).Clone();
            Image = RgbLight.GetImage();
        }
    }
}
