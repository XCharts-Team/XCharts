using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Line chart relates all the data points symbol by broken lines, which is used to show the trend of data changing. 
    /// It could be used in both rectangular coordinate andpolar coordinate.
    /// ||折线图是用折线将各个数据点标志连接起来的图表，用于展现数据的变化趋势。可用于直角坐标系和极坐标系上。
    /// 设置 areaStyle 后可以绘制面积图。
    /// </summary>
    [AddComponentMenu("XCharts/LineChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class LineChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }

        /// <summary>
        /// default area line chart.
        /// || 默认面积折线图。
        /// </summary>
        public void DefaultAreaLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.EnsureComponent<AreaStyle>();
        }

        /// <summary>
        /// default smooth line chart.
        /// || 默认平滑折线图。
        /// </summary>
        public void DefaultSmoothLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.lineType = LineType.Smooth;
        }

        /// <summary>
        /// default smooth area line chart.
        /// || 默认平滑面积折线图。
        /// </summary>
        public void DefaultSmoothAreaLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.EnsureComponent<AreaStyle>();
            serie.lineType = LineType.Smooth;
        }

        /// <summary>
        /// default stack line chart.
        /// || 默认堆叠折线图。
        /// </summary>
        public void DefaultStackLineChart()
        {
            CheckChartInit();
            var serie1 = GetSerie(0);
            if (serie1 == null) return;
            serie1.stack = "stack1";
            var serie2 = Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            serie2.stack = "stack1";
        }

        /// <summary>
        /// default stack area line chart.
        /// || 默认堆叠面积折线图。
        /// </summary>
        public void DefaultStackAreaLineChart()
        {
            CheckChartInit();
            var serie1 = GetSerie(0);
            if (serie1 == null) return;
            serie1.EnsureComponent<AreaStyle>();
            serie1.stack = "stack1";
            var serie2 = Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            serie2.EnsureComponent<AreaStyle>();
            serie2.stack = "stack1";
        }

        /// <summary>
        /// default step line chart.
        /// || 默认阶梯折线图。
        /// </summary>
        public void DefaultStepLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.lineType = LineType.StepMiddle;
        }

        /// <summary>
        /// default dash line chart.
        /// || 默认虚线折线图。
        /// </summary>
        public void DefaultDashLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.lineType = LineType.Normal;
            serie.lineStyle.type = LineStyle.Type.Dashed;
        }

        /// <summary>
        /// default time line chart.
        /// || 默认时间折线图。
        /// </summary>
        public void DefaultTimeLineChart()
        {
            CheckChartInit();
            var xAxis = GetChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Time;
        }

        /// <summary>
        /// default logarithmic line chart.
        /// || 默认对数轴折线图。
        /// </summary>
        public void DefaultLogLineChart()
        {
            CheckChartInit();
            var yAxis = GetChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Log;
        }
    }
}