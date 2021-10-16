using NetHome.Helpers;
using NetHome.Services;
using NetHome.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        private readonly IEnvironment uiSettings;
        private bool xxx = true;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HomeViewModel();
            uiSettings = DependencyService.Get<IEnvironment>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            uiSettings.SetStatusBarColor((Color)Application.Current.Resources["Primary"], false);
            uiSettings.SetNavBarColor((Color)Application.Current.Resources["TabBarBackground"]);
            MessagingCenter.Subscribe<object, bool>(this, "Switched", (sender, ison) =>
            {
                xxx = ison;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (xxx)
                    {
                        LSwitch.Text = "ON";
                        LSwitch.BackgroundColor = Color.Accent;
                    }
                    else
                    {
                        LSwitch.Text = "OFF";
                        LSwitch.BackgroundColor = Color.Gray;
                    }
                });
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<object, bool>(this, "Switched");
            base.OnDisappearing();
        }

        public async void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            await _viewModel.SwitchAsync(e.Value);
        }

        protected override bool OnBackButtonPressed()
        {
            _viewModel.Disconnect();
            return base.OnBackButtonPressed();
        }

        private async void LSwitch_Clicked(object sender, EventArgs e)
        {
            await _viewModel.SwitchAsync(!xxx);
        }
    }
}