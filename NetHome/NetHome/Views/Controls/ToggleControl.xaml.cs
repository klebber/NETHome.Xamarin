using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Exceptions;
using NetHome.Services;
using NetHome.Views.Popups;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToggleControl : ContentView
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IDeviceStateService _deviceStateService;

        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command(async () => await PerformQuickAction());

        private Command goToFullView;
        public ICommand GoToFullView => goToFullView ??= new Command(async () => await PerformGoToFullView());

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        public bool SwitchState { get => GetSwitchState(); set => SetSwitchState(device, value); }

        private DeviceModel device;
        public DeviceModel Device { get => device; set => SetProperty(ref device, value); }

        private ImageSource imageSource;
        public ImageSource ImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        public ToggleControl(DeviceModel device)
        {
            Device = device;
            ImageSource = GetImage();
            InitializeComponent();
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceStateService = DependencyService.Get<IDeviceStateService>();
            _deviceManager.DeviceChanged += StateChangedCallback;
        }

        private void StateChangedCallback(object sender, DeviceModel newValue)
        {
            if (newValue is null || newValue.Id != Device.Id)
                return;
            Device = newValue;
            SwitchState = GetSwitchState();
        }

        private async Task PerformQuickAction()
        {
            if (IsWaiting) return;
            IsWaiting = true;
            var tempState = SwitchState;
            SwitchState = !SwitchState;
            try
            {
                await _deviceStateService.ChangeDeviceState(Device);
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.DetailedMessage, "Ok", true));
                SwitchState = tempState;
            }
            finally
            {
                IsWaiting = false;
            }
        }

        private async Task PerformGoToFullView()
        {
        }

        private string GetImage()
        {
            return Device.GetType().Name switch
            {
                nameof(AirConditionerModel) => "air_conditioner.png",
                nameof(RGBLightModel) => "light_bulb.png",
                nameof(SmartSwitchModel) => Device.Type switch
                {
                    "Boiler" => "boiler.png",
                    "Light" => "light_bulb.png",
                    _ => "switch_default.png"
                },
                _ => "switch_default.png"
            };
        }

        private bool GetSwitchState()
        {
            return Device.GetType().Name switch
            {
                nameof(AirConditionerModel) => ((AirConditionerModel)Device).Ison,
                nameof(RGBLightModel) => ((RGBLightModel)Device).Ison,
                nameof(SmartSwitchModel) => ((SmartSwitchModel)Device).Ison,
                _ => throw new InvalidOperationException()
            };
        }

        private void SetSwitchState(DeviceModel device, bool newValue, [CallerMemberName] string propertyName = null)
        {
            SetIsonValue(device, newValue);
            OnPropertyChanged(propertyName);
        }

        private static void SetIsonValue(DeviceModel device, bool newValue)
        {
            switch (device.GetType().Name)
            {
                case nameof(AirConditionerModel):
                    ((AirConditionerModel)device).Ison = newValue;
                    break;
                case nameof(RGBLightModel):
                    ((RGBLightModel)device).Ison = newValue;
                    break;
                case nameof(SmartSwitchModel):
                    ((SmartSwitchModel)device).Ison = newValue;
                    break;
                default:
                    throw new InvalidOperationException();
            };
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}