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
        Task<RequestResultPayload<List<DeviceModel>>> GetAllDevices();
        Task<RequestResultPayload<DeviceModel>> AddDevice(DevicePayload device);
        Task<RequestResultPayload<DeviceModel>> UpdateDevice(DevicePayload device);
        Task<RequestResult> DeleteDevice(DeviceModel device);
        Task<RequestResultPayload<List<string>>> GetAllRooms();
        Task<RequestResultPayload<List<string>>> GetAllDeviceTypes();
        Task<RequestResultPayload<DevicePayload>> GetDevicePayload(int deviceId);
        Task<RequestResult> AddRoom(string roomName);
        Task<RequestResult> DeleteRoom(string roomName);
        Task<RequestResult> AddType(string typeName);
        Task<RequestResult> DeleteType(string typeName);
    }
}
