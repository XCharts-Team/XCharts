using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class Pie
    {
        [SerializeField] private float m_TooltipExtraRadius;
        [SerializeField] private float m_SelectedOffset;
        public float tooltipExtraRadius { get { return m_TooltipExtraRadius; } set { m_TooltipExtraRadius = value; } }
        public float selectedOffset { get { return m_SelectedOffset; } set { m_SelectedOffset = value; } }

        public static Pie defaultPie
        {
            get
            {
                var pie = new Pie
                {
                    m_TooltipExtraRadius = 10f,
                    m_SelectedOffset = 10f,
                };
                return pie;
            }
        }
    }
}