/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to axis line.
    /// 坐标轴的分隔线。
    /// </summary>
    [System.Serializable]
    public class AxisLine : SubComponent
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
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// When mutiple axes exists, this option can be used to specify which axis can be "onZero" to.
        /// X 轴或者 Y 轴的轴线是否在另一个轴的 0 刻度上，只有在另一个轴为数值轴且包含 0 刻度时有效。
        /// </summary>
        public bool onZero
        {
            get { return m_OnZero; }
            set { if (PropertyUtility.SetStruct(ref m_OnZero, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// line style line width.
        /// 坐标轴线线宽。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtility.SetStruct(ref m_Width, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show the arrow symbol of axis. 
        /// 是否显示箭头。
        /// </summary>
        public bool symbol
        {
            get { return m_Symbol; }
            set { if (PropertyUtility.SetStruct(ref m_Symbol, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of arrow symbol. 
        /// 箭头宽。
        /// </summary>
        public float symbolWidth
        {
            get { return m_SymbolWidth; }
            set { if (PropertyUtility.SetStruct(ref m_SymbolWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the height of arrow symbol. 
        /// 箭头高。
        /// </summary>
        public float symbolHeight
        {
            get { return m_SymbolHeight; }
            set { if (PropertyUtility.SetStruct(ref m_SymbolHeight, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the offset of arrow symbol. 
        /// 箭头偏移。
        /// </summary>
        public float symbolOffset
        {
            get { return m_SymbolOffset; }
            set { if (PropertyUtility.SetStruct(ref m_SymbolOffset, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the dent of arrow symbol. 
        /// 箭头的凹陷程度。
        /// </summary>
        public float symbolDent
        {
            get { return m_SymbolDent; }
            set { if (PropertyUtility.SetStruct(ref m_SymbolDent, value)) SetVerticesDirty(); }
        }

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
                    m_SymbolOffset = -5f,
                    m_SymbolDent = 3,
                };
                return axisLine;
            }
        }

        public AxisLine Clone()
        {
            var axisLine = new AxisLine();
            axisLine.show = show;
            axisLine.onZero = onZero;
            axisLine.width = width;
            axisLine.symbol = symbol;
            axisLine.symbolWidth = symbolWidth;
            axisLine.symbolHeight = symbolHeight;
            axisLine.symbolOffset = symbolOffset;
            axisLine.symbolDent = symbolDent;
            return axisLine;
        }

        public void Copy(AxisLine axisLine)
        {
            show = axisLine.show;
            onZero = axisLine.onZero;
            width = axisLine.width;
            symbol = axisLine.symbol;
            symbolWidth = axisLine.symbolWidth;
            symbolHeight = axisLine.symbolHeight;
            symbolOffset = axisLine.symbolOffset;
            symbolDent = axisLine.symbolDent;
        }
    }
}