
using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the name of axis.
    /// 坐标轴名称。
    /// </summary>
    [Serializable]
    public class AxisName : ChildComponent
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
        [SerializeField] private string m_Formatter;
        [SerializeField] private Location m_Location;
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();

        /// <summary>
        /// Whether to show axis name. 
        /// 是否显示坐标名称。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the name of axis.
        /// 坐标轴名称。
        /// </summary>
        public string name
        {
            get { return m_Name; }
            set { if (PropertyUtil.SetClass(ref m_Name, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The formatter of indicator's name.
        /// 指示器名称显示的格式器。可用在雷达图。
        /// </summary>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Location of axis name.
        /// 坐标轴名称显示位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtil.SetStruct(ref m_Location, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// The text style of axis name.
        /// 文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetComponentDirty(); }
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
                    m_TextStyle = new TextStyle(),
                };
            }
        }

        public AxisName Clone()
        {
            var axisName = new AxisName();
            axisName.show = show;
            axisName.name = name;
            axisName.location = location;
            axisName.textStyle.Copy(textStyle);
            return axisName;
        }

        public void Copy(AxisName axisName)
        {
            show = axisName.show;
            name = axisName.name;
            location = axisName.location;
            textStyle.Copy(axisName.textStyle);
        }
    }
}