using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public class Car : Vehicle
    {
        private const Fuel.eFuelType k_CarFuelType = Fuel.eFuelType.Octan96;
        private const float k_MaximumBatteryTime = 2.1f;
        private const float k_MaximumAmountOfFuel = 60;
        private const int k_NumberOfCarColors = 4;
        private const int k_MaximumNumberOfCarDoors = 5;
        private const int k_NumberOfCarWheels = 4;
        private const float k_MaximumWheelAirPressure = 32; 
        private eCarColor m_Color;
        private eNumberOfCarDoors m_NumberOfDoors;

        private enum eCarColor
        {
            Red = 1,
            White,
            Black,
            Silver
        }

        private enum eNumberOfCarDoors
        {
            Two = 2,
            Three,
            Four,
            Five
        }

        private enum eCarRequiredParameters
        {
            CarColor = 7,
            NumberOfDoors
        }

        internal Car(string i_LicensePlateNumber, VehicleCreator.eVehiclesTypes i_VehicleType) : base(i_LicensePlateNumber)
        {
            if(i_VehicleType == VehicleCreator.eVehiclesTypes.ElectricCar)
            {
                m_PowerSource = new Battery(k_MaximumBatteryTime);
            }
            else if(i_VehicleType == VehicleCreator.eVehiclesTypes.FuelCar)
            {
                m_PowerSource = new Fuel(k_MaximumAmountOfFuel, k_CarFuelType);
            }
        }

        internal string Color
        {
            get
            {
                return m_Color.ToString();
            }

            set
            {
                int parsedValue;
                if(!int.TryParse(value, out parsedValue))
                {
                    throw new FormatException("ERROR. car color should be a number from the list");
                }

                if(!Enum.IsDefined(typeof(eCarColor), parsedValue))
                {
                    throw new ValueOutOfRangeException(1, k_NumberOfCarColors, "ERROR. car color selection should be between 1 - " + k_NumberOfCarColors);
                }

                Enum.TryParse<eCarColor>(value, out m_Color);
            }
        }

        internal string NumberOfDoors
        {
            get
            {
                return m_NumberOfDoors.ToString();
            }

            set
            {
                int parsedValue;

                if (!int.TryParse(value, out parsedValue) || !Enum.IsDefined(typeof(eNumberOfCarDoors), parsedValue))
                {
                    throw new ValueOutOfRangeException(2, k_MaximumNumberOfCarDoors, "ERROR. number of car doors should be between 2 - " + k_MaximumNumberOfCarDoors);
                }

                Enum.TryParse<eNumberOfCarDoors>(value, out m_NumberOfDoors);
            }
        }

        protected internal override void AddToPowerSource(float i_AmountToAdd)
        {
            m_PowerSource.AddToPowerSource(i_AmountToAdd);
        }

        public override List<string> GetVehicleFullDetailes()
        {
            List<string> vehicleDetailes = GetVehicleBaseDetailes();
            vehicleDetailes.Add("Number of doors: " + NumberOfDoors.ToString());
            vehicleDetailes.Add("Color: " + Color.ToString());
            if (m_PowerSource is Battery)
            {
                vehicleDetailes.Add("Power source type: Battery");
            }
            else if(m_PowerSource is Fuel)
            {
                vehicleDetailes.Add("Power source type: Fuel");
                vehicleDetailes.Add("Fuel type: " + k_CarFuelType.ToString());
            }

            return vehicleDetailes;
        }

        public override List<string> GetVehicleRequiredParameters()
        {
            List<string> baseParameters = GetBaseVehicleRequiredParameters();
            string carColor;

            if (m_PowerSource is Battery)
            {
                Battery.GetBatteryParameters(baseParameters);
            }
            else if (m_PowerSource is Fuel)
            {
                Fuel.GetFuelParameters(baseParameters);
            }

            carColor = string.Format(@"Choose your car color: 
1. Red
2. White
3. Black
4. Silver");

            baseParameters.Add(carColor);
            baseParameters.Add("How many doors your cars have ? (between 2 - 5)");

            return baseParameters;
        }

        public override void SetVehicleRequiredParameters(string i_ParameterToSet, int i_ParameterIndex)
        {
            eVehicleRequiredParameters vehicleParameterIndex;
            eCarRequiredParameters carParameterIndex;

            Enum.TryParse<eCarRequiredParameters>(i_ParameterIndex.ToString(), out carParameterIndex);
            if(Enum.TryParse<eVehicleRequiredParameters>(i_ParameterIndex.ToString(), out vehicleParameterIndex))
            {
                SetBaseVehicleRequiredParametersWrapper(i_ParameterToSet, i_ParameterIndex, k_NumberOfCarWheels, k_MaximumWheelAirPressure);
            }

            switch (carParameterIndex)
            {
                case eCarRequiredParameters.NumberOfDoors:
                    {
                        NumberOfDoors = i_ParameterToSet;
                        break;
                    }

                case eCarRequiredParameters.CarColor:
                    {
                        Color = i_ParameterToSet;
                        break;
                    }
            }
        }

        protected internal override void UpdatePercentageOfEnergy()
        {
            float fullCapacityValue;

            if(m_PowerSource is Fuel)
            {
                fullCapacityValue = k_MaximumAmountOfFuel;
            }
            else
            {
                fullCapacityValue = k_MaximumBatteryTime;
            }

            m_PercentageOfEnergy = m_PowerSource.AmountOfEnergyLeft / fullCapacityValue * 100;
        }
    }
}