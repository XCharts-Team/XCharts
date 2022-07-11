using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [Serializable]
    public class SerieTheme : ChildComponent
    {
        [SerializeField] protected float m_LineWidth;
        [SerializeField] protected float m_LineSymbolSize;
        [SerializeField] protected float m_ScatterSymbolSize;
        [SerializeField] protected float m_PieTooltipExtraRadius;
        [SerializeField] protected float m_SelectedRate = 1.3f;
        [SerializeField] protected float m_PieSelectedOffset;
        [SerializeField] protected Color32 m_CandlestickColor = new Color32(235, 84, 84, 255);
        [SerializeField] protected Color32 m_CandlestickColor0 = new Color32(71, 178, 98, 255);
        [SerializeField] protected float m_CandlestickBorderWidth = 1;
        [SerializeField] protected Color32 m_CandlestickBorderColor = new Color32(235, 84, 84, 255);
        [SerializeField] protected Color32 m_CandlestickBorderColor0 = new Color32(71, 178, 98, 255);

        /// <summary>
        /// the color of text.
        /// |文本颜色。
        /// </summary>
        public float lineWidth
        {
            get { return m_LineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_LineWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the symbol size of line serie.
        /// |折线图的Symbol大小。
        /// </summary>
        public float lineSymbolSize
        {
            get { return m_LineSymbolSize; }
            set { if (PropertyUtil.SetStruct(ref m_LineSymbolSize, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the selected symbol size of line serie.
        /// |折线图Symbol在被选中状态时的大小。
        /// </summary>
        public float lineSymbolSelectedSize { get { return lineSymbolSize * selectedRate; } }
        /// <summary>
        /// the symbol size of scatter serie.
        /// |散点图的Symbol大小。
        /// </summary>
        public float scatterSymbolSize
        {
            get { return m_ScatterSymbolSize; }
            set { if (PropertyUtil.SetStruct(ref m_ScatterSymbolSize, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the selected symbol size of scatter serie.
        /// |散点图的Symbol在被选中状态时的大小。
        /// </summary>
        public float scatterSymbolSelectedSize { get { return scatterSymbolSize * selectedRate; } }
        /// <summary>
        /// the rate of symbol size of line or scatter serie.
        /// |折线图或散点图在被选中时的放大倍数。
        /// </summary>
        public float selectedRate
        {
            get { return m_SelectedRate; }
            set { if (PropertyUtil.SetStruct(ref m_SelectedRate, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the extra radius of pie when actived by tooltip.
        /// |饼图鼠标移到高亮时的额外半径
        /// </summary>
        public float pieTooltipExtraRadius
        {
            get { return m_PieTooltipExtraRadius; }
            set { if (PropertyUtil.SetStruct(ref m_PieTooltipExtraRadius, value < 0 ? 0f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the center offset of pie if selected.
        /// |饼图选中时的中心点偏移。
        /// </summary>
        public float pieSelectedOffset
        {
            get { return m_PieSelectedOffset; }
            set { if (PropertyUtil.SetStruct(ref m_PieSelectedOffset, value < 0 ? 0f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// K线图阳线（涨）填充色
        /// </summary>
        public Color32 candlestickColor
        {
            get { return m_CandlestickColor; }
            set { if (PropertyUtil.SetColor(ref m_CandlestickColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// K线图阴线（跌）填充色
        /// </summary>
        public Color32 candlestickColor0
        {
            get { return m_CandlestickColor0; }
            set { if (PropertyUtil.SetColor(ref m_CandlestickColor0, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// K线图阳线（跌）边框色
        /// </summary>
        public Color32 candlestickBorderColor
        {
            get { return m_CandlestickBorderColor; }
            set { if (PropertyUtil.SetColor(ref m_CandlestickBorderColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// K线图阴线（跌）边框色
        /// </summary>
        public Color32 candlestickBorderColor0
        {
            get { return m_CandlestickBorderColor0; }
            set { if (PropertyUtil.SetColor(ref m_CandlestickBorderColor0, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// K线图边框宽度
        /// </summary>
        public float candlestickBorderWidth
        {
            get { return m_CandlestickBorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_CandlestickBorderWidth, value < 0 ? 0f : value)) SetVerticesDirty(); }
        }

        public void Copy(SerieTheme theme)
        {
            m_LineWidth = theme.lineWidth;
            m_LineSymbolSize = theme.lineSymbolSize;
            m_ScatterSymbolSize = theme.scatterSymbolSize;
            selectedRate = theme.selectedRate;
            m_PieTooltipExtraRadius = theme.pieTooltipExtraRadius;
            m_PieSelectedOffset = theme.pieSelectedOffset;
            m_CandlestickColor = theme.candlestickColor;
            m_CandlestickColor0 = theme.candlestickColor0;
            m_CandlestickBorderColor = theme.candlestickBorderColor;
            m_CandlestickBorderColor0 = theme.candlestickBorderColor0;
            m_CandlestickBorderWidth = theme.candlestickBorderWidth;
        }

        public SerieTheme(ThemeType theme)
        {
            m_LineWidth = XCSettings.serieLineWidth;
            m_LineSymbolSize = XCSettings.serieLineSymbolSize;
            m_ScatterSymbolSize = XCSettings.serieScatterSymbolSize;
            m_PieTooltipExtraRadius = XCSettings.pieTooltipExtraRadius;
            m_PieSelectedOffset = XCSettings.pieSelectedOffset;
            m_CandlestickBorderWidth = XCSettings.serieCandlestickBorderWidth;
            switch (theme)
            {
                case ThemeType.Default:
                    m_CandlestickColor = ColorUtil.GetColor("#eb5454");
                    m_CandlestickColor0 = ColorUtil.GetColor("#47b262");
                    m_CandlestickBorderColor = ColorUtil.GetColor("#eb5454");
                    m_CandlestickBorderColor0 = ColorUtil.GetColor("#47b262");
                    break;
                case ThemeType.Light:
                    m_CandlestickColor = ColorUtil.GetColor("#eb5454");
                    m_CandlestickColor0 = ColorUtil.GetColor("#47b262");
                    m_CandlestickBorderColor = ColorUtil.GetColor("#eb5454");
                    m_CandlestickBorderColor0 = ColorUtil.GetColor("#47b262");
                    break;
                case ThemeType.Dark:
                    m_CandlestickColor = ColorUtil.GetColor("#f64e56");
                    m_CandlestickColor0 = ColorUtil.GetColor("#54ea92");
                    m_CandlestickBorderColor = ColorUtil.GetColor("#f64e56");
                    m_CandlestickBorderColor0 = ColorUtil.GetColor("#54ea92");
                    break;
            }
        }
    }
}