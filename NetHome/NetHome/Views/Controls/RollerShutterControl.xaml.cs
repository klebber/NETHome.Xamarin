using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Exceptions;
using NetHome.Helpers;
using NetHome.Services;
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
        private readonly IDeviceManager _deviceManager;
        private readonly IDeviceStateService _deviceStateService;

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
            BindingContext = this;
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceStateService = DependencyService.Get<IDeviceStateService>();
            _deviceManager.DeviceChanged += StateChangedCallback;
            RollerShutter = (RollerShutterModel)device;
        }

        private void StateChangedCallback(object sender, DeviceModel newValue)
        {
            if (newValue is null || newValue.Id != RollerShutter.Id) 
                return;
            Device.BeginInvokeOnMainThread(() =>
            {
                RollerShutter = (RollerShutterModel)newValue;
            });
        }

        private async Task PerformQuickAction(int percent)
        {
            if (isWaiting) return;
            IsWaiting = true;
            var tempPercent = RollerShutter.CurrentPercentage;
            RollerShutter.CurrentPercentage = percent;
            try
            {
                await _deviceStateService.ChangeDeviceState(RollerShutter);
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
                RollerShutter.CurrentPercentage = tempPercent;
                OnPropertyChanged(nameof(RollerShutter));
            }
            finally
            {
                IsWaiting = false;
            }
        }

        private async Task PerformGoToFullView()
        {
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}