using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using NetHome.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.Helpers
{
    public class ViewHelper
    {
        public static async Task<View> GetViewForDevice(DeviceModel device)
        {
            View view = null;
            var task = new Task(() =>
            {
                
                    view = device.GetType().Name switch
                    {
                        nameof(AirConditionerModel) => new ToggleControl(device),
                        nameof(RGBLightModel) => new ToggleControl(device),
                        nameof(RollerShutterModel) => new RollerShutterControl(device),
                        nameof(SmartSwitchModel) => new ToggleControl(device),
                        _ => new DefaultControl(device)
                    };
            });
            task.Start();
            await task;
            return view;
        }
    }
}
