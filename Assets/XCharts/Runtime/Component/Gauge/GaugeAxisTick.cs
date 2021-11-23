/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// 刻度
    /// </summary>
    [System.Serializable]
    public class GaugeAxisTick : BaseLine
    {
        [SerializeField] private float m_SplitNumber = 5;
        /// <summary>
        /// 分割线之间的分割段数。
        /// </summary>
        public float splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
        public GaugeAxisTick(bool show) : base(show)
        {
        }
    }
}