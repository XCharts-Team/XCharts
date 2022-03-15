/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// A data item of serie.
    /// 系列中的一个数据项。可存储数据名和1-n维的数据。
    /// </summary>
    [System.Serializable]
    public class SerieData : SubComponent
    {
        [SerializeField] private int m_Index;
        [SerializeField] private int m_ParentIndex = -1;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Id;
        [SerializeField] private bool m_Selected;
        [SerializeField] private bool m_Ignore = false;
        [SerializeField] private float m_Radius;
        [SerializeField] private bool m_EnableIconStyle = false;
        [SerializeField] private IconStyle m_IconStyle = new IconStyle();
        [SerializeField] private bool m_EnableLabel = false;
        [SerializeField] private SerieLabel m_Label = new SerieLabel();
        [SerializeField] private bool m_EnableItemStyle = false;
        [SerializeField] private ItemStyle m_ItemStyle = new ItemStyle();
        [SerializeField] private bool m_EnableEmphasis = false;
        [SerializeField] private Emphasis m_Emphasis = new Emphasis();
        [SerializeField] private bool m_EnableSymbol = false;
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();
        [SerializeField] private List<double> m_Data = new List<double>();
        [SerializeField] private List<int> m_Children = new List<int>();

        public ChartLabel labelObject { get; set; }

        private bool m_Show = true;
        private float m_RtPieOutsideRadius;

        public int index { get { return m_Index; } set { m_Index = value; } }
        public int parentIndex { get { return m_ParentIndex; } set { m_ParentIndex = value; } }
        /// <summary>
        /// the name of data item.
        /// 数据项名称。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// 数据项的唯一id。唯一id不是必须设置的。
        /// </summary>
        public string id { get { return m_Id; } set { m_Id = value; } }
        /// <summary>
        /// 数据项图例名称。当数据项名称不为空时，图例名称即为系列名称；反之则为索引index。
        /// </summary>
        /// <value></value>
        public string legendName { get { return string.IsNullOrEmpty(name) ? ChartCached.IntToStr(index) : name; } }
        /// <summary>
        /// 自定义半径。可用在饼图中自定义某个数据项的半径。
        /// </summary>
        public float radius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// Whether the data item is selected.
        /// 该数据项是否被选中。
        /// </summary>
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        /// <summary>
        /// 是否启用单个数据项的图标设置。
        /// </summary>
        public bool enableIconStyle { get { return m_EnableIconStyle; } set { m_EnableIconStyle = value; } }
        /// <summary>
        /// the icon of data.
        /// 数据项图标样式。
        /// </summary>
        public IconStyle iconStyle { get { return m_IconStyle; } set { m_IconStyle = value; } }
        /// <summary>
        /// 是否启用单个数据项的标签设置。
        /// </summary>
        public bool enableLabel { get { return m_EnableLabel; } set { m_EnableLabel = value; } }
        /// <summary>
        /// 单个数据项的标签设置。
        /// </summary>
        public SerieLabel label { get { return m_Label; } set { m_Label = value; } }
        /// <summary>
        /// 是否启用单个数据项的样式。
        /// </summary>
        public bool enableItemStyle { get { return m_EnableItemStyle; } set { m_EnableItemStyle = value; } }
        /// <summary>
        /// 单个数据项的样式设置。
        /// </summary>
        public ItemStyle itemStyle { get { return m_ItemStyle; } set { m_ItemStyle = value; } }
        /// <summary>
        /// 是否启用单个数据项的高亮样式。
        /// </summary>
        public bool enableEmphasis { get { return m_EnableEmphasis; } set { m_EnableEmphasis = value; } }
        /// <summary>
        /// 单个数据项的高亮样式设置。
        /// </summary>
        public Emphasis emphasis { get { return m_Emphasis; } set { m_Emphasis = value; } }
        /// <summary>
        /// 是否启用单个数据项的标记设置。
        /// </summary>
        public bool enableSymbol { get { return m_EnableSymbol; } set { m_EnableSymbol = value; } }
        /// <summary>
        /// 单个数据项的标记设置。
        /// </summary>
        public SerieSymbol symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        /// <summary>
        /// 是否忽略数据。当为 true 时，数据不进行绘制。
        /// </summary>
        public bool ignore
        {
            get { return m_Ignore; }
            set { if (PropertyUtil.SetStruct(ref m_Ignore, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// An arbitrary dimension data list of data item.
        /// 可指定任意维数的数值列表。
        /// </summary>
        public List<double> data { get { return m_Data; } set { m_Data = value; } }
        public List<int> children { get { return m_Children; } set { m_Children = value; } }
        /// <summary>
        /// [default:true] Whether the data item is showed.
        /// 该数据项是否要显示。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Whether the data item is highlighted.
        /// 该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlighted { get; set; }
        public Vector3 labelPosition { get; set; }
        private bool m_CanShowLabel = true;
        /// <summary>
        /// 是否可以显示Label
        /// </summary>
        public bool canShowLabel { get { return m_CanShowLabel; } set { m_CanShowLabel = value; } }

        /// <summary>
        /// 饼图数据项的开始角度（运行时自动计算）
        /// </summary>
        public float runtimePieStartAngle { get; set; }
        /// <summary>
        /// 饼图数据项的结束角度（运行时自动计算）
        /// </summary>
        public float runtimePieToAngle { get; set; }
        /// <summary>
        /// 饼图数据项的一半时的角度（运行时自动计算）
        /// </summary>
        public float runtimePieHalfAngle { get; set; }
        /// <summary>
        /// 饼图数据项的当前角度（运行时自动计算）
        /// </summary>
        public float runtimePieCurrAngle { get; set; }
        /// <summary>
        /// 饼图数据项的内半径
        /// </summary>
        public float runtimePieInsideRadius { get; set; }
        /// <summary>
        /// 饼图数据项的外半径
        /// </summary>
        public float runtimePieOutsideRadius
        {
            get
            {
                if (radius > 0) return radius;
                else return m_RtPieOutsideRadius;
            }
            set
            {
                m_RtPieOutsideRadius = value;
            }
        }
        /// <summary>
        /// 饼图数据项的偏移半径
        /// </summary>
        public float runtimePieOffsetRadius { get; set; }
        public Vector3 runtimePosition { get; set; }
        /// <summary>
        /// 绘制区域。
        /// </summary>
        public Rect runtimeRect { get; set; }
        public Rect runtimeSubRect { get; set; }
        public int runtimeLevel { get; set; }
        public SerieData runtimeParent { get; set; }
        public Color32 runtimeColor { get; set; }
        public double runtimeArea { get; set; }
        public float runtimeAngle { get; set; }
        public Vector3 runtiemPieOffsetCenter { get; set; }
        public float runtimeStackHig { get; set; }
        public Image runtimeSymbol { get; set; }
        public List<SerieData> runtimeChildren { get { return m_RuntimeChildren; } }

        private List<double> m_PreviousData = new List<double>();
        private List<float> m_DataUpdateTime = new List<float>();
        private List<bool> m_DataUpdateFlag = new List<bool>();
        private List<Vector2> m_PolygonPoints = new List<Vector2>();
        [System.NonSerialized]
        private List<SerieData> m_RuntimeChildren = new List<SerieData>();

        public void Reset()
        {
            index = 0;
            m_ParentIndex = -1;
            labelObject = null;
            highlighted = false;
            m_Name = string.Empty;
            m_Show = true;
            m_Selected = false;
            m_CanShowLabel = true;
            m_EnableIconStyle = false;
            m_EnableSymbol = false;
            m_EnableLabel = false;
            m_EnableEmphasis = false;
            m_EnableItemStyle = false;
            m_Radius = 0;
            m_Data.Clear();
            m_PreviousData.Clear();
            m_PolygonPoints.Clear();
            m_RuntimeChildren.Clear();
            m_DataUpdateTime.Clear();
            m_DataUpdateFlag.Clear();
            m_IconStyle.Reset();
            m_Label.Reset();
            m_ItemStyle.Reset();
            m_Emphasis.Reset();
        }

        public double GetData(int index, bool inverse = false)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                return inverse ? -m_Data[index] : m_Data[index];
            }
            else return 0;
        }

        public double GetData(int index, double min, double max)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                var value = m_Data[index];
                if (value < min) return min;
                else if (value > max) return max;
                else return value;
            }
            else return 0;
        }

        public double GetPreviousData(int index, bool inverse = false)
        {
            if (index >= 0 && index < m_PreviousData.Count)
            {
                return inverse ? -m_PreviousData[index] : m_PreviousData[index];
            }
            else return 0;
        }

        public double GetFirstData(float animationDuration = 500f)
        {
            if (m_Data.Count > 0) return GetCurrData(0, animationDuration);
            return 0;
        }

        public double GetLastData()
        {
            if (m_Data.Count > 0) return m_Data[m_Data.Count - 1];
            return 0;
        }

        public double GetCurrData(int index, float animationDuration = 500f, bool inverse = false)
        {
            return GetCurrData(index, animationDuration, inverse, 0, 0);
        }

        public double GetCurrData(int index, float animationDuration, bool inverse, double min, double max)
        {
            if (index < m_DataUpdateFlag.Count && m_DataUpdateFlag[index] && animationDuration > 0)
            {
                var time = Time.time - m_DataUpdateTime[index];
                var total = animationDuration / 1000;

                var rate = time / total;
                if (rate > 1) rate = 1;
                if (rate < 1)
                {
                    CheckLastData();
                    var curr = MathUtil.Lerp(GetPreviousData(index), GetData(index), rate);
                    if (min != 0 || max != 0)
                    {
                        if (inverse)
                        {
                            var temp = min;
                            min = -max;
                            max = -temp;
                        }
                        var pre = m_PreviousData[index];
                        if (pre < min)
                        {
                            m_PreviousData[index] = min;
                            curr = min;
                        }
                        else if (pre > max)
                        {
                            m_PreviousData[index] = max;
                            curr = max;
                        }
                    }
                    curr = inverse ? -curr : curr;
                    return curr;
                }
                else
                {
                    m_DataUpdateFlag[index] = false;
                    return GetData(index, inverse);
                }
            }
            else
            {
                return GetData(index, inverse);
            }
        }

        /// <summary>
        /// the maxinum value.
        /// 最大值。
        /// </summary>
        public double GetMaxData(bool inverse = false)
        {
            if (m_Data.Count == 0) return 0;
            var temp = double.MinValue;
            for (int i = 0; i < m_Data.Count; i++)
            {
                var value = GetData(i, inverse);
                if (value > temp) temp = value;
            }
            return temp;
        }

        /// <summary>
        /// the mininum value.
        /// 最小值。
        /// </summary>
        public double GetMinData(bool inverse = false)
        {
            if (m_Data.Count == 0) return 0;
            var temp = double.MaxValue;
            for (int i = 0; i < m_Data.Count; i++)
            {
                var value = GetData(i, inverse);
                if (value < temp) temp = value;
            }
            return temp;
        }

        public bool UpdateData(int dimension, double value, bool updateAnimation, float animationDuration = 500f)
        {
            if (dimension >= 0 && dimension < data.Count)
            {
                CheckLastData();
                m_PreviousData[dimension] = GetCurrData(dimension, animationDuration);
                //m_PreviousData[dimension] = data[dimension];;
                m_DataUpdateTime[dimension] = Time.time;
                m_DataUpdateFlag[dimension] = updateAnimation;
                data[dimension] = value;
                return true;
            }
            return false;
        }

        public bool UpdateData(int dimension, double value)
        {
            if (dimension >= 0 && dimension < data.Count)
            {
                data[dimension] = value;
                return true;
            }
            return false;
        }

        private void CheckLastData()
        {
            if (m_PreviousData.Count != m_Data.Count)
            {
                m_PreviousData.Clear();
                m_DataUpdateTime.Clear();
                m_DataUpdateFlag.Clear();
                for (int i = 0; i < m_Data.Count; i++)
                {
                    m_PreviousData.Add(m_Data[i]);
                    m_DataUpdateTime.Add(Time.time);
                    m_DataUpdateFlag.Add(false);
                }
            }
        }

        public bool IsDataChanged()
        {
            foreach (var b in m_DataUpdateFlag)
                if (b) return true;
            return false;
        }

        public float GetLabelWidth()
        {
            if (labelObject != null) return labelObject.GetLabelWidth();
            else return 0;
        }

        public float GetLabelHeight()
        {
            if (labelObject != null) return labelObject.GetLabelHeight();
            return 0;
        }

        public void SetLabelActive(bool flag)
        {
            if (labelObject != null) labelObject.SetLabelActive(flag);
        }
        public void SetIconActive(bool flag)
        {
            if (labelObject != null) labelObject.SetIconActive(flag);
        }

        public void SetPolygon(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            m_PolygonPoints.Clear();
            m_PolygonPoints.Add(p1);
            m_PolygonPoints.Add(p2);
            m_PolygonPoints.Add(p3);
            m_PolygonPoints.Add(p4);
        }

        public bool IsInPolygon(Vector2 p)
        {
            if (m_PolygonPoints.Count == 0) return false;
            var inside = false;
            var j = m_PolygonPoints.Count - 1;
            for (int i = 0; i < m_PolygonPoints.Count; j = i++)
            {
                var pi = m_PolygonPoints[i];
                var pj = m_PolygonPoints[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }
    }
}
