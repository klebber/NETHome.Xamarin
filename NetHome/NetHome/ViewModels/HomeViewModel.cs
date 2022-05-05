using NetHome.Common.Models;
using NetHome.Exceptions;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views.Controls;
using NetHome.Views.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly IEnvironment _uiSettings;
        private readonly IDeviceService _deviceService;
        private readonly IDeviceManager _deviceManager;
        private readonly IWebSocketService _websocketService;

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
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _websocketService = DependencyService.Get<IWebSocketService>();
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
            _websocketService.CloseAsync();
        }

        private async Task PopulateDeviceControls()
        {
            try
            {
                await _deviceService.FetchAllDevices();
                ICollection<DeviceModel> devices = _deviceManager.GetNonSensorDevices();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    SensorsControl = new SensorsControl();
                    DeviceControls.Clear();
                    foreach (DeviceModel device in devices)
                    {
                        DeviceControls.Add(await ViewHelper.GetViewForDevice(device));
                    }
                    IsRefreshing = false;
                });
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            catch (ServerCommunicationException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            catch (ServerAuthorizationException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Authorization error", e.Message, "Ok", true));
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
