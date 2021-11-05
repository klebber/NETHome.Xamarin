using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Views.Popups;
using System;
using System.Collections.Generic;
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
    public partial class ToggleControl : ContentView
    {
        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command(async () => await PerformQuickAction());

        private Command goToFullView;
        public ICommand GoToFullView => goToFullView ??= new Command(async () => await PerformGoToFullView());

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        private bool switchState;
        public bool SwitchState { get => switchState = GetSwitchState(); set => SetSwitchState(ref switchState, value); }

        private DeviceModel device;
        public DeviceModel Device { get => device; set => SetProperty(ref device, value); }

        private ImageSource imageSource;
        public ImageSource ImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        public ToggleControl(DeviceModel device)
        {
            Device = device;
            ImageSource = GetImage();
            InitializeComponent();
        }

        private async Task PerformQuickAction()
        {
            if (isWaiting) return;
            IsWaiting = true;
            await Task.Delay(500);
            var x = SwitchState;
            SwitchState = !SwitchState;
            IsWaiting = false;
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

        private void SetSwitchState(ref bool field, bool newValue, [CallerMemberName] string propertyName = null)
        {
            switch (Device.GetType().Name)
            {
                case nameof(AirConditionerModel):
                    ((AirConditionerModel)Device).Ison = newValue;
                    break;
                case nameof(RGBLightModel):
                    ((RGBLightModel)Device).Ison = newValue;
                    break;
                case nameof(SmartSwitchModel):
                    ((SmartSwitchModel)Device).Ison = newValue;
                    break;
                default:
                    throw new InvalidOperationException();
            };
            SetProperty(ref field, newValue, propertyName);
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}