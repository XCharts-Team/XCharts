/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class StageColor
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

    [System.Serializable]
    public class GaugeAxisLine : BaseLine
    {
        [SerializeField] private Color32 m_BarColor;
        [SerializeField] private Color32 m_BarBackgroundColor = new Color32(200, 200, 200, 255);
        [SerializeField]
        private List<StageColor> m_StageColor = new List<StageColor>()
        {
            new StageColor(0.2f,new Color32(145,199,174,255)),
            new StageColor(0.8f,new Color32(99,134,158,255)),
            new StageColor(1.0f,new Color32(194,53,49,255)),
        };
        /// <summary>
        /// 进度条颜色。
        /// </summary>
        public Color32 barColor { get { return m_BarColor; } set { m_BarColor = value; } }
        /// <summary>
        /// 进度条背景颜色。
        /// </summary>
        public Color32 barBackgroundColor { get { return m_BarBackgroundColor; } set { m_BarBackgroundColor = value; } }
        /// <summary>
        /// 阶段颜色。
        /// </summary>
        public List<StageColor> stageColor { get { return m_StageColor; } set { m_StageColor = value; } }

        public GaugeAxisLine(bool show) : base(show)
        {
        }
    }
}