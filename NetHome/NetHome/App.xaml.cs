using NetHome.Services;
using Xamarin.Forms;

namespace NetHome
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.RegisterSingleton<IDeviceManager>(new DeviceManager());
            DependencyService.Register<IDeviceStateService, DeviceStateService>();
            DependencyService.RegisterSingleton<IWebSocketService>(new WebSocketService());
            DependencyService.Register<IUserService, UserService>();
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
