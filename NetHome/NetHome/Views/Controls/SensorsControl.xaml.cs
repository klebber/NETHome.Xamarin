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
        private readonly IDeviceManager _deviceManager;

        private string temp;
        public string Temp { get => temp; set => SetProperty(ref temp, value); }

        private string hum;
        public string Hum { get => hum; set => SetProperty(ref hum, value); }


        public SensorsControl()
        {
            InitializeComponent();
            BindingContext = this;
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceManager.DeviceChanged += StateChangedCallback;
            SetValues();
            RenderChildren();
        }

        private void StateChangedCallback(object sender, DeviceModel e)
        {
            if (e is null) return;
            if (e.Type == "Sensor")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetValues();
                    RenderChildren();
                });
            }

        }

        private void SetValues()
        {
            List<DeviceModel> thsensors = _deviceManager.GetSensors().Where(s => s.GetType().Name == nameof(THSensorModel)).ToList();
            if (thsensors.Count > 0)
            {
                Temp = thsensors.Select(dm => ((THSensorModel)dm).Temperature).ToList().Average().ToString();
                Hum = thsensors.Select(dm => ((THSensorModel)dm).Humidity).ToList().Average().ToString();
            }
            else
            {
                Temp = null;
                Hum = null;
            }
        }

        private void RenderChildren()
        {
            var sensors = _deviceManager.GetSensors().OrderBy(s => s.GetType().Name).ToList();
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