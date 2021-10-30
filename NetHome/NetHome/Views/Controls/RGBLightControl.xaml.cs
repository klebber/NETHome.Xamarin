using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RGBLightControl : ContentView
    {
        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command(async () => await PerformQuickAction());

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        private RGBLightModel device;
        public RGBLightModel Device { get => device; set => SetProperty(ref device, value); }

        public RGBLightControl(DeviceModel device)
        {
            InitializeComponent();
            this.device = (RGBLightModel)device;
        }
        private async Task PerformQuickAction()
        {
            if (isWaiting) return;
            IsWaiting = true;
            await Task.Delay(500);
            RGBLightModel d = device;
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