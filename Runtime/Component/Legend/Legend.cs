using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Legend component.The legend component shows different sets of tags, colors, and names. 
    /// You can control which series are not displayed by clicking on the legend.
    /// |图例组件。
    /// 图例组件展现了不同系列的标记，颜色和名字。可以通过点击图例控制哪些系列不显示。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(LegendHandler), true)]
    public class Legend : MainComponent, IPropertyChanged
    {
        public enum Type
        {
            /// <summary>
            /// 自动匹配。
            /// </summary>
            Auto,
            /// <summary>
            /// 自定义图标。
            /// </summary>
            Custom,
            /// <summary>
            /// 空心圆。
            /// </summary>
            EmptyCircle,
            /// <summary>
            /// 圆形。
            /// </summary>
            Circle,
            /// <summary>
            /// 正方形。可通过Setting的legendIconCornerRadius参数调整圆角。
            /// </summary>
            Rect,
            /// <summary>
            /// 三角形。
            /// </summary>
            Triangle,
            /// <summary>
            /// 菱形。
            /// </summary>
            Diamond,
            /// <summary>
            /// 烛台（可用于K线图）。
            /// </summary>
            Candlestick,
        }
        /// <summary>
        /// Selected mode of legend, which controls whether series can be toggled displaying by clicking legends.
        /// |图例选择的模式，控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。
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
            Single,
            /// <summary>
            /// 无法选择。
            /// </summary>
            None
        }

        [SerializeField] private bool m_Show = true;
        [SerializeField] private Type m_IconType = Type.Auto;
        [SerializeField] private SelectedMode m_SelectedMode = SelectedMode.Multiple;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private Location m_Location = new Location() { align = Location.Align.TopCenter, top = 0.125f };
        [SerializeField] private float m_ItemWidth = 25.0f;
        [SerializeField] private float m_ItemHeight = 12.0f;
        [SerializeField] private float m_ItemGap = 10f;
        [SerializeField] private bool m_ItemAutoColor = true;
        [SerializeField] private float m_ItemOpacity = 1;
        [SerializeField] private string m_Formatter;
        [SerializeField] protected string m_NumericFormatter = "";
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle();
        [SerializeField] private List<string> m_Data = new List<string>();
        [SerializeField] private List<Sprite> m_Icons = new List<Sprite>();
        [SerializeField] private List<Color> m_Colors = new List<Color>();
        [SerializeField][Since("v3.1.0")] protected ImageStyle m_Background = new ImageStyle() { show = false };
        [SerializeField][Since("v3.1.0")] protected Padding m_Padding = new Padding();
        [SerializeField][Since("v3.6.0")] private List<Vector3> m_Positions = new List<Vector3>();

        public LegendContext context = new LegendContext();

        /// <summary>
        /// Whether to show legend component.
        /// |是否显示图例组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Type of legend.
        /// |图例类型。
        /// </summary>
        public Type iconType
        {
            get { return m_IconType; }
            set { if (PropertyUtil.SetStruct(ref m_IconType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Selected mode of legend, which controls whether series can be toggled displaying by clicking legends.
        /// |选择模式。控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。
        /// </summary>
        public SelectedMode selectedMode
        {
            get { return m_SelectedMode; }
            set { if (PropertyUtil.SetStruct(ref m_SelectedMode, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Specify whether the layout of legend component is horizontal or vertical.
        /// |布局方式是横还是竖。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The location of legend.
        /// |图例显示的位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtil.SetClass(ref m_Location, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Image width of legend symbol.
        /// |图例标记的图形宽度。
        /// </summary>
        public float itemWidth
        {
            get { return m_ItemWidth; }
            set { if (PropertyUtil.SetStruct(ref m_ItemWidth, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Image height of legend symbol.
        /// |图例标记的图形高度。
        /// </summary>
        public float itemHeight
        {
            get { return m_ItemHeight; }
            set { if (PropertyUtil.SetStruct(ref m_ItemHeight, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The distance between each legend, horizontal distance in horizontal layout, and vertical distance in vertical layout.
        /// |图例每项之间的间隔。横向布局时为水平间隔，纵向布局时为纵向间隔。
        /// </summary>
        public float itemGap
        {
            get { return m_ItemGap; }
            set { if (PropertyUtil.SetStruct(ref m_ItemGap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether the legend symbol matches the color automatically.
        /// |图例标记的图形是否自动匹配颜色。
        /// </summary>
        public bool itemAutoColor
        {
            get { return m_ItemAutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_ItemAutoColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the opacity of item color.
        /// |图例标记的图形的颜色透明度。
        /// </summary>
        public float itemOpacity
        {
            get { return m_ItemOpacity; }
            set { if (PropertyUtil.SetStruct(ref m_ItemOpacity, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Standard numeric format strings.
        /// |标准数字格式字符串。用于将数值格式化显示为字符串。
        /// 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。
        /// 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string numericFormatter
        {
            get { return m_NumericFormatter; }
            set { if (PropertyUtil.SetClass(ref m_NumericFormatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Legend content string template formatter. Support for wrapping lines with \n. Template:{value}.
        /// |图例内容字符串模版格式器。支持用 \n 换行。
        /// 模板变量为图例名称 {value}。其他模板变量参考Toolip的itemFormatter。
        /// </summary>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the style of text.
        /// |文本样式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the sytle of background.
        /// |背景图样式。
        /// </summary>
        public ImageStyle background
        {
            get { return m_Background; }
            set { if (PropertyUtil.SetClass(ref m_Background, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the paddinng of item and background.
        /// |图例标记和背景的间距。
        /// </summary>
        public Padding padding
        {
            get { return m_Padding; }
            set { if (PropertyUtil.SetClass(ref m_Padding, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Data array of legend. An array item is usually a name representing string. (If it is a pie chart, 
        /// it could also be the name of a single data in the pie chart) of a series.
        /// If data is not specified, it will be auto collected from series.
        /// |图例的数据数组。数组项通常为一个字符串，每一项代表一个系列的 name（如果是饼图，也可以是饼图单个数据的 name）。
        /// 如果 data 没有被指定，会自动从当前系列中获取。指定data时里面的数据项和serie匹配时才会生效。
        /// </summary>
        public List<string> data
        {
            get { return m_Data; }
            set { if (value != null) { m_Data = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// 自定义的图例标记图形。
        /// </summary>
        public List<Sprite> icons
        {
            get { return m_Icons; }
            set { if (value != null) { m_Icons = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// the colors of legend item.
        /// |图例标记的颜色列表。
        /// </summary>
        public List<Color> colors
        {
            get { return m_Colors; }
            set { if (value != null) { m_Colors = value; SetAllDirty(); } }
        }
        /// <summary>
        /// the custom positions of legend item.
        /// |图例标记的自定义位置列表。
        /// </summary>
        public List<Vector3> positions
        {
            get { return m_Positions; }
            set { if (value != null) { m_Positions = value; SetAllDirty(); } }
        }
        /// <summary>
        /// 图表是否需要刷新（图例组件不需要刷新图表）
        /// </summary>
        public override bool vertsDirty { get { return false; } }
        /// <summary>
        /// 组件是否需要刷新
        /// </summary>
        public override bool componentDirty
        {
            get { return m_ComponentDirty || location.componentDirty || labelStyle.componentDirty; }
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            location.ClearComponentDirty();
            labelStyle.ClearComponentDirty();
        }

        /// <summary>
        /// Clear legend data.
        /// |清空。
        /// </summary>
        public override void ClearData()
        {
            m_Data.Clear();
            SetComponentDirty();
        }

        /// <summary>
        /// Whether include in legend data by the specified name.
        /// |是否包括由指定名字的图例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsData(string name)
        {
            return m_Data.Contains(name);
        }

        /// <summary>
        /// Removes the legend with the specified name.
        /// |移除指定名字的图例。
        /// </summary>
        /// <param name="name"></param>
        public void RemoveData(string name)
        {
            if (m_Data.Contains(name))
            {
                m_Data.Remove(name);
                SetComponentDirty();
            }
        }

        /// <summary>
        /// Add legend data.
        /// |添加图例。
        /// </summary>
        /// <param name="name"></param>
        public void AddData(string name)
        {
            if (!m_Data.Contains(name) && !string.IsNullOrEmpty(name))
            {
                m_Data.Add(name);
                SetComponentDirty();
            }
        }

        /// <summary>
        /// Gets the legend for the specified index.
        /// |获得指定索引的图例。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetData(int index)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                return m_Data[index];
            }
            return null;
        }

        /// <summary>
        /// Gets the index of the specified legend.
        /// |获得指定图例的索引。
        /// </summary>
        /// <param name="legendName"></param>
        /// <returns></returns>
        public int GetIndex(string legendName)
        {
            return m_Data.IndexOf(legendName);
        }

        /// <summary>
        /// Remove all legend buttons.
        /// |移除所有图例按钮。
        /// </summary>
        public void RemoveButton()
        {
            context.buttonList.Clear();
        }

        /// <summary>
        /// Bind buttons to legends.
        /// |给图例绑定按钮。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="btn"></param>
        /// <param name="total"></param>
        public void SetButton(string name, LegendItem item, int total)
        {
            context.buttonList[name] = item;
            int index = context.buttonList.Values.Count;
            item.SetIconActive(iconType == Type.Custom);
            item.SetActive(show);
        }

        /// <summary>
        /// Update the legend button color.
        /// |更新图例按钮颜色。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void UpdateButtonColor(string name, Color color)
        {
            if (context.buttonList.ContainsKey(name))
            {
                context.buttonList[name].SetIconColor(color);
            }
        }

        /// <summary>
        /// Update the text color of legend.
        /// |更新图例文字颜色。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void UpdateContentColor(string name, Color color)
        {
            if (context.buttonList.ContainsKey(name))
            {
                context.buttonList[name].SetContentColor(color);
            }
        }

        /// <summary>
        /// Gets the legend button for the specified index.
        /// |获得指定索引的图例按钮。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Sprite GetIcon(int index)
        {
            if (index >= 0 && index < m_Icons.Count)
            {
                return m_Icons[index];
            }
            else
            {
                return null;
            }
        }

        public Color GetColor(int index)
        {
            if (index >= 0 && index < m_Colors.Count)
                return m_Colors[index];
            else
                return Color.white;
        }

        public Vector3 GetPosition(int index, Vector3 defaultPos)
        {
            if (index >= 0 && index < m_Positions.Count)
                return m_Positions[index];
            else
                return defaultPos;
        }

        /// <summary>
        /// Callback handling when parameters change.
        /// |参数变更时的回调处理。
        /// </summary>
        public void OnChanged()
        {
            m_Location.OnChanged();
        }
    }
}