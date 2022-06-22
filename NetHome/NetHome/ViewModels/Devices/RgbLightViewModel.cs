using System;
using System.Diagnostics;
using NetHome.Common;
using NetHome.Helpers;
using NetHome.Views.Components;
using System.Windows.Input;
using Xamarin.Forms;

namespace NetHome.ViewModels.Devices
{
    public class RgbLightViewModel : BaseDeviceViewModel
    {
        private Command powerToggleCommand;
        public RGBLightModel RgbLight { get => (RGBLightModel)DeviceClone; set => DeviceClone = value; }
        public ICommand PowerToggleCommand => powerToggleCommand ??= new Command(async () => await PerformChangeState(SetPowerValue));
        public int? SelectedTab { get => RgbLight is not null ? (int)Enum.Parse(typeof(RgbLightMode), RgbLight.Mode) : null; set => RgbLight.Mode = ((RgbLightMode)value).ToString(); }

        protected override void SetImage()
        {
            Image = RgbLight.GetImage();
        }

        protected override void OnDeviceChanged()
        {
            OnPropertyChanged(nameof(RgbLight));
            OnPropertyChanged(nameof(SelectedTab));
        }

        internal void TabView_SelectionChanged()
        {
            BaseStateChangeCommand.Execute(null);
        }

        private void SetPowerValue()
        {
            RgbLight.Ison = !RgbLight.Ison;
        }
    }
}
