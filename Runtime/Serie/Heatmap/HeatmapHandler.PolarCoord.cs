using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// For polar coord
    /// </summary>
    internal sealed partial class HeatmapHandler
    {
        private PolarCoord m_SeriePolar;

        private void UpdateSeriePolarContext()
        {
            if (m_SeriePolar == null)
                return;

            var needCheck = (chart.isPointerInChart && m_SeriePolar.IsPointerEnter()) || m_LegendEnter;
            var lineWidth = 0f;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    var needAnimation1 = false;
                    lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    serie.interact.SetValue(ref needAnimation1, lineWidth);
                    foreach (var serieData in serie.data)
                    {
                        var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                        var symbolSize = symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                        serieData.context.highlight = false;
                        serieData.interact.SetValue(ref needAnimation1, symbolSize);
                    }
                    if (needAnimation1)
                    {
                        if (SeriesHelper.IsStack(chart.series))
                            chart.RefreshTopPainter();
                        else
                            chart.RefreshPainter(serie);
                    }
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            var themeSymbolSize = chart.theme.serie.lineSymbolSize;
            lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);

            var needInteract = false;
            if (m_LegendEnter)
            {
                serie.context.pointerEnter = true;
                serie.interact.SetValue(ref needInteract, serie.animation.interaction.GetWidth(lineWidth));
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, SerieState.Emphasis);
                    serieData.context.highlight = true;
                    serieData.interact.SetValue(ref needInteract, size);
                }
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                var dir = chart.pointerPos - new Vector2(m_SeriePolar.context.center.x, m_SeriePolar.context.center.y);
                var pointerAngle = ChartHelper.GetAngle360(Vector2.up, dir);
                var pointerRadius = Vector2.Distance(chart.pointerPos, m_SeriePolar.context.center);
                Color32 color, toColor;
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    if (pointerAngle >= serieData.context.startAngle &&
                        pointerAngle < serieData.context.toAngle &&
                        pointerRadius >= serieData.context.insideRadius &&
                        pointerRadius < serieData.context.outsideRadius)
                    {
                        serie.context.pointerItemDataIndex = i;
                        serie.context.pointerEnter = true;
                        serieData.context.highlight = true;
                    }
                    else
                    {
                        serieData.context.highlight = false;
                    }
                    var state = SerieHelper.GetSerieState(serie, serieData, true);
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, state);
                    serieData.interact.SetColor(ref needInteract, color, toColor);
                }
            }
            if (needInteract)
            {
                if (SeriesHelper.IsStack(chart.series))
                    chart.RefreshTopPainter();
                else
                    chart.RefreshPainter(serie);
            }
        }

        private void DrawPolarHeatmap(VertexHelper vh, Serie serie)
        {
            var datas = serie.data;
            if (datas.Count <= 0)
                return;

            m_SeriePolar = chart.GetChartComponent<PolarCoord>(serie.polarIndex);
            if (m_SeriePolar == null)
                return;

            var m_AngleAxis = ComponentHelper.GetAngleAxis(chart.components, m_SeriePolar.index);
            var m_RadiusAxis = ComponentHelper.GetRadiusAxis(chart.components, m_SeriePolar.index);
            if (m_AngleAxis == null || m_RadiusAxis == null)
                return;
            var visualMap = chart.GetVisualMapOfSerie(serie);

            var startAngle = m_AngleAxis.context.startAngle;
            var currDetailProgress = 0f;
            var totalDetailProgress = datas.Count;

            var xCount = AxisHelper.GetTotalSplitGridNum(m_RadiusAxis);
            var yCount = AxisHelper.GetTotalSplitGridNum(m_AngleAxis);
            var xWidth = m_SeriePolar.context.radius / xCount;
            var yWidth = 360 / yCount;

            serie.animation.InitProgress(currDetailProgress, totalDetailProgress);

            var dimension = VisualMapHelper.GetDimension(visualMap, defaultDimension);
            if (visualMap.autoMinMax)
            {
                double maxValue, minValue;
                SerieHelper.GetMinMaxData(serie, dimension, out minValue, out maxValue);
                VisualMapHelper.SetMinMax(visualMap, minValue, maxValue);
            }
            var rangeMin = visualMap.rangeMin;
            var rangeMax = visualMap.rangeMax;
            var color = chart.theme.GetColor(serie.index);

            float start, end;
            float inside, outside;
            double value, radiusValue, angleValue;
            for (int i = 0; i < datas.Count; i++)
            {
                if (serie.animation.CheckDetailBreak(i))
                    break;
                var serieData = datas[i];
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;

                radiusValue = serieData.GetData(0);
                angleValue = serieData.GetData(1);
                value = serieData.GetData(2);

                var xIndex = AxisHelper.GetAxisValueSplitIndex(m_RadiusAxis, radiusValue, true, xCount);
                var yIndex = AxisHelper.GetAxisValueSplitIndex(m_AngleAxis, angleValue, true, yCount);

                start = startAngle + yIndex * yWidth;
                end = start + yWidth;

                inside = m_SeriePolar.context.insideRadius + xIndex * xWidth;
                outside = inside + xWidth;

                serieData.context.startAngle = start;
                serieData.context.toAngle = end;
                serieData.context.halfAngle = (start + end) / 2;
                serieData.context.insideRadius = inside;
                serieData.context.outsideRadius = outside;

                if ((value < rangeMin && rangeMin != visualMap.min) ||
                    (value > rangeMax && rangeMax != visualMap.max))
                {
                    continue;
                }
                if (!visualMap.IsInSelectedValue(value)) continue;
                color = visualMap.GetColor(value);
                if (serieData.context.highlight)
                    color = ChartHelper.GetHighlightColor(color);

                var needRoundCap = serie.roundCap && inside > 0;

                serieData.context.insideRadius = inside;
                serieData.context.outsideRadius = outside;
                serieData.context.areaCenter = m_SeriePolar.context.center;
                serieData.context.position = ChartHelper.GetPosition(m_SeriePolar.context.center, (start + end) / 2, (inside + outside) / 2);

                UGL.DrawDoughnut(vh, m_SeriePolar.context.center, inside, outside, color, color,
                    ColorUtil.clearColor32, start, end, borderWidth, borderColor, serie.gap / 2, chart.settings.cicleSmoothness,
                    needRoundCap, true);
            }

            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.RefreshChart();
            }
        }
    }
}