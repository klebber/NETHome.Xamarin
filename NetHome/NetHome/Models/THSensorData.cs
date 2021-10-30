using NetHome.Common.Models;
using NetHome.Common.Models.Devices;
using System.Collections.Generic;
using System.Linq;

namespace NetHome.Models
{
    public class THSensorData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public THSensorData() { }
        public THSensorData(DeviceModel dm)
        {
            THSensorModel thsm = (THSensorModel)dm;
            Temperature = thsm.Temperature;
            Humidity = thsm.Humidity;
        }
        public THSensorData(IEnumerable<DeviceModel> iedm)
        {
            Temperature = iedm.Select(dm => ((THSensorModel)dm).Temperature).ToList().Average();
            Humidity = iedm.Select(dm => ((THSensorModel)dm).Humidity).ToList().Average();
        }
    }
}
