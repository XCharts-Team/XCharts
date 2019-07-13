
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class AxisTick
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private bool m_AlignWithLabel;
        [SerializeField] private bool m_Inside;
        [SerializeField] private float m_Length;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public bool alignWithLabel { get { return m_AlignWithLabel; } set { m_AlignWithLabel = value; } }
        public bool inside { get { return m_Inside; } set { m_Inside = value; } }
        public float length { get { return m_Length; } set { m_Length = value; } }

        public static AxisTick defaultTick
        {
            get
            {
                var tick = new AxisTick
                {
                    m_Show = true,
                    m_AlignWithLabel = false,
                    m_Inside = false,
                    m_Length = 5f
                };
                return tick;
            }
        }
    }
}