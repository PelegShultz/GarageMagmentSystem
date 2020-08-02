using System.Collections.Generic;

namespace GarageLogic
{
    public class Fuel : PowerSource
    {
        private readonly eFuelType m_FuelType;

        public enum eFuelType
        {
            Octan95 = 1,
            Octan96,
            Octan98,
            Soler
        }

        internal eFuelType FuelType
        {
            get
            {
                return m_FuelType;
            }
        }

        internal Fuel(float i_MaximumAmountOfFuel, eFuelType i_FuelType) : base(i_MaximumAmountOfFuel)
        {
            m_FuelType = i_FuelType;
        }

        internal static void GetFuelParameters(List<string> io_Parameters)
        {
            io_Parameters.Add("amout of fuel (in liters)");
        }
    }
}
