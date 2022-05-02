using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Helpers
{
    internal abstract class ToggleControlState
    {
        public DeviceModel Device { get; set; }
        public ToggleControlState(DeviceModel device)
        {
            Device = device;
        }
        public abstract bool GetState();
        public abstract void SetState(bool value);
        public abstract string GetImage();
    }

    internal class SmartSwitchState : ToggleControlState
    {
        public SmartSwitchState(DeviceModel device) : base(device) { }

        public override bool GetState()
        {
            return ((SmartSwitchModel)Device).Ison;
        }

        public override void SetState(bool value)
        {
            ((SmartSwitchModel)Device).Ison = value;
        }

        public override string GetImage()
        {
            return Device.Type switch
            {
                "Boiler" => "boiler.png",
                "Light" => "light_bulb.png",
                _ => "switch_default.png"
            };
        }
    }

    internal class RgbLightState : ToggleControlState
    {
        public RgbLightState(DeviceModel device) : base(device) { }

        public override bool GetState()
        {
            return ((RGBLightModel)Device).Ison;
        }

        public override void SetState(bool value)
        {
            ((RGBLightModel)Device).Ison = value;
        }

        public override string GetImage()
        {
            return "light_bulb.png";
        }
    }

    internal class AirConditionerState : ToggleControlState
    {
        public AirConditionerState(DeviceModel device) : base(device) { }

        public override bool GetState()
        {
            return ((AirConditionerModel)Device).Ison;
        }

        public override void SetState(bool value)
        {
            ((AirConditionerModel)Device).Ison = value;
        }

        public override string GetImage()
        {
            return "air_conditioner.png";
        }
    }
}
