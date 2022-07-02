using System;
using System.Collections.Generic;
using System.Text;
using NetHome.Common;
using NetHome.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections;
using System.Threading.Tasks;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using NetHome.Views;
using NetHome.Views.DevicePages;

namespace NetHome.ViewModels
{
    public class DeviceSettingsViewModel : BaseViewModel
    {
        private readonly IDeviceManager _deviceManager;
        private Command onRefreshed;
        private ObservableCollection<DeviceModel> devices;
        private Command<int> goToDeviceInfo;
        private Command addCommand;

        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public ICommand OnRefreshed => onRefreshed ??= new Command(PerformOnRefreshed);
        public ICommand GoToDeviceInfo => goToDeviceInfo ??= new Command<int>(async (param) => await PerformGoToDeviceInfo(param));
        public ICommand AddCommand => addCommand ??= new Command(async () => await Add());

        public DeviceSettingsViewModel()
        {
            Devices = new ObservableCollection<DeviceModel>();
            BindingBase.EnableCollectionSynchronization(Devices, null, ObservableCollectionCallback);
            _deviceManager = DependencyService.Get<IDeviceManager>();
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
            SetDevices();
        }


        private void SetDevices()
        {
            Devices = new ObservableCollection<DeviceModel>(_deviceManager.GetAllDevices());
            IsWaiting = false;
        }

        private void PerformOnRefreshed()
        {
            SetDevices();
        }

        private async Task PerformGoToDeviceInfo(int param)
        {
            var route = $"{nameof(DeviceInfoPage)}?{nameof(DeviceInfoPage.DeviceId)}={param}";
            await Shell.Current.GoToAsync(route);
        }

        private async Task Add()
        {
            var route = $"{nameof(DeviceInfoPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
