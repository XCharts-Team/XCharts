/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    /// <summary>
    /// DataZoom component is used for zooming a specific area,
    /// which enables user to investigate data in detail,
    /// or get an overview of the data, or get rid of outlier points.
    /// 
    /// <para>DataZoom 组件 用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。</para>
    /// </summary>
    [System.Serializable]
    public class DataZoom : MainComponent
    {
        /// <summary>
        /// Generally dataZoom component zoom or roam coordinate system through data filtering
        /// and set the windows of axes internally.
        /// Its behaviours vary according to filtering mode settings.
        /// 
        /// dataZoom 的运行原理是通过 数据过滤 来达到 数据窗口缩放 的效果。数据过滤模式的设置不同，效果也不同。
        /// </summary>
        public enum FilterMode
        {
            /// <summary>
            /// data that outside the window will be filtered, which may lead to some changes of windows of other axes.
            /// For each data item, it will be filtered if one of the relevant dimensions is out of the window.
            /// 
            /// 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只要有一个维度在数据窗口外，整个数据项就会被过滤掉。
            /// </summary>
            Filter,
            /// <summary>
            /// data that outside the window will be filtered, which may lead to some changes of windows of other axes.
            /// For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.
            /// 
            /// 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只有当全部维度都在数据窗口同侧外部，整个数据项才会被过滤掉。
            /// </summary>
            WeakFilter,
            /// <summary>
            /// data that outside the window will be set to NaN, which will not lead to changes of windows of other axes. 
            /// 
            /// 当前数据窗口外的数据，被 设置为空。即 不会 影响其他轴的数据范围。
            /// </summary>
            Empty,
            /// <summary>
            /// Do not filter data.
            /// 不过滤数据，只改变数轴范围。
            /// </summary>
            None
        }
        /// <summary>
        /// The value type of start and end.取值类型
        /// </summary>
        public enum RangeMode
        {
            //Value,
            /// <summary>
            /// percent value. 百分比
            /// </summary>
            Percent
        }
        [SerializeField] private bool m_Enable;
        [SerializeField] private FilterMode m_FilterMode;
        [SerializeField] private List<int> m_XAxisIndexs = new List<int>() { 0 };
        [SerializeField] private List<int> m_YAxisIndexs = new List<int>() { };
        [SerializeField] private bool m_SupportInside;
        [SerializeField] private bool m_SupportInsideScroll = true;
        [SerializeField] private bool m_SupportInsideDrag = true;
        [SerializeField] private bool m_SupportSlider;
        [SerializeField] private bool m_SupportSelect;
        [SerializeField] private bool m_ShowDataShadow;
        [SerializeField] private bool m_ShowDetail;
        [SerializeField] private bool m_ZoomLock;
        [SerializeField] private bool m_Realtime;
        [SerializeField] protected Color32 m_FillerColor;
        [SerializeField] protected Color32 m_BorderColor;
        [SerializeField] protected float m_BorderWidth;
        [SerializeField] protected Color32 m_BackgroundColor;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;
        [SerializeField] private RangeMode m_RangeMode;
        [SerializeField] private float m_Start;
        [SerializeField] private float m_End;
        [SerializeField] private float m_StartValue;
        [SerializeField] private float m_EndValue;
        [SerializeField] private int m_MinShowNum = 1;
        [Range(1f, 20f)]
        [SerializeField] private float m_ScrollSensitivity = 1.1f;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private TextStyle m_TextStyle;
        [SerializeField] private LineStyle m_LineStyle = new LineStyle(LineStyle.Type.Solid);
        [SerializeField] private AreaStyle m_AreaStyle = new AreaStyle();

        /// <summary>
        /// Whether to show dataZoom. 
        /// 是否显示缩放区域。
        /// </summary>
        public bool enable
        {
            get { return m_Enable; }
            set { if (PropertyUtil.SetStruct(ref m_Enable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The mode of data filter. 
        /// 数据过滤类型。
        /// </summary>
        public FilterMode filterMode
        {
            get { return m_FilterMode; }
            set { if (PropertyUtil.SetStruct(ref m_FilterMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify which xAxis is controlled by the dataZoom. 
        /// 控制的 x 轴索引列表。
        /// </summary>
        public List<int> xAxisIndexs
        {
            get { return m_XAxisIndexs; }
            set { if (PropertyUtil.SetClass(ref m_XAxisIndexs, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify which yAxis is controlled by the dataZoom. 
        /// 控制的 y 轴索引列表。
        /// </summary>
        public List<int> yAxisIndexs
        {
            get { return m_YAxisIndexs; }
            set { if (PropertyUtil.SetClass(ref m_YAxisIndexs, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether built-in support is supported. 
        /// Built into the coordinate system to allow the user to zoom in and out of the coordinate system by mouse dragging, 
        /// mouse wheel, finger swiping (on the touch screen).
        /// 
        /// 是否支持内置。内置于坐标系中，使用户可以在坐标系上通过鼠标拖拽、鼠标滚轮、手指滑动（触屏上）来缩放或漫游坐标系。
        /// </summary>
        public bool supportInside
        {
            get { return m_SupportInside; }
            set { if (PropertyUtil.SetStruct(ref m_SupportInside, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否支持坐标系内滚动
        /// </summary>
        public bool supportInsideScroll
        {
            get { return m_SupportInsideScroll; }
            set { if (PropertyUtil.SetStruct(ref m_SupportInsideScroll, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否支持坐标系内拖拽
        /// </summary>
        public bool supportInsideDrag
        {
            get { return m_SupportInsideDrag; }
            set { if (PropertyUtil.SetStruct(ref m_SupportInsideDrag, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether a slider is supported. There are separate sliders on which the user zooms or roams.
        /// 是否支持滑动条。有单独的滑动条，用户在滑动条上进行缩放或漫游。
        /// </summary>
        public bool supportSlider
        {
            get { return m_SupportSlider; }
            set { if (PropertyUtil.SetStruct(ref m_SupportSlider, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否支持框选。提供一个选框进行数据区域缩放。
        /// </summary>
        private bool supportSelect
        {
            get { return m_SupportSelect; }
            set { if (PropertyUtil.SetStruct(ref m_SupportSelect, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show data shadow, to indicate the data tendency in brief.
        /// default:true
        /// 是否显示数据阴影。数据阴影可以简单地反应数据走势。
        /// </summary>
        public bool showDataShadow
        {
            get { return m_ShowDataShadow; }
            set { if (PropertyUtil.SetStruct(ref m_ShowDataShadow, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show detail, that is, show the detailed data information when dragging.
        /// 是否显示detail，即拖拽时候显示详细数值信息。
        /// [default: false]
        /// </summary>
        public bool showDetail
        {
            get { return m_ShowDetail; }
            set { if (PropertyUtil.SetStruct(ref m_ShowDetail, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify whether to lock the size of window (selected area).
        /// default:false
        /// 是否锁定选择区域（或叫做数据窗口）的大小。
        /// 如果设置为 true 则锁定选择区域的大小，也就是说，只能平移，不能缩放。
        /// </summary>
        public bool zoomLock
        {
            get { return m_ZoomLock; }
            set { if (PropertyUtil.SetStruct(ref m_ZoomLock, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show data shadow in dataZoom-silder component, to indicate the data tendency in brief.
        /// default:true
        /// 拖动时，是否实时更新系列的视图。如果设置为 false，则只在拖拽结束的时候更新。默认为true，暂不支持修改。
        /// </summary>
        public bool realtime { get { return true; } }
        /// <summary>
        /// The background color of the component.
        /// 组件的背景颜色。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetStruct(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of dataZoom data area.
        /// 数据区域颜色。
        /// </summary>
        public Color32 fillerColor
        {
            get { return m_FillerColor; }
            set { if (PropertyUtil.SetColor(ref m_FillerColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the color of dataZoom border.
        /// 边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 边框宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the bottom side of the container.
        /// bottom value is a instant pixel value like 10 or float value [0-1].
        /// default:10
        /// 组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the top side of the container.
        /// top value is a instant pixel value like 10 or float value [0-1].
        /// default:10
        /// 组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the left side of the container.
        /// left value is a instant pixel value like 10 or float value [0-1].
        /// default:10
        /// 组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the right side of the container.
        /// right value is a instant pixel value like 10 or float value [0-1].
        /// default:10
        /// 组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Use absolute value or percent value in DataZoom.start and DataZoom.end.
        /// default:RangeMode.Percent.
        /// 取绝对值还是百分比。
        /// </summary>
        public RangeMode rangeMode
        {
            get { return m_RangeMode; }
            set { if (PropertyUtil.SetStruct(ref m_RangeMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The start percentage of the window out of the data extent, in the range of 0 ~ 100.
        /// [default:30].
        /// 数据窗口范围的起始百分比。范围是：0 ~ 100。
        /// </summary>
        public float start
        {
            get { return m_Start; }
            set { m_Start = value; if (m_Start < 0) m_Start = 0; if (m_Start > 100) m_Start = 100; SetVerticesDirty(); }
        }
        /// <summary>
        /// The end percentage of the window out of the data extent, in the range of 0 ~ 100.
        /// default:70
        /// 数据窗口范围的结束百分比。范围是：0 ~ 100。
        /// </summary>
        public float end
        {
            get { return m_End; }
            set { m_End = value; if (m_End < 0) m_End = 0; if (m_End > 100) m_End = 100; SetVerticesDirty(); }
        }
        /// <summary>
        /// Minimum number of display data. Minimum number of data displayed when DataZoom is enlarged to maximum.
        /// 最小显示数据个数。当DataZoom放大到最大时，最小显示的数据个数。
        /// </summary>
        public int minShowNum
        {
            get { return m_MinShowNum; }
            set { if (PropertyUtil.SetStruct(ref m_MinShowNum, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The sensitivity of dataZoom scroll.
        /// The larger the number, the more sensitive it is.
        /// default:10
        /// 缩放区域组件的敏感度。值越高每次缩放所代表的数据越多。
        /// </summary>
        public float scrollSensitivity
        {
            get { return m_ScrollSensitivity; }
            set { if (PropertyUtil.SetStruct(ref m_ScrollSensitivity, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// Specify whether the layout of dataZoom component is horizontal or vertical. What's more, 
        /// it indicates whether the horizontal axis or vertical axis is controlled by default in catesian coordinate system.
        /// 布局方式是横还是竖。不仅是布局方式，对于直角坐标系而言，也决定了，缺省情况控制横向数轴还是纵向数轴。
        /// </summary>
        /// <value></value>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// font style.
        /// 文字格式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 阴影线条样式。
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (PropertyUtil.SetClass(ref m_LineStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 阴影填充样式。
        /// </summary>
        public AreaStyle areaStyle
        {
            get { return m_AreaStyle; }
            set { if (PropertyUtil.SetClass(ref m_AreaStyle, value)) SetComponentDirty(); }
        }
        public int index { get; internal set; }
        public float runtimeX { get; private set; }
        public float runtimeY { get; private set; }
        public float runtimeWidth { get; private set; }
        public float runtimeHeight { get; private set; }
        public bool runtimeDrag { get; internal set; }
        public bool runtimeCoordinateDrag { get; internal set; }
        public bool runtimeStartDrag { get; internal set; }
        public bool runtimeEndDrag { get; internal set; }
        /// <summary>
        /// 运行时实际范围的开始值
        /// </summary>
        public double runtimeStartValue { get; internal set; }
        /// <summary>
        /// 运行时实际范围的结束值
        /// </summary>
        public double runtimeEndValue { get; internal set; }
        public bool runtimeInvert { get; set; }

        class AxisIndexValueInfo
        {
            public double min;
            public double max;
        }
        private Dictionary<int, AxisIndexValueInfo> m_XAxisIndexInfos = new Dictionary<int, AxisIndexValueInfo>();
        private Dictionary<int, AxisIndexValueInfo> m_YAxisIndexInfos = new Dictionary<int, AxisIndexValueInfo>();

        /// <summary>
        /// The start label.
        /// 组件的开始信息文本。
        /// </summary>
        private ChartText m_StartLabel { get; set; }
        /// <summary>
        /// The end label.
        /// 组件的结束信息文本。
        /// </summary>
        private ChartText m_EndLabel { get; set; }

        public static DataZoom defaultDataZoom
        {
            get
            {
                return new DataZoom()
                {
                    supportInside = true,
                    supportSlider = true,
                    filterMode = FilterMode.None,
                    xAxisIndexs = new List<int>() { 0 },
                    yAxisIndexs = new List<int>() { },
                    showDataShadow = true,
                    showDetail = false,
                    zoomLock = false,
                    m_Bottom = 10,
                    m_Left = 10,
                    m_Right = 10,
                    m_Top = 0.9f,
                    rangeMode = RangeMode.Percent,
                    start = 30,
                    end = 70,
                    m_Orient = Orient.Horizonal,
                    m_ScrollSensitivity = 10,
                    m_TextStyle = new TextStyle(),
                    m_LineStyle = new LineStyle(LineStyle.Type.Solid)
                    {
                        opacity = 0.3f
                    },
                    m_AreaStyle = new AreaStyle()
                    {
                        show = true,
                        opacity = 0.3f,
                    },
                };
            }
        }

        /// <summary>
        /// 给定的坐标是否在缩放区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startX"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IsInZoom(Vector2 pos)
        {
            if (pos.x < runtimeX - 1 || pos.x > runtimeX + runtimeWidth + 1 ||
                pos.y < runtimeY - 1 || pos.y > runtimeY + runtimeHeight + 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 给定的坐标是否在选中区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsInSelectedZoom(Vector2 pos)
        {
            switch (m_Orient)
            {
                case Orient.Horizonal:
                    var start = runtimeX + runtimeWidth * m_Start / 100;
                    var end = runtimeX + runtimeWidth * m_End / 100;
                    return ChartHelper.IsInRect(pos, start, end, runtimeY, runtimeY + runtimeHeight);
                case Orient.Vertical:
                    start = runtimeY + runtimeHeight * m_Start / 100;
                    end = runtimeY + runtimeHeight * m_End / 100;
                    return ChartHelper.IsInRect(pos, runtimeX, runtimeX + runtimeWidth, start, end);
                default: return false;
            }
        }


        public bool IsInSelectedZoom(int totalIndex, int index, bool invert)
        {
            if (totalIndex <= 0) return false;
            var tstart = invert ? 100 - end : start;
            var tend = invert ? 100 - start : end;
            var range = Mathf.RoundToInt(totalIndex * (tend - tstart) / 100);
            var min = Mathf.FloorToInt(totalIndex * tstart / 100);
            var max = Mathf.CeilToInt(totalIndex * tend / 100);
            if (min == 0) max = min + range;
            if (max == totalIndex) min = max - range;
            var flag = index >= min && index < min + range;
            //Debug.LogError("check:" + index + "," + totalIndex + "," + range + "," + min + "," + max + "," + flag);
            return flag;
        }

        /// <summary>
        /// 给定的坐标是否在开始活动条触发区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startX"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IsInStartZoom(Vector2 pos)
        {
            switch (m_Orient)
            {
                case Orient.Horizonal:
                    var start = runtimeX + runtimeWidth * m_Start / 100;
                    return ChartHelper.IsInRect(pos, start - 10, start + 10, runtimeY, runtimeY + runtimeHeight);
                case Orient.Vertical:
                    start = runtimeY + runtimeHeight * m_Start / 100;
                    return ChartHelper.IsInRect(pos, runtimeX, runtimeX + runtimeWidth, start - 10, start + 10);
                default: return false;
            }
        }

        /// <summary>
        /// 给定的坐标是否在结束活动条触发区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startX"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IsInEndZoom(Vector2 pos)
        {
            switch (m_Orient)
            {
                case Orient.Horizonal:
                    var end = runtimeX + runtimeWidth * m_End / 100;
                    return ChartHelper.IsInRect(pos, end - 10, end + 10, runtimeY, runtimeY + runtimeHeight);
                case Orient.Vertical:
                    end = runtimeY + runtimeHeight * m_End / 100;
                    return ChartHelper.IsInRect(pos, runtimeX, runtimeX + runtimeWidth, end - 10, end + 10);
                default: return false;
            }
        }


        public bool IsContainsAxis(Axis axis)
        {
            if (axis == null) return false;
            else if (axis is XAxis) return xAxisIndexs.Contains(axis.index);
            else if (axis is YAxis) return yAxisIndexs.Contains(axis.index);
            else return false;
        }
        public bool IsContainsXAxis(int index)
        {
            return xAxisIndexs != null && xAxisIndexs.Contains(index);
        }

        public bool IsContainsYAxis(int index)
        {
            return yAxisIndexs != null && yAxisIndexs.Contains(index);
        }

        public Color32 GetFillerColor(Color32 themeColor)
        {
            if (ChartHelper.IsClearColor(fillerColor)) return themeColor;
            else return fillerColor;
        }

        public Color32 GetBackgroundColor(Color32 themeColor)
        {
            if (ChartHelper.IsClearColor(backgroundColor)) return themeColor;
            else return backgroundColor;
        }
        public Color32 GetBorderColor(Color32 themeColor)
        {
            if (ChartHelper.IsClearColor(borderColor)) return themeColor;
            else return borderColor;
        }

        /// <summary>
        /// 是否显示文本
        /// </summary>
        /// <param name="flag"></param>
        internal void SetLabelActive(bool flag)
        {
            if (m_StartLabel != null && m_StartLabel.gameObject.activeInHierarchy != flag)
            {
                m_StartLabel.gameObject.SetActive(flag);
            }
            if (m_EndLabel != null && m_EndLabel.gameObject.activeInHierarchy != flag)
            {
                m_EndLabel.gameObject.SetActive(flag);
            }
        }

        /// <summary>
        /// 设置开始文本内容
        /// </summary>
        /// <param name="text"></param>
        internal void SetStartLabelText(string text)
        {
            if (m_StartLabel != null) m_StartLabel.SetText(text);
        }

        /// <summary>
        /// 设置结束文本内容
        /// </summary>
        /// <param name="text"></param>
        internal void SetEndLabelText(string text)
        {
            if (m_EndLabel != null) m_EndLabel.SetText(text);
        }

        internal void SetStartLabel(ChartText startLabel)
        {
            m_StartLabel = startLabel;
        }

        internal void SetEndLabel(ChartText endLabel)
        {
            m_EndLabel = endLabel;
        }

        internal void UpdateStartLabelPosition(Vector3 pos)
        {
            m_StartLabel.SetLocalPosition(pos);
        }

        internal void UpdateEndLabelPosition(Vector3 pos)
        {
            m_EndLabel.SetLocalPosition(pos);
        }

        internal void UpdateRuntimeData(float chartX, float chartY, float chartWidth, float chartHeight)
        {
            var runtimeLeft = left <= 1 ? left * chartWidth : left;
            var runtimeBottom = bottom <= 1 ? bottom * chartHeight : bottom;
            var runtimeTop = top <= 1 ? top * chartHeight : top;
            var runtimeRight = right <= 1 ? right * chartWidth : right;
            runtimeX = chartX + runtimeLeft;
            runtimeY = chartY + runtimeBottom;
            runtimeWidth = chartWidth - runtimeLeft - runtimeRight;
            runtimeHeight = chartHeight - runtimeTop - runtimeBottom;
        }

        internal void SetXAxisIndexValueInfo(int xAxisIndex, double min, double max)
        {
            if (!m_XAxisIndexInfos.ContainsKey(xAxisIndex))
            {
                m_XAxisIndexInfos[xAxisIndex] = new AxisIndexValueInfo()
                {
                    min = min,
                    max = max
                };
            }
            else
            {
                m_XAxisIndexInfos[xAxisIndex].min = min;
                m_XAxisIndexInfos[xAxisIndex].max = max;
            }
        }

        internal void SetYAxisIndexValueInfo(int yAxisIndex, double min, double max)
        {
            if (!m_YAxisIndexInfos.ContainsKey(yAxisIndex))
            {
                m_YAxisIndexInfos[yAxisIndex] = new AxisIndexValueInfo()
                {
                    min = min,
                    max = max
                };
            }
            else
            {
                m_YAxisIndexInfos[yAxisIndex].min = min;
                m_YAxisIndexInfos[yAxisIndex].max = max;
            }
        }

        internal bool IsXAxisIndexValue(int axisIndex)
        {
            return m_XAxisIndexInfos.ContainsKey(axisIndex);
        }

        internal bool IsYAxisIndexValue(int axisIndex)
        {
            return m_YAxisIndexInfos.ContainsKey(axisIndex);
        }

        internal void GetXAxisIndexValue(int axisIndex, out double min, out double max)
        {
            min = 0;
            max = 0;
            if (m_XAxisIndexInfos.ContainsKey(axisIndex))
            {
                var info = m_XAxisIndexInfos[axisIndex];
                min = info.min;
                max = info.max;
            }
        }
        internal void GetYAxisIndexValue(int axisIndex, out double min, out double max)
        {
            min = 0;
            max = 0;
            if (m_YAxisIndexInfos.ContainsKey(axisIndex))
            {
                var info = m_YAxisIndexInfos[axisIndex];
                min = info.min;
                max = info.max;
            }
        }
    }


    internal class DataZoomHandler : IComponentHandler
    {
        public CoordinateChart chart;
        private Vector2 m_LastTouchPos0;
        private Vector2 m_LastTouchPos1;
        private bool m_CheckDataZoomLabel;
        private float m_DataZoomLastStartIndex;
        private float m_DataZoomLastEndIndex;

        public DataZoomHandler(BaseChart chart)
        {
            this.chart = chart as CoordinateChart;
        }

        public void Init() { }

        public void Update()
        {
            if (chart == null) return;
            foreach (var dataZoom in chart.dataZooms)
            {
                CheckDataZoomScale(dataZoom);
                CheckDataZoomLabel(dataZoom);
            }
        }

        public void DrawBase(VertexHelper vh)
        {
        }

        public void DrawTop(VertexHelper vh)
        {
            if (chart == null) return;
            foreach (var dataZoom in chart.dataZooms)
            {
                switch (dataZoom.orient)
                {
                    case Orient.Horizonal:
                        DrawHorizonalDataZoomSlider(vh, dataZoom);
                        break;
                    case Orient.Vertical:
                        DrawVerticalDataZoomSlider(vh, dataZoom);
                        break;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (chart == null) return;
            if (Input.touchCount > 1) return;
            Vector2 pos;
            if (!chart.ScreenPointToChartPoint(eventData.position, out pos))
            {
                return;
            }
            foreach (var dataZoom in chart.dataZooms)
            {
                if (!dataZoom.enable) continue;
                var grid = chart.GetDataZoomGridOrDefault(dataZoom);
                if (dataZoom.supportInside && dataZoom.supportInsideDrag)
                {
                    if (chart.IsInGrid(grid, pos))
                    {
                        dataZoom.runtimeCoordinateDrag = true;
                    }
                }
                if (dataZoom.supportSlider)
                {
                    if (!dataZoom.zoomLock)
                    {
                        if (dataZoom.IsInStartZoom(pos))
                        {
                            dataZoom.runtimeStartDrag = true;
                        }
                        else if (dataZoom.IsInEndZoom(pos))
                        {
                            dataZoom.runtimeEndDrag = true;
                        }
                        else if (dataZoom.IsInSelectedZoom(pos))
                        {
                            dataZoom.runtimeDrag = true;
                        }
                    }
                    else if (dataZoom.IsInSelectedZoom(pos))
                    {
                        dataZoom.runtimeDrag = true;
                    }
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (chart == null) return;
            if (Input.touchCount > 1) return;
            foreach (var dataZoom in chart.dataZooms)
            {
                var grid = chart.GetDataZoomGridOrDefault(dataZoom);
                switch (dataZoom.orient)
                {
                    case Orient.Horizonal:
                        var deltaPercent = eventData.delta.x / grid.runtimeWidth * 100;
                        OnDragInside(dataZoom, deltaPercent);
                        OnDragSlider(dataZoom, deltaPercent);
                        break;
                    case Orient.Vertical:
                        deltaPercent = eventData.delta.y / grid.runtimeHeight * 100;
                        OnDragInside(dataZoom, deltaPercent);
                        OnDragSlider(dataZoom, deltaPercent);
                        break;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (chart == null) return;
            foreach (var dataZoom in chart.dataZooms)
            {
                if (dataZoom.runtimeDrag || dataZoom.runtimeStartDrag || dataZoom.runtimeEndDrag
                    || dataZoom.runtimeCoordinateDrag)
                {
                    chart.RefreshChart();
                }
                dataZoom.runtimeDrag = false;
                dataZoom.runtimeCoordinateDrag = false;
                dataZoom.runtimeStartDrag = false;
                dataZoom.runtimeEndDrag = false;
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (chart == null) return;
            if (Input.touchCount > 1) return;
            Vector2 localPos;
            if (!chart.ScreenPointToChartPoint(eventData.position, out localPos))
            {
                return;
            }
            foreach (var dataZoom in chart.dataZooms)
            {
                var grid = chart.GetDataZoomGridOrDefault(dataZoom);
                if (dataZoom.IsInStartZoom(localPos) ||
                    dataZoom.IsInEndZoom(localPos))
                {
                    return;
                }
                if (dataZoom.IsInZoom(localPos)
                    && !dataZoom.IsInSelectedZoom(localPos))
                {
                    var pointerX = localPos.x;
                    var selectWidth = grid.runtimeWidth * (dataZoom.end - dataZoom.start) / 100;
                    var startX = pointerX - selectWidth / 2;
                    var endX = pointerX + selectWidth / 2;
                    if (startX < grid.runtimeX)
                    {
                        startX = grid.runtimeX;
                        endX = grid.runtimeX + selectWidth;
                    }
                    else if (endX > grid.runtimeX + grid.runtimeWidth)
                    {
                        endX = grid.runtimeX + grid.runtimeWidth;
                        startX = grid.runtimeX + grid.runtimeWidth - selectWidth;
                    }
                    var start = (startX - grid.runtimeX) / grid.runtimeWidth * 100;
                    var end = (endX - grid.runtimeX) / grid.runtimeWidth * 100;
                    UpdateDataZoomRange(dataZoom, start, end);
                }
            }
        }
        public void OnScroll(PointerEventData eventData)
        {
            if (chart == null) return;
            if (Input.touchCount > 1) return;
            Vector2 pos;
            if (!chart.ScreenPointToChartPoint(eventData.position, out pos)) return;
            foreach (var dataZoom in chart.dataZooms)
            {
                if (!dataZoom.enable || dataZoom.zoomLock) continue;
                var grid = chart.GetDataZoomGridOrDefault(dataZoom);
                if ((dataZoom.supportInside && dataZoom.supportInsideScroll && chart.IsInGrid(grid, pos)) ||
                    dataZoom.IsInZoom(pos))
                {
                    ScaleDataZoom(dataZoom, eventData.scrollDelta.y * dataZoom.scrollSensitivity);
                }
            }
        }

        private void OnDragInside(DataZoom dataZoom, float deltaPercent)
        {
            if (deltaPercent == 0) return;
            if (Input.touchCount > 1) return;
            if (!dataZoom.supportInside || !dataZoom.supportInsideDrag) return;
            if (!dataZoom.runtimeCoordinateDrag) return;
            var diff = dataZoom.end - dataZoom.start;
            if (deltaPercent > 0)
            {
                if (dataZoom.start > 0)
                {
                    var start = dataZoom.start - deltaPercent;
                    if (start < 0) start = 0;
                    var end = start + diff;
                    UpdateDataZoomRange(dataZoom, start, end);
                }
            }
            else
            {
                if (dataZoom.end < 100)
                {
                    var end = dataZoom.end - deltaPercent;
                    if (end > 100) end = 100;
                    var start = end - diff;
                    UpdateDataZoomRange(dataZoom, start, end);
                }
            }
        }

        private void OnDragSlider(DataZoom dataZoom, float deltaPercent)
        {
            if (Input.touchCount > 1) return;
            if (!dataZoom.supportSlider) return;
            if (dataZoom.runtimeStartDrag)
            {
                var start = dataZoom.start + deltaPercent;
                if (start > dataZoom.end)
                {
                    start = dataZoom.end;
                    dataZoom.runtimeEndDrag = true;
                    dataZoom.runtimeStartDrag = false;
                }
                UpdateDataZoomRange(dataZoom, start, dataZoom.end);
            }
            else if (dataZoom.runtimeEndDrag)
            {
                var end = dataZoom.end + deltaPercent;
                if (end < dataZoom.start)
                {
                    end = dataZoom.start;
                    dataZoom.runtimeStartDrag = true;
                    dataZoom.runtimeEndDrag = false;
                }
                UpdateDataZoomRange(dataZoom, dataZoom.start, end);
            }
            else if (dataZoom.runtimeDrag)
            {
                if (deltaPercent > 0)
                {
                    if (dataZoom.end + deltaPercent > 100) deltaPercent = 100 - dataZoom.end;
                }
                else
                {
                    if (dataZoom.start + deltaPercent < 0) deltaPercent = -dataZoom.start;
                }
                UpdateDataZoomRange(dataZoom, dataZoom.start + deltaPercent, dataZoom.end + deltaPercent);
            }
        }

        private void ScaleDataZoom(DataZoom dataZoom, float delta)
        {
            var grid = chart.GetDataZoomGridOrDefault(dataZoom);
            var deltaPercent = dataZoom.orient == Orient.Horizonal ?
                     Mathf.Abs(delta / grid.runtimeWidth * 100) :
                      Mathf.Abs(delta / grid.runtimeHeight * 100);
            if (delta > 0)
            {
                if (dataZoom.end <= dataZoom.start) return;
                UpdateDataZoomRange(dataZoom, dataZoom.start + deltaPercent, dataZoom.end - deltaPercent);
            }
            else
            {
                UpdateDataZoomRange(dataZoom, dataZoom.start - deltaPercent, dataZoom.end + deltaPercent);
            }
        }

        public void UpdateDataZoomRange(DataZoom dataZoom, float start, float end)
        {
            if (end > 100) end = 100;
            if (start < 0) start = 0;
            if (end < start) end = start;
            dataZoom.start = start;
            dataZoom.end = end;
            if (dataZoom.realtime)
            {
                chart.OnDataZoomRangeChanged(dataZoom);
                chart.RefreshChart();
            }
        }

        public void RefreshDataZoomLabel()
        {
            m_CheckDataZoomLabel = true;
        }

        private void CheckDataZoomScale(DataZoom dataZoom)
        {
            if (!dataZoom.enable || dataZoom.zoomLock || !dataZoom.supportInside || !dataZoom.supportInsideDrag) return;

            if (Input.touchCount == 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began)
                {
                    m_LastTouchPos0 = touch0.position;
                    m_LastTouchPos1 = touch1.position;
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    var tempPos0 = touch0.position;
                    var tempPos1 = touch1.position;
                    var currDist = Vector2.Distance(tempPos0, tempPos1);
                    var lastDist = Vector2.Distance(m_LastTouchPos0, m_LastTouchPos1);
                    var delta = (currDist - lastDist);
                    ScaleDataZoom(dataZoom, delta / dataZoom.scrollSensitivity);
                    m_LastTouchPos0 = tempPos0;
                    m_LastTouchPos1 = tempPos1;
                }
            }
        }

        private void CheckDataZoomLabel(DataZoom dataZoom)
        {
            if (dataZoom.enable && dataZoom.supportSlider && dataZoom.showDetail)
            {
                Vector2 local;
                if (!chart.ScreenPointToChartPoint(Input.mousePosition, out local))
                {
                    dataZoom.SetLabelActive(false);
                    return;
                }
                if (dataZoom.IsInSelectedZoom(local)
                    || dataZoom.IsInStartZoom(local)
                    || dataZoom.IsInEndZoom(local))
                {
                    dataZoom.SetLabelActive(true);
                    RefreshDataZoomLabel();
                }
                else
                {
                    dataZoom.SetLabelActive(false);
                }
            }
            if (m_CheckDataZoomLabel && dataZoom.xAxisIndexs.Count > 0)
            {
                m_CheckDataZoomLabel = false;
                var xAxis = chart.GetXAxis(dataZoom.xAxisIndexs[0]);
                var startIndex = (int)((xAxis.data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((xAxis.data.Count - 1) * dataZoom.end / 100);
                if (m_DataZoomLastStartIndex != startIndex || m_DataZoomLastEndIndex != endIndex)
                {
                    m_DataZoomLastStartIndex = startIndex;
                    m_DataZoomLastEndIndex = endIndex;
                    if (xAxis.data.Count > 0)
                    {
                        dataZoom.SetStartLabelText(xAxis.data[startIndex]);
                        dataZoom.SetEndLabelText(xAxis.data[endIndex]);
                    }
                    else if (xAxis.IsTime())
                    {
                        //TODO:
                        dataZoom.SetStartLabelText("");
                        dataZoom.SetEndLabelText("");
                    }
                    chart.InitAxisX();
                }
                var start = dataZoom.runtimeX + dataZoom.runtimeWidth * dataZoom.start / 100;
                var end = dataZoom.runtimeX + dataZoom.runtimeWidth * dataZoom.end / 100;
                var hig = dataZoom.runtimeHeight;
                dataZoom.UpdateStartLabelPosition(new Vector3(start - 10, chart.chartY + dataZoom.bottom + hig / 2));
                dataZoom.UpdateEndLabelPosition(new Vector3(end + 10, chart.chartY + dataZoom.bottom + hig / 2));
            }
        }

        private void DrawHorizonalDataZoomSlider(VertexHelper vh, DataZoom dataZoom)
        {
            if (!dataZoom.enable || !dataZoom.supportSlider) return;
            var p1 = new Vector3(dataZoom.runtimeX, dataZoom.runtimeY);
            var p2 = new Vector3(dataZoom.runtimeX, dataZoom.runtimeY + dataZoom.runtimeHeight);
            var p3 = new Vector3(dataZoom.runtimeX + dataZoom.runtimeWidth, dataZoom.runtimeY + dataZoom.runtimeHeight);
            var p4 = new Vector3(dataZoom.runtimeX + dataZoom.runtimeWidth, dataZoom.runtimeY);
            var lineColor = dataZoom.lineStyle.GetColor(chart.theme.dataZoom.dataLineColor);
            var lineWidth = dataZoom.lineStyle.GetWidth(chart.theme.dataZoom.dataLineWidth);
            var borderWidth = dataZoom.borderWidth == 0 ? chart.theme.dataZoom.borderWidth : dataZoom.borderWidth;
            var borderColor = dataZoom.GetBorderColor(chart.theme.dataZoom.borderColor);
            var backgroundColor = dataZoom.GetBackgroundColor(chart.theme.dataZoom.backgroundColor);
            var areaColor = dataZoom.areaStyle.GetColor(chart.theme.dataZoom.dataAreaColor);
            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, backgroundColor);
            var centerPos = new Vector3(dataZoom.runtimeX + dataZoom.runtimeWidth / 2,
                dataZoom.runtimeY + dataZoom.runtimeHeight / 2);
            UGL.DrawBorder(vh, centerPos, dataZoom.runtimeWidth, dataZoom.runtimeHeight, borderWidth, borderColor);
            if (dataZoom.showDataShadow && chart.series.Count > 0)
            {
                Serie serie = chart.series.list[0];
                Axis axis = chart.GetYAxis(0);
                var showData = serie.GetDataList(null);
                float scaleWid = dataZoom.runtimeWidth / (showData.Count - 1);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                double minValue = 0;
                double maxValue = 0;
                SeriesHelper.GetYMinMaxValue(chart.series, null, 0, chart.IsValue(), axis.inverse, out minValue, out maxValue);
                AxisHelper.AdjustMinMaxValue(axis, ref minValue, ref maxValue, true);

                int rate = 1;
                var sampleDist = serie.sampleDist < 2 ? 2 : serie.sampleDist;
                var maxCount = showData.Count;
                if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (dataZoom.runtimeWidth / sampleDist));
                if (rate < 1) rate = 1;
                var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                    chart.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
                var dataChanging = false;
                for (int i = 0; i < maxCount; i += rate)
                {
                    double value = chart.SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage, i,
                    serie.animation.GetUpdateAnimationDuration(), ref dataChanging, axis);
                    float pX = dataZoom.runtimeX + i * scaleWid;
                    float dataHig = (float)((maxValue - minValue) == 0 ? 0 :
                        (value - minValue) / (maxValue - minValue) * dataZoom.runtimeHeight);
                    np = new Vector3(pX, chart.chartY + dataZoom.bottom + dataHig);
                    if (i > 0)
                    {
                        UGL.DrawLine(vh, lp, np, lineWidth, lineColor);
                        Vector3 alp = new Vector3(lp.x, lp.y - lineWidth);
                        Vector3 anp = new Vector3(np.x, np.y - lineWidth);

                        Vector3 tnp = new Vector3(np.x, chart.chartY + dataZoom.bottom + lineWidth);
                        Vector3 tlp = new Vector3(lp.x, chart.chartY + dataZoom.bottom + lineWidth);
                        UGL.DrawQuadrilateral(vh, alp, anp, tnp, tlp, areaColor);
                    }
                    lp = np;
                }
                if (dataChanging)
                {
                    chart.RefreshTopPainter();
                }
            }
            switch (dataZoom.rangeMode)
            {
                case DataZoom.RangeMode.Percent:
                    var start = dataZoom.runtimeX + dataZoom.runtimeWidth * dataZoom.start / 100;
                    var end = dataZoom.runtimeX + dataZoom.runtimeWidth * dataZoom.end / 100;
                    var fillerColor = dataZoom.GetFillerColor(chart.theme.dataZoom.fillerColor);

                    p1 = new Vector2(start, dataZoom.runtimeY);
                    p2 = new Vector2(start, dataZoom.runtimeY + dataZoom.runtimeHeight);
                    p3 = new Vector2(end, dataZoom.runtimeY + dataZoom.runtimeHeight);
                    p4 = new Vector2(end, dataZoom.runtimeY);
                    UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, fillerColor);
                    UGL.DrawLine(vh, p1, p2, lineWidth, fillerColor);
                    UGL.DrawLine(vh, p3, p4, lineWidth, fillerColor);
                    break;
            }
        }

        private void DrawVerticalDataZoomSlider(VertexHelper vh, DataZoom dataZoom)
        {
            if (!dataZoom.enable || !dataZoom.supportSlider) return;
            var p1 = new Vector3(dataZoom.runtimeX, dataZoom.runtimeY);
            var p2 = new Vector3(dataZoom.runtimeX, dataZoom.runtimeY + dataZoom.runtimeHeight);
            var p3 = new Vector3(dataZoom.runtimeX + dataZoom.runtimeWidth, dataZoom.runtimeY + dataZoom.runtimeHeight);
            var p4 = new Vector3(dataZoom.runtimeX + dataZoom.runtimeWidth, dataZoom.runtimeY);
            var lineColor = dataZoom.lineStyle.GetColor(chart.theme.dataZoom.dataLineColor);
            var lineWidth = dataZoom.lineStyle.GetWidth(chart.theme.dataZoom.dataLineWidth);
            var borderWidth = dataZoom.borderWidth == 0 ? chart.theme.dataZoom.borderWidth : dataZoom.borderWidth;
            var borderColor = dataZoom.GetBorderColor(chart.theme.dataZoom.borderColor);
            var backgroundColor = dataZoom.GetBackgroundColor(chart.theme.dataZoom.backgroundColor);
            var areaColor = dataZoom.areaStyle.GetColor(chart.theme.dataZoom.dataAreaColor);
            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, backgroundColor);
            var centerPos = new Vector3(dataZoom.runtimeX + dataZoom.runtimeWidth / 2,
                dataZoom.runtimeY + dataZoom.runtimeHeight / 2);
            UGL.DrawBorder(vh, centerPos, dataZoom.runtimeWidth, dataZoom.runtimeHeight, borderWidth, borderColor);
            if (dataZoom.showDataShadow && chart.series.Count > 0)
            {
                Serie serie = chart.series.list[0];
                Axis axis = chart.GetYAxis(0);
                var showData = serie.GetDataList(null);
                float scaleWid = dataZoom.runtimeHeight / (showData.Count - 1);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                double minValue = 0;
                double maxValue = 0;
                SeriesHelper.GetYMinMaxValue(chart.series, null, 0, chart.IsValue(), axis.inverse, out minValue, out maxValue);
                AxisHelper.AdjustMinMaxValue(axis, ref minValue, ref maxValue, true);

                int rate = 1;
                var sampleDist = serie.sampleDist < 2 ? 2 : serie.sampleDist;
                var maxCount = showData.Count;
                if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (dataZoom.runtimeHeight / sampleDist));
                if (rate < 1) rate = 1;
                var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                    chart.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
                var dataChanging = false;
                for (int i = 0; i < maxCount; i += rate)
                {
                    double value = chart.SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage, i,
                    serie.animation.GetUpdateAnimationDuration(), ref dataChanging, axis);
                    float pY = dataZoom.runtimeY + i * scaleWid;
                    float dataHig = (maxValue - minValue) == 0 ? 0 :
                        (float)((value - minValue) / (maxValue - minValue) * dataZoom.runtimeWidth);
                    np = new Vector3(chart.chartX + chart.chartWidth - dataZoom.right - dataHig, pY);
                    if (i > 0)
                    {
                        UGL.DrawLine(vh, lp, np, lineWidth, lineColor);
                        Vector3 alp = new Vector3(lp.x, lp.y - lineWidth);
                        Vector3 anp = new Vector3(np.x, np.y - lineWidth);

                        Vector3 tnp = new Vector3(np.x, chart.chartY + dataZoom.bottom + lineWidth);
                        Vector3 tlp = new Vector3(lp.x, chart.chartY + dataZoom.bottom + lineWidth);
                        UGL.DrawQuadrilateral(vh, alp, anp, tnp, tlp, areaColor);
                    }
                    lp = np;
                }
                if (dataChanging)
                {
                    chart.RefreshTopPainter();
                }
            }
            switch (dataZoom.rangeMode)
            {
                case DataZoom.RangeMode.Percent:
                    var start = dataZoom.runtimeY + dataZoom.runtimeHeight * dataZoom.start / 100;
                    var end = dataZoom.runtimeY + dataZoom.runtimeHeight * dataZoom.end / 100;
                    var fillerColor = dataZoom.GetFillerColor(chart.theme.dataZoom.fillerColor);

                    p1 = new Vector2(dataZoom.runtimeX, start);
                    p2 = new Vector2(dataZoom.runtimeX + dataZoom.runtimeWidth, start);
                    p3 = new Vector2(dataZoom.runtimeX + dataZoom.runtimeWidth, end);
                    p4 = new Vector2(dataZoom.runtimeX, end);
                    UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, fillerColor);
                    UGL.DrawLine(vh, p1, p2, lineWidth, fillerColor);
                    UGL.DrawLine(vh, p3, p4, lineWidth, fillerColor);
                    break;
            }
        }
    }
}