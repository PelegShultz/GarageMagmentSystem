using System;
using System.Collections.Generic;
using System.Text;
using GarageLogic;

namespace ConsoleUI
{
    internal class ConsoleUI
    {
        private const int k_MinimumMenuOption = 1;
        private const int k_MaximumMenuOption = 9;
        private readonly GarageManager m_Garage = new GarageManager();

        internal enum eMenuOptions
        {
            AddNewVehicleToGarage = 1,
            PrintVehiclesInSpecificStatus,
            PrintAllVehiclesLicensePlates,
            ChangeVehicleStatus,
            FillVehiclesWheelsToMaximum,
            AddFuelToVehicle,
            ChargeVehicleBattery,
            PrintFullDetailesOfSpecificVehicle,
            Exit
        }

        internal void RunGarage()
        {
            eMenuOptions userSelection;
            do
            {
                PrintMenu();
                userSelection = SelectedOption();
                ExecuteSelection(userSelection);
            } 
            while (userSelection != eMenuOptions.Exit);
        }

        internal void PrintMenu()
        {
            Console.WriteLine("Welcome to Shahaf & Peleg Garage!");
            PrintEnumWithSpaces(typeof(eMenuOptions));
            Console.WriteLine("Please select an option from the list above: ");
        }

        internal eMenuOptions SelectedOption()
        {
            string choosenSelectionString;
            int choosenSelection;
            eMenuOptions selectedOption;
            choosenSelectionString = Console.ReadLine();
            while (!int.TryParse(choosenSelectionString, out choosenSelection) || !Enum.IsDefined(typeof(eMenuOptions), choosenSelection))
            {
                Console.WriteLine("ERROR. select a valid option.");
                PrintMenu();
                choosenSelectionString = Console.ReadLine();
            }

            Enum.TryParse(choosenSelectionString, out selectedOption);
            return selectedOption;
        }

        internal void ExecuteSelection(eMenuOptions i_SelectedOption)
        {
            switch (i_SelectedOption)
            {
                case eMenuOptions.AddNewVehicleToGarage:
                    {
                        AddVehicleToGarage();
                        break;
                    }
                case eMenuOptions.PrintVehiclesInSpecificStatus:
                    {
                        PrintVehiclesByCategory();
                        break;
                    }
                case eMenuOptions.PrintAllVehiclesLicensePlates:
                    {
                        PrintAllVehiclesLicensePlates();
                        break;
                    }
                case eMenuOptions.ChangeVehicleStatus:
                    {
                        ChangeVehicleStatus();
                        break;
                    }
                case eMenuOptions.FillVehiclesWheelsToMaximum:
                    {
                        FillWheelsAirToMaximum();
                        break;
                    }
                case eMenuOptions.AddFuelToVehicle:
                    {
                        AddFuel();
                        break;
                    }
                case eMenuOptions.ChargeVehicleBattery:
                    {
                        ChargeBattery();
                        break;
                    }
                case eMenuOptions.PrintFullDetailesOfSpecificVehicle:
                    {
                        PrintVehicleFullDeatailes();
                        break;
                    }
                case eMenuOptions.Exit:
                    {
                        Console.WriteLine("Bye Bye!");
                        break;
                    }
            }
        }

        internal void PrintAllVehiclesLicensePlates()
        {
            List<Vehicle> vehiclesArray = m_Garage.GetAllVehicles();
            if (vehiclesArray.Count == 0)
            {
                Console.WriteLine("There are no vehicles at the garage");
            }
            else
            {
                Console.WriteLine("The following vehicles visited the garage:");
                foreach (Vehicle vehicle in vehiclesArray)
                {
                    Console.WriteLine(vehicle.LicensePlateNumber);
                }
            }
        }

        internal void GetVehicleType(out VehicleCreator.eVehiclesTypes o_VehicelType)
        {
            string vehicelTypeString;
            int vehicleTypeOption;

            Console.WriteLine("Select your vehicle type:");
            PrintEnumWithSpaces(typeof(VehicleCreator.eVehiclesTypes));
            vehicelTypeString = Console.ReadLine();
            while (!int.TryParse(vehicelTypeString, out vehicleTypeOption) || !Enum.IsDefined(typeof(VehicleCreator.eVehiclesTypes), vehicleTypeOption))
            {
                Console.WriteLine("Invalid input!, Select your vehicle type:");
                PrintEnumWithSpaces(typeof(VehicleCreator.eVehiclesTypes));
                vehicelTypeString = Console.ReadLine();
            }

            Enum.TryParse(vehicelTypeString, out o_VehicelType);
        }

