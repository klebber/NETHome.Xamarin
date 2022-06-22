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
            IsWaiting = true;
            if (!Preferences.ContainsKey("ServerAddress") || string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty)))
            {
                IsWaiting = false;
                await Shell.Current.ShowPopupAsync(new Alert("No server url!", "You must set an address of the server first.", "Ok", true));
                return;
            }
            LoginRequest loginRequest = new()
            {
                Username = Username,
                Password = Password
            };

            try
            {
                await _userService.Login(loginRequest);
                await GoToHomePageAsync();
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            catch (ServerCommunicationException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            finally
            {
                IsWaiting = false;
            }
        }

        private async Task RegisterAsync()
        {
            if (IsWaiting) return;
            if (!Preferences.ContainsKey("ServerAddress") || string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty)))
            {
                await Shell.Current.ShowPopupAsync(new Alert("Server address not set!", "You must set an address of the server first.", "Ok", true));
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

        private async Task ValidateExistingToken()
        {
            IsWaiting = true;
            try
            {
                await _userService.Validate();
                await GoToHomePageAsync();
            }
            catch (BadResponseException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
                UserDataManager.ClearUserData();
            }
            catch (ServerCommunicationException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            catch (ServerAuthorizationException)
            {
                UserDataManager.ClearUserData();
                await Shell.Current.ShowPopupAsync(new Alert("Authorization error", "Your token has expired. Please login again.", "Ok", true));
            }
            finally
            {
                IsWaiting = false;
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
            if (await SecureStorage.GetAsync("AuthorizationToken") is not null)
                await ValidateExistingToken();
        }

    }
}
