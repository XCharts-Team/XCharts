using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the name of axis.
    /// ||坐标轴名称。
    /// </summary>
    [Serializable]
    public class AxisName : ChildComponent
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private string m_Name;
        [SerializeField][Since("v3.1.0")] private bool m_OnZero;
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle();

        /// <summary>
        /// Whether to show axis name.
        /// ||是否显示坐标轴名称。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the name of axis.
        /// ||坐标轴名称。
        /// </summary>
        public string name
        {
            get { return m_Name; }
            set { if (PropertyUtil.SetClass(ref m_Name, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether the axis name position are the same with 0 position of YAxis.
        /// ||坐标轴名称的位置是否保持和Y轴0刻度一致。
        /// </summary>
        public bool onZero
        {
            get { return m_OnZero; }
            set { if (PropertyUtil.SetStruct(ref m_OnZero, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The text style of axis name.
        /// ||文本样式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
        }

        public static AxisName defaultAxisName
        {
            get
            {
                var axisName = new AxisName()
                {
                    m_Show = false,
                    m_Name = "axisName",
                    m_LabelStyle = new LabelStyle()
                };
                axisName.labelStyle.position = LabelStyle.Position.End;
                return axisName;
            }
        }

        public AxisName Clone()
        {
            var axisName = new AxisName();
            axisName.show = show;
            axisName.name = name;
            axisName.m_LabelStyle.Copy(m_LabelStyle);
            return axisName;
        }

        public void Copy(AxisName axisName)
        {
            show = axisName.show;
            name = axisName.name;
            m_LabelStyle.Copy(axisName.labelStyle);
        }
    }
}