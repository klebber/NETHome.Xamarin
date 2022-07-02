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
                return new RequestResultPayload<List<DeviceModel>>(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
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
                return new RequestResultPayload<DeviceModel>(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
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
                return new RequestResultPayload<DeviceModel>(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
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
                return new RequestResult(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<RequestResultPayload<List<string>>> GetAllRooms()
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync("api/devices/getrooms");
                var stream = await response.Content.ReadAsStreamAsync();
                var rooms =  await JsonSerializer.DeserializeAsync<List<string>>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<List<string>>(true, rooms);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<List<string>>(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<RequestResultPayload<List<string>>> GetAllDeviceTypes()
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync("api/devices/gettypes");
                var stream = await response.Content.ReadAsStreamAsync();
                var types = await JsonSerializer.DeserializeAsync<List<string>>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<List<string>>(true, types);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<List<string>>(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
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
                return new RequestResultPayload<DevicePayload>(false)
                {
                    ErrorType = e.GetType().Name,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
