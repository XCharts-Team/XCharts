using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// The pie chart is mainly used for showing proportion of different categories. Each arc length represents the proportion of data quantity.    
    /// || 饼图主要用于显示不同类目占比的情况，通过弧长来反映数据的大小占比。     
    /// </summary>
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class PieChart : BaseChart
    {
        protected override void DefaultChart()
        {
            var legend = EnsureChartComponent<Legend>();
            legend.show = true;

            RemoveData();
            Pie.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// default label pie chart.
        /// || 默认带标签饼图。
        /// </summary>
        public void DefaultLabelPieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }

        /// <summary>
        /// default donut pie chart.
        /// || 默认甜甜圈饼图。
        /// </summary>
        public void DefaultDonutPieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.radius[0] = 0.20f;
            serie.radius[1] = 0.28f;
        }

        /// <summary>
        /// default label donut pie chart.
        /// || 默认带标签甜甜圈饼图。
        /// </summary>
        public void DefaultLabelDonutPieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.radius[0] = 0.20f;
            serie.radius[1] = 0.28f;
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }

        /// <summary>
        /// default rose pie chart.
        /// || 默认玫瑰饼图。
        /// </summary>
        public void DefaultRadiusRosePieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.pieRoseType = RoseType.Radius;
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }

        /// <summary>
        /// default area rose pie chart.
        /// || 默认面积玫瑰饼图。
        /// </summary>
        public void DefaultAreaRosePieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.pieRoseType = RoseType.Area;
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }
    }
}