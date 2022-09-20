using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// For polar coord
    /// </summary>
    internal sealed partial class LineHandler
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
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var angle0 = serieData.context.angle;
                    var angle1 = i >= serie.dataCount - 1 ? angle0 : serie.data[i + 1].context.angle;

                    if (pointerAngle >= angle0 && pointerAngle < angle1)
                    {
                        serie.context.pointerItemDataIndex = i;
                        serie.context.pointerEnter = true;
                        serieData.context.highlight = true;
                    }
                    else
                    {
                        serieData.context.highlight = false;
                    }
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

        private void DrawPolarLine(VertexHelper vh, Serie serie)
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

            var startAngle = m_AngleAxis.startAngle;
            var firstSerieData = datas[0];
            var lp = PolarHelper.UpdatePolarAngleAndPos(m_SeriePolar, m_AngleAxis, m_RadiusAxis, firstSerieData);
            var cp = Vector3.zero;
            var lineColor = SerieHelper.GetLineColor(serie, null, chart.theme, serie.context.colorIndex);
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            var currDetailProgress = 0f;
            var totalDetailProgress = datas.Count;

            serie.animation.InitProgress(currDetailProgress, totalDetailProgress);

            var ltp = Vector3.zero;
            var lbp = Vector3.zero;
            var ntp = Vector3.zero;
            var nbp = Vector3.zero;
            var itp = Vector3.zero;
            var ibp = Vector3.zero;
            var clp = Vector3.zero;
            var crp = Vector3.zero;
            bool bitp = true, bibp = true;
            if (datas.Count <= 2)
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    var serieData = datas[i];
                    cp = PolarHelper.UpdatePolarAngleAndPos(m_SeriePolar, m_AngleAxis, m_RadiusAxis, datas[i]);
                    serieData.context.position = cp;
                    serie.context.dataPoints.Add(cp);
                }
                UGL.DrawLine(vh, serie.context.dataPoints, lineWidth, lineColor, false, false);
            }
            else
            {
                for (int i = 1; i < datas.Count; i++)
                {
                    if (serie.animation.CheckDetailBreak(i))
                        break;

                    var serieData = datas[i];
                    cp = PolarHelper.UpdatePolarAngleAndPos(m_SeriePolar, m_AngleAxis, m_RadiusAxis, datas[i]);
                    serieData.context.position = cp;
                    serie.context.dataPoints.Add(cp);

                    var np = i == datas.Count - 1 ? cp :
                        PolarHelper.UpdatePolarAngleAndPos(m_SeriePolar, m_AngleAxis, m_RadiusAxis, datas[i + 1]);

                    UGLHelper.GetLinePoints(lp, cp, np, lineWidth,
                        ref ltp, ref lbp,
                        ref ntp, ref nbp,
                        ref itp, ref ibp,
                        ref clp, ref crp,
                        ref bitp, ref bibp, i);

                    if (i == 1)
                    {
                        UGL.AddVertToVertexHelper(vh, ltp, lbp, lineColor, false);
                    }

                    if (bitp == bibp)
                    {
                        if (bitp)
                            UGL.AddVertToVertexHelper(vh, itp, ibp, lineColor, true);
                        else
                        {
                            UGL.AddVertToVertexHelper(vh, ltp, clp, lineColor, true);
                            UGL.AddVertToVertexHelper(vh, ltp, crp, lineColor, true);
                        }
                    }
                    else
                    {
                        if (bitp)
                        {
                            UGL.AddVertToVertexHelper(vh, itp, clp, lineColor, true);
                            UGL.AddVertToVertexHelper(vh, itp, crp, lineColor, true);
                        }
                        else if (bibp)
                        {
                            UGL.AddVertToVertexHelper(vh, clp, ibp, lineColor, true);
                            UGL.AddVertToVertexHelper(vh, crp, ibp, lineColor, true);
                        }
                    }
                    lp = cp;
                }
            }

            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.RefreshChart();
            }
        }

        private void DrawPolarLineArrow(VertexHelper vh, Serie serie)
        {
            if (!serie.show || serie.lineArrow == null || !serie.lineArrow.show)
                return;

            if (serie.context.dataPoints.Count < 2)
                return;

            var lineColor = SerieHelper.GetLineColor(serie, null, chart.theme, serie.context.colorIndex);
            var startPos = Vector3.zero;
            var arrowPos = Vector3.zero;
            var lineArrow = serie.lineArrow.arrow;
            var dataPoints = serie.context.dataPoints;
            switch (serie.lineArrow.position)
            {
                case LineArrow.Position.End:
                    if (dataPoints.Count < 3)
                    {
                        startPos = dataPoints[dataPoints.Count - 2];
                        arrowPos = dataPoints[dataPoints.Count - 1];
                    }
                    else
                    {
                        startPos = dataPoints[dataPoints.Count - 3];
                        arrowPos = dataPoints[dataPoints.Count - 2];
                    }
                    UGL.DrawArrow(vh, startPos, arrowPos, lineArrow.width, lineArrow.height,
                        lineArrow.offset, lineArrow.dent, lineArrow.GetColor(lineColor));

                    break;

                case LineArrow.Position.Start:
                    startPos = dataPoints[1];
                    arrowPos = dataPoints[0];
                    UGL.DrawArrow(vh, startPos, arrowPos, lineArrow.width, lineArrow.height,
                        lineArrow.offset, lineArrow.dent, lineArrow.GetColor(lineColor));
                    break;
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
                float symbolBorder = 0f;
                float[] cornerRadius = null;
                Color32 symbolColor, symbolToColor, symbolEmptyColor, borderColor;
                for (int i = 0; i < count; i++)
                {
                    var serieData = serie.GetSerieData(i);
                    var state = SerieHelper.GetSerieState(serie, serieData, true);
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData, state);
                    if (ChartHelper.IsIngore(serieData.context.position))
                        continue;

                    if (!symbol.show || !symbol.ShowSymbol(i, count))
                        continue;

                    var symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, chart.theme.serie.lineSymbolSize, state);
                    SerieHelper.GetItemColor(out symbolColor, out symbolToColor, out symbolEmptyColor, serie, serieData, chart.theme, n);
                    SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, null, chart.theme, state);

                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, serieData.context.position,
                        symbolColor, symbolToColor, symbolEmptyColor, borderColor, symbol.gap, cornerRadius);
                }
            }
        }
    }
}