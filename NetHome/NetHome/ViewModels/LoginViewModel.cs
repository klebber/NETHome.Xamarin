using NetHome.Common.Models;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using NetHome.Views.Popups;
using NetHome.Common;

namespace NetHome.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly IEnvironment _uiSettings;

        private string username;
        public string Username { get => username; set => SetProperty(ref username, value); }

        private string password;
        public string Password { get => password; set => SetProperty(ref password, value); }

        private bool isLoading = false;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public event PropertyChangedEventHandler PropertyChanged;

        private Command loginCommand;

        public ICommand LoginCommand => loginCommand ??= new Command(async () => await LoginAsync());

        private Command registerCommand;

        public ICommand RegisterCommand => registerCommand ??= new Command(async () => await RegisterAsync());

        private Command addressSetupCommand;
        public ICommand AddressSetupCommand => addressSetupCommand ??= new Command(async () => await AddressSetup());

        public LoginViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _uiSettings = DependencyService.Get<IEnvironment>();
        }

        private async Task LoginAsync()
        {
            IsLoading = true;
            if (!Preferences.ContainsKey("ServerAddress") || string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty)))
            {
                IsLoading = false;
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
                await GoToHomePage();
            }
            catch (ServerException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            catch (OperationCanceledException)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Server unreachable!",
                    "Http request has timed out. Check if server address is correct and if server is running.",
                    "Ok",
                    true));
            }
            IsLoading = false;
        }

        private async Task RegisterAsync()
        {
            if (IsLoading) return;
            if (!Preferences.ContainsKey("ServerAddress") || string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty)))
            {
                await Shell.Current.ShowPopupAsync(new Alert("Server address not set!", "You must set an address of the server first.", "Ok", true));
                return;
            }
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        private async Task AddressSetup()
        {
            if (IsLoading) return;
            string current = Preferences.Get("ServerAddress", string.Empty);
            string result = await Shell.Current.ShowPopupAsync(new Propmpt(
                "Server Adress", "You can set url address of a server here:",
                "URL", current, "Save", true, true, keyboard: Keyboard.Url));
            if (result is null) return;
            if (string.IsNullOrWhiteSpace(result))
            {
                Preferences.Remove("ServerAddress");
                return;
            }
            else if (Uri.IsWellFormedUriString(result, UriKind.Absolute))
            {
                Preferences.Set("ServerAddress", result);
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
            if (await SecureStorage.GetAsync("AuthorizationToken") is null) return;
            IsLoading = true;
            try
            {
                await _userService.Validate();
                await GoToHomePage();
            }
            catch (ServerException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            catch (OperationCanceledException)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Server unreachable!",
                    "Http request has timed out. Check if server address is correct and if server is running.",
                    "Ok",
                    true));
            }
            //finally // zasto ovo daje exception?
            //{
            //    _userService.ClearUserData();
            //}
            IsLoading = false;
        }

        private async Task GoToHomePage()
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

        private bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

    }
}
