using NetHome.Common;
using NetHome.Helpers;

namespace NetHome.ViewModels.Devices
{
    public class SmartSwitchViewModel : BaseDeviceViewModel
    {
        public SmartSwitchModel SmartSwitch { get => (SmartSwitchModel)DeviceClone; set => DeviceClone = value; }

        internal void OnAppearing(int deviceId)
        {
            DeviceClone = _deviceManager.GetDeviceById(deviceId).Clone();
            Image = SmartSwitch.GetImage();
        }

    }
}
