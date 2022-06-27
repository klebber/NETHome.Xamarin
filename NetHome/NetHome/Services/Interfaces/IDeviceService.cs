using NetHome.Common;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetHome.Services
{
    public interface IDeviceService
    {
        Task<RequestResult> FetchAllDevices();
    }
}
