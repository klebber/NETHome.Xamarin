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
    public partial class RollerShutterControl : ContentView
    {
        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command<int>(async (percent) => await PerformQuickAction(percent));

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        private RollerShutterModel device;
        public RollerShutterModel Device { get => device; set => SetProperty(ref device, value); }

        public RollerShutterControl(DeviceModel device)
        {
            InitializeComponent();
            this.device = (RollerShutterModel)device;
        }

        private async Task PerformQuickAction(int percent)
        {
            IsWaiting = true;
            await Task.Delay(500);
            RollerShutterModel d = device;
            d.CurrentPercentage = percent;
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