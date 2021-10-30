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
    public partial class AirConditionerControl : ContentView
    {
        private Command quickAction;
        public ICommand QuickAction => quickAction ??= new Command(async () => await PerformQuickAction());

        private bool isWaiting;
        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        private AirConditionerModel device;
        public AirConditionerModel Device { get => device; set => SetProperty(ref device, value); }

        public AirConditionerControl(DeviceModel device)
        {
            InitializeComponent();
            this.device = (AirConditionerModel)device;
        }
        private async Task PerformQuickAction()
        {
            IsWaiting = true;
            await Task.Delay(500);
            AirConditionerModel d = device;
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