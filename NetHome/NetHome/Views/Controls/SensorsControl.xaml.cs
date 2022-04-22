using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Helpers;
using NetHome.Services;
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
        private List<DeviceModel> sensors;
        private readonly IDeviceManager _deviceManager;

        private double temp;
        public double Temp { get => temp; set => SetProperty(ref temp, value); }

        private double hum;
        public double Hum { get => hum; set => SetProperty(ref hum, value); }

        private readonly IDeviceStateService _deviceStateService;

        public SensorsControl(ICollection<DeviceModel> sensors)
        {
            InitializeComponent();
            _deviceStateService = DependencyService.Get<IDeviceStateService>();
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceManager.DeviceChanged += StateChangedCallback;
            RenderComponents(sensors);
        }

        private void StateChangedCallback(object sender, DeviceModel e)
        {
            if (e is null) return;
            if (sensors.Any(s => s.Id == e.Id))
            {
                sensors.Remove(sensors.Single(s => s.Id == e.Id));
                sensors.Add(e);
                RenderComponents(sensors);
            }

        }

        private void RenderComponents(ICollection<DeviceModel> newValue)
        {
            sensors = newValue.OrderBy(s => s.GetType().Name).ToList();
            List<DeviceModel> thsensors = sensors.Where(s => s.GetType().Name == nameof(THSensorModel)).ToList();
            Temp = thsensors.Select(dm => ((THSensorModel)dm).Temperature).ToList().Average();
            Hum = thsensors.Select(dm => ((THSensorModel)dm).Humidity).ToList().Average();
            Stack.Children.Clear();
            foreach (DeviceModel sensor in sensors)
            {
                Stack.Children.Add(new SensorView(sensor));
            }
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}