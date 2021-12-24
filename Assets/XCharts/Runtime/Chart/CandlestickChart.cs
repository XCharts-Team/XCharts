

using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/CandlestickChart", 23)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class CandlestickChart : BaseChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            AddChartComponentWhenNoExist<GridCoord>();
            AddChartComponentWhenNoExist<XAxis>();
            AddChartComponentWhenNoExist<YAxis>();

            var tooltip = GetChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Shadow;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            Candlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
#endif
    }
}
