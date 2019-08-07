using System.Collections.Generic;
using UnityEngine;

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
        /// 正方形。
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
    public delegate float SymbolSizeCallback(List<float> data);

    /// <summary>
    /// 系列数据项的标记的图形
    /// </summary>
    [System.Serializable]
    public class SerieSymbol
    {
        [SerializeField] private SerieSymbolType m_Type = SerieSymbolType.EmptyCircle;
        [SerializeField] private SerieSymbolSizeType m_SizeType = SerieSymbolSizeType.Custom;
        [SerializeField] private float m_Size = 6f;
        [SerializeField] private float m_SelectedSize = 10f;
        [SerializeField] private int m_DataIndex = 1;
        [SerializeField] private float m_DataScale = 1;
        [SerializeField] private float m_SelectedDataScale = 1.5f;
        [SerializeField] private SymbolSizeCallback m_SizeCallback;
        [SerializeField] private SymbolSizeCallback m_SelectedSizeCallback;
        [SerializeField] private Color m_Color;
        [SerializeField] [Range(0, 1)] private float m_Opacity = 1;

        /// <summary>
        /// the type of symbol.
        /// 标记类型。
        /// </summary>
        /// <value></value>
        public SerieSymbolType type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// the type of symbol size.
        /// 标记图形的大小获取方式。
        /// </summary>
        /// <value></value>
        public SerieSymbolSizeType sizeType { get { return m_SizeType; } set { m_SizeType = value; } }
        /// <summary>
        /// the size of symbol.
        /// 标记的大小。
        /// </summary>
        /// <value></value>
        public float size { get { return m_Size; } set { m_Size = value; } }
        /// <summary>
        /// the size of selected symbol.
        /// 被选中的标记的大小。
        /// </summary>
        /// <value></value>
        public float selectedSize { get { return m_SelectedSize; } set { m_SelectedSize = value; } }
        /// <summary>
        /// whitch data index is when the sizeType assined as FromData.
        /// 当sizeType指定为FromData时，指定的数据源索引。
        /// </summary>
        /// <value></value>
        public int dataIndex { get { return m_DataIndex; } set { m_DataIndex = value; } }
        /// <summary>
        /// the scale of data when sizeType assined as FromData.
        /// 当sizeType指定为FromData时，指定的倍数系数。
        /// </summary>
        /// <value></value>
        public float dataScale { get { return m_DataScale; } set { m_DataScale = value; } }
        /// <summary>
        /// the scale of selected data when sizeType assined as FromData.
        /// 当sizeType指定为FromData时，指定的高亮倍数系数。
        /// </summary>
        /// <value></value>
        public float selectedDataScale { get { return m_SelectedDataScale; } set { m_SelectedDataScale = value; } }
        /// <summary>
        /// the callback of size when sizeType assined as Callback.
        /// 当sizeType指定为Callback时，指定的回调函数。
        /// </summary>
        /// <value></value>
        public SymbolSizeCallback sizeCallback { get { return m_SizeCallback; } set { m_SizeCallback = value; } }
        /// <summary>
        /// the callback of size when sizeType assined as Callback.
        /// 当sizeType指定为Callback时，指定的高亮回调函数。
        /// </summary>
        /// <value></value>
        public SymbolSizeCallback selectedSizeCallback { get { return m_SelectedSizeCallback; } set { m_SelectedSizeCallback = value; } }
        /// <summary>
        /// the color of symbol,default from serie.
        /// 标记图形的颜色，默认和系列一致。
        /// </summary>
        /// <value></value>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// the opacity of color.
        /// 图形标记的透明度。
        /// </summary>
        /// <value></value>
        public float opacity { get { return m_Opacity; } set { m_Opacity = value; } }
        private List<float> m_AnimationSize = new List<float>() { 0, 5, 10 };
        /// <summary>
        /// the setting for effect scatter.
        /// 带有涟漪特效动画的散点图的动画参数。
        /// </summary>
        /// <value></value>
        public List<float> animationSize { get { return m_AnimationSize; } }

        /// <summary>
        /// 根据指定的sizeType获得标记的大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float GetSize(List<float> data)
        {
            if (data == null) return size;
            switch (m_SizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return size;
                case SerieSymbolSizeType.FromData:
                    if (dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return data[dataIndex] * m_DataScale;
                    }
                    else
                    {
                        return size;
                    }
                case SerieSymbolSizeType.Callback:
                    if (sizeCallback != null) return sizeCallback(data);
                    else return size;
                default: return size;
            }
        }

        /// <summary>
        /// 根据sizeType获得高亮时的标记大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float GetSelectedSize(List<float> data)
        {
            if (data == null) return selectedSize;
            switch (m_SizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return selectedSize;
                case SerieSymbolSizeType.FromData:
                    if (dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return data[dataIndex] * m_SelectedDataScale;
                    }
                    else
                    {
                        return selectedSize;
                    }
                case SerieSymbolSizeType.Callback:
                    if (selectedSizeCallback != null) return selectedSizeCallback(data);
                    else return selectedSize;
                default: return selectedSize;
            }
        }
    }
}
