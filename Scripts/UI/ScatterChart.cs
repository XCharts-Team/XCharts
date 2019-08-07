using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/ScatterChart", 17)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ScatterChart : CoordinateChart
    {
        private float m_EffectScatterSpeed = 15;
        private float m_EffectScatterSize;
        private float m_EffectScatterAplha;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "ScatterChart";
            m_Tooltip.type = Tooltip.Type.None;
            m_XAxises[0].type = Axis.AxisType.Value;
            m_XAxises[0].boundaryGap = false;
            m_YAxises[1].type = Axis.AxisType.Value;
            m_XAxises[1].boundaryGap = false;
            RemoveData();
            AddSerie("serie1", SerieType.Scatter);
            for (int i = 0; i < 10; i++)
            {
                AddData(0, Random.Range(10, 100), Random.Range(10, 100));
            }
        }
#endif

        protected override void Update()
        {
            base.Update();
            bool hasEffectScatter = false;
            foreach (var serie in m_Series.series)
            {
                if (serie.type == SerieType.EffectScatter)
                {
                    hasEffectScatter = true;
                    for (int i = 0; i < serie.symbol.animationSize.Count; ++i)
                    {
                        serie.symbol.animationSize[i] += m_EffectScatterSpeed * Time.deltaTime;
                        if (serie.symbol.animationSize[i] > serie.symbol.size)
                        {
                            serie.symbol.animationSize[i] = i * 5;
                        }
                    }
                }
            }
            if (hasEffectScatter)
            {
                RefreshChart();
            }
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            HashSet<string> serieNameSet = new HashSet<string>();
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                serie.index = i;
                var yAxis = m_YAxises[serie.axisIndex];
                var xAxis = m_XAxises[serie.axisIndex];
                if (string.IsNullOrEmpty(serie.name)) serieNameCount++;
                else if (!serieNameSet.Contains(serie.name))
                {
                    serieNameSet.Add(serie.name);
                    serieNameCount++;
                }
                if (serie.dataCount <= 0 || !serie.show)
                {
                    continue;
                }
                var color = serie.symbol.color != Color.clear ? serie.symbol.color : (Color)m_ThemeInfo.GetColor(serieNameCount);
                color.a *= serie.symbol.opacity;
                int maxCount = maxShowDataNumber > 0 ?
                    (maxShowDataNumber > serie.dataCount ? serie.dataCount : maxShowDataNumber)
                    : serie.dataCount;
                for (int n = minShowDataNumber; n < maxCount; n++)
                {
                    var serieData = serie.GetDataList(m_DataZoom)[n];
                    float xValue = serieData.data[0];
                    float yValue = serieData.data[1];
                    float pX = coordinateX + xAxis.axisLine.width;
                    float pY = coordinateY + yAxis.axisLine.width;
                    float xDataHig = (xValue - xAxis.minValue) / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                    float yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                    var pos = new Vector3(pX + xDataHig, pY + yDataHig);

                    var datas = serie.data[n].data;
                    float symbolSize = 0;
                    if (serie.highlighted || serieData.highlighted)
                    {
                        symbolSize = serie.symbol.GetSelectedSize(datas);
                    }
                    else
                    {
                        symbolSize = serie.symbol.GetSize(datas);
                    }
                    if (symbolSize > 100) symbolSize = 100;
                    if (serie.type == SerieType.EffectScatter)
                    {
                        for (int count = 0; count < serie.symbol.animationSize.Count; count++)
                        {
                            var nowSize = serie.symbol.animationSize[count];
                            color.a = (symbolSize - nowSize) / symbolSize;
                            DrawSymbol(vh, serie.symbol.type, nowSize, 3, pos, color);
                        }
                        RefreshChart();
                    }
                    else
                    {
                        DrawSymbol(vh, serie.symbol.type, symbolSize, 3, pos, color);
                    }
                }
                if (vh.currentVertCount > 60000)
                {
                    m_Large++;
                    RefreshChart();
                    return;
                }
            }
        }
    }
}
