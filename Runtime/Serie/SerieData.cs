using System;
using System.Collections.Generic;
using UnityEngine;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// A data item of serie.
    /// |系列中的一个数据项。可存储数据名和1-n维个数据。
    /// </summary>
    [System.Serializable]
    public class SerieData : ChildComponent
    {
        public static List<string> extraFieldList = new List<string>()
        {
            "m_Id",
            "m_ParentId",
            "m_Ignore",
            "m_Selected",
            "m_Radius"
        };
        public static Dictionary<Type, string> extraComponentMap = new Dictionary<Type, string>
        { { typeof(ItemStyle), "m_ItemStyles" },
            { typeof(LabelStyle), "m_Labels" },
            { typeof(LabelLine), "m_LabelLines" },
            { typeof(SerieSymbol), "m_Symbols" },
            { typeof(LineStyle), "m_LineStyles" },
            { typeof(AreaStyle), "m_AreaStyles" },
            { typeof(TitleStyle), "m_TitleStyles" },
            { typeof(EmphasisItemStyle), "m_EmphasisItemStyles" },
            { typeof(EmphasisLabelStyle), "m_EmphasisLabels" },
            { typeof(EmphasisLabelLine), "m_EmphasisLabelLines" },
        };

        [SerializeField] private int m_Index;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Id;
        [SerializeField] private string m_ParentId;
        [SerializeField] private bool m_Ignore;
        [SerializeField] private bool m_Selected;
        [SerializeField] private float m_Radius;
        [SerializeField][IgnoreDoc] private List<ItemStyle> m_ItemStyles = new List<ItemStyle>();
        [SerializeField][IgnoreDoc] private List<LabelStyle> m_Labels = new List<LabelStyle>();
        [SerializeField][IgnoreDoc] private List<LabelLine> m_LabelLines = new List<LabelLine>();
        [SerializeField][IgnoreDoc] private List<SerieSymbol> m_Symbols = new List<SerieSymbol>();
        [SerializeField][IgnoreDoc] private List<LineStyle> m_LineStyles = new List<LineStyle>();
        [SerializeField][IgnoreDoc] private List<AreaStyle> m_AreaStyles = new List<AreaStyle>();
        [SerializeField][IgnoreDoc] private List<TitleStyle> m_TitleStyles = new List<TitleStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisItemStyle> m_EmphasisItemStyles = new List<EmphasisItemStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisLabelStyle> m_EmphasisLabels = new List<EmphasisLabelStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisLabelLine> m_EmphasisLabelLines = new List<EmphasisLabelLine>();
        [SerializeField] private List<double> m_Data = new List<double>();

        [NonSerialized] public SerieDataContext context = new SerieDataContext();
        [NonSerialized] public InteractData interact = new InteractData();

        public ChartLabel labelObject { get; set; }
        public ChartLabel titleObject { get; set; }

        private bool m_Show = true;
        /// <summary>
        /// the index of SerieData.
        /// |数据项索引。
        /// </summary>
        public override int index { get { return m_Index; } set { m_Index = value; } }
        /// <summary>
        /// the name of data item.
        /// |数据项名称。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// the id of data.
        /// |数据项的唯一id。唯一id不是必须设置的。
        /// </summary>
        public string id { get { return m_Id; } set { m_Id = value; } }
        /// <summary>
        /// the id of parent SerieData.
        /// |父节点id。父节点id不是必须设置的。
        /// </summary>
        public string parentId { get { return m_ParentId; } set { m_ParentId = value; } }
        /// <summary>
        /// 是否忽略数据。当为 true 时，数据不进行绘制。
        /// </summary>
        public bool ignore
        {
            get { return m_Ignore; }
            set { if (PropertyUtil.SetStruct(ref m_Ignore, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 自定义半径。可用在饼图中自定义某个数据项的半径。
        /// </summary>
        public float radius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// Whether the data item is selected.
        /// |该数据项是否被选中。
        /// </summary>
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        /// <summary>
        /// 数据项图例名称。当数据项名称不为空时，图例名称即为系列名称；反之则为索引index。
        /// </summary>
        /// <value></value>
        public string legendName { get { return string.IsNullOrEmpty(name) ? ChartCached.IntToStr(index) : name; } }

        /// <summary>
        /// 单个数据项的标签设置。
        /// </summary>
        public LabelStyle labelStyle { get { return m_Labels.Count > 0 ? m_Labels[0] : null; } }
        public LabelLine labelLine { get { return m_LabelLines.Count > 0 ? m_LabelLines[0] : null; } }
        /// <summary>
        /// 单个数据项的样式设置。
        /// </summary>
        public ItemStyle itemStyle { get { return m_ItemStyles.Count > 0 ? m_ItemStyles[0] : null; } }
        /// <summary>
        /// 单个数据项的标记设置。
        /// </summary>
        public SerieSymbol symbol { get { return m_Symbols.Count > 0 ? m_Symbols[0] : null; } }
        public LineStyle lineStyle { get { return m_LineStyles.Count > 0 ? m_LineStyles[0] : null; } }
        public AreaStyle areaStyle { get { return m_AreaStyles.Count > 0 ? m_AreaStyles[0] : null; } }
        public TitleStyle titleStyle { get { return m_TitleStyles.Count > 0 ? m_TitleStyles[0] : null; } }
        /// <summary>
        /// 高亮的图形样式
        /// </summary>
        public EmphasisItemStyle emphasisItemStyle { get { return m_EmphasisItemStyles.Count > 0 ? m_EmphasisItemStyles[0] : null; } }
        /// <summary>
        /// 高亮时的标签样式
        /// </summary>
        public EmphasisLabelStyle emphasisLabel { get { return m_EmphasisLabels.Count > 0 ? m_EmphasisLabels[0] : null; } }
        /// <summary>
        /// 高亮时的标签引导线样式
        /// </summary>
        public EmphasisLabelLine emphasisLabelLine { get { return m_EmphasisLabelLines.Count > 0 ? m_EmphasisLabelLines[0] : null; } }

        /// <summary>
        /// An arbitrary dimension data list of data item.
        /// |可指定任意维数的数值列表。
        /// </summary>
        public List<double> data { get { return m_Data; } set { m_Data = value; } }
        /// <summary>
        /// [default:true] Whether the data item is showed.
        /// |该数据项是否要显示。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }

        private List<double> m_PreviousData = new List<double>();
        private List<float> m_DataUpdateTime = new List<float>();
        private List<bool> m_DataUpdateFlag = new List<bool>();
        private List<Vector2> m_PolygonPoints = new List<Vector2>();

        public override bool vertsDirty
        {
            get
            {
                return m_VertsDirty ||
                    (labelLine != null && labelLine.vertsDirty) ||
                    (itemStyle != null && itemStyle.vertsDirty) ||
                    (symbol != null && symbol.vertsDirty) ||
                    (lineStyle != null && lineStyle.vertsDirty) ||
                    (areaStyle != null && areaStyle.vertsDirty) ||
                    (emphasisItemStyle != null && emphasisItemStyle.vertsDirty);
            }
        }
        public override bool componentDirty
        {
            get
            {
                return m_ComponentDirty ||
                    (labelStyle != null && labelStyle.componentDirty) ||
                    (labelLine != null && labelLine.componentDirty) ||
                    (titleStyle != null && titleStyle.componentDirty) ||
                    (emphasisLabel != null && emphasisLabel.componentDirty) ||
                    (emphasisLabelLine != null && emphasisLabelLine.componentDirty);
            }
        }

        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            if (labelLine != null) labelLine.ClearVerticesDirty();
            if (itemStyle != null) itemStyle.ClearVerticesDirty();
            if (lineStyle != null) lineStyle.ClearVerticesDirty();
            if (areaStyle != null) areaStyle.ClearVerticesDirty();
            if (symbol != null) symbol.ClearVerticesDirty();
            if (emphasisItemStyle != null) emphasisItemStyle.ClearVerticesDirty();
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            if (labelLine != null) labelLine.ClearComponentDirty();
            if (itemStyle != null) itemStyle.ClearComponentDirty();
            if (lineStyle != null) lineStyle.ClearComponentDirty();
            if (areaStyle != null) areaStyle.ClearComponentDirty();
            if (symbol != null) symbol.ClearComponentDirty();
            if (emphasisLabel != null) emphasisLabel.ClearComponentDirty();
            if (emphasisLabelLine != null) emphasisLabelLine.ClearComponentDirty();
        }

        public void Reset()
        {
            index = 0;
            m_Id = null;
            m_ParentId = null;
            labelObject = null;
            m_Name = string.Empty;
            m_Show = true;
            context.Reset();
            interact.Reset();
            m_Data.Clear();
            m_PreviousData.Clear();
            m_DataUpdateTime.Clear();
            m_DataUpdateFlag.Clear();
            m_Labels.Clear();
            m_LabelLines.Clear();
            m_ItemStyles.Clear();
            m_Symbols.Clear();
            m_LineStyles.Clear();
            m_AreaStyles.Clear();
            m_TitleStyles.Clear();
            m_EmphasisItemStyles.Clear();
            m_EmphasisLabels.Clear();
            m_EmphasisLabelLines.Clear();
        }

        public T GetOrAddComponent<T>() where T : ChildComponent, ISerieDataComponent
        {
            return GetOrAddComponent(typeof(T)) as T;
        }

        public ISerieDataComponent GetOrAddComponent(Type type)
        {
            if (type == typeof(ItemStyle))
            {
                if (m_ItemStyles.Count == 0)
                    m_ItemStyles.Add(new ItemStyle() { show = true });
                return m_ItemStyles[0];
            }
            else if (type == typeof(LabelStyle))
            {
                if (m_Labels.Count == 0)
                    m_Labels.Add(new LabelStyle() { show = true });
                return m_Labels[0];
            }
            else if (type == typeof(LabelLine))
            {
                if (m_LabelLines.Count == 0)
                    m_LabelLines.Add(new LabelLine() { show = true });
                return m_LabelLines[0];
            }
            else if (type == typeof(EmphasisItemStyle))
            {
                if (m_EmphasisItemStyles.Count == 0)
                    m_EmphasisItemStyles.Add(new EmphasisItemStyle() { show = true });
                return m_EmphasisItemStyles[0];
            }
            else if (type == typeof(EmphasisLabelStyle))
            {
                if (m_EmphasisLabels.Count == 0)
                    m_EmphasisLabels.Add(new EmphasisLabelStyle() { show = true });
                return m_EmphasisLabels[0];
            }
            else if (type == typeof(EmphasisLabelLine))
            {
                if (m_EmphasisLabelLines.Count == 0)
                    m_EmphasisLabelLines.Add(new EmphasisLabelLine() { show = true });
                return m_EmphasisLabelLines[0];
            }
            else if (type == typeof(SerieSymbol))
            {
                if (m_Symbols.Count == 0)
                    m_Symbols.Add(new SerieSymbol() { show = true });
                return m_Symbols[0];
            }
            else if (type == typeof(LineStyle))
            {
                if (m_LineStyles.Count == 0)
                    m_LineStyles.Add(new LineStyle() { show = true });
                return m_LineStyles[0];
            }
            else if (type == typeof(AreaStyle))
            {
                if (m_AreaStyles.Count == 0)
                    m_AreaStyles.Add(new AreaStyle() { show = true });
                return m_AreaStyles[0];
            }
            else if (type == typeof(TitleStyle))
            {
                if (m_TitleStyles.Count == 0)
                    m_TitleStyles.Add(new TitleStyle() { show = true });
                return m_TitleStyles[0];
            }
            else
            {
                throw new System.Exception("SerieData not support component:" + type);
            }
        }

        public void RemoveAllComponent()
        {
            m_ItemStyles.Clear();
            m_Labels.Clear();
            m_LabelLines.Clear();
            m_Symbols.Clear();
            m_EmphasisItemStyles.Clear();
            m_EmphasisLabels.Clear();
            m_EmphasisLabelLines.Clear();
            m_LineStyles.Clear();
            m_AreaStyles.Clear();
            m_TitleStyles.Clear();
        }

        public void RemoveComponent<T>() where T : ISerieDataComponent
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            if (type == typeof(ItemStyle))
                m_ItemStyles.Clear();
            else if (type == typeof(LabelStyle))
                m_Labels.Clear();
            else if (type == typeof(LabelLine))
                m_LabelLines.Clear();
            else if (type == typeof(EmphasisItemStyle))
                m_EmphasisItemStyles.Clear();
            else if (type == typeof(EmphasisLabelStyle))
                m_EmphasisLabels.Clear();
            else if (type == typeof(EmphasisLabelLine))
                m_EmphasisLabelLines.Clear();
            else if (type == typeof(SerieSymbol))
                m_Symbols.Clear();
            else if (type == typeof(LineStyle))
                m_LineStyles.Clear();
            else if (type == typeof(AreaStyle))
                m_AreaStyles.Clear();
            else if (type == typeof(TitleStyle))
                m_TitleStyles.Clear();
            else
                throw new System.Exception("SerieData not support component:" + type);
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
        /// |最大值。
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
        /// |最小值。
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

        public double GetTotalData()
        {
            var total = 0d;
            foreach (var value in m_Data)
                total += value;
            return total;
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
            for (int i = 0; i < m_DataUpdateFlag.Count; i++)
                if (m_DataUpdateFlag[i]) return true;
            return false;
        }

        public float GetLabelWidth()
        {
            if (labelObject != null) return labelObject.GetTextWidth();
            else return 0;
        }

        public float GetLabelHeight()
        {
            if (labelObject != null) return labelObject.GetTextHeight();
            return 0;
        }

        public void SetLabelActive(bool flag)
        {
            if (labelObject != null) labelObject.SetActive(flag);
        }
        public void SetIconActive(bool flag)
        {
            if (labelObject != null) labelObject.SetActive(flag);
        }

        public void SetPolygon(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            m_PolygonPoints.Clear();
            m_PolygonPoints.Add(p1);
            m_PolygonPoints.Add(p2);
            m_PolygonPoints.Add(p3);
            m_PolygonPoints.Add(p4);
        }
        public void SetPolygon(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5)
        {
            SetPolygon(p1, p2, p3, p4);
            m_PolygonPoints.Add(p5);
        }

        public bool IsInPolygon(Vector2 p)
        {
            return UGLHelper.IsPointInPolygon(p, m_PolygonPoints);
        }
    }
}