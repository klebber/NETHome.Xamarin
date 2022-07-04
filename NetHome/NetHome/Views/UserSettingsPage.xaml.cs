using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetHome.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettingsPage : ContentPage
    {
        private readonly UserSettingsViewModel _viewModel;
        public UserSettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new UserSettingsViewModel();
        }
        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();
        }
    }
}