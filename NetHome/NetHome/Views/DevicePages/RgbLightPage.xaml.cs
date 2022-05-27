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
    public partial class RgbLightPage : ContentPage
    {
        private readonly RgbLightViewModel _viewModel;
        public int DeviceId { get; set; }

        public RgbLightPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RgbLightViewModel();
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(DeviceId);
            base.OnAppearing();
        }
    }
}