using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public enum SerieSymbolType
    {
        EmptyCircle,
        Circle,
        Rect,
        Triangle,
        Diamond,
        None,
    }

    /// <summary>
    /// The way to get serie symbol size.
    /// <para> `Custom`:Specify constant for symbol size. </para>
    /// <para> `FromData`:Specify the dataIndex and dataScale to calculate symbol size,the formula:data[dataIndex]*dataScale. </para>
    /// <para> `Callback`:Specify callback function for symbol size. </para>
    /// </summary>
    public enum SerieSymbolSizeType
    {
        /// <summary>
        /// Specify constant for symbol size.
        /// </summary>
        Custom,
        /// <summary>
        /// Specify the dataIndex and dataScale to calculate symbol size
        /// </summary>
        FromData,
        /// <summary>
        /// Specify callback function for symbol size
        /// </summary>
        Callback,
    }

    public delegate float SymbolSizeCallback(List<float> data);

    [System.Serializable]
    public class SerieSymbol
    {
        [SerializeField] private SerieSymbolType m_Type = SerieSymbolType.EmptyCircle;
        [SerializeField] private SerieSymbolSizeType m_SizeType = SerieSymbolSizeType.Custom;
        [SerializeField] private float m_Size = 20f;
        [SerializeField] private float m_SelectedSize = 30f;
        [SerializeField] private int m_DataIndex = 1;
        [SerializeField] private float m_DataScale = 1;
        [SerializeField] private float m_SelectedDataScale = 1.5f;
        [SerializeField] private SymbolSizeCallback m_SizeCallback;
        [SerializeField] private SymbolSizeCallback m_SelectedSizeCallback;

        public SerieSymbolType type { get { return m_Type; } set { m_Type = value; } }
        public float size { get { return m_Size; } set { m_Size = value; } }
        public float selectedSize { get { return m_SelectedSize; } set { m_SelectedSize = value; } }
        public int dataIndex { get { return m_DataIndex; } set { m_DataIndex = value; } }
        public float dataScale { get { return m_DataScale; } set { m_DataScale = value; } }
        public float selectedDataScale { get { return m_SelectedDataScale; } set { m_SelectedDataScale = value; } }
        public SymbolSizeCallback sizeCallback { get { return m_SizeCallback; } set { m_SizeCallback = value; } }
        public SymbolSizeCallback selectedSizeCallback { get { return m_SelectedSizeCallback; } set { m_SelectedSizeCallback = value; } }

        private List<float> m_AnimationSize = new List<float>() { 0, 5, 10 };
        public List<float> animationSize { get { return m_AnimationSize; } }
        public Color animationColor { get; set; }

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
