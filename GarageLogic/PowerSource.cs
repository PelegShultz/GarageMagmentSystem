using System;

namespace GarageLogic
{
    public abstract class PowerSource
    {
        protected readonly float r_MaximumAmountOfEnergy;
        protected float m_AmountOfEnergyLeft;

        protected internal PowerSource(float i_MaximumAmountOfEnergy)
        {
            r_MaximumAmountOfEnergy = i_MaximumAmountOfEnergy;
        }

        internal float AmountOfEnergyLeft
        {
            get
            {
                return m_AmountOfEnergyLeft;
            }
        }

        internal void AddToPowerSource(float i_AmountToAdd)
        {
            if (i_AmountToAdd + m_AmountOfEnergyLeft > r_MaximumAmountOfEnergy || i_AmountToAdd < 0)
            {
                throw new ValueOutOfRangeException(0, r_MaximumAmountOfEnergy - m_AmountOfEnergyLeft, "ERROR. Values should be between 0 - " + (r_MaximumAmountOfEnergy - m_AmountOfEnergyLeft));
            }

            m_AmountOfEnergyLeft += i_AmountToAdd;
        }

        internal void SetPowersourceCurrentValue(string i_CurrentValue)
        {
            float currentValue;
            if (!float.TryParse(i_CurrentValue, out currentValue) || i_CurrentValue.Length == 0)
            {
                throw new FormatException("ERROR. Power source value should be a rational number");
            }

            if (currentValue > r_MaximumAmountOfEnergy || currentValue < 0)
            {
                throw new ValueOutOfRangeException(0, r_MaximumAmountOfEnergy, "ERROR. Power source value sholud be between 0 - " + r_MaximumAmountOfEnergy);
            }

            m_AmountOfEnergyLeft = currentValue;
        }
    }
}
