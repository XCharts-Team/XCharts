using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the name of axis.
    /// 坐标轴名称。
    /// </summary>
    [Serializable]
    public class AxisName
    {
        /// <summary>
        /// the location of axis name.
        /// 坐标轴名称显示位置。
        /// </summary>
        public enum Location
        {
            Start,
            Middle,
            End
        }
        [SerializeField] private bool m_Show;
        [SerializeField] private string m_Name;
        [SerializeField] private Location m_Location;
        [SerializeField] private Vector2 m_Offset;
        [SerializeField] private float m_Rotate;
        [SerializeField] private Color m_Color;
        [SerializeField] private int m_FontSize;
        [SerializeField] private FontStyle m_FontStyle;

        /// <summary>
        /// Whether to show axis name. 
        /// 是否显示坐标名称。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// the name of axis.
        /// 坐标轴名称。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// Location of axis name.
        /// 坐标轴名称显示位置。
        /// </summary>
        public Location location { get { return m_Location; } set { m_Location = value; } }
        /// <summary>
        /// the offset of axis name and axis line.
        /// 坐标轴名称与轴线之间的偏移。
        /// </summary>
        public Vector2 offset { get { return m_Offset; } set { m_Offset = value; } }
        /// <summary>
        /// Rotation of axis name.
        /// 坐标轴名字旋转，角度值。
        /// </summary>
        public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
        /// <summary>
        /// Color of axis name. 
        /// 坐标轴名称的文字颜色。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// axis name font size. 
        /// 坐标轴名称的文字大小。
        /// </summary>
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
        /// <summary>
        /// axis name font style. 
        /// 坐标轴名称的文字风格。
        /// </summary>
        public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

        public static AxisName defaultAxisName
        {
            get
            {
                return new AxisName()
                {
                    m_Show = false,
                    m_Name = "axisName",
                    m_Location = Location.End,
                    m_Rotate = 0,
                    m_Color = Color.clear,
                    m_FontSize = 18,
                    m_FontStyle = FontStyle.Normal
                };
            }
        }

        public void Copy(AxisName other)
        {
            m_Show = other.show;
            m_Name = other.name;
            m_Location = other.location;
            m_Offset = other.offset;
            m_Rotate = other.rotate;
            m_Color = other.color;
            m_FontSize = other.fontSize;
            m_FontStyle = other.fontStyle;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (AxisName)obj;
            return m_Show == other.show &&
                m_Name.Equals(other.name) &&
                m_Location == other.location &&
                m_Rotate == other.rotate &&
                m_Color == other.color &&
                m_Offset == other.offset &&
                m_FontSize == other.fontSize &&
                m_FontStyle == other.fontStyle;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}