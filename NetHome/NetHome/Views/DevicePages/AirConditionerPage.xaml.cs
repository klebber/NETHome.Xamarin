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
    public partial class AirConditionerPage : ContentPage
    {
        private readonly AirConditionerViewModel _viewModel;
        public int DeviceId { get; set; }

        public AirConditionerPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AirConditionerViewModel();
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(DeviceId);
            base.OnAppearing();
        }

        void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            _viewModel.OnSwitchToggled(sender, e);
        }

        void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            _viewModel.OnStepperValueChanged(sender, e);
        }
    }
}