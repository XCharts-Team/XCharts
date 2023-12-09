using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the link of serie data. Used for sankey chart. Sankey chart only supports directed acyclic graph. make sure the data link is directed acyclic graph.
    /// ||数据节点之间的连线。可用于桑基图等，桑基图只支持有向无环图，请保证数据的连线是有向无环图。
    /// </summary>
    [System.Serializable]
    [Since("v3.10.0")]
    public class SerieDataLink : ChildComponent
    {
        [SerializeField] private string m_Source;
        [SerializeField] private string m_Target;
        [SerializeField] private double m_Value;

        /// <summary>
        /// the source node name.
        /// ||边的源节点名称。
        /// </summary>
        public string source
        {
            get { return m_Source; }
            set { m_Source = value; }
        }

        /// <summary>
        /// the target node name.
        /// ||边的目标节点名称。
        /// </summary>
        public string target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        /// <summary>
        /// the value of link. decide the width of link.
        /// ||边的值。决定边的宽度。
        /// </summary>
        public double value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
    }
}