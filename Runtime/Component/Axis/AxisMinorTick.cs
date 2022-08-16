using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Settings related to axis minor tick.
    /// |坐标轴次刻度相关设置。注意：次刻度无法在类目轴中使用。
    /// </summary>
    [System.Serializable]
    [Since("v3.2.0")]
    public class AxisMinorTick : BaseLine
    {
        [SerializeField] protected int m_SplitNumber = 5;
        [SerializeField] private bool m_AutoColor;

        /// <summary>
        /// Number of segments that the axis is split into.
        /// |分隔线之间分割的刻度数。
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtil.SetStruct(ref m_SplitNumber, value)) SetAllDirty(); }
        }
        public bool autoColor { get { return m_AutoColor; } set { m_AutoColor = value; } }

        public override bool vertsDirty { get { return m_VertsDirty || m_LineStyle.anyDirty; } }
        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            m_LineStyle.ClearVerticesDirty();
        }
        public static AxisMinorTick defaultMinorTick
        {
            get
            {
                var tick = new AxisMinorTick
                {
                    m_Show = false
                };
                return tick;
            }
        }

        public AxisMinorTick Clone()
        {
            var axisTick = new AxisMinorTick();
            axisTick.show = show;
            axisTick.splitNumber = splitNumber;
            axisTick.autoColor = autoColor;
            axisTick.lineStyle = lineStyle.Clone();
            return axisTick;
        }

        public void Copy(AxisMinorTick axisTick)
        {
            show = axisTick.show;
            splitNumber = axisTick.splitNumber;
            autoColor = axisTick.autoColor;
            lineStyle.Copy(axisTick.lineStyle);
        }
    }
}