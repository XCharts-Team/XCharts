using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class VisualMapRange : ChildComponent
    {
        [SerializeField] private double m_Min;
        [SerializeField] private double m_Max;
        [SerializeField] private string m_Label;
        [SerializeField] private Color32 m_Color;

        /// <summary>
        /// 范围最小值
        /// </summary>
        public double min { get { return m_Min; } set { m_Min = value; } }
        /// <summary>
        /// 范围最大值
        /// </summary>
        public double max { get { return m_Max; } set { m_Max = value; } }
        /// <summary>
        /// 文字描述
        /// </summary>
        public string label { get { return m_Label; } set { m_Label = value; } }
        /// <summary>
        /// 颜色
        /// </summary>
        public Color32 color { get { return m_Color; } set { m_Color = value; } }

        public bool Contains(double value, double minMaxRange)
        {
            if (m_Min == 0 && m_Max == 0) return false;
            var cmin = System.Math.Abs(m_Min) < 1 ? minMaxRange * m_Min : m_Min;
            var cmax = System.Math.Abs(m_Max) < 1 ? minMaxRange * m_Max : m_Max;
            return value >= cmin && value < cmax;
        }
    }

    /// <summary>
    /// VisualMap component. Mapping data to visual elements such as colors.
    /// ||视觉映射组件。用于进行『视觉编码』，也就是将数据映射到视觉元素（视觉通道）。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(VisualMapHandler), true)]
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

        [SerializeField] private bool m_Show = true;
        [SerializeField] private bool m_ShowUI = false;
        [SerializeField] private Type m_Type = Type.Continuous;
        [SerializeField] private SelectedMode m_SelectedMode = SelectedMode.Multiple;
        [SerializeField] private int m_SerieIndex = 0;
        [SerializeField] private double m_Min = 0;
        [SerializeField] private double m_Max = 0;

        [SerializeField] private double[] m_Range = new double[2] { 0, 0 };
        [SerializeField] private string[] m_Text = new string[2] { "", "" };
        [SerializeField] private float[] m_TextGap = new float[2] { 10f, 10f };
        [SerializeField] private int m_SplitNumber = 5;
        [SerializeField] private bool m_Calculable = false;
        [SerializeField] private bool m_Realtime = true;
        [SerializeField] private float m_ItemWidth = 20f;
        [SerializeField] private float m_ItemHeight = 140f;
        [SerializeField] private float m_ItemGap = 10f;
        [SerializeField] private float m_BorderWidth = 0;
        [SerializeField] private int m_Dimension = -1;
        [SerializeField] private bool m_HoverLink = true;
        [SerializeField] private bool m_AutoMinMax = true;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private Location m_Location = Location.defaultLeft;
        [SerializeField] private bool m_WorkOnLine = true;
        [SerializeField] private bool m_WorkOnArea = false;

        [SerializeField] private List<VisualMapRange> m_OutOfRange = new List<VisualMapRange>() { new VisualMapRange() { color = Color.gray } };
        [SerializeField] private List<VisualMapRange> m_InRange = new List<VisualMapRange>();

        public VisualMapContext context = new VisualMapContext();

        /// <summary>
        /// Whether to enable components. 
        /// ||组件是否生效。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to display components. If set to false, it will not show up, but the data mapping function still exists.
        /// ||是否显示组件。如果设置为 false，不会显示，但是数据映射的功能还存在。
        /// </summary>
        public bool showUI
        {
            get { return m_ShowUI; }
            set { if (PropertyUtil.SetStruct(ref m_ShowUI, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the type of visualmap component.
        /// ||组件类型。
        /// </summary>
        public Type type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the selected mode for Piecewise visualMap.
        /// ||选择模式。
        /// </summary>
        public SelectedMode selectedMode
        {
            get { return m_SelectedMode; }
            set { if (PropertyUtil.SetStruct(ref m_SelectedMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the serie index of visualMap.
        /// ||影响的serie索引。
        /// </summary>
        public int serieIndex
        {
            get { return m_SerieIndex; }
            set { if (PropertyUtil.SetStruct(ref m_SerieIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The minimum allowed. 'min' must be user specified. [visualmap.min, visualmap.max] forms the "domain" of the visualMap.
        /// ||
        /// 允许的最小值。`autoMinMax`为`false`时必须指定。[visualMap.min, visualMap.max] 形成了视觉映射的『定义域』。
        /// </summary>
        public double min
        {
            get { return m_Min; }
            set { if (PropertyUtil.SetStruct(ref m_Min, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The maximum allowed. 'max' must be user specified. [visualmap.min, visualmap.max] forms the "domain" of the visualMap.
        /// ||
        /// 允许的最大值。`autoMinMax`为`false`时必须指定。[visualMap.min, visualMax.max] 形成了视觉映射的『定义域』。
        /// </summary>
        public double max
        {
            get { return m_Max; }
            set { m_Max = (value < min ? min + 1 : value); SetVerticesDirty(); }
        }
        /// <summary>
        /// Specifies the position of the numeric value corresponding to the handle. Range should be within the range of [min,max].
        /// ||
        /// 指定手柄对应数值的位置。range 应在[min,max]范围内。
        /// </summary>
        public double[] range { get { return m_Range; } }
        /// <summary>
        /// Text on both ends.
        /// ||两端的文本，如 ['High', 'Low']。
        /// </summary>
        public string[] text { get { return m_Text; } }
        /// <summary>
        /// The distance between the two text bodies.
        /// ||两端文字主体之间的距离，单位为px。
        /// </summary>
        public float[] textGap { get { return m_TextGap; } }
        /// <summary>
        /// For continuous data, it is automatically evenly divided into several segments 
        /// and automatically matches the size of inRange color list when the default is 0.
        /// ||
        /// 对于连续型数据，自动平均切分成几段，默认为0时自动匹配inRange颜色列表大小。
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtil.SetStruct(ref m_SplitNumber, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether the handle used for dragging is displayed (the handle can be dragged to adjust the selected range).
        /// ||
        /// 是否显示拖拽用的手柄（手柄能拖拽调整选中范围）。
        /// </summary>
        public bool calculable
        {
            get { return m_Calculable; }
            set { if (PropertyUtil.SetStruct(ref m_Calculable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to update in real time while dragging.
        /// ||
        /// 拖拽时，是否实时更新。
        /// </summary>
        public bool realtime
        {
            get { return m_Realtime; }
            set { if (PropertyUtil.SetStruct(ref m_Realtime, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The width of the figure, that is, the width of the color bar.
        /// ||
        /// 图形的宽度，即颜色条的宽度。
        /// </summary>
        public float itemWidth
        {
            get { return m_ItemWidth; }
            set { if (PropertyUtil.SetStruct(ref m_ItemWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The height of the figure, that is, the height of the color bar.
        /// ||
        /// 图形的高度，即颜色条的高度。
        /// </summary>
        public float itemHeight
        {
            get { return m_ItemHeight; }
            set { if (PropertyUtil.SetStruct(ref m_ItemHeight, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 每个图元之间的间隔距离。
        /// </summary>
        public float itemGap
        {
            get { return m_ItemGap; }
            set { if (PropertyUtil.SetStruct(ref m_ItemGap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Border line width.
        /// ||
        /// 边框线宽，单位px。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specifies "which dimension" of the data to map to the visual element. "Data" is series.data.
        /// ||Starting at 1, the default is 0 to take the last dimension in data.
        /// ||
        /// 指定用数据的『哪个维度』，映射到视觉元素上。『数据』即 series.data。从1开始，默认为0取 data 中最后一个维度。
        /// </summary>
        public int dimension
        {
            get { return m_Dimension; }
            set { if (PropertyUtil.SetStruct(ref m_Dimension, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// When the hoverLink function is turned on, when the mouse hovers over the visualMap component, 
        /// the corresponding value of the mouse position is highlighted in the corresponding graphic element in the diagram.
        /// ||Conversely, when the mouse hovers over a graphic element in a diagram, 
        /// the corresponding value of the visualMap component is triangulated in the corresponding position.
        /// ||
        /// 打开 hoverLink 功能时，鼠标悬浮到 visualMap 组件上时，鼠标位置对应的数值 在 图表中对应的图形元素，会高亮。
        /// 反之，鼠标悬浮到图表中的图形元素上时，在 visualMap 组件的相应位置会有三角提示其所对应的数值。
        /// </summary>
        public bool hoverLink
        {
            get { return m_HoverLink; }
            set { if (PropertyUtil.SetStruct(ref m_HoverLink, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Automatically set min, Max value
        /// 自动设置min，max的值
        /// </summary>
        public bool autoMinMax
        {
            get { return m_AutoMinMax; }
            set { if (PropertyUtil.SetStruct(ref m_AutoMinMax, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify whether the layout of component is horizontal or vertical.
        /// ||
        /// 布局方式是横还是竖。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The location of component.
        /// ||组件显示的位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtil.SetClass(ref m_Location, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether the visualmap is work on linestyle of linechart.
        /// ||组件是否对LineChart的LineStyle有效。
        /// </summary>
        public bool workOnLine
        {
            get { return m_WorkOnLine; }
            set { if (PropertyUtil.SetStruct(ref m_WorkOnLine, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether the visualmap is work on areaStyle of linechart.
        /// ||组件是否对LineChart的AreaStyle有效。
        /// </summary>
        public bool workOnArea
        {
            get { return m_WorkOnArea; }
            set { if (PropertyUtil.SetStruct(ref m_WorkOnArea, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Defines a visual color outside of the selected range.
        /// ||定义 在选中范围外 的视觉颜色。
        /// </summary>
        public List<VisualMapRange> outOfRange
        {
            get { return m_OutOfRange; }
            set { if (value != null) { m_OutOfRange = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// 分段式每一段的相关配置。
        /// </summary>
        public List<VisualMapRange> inRange
        {
            get { return m_InRange; }
            set { if (value != null) { m_InRange = value; SetVerticesDirty(); } }
        }

        public override bool vertsDirty { get { return m_VertsDirty || location.anyDirty; } }
        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            location.ClearVerticesDirty();
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            location.ClearComponentDirty();
        }

        public double rangeMin
        {
            get
            {
                if (m_Range[0] == 0 && m_Range[1] == 0) return min;
                else if (m_Range[0] < min || m_Range[0] > max) return min;
                else return m_Range[0];
            }
            set
            {
                if (value >= min && value <= m_Range[1]) m_Range[0] = value;
            }
        }

        public double rangeMax
        {
            get
            {
                if (m_Range[0] == 0 && m_Range[1] == 0) return max;
                if (m_Range[1] >= m_Range[0] && m_Range[1] < max) return m_Range[1];
                else return max;
            }
            set
            {
                if (value >= m_Range[0] && value <= max) m_Range[1] = value;
            }
        }

        public float runtimeRangeMinHeight { get { return (float) ((rangeMin - min) / (max - min) * itemHeight); } }
        public float runtimeRangeMaxHeight { get { return (float) ((rangeMax - min) / (max - min) * itemHeight); } }

        public void AddColors(List<Color32> colors)
        {
            m_InRange.Clear();
            foreach (var color in colors)
            {
                m_InRange.Add(new VisualMapRange()
                {
                    color = color
                });
            }
        }

        public void AddColors(List<string> colors)
        {
            m_InRange.Clear();
            foreach (var str in colors)
            {
                m_InRange.Add(new VisualMapRange()
                {
                    color = ThemeStyle.GetColor(str)
                });
            }
        }

        public Color32 GetColor(double value)
        {
            int index = GetIndex(value);
            if (index == -1)
            {
                if (m_OutOfRange.Count > 0)
                    return m_OutOfRange[0].color;
                else
                    return ChartConst.clearColor32;
            }

            if (m_Type == VisualMap.Type.Piecewise)
            {
                return m_InRange[index].color;
            }
            else
            {
                int splitNumber = m_InRange.Count;
                var diff = (m_Max - m_Min) / (splitNumber - 1);
                var nowMin = m_Min + index * diff;
                var rate = (value - nowMin) / diff;
                if (index == splitNumber - 1)
                    return m_InRange[index].color;
                else
                    return Color32.Lerp(m_InRange[index].color, m_InRange[index + 1].color, (float) rate);
            }
        }

        private bool IsNeedPieceColor(double value, out int index)
        {
            bool flag = false;
            index = -1;
            for (int i = 0; i < m_InRange.Count; i++)
            {
                var range = m_InRange[i];
                if (range.min != 0 || range.max != 0)
                {
                    flag = true;
                    if (range.Contains(value, max - min))
                    {
                        index = i;
                        return true;
                    }
                }
            }
            return flag;
        }

        private Color32 GetPiecesColor(double value)
        {
            foreach (var piece in m_InRange)
            {
                if (piece.Contains(value, max - min))
                {
                    return piece.color;
                }
            }
            if (m_OutOfRange.Count > 0)
                return m_OutOfRange[0].color;
            else
                return ChartConst.clearColor32;
        }

        public int GetIndex(double value)
        {
            int splitNumber = m_InRange.Count;
            if (splitNumber <= 0)
                return -1;
            var index = -1;
            if (IsNeedPieceColor(value, out index))
            {
                return index;
            }
            value = MathUtil.Clamp(value, m_Min, m_Max);

            var diff = (m_Max - m_Min) / (splitNumber - 1);

            for (int i = 0; i < splitNumber; i++)
            {
                if (value <= m_Min + (i + 1) * diff)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public bool IsPiecewise()
        {
            return m_Type == VisualMap.Type.Piecewise;
        }

        public bool IsInSelectedValue(double value)
        {
            if (context.pointerIndex < 0)
                return true;
            else
                return context.pointerIndex == GetIndex(value);
        }

        public double GetValue(Vector3 pos, Rect chartRect)
        {
            var vertical = orient == Orient.Vertical;
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            var pos1 = centerPos + (vertical ? Vector3.down : Vector3.left) * itemHeight / 2;
            var pos2 = centerPos + (vertical ? Vector3.up : Vector3.right) * itemHeight / 2;

            if (vertical)
            {
                if (pos.y < pos1.y)
                    return min;
                else if (pos.y > pos2.y)
                    return max;
                else
                    return min + (pos.y - pos1.y) / (pos2.y - pos1.y) * (max - min);
            }
            else
            {
                if (pos.x < pos1.x)
                    return min;
                else if (pos.x > pos2.x)
                    return max;
                else
                    return min + (pos.x - pos1.x) / (pos2.x - pos1.x) * (max - min);
            }
        }

        public bool IsInRect(Vector3 local, Rect chartRect, float triangleLen = 20)
        {
            var centerPos = new Vector3(chartRect.x, chartRect.y) + location.GetPosition(chartRect.width, chartRect.height);
            var diff = calculable ? triangleLen : 0;

            if (local.x >= centerPos.x - itemWidth / 2 - diff &&
                local.x <= centerPos.x + itemWidth / 2 + diff &&
                local.y >= centerPos.y - itemHeight / 2 - diff &&
                local.y <= centerPos.y + itemHeight / 2 + diff)
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

                return local.x >= centerPos.x - itemWidth / 2 &&
                    local.x <= centerPos.x + itemWidth / 2 &&
                    local.y >= pos1.y + runtimeRangeMinHeight &&
                    local.y <= pos1.y + runtimeRangeMaxHeight;
            }
            else
            {
                var pos1 = centerPos + Vector3.left * itemHeight / 2;
                return local.x >= pos1.x + runtimeRangeMinHeight &&
                    local.x <= pos1.x + runtimeRangeMaxHeight &&
                    local.y >= centerPos.y - itemWidth / 2 &&
                    local.y <= centerPos.y + itemWidth / 2;
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

                return local.x >= cpos.x - radius &&
                    local.x <= cpos.x + radius &&
                    local.y >= cpos.y - radius &&
                    local.y <= cpos.y + radius;
            }
            else
            {
                var radius = triangleLen / 2;
                var pos1 = centerPos + Vector3.left * itemHeight / 2;
                var cpos = new Vector3(pos1.x + runtimeRangeMinHeight, pos1.y + itemWidth / 2 + radius);

                return local.x >= cpos.x - radius &&
                    local.x <= cpos.x + radius &&
                    local.y >= cpos.y - radius &&
                    local.y <= cpos.y + radius;
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

                return local.x >= cpos.x - radius &&
                    local.x <= cpos.x + radius &&
                    local.y >= cpos.y - radius &&
                    local.y <= cpos.y + radius;
            }
            else
            {
                var radius = triangleLen / 2;
                var pos1 = centerPos + Vector3.left * itemHeight / 2;
                var cpos = new Vector3(pos1.x + runtimeRangeMaxHeight + radius, pos1.y + itemWidth / 2 + radius);

                return local.x >= cpos.x - radius &&
                    local.x <= cpos.x + radius &&
                    local.y >= cpos.y - radius &&
                    local.y <= cpos.y + radius;
            }
        }
    }
}