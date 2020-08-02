using System.Collections.Generic;

namespace GarageLogic
{
    public class Battery : PowerSource
    {
        internal Battery(float i_MaximumBatteryTime) : base(i_MaximumBatteryTime)
        {
        }

        internal static void GetBatteryParameters(List<string> io_Parameters)
        {
            io_Parameters.Add("time left in battery (in hours)");
        }
    }
}
