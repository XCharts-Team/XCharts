using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// For polar coord
    /// </summary>
    internal sealed partial class BarHandler
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
                    serie.interact.SetValue(ref needAnimation1, lineWidth, false);
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
                serie.interact.SetValue(ref needInteract, lineWidth, true, chart.theme.serie.selectedRate);
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var size = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, themeSymbolSize, SerieState.Emphasis);
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

        private void DrawPolarBar(VertexHelper vh, Serie serie)
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

            var startAngle = m_AngleAxis.context.startAngle;
            var currDetailProgress = 0f;
            var totalDetailProgress = datas.Count;

            serie.animation.InitProgress(currDetailProgress, totalDetailProgress);

            var isStack = SeriesHelper.IsStack<Bar>(chart.series, serie.stack);
            if (isStack)
                SeriesHelper.UpdateStackDataList(chart.series, serie, null, m_StackSerieData);

            var barCount = chart.GetSerieBarRealCount<Bar>();
            var categoryWidth = m_AngleAxis.IsCategory() ?
                AxisHelper.GetDataWidth(m_AngleAxis, 360, datas.Count, null) :
                AxisHelper.GetDataWidth(m_RadiusAxis, m_SeriePolar.context.radius, datas.Count, null);
            var barGap = chart.GetSerieBarGap<Bar>();
            var totalBarWidth = chart.GetSerieTotalWidth<Bar>(categoryWidth, barGap, barCount);
            var barWidth = serie.GetBarWidth(categoryWidth, barCount);
            var offset = (categoryWidth - totalBarWidth) * 0.5f;
            var serieReadIndex = chart.GetSerieIndexIfStack<Bar>(serie);
            float gap = serie.barGap == -1 ? offset : offset + chart.GetSerieTotalGap<Bar>(categoryWidth, barGap, serieReadIndex);

            var areaColor = ColorUtil.clearColor32;
            var areaToColor = ColorUtil.clearColor32;
            var interacting = false;

            float start, end;
            float inside, outside;
            double radiusValue, angleValue;
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
                if (m_AngleAxis.IsCategory())
                {
                    start = (float) (startAngle + categoryWidth * angleValue + gap);
                    end = start + barWidth;
                    inside = m_SeriePolar.context.insideRadius;
                    if (isStack)
                    {
                        for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                            inside += m_StackSerieData[n][i].context.stackHeight;
                    }
                    outside = inside + m_RadiusAxis.GetValueLength(radiusValue, m_SeriePolar.context.radius);
                    serieData.context.stackHeight = outside - inside;
                }
                else
                {
                    start = startAngle;
                    if (isStack)
                    {
                        for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                            start += m_StackSerieData[n][i].context.stackHeight;
                    }
                    end = start + m_AngleAxis.GetValueLength(angleValue, 360);
                    serieData.context.stackHeight = end - start;
                    inside = m_SeriePolar.context.insideRadius + categoryWidth * (float) radiusValue + gap;
                    outside = inside + barWidth;
                }
                serieData.context.startAngle = start;
                serieData.context.toAngle = end;
                serieData.context.halfAngle = (start + end) / 2;

                if (!serieData.interact.TryGetColor(ref areaColor, ref areaToColor, ref interacting))
                {
                    SerieHelper.GetItemColor(out areaColor, out areaToColor, serie, serieData, chart.theme);
                    serieData.interact.SetColor(ref interacting, areaColor, areaToColor);
                }

                var needRoundCap = serie.roundCap && inside > 0;

                serieData.context.insideRadius = inside;
                serieData.context.outsideRadius = outside;
                serieData.context.areaCenter = m_SeriePolar.context.center;
                serieData.context.position = ChartHelper.GetPosition(m_SeriePolar.context.center, (start + end) / 2, (inside + outside) / 2);

                UGL.DrawDoughnut(vh, m_SeriePolar.context.center, inside, outside, areaColor, areaToColor,
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