using NetHome.Common.JsonConverters;
using NetHome.Common;
using System.Text.Json;

namespace NetHome.Helpers
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions GetOptions()
        {
            JsonSerializerOptions options = new();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new RuntimeTypeConverter<DeviceModel>());
            return options;
        }
    }
}
