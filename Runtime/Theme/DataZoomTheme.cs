using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [Serializable]
    public class DataZoomTheme : ComponentTheme
    {
        [SerializeField] protected float m_BorderWidth;
        [SerializeField] protected float m_DataLineWidth;
        [SerializeField] protected Color32 m_FillerColor;
        [SerializeField] protected Color32 m_BorderColor;
        [SerializeField] protected Color32 m_DataLineColor;
        [SerializeField] protected Color32 m_DataAreaColor;
        [SerializeField] protected Color32 m_BackgroundColor;

        /// <summary>
        /// the width of border line.
        /// ||边框线宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of data line.
        /// ||数据阴影线宽。
        /// </summary>
        public float dataLineWidth
        {
            get { return m_DataLineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_DataLineWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of dataZoom data area.
        /// ||数据区域颜色。
        /// </summary>
        public Color32 fillerColor
        {
            get { return m_FillerColor; }
            set { if (PropertyUtil.SetColor(ref m_FillerColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the color of dataZoom border.
        /// ||边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of data area line.
        /// ||数据阴影的线条颜色。
        /// </summary>
        public Color32 dataLineColor
        {
            get { return m_DataLineColor; }
            set { if (PropertyUtil.SetColor(ref m_DataLineColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of data area line.
        /// ||数据阴影的填充颜色。
        /// </summary>
        public Color32 dataAreaColor
        {
            get { return m_DataAreaColor; }
            set { if (PropertyUtil.SetColor(ref m_DataAreaColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the background color of datazoom.
        /// ||背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetComponentDirty(); }
        }

        public DataZoomTheme(ThemeType theme) : base(theme)
        {
            m_BorderWidth = XCSettings.dataZoomBorderWidth;
            m_DataLineWidth = XCSettings.dataZoomDataLineWidth;
            m_BackgroundColor = Color.clear;
            switch (theme)
            {
                case ThemeType.Default:
                    m_TextColor = ColorUtil.GetColor("#333");
                    m_FillerColor = new Color32(167, 183, 204, 110);
                    m_BorderColor = ColorUtil.GetColor("#ddd");
                    m_DataLineColor = ColorUtil.GetColor("#2f4554");
                    m_DataAreaColor = new Color32(47, 69, 84, 85);
                    break;
                case ThemeType.Light:
                    m_TextColor = ColorUtil.GetColor("#333");
                    m_FillerColor = new Color32(167, 183, 204, 110);
                    m_BorderColor = ColorUtil.GetColor("#ddd");
                    m_DataLineColor = ColorUtil.GetColor("#2f4554");
                    m_DataAreaColor = new Color32(47, 69, 84, 85);
                    break;
                case ThemeType.Dark:
                    m_TextColor = ColorUtil.GetColor("#B9B8CE");
                    m_FillerColor = new Color32(135, 163, 206, (byte) (0.2f * 255));
                    m_BorderColor = ColorUtil.GetColor("#71708A");
                    m_DataLineColor = ColorUtil.GetColor("#71708A");
                    m_DataAreaColor = ColorUtil.GetColor("#71708A");
                    break;
            }
        }

        public void Copy(DataZoomTheme theme)
        {
            base.Copy(theme);
            m_BorderWidth = theme.borderWidth;
            m_DataLineWidth = theme.dataLineWidth;
            m_FillerColor = theme.fillerColor;
            m_BorderColor = theme.borderColor;
            m_DataLineColor = theme.dataLineColor;
            m_DataAreaColor = theme.dataAreaColor;
            m_BackgroundColor = theme.backgroundColor;
        }
    }
}