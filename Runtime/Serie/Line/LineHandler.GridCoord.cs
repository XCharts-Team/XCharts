using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// For grid coord
    /// </summary>
    internal sealed partial class LineHandler : SerieHandler<Line>
    {
        List<List<SerieData>> m_StackSerieData = new List<List<SerieData>>();
        private GridCoord m_SerieGrid;

        public override Vector3 GetSerieDataLabelOffset(SerieData serieData, LabelStyle label)
        {
            var invert = label.autoOffset &&
                SerieHelper.IsDownPoint(serie, serieData.index) &&
                (serie.areaStyle == null || !serie.areaStyle.show);
            if (invert)
            {
                var offset = label.GetOffset(serie.context.insideRadius);
                return new Vector3(offset.x, -offset.y, offset.z);
            }
            else
            {
                return label.GetOffset(serie.context.insideRadius);
            }
        }

        private void UpdateSerieGridContext()
        {
            if (m_SerieGrid == null)
                return;
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            var needCheck = (chart.isPointerInChart && m_SerieGrid.IsPointerEnter()) || m_LegendEnter;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    serie.highlight = false;
                    serie.ResetInteract();
                    foreach (var serieData in serie.data)
                        serieData.context.highlight = false;
                    if (SeriesHelper.IsStack(chart.series))
                        chart.RefreshTopPainter();
                    else
                        chart.RefreshPainter(serie);
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            var themeSymbolSize = chart.theme.serie.lineSymbolSize;
            var themeSymbolSelectedSize = chart.theme.serie.lineSymbolSelectedSize;
            var needInteract = false;
            if (m_LegendEnter)
            {
                serie.interact.SetValue(ref needInteract, lineWidth, true, chart.theme.serie.selectedRate);
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    var symbolSelectedSize = symbol.GetSelectedSize(serieData.data, themeSymbolSelectedSize);

                    serieData.context.highlight = true;
                    serieData.interact.SetValue(ref needInteract, symbolSelectedSize);
                }
            }
            else if (serie.context.isTriggerByAxis)
            {
                serie.context.pointerEnter = true;
                serie.interact.SetValue(ref needInteract, lineWidth, true, chart.theme.serie.selectedRate);
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);
                    var symbolSelectedSize = symbol.GetSelectedSize(serieData.data, themeSymbolSelectedSize);

                    if (i == serie.context.pointerItemDataIndex)
                    {
                        serieData.context.highlight = true;
                        serieData.interact.SetValue(ref needInteract, symbolSelectedSize);
                    }
                    else
                    {
                        serieData.context.highlight = false;
                        serieData.interact.SetValue(ref needInteract, symbolSize);
                    }
                }
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    serieData.index = i;
                    var dist = Vector3.Distance(chart.pointerPos, serieData.context.position);
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);
                    var symbolSelectedSize = symbol.GetSelectedSize(serieData.data, themeSymbolSelectedSize);
                    if (dist <= symbolSelectedSize)
                    {
                        serie.context.pointerItemDataIndex = serieData.index;
                        serie.context.pointerEnter = true;
                        serie.interact.SetValue(ref needInteract, lineWidth, true);
                        serieData.context.highlight = true;
                        serieData.interact.SetValue(ref needInteract, symbolSelectedSize);
                    }
                    else
                    {
                        serieData.context.highlight = false;
                        serieData.interact.SetValue(ref needInteract, symbolSize);
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

        private void DrawLinePoint(VertexHelper vh, Serie serie)
        {
            if (!serie.show || serie.IsPerformanceMode())
                return;

            if (m_SerieGrid == null)
                return;

            var count = serie.context.dataPoints.Count;
            var clip = SeriesHelper.IsAnyClipSerie(chart.series);
            var theme = chart.theme;
            var interacting = false;
            var lineArrow = serie.lineArrow;
            var visualMap = chart.GetVisualMapOfSerie(serie);
            var isVisualMapGradient = VisualMapHelper.IsNeedLineGradient(visualMap);

            Axis axis;
            Axis relativedAxis;
            chart.GetSerieGridCoordAxis(serie, out axis, out relativedAxis);

            for (int i = 0; i < count; i++)
            {
                var serieData = serie.GetSerieData(i);
                if (serieData == null)
                    continue;
                if (serieData.context.isClip)
                    continue;

                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);

                if (!symbol.show || !symbol.ShowSymbol(i, count))
                    continue;

                var pos = serie.context.dataPoints[i];
                if (lineArrow != null && lineArrow.show)
                {
                    if (lineArrow.position == LineArrow.Position.Start && i == 0)
                        continue;
                    if (lineArrow.position == LineArrow.Position.End && i == count - 1)
                        continue;
                }

                if (ChartHelper.IsIngore(pos))
                    continue;

                var highlight = serie.data[i].context.highlight || serie.highlight;
                var symbolSize = highlight ?
                    theme.serie.lineSymbolSelectedSize :
                    theme.serie.lineSymbolSize;
                if (!serieData.interact.TryGetValue(ref symbolSize, ref interacting))
                {
                    symbolSize = highlight ?
                        symbol.GetSelectedSize(serieData.data, symbolSize) :
                        symbol.GetSize(serieData.data, symbolSize);
                    serieData.interact.SetValue(ref interacting, symbolSize);
                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                }
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, theme, serie.index, highlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, theme, serie.index, highlight);
                var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, theme, serie.index, highlight, false);

                if (isVisualMapGradient)
                {
                    symbolColor = VisualMapHelper.GetLineGradientColor(visualMap, pos, m_SerieGrid, axis, relativedAxis, symbolColor);
                    symbolToColor = symbolColor;
                }
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var borderColor = SerieHelper.GetSymbolBorderColor(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                chart.DrawClipSymbol(vh, symbol.type, symbolSize, symbolBorder, pos,
                    symbolColor, symbolToColor, symbolEmptyColor, borderColor, symbol.gap, clip, cornerRadius, m_SerieGrid,
                    i > 0 ? serie.context.dataPoints[i - 1] : m_SerieGrid.context.position);
            }
            if (interacting)
            {
                if (SeriesHelper.IsStack(chart.series))
                    chart.RefreshTopPainter();
                else
                    chart.RefreshPainter(serie);
            }
        }

        private void DrawLineArrow(VertexHelper vh, Serie serie)
        {
            if (!serie.show || serie.lineArrow == null || !serie.lineArrow.show)
                return;

            if (serie.context.dataPoints.Count < 2)
                return;

            var lineColor = SerieHelper.GetLineColor(serie, null, chart.theme, serie.index, false);
            var startPos = Vector3.zero;
            var arrowPos = Vector3.zero;
            var lineArrow = serie.lineArrow.arrow;
            var dataPoints = serie.context.drawPoints;
            switch (serie.lineArrow.position)
            {
                case LineArrow.Position.End:
                    if (dataPoints.Count < 3)
                    {
                        startPos = dataPoints[dataPoints.Count - 2].position;
                        arrowPos = dataPoints[dataPoints.Count - 1].position;
                    }
                    else
                    {
                        startPos = dataPoints[dataPoints.Count - 3].position;
                        arrowPos = dataPoints[dataPoints.Count - 2].position;
                    }
                    UGL.DrawArrow(vh, startPos, arrowPos, lineArrow.width, lineArrow.height,
                        lineArrow.offset, lineArrow.dent, lineArrow.GetColor(lineColor));

                    break;

                case LineArrow.Position.Start:
                    startPos = dataPoints[1].position;
                    arrowPos = dataPoints[0].position;
                    UGL.DrawArrow(vh, startPos, arrowPos, lineArrow.width, lineArrow.height,
                        lineArrow.offset, lineArrow.dent, lineArrow.GetColor(lineColor));

                    break;
            }
        }

        private void DrawLineSerie(VertexHelper vh, Line serie)
        {
            if (!serie.show)
                return;
            if (serie.animation.HasFadeOut())
                return;

            Axis axis;
            Axis relativedAxis;
            var isY = chart.GetSerieGridCoordAxis(serie, out axis, out relativedAxis);

            if (axis == null)
                return;
            if (relativedAxis == null)
                return;

            m_SerieGrid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            if (m_SerieGrid == null)
                return;
            if (m_EndLabel != null && !m_SerieGrid.context.endLabelList.Contains(m_EndLabel))
            {
                m_SerieGrid.context.endLabelList.Add(m_EndLabel);
            }

            var visualMap = chart.GetVisualMapOfSerie(serie);
            var dataZoom = chart.GetDataZoomOfAxis(axis);
            var showData = serie.GetDataList(dataZoom);

            if (showData.Count <= 0)
                return;

            var axisLength = isY ? m_SerieGrid.context.height : m_SerieGrid.context.width;
            var scaleWid = AxisHelper.GetDataWidth(axis, axisLength, showData.Count, dataZoom);

            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;
            int rate = LineHelper.GetDataAverageRate(serie, m_SerieGrid, maxCount, false);
            var totalAverage = serie.sampleAverage > 0 ?
                serie.sampleAverage :
                DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();

            var interacting = false;
            var lineWidth = LineHelper.GetLineWidth(ref interacting, serie, chart.theme.serie.lineWidth);

            axis.context.scaleWidth = scaleWid;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;

            Serie lastSerie = null;
            var isStack = SeriesHelper.IsStack<Line>(chart.series, serie.stack);
            if (isStack)
            {
                lastSerie = SeriesHelper.GetLastStackSerie(chart.series, serie);
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);
            }
            var lp = Vector3.zero;
            for (int i = serie.minShow; i < maxCount; i += rate)
            {
                var serieData = showData[i];
                var isIgnore = serie.IsIgnoreValue(serieData);
                if (isIgnore)
                {
                    serieData.context.stackHeight = 0;
                    serieData.context.position = Vector3.zero;
                    if (serie.ignoreLineBreak && serie.context.dataIgnores.Count > 0)
                    {
                        serie.context.dataIgnores[serie.context.dataIgnores.Count - 1] = true;
                    }
                }
                else
                {
                    var np = Vector3.zero;
                    var xValue = axis.IsCategory() ? i : serieData.GetData(0, axis.inverse);
                    var relativedValue = DataHelper.SampleValue(ref showData, serie.sampleType, rate, serie.minShow,
                        maxCount, totalAverage, i, dataChangeDuration, ref dataChanging, relativedAxis);

                    serieData.context.stackHeight = GetDataPoint(isY, axis, relativedAxis, m_SerieGrid, xValue, relativedValue,
                        i, scaleWid, isStack, ref np);
                    serieData.context.isClip = false;
                    if (serie.clip && !m_SerieGrid.Contains(np))
                    {
                        if (m_SerieGrid.BoundaryPoint(lp, np, ref np))
                        {
                            serieData.context.isClip = true;
                        }
                    }
                    serie.context.dataIgnores.Add(false);
                    serieData.context.position = np;
                    serie.context.dataPoints.Add(np);
                    lp = np;
                }
            }

            if (dataChanging || interacting)
                chart.RefreshPainter(serie);

            if (serie.context.dataPoints.Count <= 0)
                return;

            serie.animation.InitProgress(serie.context.dataPoints, isY);

            VisualMapHelper.AutoSetLineMinMax(visualMap, serie, isY, axis, relativedAxis);
            LineHelper.UpdateSerieDrawPoints(serie, chart.settings, chart.theme, visualMap, lineWidth, isY);
            LineHelper.DrawSerieLineArea(vh, serie, lastSerie, chart.theme, visualMap, isY, axis, relativedAxis, m_SerieGrid);
            LineHelper.DrawSerieLine(vh, chart.theme, serie, visualMap, m_SerieGrid, axis, relativedAxis, lineWidth);

            serie.context.vertCount = vh.currentVertCount;

            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.RefreshPainter(serie);
            }
        }

        private float GetDataPoint(bool isY, Axis axis, Axis relativedAxis, GridCoord grid, double xValue,
            double yValue, int i, float scaleWid, bool isStack, ref Vector3 np)
        {
            float xPos, yPos;
            var gridXY = isY ? grid.context.x : grid.context.y;
            var valueHig = 0f;
            if (isY)
            {
                valueHig = AxisHelper.GetAxisValueDistance(grid, relativedAxis, scaleWid, yValue);
                valueHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, valueHig);

                xPos = gridXY + valueHig;
                yPos = AxisHelper.GetAxisValuePosition(grid, axis, scaleWid, xValue);

                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        xPos += m_StackSerieData[n][i].context.stackHeight;
                }
            }
            else
            {

                valueHig = AxisHelper.GetAxisValueDistance(grid, relativedAxis, scaleWid, yValue);
                valueHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, valueHig);

                yPos = gridXY + valueHig;
                xPos = AxisHelper.GetAxisValuePosition(grid, axis, scaleWid, xValue);

                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        yPos += m_StackSerieData[n][i].context.stackHeight;
                }
            }
            np = new Vector3(xPos, yPos);
            return valueHig;
        }
    }
}