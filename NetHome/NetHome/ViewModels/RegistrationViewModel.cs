using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Services;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
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


        private Command registerCommand;

        public ICommand RegisterCommand => registerCommand ??= new Command(async () => await RegisterAsync());

        public RegistrationViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
        }
        private async Task RegisterAsync()
        {
            if (IsWaiting) return;
            IsWaiting = true;

            if (!await Validate())
            {
                IsWaiting = false;
                return;
            }

            var response = await _userService.Register(new RegisterRequest()
            {
                Username = Username,
                Password = Password,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Age = int.Parse(Age),
                Gender = Gender
            });

            IsWaiting = false;

            if (response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Registration successful!", "You can now use your credentials to login.", "Ok", true));
                await Shell.Current.GoToAsync("..");
            }
            else if (response.ShowMessage)
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
            }
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
    }
}
