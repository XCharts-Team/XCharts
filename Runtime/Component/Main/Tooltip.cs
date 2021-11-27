/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// Tooltip component.
    /// 提示框组件。
    /// </summary>
    [System.Serializable]
    public class Tooltip : MainComponent
    {
        /// <summary>
        /// Indicator type.
        /// 指示器类型。
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// line indicator.
            /// 直线指示器
            /// </summary>
            Line,
            /// <summary>
            /// shadow crosshair indicator.
            /// 阴影指示器
            /// </summary>
            Shadow,
            /// <summary>
            /// no indicator displayed.
            /// 无指示器
            /// </summary>
            None,
            /// <summary>
            /// crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.
            /// 十字准星指示器。坐标轴显示Label和交叉线。
            /// </summary>
            Corss
        }

        [SerializeField] private bool m_Show;
        [SerializeField] private Type m_Type;
        [SerializeField] private string m_Formatter;
        [SerializeField] private string m_ItemFormatter;
        [SerializeField] private string m_TitleFormatter;
        [SerializeField] private float m_FixedWidth = 0;
        [SerializeField] private float m_FixedHeight = 0;
        [SerializeField] private float m_MinWidth = 0;
        [SerializeField] private float m_MinHeight = 0;
        [SerializeField] private string m_NumericFormatter = "";
        [SerializeField] private float m_PaddingLeftRight = 5f;
        [SerializeField] private float m_PaddingTopBottom = 5f;
        [SerializeField] private bool m_IgnoreDataShow = false;
        [SerializeField] private string m_IgnoreDataDefaultContent = "-";
        [SerializeField] private bool m_AlwayShow = false;
        [SerializeField] private Vector2 m_Offset = new Vector2(18f, -25f);
        [SerializeField] private Sprite m_BackgroundImage;
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();
        [SerializeField] private LineStyle m_LineStyle = new LineStyle(LineStyle.Type.None);
        private DelegateTooltipPosition m_PositionFunction;

        private GameObject m_GameObject;
        private GameObject m_Content;
        private ChartText m_ContentText;
        private Image m_ContentImage;
        private RectTransform m_ContentRect;
        private RectTransform m_ContentTextRect;
        private List<int> lastDataIndex = new List<int>();

        /// <summary>
        /// Whether to show the tooltip component.
        /// 是否显示提示框组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) { SetAllDirty(); SetActive(value); } }
        }
        /// <summary>
        /// Indicator type.
        /// 提示框指示器类型。
        /// </summary>
        public Type type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetAllDirty(); }
        }
        /// <summary>
        /// A string template formatter for the total content of the prompt box. Support for wrapping lines with \n. 
        /// When formatter is not null, use formatter first, otherwise use itemFormatter.
        /// Template variables are {.}, {a}, {b}, {c}, {d}.
        /// {.} is the dot of the corresponding color of a Serie that is currently indicated or whose index is 0.
        /// {a} is the series name of the serie that is currently indicated or whose index is 0.
        /// {b} is the name of the data item serieData that is currently indicated or whose index is 0, or a category value (such as the X-axis of a line chart).
        /// {c} is the value of a Y-dimension (dimesion is 1) from a Serie that is currently indicated or whose index is 0.
        /// {d} is the percentage value of Y-dimensions (dimesion is 1) from serie that is currently indicated or whose index is 0, with no % sign.
        /// {e} is the name of the data item serieData that is currently indicated or whose index is 0.
        /// {.1} represents a dot from serie corresponding color that specifies index as 1.
        /// 1 in {a1}, {b1}, {c1} represents a serie that specifies an index of 1.
        /// {c1:2} represents the third data from serie's current indication data item indexed to 1 (a data item has multiple data, index 2 represents the third data).
        /// {c1:2-2} represents the third data item from serie's third data item indexed to 1 (i.e., which data item must be specified to specify).
        /// {d1:2: F2} indicates that a formatted string with a value specified separately is F2 (numericFormatter is used when numericFormatter is not specified).
        /// {d:0.##} indicates that a formatted string with a value specified separately is 0.##   (used for percentage, reserved 2 valid digits while avoiding the situation similar to "100.00%" when using f2 ).
        /// Example: "{a}, {c}", "{a1}, {c1: f1}", "{a1}, {c1:0: f1}", "{a1} : {c1:1-1: f1}"
        /// 提示框总内容的字符串模版格式器。支持用 \n 换行。当formatter不为空时，优先使用formatter，否则使用itemFormatter。
        /// 模板变量有{.}、{a}、{b}、{c}、{d}。
        /// {.}为当前所指示或index为0的serie的对应颜色的圆点。
        /// {a}为当前所指示或index为0的serie的系列名name。
        /// {b}为当前所指示或index为0的serie的数据项serieData的name，或者类目值（如折线图的X轴）。
        /// {c}为当前所指示或index为0的serie的y维（dimesion为1）的数值。
        /// {d}为当前所指示或index为0的serie的y维（dimesion为1）百分比值，注意不带%号。
        /// {e}为当前所指示或index为0的serie的数据项serieData的name。
        /// {.1}表示指定index为1的serie对应颜色的圆点。
        /// {a1}、{b1}、{c1}中的1表示指定index为1的serie。
        /// {c1:2}表示索引为1的serie的当前指示数据项的第3个数据（一个数据项有多个数据，index为2表示第3个数据）。
        /// {c1:2-2}表示索引为1的serie的第3个数据项的第3个数据（也就是要指定第几个数据项时必须要指定第几个数据）。
        /// {d1:2:f2}表示单独指定了数值的格式化字符串为f2（不指定时用numericFormatter）。
        /// {d:0.##} 表示单独指定了数值的格式化字符串为 0.## （用于百分比，保留2位有效数同时又能避免使用 f2 而出现的类似于"100.00%"的情况 ）。
        /// 示例："{a}:{c}"、"{a1}:{c1:f1}"、"{a1}:{c1:0:f1}"、"{a1}:{c1:1-1:f1}"
        /// </summary>
        public string formatter { get { return m_Formatter; } set { m_Formatter = value; } }
        /// <summary>
        /// The string template formatter for the tooltip title content. Support for wrapping lines with \n. 
        /// This is only valid if the itemFormatter is in effect. 
        /// The placeholder {I} can be set separately to indicate that the title is ignored and not displayed.
        /// 提示框标题内容的字符串模版格式器。支持用 \n 换行。仅当itemFormatter生效时才有效。可以单独设置占位符{i}表示忽略不显示title。
        /// </summary>
        public string titleFormatter { get { return m_TitleFormatter; } set { m_TitleFormatter = value; } }
        /// <summary>
        /// a string template formatter for a single Serie or data item content. Support for wrapping lines with \n. 
        /// When formatter is not null, use formatter first, otherwise use itemFormatter.
        /// 提示框单个serie或数据项内容的字符串模版格式器。支持用 \n 换行。当formatter不为空时，优先使用formatter，否则使用itemFormatter。
        /// </summary>
        public string itemFormatter { get { return m_ItemFormatter; } set { m_ItemFormatter = value; } }

        /// <summary>
        /// Fixed width. Higher priority than minWidth.
        /// 固定宽度。比 minWidth 优先。
        /// </summary>
        public float fixedWidth { get { return m_FixedWidth; } set { m_FixedWidth = value; } }
        /// <summary>
        /// Fixed height. Higher priority than minHeight.
        /// 固定高度。比 minHeight 优先。
        /// </summary>
        public float fixedHeight { get { return m_FixedHeight; } set { m_FixedHeight = value; } }
        /// <summary>
        /// Minimum width. If fixedWidth has a value, get fixedWidth first.
        /// 最小宽度。如若 fixedWidth 设有值，优先取 fixedWidth。
        /// </summary>
        public float minWidth { get { return m_MinWidth; } set { m_MinWidth = value; } }
        /// <summary>
        /// Minimum height. If fixedHeight has a value, take priority over fixedHeight.
        /// 最小高度。如若 fixedHeight 设有值，优先取 fixedHeight。
        /// </summary>
        public float minHeight { get { return m_MinHeight; } set { m_MinHeight = value; } }
        /// <summary>
        /// Standard numeric format string. Used to format numeric values to display as strings. 
        /// Using 'Axx' form: 'A' is the single character of the format specifier, supporting 'C' currency, 
        /// 'D' decimal, 'E' exponent, 'F' number of vertices, 'G' regular, 'N' digits, 'P' percentage, 
        /// 'R' round tripping, 'X' hex etc. 'XX' is the precision specification, from '0' - '99'.
        /// 标准数字格式字符串。用于将数值格式化显示为字符串。
        /// 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。
        /// 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        /// <value></value>
        public string numericFormatter
        {
            get { return m_NumericFormatter; }
            set { if (PropertyUtil.SetClass(ref m_NumericFormatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the text padding of left and right. defaut:5.
        /// 左右边距。
        /// </summary>
        public float paddingLeftRight { get { return m_PaddingLeftRight; } set { m_PaddingLeftRight = value; } }
        /// <summary>
        /// the text padding of top and bottom. defaut:5.
        /// 上下边距。
        /// </summary>
        public float paddingTopBottom { get { return m_PaddingTopBottom; } set { m_PaddingTopBottom = value; } }
        /// <summary>
        /// Whether to show ignored data on tooltip.
        /// 是否显示忽略数据在tooltip上。
        /// </summary>
        public bool ignoreDataShow { get { return m_IgnoreDataShow; } set { m_IgnoreDataShow = value; } }
        /// <summary>
        /// The default display character information for ignored data.
        /// 被忽略数据的默认显示字符信息。
        /// </summary>
        public string ignoreDataDefaultContent { get { return m_IgnoreDataDefaultContent; } set { m_IgnoreDataDefaultContent = value; } }
        /// <summary>
        /// The image of icon.
        /// 图标的图片。
        /// </summary>
        public Sprite backgroundImage { get { return m_BackgroundImage; } set { m_BackgroundImage = value; SetBackground(m_BackgroundImage); } }
        /// <summary>
        /// Whether to trigger after always display.
        /// 是否触发后一直显示。
        /// </summary>
        public bool alwayShow { get { return m_AlwayShow; } set { m_AlwayShow = value; } }
        /// <summary>
        /// The position offset of tooltip relative to the mouse position.
        /// 提示框相对于鼠标位置的偏移。
        /// </summary>
        public Vector2 offset { get { return m_Offset; } set { m_Offset = value; } }
        /// <summary>
        /// the text style of content.
        /// 提示框内容文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (value != null) { m_TextStyle = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// the line style of indicator line.
        /// 指示线样式。
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (value != null) m_LineStyle = value; SetComponentDirty(); }
        }

        /// <summary>
        /// 组件是否需要刷新
        /// </summary>
        public override bool componentDirty
        {
            get { return m_ComponentDirty || lineStyle.componentDirty || textStyle.componentDirty; }
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            lineStyle.ClearComponentDirty();
            textStyle.ClearComponentDirty();
        }
        /// <summary>
        /// 当前提示框所指示的Serie索引（目前只对散点图有效）。
        /// </summary>
        public Dictionary<int, List<int>> runtimeSerieIndex = new Dictionary<int, List<int>>();
        /// <summary>
        /// The data index currently indicated by Tooltip.
        /// 当前提示框所指示的数据项索引。
        /// </summary>
        public List<int> runtimeDataIndex { get { return m_RuntimeDateIndex; } internal set { m_RuntimeDateIndex = value; } }
        private List<int> m_RuntimeDateIndex = new List<int>() { -1, -1 };
        /// <summary>
        /// the value for x indicator label.
        /// 指示器X轴上要显示的值。
        /// </summary>
        public double[] runtimeXValues { get { return m_RuntimeXValue; } internal set { m_RuntimeXValue = value; } }
        private double[] m_RuntimeXValue = new double[2] { -1, -1 };
        /// <summary>
        /// the value for y indicator label. 
        /// 指示器Y轴上要显示的值。
        /// </summary>
        public double[] runtimeYValues { get { return m_RuntimeYValue; } internal set { m_RuntimeYValue = value; } }
        private double[] m_RuntimeYValue = new double[2] { -1, -1 };
        /// <summary>
        /// the current pointer position.
        /// 当前鼠标位置。
        /// </summary>
        public Vector2 runtimePointerPos { get; internal set; }
        /// <summary>
        /// the width of tooltip. 
        /// 提示框宽。
        /// </summary>
        public float runtimeWidth { get { return m_ContentRect.sizeDelta.x; } }
        /// <summary>
        /// the height of tooltip. 
        /// 提示框高。
        /// </summary>
        public float runtimeHeight { get { return m_ContentRect.sizeDelta.y; } }
        /// <summary>
        /// Whether the tooltip has been initialized. 
        /// 提示框是否已初始化。
        /// </summary>
        public bool runtimeInited { get { return m_GameObject != null; } }
        /// <summary>
        /// the gameObject of tooltip. 
        /// 提示框的gameObject。
        /// </summary>
        public GameObject runtimeGameObject { get { return m_GameObject; } }
        /// <summary>
        /// 当前指示的角度。
        /// </summary>
        public float runtimeAngle { get; internal set; }
        /// <summary>
        /// 当前指示的Grid索引。
        /// </summary>
        public int runtimeGridIndex { get; internal set; }
        public int runtimePolarIndex { get; internal set; }

        public DelegateTooltipPosition positionFunction
        {
            get { return m_PositionFunction; }
            set { m_PositionFunction = value; }
        }

        public static Tooltip defaultTooltip
        {
            get
            {
                var tooltip = new Tooltip
                {
                    m_Show = true
                };
                return tooltip;
            }
        }

        /// <summary>
        /// 绑定提示框gameObject
        /// </summary>
        /// <param name="obj"></param>
        public void SetObj(GameObject obj)
        {
            m_GameObject = obj;
            m_GameObject.SetActive(false);
        }

        /// <summary>
        /// 绑定提示框的文本框gameObject
        /// </summary>
        /// <param name="content"></param>
        public void SetContentObj(GameObject content)
        {
            m_Content = content;
            m_ContentRect = m_Content.GetComponent<RectTransform>();
            m_ContentImage = m_Content.GetComponent<Image>();
            m_ContentImage.raycastTarget = false;
            m_ContentText = new ChartText(m_Content);
            if (m_ContentText != null)
            {
                m_ContentTextRect = m_ContentText.gameObject.GetComponentInChildren<RectTransform>();
            }
            SetBackground(backgroundImage);
        }

        /// <summary>
        /// Keep Tooltiop displayed at the top. 
        /// 保持Tooltiop显示在最顶上
        /// </summary>
        public void UpdateToTop()
        {
            if (m_GameObject == null) return;
            int count = m_GameObject.transform.parent.childCount;
            m_GameObject.GetComponent<RectTransform>().SetSiblingIndex(count - 1);
        }

        /// <summary>
        /// 设置提示框文本背景色
        /// </summary>
        /// <param name="color"></param>
        public void SetContentBackgroundColor(Color color)
        {
            if (m_ContentImage != null)
                m_ContentImage.color = color;
        }

        /// <summary>
        /// 设置提示框文本背景图片
        /// </summary>
        /// <param name="sprite"></param>
        public void SetBackground(Sprite sprite)
        {
            if (m_ContentImage != null)
            {
                m_ContentImage.type = Image.Type.Sliced;
                m_ContentImage.sprite = sprite;
            }
        }

        /// <summary>
        /// 设置提示框文本字体颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetContentTextColor(Color color)
        {
            if (m_ContentText != null)
            {
                m_ContentText.SetColor(color);
            }
        }

        /// <summary>
        /// 设置提示框文本内容
        /// </summary>
        /// <param name="txt"></param>
        public void UpdateContentText(string txt)
        {
            if (m_ContentText != null)
            {
                m_ContentText.SetText(txt);
                float wid, hig;
                if (m_FixedWidth > 0) wid = m_FixedWidth;
                else if (m_MinWidth > 0 && m_ContentText.GetPreferredWidth() < m_MinWidth) wid = m_MinWidth;
                else wid = m_ContentText.GetPreferredWidth() + m_PaddingLeftRight * 2;
                if (m_FixedHeight > 0) hig = m_FixedHeight;
                else if (m_MinHeight > 0 && m_ContentText.GetPreferredHeight() < m_MinHeight) hig = m_MinHeight;
                else hig = m_ContentText.GetPreferredHeight() + m_PaddingTopBottom * 2;
                if (m_ContentRect != null) m_ContentRect.sizeDelta = new Vector2(wid, hig);
                if (m_ContentTextRect != null)
                {
                    m_ContentTextRect.anchoredPosition = new Vector3(m_PaddingLeftRight, -m_PaddingTopBottom);
                }
            }
        }

        /// <summary>
        /// 清除提示框指示数据
        /// </summary>
        internal void ClearValue()
        {
            for (int i = 0; i < runtimeDataIndex.Count; i++) runtimeDataIndex[i] = -1;
            for (int i = 0; i < runtimeXValues.Length; i++) runtimeXValues[i] = -1;
            for (int i = 0; i < runtimeYValues.Length; i++) runtimeYValues[i] = -1;
        }

        /// <summary>
        /// 提示框是否显示
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return m_GameObject != null && m_GameObject.activeInHierarchy;
        }

        /// <summary>
        /// 设置提示框是否显示
        /// </summary>
        /// <param name="flag"></param>
        public void SetActive(bool flag)
        {
            if (!flag && m_AlwayShow) return;
            if (lastDataIndex.Count >= 2)
                lastDataIndex[0] = lastDataIndex[1] = -1;
            if (m_GameObject && m_GameObject.activeInHierarchy != flag)
                m_GameObject.SetActive(flag);
        }

        /// <summary>
        /// 更新文本框位置
        /// </summary>
        /// <param name="pos"></param>
        public void UpdateContentPos(Vector2 pos)
        {
            if (m_Content)
            {
                if (m_PositionFunction != null)
                    m_Content.transform.localPosition = m_PositionFunction(pos);
                else
                    m_Content.transform.localPosition = pos;
            }
        }

        /// <summary>
        /// 获得当前提示框的位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetContentPos()
        {
            if (m_Content)
                return m_Content.transform.localPosition;
            else
                return Vector3.zero;
        }

        /// <summary>
        /// Whether the data item indicated by tooltip has changed. 
        /// 提示框所指示的数据项是否发生变化。
        /// </summary>
        /// <returns></returns>
        public bool IsDataIndexChanged()
        {
            if (runtimeDataIndex.Count < 2 || lastDataIndex.Count < 2) return false;
            return runtimeDataIndex[0] != lastDataIndex[0] ||
                runtimeDataIndex[1] != lastDataIndex[1];
        }

        /// <summary>
        /// 当前索引缓存
        /// </summary>
        internal void UpdateLastDataIndex()
        {
            if (lastDataIndex.Count > 0 && runtimeDataIndex.Count > 0) lastDataIndex[0] = runtimeDataIndex[0];
            if (lastDataIndex.Count > 0 && runtimeDataIndex.Count > 1) lastDataIndex[1] = runtimeDataIndex[1];
        }

        /// <summary>
        /// 当前提示框是否选中数据项
        /// </summary>
        /// <returns></returns>
        public bool IsSelected()
        {
            foreach (var index in runtimeDataIndex)
                if (index >= 0) return true;
            return false;
        }

        /// <summary>
        /// 指定索引的数据项是否被提示框选中
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsSelected(int index)
        {
            foreach (var temp in runtimeDataIndex)
                if (temp == index) return true;
            return false;
        }

        public void ClearSerieDataIndex()
        {
            foreach (var kv in runtimeSerieIndex)
            {
                kv.Value.Clear();
            }
        }

        public void AddSerieDataIndex(int serieIndex, int dataIndex)
        {
            if (!runtimeSerieIndex.ContainsKey(serieIndex))
            {
                runtimeSerieIndex[serieIndex] = new List<int>();
            }
            runtimeSerieIndex[serieIndex].Add(dataIndex);
        }

        public bool isAnySerieDataIndex()
        {
            foreach (var kv in runtimeSerieIndex)
            {
                if (kv.Value.Count > 0) return true;
            }
            return false;
        }
    }
}