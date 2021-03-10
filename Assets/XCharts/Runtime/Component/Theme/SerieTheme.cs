/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    public class SerieTheme : MainComponent
    {
        [SerializeField] protected float m_LineWidth;
        [SerializeField] protected float m_LineSymbolSize;
        [SerializeField] protected float m_LineSymbolSelectedSize;
        [SerializeField] protected float m_ScatterSymbolSize;
        [SerializeField] protected float m_ScatterSymbolSelectedSize;
        [SerializeField] protected float m_PieTooltipExtraRadius;
        [SerializeField] protected float m_PieSelectedOffset;
        [SerializeField] protected Color32 m_CandlestickColor = new Color32(194, 53, 49, 255);
        [SerializeField] protected Color32 m_CandlestickColor0 = new Color32(49, 70, 86, 255);
        [SerializeField] protected float m_CandlestickBorderWidth = 1;
        [SerializeField] protected Color32 m_CandlestickBorderColor = new Color32(194, 53, 49, 255);
        [SerializeField] protected Color32 m_CandlestickBorderColor0 = new Color32(49, 70, 86, 255);

        /// <summary>
        /// the color of text.
        /// 文本颜色。
        /// </summary>
        public float lineWidth
        {
            get { return m_LineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_LineWidth, value)) SetVerticesDirty(); }
        }
        public float lineSymbolSize
        {
            get { return m_LineSymbolSize; }
            set { if (PropertyUtil.SetStruct(ref m_LineSymbolSize, value)) SetVerticesDirty(); }
        }
        public float lineSymbolSelectedSize
        {
            get { return m_LineSymbolSelectedSize; }
            set { if (PropertyUtil.SetStruct(ref m_LineSymbolSelectedSize, value)) SetVerticesDirty(); }
        }
        public float scatterSymbolSize
        {
            get { return m_ScatterSymbolSize; }
            set { if (PropertyUtil.SetStruct(ref m_ScatterSymbolSize, value)) SetVerticesDirty(); }
        }
        public float scatterSymbolSelectedSize
        {
            get { return m_ScatterSymbolSelectedSize; }
            set { if (PropertyUtil.SetStruct(ref m_ScatterSymbolSelectedSize, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// 饼图鼠标移到高亮时的额外半径
        /// </summary>
        public float pieTooltipExtraRadius
        {
            get { return m_PieTooltipExtraRadius; }
            set { if (PropertyUtil.SetStruct(ref m_PieTooltipExtraRadius, value < 0 ? 0f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 饼图选中时的中心点偏移
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
            m_LineSymbolSelectedSize = theme.lineSymbolSelectedSize;
            m_ScatterSymbolSize = theme.scatterSymbolSize;
            m_ScatterSymbolSelectedSize = theme.scatterSymbolSelectedSize;
            m_PieTooltipExtraRadius = theme.pieTooltipExtraRadius;
            m_PieSelectedOffset = theme.pieSelectedOffset;
            m_CandlestickColor = theme.candlestickColor;
            m_CandlestickColor0 = theme.candlestickColor0;
            m_CandlestickBorderColor = theme.candlestickBorderColor;
            m_CandlestickBorderColor0 = theme.candlestickBorderColor0;
            m_CandlestickBorderWidth = theme.candlestickBorderWidth;
        }

        public SerieTheme(Theme theme)
        {
            m_LineWidth = XChartsSettings.serieLineWidth;
            m_LineSymbolSize = XChartsSettings.serieLineSymbolSize;
            m_LineSymbolSelectedSize = XChartsSettings.serieLineSymbolSelectedSize;
            m_ScatterSymbolSize = XChartsSettings.serieScatterSymbolSize;
            m_ScatterSymbolSelectedSize = XChartsSettings.serieScatterSymbolSelectedSize;
            m_PieTooltipExtraRadius = XChartsSettings.pieTooltipExtraRadius;
            m_PieSelectedOffset = XChartsSettings.pieSelectedOffset;
            m_CandlestickBorderWidth = XChartsSettings.serieCandlestickBorderWidth;
            switch (theme)
            {
                case Theme.Default:
                    m_CandlestickColor = ColorUtil.GetColor("#c23531");
                    m_CandlestickColor0 = ColorUtil.GetColor("#314656");
                    m_CandlestickBorderColor = ColorUtil.GetColor("#c23531");
                    m_CandlestickBorderColor0 = ColorUtil.GetColor("#314656");
                    break;
                case Theme.Light:
                    m_CandlestickColor = ColorUtil.GetColor("#c23531");
                    m_CandlestickColor0 = ColorUtil.GetColor("#314656");
                    m_CandlestickBorderColor = ColorUtil.GetColor("#c23531");
                    m_CandlestickBorderColor0 = ColorUtil.GetColor("#314656");
                    break;
                case Theme.Dark:
                    m_CandlestickColor = ColorUtil.GetColor("#c23531");
                    m_CandlestickColor0 = ColorUtil.GetColor("#314656");
                    m_CandlestickBorderColor = ColorUtil.GetColor("#c23531");
                    m_CandlestickBorderColor0 = ColorUtil.GetColor("#314656");
                    break;
            }
        }
    }
}