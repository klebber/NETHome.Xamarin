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
        Task<RequestResultPayload<List<RoomModel>>> GetAllRooms();
        Task<RequestResultPayload<List<DeviceTypeModel>>> GetAllDeviceTypes();
        Task<RequestResultPayload<DevicePayload>> GetDevicePayload(int deviceId);
        Task<RequestResult> AddRoom(RoomModel room);
        Task<RequestResult> DeleteRoom(RoomModel room);
        Task<RequestResult> AddType(DeviceTypeModel type);
        Task<RequestResult> DeleteType(DeviceTypeModel type);
    }
}
