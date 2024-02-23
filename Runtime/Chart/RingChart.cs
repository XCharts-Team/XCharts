using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Ring chart is mainly used to show the proportion of each item and the relationship between the items.
    /// || 环形图主要用于显示每一项的比例以及各项之间的关系。
    /// </summary>
    [AddComponentMenu("XCharts/RingChart", 20)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class RingChart : BaseChart
    {
        protected override void DefaultChart()
        {
            GetChartComponent<Tooltip>().type = Tooltip.Type.Line;
            RemoveData();
            Ring.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// default multiple ring chart.
        /// || 默认多圆环图。
        /// </summary>
        public void DefaultMultipleRingChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.label.show = false;
            AddData(0, UnityEngine.Random.Range(30, 90), 100, "data2");
            AddData(0, UnityEngine.Random.Range(30, 90), 100, "data3");
        }
    }
}