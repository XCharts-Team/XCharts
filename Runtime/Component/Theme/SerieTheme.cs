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

        public void Copy(SerieTheme theme)
        {
            m_LineWidth = theme.lineWidth;
            m_LineSymbolSize = theme.lineSymbolSize;
            m_LineSymbolSelectedSize = theme.lineSymbolSelectedSize;
            m_ScatterSymbolSize = theme.scatterSymbolSize;
            m_ScatterSymbolSelectedSize = theme.scatterSymbolSelectedSize;
            m_PieTooltipExtraRadius = theme.pieTooltipExtraRadius;
            m_PieSelectedOffset = theme.pieSelectedOffset;
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
        }
    }
}