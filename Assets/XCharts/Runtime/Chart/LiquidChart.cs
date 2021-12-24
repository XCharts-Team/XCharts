
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// 水位图
    /// </summary>
    [AddComponentMenu("XCharts/LiquidChart", 22)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LiquidChart : BaseChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            GetChartComponent<Tooltip>().type = Tooltip.Type.Line;
            RemoveData();

            Liquid.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
#endif
    }
}
