using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class Pie
    {
        [SerializeField] private string m_Name;
        [SerializeField] private float m_InsideRadius;
        [SerializeField] private float m_OutsideRadius;
        [SerializeField] private float m_TooltipExtraRadius;
        [SerializeField] private bool m_Selected;
        [SerializeField] private int m_SelectedIndex;
        [SerializeField] private float m_SelectedOffset;
        [SerializeField] private bool m_Rose;
        [SerializeField] private float m_Space;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;

        public string name { get { return m_Name; } set { m_Name = value; } }
        public float insideRadius { get { return m_InsideRadius; } set { m_InsideRadius = value; } }
        public float outsideRadius { get { return m_OutsideRadius; } set { m_OutsideRadius = value; } }
        public float tooltipExtraRadius { get { return m_TooltipExtraRadius; } set { m_TooltipExtraRadius = value; } }
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        public int selectedIndex { get { return m_SelectedIndex; } set { m_SelectedIndex = value; } }
        public float selectedOffset { get { return m_SelectedOffset; } set { m_SelectedOffset = value; } }
        public bool rose { get { return m_Rose; } set { m_Rose = value; } }
        public float space { get { return m_Space; } set { m_Space = value; } }
        public float left { get { return m_Left; } set { m_Left = value; } }
        public float right { get { return m_Right; } set { m_Right = value; } }
        public float top { get { return m_Top; } set { m_Top = value; } }
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; } }

        public static Pie defaultPie
        {
            get
            {
                var pie = new Pie
                {
                    m_Name = "Pie",
                    m_InsideRadius = 0f,
                    m_OutsideRadius = 80f,
                    m_TooltipExtraRadius = 10f,
                    m_Rose = false,
                    m_Selected = false,
                    m_SelectedOffset = 10,
                };
                return pie;
            }
        }
    }
}