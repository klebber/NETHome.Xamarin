using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IEnvironment _uiSettings;
        private readonly IWebSocketService _websocketService;
        private string username;
        private string password;
        private Command loginCommand;
        private Command registerCommand;
        private Command addressSetupCommand;

        public string Username { get => username; set => SetProperty(ref username, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public ICommand LoginCommand => loginCommand ??= new Command(async () => await LoginAsync());
        public ICommand RegisterCommand => registerCommand ??= new Command(async () => await RegisterAsync());
        public ICommand AddressSetupCommand => addressSetupCommand ??= new Command(async () => await AddressSetup());

        public LoginViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _uiSettings = DependencyService.Get<IEnvironment>();
            _websocketService = DependencyService.Get<IWebSocketService>();
        }

        private async Task LoginAsync()
        {
            if (IsWaiting) return;
            IsWaiting = true;

            var response = await _userService.Login(new LoginRequest()
            {
                Username = Username,
                Password = Password
            });

            IsWaiting = false;

            if (response.IsSuccessful)
                await GoToHomePageAsync();
            else if (response.ShowMessage)
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
        }

        private async Task ValidateExistingToken()
        {
            if (IsWaiting) return;
            IsWaiting = true;
            var response = await _userService.Validate();

            IsWaiting = false;

            if (response.IsSuccessful)
                await GoToHomePageAsync();
            else if (response.ShowMessage)
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
        }

        private async Task RegisterAsync()
        {
            if (IsWaiting) return;
            if (!UserDataManager.UriExists())
            {
                await Shell.Current.ShowPopupAsync(new Alert("Uri not found!", "Please enter a valid server uri.", "Ok", true));
                return;
            }
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        private async Task AddressSetup()
        {
            if (IsWaiting) return;
            string current = UserDataManager.GetUri();
            string result = await Shell.Current.ShowPopupAsync(new Propmpt(
                "Server Adress", "You can set url address of a server here:",
                "URL", current, "Save", true, true, keyboard: Keyboard.Url));
            if (result is null) return;
            if (string.IsNullOrWhiteSpace(result))
            {
                UserDataManager.RemoveUri();
                return;
            }
            else if (Uri.IsWellFormedUriString(result, UriKind.Absolute))
            {
                UserDataManager.SetUri(result);
            }
            else
            {
                await Shell.Current.ShowPopupAsync(new Alert(
                    "Incorrect url!",
                    "Server url you have entered is in incorrect format. Please try again.",
                    "Ok", true));
            }
        }

        private async Task GoToHomePageAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        internal async Task OnAppearing()
        {
            Username = string.Empty;
            Password = string.Empty;
            _uiSettings.SetStatusBarColor((Color)Application.Current.Resources["Primary"], false);
            _uiSettings.SetNavBarColor((Color)Application.Current.Resources["Primary"]);
            await ValidateExistingToken();
        }

    }
}
