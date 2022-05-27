using NetHome.Common;
using NetHome.Helpers;

namespace NetHome.ViewModels.Devices
{
    public class RollerShutterViewModel : BaseDeviceViewModel
    {
        public RollerShutterModel RollerShutter { get => (RollerShutterModel)DeviceClone; set => DeviceClone = value; }

        internal void OnAppearing(int deviceId)
        {
            DeviceClone = _deviceManager.GetDeviceById(deviceId).Clone();
            Image = RollerShutter.GetImage();
        }
    }
}
