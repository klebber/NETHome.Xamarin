using System.Text.Json;
using System.Threading.Tasks;
using NetHome.Common;
using NetHome.Views.Controls;
using Xamarin.Forms;

namespace NetHome.Helpers
{
    public static class Extensions
    {
        public static DeviceModel Clone(this DeviceModel model)
        {
            var json = JsonSerializer.SerializeToDocument(model, JsonHelper.GetOptions());
            return (DeviceModel)JsonSerializer.Deserialize(json, typeof(DeviceModel), JsonHelper.GetOptions());
        }

        public static async Task<View> GetView(this DeviceModel device)
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

        public static string GetImage(this AirConditionerModel _) => "air_conditioner.png";

        public static string GetImage(this RollerShutterModel _) => "roller_shutter.png";

        public static string GetImage(this RGBLightModel _) => "light_bulb.png";

        public static string GetImage(this SmartSwitchModel model)
        {
            return model.Type switch
            {
                "Boiler" => "boiler.png",
                "Light" => "light_bulb.png",
                _ => "switch_default.png"
            };
        }
    }
}
