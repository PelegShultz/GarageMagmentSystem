using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public class GarageManager
    {
        private Dictionary<int, Vehicle> m_VehiclesDictionary = new Dictionary<int, Vehicle>();

        public Vehicle CreateVehicle(string i_LicensePlateNumber, VehicleCreator.eVehiclesTypes i_VehicelType)
        {
            Vehicle newVehicle;

            if(m_VehiclesDictionary.TryGetValue(i_LicensePlateNumber.GetHashCode(), out newVehicle))
            {
                throw new ArgumentException("ERROR. The car with license plate number" + i_LicensePlateNumber + "is already exist");
            }

            newVehicle = VehicleCreator.CreateVehicle(i_LicensePlateNumber, i_VehicelType);

            return newVehicle;
        }

        public void AddVehicleToGarage(ref Vehicle i_NewVehicle)
        {
            Vehicle vehicleSearchedInDirectory;
            if (m_VehiclesDictionary.TryGetValue(i_NewVehicle.GetHashCode(), out vehicleSearchedInDirectory))
            {
                throw new ArgumentException("ERROR. The car with license plate number" + vehicleSearchedInDirectory.LicensePlateNumber + "is already exist");
            }

            m_VehiclesDictionary.Add(i_NewVehicle.GetHashCode(), i_NewVehicle);
        }

        public Dictionary<int, Vehicle> VehiclesDictionary
        {
            get
            {
                return m_VehiclesDictionary;
            }
        }

        public bool IsVehicleExist(string i_LicensePlateNumber)
        {
            Vehicle vehicleToLookFor;

            return m_VehiclesDictionary.TryGetValue(i_LicensePlateNumber.GetHashCode(), out vehicleToLookFor);
        }

        public void ChangeVehicleStatus(string i_LicensePlateNumber, Vehicle.eRepairStatus i_ReapairStatus)
        {
            Vehicle vehicleToChangeStatus;
            if(!m_VehiclesDictionary.TryGetValue(i_LicensePlateNumber.GetHashCode(), out vehicleToChangeStatus))
            {
                throw new ArgumentException("ERROR. The Vehicle with license plate number " + i_LicensePlateNumber + " not exist");
            }

            if(!Enum.IsDefined(typeof(Vehicle.eRepairStatus), i_ReapairStatus))
            {
                throw new ArgumentException("ERROR. New vheicle status not exist");
            }

            vehicleToChangeStatus.Status = i_ReapairStatus;
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicleToReturn = new List<Vehicle>();
            foreach (KeyValuePair<int, Vehicle> keyAndValue in m_VehiclesDictionary)
            {
                vehicleToReturn.Add(keyAndValue.Value);
            }

            return vehicleToReturn;
        }

        public List<Vehicle> GetVehiclesInSpesificStatus(Vehicle.eRepairStatus i_Status)
        {
            List<Vehicle> vehicleToReturn = new List<Vehicle>();

            if(!Enum.IsDefined(typeof(Vehicle.eRepairStatus), i_Status))
            {
                throw new ArgumentException("ERROR. " + i_Status + " status is not exist");
            }

            foreach(KeyValuePair<int, Vehicle> keyAndValue in m_VehiclesDictionary)
            {
                if(keyAndValue.Value.Status == i_Status)
                {
                    vehicleToReturn.Add(keyAndValue.Value);
                }
            }

            return vehicleToReturn;
        }

        public void FillWheelsAirToMaximum(string i_LicensePlateNumber)
        {
            Vehicle vehicleToFillAir;

            m_VehiclesDictionary.TryGetValue(i_LicensePlateNumber.GetHashCode(), out vehicleToFillAir);
            vehicleToFillAir.FillAllWheels();
        }

        public void AddFuel(string i_LicensePlateNumber, float i_AmountOfFuel, Fuel.eFuelType i_FuelType)
        {
            Vehicle vehicleToFill;
            Fuel vehicleFuelTank;

            if (!m_VehiclesDictionary.TryGetValue(i_LicensePlateNumber.GetHashCode(), out vehicleToFill))
            {
                throw new ArgumentException("ERROR. The Vehicle with license plate number " + i_LicensePlateNumber + " not exist");
            }

            vehicleFuelTank = vehicleToFill.GetPowerSource() as Fuel;
            if (vehicleFuelTank == null)
            {
                throw new ArgumentException("ERROR. The vehicle is not working on fuel");
            }

            if(!Enum.IsDefined(typeof(Fuel.eFuelType), i_FuelType))
            {
                throw new ArgumentException("ERROR. select fuel type from the list.");
            }

            if (vehicleFuelTank.FuelType != i_FuelType)
            {
                throw new ArgumentException(i_FuelType + " is not the right fuel type for this vehicle, " + vehicleFuelTank.FuelType + " is the right fuel type");
            }

            vehicleToFill.AddToPowerSource(i_AmountOfFuel);
            vehicleToFill.UpdatePercentageOfEnergy();
        }

        public void ChargeBattery(string i_LicensePlateNumber, float i_HoursToAdd)
        {
            Vehicle vehicleToFill;
            Battery vehicleBattery;

            if (!m_VehiclesDictionary.TryGetValue(i_LicensePlateNumber.GetHashCode(), out vehicleToFill))
            {
                throw new ArgumentException("ERROR. The Vehicle with license plate number " + i_LicensePlateNumber + " not exist");
            }

            vehicleBattery = vehicleToFill.GetPowerSource() as Battery;
            if (vehicleBattery == null)
            {
                throw new ArgumentException("ERROR. This vehicle is not working on battery");
            }

            vehicleToFill.AddToPowerSource(i_HoursToAdd);
            vehicleToFill.UpdatePercentageOfEnergy();
        }
    }
}
