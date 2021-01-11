/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to base line.
    /// 线条基础配置。
    /// </summary>
    [System.Serializable]
    public class BaseLine : SubComponent
    {
        [SerializeField] protected bool m_Show;
        [SerializeField] protected LineStyle m_LineStyle = new LineStyle();

        /// <summary>
        /// Set this to false to prevent the axis line from showing.
        /// 是否显示坐标轴轴线。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 线条样式
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (value != null) { m_LineStyle = value; SetVerticesDirty(); } }
        }

        public static BaseLine defaultBaseLine
        {
            get
            {
                var axisLine = new BaseLine
                {
                    m_Show = true,
                    m_LineStyle = new LineStyle()
                };
                return axisLine;
            }
        }

        public BaseLine()
        {
            lineStyle = new LineStyle();
        }

        public BaseLine(bool show) : base()
        {
            m_Show = show;
        }

        public void Copy(BaseLine axisLine)
        {
            show = axisLine.show;
            lineStyle.Copy(axisLine.lineStyle);
        }

        public LineStyle.Type GetType(LineStyle.Type themeType)
        {
            return lineStyle.GetType(themeType);
        }

        public float GetWidth(float themeWidth)
        {
            return lineStyle.GetWidth(themeWidth);
        }

        public float GetLength(float themeLength)
        {
            return lineStyle.GetLength(themeLength);
        }

        public Color32 GetColor(Color32 themeColor)
        {
            return lineStyle.GetColor(themeColor);
        }
    }
}