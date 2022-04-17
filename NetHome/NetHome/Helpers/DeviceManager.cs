using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.Helpers
{
    public static class DeviceManager
    {
        private static readonly DeviceList<DeviceModel> _devices = new();

        public static void SetList(ICollection<DeviceModel> list)
        {
            _devices.Clear();
            _devices.AddRange(list);
        }

        public static void Updated(DeviceModel device)
        {
            _devices.Update(device);
            MessagingCenter.Send<object, DeviceModel>(null, device.Id.ToString(), device);
        }

        public static List<DeviceModel> GetSensors()
        {
            return _devices.Where(d => d.Type == "Sensor").ToList();
        }

        public static List<DeviceModel> GetNonSensorDevices()
        {
            return _devices.Where(d => d.Type != "Sensor").ToList();
        }
    }
}
