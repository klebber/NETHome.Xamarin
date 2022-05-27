using NetHome.Common;
using NetHome.Views.DevicePages;

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
        public abstract string GetPageName();
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
            return ((SmartSwitchModel)Device).GetImage();
        }

        public override string GetPageName()
        {
            return nameof(SmartSwitchPage);
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
            return ((RGBLightModel)Device).GetImage();
        }

        public override string GetPageName()
        {
            return nameof(RgbLightPage);
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
            return ((AirConditionerModel)Device).GetImage();
        }

        public override string GetPageName()
        {
            return nameof(AirConditionerPage);
        }
    }
}
