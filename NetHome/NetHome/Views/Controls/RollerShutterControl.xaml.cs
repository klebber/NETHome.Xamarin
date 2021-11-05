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
    public partial class RollerShutterControl : ContentView
    {
        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command<int>(async (percent) => await PerformQuickAction(percent));

        private Command goToFullView;
        public ICommand GoToFullView => goToFullView ??= new Command(async () => await PerformGoToFullView());

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        private RollerShutterModel rollerShutter;
        public RollerShutterModel RollerShutter { get => rollerShutter; set => SetProperty(ref rollerShutter, value); }

        public RollerShutterControl(DeviceModel device)
        {
            InitializeComponent();
            RollerShutter = (RollerShutterModel)device;
        }

        private async Task PerformQuickAction(int percent)
        {
            if (isWaiting) return;
            IsWaiting = true;
            await Task.Delay(500);
            RollerShutterModel d = RollerShutter;
            d.CurrentPercentage = percent;
            RollerShutter = d;
            IsWaiting = false;
        }

        private async Task PerformGoToFullView()
        {
            await Shell.Current.ShowPopupAsync(new Alert("123", RollerShutter.CurrentPercentage.ToString(), "Nope", true));
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}