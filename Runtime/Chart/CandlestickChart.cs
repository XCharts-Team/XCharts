using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// A candlestick chart is a style of financial chart used to describe price movements of a security, derivative, or currency.
    /// || 蜡烛图，也叫K线图，用于描述证券、衍生品或货币的价格走势的一种金融图表样式。
    /// </summary>
    [AddComponentMenu("XCharts/CandlestickChart", 23)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class CandlestickChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            var serie = Candlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < serie.dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}