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
        private readonly IServerConnection _serverConnection;

        private Command logoutCommand;
        public ICommand LogoutCommand => logoutCommand ??= new Command(async () => await LogoutAsync());

        public SettingsViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _serverConnection = DependencyService.Get<IServerConnection>();
        }

        private async Task LogoutAsync()
        {
            _userService.ClearUserData();
            await _serverConnection.Disconnect();
            SecureStorage.Remove("AuthorizationToken");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
