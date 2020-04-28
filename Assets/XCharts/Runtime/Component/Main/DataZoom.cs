/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// DataZoom component is used for zooming a specific area,
    /// which enables user to investigate data in detail,
    /// or get an overview of the data, or get rid of outlier points.
    /// <para>DataZoom 组件 用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。</para>
    /// </summary>
    [System.Serializable]
    public class DataZoom : MainComponent
    {
        /// <summary>
        /// Generally dataZoom component zoom or roam coordinate system through data filtering
        /// and set the windows of axes internally.
        /// Its behaviours vary according to filtering mode settings.
        /// dataZoom 的运行原理是通过 数据过滤 来达到 数据窗口缩放 的效果。数据过滤模式的设置不同，效果也不同。
        /// </summary>
        public enum FilterMode
        {
            /// <summary>
            /// data that outside the window will be filtered, which may lead to some changes of windows of other axes.
            /// For each data item, it will be filtered if one of the relevant dimensions is out of the window.
            /// 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只要有一个维度在数据窗口外，整个数据项就会被过滤掉。
            /// </summary>
            Filter,
            /// <summary>
            /// data that outside the window will be filtered, which may lead to some changes of windows of other axes.
            /// For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.
            /// 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只有当全部维度都在数据窗口同侧外部，整个数据项才会被过滤掉。
            /// </summary>
            WeakFilter,
            /// <summary>
            /// data that outside the window will be set to NaN, which will not lead to changes of windows of other axes. 
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
        [SerializeField] private int m_XAxisIndex;
        [SerializeField] private int m_YAxisIndex;
        [SerializeField] private bool m_SupportInside;
        [SerializeField] private bool m_SupportSlider;
        [SerializeField] private bool m_SupportSelect;
        [SerializeField] private bool m_ShowDataShadow;
        [SerializeField] private bool m_ShowDetail;
        [SerializeField] private bool m_ZoomLock;
        [SerializeField] private bool m_Realtime;
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField] private float m_Height;
        [SerializeField] private float m_Bottom;
        [SerializeField] private RangeMode m_RangeMode;
        [SerializeField] private float m_Start;
        [SerializeField] private float m_End;
        [SerializeField] private float m_StartValue;
        [SerializeField] private float m_EndValue;
        [SerializeField] private int m_MinShowNum = 1;
        [Range(1f, 20f)]
        [SerializeField] private float m_ScrollSensitivity = 1.1f;
        [SerializeField] private int m_FontSize = 18;
        [SerializeField] private FontStyle m_FontStyle;

        /// <summary>
        /// Whether to show dataZoom. 
        /// 是否显示缩放区域。
        /// </summary>
        public bool enable
        {
            get { return m_Enable; }
            set { if (PropertyUtility.SetStruct(ref m_Enable, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The mode of data filter. 
        /// 数据过滤类型。
        /// </summary>
        public FilterMode filterMode
        {
            get { return m_FilterMode; }
            set { if (PropertyUtility.SetStruct(ref m_FilterMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify which xAxis is controlled by the dataZoom. 
        /// 控制哪一个 x 轴。
        /// </summary>
        public int xAxisIndex
        {
            get { return m_XAxisIndex; }
            set { if (PropertyUtility.SetStruct(ref m_XAxisIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Specify which yAxis is controlled by the dataZoom. 
        /// 控制哪一个 y 轴。
        /// </summary>
        public int yAxisIndex
        {
            get { return m_YAxisIndex; }
            set { if (PropertyUtility.SetStruct(ref m_YAxisIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否支持内置。内置于坐标系中，使用户可以在坐标系上通过鼠标拖拽、鼠标滚轮、手指滑动（触屏上）来缩放或漫游坐标系。
        /// </summary>
        public bool supportInside
        {
            get { return m_SupportInside; }
            set { if (PropertyUtility.SetStruct(ref m_SupportInside, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否支持滑动条。有单独的滑动条，用户在滑动条上进行缩放或漫游。
        /// </summary>
        public bool supportSlider
        {
            get { return m_SupportSlider; }
            set { if (PropertyUtility.SetStruct(ref m_SupportSlider, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否支持框选。提供一个选框进行数据区域缩放。
        /// </summary>
        private bool supportSelect
        {
            get { return m_SupportSelect; }
            set { if (PropertyUtility.SetStruct(ref m_SupportSelect, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show data shadow, to indicate the data tendency in brief.
        /// default:true
        /// 是否显示数据阴影。数据阴影可以简单地反应数据走势。
        /// </summary>
        public bool showDataShadow
        {
            get { return m_ShowDataShadow; }
            set { if (PropertyUtility.SetStruct(ref m_ShowDataShadow, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show detail, that is, show the detailed data information when dragging.
        /// 是否显示detail，即拖拽时候显示详细数值信息。
        /// </summary>
        public bool showDetail
        {
            get { return m_ShowDetail; }
            set { if (PropertyUtility.SetStruct(ref m_ShowDetail, value)) SetVerticesDirty(); }
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
            set { if (PropertyUtility.SetStruct(ref m_ZoomLock, value)) SetVerticesDirty(); }
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
        private Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtility.SetStruct(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between dataZoom component and the bottom side of the container.
        /// bottom value is a instant pixel value like 10.
        /// default:10
        /// 组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtility.SetStruct(ref m_Bottom, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The height of dataZoom component.
        /// height value is a instant pixel value like 10.
        /// default:50
        /// 组件高度。
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtility.SetStruct(ref m_Height, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Use absolute value or percent value in DataZoom.start and DataZoom.end.
        /// default:RangeMode.Percent.
        /// 取绝对值还是百分比。
        /// </summary>
        public RangeMode rangeMode
        {
            get { return m_RangeMode; }
            set { if (PropertyUtility.SetStruct(ref m_RangeMode, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The start percentage of the window out of the data extent, in the range of 0 ~ 100.
        /// default:30
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
        /// 最小显示数据个数。当DataZoom放大到最大时，最小显示的数据个数。
        /// </summary>
        public int minShowNum
        {
            get { return m_MinShowNum; }
            set { if (PropertyUtility.SetStruct(ref m_MinShowNum, value)) SetVerticesDirty(); }
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
            set { if (PropertyUtility.SetStruct(ref m_ScrollSensitivity, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// font size.
        /// 文字的字体大小。
        /// </summary>
        public int fontSize
        {
            get { return m_FontSize; }
            set { if (PropertyUtility.SetStruct(ref m_FontSize, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// font style.
        /// 文字字体的风格。
        /// </summary>
        public FontStyle fontStyle
        {
            get { return m_FontStyle; }
            set { if (PropertyUtility.SetStruct(ref m_FontStyle, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// The start label.
        /// 组件的开始信息文本。
        /// </summary>
        private Text m_StartLabel { get; set; }
        /// <summary>
        /// The end label.
        /// 组件的结束信息文本。
        /// </summary>
        private Text m_EndLabel { get; set; }

        public static DataZoom defaultDataZoom
        {
            get
            {
                return new DataZoom()
                {
                    filterMode = FilterMode.None,
                    xAxisIndex = 0,
                    yAxisIndex = 0,
                    showDataShadow = true,
                    showDetail = false,
                    zoomLock = false,
                    m_Height = 0,
                    m_Bottom = 10,
                    rangeMode = RangeMode.Percent,
                    start = 30,
                    end = 70,
                    m_ScrollSensitivity = 10,
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
        public bool IsInZoom(Vector2 pos, float startX, float startY, float width)
        {
            Rect rect = Rect.MinMaxRect(startX, startY + m_Bottom, startX + width, startY + m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        /// <summary>
        /// 给定的坐标是否在选中区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startX"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IsInSelectedZoom(Vector2 pos, float startX, float startY, float width)
        {
            var start = startX + width * m_Start / 100;
            var end = startX + width * m_End / 100;
            Rect rect = Rect.MinMaxRect(start, startY + m_Bottom, end, startY + m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        /// <summary>
        /// 给定的坐标是否在开始活动条触发区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startX"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IsInStartZoom(Vector2 pos, float startX, float startY, float width)
        {
            var start = startX + width * m_Start / 100;
            Rect rect = Rect.MinMaxRect(start - 10, startY + m_Bottom, start + 10, startY + m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        /// <summary>
        /// 给定的坐标是否在结束活动条触发区域内
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startX"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IsInEndZoom(Vector2 pos, float startX, float startY, float width)
        {
            var end = startX + width * m_End / 100;
            Rect rect = Rect.MinMaxRect(end - 10, startY + m_Bottom, end + 10, startY + m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        /// <summary>
        /// 是否显示文本
        /// </summary>
        /// <param name="flag"></param>
        internal void SetLabelActive(bool flag)
        {
            if (m_StartLabel && m_StartLabel.gameObject.activeInHierarchy != flag)
            {
                m_StartLabel.gameObject.SetActive(flag);
            }
            if (m_EndLabel && m_EndLabel.gameObject.activeInHierarchy != flag)
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
            if (m_StartLabel) m_StartLabel.text = text;
        }

        /// <summary>
        /// 设置结束文本内容
        /// </summary>
        /// <param name="text"></param>
        internal void SetEndLabelText(string text)
        {
            if (m_EndLabel) m_EndLabel.text = text;
        }

        /// <summary>
        /// 获取DataZoom的高，当height设置为0时，自动计算合适的偏移。
        /// </summary>
        /// <param name="gridBottom"></param>
        /// <returns></returns>
        internal float GetHeight(float gridBottom)
        {
            if (height <= 0)
            {
                height = gridBottom - bottom - 30;
                if (height < 10) height = 10;
                return height;
            }
            else return height;
        }

        internal void SetStartLabel(Text startLabel)
        {
            m_StartLabel = startLabel;
        }

        internal void SetEndLabel(Text endLabel)
        {
            m_EndLabel = endLabel;
        }

        internal void UpdateStartLabelPosition(Vector3 pos)
        {
            m_StartLabel.transform.localPosition = pos;
        }

        internal void UpdateEndLabelPosition(Vector3 pos)
        {
            m_EndLabel.transform.localPosition = pos;
        }
    }
}