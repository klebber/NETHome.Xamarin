using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IDeviceManager _deviceManager;

        private Command logoutCommand;
        public ICommand LogoutCommand => logoutCommand ??= new Command(async () => await LogoutAsync());

        public SettingsViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _deviceManager = DependencyService.Get<IDeviceManager>();
        }

        private async Task LogoutAsync()
        {
            _deviceManager.PerformAction(Actions.Logout);
            var logout = _userService.Logout();
            var view = Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            await Task.WhenAll(logout, view);
        }
    }
}
