using NetHome.Common;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Services
{
    public interface IDeviceManager
    {
        event EventHandler<DeviceModel> DeviceChanged;
        event EventHandler<Actions> ActionPerformed;
        void SetList(ICollection<DeviceModel> list);
        void Updated(DeviceModel device);
        void PerformAction(Actions action);
        List<DeviceModel> GetSensors();
        List<DeviceModel> GetNonSensorDevices();
        DeviceModel GetDeviceById(int id);
        void ClearDevices();
    }
}
