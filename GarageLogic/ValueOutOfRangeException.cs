using System;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MaximumValue;
        private float m_MinimumValue;

        internal ValueOutOfRangeException(float i_MinimumValue, float i_MaximumValue, string i_Message) : base(i_Message)
        {
            m_MaximumValue = i_MaximumValue;
            m_MinimumValue = i_MinimumValue;
        }
    }
}
