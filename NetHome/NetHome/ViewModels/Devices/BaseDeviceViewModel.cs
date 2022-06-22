using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace NetHome.ViewModels.Devices
{
    public abstract class BaseDeviceViewModel : BaseViewModel
    {
        protected readonly IDeviceManager _deviceManager;
        protected readonly IDeviceStateService _deviceStateService;
        private ImageSource image;
        private DeviceModel deviceClone;
        private Command baseChangeState;

        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public DeviceModel DeviceClone { get => deviceClone; set => SetProperty(ref deviceClone, value); }
        public ICommand BaseStateChangeCommand => baseChangeState ??= new Command(async () => await PerformChangeState());


        public BaseDeviceViewModel()
        {
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceStateService = DependencyService.Get<IDeviceStateService>();
            _deviceManager.DeviceChanged += StateChangedCallback;
        }

        protected async Task PerformChangeState(Action action = null)
        {
            if (IsWaiting) return;
            IsWaiting = true;
            try
            {
                if (action is not null) action();
                await _deviceStateService.ChangeDeviceState(deviceClone);
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
                FetchDeviceClone();
            }
            finally
            {
                IsWaiting = false;
            }
        }


        public void OnAppearing(int deviceId)
        {
            FetchDeviceClone(deviceId);
            SetImage();
            IsWaiting = false;
        }

        private void FetchDeviceClone()
        {
            DeviceClone = _deviceManager.GetDeviceById(DeviceClone.Id).Clone();
            OnDeviceChanged();
        }

        private void FetchDeviceClone(int deviceId)
        {
            DeviceClone = _deviceManager.GetDeviceById(deviceId).Clone();
            OnDeviceChanged();
        }

        protected abstract void OnDeviceChanged();

        protected abstract void SetImage();

        protected void StateChangedCallback(object sender, DeviceModel newValue)
        {
            if (newValue is null || newValue.Id != DeviceClone.Id)
                return;
            Device.BeginInvokeOnMainThread(() => FetchDeviceClone());
        }
    }
}
