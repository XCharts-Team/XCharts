using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 数据项的其他基础数据。
    /// </summary>
    [System.Serializable]
    public class SerieDataBaseInfo : ChildComponent, ISerieDataComponent
    {
        [SerializeField] private bool m_Ignore = false;
        [SerializeField] private bool m_Selected;
        [SerializeField] private float m_Radius;

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
    }
}