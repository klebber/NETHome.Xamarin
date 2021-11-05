using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views.Controls;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ISignalRConnection _signalRConnection;
        private readonly IEnvironment _uiSettings;
        private readonly IDeviceService _deviceService;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<View> deviceControls;
        public ObservableCollection<View> DeviceControls { get => deviceControls; set => SetProperty(ref deviceControls, value); }

        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

        private Command onRefreshed;
        public ICommand OnRefreshed => onRefreshed ??= new Command(async () => await Task.Run(async () => await PopulateDeviceControls()));

        public HomeViewModel()
        {
            DeviceControls = new ObservableCollection<View>();
            BindingBase.EnableCollectionSynchronization(DeviceControls, null, ObservableCollectionCallback);
            _signalRConnection = DependencyService.Get<ISignalRConnection>();
            _uiSettings = DependencyService.Get<IEnvironment>();
            _deviceService = DependencyService.Get<IDeviceService>();
        }

        private void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }

        internal void OnAppearing()
        {
            _uiSettings.SetStatusBarColor((Color)Application.Current.Resources["Secondary"], false);
            _uiSettings.SetNavBarColor((Color)Application.Current.Resources["TabBarBackground"]);
        }

        internal void OnDisappearing()
        {
        }

        internal void DisconnectSignalR()
        {
            _signalRConnection.Disconnect();
        }

        internal void OnBackButtonPressed()
        {
            DisconnectSignalR();
        }

        private async Task PopulateDeviceControls()
        {
            ICollection<DeviceModel> devices = await _deviceService.GetAll();

            MainThread.BeginInvokeOnMainThread(() => DeviceControls = new ObservableCollection<View>());

            ICollection<DeviceModel> sensors = devices.Where(d => d.Type == "Sensor").ToList();
            devices = devices.Except(sensors).ToList();

            MainThread.BeginInvokeOnMainThread(() => DeviceControls.Add(new SensorsControl(sensors)));

            foreach (DeviceModel device in devices)
            {
                View view = device.GetType().Name switch
                {
                    nameof(AirConditionerModel) => new ToggleControl(device),
                    nameof(RGBLightModel) => new ToggleControl(device),
                    nameof(RollerShutterModel) => new RollerShutterControl(device),
                    nameof(SmartSwitchModel) => new ToggleControl(device),
                    _ => new DefaultControl(device)
                };
                MainThread.BeginInvokeOnMainThread(() => DeviceControls.Add(view));
            }
            MainThread.BeginInvokeOnMainThread(() => IsRefreshing = false);
        }


        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

    }
}
