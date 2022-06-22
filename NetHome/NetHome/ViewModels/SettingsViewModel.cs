using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class SettingsViewModel
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
