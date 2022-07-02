using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views;
using NetHome.Views.DevicePages;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IDeviceManager _deviceManager;
        private Command logoutCommand;
        private Command goToAccountInfoPage;
        private Command goToDeviceSettingsPage;
        private Command goToUserSettingsPage;
        private UserModel user;

        public UserModel User { get => user; set { SetProperty(ref user, value); OnPropertyChanged(nameof(IsAdmin)); OnPropertyChanged(nameof(IsOwner)); } }
        public ICommand LogoutCommand => logoutCommand ??= new Command(async () => await LogoutAsync());
        public ICommand GoToAccountInfoPage => goToAccountInfoPage ??= new Command(async () => await PerformGoToAccountInfoPage());
        public ICommand GoToDeviceSettingsPage => goToDeviceSettingsPage ??= new Command(async () => await PerformGoToDeviceSettingsPage());
        public ICommand GoToUserSettingsPage => goToUserSettingsPage ??= new Command(async () => await PerformGoToUserSettingsPage());
        public bool IsAdmin { get => UserDataManager.GetUserData().Roles.Contains("Admin") || IsOwner; }
        public bool IsOwner { get => UserDataManager.GetUserData().Roles.Contains("Owner"); }

        public SettingsViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _deviceManager = DependencyService.Get<IDeviceManager>();
        }

        public void OnAppearing()
        {
            User = UserDataManager.GetUserData();
    }

        private async Task LogoutAsync()
        {
            _deviceManager.PerformAction(Actions.Logout);
            var logout = _userService.Logout();
            var view = Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            await Task.WhenAll(logout, view);
        }

        private async Task PerformGoToAccountInfoPage()
        {
            var route = $"{nameof(AccountInfoPage)}?{nameof(AccountInfoPage.UserId)}={User.Id}";
            await Shell.Current.GoToAsync(route);
        }

        private async Task PerformGoToDeviceSettingsPage()
        {
            var route = $"{nameof(DeviceSettingsPage)}";
            await Shell.Current.GoToAsync(route);
        }

        private async Task PerformGoToUserSettingsPage()
        {
        }
    }
}
