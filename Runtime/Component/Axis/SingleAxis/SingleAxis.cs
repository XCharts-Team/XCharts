using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Single axis.
    /// |单轴。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(SingleAxisHander), true)]
    public class SingleAxis : Axis, IUpdateRuntimeData
    {
        [SerializeField] protected Orient m_Orient = Orient.Horizonal;
        [SerializeField] private float m_Left = 0.1f;
        [SerializeField] private float m_Right = 0.1f;
        [SerializeField] private float m_Top = 0f;
        [SerializeField] private float m_Bottom = 0.2f;
        [SerializeField] private float m_Width = 0;
        [SerializeField] private float m_Height = 50;

        /// <summary>
        /// Orientation of the axis. By default, it's 'Horizontal'. You can set it to be 'Vertical' to make a vertical axis.
        /// |坐标轴朝向。默认为水平朝向。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// |组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the right side of the container.
        /// |组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the top side of the container.
        /// |组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the bottom side of the container.
        /// |组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetAllDirty(); }
        }
        /// <summary>
        /// width of axis.
        /// |坐标轴宽。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetAllDirty(); }
        }
        /// <summary>
        /// height of axis.
        /// |坐标轴高。
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetAllDirty(); }
        }

        public void UpdateRuntimeData(float chartX, float chartY, float chartWidth, float chartHeight)
        {
            context.left = left <= 1 ? left * chartWidth : left;
            context.bottom = bottom <= 1 ? bottom * chartHeight : bottom;
            context.top = top <= 1 ? top * chartHeight : top;
            context.right = right <= 1 ? right * chartWidth : right;

            context.height = height <= 1 ? height * chartHeight : height;

            if (m_Orient == Orient.Horizonal)
            {
                context.width = width == 0 ?
                    chartWidth - context.left - context.right :
                    (width <= 1 ? chartWidth * width : width);
            }
            else
            {
                context.width = width == 0 ?
                    chartHeight - context.top - context.bottom :
                    (width <= 1 ? chartHeight * width : width);
            }

            if (context.left != 0 && context.right == 0)
                context.x = chartX + context.left;
            else if (context.left == 0 && context.right != 0)
                context.x = chartX + chartWidth - context.right - context.width;
            else
                context.x = chartX + context.left;

            if (context.bottom != 0 && context.top == 0)
                context.y = chartY + context.bottom;
            else if (context.bottom == 0 && context.top != 0)
                context.y = chartY + chartHeight - context.top - context.height;
            else
                context.y = chartY + context.bottom;

            context.position = new Vector3(context.x, context.y);
        }

        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Category;
            m_Min = 0;
            m_Max = 0;
            m_SplitNumber = 0;
            m_BoundaryGap = true;
            m_Position = AxisPosition.Bottom;
            m_Offset = 0;

            m_Left = 0.1f;
            m_Right = 0.1f;
            m_Top = 0;
            m_Bottom = 0.2f;
            m_Width = 0;
            m_Height = 50;

            m_Data = new List<string>() { "x1", "x2", "x3", "x4", "x5" };
            m_Icons = new List<Sprite>(5);
            splitLine.show = false;
            splitLine.lineStyle.type = LineStyle.Type.None;
            axisLabel.textLimit.enable = true;
            axisTick.showStartTick = true;
            axisTick.showEndTick = true;
        }
    }
}