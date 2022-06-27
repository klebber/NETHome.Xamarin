using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NetHome.Common;
using NetHome.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorsControl : ContentView
    {
        private readonly IDeviceManager _deviceManager;
        private string temp;
        private string hum;
        private bool hasDevices;

        public string Temp { get => temp; set => SetProperty(ref temp, value); }
        public string Hum { get => hum; set => SetProperty(ref hum, value); }
        public bool HasDevices { get => hasDevices; set { SetProperty(ref hasDevices, value); OnPropertyChanged(nameof(HasDevices)); } }


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
            HasDevices = _deviceManager.GetSensors().Count != 0;
            if (!HasDevices)
                return;
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