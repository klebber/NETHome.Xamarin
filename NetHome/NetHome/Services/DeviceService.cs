using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceManager _deviceManager;
        public DeviceService()
        {
            _deviceManager = DependencyService.Get<IDeviceManager>();
        }

        public async Task<RequestResultPayload<List<DeviceModel>>> GetAllDevices()
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync("api/devices/getall");
                var stream = await response.Content.ReadAsStreamAsync();
                var devices = await JsonSerializer.DeserializeAsync<List<DeviceModel>>(stream, JsonHelper.GetOptions());
                _deviceManager.SetList(devices);
                return new RequestResultPayload<List<DeviceModel>>(true, devices);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<List<DeviceModel>>(e);
            }
        }

        public async Task<RequestResultPayload<DeviceModel>> AddDevice(DevicePayload device)
        {
            try
            {
                var json = JsonSerializer.Serialize(device, JsonHelper.GetOptions());
                var response = await HttpRequestHelper.PostAsync("api/devices/add", json);
                var stream = await response.Content.ReadAsStreamAsync();
                var newValue = await JsonSerializer.DeserializeAsync<DeviceModel>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<DeviceModel>(true, newValue);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<DeviceModel>(e);
            }
        }

        public async Task<RequestResultPayload<DeviceModel>> UpdateDevice(DevicePayload device)
        {
            try
            {
                var json = JsonSerializer.Serialize(device, JsonHelper.GetOptions());
                var response = await HttpRequestHelper.PostAsync("api/devices/update", json);
                var stream = await response.Content.ReadAsStreamAsync();
                var newValue = await JsonSerializer.DeserializeAsync<DeviceModel>(stream, JsonHelper.GetOptions());
                _deviceManager.Updated(newValue);
                return new RequestResultPayload<DeviceModel>(true, newValue);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<DeviceModel>(e);
            }
        }

        public async Task<RequestResult> DeleteDevice(DeviceModel device)
        {
            try
            {
                var json = JsonSerializer.Serialize(device, JsonHelper.GetOptions());
                await HttpRequestHelper.PostAsync("api/devices/delete", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }

        public async Task<RequestResultPayload<List<RoomModel>>> GetAllRooms()
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync("api/devices/getrooms");
                var stream = await response.Content.ReadAsStreamAsync();
                var rooms =  await JsonSerializer.DeserializeAsync<List<RoomModel>>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<List<RoomModel>>(true, rooms);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<List<RoomModel>>(e);
            }
        }

        public async Task<RequestResultPayload<List<DeviceTypeModel>>> GetAllDeviceTypes()
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync("api/devices/gettypes");
                var stream = await response.Content.ReadAsStreamAsync();
                var types = await JsonSerializer.DeserializeAsync<List<DeviceTypeModel>>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<List<DeviceTypeModel>>(true, types);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<List<DeviceTypeModel>>(e);
            }
        }

        public async Task<RequestResultPayload<DevicePayload>> GetDevicePayload(int deviceId)
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync($"api/devices/getpayload?id={deviceId}");
                var stream = await response.Content.ReadAsStreamAsync();
                var newValue = await JsonSerializer.DeserializeAsync<DevicePayload>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<DevicePayload>(true, newValue);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<DevicePayload>(e);
            }
        }

        public async Task<RequestResult> AddRoom(RoomModel room)
        {
            try
            {
                var json = JsonSerializer.Serialize(room, JsonHelper.GetOptions());
                await HttpRequestHelper.PostAsync("api/devices/addroom", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }

        public async Task<RequestResult> DeleteRoom(RoomModel room)
        {
            try
            {
                var json = JsonSerializer.Serialize(room, JsonHelper.GetOptions());
                await HttpRequestHelper.PostAsync("api/devices/deleteroom", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }

        public async Task<RequestResult> AddType(DeviceTypeModel type)
        {
            try
            {
                var json = JsonSerializer.Serialize(type, JsonHelper.GetOptions());
                await HttpRequestHelper.PostAsync("api/devices/addtype", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }

        public async Task<RequestResult> DeleteType(DeviceTypeModel type)
        {
            try
            {
                var json = JsonSerializer.Serialize(type, JsonHelper.GetOptions());
                await HttpRequestHelper.PostAsync("api/devices/deletetype", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }
    }
}
