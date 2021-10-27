using NetHome.Services;
using Xamarin.Forms;

namespace NetHome
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<IUserService, UserService>();
            DependencyService.Register<ISignalRConnection, SignalRConnection>();
            DependencyService.Register<IDeviceService, DeviceService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
