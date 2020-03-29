/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// 图形样式。
    /// </summary>
    [System.Serializable]
    public class ItemStyle : SubComponent
    {
        /// <summary>
        /// 线的类型。
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// 实线
            /// </summary>
            Solid,
            /// <summary>
            /// 虚线
            /// </summary>
            Dashed,
            /// <summary>
            /// 点线
            /// </summary>
            Dotted
        }
        [SerializeField] private bool m_Show = false;
        [SerializeField] private Color m_Color;
        [SerializeField] private Color m_ToColor;
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField] private float m_BackgroundWidth;
        [SerializeField] private Color m_CenterColor;
        [SerializeField] private float m_CenterGap;
        [SerializeField] private Type m_BorderType = Type.Solid;
        [SerializeField] private float m_BorderWidth = 0;
        [SerializeField] private Color m_BorderColor;
        [SerializeField] [Range(0, 1)] private float m_Opacity = 1;
        [SerializeField] private string m_TooltipFormatter;
        [SerializeField] private float[] m_CornerRadius = new float[] { 0, 0, 0, 0 };

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项颜色。
        /// </summary>
        public Color color
        {
            get { return m_Color; }
            set { if (PropertyUtility.SetColor(ref m_Color, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Gradient color, start color to toColor.
        /// 渐变色的终点颜色。
        /// </summary>
        public Color toColor
        {
            get { return m_ToColor; }
            set { if (PropertyUtility.SetColor(ref m_ToColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项背景颜色。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtility.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 中心区域颜色。
        /// </summary>
        public Color centerColor
        {
            get { return m_CenterColor; }
            set { if (PropertyUtility.SetColor(ref m_CenterColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 中心区域间隙。
        /// </summary>
        public float centerGap
        {
            get { return m_CenterGap; }
            set { if (PropertyUtility.SetStruct(ref m_CenterGap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项背景颜色。
        /// </summary>
        public float backgroundWidth
        {
            get { return m_BackgroundWidth; }
            set { if (PropertyUtility.SetStruct(ref m_BackgroundWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框的类型。
        /// </summary>
        public Type borderType
        {
            get { return m_BorderType; }
            set { if (PropertyUtility.SetStruct(ref m_BorderType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框的颜色。
        /// </summary>
        public Color borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtility.SetColor(ref m_BorderColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtility.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
        /// </summary>
        public float opacity
        {
            get { return m_Opacity; }
            set { if (PropertyUtility.SetStruct(ref m_Opacity, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 提示框单项的字符串模版格式器。具体配置参考`Tooltip`的`formatter`
        /// </summary>
        public string tooltipFormatter
        {
            get { return m_TooltipFormatter; }
            set { if (PropertyUtility.SetClass(ref m_TooltipFormatter, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, upper right, bottom right and bottom left)).
        /// 圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。
        /// </summary>
        public float[] cornerRadius
        {
            get { return m_CornerRadius; }
            set { if (PropertyUtility.SetClass(ref m_CornerRadius, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 实际边框宽。边框不显示时为0。
        /// </summary>
        public float runtimeBorderWidth { get { return NeedShowBorder() ? borderWidth : 0; } }

        /// <summary>
        /// 是否需要显示边框。
        /// </summary>
        public bool NeedShowBorder()
        {
            return borderWidth != 0 && borderColor != Color.clear;
        }

        public Color GetColor()
        {
            var color = m_Color;
            color.a *= m_Opacity;
            return color;
        }
    }
}