/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the name of axis.
    /// 坐标轴名称。
    /// </summary>
    [Serializable]
    public class AxisName : SubComponent
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
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the name of axis.
        /// 坐标轴名称。
        /// </summary>
        public string name
        {
            get { return m_Name; }
            set { if (PropertyUtility.SetClass(ref m_Name, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Location of axis name.
        /// 坐标轴名称显示位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtility.SetStruct(ref m_Location, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the offset of axis name and axis line.
        /// 坐标轴名称与轴线之间的偏移。
        /// </summary>
        public Vector2 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtility.SetStruct(ref m_Offset, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Rotation of axis name.
        /// 坐标轴名字旋转，角度值。
        /// </summary>
        public float rotate
        {
            get { return m_Rotate; }
            set { if (PropertyUtility.SetStruct(ref m_Rotate, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Color of axis name. 
        /// 坐标轴名称的文字颜色。
        /// </summary>
        public Color color
        {
            get { return m_Color; }
            set { if (PropertyUtility.SetColor(ref m_Color, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// axis name font size. 
        /// 坐标轴名称的文字大小。
        /// </summary>
        public int fontSize
        {
            get { return m_FontSize; }
            set { if (PropertyUtility.SetStruct(ref m_FontSize, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// axis name font style. 
        /// 坐标轴名称的文字风格。
        /// </summary>
        public FontStyle fontStyle
        {
            get { return m_FontStyle; }
            set { if (PropertyUtility.SetStruct(ref m_FontStyle, value)) SetComponentDirty(); }
        }

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

        public AxisName Clone()
        {
            var axisName = new AxisName();
            axisName.show = show;
            axisName.name = name;
            axisName.location = location;
            axisName.offset = offset;
            axisName.rotate = rotate;
            axisName.color = color;
            axisName.fontSize = fontSize;
            axisName.fontStyle = fontStyle;
            return axisName;
        }

        public void Copy(AxisName axisName)
        {
            show = axisName.show;
            name = axisName.name;
            location = axisName.location;
            offset = axisName.offset;
            rotate = axisName.rotate;
            color = axisName.color;
            fontSize = axisName.fontSize;
            fontStyle = axisName.fontStyle;
        }
    }
}