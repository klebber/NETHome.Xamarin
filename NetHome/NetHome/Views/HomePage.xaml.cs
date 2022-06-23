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
        public HomePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HomeViewModel();
            _viewModel.IsWaiting = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            _viewModel.OnBackButtonPressed();
            return base.OnBackButtonPressed();
        }
    }
}