        internal void AddVehicleToGarage()
        {
            string licensePlateNumber;

            Console.WriteLine("Plese enter your license plate number (7 or 8 digits): ");
            licensePlateNumber = Console.ReadLine();
            while (!IsLlicensePlateNumberValid(licensePlateNumber))
            {
                Console.WriteLine("This license plate number is invalid.");
                Console.WriteLine("Plese enter your license plate number (7 or 8 digits): ");
                licensePlateNumber = Console.ReadLine();
            }

            if (m_Garage.IsVehicleExist(licensePlateNumber))
            {
                Console.WriteLine("This vehicle already in garage, status changed to in repair.");
                m_Garage.ChangeVehicleStatus(licensePlateNumber, Vehicle.eRepairStatus.InRepair);
            }
            else
            {
                Vehicle vehicleToAdd;
                List<string> requiredParameters;
                string parametersFromUser;
                VehicleCreator.eVehiclesTypes vehicelType;
                StringBuilder requiredParameter = new StringBuilder();
                int ParameterIndex = 0;

                GetVehicleType(out vehicelType);
                vehicleToAdd = m_Garage.CreateVehicle(licensePlateNumber, vehicelType);
                requiredParameters = vehicleToAdd.GetVehicleRequiredParameters();
                foreach (string inputString in requiredParameters)
                {
                    requiredParameter.Append("Please enter ");
                    requiredParameter.Append(inputString);
                    requiredParameter.Append(": ");
                    Console.WriteLine(requiredParameter);
                    parametersFromUser = Console.ReadLine();
                    requiredParameter.Clear();
                    ParameterIndex++;

                    bool IsValidInput = false;
                    while (!IsValidInput)
                    {
                        try
                        {
                            vehicleToAdd.SetVehicleRequiredParameters(parametersFromUser, ParameterIndex);
                            IsValidInput = true;
                        }
                        catch (FormatException formatException)
                        {
                            Console.WriteLine(formatException.Message);
                            parametersFromUser = Console.ReadLine();
                            IsValidInput = false;
                        }
                        catch (ArgumentException argumentException)
                        {
                            Console.WriteLine(argumentException.Message);
                            parametersFromUser = Console.ReadLine();
                            IsValidInput = false;
                        }
                        catch (ValueOutOfRangeException valueOutOfRangeException)
                        {
                            Console.WriteLine(valueOutOfRangeException.Message);
                            parametersFromUser = Console.ReadLine();
                            IsValidInput = false;
                        }
                    }
                }

                m_Garage.AddVehicleToGarage(ref vehicleToAdd);
                Console.WriteLine("Vehicle added succesfully!");
            }
        }

        internal void PrintVehiclesByCategory()
        {
            List<Vehicle> VehiclesArrayToPrint = null;
            string choosenVehicleStatusToPrint;
            Vehicle.eRepairStatus reapairStatus;
            int vehicleStatusNumber;

            Console.WriteLine("Select repair status:");
            PrintEnumWithSpaces(typeof(Vehicle.eRepairStatus));
            choosenVehicleStatusToPrint = Console.ReadLine();
            while (!int.TryParse(choosenVehicleStatusToPrint, out vehicleStatusNumber) || !Enum.IsDefined(typeof(Vehicle.eRepairStatus), vehicleStatusNumber))
            {
                Console.WriteLine("Invalid input!, select repair status:");
                PrintEnumWithSpaces(typeof(Vehicle.eRepairStatus));
                choosenVehicleStatusToPrint = Console.ReadLine();
            }

            Enum.TryParse(choosenVehicleStatusToPrint, out reapairStatus);
            VehiclesArrayToPrint = m_Garage.GetVehiclesInSpesificStatus(reapairStatus);
            if (VehiclesArrayToPrint.Count == 0)
            {
                Console.WriteLine("There are no vehicles at the desired repair status");
            }
            else
            {
                Console.WriteLine("The following vehicles are at the desired repair status:");
                foreach (Vehicle vehicle in VehiclesArrayToPrint)
                {
                    Console.WriteLine(vehicle.LicensePlateNumber);
                }
            }
        }

