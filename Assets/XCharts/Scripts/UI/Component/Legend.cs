using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// 图例组件。
    /// 图例组件展现了不同系列的标记，颜色和名字。可以通过点击图例控制哪些系列不显示。
    /// </summary>
    [System.Serializable]
    public class Legend : JsonDataSupport, IPropertyChanged, IEquatable<Legend>
    {
        /// <summary>
        /// Selected mode of legend, which controls whether series can be toggled displaying by clicking legends. 
        /// It is enabled by default, and you may set it to be false to disabled it.
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
        [SerializeField] private SelectedMode m_SelectedMode;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private Location m_Location = Location.defaultRight;
        [SerializeField] private float m_ItemWidth = 50.0f;
        [SerializeField] private float m_ItemHeight = 20.0f;
        [SerializeField] private float m_ItemGap = 5;
        [SerializeField] private int m_ItemFontSize = 18;
        [SerializeField] private List<string> m_Data = new List<string>();

        private Dictionary<string, Button> m_DataBtnList = new Dictionary<string, Button>();

        /// <summary>
        /// Whether to show legend component. 
        /// 是否显示图例组件。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Selected mode of legend, which controls whether series can be toggled displaying by clicking legends. 
        /// 选择模式。控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。
        /// </summary>
        /// <value></value>
        public SelectedMode selectedMode { get { return m_SelectedMode; } set { m_SelectedMode = value; } }
        /// <summary>
        /// Specify whether the layout of legend component is horizontal or vertical. 
        /// 布局方式是横还是竖。
        /// </summary>
        public Orient orient { get { return m_Orient; } set { m_Orient = value; } }
        /// <summary>
        /// The location of legend.
        /// 图例显示的位置。
        /// </summary>
        public Location location { get { return m_Location; } set { m_Location = value; } }
        /// <summary>
        /// the width of legend item.
        /// 每个图例项的宽度。
        /// </summary>
        public float itemWidth { get { return m_ItemWidth; } set { m_ItemWidth = value; } }
        /// <summary>
        /// the height of legend item.
        /// 每个图例项的高度。
        /// </summary>
        public float itemHeight { get { return m_ItemHeight; } set { m_ItemHeight = value; } }
        /// <summary>
        /// The distance between each legend, horizontal distance in horizontal layout, and vertical distance in vertical layout.
        /// 图例每项之间的间隔。横向布局时为水平间隔，纵向布局时为纵向间隔。
        /// </summary>
        public float itemGap { get { return m_ItemGap; } set { m_ItemGap = value; } }
        /// <summary>
        /// font size of item text.
        /// 图例项的字体大小。
        /// </summary>
        public int itemFontSize { get { return m_ItemFontSize; } set { m_ItemFontSize = value; } }
        /// <summary>
        /// Data array of legend. An array item is usually a name representing string. (If it is a pie chart, 
        /// it could also be the name of a single data in the pie chart) of a series. 
        /// If data is not specified, it will be auto collected from series. 
        /// 图例的数据数组。数组项通常为一个字符串，每一项代表一个系列的 name（如果是饼图，也可以是饼图单个数据的 name）。
        /// 如果 data 没有被指定，会自动从当前系列中获取。指定data时里面的数据项和serie匹配时才会生效。
        /// </summary>
        public List<string> data { get { return m_Data; } }
        /// <summary>
        /// the button list of legend.
        /// 图例按钮列表。
        /// </summary>
        /// <value></value>
        public Dictionary<string, Button> buttonList { get { return m_DataBtnList; } }

        /// <summary>
        /// 一个在顶部居中显示的默认图例。
        /// </summary>
        public static Legend defaultLegend
        {
            get
            {
                var legend = new Legend
                {
                    m_Show = false,
                    m_SelectedMode = SelectedMode.Multiple,
                    m_Orient = Orient.Horizonal,
                    m_Location = Location.defaultTop,
                    m_ItemWidth = 60.0f,
                    m_ItemHeight = 20.0f,
                    m_ItemGap = 5,
                    m_ItemFontSize = 16
                };
                legend.location.top = 30;
                return legend;
            }
        }
        public void Copy(Legend legend)
        {
            m_Show = legend.show;
            m_SelectedMode = legend.selectedMode;
            m_Orient = legend.orient;
            m_Location.Copy(legend.location);
            m_ItemWidth = legend.itemWidth;
            m_ItemHeight = legend.itemHeight;
            m_ItemGap = legend.itemGap;
            m_ItemFontSize = legend.itemFontSize;
            m_Data.Clear();
            foreach (var d in legend.data) m_Data.Add(d);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Legend)
            {
                return Equals((Legend)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Legend other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return show == other.show &&
                selectedMode == other.selectedMode &&
                orient == other.orient &&
                location == other.location &&
                itemWidth == other.itemWidth &&
                itemHeight == other.itemHeight &&
                itemGap == other.itemGap &&
                itemFontSize == other.itemFontSize &&
                ChartHelper.IsValueEqualsList<string>(m_Data, other.data);
        }

        public static bool operator ==(Legend left, Legend right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }
            return Equals(left, right);
        }

        public static bool operator !=(Legend left, Legend right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void ClearData()
        {
            m_Data.Clear();
        }

        /// <summary>
        /// 是否包括由指定名字的图例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsData(string name)
        {
            return m_Data.Contains(name);
        }

        /// <summary>
        /// 移除指定名字的图例
        /// </summary>
        /// <param name="name"></param>
        public void RemoveData(string name)
        {
            if (m_Data.Contains(name))
            {
                m_Data.Remove(name);
            }
        }

        /// <summary>
        /// 添加图例项
        /// </summary>
        /// <param name="name"></param>
        public void AddData(string name)
        {
            if (!m_Data.Contains(name) && !string.IsNullOrEmpty(name))
            {
                m_Data.Add(name);
            }
        }

        /// <summary>
        /// 获得指定索引的图例
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
        /// 获得指定图例的索引
        /// </summary>
        /// <param name="legendName"></param>
        /// <returns></returns>
        public int GetIndex(string legendName)
        {
            return m_Data.IndexOf(legendName);
        }

        /// <summary>
        /// 移除所有图例按钮
        /// </summary>
        public void RemoveButton()
        {
            m_DataBtnList.Clear();
        }

        /// <summary>
        /// 给图例绑定按钮
        /// </summary>
        /// <param name="name"></param>
        /// <param name="btn"></param>
        /// <param name="total"></param>
        public void SetButton(string name, Button btn, int total)
        {
            int index = m_DataBtnList.Values.Count;
            btn.transform.localPosition = GetButtonLocationPosition(total, index);
            m_DataBtnList[name] = btn;
            btn.gameObject.SetActive(show);
            btn.GetComponentInChildren<Text>().text = name;
        }

        /// <summary>
        /// 更新图例按钮颜色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void UpdateButtonColor(string name, Color color)
        {
            if (m_DataBtnList.ContainsKey(name))
            {
                m_DataBtnList[name].GetComponent<Image>().color = color;
            }
        }

        /// <summary>
        /// 参数变更时的回调处理
        /// </summary>
        public void OnChanged()
        {
            m_Location.OnChanged();
        }

        /// <summary>
        /// 根据图例的布局和位置类型获得具体位置
        /// </summary>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 GetButtonLocationPosition(int size, int index)
        {
            switch (m_Orient)
            {
                case Orient.Vertical:
                    switch (m_Location.align)
                    {
                        case Location.Align.TopCenter:
                        case Location.Align.TopLeft:
                        case Location.Align.TopRight:
                            return new Vector2(0, -index * (itemHeight + itemGap));

                        case Location.Align.Center:
                        case Location.Align.CenterLeft:
                        case Location.Align.CenterRight:
                            float totalHeight = size * itemHeight + (size - 1) * itemGap;
                            float startY = totalHeight / 2;
                            return new Vector2(0, startY - index * (itemHeight + itemGap));

                        case Location.Align.BottomCenter:
                        case Location.Align.BottomLeft:
                        case Location.Align.BottomRight:
                            return new Vector2(0, (size - index - 1) * (itemHeight + itemGap));
                    }
                    return Vector2.zero;

                case Orient.Horizonal:
                    switch (m_Location.align)
                    {
                        case Location.Align.TopLeft:
                        case Location.Align.CenterLeft:
                        case Location.Align.BottomLeft:
                            return new Vector2(index * (itemWidth + itemGap), 0);

                        case Location.Align.TopCenter:
                        case Location.Align.Center:
                        case Location.Align.BottomCenter:
                            float totalWidth = size * itemWidth + (size - 1) * itemGap;
                            float startX = totalWidth / 2;
                            return new Vector2(-startX + itemWidth / 2 + index * (itemWidth + itemGap), 0);
                        case Location.Align.TopRight:
                        case Location.Align.CenterRight:
                        case Location.Align.BottomRight:
                            return new Vector2(-(size - index - 1) * (itemWidth + itemGap), 0);
                    }
                    return Vector2.zero;
            }
            return Vector2.zero;
        }

        /// <summary>
        /// 从json字符串解析数据，json格式如：['邮件营销','联盟广告','视频广告','直接访问','搜索引擎']
        /// </summary>
        /// <param name="jsonData"></param>
        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            m_Data = ChartHelper.ParseStringFromString(jsonData);
        }
    }
}
