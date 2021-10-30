using NetHome.Common.JsonConverters;
using NetHome.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace NetHome.Helpers
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions GetOptions()
        {
            JsonSerializerOptions options = new();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new RuntimeTypeConverter<DeviceModel>("NetHome.Common.Models.Devices.", "NetHome.Common"));
            return options;
        }
    }
}