        internal void ChangeVehicleStatus()
        {
            string licensePlateNumberToChangeStatus, newStatusChoose;
            int newStatusNumber;
            Vehicle.eRepairStatus chooenStatus;

            Console.WriteLine("Please enter license plate number: ");
            licensePlateNumberToChangeStatus = Console.ReadLine();
            CheckIfLicensePlateValidation(licensePlateNumberToChangeStatus);
            Console.WriteLine("Select new repair status: ");
            PrintEnumWithSpaces(typeof(Vehicle.eRepairStatus));
            newStatusChoose = Console.ReadLine();
            while (!int.TryParse(newStatusChoose, out newStatusNumber) || !Enum.IsDefined(typeof(Vehicle.eRepairStatus), newStatusNumber))
            {
                Console.WriteLine("Invalid input!, Select new repair status: ");
                PrintEnumWithSpaces(typeof(Vehicle.eRepairStatus));
                newStatusChoose = Console.ReadLine();
            }

            Enum.TryParse(newStatusChoose, out chooenStatus);
            m_Garage.ChangeVehicleStatus(licensePlateNumberToChangeStatus, chooenStatus);
            Console.WriteLine("Vehicle status changed!");
        }

        internal void FillWheelsAirToMaximum()
        {
            string licensePlateNumber;
            Console.WriteLine("Please enter license plate number: ");
            licensePlateNumber = Console.ReadLine();
            CheckIfLicensePlateValidation(licensePlateNumber);
            m_Garage.FillWheelsAirToMaximum(licensePlateNumber);
            Console.WriteLine("Vehicle wheels now full of air!");
        }

