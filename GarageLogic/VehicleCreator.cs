using System;
using System.Collections.Generic;
using System.Text;

namespace GarageLogic
{
    public class VehicleCreator
    {
        public enum eVehiclesTypes
        {
            FuelMotorcycle = 1,
            ElectricMotorcycle,
            FuelCar,
            ElectricCar,
            Truck,
        }

        internal static Vehicle CreateVehicle(string i_LicensePlateNumber, eVehiclesTypes i_VehicleType)
        {
            Vehicle newVehicle = null;
            if (!Enum.IsDefined(typeof(eVehiclesTypes), i_VehicleType))
            {
                throw new ArgumentException("ERROR. " + i_VehicleType + "is not exist");
            }

            switch(i_VehicleType)
            {
                case eVehiclesTypes.ElectricCar:
                case eVehiclesTypes.FuelCar:
                    {
                        newVehicle = new Car(i_LicensePlateNumber, i_VehicleType);
                        break;
                    }

                case eVehiclesTypes.ElectricMotorcycle:
                case eVehiclesTypes.FuelMotorcycle:
                    {
                        newVehicle = new Motorcycle(i_LicensePlateNumber, i_VehicleType);
                        break;
                    }

                case eVehiclesTypes.Truck:
                    {
                        newVehicle = new Truck(i_LicensePlateNumber);
                        break;
                    }
            }

            return newVehicle;
        }
    }
}
