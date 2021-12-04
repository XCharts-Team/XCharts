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
    /// the type of symbol.
    /// 标记图形的类型。
    /// </summary>
    public enum SerieSymbolType
    {
        /// <summary>
        /// 空心圆。
        /// </summary>
        EmptyCircle,
        /// <summary>
        /// 圆形。
        /// </summary>
        Circle,
        /// <summary>
        /// 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。
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
        /// 不显示标记。
        /// </summary>
        None,
        /// <summary>
        /// 箭头。
        /// </summary>
        Arrow,
        /// <summary>
        /// 自定义标记。
        /// </summary>
        Custom,
        EmptyRect,
        EmptyTriangle,
        EmptyDiamond
    }

    /// <summary>
    /// The way to get serie symbol size.
    /// 获取标记图形大小的方式。
    /// </summary>
    public enum SerieSymbolSizeType
    {
        /// <summary>
        /// Specify constant for symbol size.
        /// 自定义大小。
        /// </summary>
        Custom,
        /// <summary>
        /// Specify the dataIndex and dataScale to calculate symbol size.
        /// 通过 dataIndex 从数据中获取，再乘以一个比例系数 dataScale 。
        /// </summary>
        FromData,
        /// <summary>
        /// Specify callback function for symbol size.
        /// 通过回调函数获取。
        /// </summary>
        Callback,
    }

    /// <summary>
    /// 获取标记大小的回调。
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public delegate float SymbolSizeCallback(List<double> data);

    /// <summary>
    /// 系列数据项的标记的图形
    /// </summary>
    [System.Serializable]
    public class SerieSymbol : SubComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private SerieSymbolType m_Type = SerieSymbolType.EmptyCircle;
        [SerializeField] private SerieSymbolSizeType m_SizeType = SerieSymbolSizeType.Custom;
        [SerializeField] private float m_Size = 0f;
        [SerializeField] private float m_SelectedSize = 0f;
        [SerializeField] private int m_DataIndex = 1;
        [SerializeField] private float m_DataScale = 1;
        [SerializeField] private float m_SelectedDataScale = 1.5f;
        [SerializeField] private SymbolSizeCallback m_SizeCallback;
        [SerializeField] private SymbolSizeCallback m_SelectedSizeCallback;
        [SerializeField] private int m_StartIndex;
        [SerializeField] private int m_Interval;
        [SerializeField] private bool m_ForceShowLast = false;
        [SerializeField] private float m_Gap = 0;
        [SerializeField] private float m_Width = 0f;
        [SerializeField] private float m_Height = 0f;
        [SerializeField] private bool m_Repeat = false;
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        [SerializeField] private Sprite m_Image;
        [SerializeField] private Image.Type m_ImageType;

        public void Reset()
        {
            m_Show = false;
            m_Type = SerieSymbolType.EmptyCircle;
            m_SizeType = SerieSymbolSizeType.Custom;
            m_Size = 0f;
            m_SelectedSize = 0f;
            m_DataIndex = 1;
            m_DataScale = 1;
            m_SelectedDataScale = 1.5f;
            m_SizeCallback = null;
            m_SelectedSizeCallback = null;
            m_StartIndex = 0;
            m_Interval = 0;
            m_ForceShowLast = false;
            m_Gap = 0;
            m_Width = 0f;
            m_Height = 0f;
            m_Repeat = false;
            m_Offset = Vector2.zero;
            m_Image = null;
            m_ImageType = Image.Type.Simple;
        }

        /// <summary>
        /// Whether the symbol is showed.
        /// 是否显示标记。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of symbol.
        /// 标记类型。
        /// </summary>
        public SerieSymbolType type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the type of symbol size.
        /// 标记图形的大小获取方式。
        /// </summary>
        public SerieSymbolSizeType sizeType
        {
            get { return m_SizeType; }
            set { if (PropertyUtil.SetStruct(ref m_SizeType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the size of symbol.
        /// 标记的大小。
        /// </summary>
        public float size
        {
            get { return m_Size; }
            set { if (PropertyUtil.SetStruct(ref m_Size, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the size of selected symbol.
        /// 被选中的标记的大小。
        /// </summary>
        public float selectedSize
        {
            get { return m_SelectedSize; }
            set { if (PropertyUtil.SetStruct(ref m_SelectedSize, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// whitch data index is when the sizeType assined as FromData.
        /// 当sizeType指定为FromData时，指定的数据源索引。
        /// </summary>
        public int dataIndex
        {
            get { return m_DataIndex; }
            set { if (PropertyUtil.SetStruct(ref m_DataIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the scale of data when sizeType assined as FromData.
        /// 当sizeType指定为FromData时，指定的倍数系数。
        /// </summary>
        public float dataScale
        {
            get { return m_DataScale; }
            set { if (PropertyUtil.SetStruct(ref m_DataScale, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the scale of selected data when sizeType assined as FromData.
        /// 当sizeType指定为FromData时，指定的高亮倍数系数。
        /// </summary>
        public float selectedDataScale
        {
            get { return m_SelectedDataScale; }
            set { if (PropertyUtil.SetStruct(ref m_SelectedDataScale, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the callback of size when sizeType assined as Callback.
        /// 当sizeType指定为Callback时，指定的回调函数。
        /// </summary>
        public SymbolSizeCallback sizeCallback
        {
            get { return m_SizeCallback; }
            set { if (PropertyUtil.SetClass(ref m_SizeCallback, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the callback of size when sizeType assined as Callback.
        /// 当sizeType指定为Callback时，指定的高亮回调函数。
        /// </summary>
        public SymbolSizeCallback selectedSizeCallback
        {
            get { return m_SelectedSizeCallback; }
            set { if (PropertyUtil.SetClass(ref m_SelectedSizeCallback, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the index start to show symbol.
        /// 开始显示图形标记的索引。
        /// </summary>
        public int startIndex
        {
            get { return m_StartIndex; }
            set { if (PropertyUtil.SetStruct(ref m_StartIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the interval of show symbol.
        /// 显示图形标记的间隔。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。
        /// </summary>
        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// whether to show the last symbol.
        /// 是否强制显示最后一个图形标记。
        /// </summary>
        public bool forceShowLast
        {
            get { return m_ForceShowLast; }
            set { if (PropertyUtil.SetStruct(ref m_ForceShowLast, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the gap of symbol and line segment.
        /// 图形标记和线条的间隙距离。
        /// </summary>
        public float gap
        {
            get { return m_Gap; }
            set { if (PropertyUtil.SetStruct(ref m_Gap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 图形的宽。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形的高。
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetAllDirty(); }
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
        /// 自定义的标记图形。
        /// </summary>
        public Sprite image
        {
            get { return m_Image; }
            set { if (PropertyUtil.SetClass(ref m_Image, value)) SetAllDirty(); }
        }
        public Image.Type imageType
        {
            get { return m_ImageType; }
            set { if (PropertyUtil.SetStruct(ref m_ImageType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形的偏移。
        /// </summary>
        public Vector2 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetAllDirty(); }
        }
        public Vector3 offset3 { get { return new Vector3(m_Offset.x, m_Offset.y, 0); } }
        private List<float> m_AnimationSize = new List<float>() { 0, 5, 10 };
        /// <summary>
        /// the setting for effect scatter.
        /// 带有涟漪特效动画的散点图的动画参数。
        /// </summary>
        public List<float> animationSize { get { return m_AnimationSize; } }

        /// <summary>
        /// 根据指定的sizeType获得标记的大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float GetSize(List<double> data, float themeSize)
        {
            switch (m_SizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return size == 0 ? themeSize : size;
                case SerieSymbolSizeType.FromData:
                    if (data != null && dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return (float)data[dataIndex] * m_DataScale;
                    }
                    else
                    {
                        return size == 0 ? themeSize : size;
                    }
                case SerieSymbolSizeType.Callback:
                    if (data != null && sizeCallback != null) return sizeCallback(data);
                    else return size == 0 ? themeSize : size;
                default: return size == 0 ? themeSize : size;
            }
        }

        /// <summary>
        /// 根据sizeType获得高亮时的标记大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float GetSelectedSize(List<double> data, float themeSelectedSize)
        {
            switch (m_SizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return selectedSize == 0 ? themeSelectedSize : selectedSize;
                case SerieSymbolSizeType.FromData:
                    if (data != null && dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return (float)data[dataIndex] * m_SelectedDataScale;
                    }
                    else
                    {
                        return selectedSize == 0 ? themeSelectedSize : selectedSize;
                    }
                case SerieSymbolSizeType.Callback:
                    if (data != null && selectedSizeCallback != null) return selectedSizeCallback(data);
                    else return selectedSize == 0 ? themeSelectedSize : selectedSize;
                default: return selectedSize == 0 ? themeSelectedSize : selectedSize;
            }
        }

        public bool ShowSymbol(int dataIndex, int dataCount)
        {
            if (!show) return false;
            if (dataIndex < startIndex) return false;
            if (m_Interval <= 0) return true;
            if (m_ForceShowLast && dataIndex == dataCount - 1) return true;
            return (dataIndex - startIndex) % (m_Interval + 1) == 0;
        }
    }
}