        internal void AddFuel()
        {
            string licensePlateNumber, amountOfFuelString, choosenFueluelType;
            float amountOfFuel;
            int fuelTypeNumber;
            Fuel.eFuelType choosenFuelTypeEnum;
            Vehicle vehicleToAddFuel;

            Console.WriteLine("Please enter license plate number: ");
            licensePlateNumber = Console.ReadLine();
            CheckIfLicensePlateValidation(licensePlateNumber);
            m_Garage.VehiclesDictionary.TryGetValue(licensePlateNumber.GetHashCode(), out vehicleToAddFuel);
            if (vehicleToAddFuel != null)
            {
                if (vehicleToAddFuel.IsVehicleWorkingOnFuel())
                {
                    Console.WriteLine("Enter the capacaty of fuel to add: ");
                    amountOfFuelString = Console.ReadLine();
                    while (!IsStringAFloatNumber(amountOfFuelString, out amountOfFuel))
                    {
                        Console.WriteLine("Wrong input!");
                        Console.WriteLine("Enter the capacaty of fuel to add: ");
                        amountOfFuelString = Console.ReadLine();
                    }

                    Console.WriteLine("Select fuel type: ");
                    PrintEnumWithSpaces(typeof(Fuel.eFuelType));
                    choosenFueluelType = Console.ReadLine();
                    while (!int.TryParse(choosenFueluelType, out fuelTypeNumber) || !Enum.IsDefined(typeof(Fuel.eFuelType), fuelTypeNumber))
                    {
                        Console.WriteLine("Invalid input!, Select fuel type from the list: ");
                        PrintEnumWithSpaces(typeof(Fuel.eFuelType));
                        choosenFueluelType = Console.ReadLine();
                    }

                    bool IsValidInput = false;
                    while (!IsValidInput)
                    {
                        try
                        {
                            Enum.TryParse(choosenFueluelType, out choosenFuelTypeEnum);
                            m_Garage.AddFuel(licensePlateNumber, amountOfFuel, choosenFuelTypeEnum);
                            Console.WriteLine("Fuel Added!");
                            IsValidInput = true;
                        }
                        catch (ArgumentException argumentException)
                        {
                            Console.WriteLine(argumentException.Message);
                            Console.WriteLine("Select fuel type: ");
                            choosenFueluelType = Console.ReadLine();
                            IsValidInput = false;
                        }
                        catch (ValueOutOfRangeException valueOutOfRange)
                        {
                            Console.WriteLine(valueOutOfRange.Message);
                            Console.WriteLine("Enter the capacaty of fuel to add: ");
                            amountOfFuelString = Console.ReadLine();
                            float.TryParse(amountOfFuelString, out amountOfFuel);
                            IsValidInput = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ERROR. This vehicle is not working on fuel.");
                }
            }
        }

        internal void ChargeBattery()
        {
            string licensePlateNumber, AmountOfMinutesToAddString;
            float AmountOfHoursToAdd;
            Vehicle vehicleToCharge;

            Console.WriteLine("Please enter license plate number: ");
            licensePlateNumber = Console.ReadLine();
            CheckIfLicensePlateValidation(licensePlateNumber);
            m_Garage.VehiclesDictionary.TryGetValue(licensePlateNumber.GetHashCode(), out vehicleToCharge);
            if (vehicleToCharge != null)
            {
                if (vehicleToCharge.IsVehicleWorkingOnBattery())
                {
                    Console.WriteLine("Please enter the amount of hours to add to the battery");
                    AmountOfMinutesToAddString = Console.ReadLine();
                    while (!IsStringAFloatNumber(AmountOfMinutesToAddString, out AmountOfHoursToAdd))
                    {
                        Console.WriteLine("Invalid input!");
                        Console.WriteLine("Please enter the amount of hours to add to the battery");
                        AmountOfMinutesToAddString = Console.ReadLine();
                    }

                    bool IsValidInput = false;
                    while (!IsValidInput)
                    {
                        try
                        {
                            m_Garage.ChargeBattery(licensePlateNumber, AmountOfHoursToAdd);
                            Console.WriteLine("Battery charged!");
                            IsValidInput = true;
                        }
                        catch (ArgumentException argumentException)
                        {
                            Console.WriteLine(argumentException.Message);
                            IsValidInput = true;
                        }
                        catch (ValueOutOfRangeException valueOutOfRange)
                        {
                            Console.WriteLine(valueOutOfRange.Message);
                            Console.WriteLine("Please enter the amount of hours to add to the battery");
                            AmountOfMinutesToAddString = Console.ReadLine();
                            while (!IsStringAFloatNumber(AmountOfMinutesToAddString, out AmountOfHoursToAdd))
                            {
                                Console.WriteLine("Invalid input!");
                                Console.WriteLine("Please enter the amount of hours to add to the battery");
                                AmountOfMinutesToAddString = Console.ReadLine();
                            }

                            IsValidInput = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error. This vehicle is not working on battery");
                }
            }
        }

        internal void PrintVehicleFullDeatailes()
        {
            string licensePlateNumber;
            List<string> vehicleArryToPrint;
            Vehicle vehicleToPrint;

            Console.WriteLine("Please enter license plate number: ");
            licensePlateNumber = Console.ReadLine();
            CheckIfLicensePlateValidation(licensePlateNumber);
            m_Garage.VehiclesDictionary.TryGetValue(licensePlateNumber.GetHashCode(), out vehicleToPrint);
            vehicleArryToPrint = vehicleToPrint.GetVehicleFullDetailes();
            foreach (string stringToPrint in vehicleArryToPrint)
            {
                Console.WriteLine(stringToPrint);
            }
        }

        internal bool IsStringAFloatNumber(string i_StringToCheck, out float o_NumberToReturn)
        {
            return float.TryParse(i_StringToCheck, out o_NumberToReturn);
        }

        internal bool IsLlicensePlateNumberValid(string i_StringToCheck)
        {
            int licensePlateNumber;
            return int.TryParse(i_StringToCheck, out licensePlateNumber) && (i_StringToCheck.Length == 8 || i_StringToCheck.Length == 7);
        }

        internal void CheckIfLicensePlateValidation(string io_LicensePlateNumber)
        {
            while (!IsLlicensePlateNumberValid(io_LicensePlateNumber))
            {
                Console.WriteLine("This input invalid! Please enter license plate number: ");
                io_LicensePlateNumber = Console.ReadLine();
            }

            if (!m_Garage.IsVehicleExist(io_LicensePlateNumber))
            {
                string noExistCarMessege = string.Format(@"Car with license plate: {0} not exist in system.
return to the menu", 
io_LicensePlateNumber);
                Console.WriteLine(noExistCarMessege);
            }
        }

        internal void PrintEnumWithSpaces(Type i_TypeOfEnum)
        {
            StringBuilder enumOption = new StringBuilder();
            char charToConvertToLowercase;
            string[] stringsInTheEnum = Enum.GetNames(i_TypeOfEnum);

            for (int i = 0; i < stringsInTheEnum.Length; i++)
            {
                enumOption.Append(i + 1 + ". ");
                for (int j = 1; j < stringsInTheEnum[i].Length; j++)
                {
                    if (char.IsUpper(stringsInTheEnum[i][j]))
                    {
                        stringsInTheEnum[i] = stringsInTheEnum[i].Insert(j, " ");
                        j++;
                        charToConvertToLowercase = char.ToLower(stringsInTheEnum[i][j]);
                        stringsInTheEnum[i] = stringsInTheEnum[i].Remove(j, 1);
                        stringsInTheEnum[i] = stringsInTheEnum[i].Insert(j, charToConvertToLowercase.ToString());
                    }
                }

                enumOption.Append(stringsInTheEnum[i] + Environment.NewLine);
            }

            Console.WriteLine(enumOption);
        }
    }
}