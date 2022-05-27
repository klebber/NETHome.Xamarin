using NetHome.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IDeviceStateService
    {
        Task ChangeDeviceState(DeviceModel device);
        void DeviceStateChanged(DeviceModel device);
    }
}
