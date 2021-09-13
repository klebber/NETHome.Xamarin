using NetHome.Helpers;
using NetHome.Models.User;
using NetHome.Services;
using NetHome.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        public LoginViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _uiSettings = DependencyService.Get<IEnvironment>();
        }

        internal async Task LoginAsync()
        {
            IsLoading = true;
            LoginModel loginModel = new LoginModel()
            {
                Username = Username,
                Password = Password
            };

            if (await _userService.Login(loginModel))
            {
                await GoToHomePage();
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
                await Shell.Current.DisplayAlert("Login error!", "Incorrect Username and/or Password.", "Ok");
            }
        }

        internal async Task ValidateExistingToken()
        {
            IsLoading = true;
            if (await _userService.Validate())
            {
                await GoToHomePage();
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

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
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
