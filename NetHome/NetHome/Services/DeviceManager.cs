using NetHome.Common.Models;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Helpers;

namespace NetHome.Services
{
    public class DeviceManager : IDeviceManager
    {
        private static DeviceManager _instance;
        public static DeviceManager Instance => _instance ??= new DeviceManager();

        private readonly DeviceList<DeviceModel> _devices;
        private readonly WeakEventManager<DeviceModel> _eventManager;

        public event EventHandler<DeviceModel> DeviceChanged
        {
            add => _eventManager.AddEventHandler(value);
            remove => _eventManager.RemoveEventHandler(value);
        }

        private DeviceManager()
        {
            _devices = new DeviceList<DeviceModel>();
            _eventManager = new WeakEventManager<DeviceModel>();
        }

        public void SetList(ICollection<DeviceModel> list)
        {
            _devices.Clear();
            _devices.AddRange(list);
        }

        public void Updated(DeviceModel device)
        {
            _devices.Update(device);
            _eventManager.RaiseEvent(new object(), device, nameof(DeviceChanged));
        }

        public List<DeviceModel> GetSensors()
        {
            return _devices.Where(d => d.Type == "Sensor").ToList();
        }

        public List<DeviceModel> GetNonSensorDevices()
        {
            return _devices.Where(d => d.Type != "Sensor").ToList();
        }

        public DeviceModel GetDeviceById(int id)
        {
            return _devices.SingleOrDefault(d => d.Id == id);
        }
    }
}
