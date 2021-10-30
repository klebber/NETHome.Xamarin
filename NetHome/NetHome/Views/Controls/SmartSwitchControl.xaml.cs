using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmartSwitchControl : ContentView
    {
        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command(async () => await PerformQuickAction());

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        private SmartSwitchModel device;
        public SmartSwitchModel Device { get => device; set => SetProperty(ref device, value); }

        private ImageSource imageSource;
        public ImageSource ImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        public SmartSwitchControl(DeviceModel device)
        {
            InitializeComponent();
            this.device = (SmartSwitchModel)device;
            ImageSource = Device.Type switch
            {
                "Boiler" => "boiler.png",
                "Light" => "light_bulb.png",
                _ => "switch_default.png"
            };
        }
        private async Task PerformQuickAction()
        {
            IsWaiting = true;
            await Task.Delay(500);
            SmartSwitchModel d = device;
            d.Ison = !d.Ison;
            Device = d;
            IsWaiting = false;
        }
        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }

    }
}