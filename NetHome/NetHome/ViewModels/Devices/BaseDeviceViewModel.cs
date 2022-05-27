using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Services;
using Xamarin.Forms;

namespace NetHome.ViewModels.Devices
{
    public abstract class BaseDeviceViewModel : BindableObject
    {
        protected readonly IDeviceManager _deviceManager;
        protected readonly IDeviceStateService _deviceStateService;
        private bool isWaiting;
        private ImageSource image;
        private DeviceModel deviceClone;
        private Command changeState;

        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }
        public ImageSource Image { get => image; set => SetProperty(ref image, value); }
        public DeviceModel DeviceClone { get => deviceClone; set => SetProperty(ref deviceClone, value); }
        public ICommand ChangeState => changeState ??= new Command(async () => await PerformChangeState());


        public BaseDeviceViewModel()
        {
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceStateService = DependencyService.Get<IDeviceStateService>();
            _deviceManager.DeviceChanged += StateChangedCallback;
        }

        private async Task PerformChangeState()
        {
            if (IsWaiting) return;
            IsWaiting = true;
            await _deviceStateService.ChangeDeviceState(deviceClone);
            IsWaiting = false;
        }

        protected void StateChangedCallback(object sender, DeviceModel newValue)
        {
            if (newValue is null || newValue.Id != DeviceClone.Id)
                return;
            Device.BeginInvokeOnMainThread(() => DeviceClone = newValue);
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
