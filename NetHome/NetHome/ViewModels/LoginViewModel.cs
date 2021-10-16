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

namespace NetHome.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly IEnvironment _uiSettings;
        private readonly IServerConnection _serverConnection;

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
            _serverConnection = DependencyService.Get<IServerConnection>();
        }

        private async Task LoginAsync()
        {
            if (!Preferences.ContainsKey("ServerAddress") || string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty)))
            {
                await Shell.Current.ShowPopupAsync(new CustomAlert("No server url!", "You must set an address of the server first.", "Ok", true));
                return;
            }
            IsLoading = true;
            var loginModel = new LoginModel()
            {
                Username = Username,
                Password = Password
            };

            if (await _userService.Login(loginModel))
            {
                if(await _serverConnection.Connect())
                {
                    await GoToHomePage();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Connection error!", "Could not connect to hub!", "Ok");
                }
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
                await Shell.Current.DisplayAlert("Login error!", "Incorrect Username and/or Password!", "Ok");
            }
        }

        private async Task RegisterAsync()
        {
            if (!Preferences.ContainsKey("ServerAddress") || string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty)))
            {
                await Shell.Current.DisplayAlert("Server address not set!", "You must set an address of the server first.", "Ok");
                return;
            }
            if (isLoading) return;
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        private async Task AddressSetup()
        {
            if (isLoading) return;
            string current = Preferences.Get("ServerAddress", string.Empty);
            string result = await Shell.Current.ShowPopupAsync(new CustomPropmpt(
                "Server Adress", "You can set url address of a server here:",
                "URL", current, "Save", true, true));
            if (result == null) return;
            if (string.IsNullOrWhiteSpace(result))
                Preferences.Remove("ServerAddress");
            else
                Preferences.Set("ServerAddress", result); //TODO proveri da li je validan url
        }

        private async Task ValidateExistingToken()
        {
            IsLoading = true;
            if (await _userService.Validate())
            {
                if (await _serverConnection.Connect())
                {
                    await GoToHomePage();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Connection error!", "Could not connect to hub!", "Ok");
                }
            }
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
