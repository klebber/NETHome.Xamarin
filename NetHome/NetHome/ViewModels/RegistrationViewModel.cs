using NetHome.Common;
using NetHome.Common.Models;
using NetHome.Helpers;
using NetHome.Services;
using NetHome.Views;
using NetHome.Views.Popups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }

        private bool isLoading = false;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        public event PropertyChangedEventHandler PropertyChanged;

        private Command registerCommand;

        public ICommand RegisterCommand => registerCommand ??= new Command(async () => await RegisterAsync());

        public RegistrationViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
        }
        private async Task RegisterAsync()
        {
            IsLoading = true;
            if (!await Validate())
            {
                IsLoading = false;
                return;
            }
            RegisterRequest reg = new()
            {
                Username = Username,
                Password = Password,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Age = int.Parse(Age),
                Gender = Gender
            };
            try
            {
                await _userService.Register(reg);
                await Shell.Current.ShowPopupAsync(new Alert("Registration successful!", "You can now use your credentials to login.", "Ok", true));
                await Shell.Current.GoToAsync("..");
            }
            catch (ServerCommunicationException e)
            {
                await Shell.Current.ShowPopupAsync(new Alert(e.Reason, e.Message, "Ok", true));
            }
            IsLoading = false;
        }

        private async Task<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 4 || Username.Length > 16)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Invalid username!", "Ok", true));
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 4 || Password.Length > 16)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Invalid password!", "Ok", true));
                return false;
            }
            if (Password != PasswordRepeat)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Passwords should match!", "Ok", true));
                return false;
            }
            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email))
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Invalid email!", "Ok", true));
                return false;
            }
            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length < 2 || FirstName.Length > 32)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Invalid frist name!", "Ok", true));
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastName) || LastName.Length < 2 || LastName.Length > 32)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Invalid last name!", "Ok", true));
                return false;
            }
            if (string.IsNullOrWhiteSpace(Age) || !int.TryParse(Age, out int age) || age < 13 || age > 100)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Input error!", "Invalid last name!", "Ok", true));
                return false;
            }
            return true;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                MailAddress addr = new(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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
