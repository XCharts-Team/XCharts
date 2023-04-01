using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// Tooltip component.
    /// |提示框组件。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(TooltipHandler), true)]
    public class Tooltip : MainComponent
    {
        /// <summary>
        /// Indicator type.
        /// |指示器类型。
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// line indicator.
            /// |直线指示器
            /// </summary>
            Line,
            /// <summary>
            /// shadow crosshair indicator.
            /// |阴影指示器
            /// </summary>
            Shadow,
            /// <summary>
            /// no indicator displayed.
            /// |无指示器
            /// </summary>
            None,
            /// <summary>
            /// crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.
            /// |十字准星指示器。坐标轴显示Label和交叉线。
            /// </summary>
            Corss
        }

        /// <summary>
        /// Trigger strategy.
        /// |触发类型。
        /// </summary>
        public enum Trigger
        {
            /// <summary>
            /// Triggered by data item, which is mainly used for charts that don't have a category axis like scatter charts or pie charts.
            /// |数据项图形触发，主要在散点图，饼图等无类目轴的图表中使用。
            /// </summary>
            Item,
            /// <summary>
            /// Triggered by axes, which is mainly used for charts that have category axes, like bar charts or line charts.
            /// |坐标轴触发，主要在柱状图，折线图等会使用类目轴的图表中使用。
            /// </summary>
            Axis,
            /// <summary>
            /// Trigger nothing.
            /// |什么都不触发。
            /// </summary>
            None
        }
        /// <summary>
        /// Position type.
        /// |坐标类型。
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// Auto. The mobile platform is displayed at the top, and the non-mobile platform follows the mouse position.
            /// |自适应。移动平台靠顶部显示，非移动平台跟随鼠标位置。
            /// </summary>
            Auto,
            /// <summary>
            /// Custom. Fully customize display position (x,y).
            /// |自定义。完全自定义显示位置(x,y)。
            /// </summary>
            Custom,
            /// <summary>
            /// Just fix the coordinate X. Y follows the mouse position.
            /// |只固定坐标X。Y跟随鼠标位置。
            /// </summary>
            FixedX,
            /// <summary>
            /// Just fix the coordinate Y. X follows the mouse position.
            /// |只固定坐标Y。X跟随鼠标位置。
            FixedY
        }

        [SerializeField] private bool m_Show = true;
        [SerializeField] private Type m_Type;
        [SerializeField] private Trigger m_Trigger = Trigger.Item;
        [SerializeField][Since("v3.3.0")] private Position m_Position = Position.Auto;
        [SerializeField] private string m_ItemFormatter;
        [SerializeField] private string m_TitleFormatter;
        [SerializeField] private string m_Marker = "●";
        [SerializeField] private float m_FixedWidth = 0;
        [SerializeField] private float m_FixedHeight = 0;
        [SerializeField] private float m_MinWidth = 0;
        [SerializeField] private float m_MinHeight = 0;
        [SerializeField] private string m_NumericFormatter = "";
        [SerializeField] private int m_PaddingLeftRight = 10;
        [SerializeField] private int m_PaddingTopBottom = 10;
        [SerializeField] private bool m_IgnoreDataShow = false;
        [SerializeField] private string m_IgnoreDataDefaultContent = "-";
        [SerializeField] private bool m_ShowContent = true;
        [SerializeField] private bool m_AlwayShowContent = false;
        [SerializeField] private Vector2 m_Offset = new Vector2(18f, -25f);
        [SerializeField] private Sprite m_BackgroundImage;
        [SerializeField] private Image.Type m_BackgroundType = Image.Type.Simple;
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField] private float m_BorderWidth = 2f;
        [SerializeField] private float m_FixedX = 0f;
        [SerializeField] private float m_FixedY = 0.7f;
        [SerializeField] private float m_TitleHeight = 25f;
        [SerializeField] private float m_ItemHeight = 25f;
        [SerializeField] private Color32 m_BorderColor = new Color32(230, 230, 230, 255);
        [SerializeField] private LineStyle m_LineStyle = new LineStyle(LineStyle.Type.None);
        [SerializeField]
        private LabelStyle m_TitleLabelStyle = new LabelStyle()
        {
            textStyle = new TextStyle() { alignment = TextAnchor.MiddleLeft }
        };
        [SerializeField]
        private List<LabelStyle> m_ContentLabelStyles = new List<LabelStyle>()
        {
            new LabelStyle() { textPadding = new TextPadding(0, 5, 0, 0), textStyle = new TextStyle() { alignment = TextAnchor.MiddleLeft } },
            new LabelStyle() { textPadding = new TextPadding(0, 20, 0, 0), textStyle = new TextStyle() { alignment = TextAnchor.MiddleLeft } },
            new LabelStyle() { textPadding = new TextPadding(0, 0, 0, 0), textStyle = new TextStyle() { alignment = TextAnchor.MiddleRight } }
        };

        public TooltipContext context = new TooltipContext();
        public TooltipView view;

        /// <summary>
        /// Whether to show the tooltip component.
        /// |是否显示提示框组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) { SetAllDirty(); SetActive(value); } }
        }
        /// <summary>
        /// Indicator type.
        /// |提示框指示器类型。
        /// </summary>
        public Type type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Type of triggering.
        /// |触发类型。
        /// </summary>
        public Trigger trigger
        {
            get { return m_Trigger; }
            set { if (PropertyUtil.SetStruct(ref m_Trigger, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Type of position.
        /// |显示位置类型。
        /// </summary>
        public Position position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The string template formatter for the tooltip title content. Support for wrapping lines with \n.
        /// The placeholder {I} can be set separately to indicate that the title is ignored and not displayed.
        /// Template see itemFormatter.
        /// |提示框标题内容的字符串模版格式器。支持用 \n 换行。可以单独设置占位符{i}表示忽略不显示title。
        /// 模板变量有{.}、{a}、{b}、{c}、{d}、{e}、{f}、{g}。<br/>
        /// {.}为当前所指示或index为0的serie的对应颜色的圆点。<br/>
        /// {a}为当前所指示或index为0的serie的系列名name。<br/>
        /// {b}为当前所指示或index为0的serie的数据项serieData的name，或者类目值（如折线图的X轴）。<br/>
        /// {c}为当前所指示或index为0的serie的y维（dimesion为1）的数值。<br/>
        /// {d}为当前所指示或index为0的serie的y维（dimesion为1）百分比值，注意不带%号。<br/>
        /// {e}为当前所指示或index为0的serie的数据项serieData的name。<br/>
        /// {h}为当前所指示或index为0的serie的数据项serieData的十六进制颜色值。<br/>
        /// {f}为数据总和。<br/>
        /// {g}为数据总个数。<br/>
        /// {.1}表示指定index为1的serie对应颜色的圆点。<br/>
        /// {a1}、{b1}、{c1}中的1表示指定index为1的serie。<br/>
        /// {c1:2}表示索引为1的serie的当前指示数据项的第3个数据（一个数据项有多个数据，index为2表示第3个数据）。<br/>
        /// {c1:2-2}表示索引为1的serie的第3个数据项的第3个数据（也就是要指定第几个数据项时必须要指定第几个数据）。<br/>
        /// {d1:2:f2}表示单独指定了数值的格式化字符串为f2（不指定时用numericFormatter）。<br/>
        /// {d:0.##} 表示单独指定了数值的格式化字符串为 0.## （用于百分比，保留2位有效数同时又能避免使用 f2 而出现的类似于"100.00%"的情况 ）。<br/>
        /// 示例："{a}:{c}"、"{a1}:{c1:f1}"、"{a1}:{c1:0:f1}"、"{a1}:{c1:1-1:f1}"
        /// </summary>
        /// </summary>
        public string titleFormatter { get { return m_TitleFormatter; } set { m_TitleFormatter = value; } }
        /// <summary>
        /// a string template formatter for a single Serie or data item content. Support for wrapping lines with \n.
        /// Template variables are {.}, {a}, {b}, {c}, {d}.<br/>
        /// {.} is the dot of the corresponding color of a Serie that is currently indicated or whose index is 0.<br/>
        /// {a} is the series name of the serie that is currently indicated or whose index is 0.<br/>
        /// {b} is the name of the data item serieData that is currently indicated or whose index is 0, or a category value (such as the X-axis of a line chart).<br/>
        /// {c} is the value of a Y-dimension (dimesion is 1) from a Serie that is currently indicated or whose index is 0.<br/>
        /// {d} is the percentage value of Y-dimensions (dimesion is 1) from serie that is currently indicated or whose index is 0, with no % sign.<br/>
        /// {e} is the name of the data item serieData that is currently indicated or whose index is 0.<br/>
        /// {f} is sum of data.<br/>
        /// {.1} represents a dot from serie corresponding color that specifies index as 1.<br/>
        /// 1 in {a1}, {b1}, {c1} represents a serie that specifies an index of 1.<br/>
        /// {c1:2} represents the third data from serie's current indication data item indexed to 1 (a data item has multiple data, index 2 represents the third data).<br/>
        /// {c1:2-2} represents the third data item from serie's third data item indexed to 1 (i.e., which data item must be specified to specify).<br/>
        /// {d1:2: F2} indicates that a formatted string with a value specified separately is F2 (numericFormatter is used when numericFormatter is not specified).<br/>
        /// {d:0.##} indicates that a formatted string with a value specified separately is 0.##   (used for percentage, reserved 2 valid digits while avoiding the situation similar to "100.00%" when using f2 ).<br/>
        /// Example: "{a}, {c}", "{a1}, {c1: f1}", "{a1}, {c1:0: f1}", "{a1} : {c1:1-1: f1}"<br/>
        /// |提示框单个serie或数据项内容的字符串模版格式器。支持用 \n 换行。用|来表示多个列的分隔。
        /// 模板变量有{.}、{a}、{b}、{c}、{d}、{e}、{f}、{g}。<br/>
        /// {i}或-表示忽略当前项。
        /// {.}为当前所指示的serie或数据项的对应颜色的圆点。<br/>
        /// {a}为当前所指示的serie或数据项的系列名name。<br/>
        /// {b}为当前所指示的serie或数据项的数据项serieData的name，或者类目值（如折线图的X轴）。<br/>
        /// {c}为当前所指示的serie或数据项的y维（dimesion为1）的数值。<br/>
        /// {d}为当前所指示的serie或数据项的y维（dimesion为1）百分比值，注意不带%号。<br/>
        /// {e}为当前所指示的serie或数据项的数据项serieData的name。<br/>
        /// {f}为当前所指示的serie的默认维度的数据总和。<br/>
        /// {g}为当前所指示的serie的数据总个数。<br/>
        /// {h}为当前所指示的serie的十六进制颜色值。<br/>
        /// {c0}表示当前数据项维度为0的数据。<br/>
        /// {c1}表示当前数据项维度为1的数据。<br/>
        /// {d3}表示维度3的数据的百分比。它的分母是默认维度（一般是1维度）数据。<br/>
        /// |表示多个列的分隔。<br>
        /// 示例："{i}", "{.}|{a}|{c}", "{.}|{b}|{c2:f2}"
        /// </summary>
        public string itemFormatter { get { return m_ItemFormatter; } set { m_ItemFormatter = value; } }
        /// <summary>
        /// the marker of serie.
        /// |serie的符号标志。
        /// </summary>
        public string marker { get { return m_Marker; } set { m_Marker = value; } }
        /// <summary>
        /// Fixed width. Higher priority than minWidth.
        /// |固定宽度。比 minWidth 优先。
        /// </summary>
        public float fixedWidth { get { return m_FixedWidth; } set { m_FixedWidth = value; } }
        /// <summary>
        /// Fixed height. Higher priority than minHeight.
        /// |固定高度。比 minHeight 优先。
        /// </summary>
        public float fixedHeight { get { return m_FixedHeight; } set { m_FixedHeight = value; } }
        /// <summary>
        /// Minimum width. If fixedWidth has a value, get fixedWidth first.
        /// |最小宽度。如若 fixedWidth 设有值，优先取 fixedWidth。
        /// </summary>
        public float minWidth { get { return m_MinWidth; } set { m_MinWidth = value; } }
        /// <summary>
        /// Minimum height. If fixedHeight has a value, take priority over fixedHeight.
        /// |最小高度。如若 fixedHeight 设有值，优先取 fixedHeight。
        /// </summary>
        public float minHeight { get { return m_MinHeight; } set { m_MinHeight = value; } }
        /// <summary>
        /// Standard numeric format string. Used to format numeric values to display as strings.
        /// Using 'Axx' form: 'A' is the single character of the format specifier, supporting 'C' currency, 
        /// 'D' decimal, 'E' exponent, 'F' number of vertices, 'G' regular, 'N' digits, 'P' percentage, 
        /// 'R' round tripping, 'X' hex etc. 'XX' is the precision specification, from '0' - '99'.
        /// |标准数字格式字符串。用于将数值格式化显示为字符串。
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
        /// |左右边距。
        /// </summary>
        public int paddingLeftRight { get { return m_PaddingLeftRight; } set { m_PaddingLeftRight = value; } }
        /// <summary>
        /// the text padding of top and bottom. defaut:5.
        /// |上下边距。
        /// </summary>
        public int paddingTopBottom { get { return m_PaddingTopBottom; } set { m_PaddingTopBottom = value; } }
        /// <summary>
        /// Whether to show ignored data on tooltip.
        /// |是否显示忽略数据在tooltip上。
        /// </summary>
        public bool ignoreDataShow { get { return m_IgnoreDataShow; } set { m_IgnoreDataShow = value; } }
        /// <summary>
        /// The default display character information for ignored data.
        /// |被忽略数据的默认显示字符信息。如果设置为空，则表示完全不显示忽略数据。
        /// </summary>
        public string ignoreDataDefaultContent { get { return m_IgnoreDataDefaultContent; } set { m_IgnoreDataDefaultContent = value; } }
        /// <summary>
        /// The background image of tooltip.
        /// |提示框的背景图片。
        /// </summary>
        public Sprite backgroundImage { get { return m_BackgroundImage; } set { m_BackgroundImage = value; SetComponentDirty(); } }
        /// <summary>
        /// The background type of tooltip.
        /// |提示框的背景图片显示类型。
        /// </summary>
        public Image.Type backgroundType { get { return m_BackgroundType; } set { m_BackgroundType = value; SetComponentDirty(); } }
        /// <summary>
        /// The background color of tooltip.
        /// |提示框的背景颜色。
        /// </summary>
        public Color backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; SetComponentDirty(); } }
        /// <summary>
        /// Whether to trigger after always display.
        /// |是否触发后一直显示提示框浮层。
        /// </summary>
        public bool alwayShowContent { get { return m_AlwayShowContent; } set { m_AlwayShowContent = value; } }
        /// <summary>
        /// Whether to show the tooltip floating layer, whose default value is true.
        /// It should be configurated to be false, if you only need tooltip to trigger the event or show the axisPointer without content.
        /// |是否显示提示框浮层，默认显示。只需tooltip触发事件或显示axisPointer而不需要显示内容时可配置该项为false。
        /// </summary>
        public bool showContent { get { return m_ShowContent; } set { m_ShowContent = value; } }
        /// <summary>
        /// The position offset of tooltip relative to the mouse position.
        /// |提示框相对于鼠标位置的偏移。
        /// </summary>
        public Vector2 offset { get { return m_Offset; } set { m_Offset = value; } }
        /// <summary>
        /// the width of tooltip border.
        /// |边框线宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of tooltip border.
        /// |边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the x positionn of fixedX.
        /// |固定X位置的坐标。
        /// </summary>
        public float fixedX
        {
            get { return m_FixedX; }
            set { if (PropertyUtil.SetStruct(ref m_FixedX, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the y position of fixedY.
        /// |固定Y位置的坐标。
        /// </summary>
        public float fixedY
        {
            get { return m_FixedY; }
            set { if (PropertyUtil.SetStruct(ref m_FixedY, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// height of title text.
        /// |标题文本的高。
        /// </summary>
        public float titleHeight
        {
            get { return m_TitleHeight; }
            set { if (PropertyUtil.SetStruct(ref m_TitleHeight, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// height of content text.
        /// |数据项文本的高。
        /// </summary>
        public float itemHeight
        {
            get { return m_ItemHeight; }
            set { if (PropertyUtil.SetStruct(ref m_ItemHeight, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the textstyle of title.
        /// |标题的文本样式。
        /// </summary>
        public LabelStyle titleLabelStyle
        {
            get { return m_TitleLabelStyle; }
            set { if (value != null) { m_TitleLabelStyle = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// the textstyle list of content.
        /// |内容部分的文本样式列表。和列一一对应。
        /// </summary>
        public List<LabelStyle> contentLabelStyles
        {
            get { return m_ContentLabelStyles; }
            set { if (value != null) { m_ContentLabelStyles = value; SetComponentDirty(); } }
        }

        /// <summary>
        /// the line style of indicator line.
        /// |指示线样式。
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
            get { return m_ComponentDirty || lineStyle.componentDirty; }
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            lineStyle.ClearComponentDirty();
        }
        /// <summary>
        /// 当前提示框所指示的Serie索引（目前只对散点图有效）。
        /// </summary>
        public Dictionary<int, List<int>> runtimeSerieIndex = new Dictionary<int, List<int>>();
        /// <summary>
        /// The data index currently indicated by Tooltip.
        /// |当前提示框所指示的数据项索引。
        /// </summary>
        public List<int> runtimeDataIndex { get { return m_RuntimeDateIndex; } internal set { m_RuntimeDateIndex = value; } }
        private List<int> m_RuntimeDateIndex = new List<int>() {-1, -1 };

        /// <summary>
        /// Keep Tooltiop displayed at the top.
        /// |保持Tooltiop显示在最顶上
        /// </summary>
        public void KeepTop()
        {
            gameObject.transform.SetAsLastSibling();
        }

        public override void ClearData()
        {
            ClearValue();
        }

        /// <summary>
        /// 清除提示框指示数据
        /// </summary>
        internal void ClearValue()
        {
            for (int i = 0; i < runtimeDataIndex.Count; i++) runtimeDataIndex[i] = -1;
        }

        /// <summary>
        /// 提示框是否显示
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return gameObject != null && gameObject.activeInHierarchy;
        }

        /// <summary>
        /// 设置Tooltip组件是否显示
        /// </summary>
        /// <param name="flag"></param>
        public void SetActive(bool flag)
        {
            if (gameObject && gameObject.activeInHierarchy != flag)
            {
                gameObject.SetActive(alwayShowContent ? true : flag);
            }
            SetContentActive(flag);
        }

        /// <summary>
        /// 更新文本框位置
        /// </summary>
        /// <param name="pos"></param>
        public void UpdateContentPos(Vector2 pos, float width, float height)
        {
            if (view != null)
            {
                switch (m_Position)
                {
                    case Position.Auto:
#if UNITY_ANDROID || UNITY_IOS
                        if (m_FixedY == 0) pos.y = ChartHelper.GetActualValue(0.7f, height);
                        else pos.y = ChartHelper.GetActualValue(m_FixedY, height);
#endif
                        break;
                    case Position.Custom:
                        pos.x = ChartHelper.GetActualValue(m_FixedX, width);
                        pos.y = ChartHelper.GetActualValue(m_FixedY, height);
                        break;
                    case Position.FixedX:
                        pos.x = ChartHelper.GetActualValue(m_FixedX, width);
                        break;
                    case Position.FixedY:
                        pos.y = ChartHelper.GetActualValue(m_FixedY, height);
                        break;
                }
                view.UpdatePosition(pos);
            }
        }

        /// <summary>
        /// 设置文本框是否显示
        /// </summary>
        /// <param name="flag"></param>
        public void SetContentActive(bool flag)
        {
            if (view == null)
                return;

            view.SetActive(alwayShowContent ? true : flag);
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

        public bool IsTriggerItem()
        {
            return trigger == Trigger.Item;
        }

        public bool IsTriggerAxis()
        {
            return trigger == Trigger.Axis;
        }

        public LabelStyle GetContentLabelStyle(int index)
        {
            if (m_ContentLabelStyles.Count == 0)
                return null;

            if (index < 0)
                index = 0;
            else if (index > m_ContentLabelStyles.Count - 1)
                index = m_ContentLabelStyles.Count - 1;

            return m_ContentLabelStyles[index];
        }
    }
}