﻿using System;
using System.Windows.Input;
using NetHome.Common;
using NetHome.Helpers;
using Xamarin.Forms;

namespace NetHome.ViewModels.Devices
{
    public class AirConditionerViewModel : BaseDeviceViewModel
    {
        private Command buttonClickCommand;
        public AirConditionerModel AirConditioner { get => (AirConditionerModel)DeviceClone; set => DeviceClone = value; }
        public ICommand ButtonClickCommand => buttonClickCommand ??= new Command(async () => await PerformChangeState(SetPowerValue));
        public AirConditionerSpeed? ACSpeed => AirConditioner is not null ? (AirConditionerSpeed?)AirConditioner.FanSpeed : null;

        private void SetPowerValue()
        {
            AirConditioner.Ison = !AirConditioner.Ison;
        }

        public void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            BaseStateChangeCommand.Execute(null);
        }

        public void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            BaseStateChangeCommand.Execute(null);
        }

        protected override void SetImage()
        {
            Image = AirConditioner.GetImage();
        }

        protected override void OnDeviceChanged()
        {
            OnPropertyChanged(nameof(AirConditioner));
            OnPropertyChanged(nameof(ACSpeed));
        }
    }
}
