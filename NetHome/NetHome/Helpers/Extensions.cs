using System;
using System.Text.Json;
using System.Threading.Tasks;
using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Views.Controls;
using Xamarin.Forms;

namespace NetHome.Helpers
{
    public static class Extensions
    {
        public static bool IsAdminOrOwner(this UserModel user)
        {
            return user != null && (user.Roles.Contains("Owner") || user.Roles.Contains("Admin"));
        }

        public static string GetErrorType(this Exception e)
        {
            return e is BadResponseException exception ? exception.Reason : e.GetType().Name;
        }

        public static T GetValue<T>(this ResourceDictionary dictionary, string key)
        {
            dictionary.TryGetValue(key, out object value);
            return (T)value;
        }

        public static T Clone<T>(this T model)
        {
            var json = JsonSerializer.SerializeToDocument(model, JsonHelper.GetOptions());
            return (T)JsonSerializer.Deserialize(json, typeof(T), JsonHelper.GetOptions());
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

        public static Color GetRGB(this RGBLightModel rgb)
        {
            return Color.FromRgb(rgb.Red, rgb.Green, rgb.Blue);
        }
        
        public static void SetRGB(this RGBLightModel rgb, Color color)
        {
            rgb.Red = (int)color.R;
            rgb.Green = (int)color.G;
            rgb.Blue = (int)color.B;
        }
    }
}
