using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class AxisLine
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private bool m_OnZero;
        [SerializeField] private bool m_Symbol;
        [SerializeField] private float m_SymbolWidth;
        [SerializeField] private float m_SymbolHeight;
        [SerializeField] private float m_SymbolOffset;
        [SerializeField] private float m_SymbolDent;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public bool onZero { get { return m_OnZero; } set { m_OnZero = value; } }
        public bool symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        public float symbolWidth { get { return m_SymbolWidth; } set { m_SymbolWidth = value; } }
        public float symbolHeight { get { return m_SymbolHeight; } set { m_SymbolHeight = value; } }
        public float symbolOffset { get { return m_SymbolOffset; } set { m_SymbolOffset = value; } }
        public float symbolDent { get { return m_SymbolDent; } set { m_SymbolDent = value; } }

        public static AxisLine defaultAxisLine
        {
            get
            {
                var axisLine = new AxisLine
                {
                    m_Show = true,
                    m_OnZero = true,
                    m_Symbol = false,
                    m_SymbolWidth = 10,
                    m_SymbolHeight = 15,
                    m_SymbolOffset = 0,
                    m_SymbolDent = 3,
                };
                return axisLine;
            }
        }
    }
}