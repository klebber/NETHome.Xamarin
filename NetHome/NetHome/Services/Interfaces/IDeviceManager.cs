using NetHome.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Services
{
    public interface IDeviceManager
    {
        event EventHandler<DeviceModel> DeviceChanged;
        void SetList(ICollection<DeviceModel> list);
        void Updated(DeviceModel device);
        List<DeviceModel> GetSensors();
        List<DeviceModel> GetNonSensorDevices();
        DeviceModel GetDeviceById(int id);
    }
}
