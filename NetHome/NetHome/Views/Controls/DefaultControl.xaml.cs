using NetHome.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultControl : ContentView
    {
        private Command goToFullView;
        public ICommand GoToFullView => goToFullView ??= new Command(async () => await PerformGoToFullView());

        private DeviceModel device;
        public DeviceModel Device { get => device; set => SetProperty(ref device, value); }
        public DefaultControl(DeviceModel device)
        {
            InitializeComponent();
            BindingContext = this;
            this.device = device;
        }

        private Task PerformGoToFullView()
        {
            throw new NotImplementedException();
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}