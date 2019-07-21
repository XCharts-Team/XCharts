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
        [SerializeField] private Scatter m_Scatter = Scatter.defaultScatter;

        public Scatter scatter { get { return m_Scatter; } }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Scatter = Scatter.defaultScatter;
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
                AddXYData(0, Random.Range(10, 100), Random.Range(10, 100));
            }
        }
#endif

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
                int maxCount = maxShowDataNumber > 0 ?
                    (maxShowDataNumber > serie.dataCount ? serie.dataCount : maxShowDataNumber)
                    : serie.dataCount;
                int dataCount = (maxCount - minShowDataNumber);
                for (int n = minShowDataNumber; n < maxCount; n++)
                {
                    float xValue, yValue;
                    serie.GetXYData(n, m_DataZoom, out xValue, out yValue);
                    float pX = coordinateX + m_Coordinate.tickness;
                    float pY = coordinateY + m_Coordinate.tickness;
                    float xDataHig = (xValue - xAxis.minValue) / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                    float yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                    var pos = new Vector3(pX + xDataHig, pY + yDataHig);
                    var color = m_ThemeInfo.GetColor(serieNameCount);
                    color.a = 200;
                    var datas = serie.data[n].data;
                    float symbolSize = 0;
                    if (serie.selected && n == m_Tooltip.dataIndex[serie.axisIndex])
                    {
                        symbolSize = serie.symbol.GetSelectedSize(datas);
                    }
                    else
                    {
                        symbolSize = serie.symbol.GetSize(datas);
                    }
                    if (symbolSize > 100) symbolSize = 100;
                    DrawSymbol(vh, serie.symbol.type, symbolSize, 3, pos, color);
                }
                if (vh.currentVertCount > 60000)
                {
                    m_Large++;
                    Debug.LogError("large:" + m_Large + "," + vh.currentVertCount);
                    RefreshChart();
                    return;
                }
            }
        }
    }
}
