using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// The way to get serie symbol size.
    /// |获取标记图形大小的方式。
    /// </summary>
    public enum SymbolSizeType
    {
        /// <summary>
        /// Specify constant for symbol size.
        /// |自定义大小。
        /// </summary>
        Custom,
        /// <summary>
        /// Specify the dataIndex and dataScale to calculate symbol size.
        /// |通过 dataIndex 从数据中获取，再乘以一个比例系数 dataScale 。
        /// </summary>
        FromData,
        /// <summary>
        /// Specify function for symbol size.
        /// |通过委托函数获取。
        /// </summary>
        Function,
    }

    /// <summary>
    /// 系列数据项的标记的图形
    /// </summary>
    [System.Serializable]
    public class SerieSymbol : SymbolStyle, ISerieDataComponent
    {
        [SerializeField] private SymbolSizeType m_SizeType = SymbolSizeType.Custom;
        [SerializeField] private int m_DataIndex = 1;
        [SerializeField] private float m_DataScale = 1;
        [SerializeField] private SymbolSizeFunction m_SizeFunction;
        [SerializeField] private int m_StartIndex;
        [SerializeField] private int m_Interval;
        [SerializeField] private bool m_ForceShowLast = false;
        [SerializeField] private bool m_Repeat = false;

        public override void Reset()
        {
            base.Reset();
            m_SizeType = SymbolSizeType.Custom;
            m_DataIndex = 1;
            m_DataScale = 1;
            m_SizeFunction = null;
            m_StartIndex = 0;
            m_Interval = 0;
            m_ForceShowLast = false;
            m_Repeat = false;
        }

        /// <summary>
        /// the type of symbol size.
        /// |标记图形的大小获取方式。
        /// </summary>
        public SymbolSizeType sizeType
        {
            get { return m_SizeType; }
            set { if (PropertyUtil.SetStruct(ref m_SizeType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// whitch data index is when the sizeType assined as FromData.
        /// |当sizeType指定为FromData时，指定的数据源索引。
        /// </summary>
        public int dataIndex
        {
            get { return m_DataIndex; }
            set { if (PropertyUtil.SetStruct(ref m_DataIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the scale of data when sizeType assined as FromData.
        /// |当sizeType指定为FromData时，指定的倍数系数。
        /// </summary>
        public float dataScale
        {
            get { return m_DataScale; }
            set { if (PropertyUtil.SetStruct(ref m_DataScale, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the function of size when sizeType assined as Function.
        /// |当sizeType指定为Function时，指定的委托函数。
        /// </summary>
        public SymbolSizeFunction sizeFunction
        {
            get { return m_SizeFunction; }
            set { if (PropertyUtil.SetClass(ref m_SizeFunction, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the index start to show symbol.
        /// |开始显示图形标记的索引。
        /// </summary>
        public int startIndex
        {
            get { return m_StartIndex; }
            set { if (PropertyUtil.SetStruct(ref m_StartIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the interval of show symbol.
        /// |显示图形标记的间隔。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。
        /// </summary>
        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// whether to show the last symbol.
        /// |是否强制显示最后一个图形标记。
        /// </summary>
        public bool forceShowLast
        {
            get { return m_ForceShowLast; }
            set { if (PropertyUtil.SetStruct(ref m_ForceShowLast, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 图形是否重复。
        /// </summary>
        public bool repeat
        {
            get { return m_Repeat; }
            set { if (PropertyUtil.SetStruct(ref m_Repeat, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 根据指定的sizeType获得标记的大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float GetSize(List<double> data, float themeSize)
        {
            switch (m_SizeType)
            {
                case SymbolSizeType.Custom:
                    return size == 0 ? themeSize : size;
                case SymbolSizeType.FromData:
                    if (data != null && dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return (float) data[dataIndex] * m_DataScale;
                    }
                    else
                    {
                        return size == 0 ? themeSize : size;
                    }
                case SymbolSizeType.Function:
                    if (data != null && sizeFunction != null) return sizeFunction(data);
                    else return size == 0 ? themeSize : size;
                default:
                    return size == 0 ? themeSize : size;
            }
        }

        public bool ShowSymbol(int dataIndex, int dataCount)
        {
            if (!show)
                return false;

            if (dataIndex < startIndex)
                return false;

            if (m_Interval <= 0)
                return true;

            if (m_ForceShowLast && dataIndex == dataCount - 1)
                return true;

            return (dataIndex - startIndex) % (m_Interval + 1) == 0;
        }
    }
}