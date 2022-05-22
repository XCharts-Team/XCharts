using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class StageColor : ChildComponent
    {
        [SerializeField] private float m_Percent;
        [SerializeField] private Color32 m_Color;
        /// <summary>
        /// 结束位置百分比。
        /// </summary>
        public float percent { get { return m_Percent; } set { m_Percent = value; } }
        /// <summary>
        /// 颜色。
        /// </summary>
        public Color32 color { get { return m_Color; } set { m_Color = value; } }

        public StageColor(float percent, Color32 color)
        {
            m_Percent = percent;
            m_Color = color;
        }
    }
}