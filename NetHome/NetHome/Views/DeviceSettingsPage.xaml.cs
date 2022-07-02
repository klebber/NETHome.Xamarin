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
    public partial class DeviceSettingsPage : ContentPage
    {
        private readonly DeviceSettingsViewModel _viewModel;
        public DeviceSettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DeviceSettingsViewModel();
        }
        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();
        }
    }
}