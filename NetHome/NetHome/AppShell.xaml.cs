using NetHome.Views;
using NetHome.Views.DevicePages;
using Xamarin.Forms;

namespace NetHome
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(AirConditionerPage), typeof(AirConditionerPage));
            Routing.RegisterRoute(nameof(RgbLightPage), typeof(RgbLightPage));
            Routing.RegisterRoute(nameof(RollerShutterPage), typeof(RollerShutterPage));
            Routing.RegisterRoute(nameof(SmartSwitchPage), typeof(SmartSwitchPage));
            Routing.RegisterRoute(nameof(AccountInfoPage), typeof(AccountInfoPage));
            Routing.RegisterRoute(nameof(DeviceSettingsPage), typeof(DeviceSettingsPage));
            Routing.RegisterRoute(nameof(DeviceInfoPage), typeof(DeviceInfoPage));
            Routing.RegisterRoute(nameof(UserSettingsPage), typeof(UserSettingsPage));
        }
    }
}
