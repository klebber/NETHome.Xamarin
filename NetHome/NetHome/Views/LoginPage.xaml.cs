using NetHome.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();
            UsernameEntry.ReturnCommand = new Command(() => PasswordEntry.Focus());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }

    }
}