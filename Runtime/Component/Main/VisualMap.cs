/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// VisualMap component.
    /// 视觉映射组件。用于进行『视觉编码』，也就是将数据映射到视觉元素（视觉通道）。
    /// </summary>
    [System.Serializable]
    public class VisualMap : MainComponent
    {
        /// <summary>
        /// 类型。分为连续型和分段型。
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// 连续型。
            /// </summary>
            Continuous,
            /// <summary>
            /// 分段型。
            /// </summary>
            Piecewise
        }

        /// <summary>
        /// 选择模式
        /// </summary>
        public enum SelectedMode
        {
            /// <summary>
            /// 多选。
            /// </summary>
            Multiple,
            /// <summary>
            /// 单选。
            /// </summary>
            Single
        }

        [SerializeField] private bool m_Enable = false;
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Type m_Type = Type.Continuous;
        [SerializeField] private SelectedMode m_SelectedMode = SelectedMode.Multiple;
        [SerializeField] private float m_Min = 0;
        [SerializeField] private float m_Max = 100f;

        [SerializeField] private float[] m_Range = new float[2] { 0, 100f };
        [SerializeField] private string[] m_Text = new string[2] { "", "" };
        [SerializeField] private float[] m_TextGap = new float[2] { 10f, 10f };
        [SerializeField] private int m_SplitNumber = 5;
        [SerializeField] private bool m_Calculable = false;
        [SerializeField] private bool m_Realtime = true;
        [SerializeField] private float m_ItemWidth = 20f;
        [SerializeField] private float m_ItemHeight = 140f;
        [SerializeField] private float m_BorderWidth = 0;
        [SerializeField] private int m_Dimension = 0;
        [SerializeField] private bool m_HoverLink = true;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private Location m_Location = Location.defaultLeft;
        [SerializeField] private List<Color> m_InRange = new List<Color>();
        [SerializeField] private List<Color> m_OutOfRange = new List<Color>();

        /// <summary>
        /// 是否开启组件功能。
        /// </summary>
        public bool enable
        {
            get { return m_Enable; }
            set { if (PropertyUtility.SetStruct(ref m_Enable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否显示组件。如果设置为 false，不会显示，但是数据映射的功能还存在。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Enable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 组件类型。
        /// </summary>
        public Type type
        {
            get { return m_Type; }
            set { if (PropertyUtility.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 选择模式。
        /// </summary>
        public SelectedMode selectedMode
        {
            get { return m_SelectedMode; }
            set { if (PropertyUtility.SetStruct(ref m_SelectedMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 允许的最小值。'min' 必须用户指定。[visualMap.min, visualMap.max] 形成了视觉映射的『定义域』。
        /// </summary>
        public float min
        {
            get { return m_Min; }
            set { if (PropertyUtility.SetStruct(ref m_Min, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 允许的最大值。'max' 必须用户指定。[visualMap.min, visualMax.max] 形成了视觉映射的『定义域』。
        /// </summary>
        public float max
        {
            get { return m_Max; }
            set { m_Max = value < min ? min + 1 : value; SetVerticesDirty(); }
        }
        /// <summary>
        /// 指定手柄对应数值的位置。range 应在 min max 范围内。
        /// </summary>
        public float[] range { get { return m_Range; } }
        /// <summary>
        /// 两端的文本，如 ['High', 'Low']。
        /// </summary>
        public string[] text { get { return m_Text; } }
        /// <summary>
        /// 两端文字主体之间的距离，单位为px。
        /// </summary>
        public float[] textGap { get { return m_TextGap; } }
        /// <summary>
        /// 对于连续型数据，自动平均切分成几段，默认为0时自动匹配inRange颜色列表大小。
        /// </summary>
        /// <value></value>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtility.SetStruct(ref m_SplitNumber, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否显示拖拽用的手柄（手柄能拖拽调整选中范围）。
        /// </summary>
        public bool calculable
        {
            get { return m_Calculable; }
            set { if (PropertyUtility.SetStruct(ref m_Calculable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 拖拽时，是否实时更新。
        /// </summary>
        public bool realtime
        {
            get { return m_Realtime; }
            set { if (PropertyUtility.SetStruct(ref m_Realtime, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 图形的宽度，即颜色条的宽度。
        /// </summary>
        public float itemWidth
        {
            get { return m_ItemWidth; }
            set { if (PropertyUtility.SetStruct(ref m_ItemWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 图形的高度，即颜色条的高度。
        /// </summary>
        public float itemHeight
        {
            get { return m_ItemHeight; }
            set { if (PropertyUtility.SetStruct(ref m_ItemHeight, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框线宽，单位px。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtility.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 指定用数据的『哪个维度』，映射到视觉元素上。『数据』即 series.data。从1开始，默认为0取 data 中最后一个维度。
        /// </summary>
        public int dimension
        {
            get { return m_Dimension; }
            set { if (PropertyUtility.SetStruct(ref m_Dimension, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 打开 hoverLink 功能时，鼠标悬浮到 visualMap 组件上时，鼠标位置对应的数值 在 图表中对应的图形元素，会高亮。
        /// 反之，鼠标悬浮到图表中的图形元素上时，在 visualMap 组件的相应位置会有三角提示其所对应的数值。
        /// </summary>
        /// <value></value>
        public bool hoverLink
        {
            get { return m_HoverLink; }
            set { if (PropertyUtility.SetStruct(ref m_HoverLink, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify whether the layout of component is horizontal or vertical. 
        /// 布局方式是横还是竖。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtility.SetStruct(ref m_Orient, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The location of component.
        /// 组件显示的位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtility.SetClass(ref m_Location, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 定义 在选中范围中 的视觉颜色。
        /// </summary>
        public List<Color> inRange
        {
            get { return m_InRange; }
            set { if (value != null) { m_InRange = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// 定义 在选中范围外 的视觉颜色。
        /// </summary>
        public List<Color> outOfRange
        {
            get { return m_OutOfRange; }
            set { if (value != null) { m_OutOfRange = value; SetVerticesDirty(); } }
        }

        public override bool vertsDirty { get { return m_VertsDirty || location.anyDirty; } }
        internal override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            location.ClearVerticesDirty();
        }

        internal override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            location.ClearComponentDirty();
        }

        /// <summary>
        /// 鼠标悬停选中的index
        /// </summary>
        /// <value></value>
        public int runtimeSelectedIndex { get; internal set; }
        public float runtimeSelectedValue { get; internal set; }
        /// <summary>
        /// the current pointer position.
        /// 当前鼠标位置。
        /// </summary>
        public Vector2 runtimePointerPos { get; internal set; }
        public bool runtimeIsVertical { get { return orient == Orient.Vertical; } }
        public float rangeMin
        {
            get
            {
                if (m_Range[0] < min || m_Range[0] > max) return min;
                else return m_Range[0];
            }
            set
            {
                if (value >= min && value <= m_Range[1]) m_Range[0] = value;
            }
        }

        public float rangeMax
        {
            get
            {
                if (m_Range[1] >= m_Range[0] && m_Range[1] < max) return m_Range[1];
                else return max;
            }
            set
            {
                if (value >= m_Range[0] && value <= max) m_Range[1] = value;
            }
        }

        public int runtimeSplitNumber
        {
            get
            {
                if (splitNumber > 0 && splitNumber <= m_InRange.Count) return splitNumber;
                else return inRange.Count;
            }
        }

        public float runtimeRangeMinHeight { get { return (rangeMin - min) / (max - min) * itemHeight; } }
        public float runtimeRangeMaxHeight { get { return (rangeMax - min) / (max - min) * itemHeight; } }

        private List<Color> m_RtInRange = new List<Color>();
        public List<Color> runtimeInRange
        {
            get
            {
                if (splitNumber == 0 || m_InRange.Count >= splitNumber || m_InRange.Count < 1) return m_InRange;
                else
                {
                    if (m_RtInRange.Count != runtimeSplitNumber)
                    {
                        m_RtInRange.Clear();
                        var total = max - min;
                        var diff1 = total / (m_InRange.Count - 1);
                        var diff2 = total / splitNumber;

                        var inCount = 0;
                        var inValue = min;
                        var rtValue = min;


                        for (int i = 0; i < splitNumber; i++)
                        {
                            rtValue += diff2;
                            if (rtValue > inValue + diff1)
                            {
                                inValue += diff1;
                                inCount++;
                            }
                            if (i == splitNumber - 1)
                            {
                                m_RtInRange.Add(m_InRange[m_InRange.Count - 1]);
                            }
                            else
                            {
                                var rate = (rtValue - inValue) / diff1;
                                m_RtInRange.Add(Color.Lerp(m_InRange[inCount], m_InRange[inCount + 1], rate));
                            }
                        }
                    }
                    return m_RtInRange;
                }
            }
        }

        public Color GetColor(float value)
        {
            int splitNumber = runtimeInRange.Count;
            if (splitNumber <= 0) return Color.clear;
            value = Mathf.Clamp(value, min, max);

            var diff = (max - min) / (splitNumber - 1);
            var index = GetIndex(value);
            var nowMin = min + index * diff;
            var rate = (value - nowMin) / diff;
            if (index == splitNumber - 1) return runtimeInRange[index];
            else return Color.Lerp(runtimeInRange[index], runtimeInRange[index + 1], rate);
        }

        public int GetIndex(float value)
        {
            int splitNumber = runtimeInRange.Count;
            if (splitNumber <= 0) return -1;
            value = Mathf.Clamp(value, min, max);

            var diff = (max - min) / (splitNumber - 1);
            var index = -1;
            for (int i = 0; i < splitNumber; i++)
            {
                if (value <= min + (i + 1) * diff)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public bool IsInSelectedValue(float value)
        {
            if (runtimeSelectedIndex < 0) return true;
            else
            {
                return runtimeSelectedIndex == GetIndex(value);
            }
        }

        public float GetValue(Vector3 pos, Rect chartRect)
        {
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            var pos1 = centerPos + (runtimeIsVertical ? Vector3.down : Vector3.left) * itemHeight / 2;
            var pos2 = centerPos + (runtimeIsVertical ? Vector3.up : Vector3.right) * itemHeight / 2;
            if (runtimeIsVertical)
            {
                if (pos.y < pos1.y) return min;
                else if (pos.y > pos2.y) return max;
                else return min + (pos.y - pos1.y) / (pos2.y - pos1.y) * (max - min);
            }
            else
            {
                if (pos.x < pos1.x) return min;
                else if (pos.x > pos2.x) return max;
                else return min + (pos.x - pos1.x) / (pos2.x - pos1.x) * (max - min);
            }
        }

        public bool IsInRect(Vector3 local, Rect chartRect, float triangleLen = 20)
        {
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            var diff = calculable ? triangleLen : 0;
            if (local.x >= centerPos.x - itemWidth / 2 - diff && local.x <= centerPos.x + itemWidth / 2 + diff &&
                local.y >= centerPos.y - itemHeight / 2 - diff && local.y <= centerPos.y + itemHeight / 2 + diff)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInRangeRect(Vector3 local, Rect chartRect)
        {
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            if (orient == Orient.Vertical)
            {
                var pos1 = centerPos + Vector3.down * itemHeight / 2;
                return local.x >= centerPos.x - itemWidth / 2 && local.x <= centerPos.x + itemWidth / 2 &&
                local.y >= pos1.y + runtimeRangeMinHeight && local.y <= pos1.y + runtimeRangeMaxHeight;
            }
            else
            {
                var pos1 = centerPos + Vector3.left * itemHeight / 2;
                return local.x >= pos1.x + runtimeRangeMinHeight && local.x <= pos1.x + runtimeRangeMaxHeight &&
                local.y >= centerPos.y - itemWidth / 2 && local.y <= centerPos.y + itemWidth / 2;
            }
        }

        public bool IsInRangeMinRect(Vector3 local, Rect chartRect, float triangleLen)
        {
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            if (orient == Orient.Vertical)
            {
                var radius = triangleLen / 2;
                var pos1 = centerPos + Vector3.down * itemHeight / 2;
                var cpos = new Vector3(pos1.x + itemWidth / 2 + radius, pos1.y + runtimeRangeMinHeight - radius);

                return local.x >= cpos.x - radius && local.x <= cpos.x + radius &&
                local.y >= cpos.y - radius && local.y <= cpos.y + radius;
            }
            else
            {
                var radius = triangleLen / 2;
                var pos1 = centerPos + Vector3.left * itemHeight / 2;
                var cpos = new Vector3(pos1.x + runtimeRangeMinHeight, pos1.y + itemWidth / 2 + radius);
                return local.x >= cpos.x - radius && local.x <= cpos.x + radius &&
                local.y >= cpos.y - radius && local.y <= cpos.y + radius;
            }
        }

        public bool IsInRangeMaxRect(Vector3 local, Rect chartRect, float triangleLen)
        {
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            if (orient == Orient.Vertical)
            {
                var radius = triangleLen / 2;
                var pos1 = centerPos + Vector3.down * itemHeight / 2;
                var cpos = new Vector3(pos1.x + itemWidth / 2 + radius, pos1.y + runtimeRangeMaxHeight + radius);

                return local.x >= cpos.x - radius && local.x <= cpos.x + radius &&
                local.y >= cpos.y - radius && local.y <= cpos.y + radius;
            }
            else
            {
                var radius = triangleLen / 2;
                var pos1 = centerPos + Vector3.left * itemHeight / 2;
                var cpos = new Vector3(pos1.x + runtimeRangeMaxHeight + radius, pos1.y + itemWidth / 2 + radius);
                return local.x >= cpos.x - radius && local.x <= cpos.x + radius &&
                local.y >= cpos.y - radius && local.y <= cpos.y + radius;
            }
        }
    }
}