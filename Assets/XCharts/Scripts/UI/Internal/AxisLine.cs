using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to axis line.
    /// 坐标轴的分隔线。
    /// </summary>
    [System.Serializable]    
    public class AxisLine
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private bool m_OnZero;
        [SerializeField] private float m_Width = 0.6f;
        [SerializeField] private bool m_Symbol;
        [SerializeField] private float m_SymbolWidth;
        [SerializeField] private float m_SymbolHeight;
        [SerializeField] private float m_SymbolOffset;
        [SerializeField] private float m_SymbolDent;

        /// <summary>
        /// Set this to false to prevent the axis line from showing.
        /// 是否显示坐标轴轴线。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// When mutiple axes exists, this option can be used to specify which axis can be "onZero" to.
        /// X 轴或者 Y 轴的轴线是否在另一个轴的 0 刻度上，只有在另一个轴为数值轴且包含 0 刻度时有效。
        /// </summary>
        public bool onZero { get { return m_OnZero; } set { m_OnZero = value; } }
        /// <summary>
        /// line style line width.
        /// 坐标轴线线宽。
        /// </summary>
        public float width { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// Whether to show the arrow symbol of axis. 
        /// 是否显示箭头。
        /// </summary>
        public bool symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        /// <summary>
        /// the width of arrow symbol. 
        /// 箭头宽。
        /// </summary>
        public float symbolWidth { get { return m_SymbolWidth; } set { m_SymbolWidth = value; } }
        /// <summary>
        /// the height of arrow symbol. 
        /// 箭头高。
        /// </summary>
        public float symbolHeight { get { return m_SymbolHeight; } set { m_SymbolHeight = value; } }
        /// <summary>
        /// the offset of arrow symbol. 
        /// 箭头偏移。
        /// </summary>
        public float symbolOffset { get { return m_SymbolOffset; } set { m_SymbolOffset = value; } }
        /// <summary>
        /// the dent of arrow symbol. 
        /// 箭头的凹陷程度。
        /// </summary>
        public float symbolDent { get { return m_SymbolDent; } set { m_SymbolDent = value; } }

        public static AxisLine defaultAxisLine
        {
            get
            {
                var axisLine = new AxisLine
                {
                    m_Show = true,
                    m_OnZero = true,
                    m_Width = 0.7f,
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