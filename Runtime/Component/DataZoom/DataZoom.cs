using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// DataZoom component is used for zooming a specific area,
    /// which enables user to investigate data in detail,
    /// or get an overview of the data, or get rid of outlier points.
    /// ||DataZoom 组件 用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(DataZoomHandler), true)]
    public class DataZoom : MainComponent, IUpdateRuntimeData
    {
        /// <summary>
        /// Generally dataZoom component zoom or roam coordinate system through data filtering
        /// and set the windows of axes internally.
        /// Its behaviours vary according to filtering mode settings.
        /// ||dataZoom 的运行原理是通过 数据过滤 来达到 数据窗口缩放 的效果。数据过滤模式的设置不同，效果也不同。
        /// </summary>
        public enum FilterMode
        {
            /// <summary>
            /// data that outside the window will be filtered, which may lead to some changes of windows of other axes.
            /// For each data item, it will be filtered if one of the relevant dimensions is out of the window.
            /// ||当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只要有一个维度在数据窗口外，整个数据项就会被过滤掉。
            /// </summary>
            Filter,
            /// <summary>
            /// data that outside the window will be filtered, which may lead to some changes of windows of other axes.
            /// For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.
            /// ||当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只有当全部维度都在数据窗口同侧外部，整个数据项才会被过滤掉。
            /// </summary>
            WeakFilter,
            /// <summary>
            /// data that outside the window will be set to NaN, which will not lead to changes of windows of other axes. 
            /// ||当前数据窗口外的数据，被 设置为空。即 不会 影响其他轴的数据范围。
            /// </summary>
            Empty,
            /// <summary>
            /// Do not filter data.
            /// ||不过滤数据，只改变数轴范围。
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
            /// percent value. 
            /// ||百分比。
            /// </summary>
            Percent
        }

        [SerializeField] private bool m_Enable = true;
        [SerializeField] private FilterMode m_FilterMode;
        [SerializeField] private List<int> m_XAxisIndexs = new List<int>() { 0 };
        [SerializeField] private List<int> m_YAxisIndexs = new List<int>() { };
        [SerializeField] private bool m_SupportInside;
        [SerializeField] private bool m_SupportInsideScroll = true;
        [SerializeField] private bool m_SupportInsideDrag = true;
        [SerializeField] private bool m_SupportSlider;
        [SerializeField] private bool m_SupportMarquee;
        [SerializeField] private bool m_ShowDataShadow;
        [SerializeField] private bool m_ShowDetail;
        [SerializeField] private bool m_ZoomLock;
        //[SerializeField] private bool m_Realtime;
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
        [SerializeField] private int m_MinShowNum = 2;
        [Range(1f, 20f)]
        [SerializeField] private float m_ScrollSensitivity = 1.1f;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle();
        [SerializeField] private LineStyle m_LineStyle = new LineStyle(LineStyle.Type.Solid);
        [SerializeField] private AreaStyle m_AreaStyle = new AreaStyle();
        [SerializeField][Since("v3.5.0")] private MarqueeStyle m_MarqueeStyle = new MarqueeStyle();
        [SerializeField][Since("v3.6.0")] private bool m_StartLock;
        [SerializeField][Since("v3.6.0")] private bool m_EndLock;

        public DataZoomContext context = new DataZoomContext();
        private CustomDataZoomStartEndFunction m_StartEndFunction;

        /// <summary>
        /// Whether to show dataZoom.
        /// ||是否显示缩放区域。
        /// </summary>
        public bool enable
        {
            get { return m_Enable; }
            set { if (PropertyUtil.SetStruct(ref m_Enable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The mode of data filter.
        /// ||数据过滤类型。
        /// </summary>
        public FilterMode filterMode
        {
            get { return m_FilterMode; }
            set { if (PropertyUtil.SetStruct(ref m_FilterMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify which xAxis is controlled by the dataZoom.
        /// ||控制的 x 轴索引列表。
        /// </summary>
        public List<int> xAxisIndexs
        {
            get { return m_XAxisIndexs; }
            set { if (PropertyUtil.SetClass(ref m_XAxisIndexs, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify which yAxis is controlled by the dataZoom.
        /// ||控制的 y 轴索引列表。
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
        /// ||是否支持内置。内置于坐标系中，使用户可以在坐标系上通过鼠标拖拽、鼠标滚轮、手指滑动（触屏上）来缩放或漫游坐标系。
        /// </summary>
        public bool supportInside
        {
            get { return m_SupportInside; }
            set { if (PropertyUtil.SetStruct(ref m_SupportInside, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether inside scrolling is supported.
        /// ||是否支持坐标系内滚动
        /// </summary>
        public bool supportInsideScroll
        {
            get { return m_SupportInsideScroll; }
            set { if (PropertyUtil.SetStruct(ref m_SupportInsideScroll, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether insde drag is supported.
        /// ||是否支持坐标系内拖拽
        /// </summary>
        public bool supportInsideDrag
        {
            get { return m_SupportInsideDrag; }
            set { if (PropertyUtil.SetStruct(ref m_SupportInsideDrag, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether a slider is supported. There are separate sliders on which the user zooms or roams.
        /// ||是否支持滑动条。有单独的滑动条，用户在滑动条上进行缩放或漫游。
        /// </summary>
        public bool supportSlider
        {
            get { return m_SupportSlider; }
            set { if (PropertyUtil.SetStruct(ref m_SupportSlider, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Supported Box Selected. Provides a marquee for scaling the data area.
        /// ||是否支持框选。提供一个选框进行数据区域缩放。
        /// </summary>
        public bool supportMarquee
        {
            get { return m_SupportMarquee; }
            set { if (PropertyUtil.SetStruct(ref m_SupportMarquee, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show data shadow, to indicate the data tendency in brief.
        /// ||是否显示数据阴影。数据阴影可以简单地反应数据走势。
        /// </summary>
        public bool showDataShadow
        {
            get { return m_ShowDataShadow; }
            set { if (PropertyUtil.SetStruct(ref m_ShowDataShadow, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show detail, that is, show the detailed data information when dragging.
        /// ||是否显示detail，即拖拽时候显示详细数值信息。
        /// </summary>
        public bool showDetail
        {
            get { return m_ShowDetail; }
            set { if (PropertyUtil.SetStruct(ref m_ShowDetail, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify whether to lock the size of window (selected area).
        /// ||是否锁定选择区域（或叫做数据窗口）的大小。
        /// 如果设置为 true 则锁定选择区域的大小，也就是说，只能平移，不能缩放。
        /// </summary>
        public bool zoomLock
        {
            get { return m_ZoomLock; }
            set { if (PropertyUtil.SetStruct(ref m_ZoomLock, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show data shadow in dataZoom-silder component, to indicate the data tendency in brief.
        /// ||拖动时，是否实时更新系列的视图。如果设置为 false，则只在拖拽结束的时候更新。默认为true，暂不支持修改。
        /// </summary>
        public bool realtime { get { return true; } }
        /// <summary>
        /// The background color of the component.
        /// ||组件的背景颜色。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetStruct(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of dataZoom data area.
        /// ||数据区域颜色。
        /// </summary>
        public Color32 fillerColor
        {
            get { return m_FillerColor; }
            set { if (PropertyUtil.SetColor(ref m_FillerColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the color of dataZoom border.
        /// ||边框颜色。
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
        /// ||组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the top side of the container.
        /// top value is a instant pixel value like 10 or float value [0-1].
        /// ||组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the left side of the container.
        /// left value is a instant pixel value like 10 or float value [0-1].
        /// ||组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the right side of the container.
        /// right value is a instant pixel value like 10 or float value [0-1].
        /// ||组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Use absolute value or percent value in DataZoom.start and DataZoom.end.
        /// ||取绝对值还是百分比。
        /// </summary>
        public RangeMode rangeMode
        {
            get { return m_RangeMode; }
            set { if (PropertyUtil.SetStruct(ref m_RangeMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The start percentage of the window out of the data extent, in the range of 0 ~ 100.
        /// ||数据窗口范围的起始百分比。范围是：0 ~ 100。
        /// </summary>
        public float start
        {
            get { return m_Start; }
            set { m_Start = value; if (m_Start < 0) m_Start = 0; if (m_Start > 100) m_Start = 100; SetVerticesDirty(); }
        }
        /// <summary>
        /// Lock start value.
        /// ||固定起始值，不让改变。
        /// </summary>
        public bool startLock
        {
            get { return m_StartLock; }
            set { if (PropertyUtil.SetStruct(ref m_StartLock, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Lock end value.
        /// ||固定结束值，不让改变。
        /// </summary>
        public bool endLock
        {
            get { return m_EndLock; }
            set { if (PropertyUtil.SetStruct(ref m_EndLock, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The end percentage of the window out of the data extent, in the range of 0 ~ 100.
        /// ||数据窗口范围的结束百分比。范围是：0 ~ 100。
        /// </summary>
        public float end
        {
            get { return m_End; }
            set { m_End = value; if (m_End < 0) m_End = 0; if (m_End > 100) m_End = 100; SetVerticesDirty(); }
        }
        /// <summary>
        /// Minimum number of display data. Minimum number of data displayed when DataZoom is enlarged to maximum.
        /// ||最小显示数据个数。当DataZoom放大到最大时，最小显示的数据个数。
        /// </summary>
        public int minShowNum
        {
            get { return m_MinShowNum; }
            set { if (PropertyUtil.SetStruct(ref m_MinShowNum, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The sensitivity of dataZoom scroll.
        /// The larger the number, the more sensitive it is.
        /// ||缩放区域组件的敏感度。值越高每次缩放所代表的数据越多。
        /// </summary>
        public float scrollSensitivity
        {
            get { return m_ScrollSensitivity; }
            set { if (PropertyUtil.SetStruct(ref m_ScrollSensitivity, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify whether the layout of dataZoom component is horizontal or vertical. What's more, 
        /// it indicates whether the horizontal axis or vertical axis is controlled by default in catesian coordinate system.
        /// ||布局方式是横还是竖。不仅是布局方式，对于直角坐标系而言，也决定了，缺省情况控制横向数轴还是纵向数轴。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// label style.
        /// ||文本标签格式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
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
        /// <summary>
        /// 选取框样式。
        /// </summary>
        public MarqueeStyle marqueeStyle
        {
            get { return m_MarqueeStyle; }
            set { if (PropertyUtil.SetClass(ref m_MarqueeStyle, value)) SetAllDirty(); }
        }
        /// <summary>
        /// start和end变更委托。
        /// </summary>
        public CustomDataZoomStartEndFunction startEndFunction { get { return m_StartEndFunction; } set { m_StartEndFunction = value; } }

        class AxisIndexValueInfo
        {
            public double rawMin;
            public double rawMax;
            public double min;
            public double max;
        }
        private Dictionary<int, AxisIndexValueInfo> m_XAxisIndexInfos = new Dictionary<int, AxisIndexValueInfo>();
        private Dictionary<int, AxisIndexValueInfo> m_YAxisIndexInfos = new Dictionary<int, AxisIndexValueInfo>();

        /// <summary>
        /// The start label.
        /// ||组件的开始信息文本。
        /// </summary>
        private ChartLabel m_StartLabel { get; set; }
        /// <summary>
        /// The end label.
        /// ||组件的结束信息文本。
        /// </summary>
        private ChartLabel m_EndLabel { get; set; }

        public override void SetDefaultValue()
        {
            supportInside = true;
            supportSlider = true;
            filterMode = FilterMode.None;
            xAxisIndexs = new List<int>() { 0 };
            yAxisIndexs = new List<int>() { };
            showDataShadow = true;
            showDetail = false;
            zoomLock = false;
            m_Bottom = 10;
            m_Left = 10;
            m_Right = 10;
            m_Top = 0.9f;
            rangeMode = RangeMode.Percent;
            start = 30;
            end = 70;
            m_Orient = Orient.Horizonal;
            m_ScrollSensitivity = 10;
            m_LabelStyle = new LabelStyle();
            m_LineStyle = new LineStyle(LineStyle.Type.Solid)
            {
                opacity = 0.3f
            };
            m_AreaStyle = new AreaStyle()
            {
                show = true,
                opacity = 0.3f
            };
            m_MarqueeStyle = new MarqueeStyle();
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
            if (pos.x < context.x - 1 || pos.x > context.x + context.width + 1 ||
                pos.y < context.y - 1 || pos.y > context.y + context.height + 1)
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
                    var start = context.x + context.width * m_Start / 100;
                    var end = context.x + context.width * m_End / 100;
                    return ChartHelper.IsInRect(pos, start, end, context.y, context.y + context.height);
                case Orient.Vertical:
                    start = context.y + context.height * m_Start / 100;
                    end = context.y + context.height * m_End / 100;
                    return ChartHelper.IsInRect(pos, context.x, context.x + context.width, start, end);
                default:
                    return false;
            }
        }

        public bool IsInSelectedZoom(int totalIndex, int index, bool invert)
        {
            if (totalIndex <= 0)
                return false;

            var tstart = invert ? 100 - end : start;
            var tend = invert ? 100 - start : end;
            var range = Mathf.RoundToInt(totalIndex * (tend - tstart) / 100);
            var min = Mathf.FloorToInt(totalIndex * tstart / 100);
            var max = Mathf.CeilToInt(totalIndex * tend / 100);
            if (min == 0) max = min + range;
            if (max == totalIndex) min = max - range;
            var flag = index >= min && index < min + range;
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
                    var start = context.x + context.width * m_Start / 100;
                    return ChartHelper.IsInRect(pos, start - 10, start + 10, context.y, context.y + context.height);
                case Orient.Vertical:
                    start = context.y + context.height * m_Start / 100;
                    return ChartHelper.IsInRect(pos, context.x, context.x + context.width, start - 10, start + 10);
                default:
                    return false;
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
                    var end = context.x + context.width * m_End / 100;
                    return ChartHelper.IsInRect(pos, end - 10, end + 10, context.y, context.y + context.height);
                case Orient.Vertical:
                    end = context.y + context.height * m_End / 100;
                    return ChartHelper.IsInRect(pos, context.x, context.x + context.width, end - 10, end + 10);
                default:
                    return false;
            }
        }

        public bool IsInMarqueeArea(SerieData serieData)
        {
            return IsInMarqueeArea(serieData.context.position);
        }

        public bool IsInMarqueeArea(Vector2 pos)
        {
            if (!supportMarquee) return false;
            if (context.marqueeRect.width >= 0)
            {
                return context.marqueeRect.Contains(pos);
            }
            else
            {
                var rect = context.marqueeRect;
                return (new Rect(rect.x + rect.width, rect.y, -rect.width, rect.height)).Contains(pos);
            }
        }

        public bool IsContainsAxis(Axis axis)
        {
            if (axis == null)
                return false;
            else if (axis is XAxis)
                return xAxisIndexs.Contains(axis.index);
            else if (axis is YAxis)
                return yAxisIndexs.Contains(axis.index);
            else
                return false;
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
            if (ChartHelper.IsClearColor(fillerColor))
                return themeColor;
            else
                return fillerColor;
        }

        public Color32 GetBackgroundColor(Color32 themeColor)
        {
            if (ChartHelper.IsClearColor(backgroundColor))
                return themeColor;
            else
                return backgroundColor;
        }
        public Color32 GetBorderColor(Color32 themeColor)
        {
            if (ChartHelper.IsClearColor(borderColor))
                return themeColor;
            else
                return borderColor;
        }

        /// <summary>
        /// 是否显示文本
        /// </summary>
        /// <param name="flag"></param>
        internal void SetLabelActive(bool flag)
        {
            m_StartLabel.SetActive(flag);
            m_EndLabel.SetActive(flag);
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

        internal void SetStartLabel(ChartLabel startLabel)
        {
            m_StartLabel = startLabel;
        }

        internal void SetEndLabel(ChartLabel endLabel)
        {
            m_EndLabel = endLabel;
        }

        internal void UpdateStartLabelPosition(Vector3 pos)
        {
            if (m_StartLabel != null) m_StartLabel.SetPosition(pos);
        }

        internal void UpdateEndLabelPosition(Vector3 pos)
        {
            if (m_EndLabel != null) m_EndLabel.SetPosition(pos);
        }

        public void UpdateRuntimeData(BaseChart chart)
        {
            var chartX = chart.chartX;
            var chartY = chart.chartY;
            var chartWidth = chart.chartWidth;
            var chartHeight = chart.chartHeight;
            var runtimeLeft = left <= 1 ? left * chartWidth : left;
            var runtimeBottom = bottom <= 1 ? bottom * chartHeight : bottom;
            var runtimeTop = top <= 1 ? top * chartHeight : top;
            var runtimeRight = right <= 1 ? right * chartWidth : right;
            context.x = chartX + runtimeLeft;
            context.y = chartY + runtimeBottom;
            context.width = chartWidth - runtimeLeft - runtimeRight;
            context.height = chartHeight - runtimeTop - runtimeBottom;
        }

        internal void SetXAxisIndexValueInfo(int xAxisIndex, ref double min, ref double max)
        {
            AxisIndexValueInfo info;
            if (!m_XAxisIndexInfos.TryGetValue(xAxisIndex, out info))
            {
                info = new AxisIndexValueInfo();
                m_XAxisIndexInfos[xAxisIndex] = info;
            }
            info.rawMin = min;
            info.rawMax = max;
            info.min = min + (max - min) * start / 100;
            info.max = min + (max - min) * end / 100;
            min = info.min;
            max = info.max;
        }

        internal void SetYAxisIndexValueInfo(int yAxisIndex, ref double min, ref double max)
        {
            AxisIndexValueInfo info;
            if (!m_YAxisIndexInfos.TryGetValue(yAxisIndex, out info))
            {
                info = new AxisIndexValueInfo();
                m_YAxisIndexInfos[yAxisIndex] = info;
            }
            info.rawMin = min;
            info.rawMax = max;
            info.min = min + (max - min) * start / 100;
            info.max = min + (max - min) * end / 100;
            min = info.min;
            max = info.max;
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
            AxisIndexValueInfo info;
            if (m_XAxisIndexInfos.TryGetValue(axisIndex, out info))
            {
                var range = info.rawMax - info.rawMin;
                min = info.rawMin + range * m_Start / 100;
                max = info.rawMin + range * m_End / 100;
            }
            else
            {
                min = 0;
                max = 0;
            }
        }
        internal void GetYAxisIndexValue(int axisIndex, out double min, out double max)
        {
            AxisIndexValueInfo info;
            if (m_YAxisIndexInfos.TryGetValue(axisIndex, out info))
            {
                var range = info.rawMax - info.rawMin;
                min = info.rawMin + range * m_Start / 100;
                max = info.rawMin + range * m_End / 100;
            }
            else
            {
                min = 0;
                max = 0;
            }
        }
    }
}