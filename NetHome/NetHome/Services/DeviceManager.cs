using NetHome.Common;
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
        private readonly DeviceList<DeviceModel> _devices;
        private readonly WeakEventManager<DeviceModel> _devicesEventManager;
        private readonly WeakEventManager<Actions> _actionEventManager;

        public event EventHandler<DeviceModel> DeviceChanged
        {
            add => _devicesEventManager.AddEventHandler(value);
            remove => _devicesEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<Actions> ActionPerformed
        {
            add => _actionEventManager.AddEventHandler(value);
            remove => _actionEventManager.RemoveEventHandler(value);
        }

        public DeviceManager()
        {
            _devices = new DeviceList<DeviceModel>();
            _devicesEventManager = new WeakEventManager<DeviceModel>();
            _actionEventManager = new WeakEventManager<Actions>();
        }

        public void SetList(ICollection<DeviceModel> list)
        {
            _devices.Clear();
            _devices.AddRange(list);
        }

        public void Updated(DeviceModel device)
        {
            _devices.Update(device);
            _devicesEventManager.RaiseEvent(new object(), device, nameof(DeviceChanged));
        }
        
        public void PerformAction(Actions action)
        {
            _actionEventManager.RaiseEvent(new object(), action, nameof(ActionPerformed));
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

        public void ClearDevices()
        {
            _devices.Clear();
        }
    }
}
