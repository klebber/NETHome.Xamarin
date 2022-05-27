using NetHome.Common;
using NetHome.Services;
using NetHome.ViewModels.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.DevicePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public partial class SmartSwitchPage : ContentPage
    {
        private readonly SmartSwitchViewModel _viewModel;
        public int DeviceId { get; set; }

        public SmartSwitchPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SmartSwitchViewModel();
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(DeviceId);
            base.OnAppearing();
        }
    }
}