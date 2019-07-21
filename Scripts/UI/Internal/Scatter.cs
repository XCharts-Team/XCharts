using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class Scatter
    {
        [SerializeField] private string m_Name;
        [SerializeField] private float m_InsideRadius;
        [SerializeField] private float m_OutsideRadius;
        [SerializeField] private float m_TooltipExtraRadius;

        public string name { get { return m_Name; } set { m_Name = value; } }

        public static Scatter defaultScatter
        {
            get
            {
                var scatter = new Scatter
                {
                };
                return scatter;
            }
        }
    }
}