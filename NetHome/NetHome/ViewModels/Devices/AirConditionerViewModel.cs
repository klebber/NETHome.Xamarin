using NetHome.Common;
using NetHome.Helpers;

namespace NetHome.ViewModels.Devices
{
    public class AirConditionerViewModel : BaseDeviceViewModel
    {
        public AirConditionerModel AirConditioner { get => (AirConditionerModel)DeviceClone; set => DeviceClone = value; }

        internal void OnAppearing(int deviceId)
        {
            DeviceClone = _deviceManager.GetDeviceById(deviceId).Clone();
            Image = AirConditioner.GetImage();
        }
    }
}
