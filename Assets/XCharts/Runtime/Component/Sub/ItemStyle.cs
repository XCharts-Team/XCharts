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
        [SerializeField] private Type m_BorderType = Type.Solid;
        [SerializeField] private float m_BorderWidth = 0;
        [SerializeField] private Color m_BorderColor;
        [SerializeField] [Range(0, 1)] private float m_Opacity = 1;

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// 数据项颜色。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// 边框的类型。
        /// </summary>
        public Type borderType { get { return m_BorderType; } set { m_BorderType = value; } }
        /// <summary>
        /// 边框的颜色。
        /// </summary>
        public Color borderColor { get { return m_BorderColor; } set { m_BorderColor = value; } }
        /// <summary>
        /// 边框宽。
        /// </summary>
        public float borderWidth { get { return m_BorderWidth; } set { m_BorderWidth = value; } }
        /// <summary>
        /// 透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
        /// </summary>
        public float opacity { get { return m_Opacity; } set { m_Opacity = value; } }
    }
}