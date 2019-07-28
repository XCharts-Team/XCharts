using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    /// <summary>
    /// A data item of serie.系列中的一个数据项。
    /// </summary>
    public class SerieData
    {
        [SerializeField] private string m_Name;
        [SerializeField] private bool m_Selected;
        [SerializeField] private List<float> m_Data = new List<float>();

        private bool m_Show = true;

        /// <summary>
        /// the name of data item.数据项名称。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// Whether the data item is selected.该数据项是否被选中。
        /// </summary>
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        /// <summary>
        /// An arbitrary dimension data list of data item.可指定任意维数的数值列表。
        /// </summary>
        /// <value></value>
        public List<float> data { get { return m_Data; } set { m_Data = value; } }
        /// <summary>
        /// Whether the data item is showed.该数据项是否要显示。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Whether the data item is highlighted.该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlighted { get; set; }
        /// <summary>
        /// the label of data item.该数据项的文本标签。
        /// </summary>
        /// <value></value>
        public Text label { get; set; }
    }
}
