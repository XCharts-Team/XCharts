/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Legend component.The legend component shows different sets of tags, colors, and names. 
    /// You can control which series are not displayed by clicking on the legend.
    /// 图例组件。
    /// 图例组件展现了不同系列的标记，颜色和名字。可以通过点击图例控制哪些系列不显示。
    /// </summary>
    [System.Serializable]
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
        }
        /// <summary>
        /// Selected mode of legend, which controls whether series can be toggled displaying by clicking legends. 
        /// 图例选择的模式，控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。
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
        [SerializeField] private Type m_IconType;
        [SerializeField] private SelectedMode m_SelectedMode;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private Location m_Location = Location.defaultRight;
        [SerializeField] private float m_ItemWidth = 25.0f;
        [SerializeField] private float m_ItemHeight = 12.0f;
        [SerializeField] private float m_ItemGap = 10f;
        [SerializeField] private bool m_ItemAutoColor = true;
        [SerializeField] private bool m_TextAutoColor = false;
        [SerializeField] private string m_Formatter;
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();
        [SerializeField] private List<string> m_Data = new List<string>();
        [SerializeField] private List<Sprite> m_Icons = new List<Sprite>();

        private Dictionary<string, LegendItem> m_DataBtnList = new Dictionary<string, LegendItem>();
        private Dictionary<int, float> m_RuntimeEachWidth = new Dictionary<int, float>();

        /// <summary>
        /// Whether to show legend component. 
        /// 是否显示图例组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Type of legend. 
        /// 图例类型。
        /// [default:Type.Auto] 
        /// </summary>
        public Type iconType
        {
            get { return m_IconType; }
            set { if (PropertyUtil.SetStruct(ref m_IconType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Selected mode of legend, which controls whether series can be toggled displaying by clicking legends. 
        /// 选择模式。控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。
        /// [default:SelectedMode.Multiple] 
        /// </summary>
        public SelectedMode selectedMode
        {
            get { return m_SelectedMode; }
            set { if (PropertyUtil.SetStruct(ref m_SelectedMode, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Specify whether the layout of legend component is horizontal or vertical. 
        /// 布局方式是横还是竖。
        /// [default:Orient.Horizonal]
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The location of legend.
        /// 图例显示的位置。
        /// [default:Location.defaultTop]
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtil.SetClass(ref m_Location, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Image width of legend symbol.
        /// 图例标记的图形宽度。
        /// [default:24f]
        /// </summary>
        public float itemWidth
        {
            get { return m_ItemWidth; }
            set { if (PropertyUtil.SetStruct(ref m_ItemWidth, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Image height of legend symbol.
        /// 图例标记的图形高度。
        /// [default:12f]
        /// </summary>
        public float itemHeight
        {
            get { return m_ItemHeight; }
            set { if (PropertyUtil.SetStruct(ref m_ItemHeight, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The distance between each legend, horizontal distance in horizontal layout, and vertical distance in vertical layout.
        /// 图例每项之间的间隔。横向布局时为水平间隔，纵向布局时为纵向间隔。
        /// [default:10f]
        /// </summary>
        public float itemGap
        {
            get { return m_ItemGap; }
            set { if (PropertyUtil.SetStruct(ref m_ItemGap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether the legend symbol matches the color automatically.
        /// 图例标记的图形是否自动匹配颜色。
        /// [default:true]
        /// </summary>
        public bool itemAutoColor
        {
            get { return m_ItemAutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_ItemAutoColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether the legend text matches the color automatically.
        /// 图例标记的文本是否自动匹配颜色。
        /// [default:false]
        /// </summary>
        public bool textAutoColor
        {
            get { return m_TextAutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_TextAutoColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Legend content string template formatter. Support for wrapping lines with \n. Template:{name}.
        /// 图例内容字符串模版格式器。支持用 \n 换行。
        /// 模板变量为图例名称 {name}。
        /// [default:null]
        /// </summary>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the style of text.
        /// 文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Data array of legend. An array item is usually a name representing string. (If it is a pie chart, 
        /// it could also be the name of a single data in the pie chart) of a series. 
        /// If data is not specified, it will be auto collected from series. 
        /// 图例的数据数组。数组项通常为一个字符串，每一项代表一个系列的 name（如果是饼图，也可以是饼图单个数据的 name）。
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
        public int index { get; internal set; }
        /// <summary>
        /// 图表是否需要刷新（图例组件不需要刷新图表）
        /// </summary>
        public override bool vertsDirty { get { return false; } }
        /// <summary>
        /// 组件是否需要刷新
        /// </summary>
        public override bool componentDirty
        {
            get { return m_ComponentDirty || location.componentDirty || textStyle.componentDirty; }
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            location.ClearComponentDirty();
            textStyle.ClearComponentDirty();
        }

        /// <summary>
        /// the button list of legend.
        /// 图例按钮列表。
        /// </summary>
        /// <value></value>
        public Dictionary<string, LegendItem> buttonList { get { return m_DataBtnList; } }
        /// <summary>
        /// 运行时图例的总宽度
        /// </summary>
        public float runtimeWidth { get; internal set; }
        /// <summary>
        /// 运行时图例的总高度
        /// </summary>
        public float runtimeHeight { get; internal set; }
        /// <summary>
        /// 多列时每列的宽度
        /// </summary>
        public Dictionary<int, float> runtimeEachWidth { get { return m_RuntimeEachWidth; } }
        /// <summary>
        /// 单列高度
        /// </summary>
        public float runtimeEachHeight { get; internal set; }

        /// <summary>
        /// 一个在顶部居中显示的默认图例。
        /// </summary>
        public static Legend defaultLegend
        {
            get
            {
                var legend = new Legend
                {
                    m_IconType = Type.Auto,
                    m_Show = false,
                    m_SelectedMode = SelectedMode.Multiple,
                    m_Orient = Orient.Horizonal,
                    m_Location = Location.defaultTop,
                    m_ItemWidth = 25.0f,
                    m_ItemHeight = 12.0f,
                    m_ItemGap = 10f,
                };
                legend.location.top = 35;
                legend.textStyle.offset = new Vector2(2, 0);
                legend.textStyle.fontSize = 0;
                return legend;
            }
        }

        /// <summary>
        /// Clear legend data.
        /// 清空。
        /// </summary>
        public void ClearData()
        {
            m_Data.Clear();
            SetComponentDirty();
        }

        /// <summary>
        /// Whether include in legend data by the specified name.
        /// 是否包括由指定名字的图例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsData(string name)
        {
            return m_Data.Contains(name);
        }

        /// <summary>
        /// Removes the legend with the specified name.
        /// 移除指定名字的图例。
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
        /// 添加图例。
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
        /// 获得指定索引的图例。
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
        /// 获得指定图例的索引。
        /// </summary>
        /// <param name="legendName"></param>
        /// <returns></returns>
        public int GetIndex(string legendName)
        {
            return m_Data.IndexOf(legendName);
        }

        /// <summary>
        /// Remove all legend buttons.
        /// 移除所有图例按钮。
        /// </summary>
        public void RemoveButton()
        {
            m_DataBtnList.Clear();
        }

        /// <summary>
        /// Bind buttons to legends.
        /// 给图例绑定按钮。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="btn"></param>
        /// <param name="total"></param>
        public void SetButton(string name, LegendItem item, int total)
        {
            m_DataBtnList[name] = item;
            int index = m_DataBtnList.Values.Count;
            item.SetIconActive(iconType == Type.Custom);
            item.SetActive(show);
        }

        /// <summary>
        /// Update the legend button color.
        /// 更新图例按钮颜色。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void UpdateButtonColor(string name, Color color)
        {
            if (m_DataBtnList.ContainsKey(name))
            {
                m_DataBtnList[name].SetIconColor(color);
            }
        }

        /// <summary>
        /// Update the text color of legend.
        /// 更新图例文字颜色。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void UpdateContentColor(string name, Color color)
        {
            if (m_DataBtnList.ContainsKey(name))
            {
                m_DataBtnList[name].SetContentColor(color);
            }
        }

        /// <summary>
        /// Gets the legend button for the specified index.
        /// 获得指定索引的图例按钮。
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

        /// <summary>
        /// Callback handling when parameters change.
        /// 参数变更时的回调处理。
        /// </summary>
        public void OnChanged()
        {
            m_Location.OnChanged();
        }

        /// <summary>
        /// 获得图例格式化后的显示内容。
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public string GetFormatterContent(string category)
        {
            if (string.IsNullOrEmpty(m_Formatter))
                return category;
            else
            {
                var content = m_Formatter.Replace("{name}", category);
                content = content.Replace("\\n", "\n");
                content = content.Replace("<br/>", "\n");
                return content;
            }
        }
    }
}
