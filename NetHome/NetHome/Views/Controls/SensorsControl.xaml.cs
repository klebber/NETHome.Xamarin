using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Models;
using NetHome.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorsControl : ContentView
    {
        public List<SensorValues> sensors;
        public List<SensorValues> Sensors { get => sensors; set => SetProperty(ref sensors, value); }
        public THSensorData Temp { get; set; }

        public SensorsControl(ICollection<DeviceModel> sensors)
        {
            InitializeComponent();
            Sensors = new List<SensorValues>();
            IEnumerable<DeviceModel> ths = sensors.Where(s => s.GetType().Name == "THSensorModel");
            Temp = ths.Count() switch
            {
                <= 0 => null,
                1 => new THSensorData(ths.First()),
                > 1 => new THSensorData(ths)
            };
            foreach (DeviceModel sensor in sensors.Except(ths))
            {
                (string value, string image) = GetSensorData(sensor);
                Sensors.Add(new SensorValues() { Name = sensor.Name, Value = value, ImageSource = image });
                OnPropertyChanged(nameof(Sensors));
            }
        }

        private (string, string) GetSensorData(DeviceModel sensor)
        {
            return sensor.GetType().Name switch
            {
                "DWSensorModel" => (((DWSensorModel)sensor).IsOpen ? "open" : "closed", ((DWSensorModel)sensor).Placement switch
                {
                    "Door" => "door.png",
                    "Window" => "window.png",
                    _ => "device_default.png"
                }),
                _ => ("", "")
            };
        }


        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}