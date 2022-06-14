using System;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Helpers;
using Xamarin.Forms;

namespace NetHome.ViewModels.Devices
{
    public class RollerShutterViewModel : BaseDeviceViewModel
    {
        public RollerShutterModel RollerShutter { get => (RollerShutterModel)DeviceClone; set => DeviceClone = value; }

        protected override void SetImage()
        {
            Image = RollerShutter.GetImage();
        }

        protected override void OnDeviceChanged()
        {
            OnPropertyChanged(nameof(RollerShutter));
        }
    }
}
