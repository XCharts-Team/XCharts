/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(GaugeHandler), true)]
    public class Gauge : Serie
    {
        [SerializeField] private GaugeType m_GaugeType = GaugeType.Pointer;
        [SerializeField] private GaugeAxis m_GaugeAxis = new GaugeAxis();
        [SerializeField] private GaugePointer m_GaugePointer = new GaugePointer();
        /// <summary>
        /// 仪表盘轴线。
        /// </summary>
        public GaugeAxis gaugeAxis
        {
            get { return m_GaugeAxis; }
            set { if (PropertyUtil.SetClass(ref m_GaugeAxis, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 仪表盘指针。
        /// </summary>
        public GaugePointer gaugePointer
        {
            get { return m_GaugePointer; }
            set { if (PropertyUtil.SetClass(ref m_GaugePointer, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 仪表盘类型。
        /// </summary>
        public GaugeType gaugeType
        {
            get { return m_GaugeType; }
            set { if (PropertyUtil.SetStruct(ref m_GaugeType, value)) SetVerticesDirty(); }
        }

        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Gauge>(serieName);
            serie.min = 0;
            serie.max = 100;
            serie.startAngle = -125;
            serie.endAngle = 125;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.5f;
            serie.radius[0] = 80;
            serie.splitNumber = 5;
            serie.animation.dataChangeEnable = true;
            serie.titleStyle.show = true;
            serie.titleStyle.textStyle.offset = new Vector2(0, 20);
            serie.label.show = true;
            serie.label.offset = new Vector3(0, -30);
            serie.itemStyle.show = true;
            serie.gaugeAxis.axisLabel.show = true;
            serie.gaugeAxis.axisLabel.margin = 18;
            chart.AddData(serie.index, UnityEngine.Random.Range(10, 90), "title");
        }

        public override bool vertsDirty
        {
            get
            {
                return base.vertsDirty
                    || gaugeAxis.vertsDirty
                    || gaugePointer.vertsDirty;
            }
        }

        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            gaugeAxis.ClearVerticesDirty();
            gaugePointer.ClearVerticesDirty();
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            gaugeAxis.ClearComponentDirty();
            gaugePointer.ClearComponentDirty();
        }
    }
}