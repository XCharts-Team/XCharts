/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    /// <summary>
    /// For polar coord
    /// </summary>
    internal sealed partial class LineHandler
    {
        private void DrawPolarLine(VertexHelper vh, Serie serie)
        {
            var datas = serie.data;
            if (datas.Count <= 0)
                return;

            var m_Polar = chart.GetChartComponent<PolarCoord>(serie.polarIndex);
            if (m_Polar == null)
                return;

            var m_AngleAxis = ComponentHelper.GetAngleAxis(chart.components, m_Polar.index);
            var m_RadiusAxis = ComponentHelper.GetRadiusAxis(chart.components, m_Polar.index);
            if (m_AngleAxis == null || m_RadiusAxis == null)
                return;

            var startAngle = m_AngleAxis.startAngle;
            var radius = m_Polar.context.radius;

            var min = m_RadiusAxis.context.minValue;
            var max = m_RadiusAxis.context.maxValue;
            var firstSerieData = datas[0];
            var startPos = GetPolarPos(m_Polar, m_AngleAxis, firstSerieData, min, max, radius);
            var nextPos = Vector3.zero;
            var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serie.index, serie.highlighted);
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            var currDetailProgress = 0f;
            var totalDetailProgress = datas.Count;

            serie.animation.InitProgress(serie.context.dataPoints.Count, currDetailProgress, totalDetailProgress);
            serie.animation.SetDataFinish(0);

            for (int i = 1; i < datas.Count; i++)
            {
                if (serie.animation.CheckDetailBreak(i))
                    break;

                var serieData = datas[i];
                nextPos = GetPolarPos(m_Polar, m_AngleAxis, datas[i], min, max, radius);
                UGL.DrawLine(vh, startPos, nextPos, lineWidth, lineColor);
                startPos = nextPos;
            }

            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.m_IsPlayingAnimation = true;
                chart.RefreshChart();
            }
        }

        private void DrawPolarLineSymbol(VertexHelper vh)
        {
            for (int n = 0; n < chart.series.Count; n++)
            {
                var serie = chart.series[n];

                if (!serie.show)
                    continue;
                if (!(serie is Line))
                    continue;

                var count = serie.dataCount;
                for (int i = 0; i < count; i++)
                {
                    var serieData = serie.GetSerieData(i);
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    if (ChartHelper.IsIngore(serieData.context.position))
                        continue;

                    bool highlight = serieData.context.highlighted || serie.highlighted;
                    if ((!symbol.show || !symbol.ShowSymbol(i, count) || serie.IsPerformanceMode())
                        && !serieData.context.highlighted)
                        continue;

                    var symbolSize = highlight
                        ? symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSize)
                        : symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);

                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, n, highlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, n, highlight);
                    var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, n, highlight, false);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, highlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);

                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, serieData.context.position,
                        symbolColor, symbolToColor, symbolEmptyColor, symbol.gap, cornerRadius);
                }
            }
        }

        private Vector3 GetPolarPos(PolarCoord m_Polar, AngleAxis m_AngleAxis, SerieData serieData, double min,
            double max, float polarRadius)
        {
            var angle = 0f;

            if (!m_AngleAxis.clockwise)
            {
                angle = m_AngleAxis.startAngle - (float)serieData.GetData(1);
            }
            else
            {
                angle = m_AngleAxis.startAngle + (float)serieData.GetData(1);
            }

            var value = serieData.GetData(0);
            var radius = (float)((value - min) / (max - min) * polarRadius);

            angle = (angle + 360) % 360;
            serieData.context.angle = angle;
            serieData.context.position = ChartHelper.GetPos(m_Polar.context.center, radius, angle, true);

            return serieData.context.position;
        }
    }
}