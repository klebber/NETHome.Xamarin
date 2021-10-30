using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Models;
using NetHome.Views.Popups;
using System;
using System.Collections;
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
        private IEnumerable<DeviceModel> thsensors;

        private THSensorData temp;
        public THSensorData Temp { get => temp; set => SetProperty(ref temp, value); }

        public SensorsControl(ICollection<DeviceModel> sensors)
        {
            InitializeComponent();
            thsensors = sensors.Where(s => s.GetType().Name == nameof(THSensorModel));
            LoadTemperatureAndHumidityValues();
            foreach (DeviceModel sensor in sensors)
            {
                Stack.Children.Add(new SensorView(sensor));
            }
        }

        private void LoadTemperatureAndHumidityValues()
        {
            Temp = thsensors.Count() == 0 ? null : new THSensorData(thsensors);
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}