using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace NetHome.ViewModels.Devices
{
    public class SmartSwitchViewModel : BaseDeviceViewModel
    {
        private Command toggleState;

        public SmartSwitchModel SmartSwitch { get => (SmartSwitchModel)DeviceClone; set => DeviceClone = value; }

        public ICommand ToggleState => toggleState ??= new Command(async () => await PerformChangeState(ToggleIson));

        private void ToggleIson()
        {
            SmartSwitch.Ison = !SmartSwitch.Ison;
        }

        protected override void SetImage()
        {
            Image = SmartSwitch.GetImage();
        }

        protected override void OnDeviceChanged()
        {
            OnPropertyChanged(nameof(SmartSwitch));
        }
    }
}
