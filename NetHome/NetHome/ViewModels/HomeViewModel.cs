using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views.Controls;
using NetHome.Views.Popups;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NetHome.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly IEnvironment _uiSettings;
        private readonly IDeviceService _deviceService;

        public event PropertyChangedEventHandler PropertyChanged;

        private SensorsControl sensorsControl;
        public SensorsControl SensorsControl { get => sensorsControl; set => SetProperty(ref sensorsControl, value); }

        private ObservableCollection<View> deviceControls = new();
        public ObservableCollection<View> DeviceControls { get => deviceControls; set => SetProperty(ref deviceControls, value); }

        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

        private Command onRefreshed;
        public ICommand OnRefreshed => onRefreshed ??= new Command(async () => await Task.Run(async () => await PopulateDeviceControls()));

        public HomeViewModel()
        {
            BindingBase.EnableCollectionSynchronization(DeviceControls, null, ObservableCollectionCallback);
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

        internal void OnBackButtonPressed()
        {
        }

        private async Task PopulateDeviceControls()
        {
            try
            {
                await _deviceService.FetchAllDevices();
                ICollection<DeviceModel> devices = DeviceManager.GetNonSensorDevices();
                ICollection<DeviceModel> sensors = DeviceManager.GetSensors();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    SensorsControl = new SensorsControl(sensors);
                    DeviceControls.Clear();
                    foreach (DeviceModel device in devices)
                    {
                        DeviceControls.Add(await ViewHelper.GetViewForDevice(device));
                    }
                    IsRefreshing = false;
                });
            }
            catch (ServerCommunicationException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.DetailedMessage, "Ok", true));
            }
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
