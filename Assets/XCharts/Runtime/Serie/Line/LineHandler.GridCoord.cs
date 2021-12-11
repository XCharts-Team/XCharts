/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    /// <summary>
    /// For grid coord
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal sealed partial class LineHandler : SerieHandler<Line>
    {
        List<List<SerieData>> m_StackSerieData = new List<List<SerieData>>();

        public override void Update()
        {
            base.Update();
            UpdateSerieContext();
        }

        public override bool SetDefaultTooltipContent(Tooltip tooltip, StringBuilder sb)
        {
            var dataIndex = serie.context.pointerItemDataIndex;
            if (!serie.context.pointerEnter || dataIndex < 0)
                return false;
            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return false;

            double xValue, yValue;
            serie.GetXYData(dataIndex, null, out xValue, out yValue);

            var isIngore = serie.IsIgnorePoint(dataIndex);
            var numericFormatter = TooltipHelper.GetItemNumericFormatter(tooltip, serie, serieData);
            var valueTxt = isIngore
                ? tooltip.ignoreDataDefaultContent
                : ChartCached.FloatToStr(yValue, numericFormatter);

            switch (tooltip.trigger)
            {
                case Tooltip.Trigger.Item:

                    var category = string.Empty;
                    var xAxis = chart.GetChartComponent<XAxis>();
                    var yAxis = chart.GetChartComponent<YAxis>();
                    if (yAxis.IsCategory())
                    {
                        category = yAxis.GetData((int)yAxis.context.pointerValue);
                    }
                    else
                    {
                        category = xAxis.IsCategory()
                            ? xAxis.GetData((int)xAxis.context.pointerValue)
                            : (isIngore
                                ? tooltip.ignoreDataDefaultContent
                                : ChartCached.FloatToStr(xValue, numericFormatter));
                    }
                    if (!string.IsNullOrEmpty(serie.serieName))
                        sb.Append(serie.serieName).Append(FormatterHelper.PH_NN);

                    sb.Append("<color=#")
                        .Append(chart.theme.GetColorStr(serie.index))
                        .Append(">● </color>");

                    if (!string.IsNullOrEmpty(category))
                        sb.Append(category).Append(":");
                    sb.Append(valueTxt);
                    break;

                case Tooltip.Trigger.Axis:

                    sb.Append("<color=#")
                        .Append(chart.theme.GetColorStr(serie.index))
                        .Append(">● </color>");

                    if (!string.IsNullOrEmpty(serie.serieName))
                        sb.Append(serie.serieName).Append(":");

                    sb.Append(valueTxt);
                    break;
            }
            return true;
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (serie.IsUseCoord<PolarCoord>())
            {
                DrawPolarLine(vh, serie);
                DrawPolarLineSymbol(vh);
            }
            else if (serie.IsUseCoord<GridCoord>())
            {
                DrawLineSerie(vh, serie);

                if (!SeriesHelper.IsStack(chart.series))
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
        }

        public override void DrawTop(VertexHelper vh)
        {
            if (serie.IsUseCoord<GridCoord>())
            {
                if (SeriesHelper.IsStack(chart.series))
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
        }

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart)
                return;

            var themeSymbolSize = chart.theme.serie.lineSymbolSize;

            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;

            foreach (var serieData in serie.data)
            {
                var dist = Vector3.Distance(chart.pointerPos, serieData.context.position);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);

                if (dist <= symbolSize)
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    serie.context.pointerEnter = true;
                    serieData.context.highlight = true;
                    chart.RefreshTopPainter();
                }
                else
                {
                    serieData.context.highlight = false;
                }
            }
        }

        private void DrawLinePoint(VertexHelper vh, Serie serie)
        {
            if (!serie.show || serie.IsPerformanceMode())
                return;

            var count = serie.context.dataPoints.Count;
            var clip = SeriesHelper.IsAnyClipSerie(chart.series);
            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;

            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex))
                return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex))
                return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex))
                return;

            var theme = chart.theme;
            for (int i = 0; i < count; i++)
            {
                var serieData = serie.GetSerieData(i);
                if (serieData == null)
                    continue;

                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);

                if (!symbol.show || !symbol.ShowSymbol(i, count))
                    continue;

                if (serie.lineArrow.show)
                {
                    if (serie.lineArrow.position == LineArrow.Position.Start && i == 0)
                        continue;
                    if (serie.lineArrow.position == LineArrow.Position.End && i == count - 1)
                        continue;
                }

                if (ChartHelper.IsIngore(serie.context.dataPoints[i]))
                    continue;

                var highlight = serie.data[i].context.highlight || serie.highlight;
                var symbolSize = highlight
                    ? symbol.GetSelectedSize(serie.data[i].data, theme.serie.lineSymbolSelectedSize)
                    : symbol.GetSize(serie.data[i].data, theme.serie.lineSymbolSize);
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, theme, serie.index, highlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, theme, serie.index, highlight);
                var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, theme, serie.index, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);

                symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                chart.DrawClipSymbol(vh, symbol.type, symbolSize, symbolBorder, serie.context.dataPoints[i],
                    symbolColor, symbolToColor, symbolEmptyColor, symbol.gap, clip, cornerRadius, grid,
                    i > 0 ? serie.context.dataPoints[i - 1] : grid.context.position);
            }
        }

        private void DrawLineArrow(VertexHelper vh, Serie serie)
        {
            if (!serie.show || !serie.lineArrow.show)
                return;

            if (serie.context.dataPoints.Count < 2)
                return;

            var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serie.index, false);
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

            var isY = ComponentHelper.IsAnyCategoryOfYAxis(chart.components);

            Axis axis;
            Axis relativedAxis;
            GridCoord grid;

            if (isY)
            {
                axis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
                relativedAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
            }
            else
            {
                axis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
                relativedAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
            }
            grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);

            if (axis == null)
                return;
            if (relativedAxis == null)
                return;
            if (grid == null)
                return;

            var visualMap = chart.GetVisualMapOfSerie(serie);
            var dataZoom = chart.GetDataZoomOfAxis(axis);
            var showData = serie.GetDataList(dataZoom);

            if (showData.Count <= 0)
                return;

            var axisLength = isY ? grid.context.height : grid.context.width;
            var scaleWid = AxisHelper.GetDataWidth(axis, axisLength, showData.Count, dataZoom);

            int maxCount = serie.maxShow > 0
                ? (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int rate = LineHelper.GetDataAverageRate(serie, grid, maxCount, false);
            var totalAverage = serie.sampleAverage > 0
                ? serie.sampleAverage
                : DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();

            axis.context.scaleWidth = scaleWid;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;

            Serie lastSerie = null;
            var isStack = SeriesHelper.IsStack<Line>(chart.series, serie.stack);
            if (isStack)
            {
                lastSerie = SeriesHelper.GetLastStackSerie(chart.series, serie);
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);
            }

            for (int i = serie.minShow; i < maxCount; i += rate)
            {
                var serieData = showData[i];
                var isIgnore = serie.IsIgnoreValue(serieData);
                if (isIgnore)
                {
                    serieData.context.stackHeight = 0;
                    serieData.context.position = Vector3.zero;
                    if (serie.ignoreLineBreak && serie.context.dataIgnore.Count > 0)
                    {
                        serie.context.dataIgnore[serie.context.dataIgnore.Count - 1] = true;
                    }
                }
                else
                {
                    var np = Vector3.zero;
                    var xValue = axis.IsCategory() ? i : serieData.GetData(0, axis.inverse);
                    var relativedValue = DataHelper.SampleValue(ref showData, serie.sampleType, rate, serie.minShow,
                        maxCount, totalAverage, i, dataChangeDuration, ref dataChanging, relativedAxis);

                    serieData.context.stackHeight = GetDataPoint(isY, axis, relativedAxis, grid, xValue, relativedValue,
                        i, scaleWid, isStack, ref np);

                    serieData.context.position = np;

                    serie.context.dataPoints.Add(np);
                    serie.context.dataIgnore.Add(false);
                }
            }

            if (dataChanging)
                chart.RefreshPainter(serie);

            if (serie.context.dataPoints.Count <= 0)
                return;

            serie.animation.InitProgress(serie.context.dataPoints, isY);
            serie.animation.SetDataFinish(0);

            VisualMapHelper.AutoSetLineMinMax(visualMap, serie, isY, axis, relativedAxis);
            LineHelper.UpdateSerieDrawPoints(serie, chart.settings, chart.theme, isY);
            LineHelper.DrawSerieLineArea(vh, serie, lastSerie, chart.theme, isY, axis, relativedAxis, grid);
            LineHelper.DrawSerieLine(vh, chart.theme, serie, visualMap, grid, axis, relativedAxis);

            serie.context.vertCount = vh.currentVertCount;

            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
        }

        private float GetDataPoint(bool isY, Axis axis, Axis relativedAxis, GridCoord grid, double xValue,
            double yValue, int i, float scaleWid, bool isStack, ref Vector3 np)
        {
            float xPos, yPos;

            if (isY)
            {
                xPos = AxisHelper.GetAxisPosition(grid, relativedAxis, scaleWid, yValue);
                yPos = AxisHelper.GetAxisPosition(grid, axis, scaleWid, xValue);
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        yPos += m_StackSerieData[n][i].context.stackHeight;
                }
            }
            else
            {
                xPos = AxisHelper.GetAxisPosition(grid, axis, scaleWid, xValue);
                yPos = AxisHelper.GetAxisPosition(grid, relativedAxis, scaleWid, yValue);
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        xPos += m_StackSerieData[n][i].context.stackHeight;
                }
            }
            np = new Vector3(xPos, yPos);
            return yPos;
        }
    }
}