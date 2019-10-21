/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// 高亮的图形样式和文本标签样式。
    /// </summary>
    [System.Serializable]
    public class Emphasis : SubComponent
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private SerieLabel m_Label = new SerieLabel();
        [SerializeField] private ItemStyle m_ItemStyle = new ItemStyle();
        /// <summary>
        /// 是否启用高亮样式。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// 图形文本标签。
        /// </summary>
        public SerieLabel label { get { return m_Label; } }
        /// <summary>
        /// 图形样式。
        /// </summary>
        public ItemStyle itemStyle { get { return m_ItemStyle; } }
    }
}