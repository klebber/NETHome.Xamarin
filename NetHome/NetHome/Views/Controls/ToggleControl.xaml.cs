using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views.Popups;
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
        private bool isWaiting;
        private Command quickAction;
        private Command goToFullView;
        private ToggleControlState state;
        private ImageSource imageSource;
        public ICommand QuickAction => quickAction ??= new Command(async () => await PerformQuickAction());
        public ICommand GoToFullView => goToFullView ??= new Command(async () => await PerformGoToFullView());
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }
        internal ToggleControlState State { get => state; set => SetProperty(ref state, value); }
        public bool CurrentState { get => State.GetState(); set => SetState(value); }
        public DeviceModel Device { get => State.Device; set => SetDevice(value); }
        public ImageSource ImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        public ToggleControl(DeviceModel device)
        {
            State = CreateState(device);
            ImageSource = State.GetImage();
            InitializeComponent();
            BindingContext = this;
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceStateService = DependencyService.Get<IDeviceStateService>();
            _deviceManager.DeviceChanged += StateChangedCallback;
        }

        private void StateChangedCallback(object sender, DeviceModel newValue)
        {
            if (newValue is null || newValue.Id != Device.Id)
                return;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Device = newValue);

        }

        private async Task PerformQuickAction()
        {
            if (IsWaiting) return;
            IsWaiting = true;
            var tempState = CurrentState;
            CurrentState = !tempState;
            try
            {
                await _deviceStateService.ChangeDeviceState(state.Device);
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
                CurrentState = tempState;
            }
            finally
            {
                IsWaiting = false;
            }
        }

        private async Task PerformGoToFullView()
        {
            var route = State.GetPageName() + "?DeviceId=" + Device.Id;
            await Shell.Current.GoToAsync(route);
        }

        private ToggleControlState CreateState(DeviceModel device)
        {
            return device.GetType().Name switch
            {
                nameof(AirConditionerModel) => new AirConditionerState(device),
                nameof(RGBLightModel) => new RgbLightState(device),
                nameof(SmartSwitchModel) => new SmartSwitchState(device),
                _ => throw new InvalidOperationException()
            };
        }

        private void SetState(bool value)
        {
            State.SetState(value);
            OnPropertyChanged(nameof(CurrentState));
        }

        private void SetDevice(DeviceModel value)
        {
            state.Device = value;
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(CurrentState));
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}