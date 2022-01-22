
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(LiquidHandler), true)]
    [RequireChartComponent(typeof(Vessel))]
    [SerieExtraComponent(typeof(LabelStyle))]
    public class Liquid : Serie, INeedSerieContainer
    {
        [SerializeField] private float m_WaveHeight = 10f;
        [SerializeField] private float m_WaveLength = 20f;
        [SerializeField] private float m_WaveSpeed = 5f;
        [SerializeField] private float m_WaveOffset = 0f;
        /// <summary>
        /// Wave length of the wave, which is relative to the diameter.
        /// 波长。为0-1小数时指直线的百分比。
        /// </summary>
        public float waveLength
        {
            get { return m_WaveLength; }
            set { if (PropertyUtil.SetStruct(ref m_WaveLength, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 波高。
        /// </summary>
        public float waveHeight
        {
            get { return m_WaveHeight; }
            set { if (PropertyUtil.SetStruct(ref m_WaveHeight, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 波偏移。
        /// </summary>
        public float waveOffset
        {
            get { return m_WaveOffset; }
            set { if (PropertyUtil.SetStruct(ref m_WaveOffset, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 波速。正数时左移，负数时右移。
        /// </summary>
        public float waveSpeed
        {
            get { return m_WaveSpeed; }
            set { if (PropertyUtil.SetStruct(ref m_WaveSpeed, value)) SetVerticesDirty(); }
        }

        public int containerIndex { get { return vesselIndex; } }
        public int containterInstanceId { get; internal set; }

        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            chart.AddChartComponentWhenNoExist<Vessel>();
            var serie = chart.AddSerie<Liquid>(serieName);
            serie.min = 0;
            serie.max = 100;
            serie.AddExtraComponent<LabelStyle>();
            serie.label.show = true;
            serie.label.textStyle.fontSize = 40;
            serie.label.formatter = "{d}%";
            serie.label.textStyle.color = new Color32(70, 70, 240, 255);
            chart.AddData(serie.index, UnityEngine.Random.Range(0, 100));
        }
    }
}