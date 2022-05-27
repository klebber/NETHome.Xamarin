using NetHome.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IDeviceService
    {
        Task FetchAllDevices();
    }
}
