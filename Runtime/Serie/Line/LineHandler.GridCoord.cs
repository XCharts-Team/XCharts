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
        private List<double> m_SampleSumPrefixCache;
        private int m_SampleSumPrefixMaxCount = -1;
        private bool m_SampleSumPrefixInverse = false;

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
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            var themeSymbolSize = chart.theme.serie.lineSymbolSize;
            var symbolVisible = serie.symbol != null && serie.symbol.show && serie.symbol.type != SymbolType.None;
            var needInteract = false;
            serie.ResetDataIndex();
            if (m_LegendEnter)
            {
                serie.context.pointerEnter = true;
                serie.interact.SetValue(ref needInteract, serie.animation.interaction.GetWidth(lineWidth));
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    serieData.context.highlight = true;
                    if (symbolVisible)
                    {
                        var size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, SerieState.Emphasis);
                        serieData.interact.SetValue(ref needInteract, size);
                    }
                }
            }
            else if (serie.context.isTriggerByAxis)
            {
                serie.context.pointerEnter = false;
                serie.interact.SetValue(ref needInteract, serie.animation.interaction.GetWidth(lineWidth));
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var highlight = i == serie.context.pointerItemDataIndex;
                    serieData.context.highlight = highlight;
                    if (symbolVisible)
                    {
                        var state = SerieHelper.GetSerieState(serie, serieData, true);
                        var size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, state);
                        serieData.interact.SetValue(ref needInteract, size);
                    }
                    if (highlight)
                    {
                        serie.context.pointerEnter = true;
                        serie.context.pointerItemDataIndex = i;
                        needInteract = true;
                    }
                }
            }
            else
            {
                var lastIndex = serie.context.pointerItemDataIndex;
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var pointerOffset = (Vector2)chart.pointerPos - (Vector2)serieData.context.position;
                    bool highlight;
                    if (symbolVisible)
                    {
                        var size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize);
                        var radius = size * 2.5f;
                        highlight = pointerOffset.sqrMagnitude <= radius * radius;
                        var state = SerieHelper.GetSerieState(serie, serieData, true);
                        size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, state);
                        serieData.interact.SetValue(ref needInteract, size);
                    }
                    else
                    {
                        var radius = themeSymbolSize * 2.5f;
                        highlight = pointerOffset.sqrMagnitude <= radius * radius;
                    }
                    serieData.context.highlight = highlight;
                    if (highlight)
                    {
                        serie.context.pointerEnter = true;
                        serie.context.pointerItemDataIndex = serieData.index;
                    }
                }
                if (lastIndex != serie.context.pointerItemDataIndex)
                {
                    needInteract = true;
                }
                if (serie.context.pointerItemDataIndex >= 0)
                    serie.interact.SetValue(ref needInteract, serie.animation.interaction.GetWidth(lineWidth));
                else
                    serie.interact.SetValue(ref needInteract, lineWidth);
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
            var interactDuration = serie.animation.GetInteractionDuration();

            Axis axis;
            Axis relativedAxis;
            chart.GetSerieGridCoordAxis(serie, out axis, out relativedAxis);

            for (int i = 0; i < count; i++)
            {
                var index = serie.context.dataIndexs[i];
                var serieData = serie.GetSerieData(index);
                if (serieData == null)
                    continue;
                if (serieData.context.isClip)
                    continue;
                var state = SerieHelper.GetSerieState(serie, serieData, true);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData, state);

                if (!symbol.show || !symbol.ShowSymbol(index, count))
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

                var symbolSize = 0f;
                if (!serieData.interact.TryGetValue(ref symbolSize, ref interacting, interactDuration))
                {
                    symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme.serie.lineSymbolSize, state);
                    serieData.interact.SetValue(ref interacting, symbolSize);
                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                }
                float symbolBorder = 0f;
                float[] cornerRadius = null;
                Color32 symbolColor, symbolToColor, symbolEmptyColor, borderColor;
                SerieHelper.GetItemColor(out symbolColor, out symbolToColor, out symbolEmptyColor, serie, serieData, theme, serie.context.colorIndex);
                SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, null, chart.theme, state);
                if (isVisualMapGradient)
                {
                    symbolColor = VisualMapHelper.GetLineGradientColor(visualMap, pos, m_SerieGrid, axis, relativedAxis, symbolColor);
                    symbolToColor = symbolColor;
                }
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

            var lineColor = SerieHelper.GetLineColor(serie, null, chart.theme, serie.context.colorIndex);
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
            var axisRelativedLength = isY ? m_SerieGrid.context.width : m_SerieGrid.context.height;

            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;
            maxCount -= serie.context.dataZoomStartIndexOffset;
            var scaleWid = AxisHelper.GetDataWidth(axis, axisLength, maxCount, dataZoom);
            var scaleRelativedWid = AxisHelper.GetDataWidth(relativedAxis, axisRelativedLength, maxCount, dataZoom);
            int rate = LineHelper.GetDataAverageRate(serie, axisLength, maxCount, false);
            var totalAverage = serie.sampleAverage > 0 ?
                serie.sampleAverage :
                DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetChangeDuration();
            var unscaledTime = serie.animation.unscaledTime;
            var dataAddDuration = 0f;
            var useCurrentData = false;
            List<double> sampleSumPrefix = null;
            if (serie.animation.enable)
            {
                dataAddDuration = serie.animation.GetAdditionDuration();
                useCurrentData = DataHelper.IsAnyDataChanged(ref showData, serie.minShow, showData.Count);
                dataChanging = useCurrentData;
            }
            if (!useCurrentData && rate > 1 &&
                (serie.sampleType == SampleType.Sum || serie.sampleType == SampleType.Average))
            {
                if (m_SampleSumPrefixCache == null || m_SampleSumPrefixMaxCount != showData.Count ||
                    m_SampleSumPrefixInverse != relativedAxis.inverse)
                {
                    m_SampleSumPrefixCache = DataHelper.BuildSampleSumPrefix(ref showData, showData.Count,
                        relativedAxis.inverse, m_SampleSumPrefixCache);
                    m_SampleSumPrefixMaxCount = showData.Count;
                    m_SampleSumPrefixInverse = relativedAxis.inverse;
                }
                sampleSumPrefix = m_SampleSumPrefixCache;
            }

            var interacting = false;
            var lineWidth = LineHelper.GetLineWidth(ref interacting, serie, chart.theme.serie.lineWidth);

            axis.context.scaleWidth = scaleWid;
            serie.context.isHorizontal = isY;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;

            Serie lastSerie = null;
            var isStack = SeriesHelper.IsStack<Line>(chart.series, serie.stack);
            if (isStack)
            {
                lastSerie = SeriesHelper.GetLastStackSerie(chart.series, serie);
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);
            }

            // Pre-compute all invariant grid/axis values for the hot loop.
            // Each GetDataPoint previously called GetAxisPositionInternal twice,
            // which redundantly checked axis types and read grid.context fields.
            var gridX = m_SerieGrid.context.x;
            var gridY = m_SerieGrid.context.y;
            var gridWidth = m_SerieGrid.context.width;
            var gridHeight = m_SerieGrid.context.height;

            // relativedAxis: value axis for distance/length computation
            // NOTE: relGridLength uses gridHeight when relativedAxis is YAxis, gridWidth otherwise.
            // This matches GetAxisPositionInternal's "isY ? grid.context.height : grid.context.width" logic.
            var relIsYAxis = relativedAxis is YAxis;
            var relGridLength = relIsYAxis ? gridHeight : gridWidth;
            var relMinValue = (float)relativedAxis.context.minValue;
            var relMinMaxRange = (float)relativedAxis.context.minMaxRange;
            var relHasRange = relMinMaxRange != 0;

            // axis: category or value axis for position computation
            var axisIsYAxis = axis is YAxis;
            var axisIsCategory = axis.IsCategory();
            var axisGridXY = axisIsYAxis ? gridY : gridX;
            var axisGridLength = axisIsYAxis ? gridHeight : gridWidth;
            var axisMinValue = (float)axis.context.minValue;
            var axisMinMaxRange = (float)axis.context.minMaxRange;
            var axisHasRange = axisMinMaxRange != 0;
            var boundaryOffset = axis.boundaryGap ? scaleWid * 0.5f : 0f;

            // gridXY for the value-axis orientation in GetDataPoint
            var dpGridXY = isY ? gridX : gridY;

            // stack count (invariant within loop)
            var stackCount = isStack ? m_StackSerieData.Count - 1 : 0;

            var lp = Vector3.zero;
            for (int i = serie.minShow; i < showData.Count; i += rate)
            {
                var serieData = showData[i];
                var realIndex = i - serie.context.dataZoomStartIndexOffset;
                var isIgnore = serie.IsIgnoreValue(serieData);
                var xValue = axisIsCategory ? realIndex : serieData.GetData(0, axis.inverse);
                var np = Vector3.zero;
                var nextIndex = Mathf.Min(i + rate, showData.Count);
                if (isIgnore)
                {
                    var relativedValue = 1d;
                    ComputeDataPointFast(isY, xValue, relativedValue, i,
                        scaleWid, dpGridXY,
                        relGridLength, relMinValue, relMinMaxRange, relHasRange,
                        axisIsCategory, axisGridXY, axisGridLength,
                        axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                        isStack, stackCount, ref np);
                    serieData.context.stackHeight = 0;
                    serieData.context.position = np;
                    for (int j = i + 1; j < nextIndex; j++)
                    {
                        var skipData = showData[j];
                        var skipRealIndex = j - serie.context.dataZoomStartIndexOffset;
                        var skipXValue = axisIsCategory ? skipRealIndex : skipData.GetData(0, axis.inverse);
                        var skipNp = Vector3.zero;
                        var skipStackHeight = ComputeDataPointFast(isY, skipXValue, relativedValue, j,
                            scaleWid, dpGridXY,
                            relGridLength, relMinValue, relMinMaxRange, relHasRange,
                            axisIsCategory, axisGridXY, axisGridLength,
                            axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                            isStack, stackCount, ref skipNp);
                        skipData.context.stackHeight = serie.IsIgnoreValue(skipData) ? 0 : skipStackHeight;
                        skipData.context.position = skipNp;
                    }
                    if (serie.ignoreLineBreak && serie.context.dataIgnores.Count > 0)
                    {
                        serie.context.dataIgnores[serie.context.dataIgnores.Count - 1] = true;
                    }
                }
                else
                {
                    var relativedValue = DataHelper.SampleValue(ref showData, serie.sampleType, rate, serie.minShow,
                        maxCount, totalAverage, i, dataAddDuration, dataChangeDuration, ref dataChanging, relativedAxis,
                        unscaledTime, useCurrentData, false, sampleSumPrefix);
                    serieData.context.stackHeight = ComputeDataPointFast(isY, xValue, relativedValue, i,
                        scaleWid, dpGridXY,
                        relGridLength, relMinValue, relMinMaxRange, relHasRange,
                        axisIsCategory, axisGridXY, axisGridLength,
                        axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                        isStack, stackCount, ref np);
                    serieData.context.isClip = false;
                    if (serie.clip && !m_SerieGrid.Contains(np))
                    {
                        //if (m_SerieGrid.BoundaryPoint(lp, np, ref np))
                        {
                            serieData.context.isClip = true;
                        }
                    }
                    serie.context.dataIgnores.Add(false);
                    serieData.context.position = np;
                    serie.context.dataPoints.Add(np);
                    serie.context.dataIndexs.Add(serieData.index);
                    lp = np;
                    for (int j = i + 1; j < nextIndex; j++)
                    {
                        var skipData = showData[j];
                        var skipRealIndex = j - serie.context.dataZoomStartIndexOffset;
                        var skipXValue = axisIsCategory ? skipRealIndex : skipData.GetData(0, axis.inverse);
                        var skipNp = Vector3.zero;
                        var skipStackHeight = ComputeDataPointFast(isY, skipXValue, relativedValue, j,
                            scaleWid, dpGridXY,
                            relGridLength, relMinValue, relMinMaxRange, relHasRange,
                            axisIsCategory, axisGridXY, axisGridLength,
                            axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                            isStack, stackCount, ref skipNp);
                        skipData.context.stackHeight = serie.IsIgnoreValue(skipData) ? 0 : skipStackHeight;
                        skipData.context.position = skipNp;
                    }
                }
            }

            if (dataChanging || interacting)
                chart.RefreshPainter(serie);

            if (serie.context.dataPoints.Count <= 0)
                return;

            serie.animation.InitProgress(serie.context.dataPoints, isY);

            VisualMapHelper.AutoSetLineMinMax(visualMap, serie, isY, axis, relativedAxis);
            LineHelper.UpdateSerieDrawPoints(serie, chart.settings, chart.theme, visualMap, lineWidth, isY, m_SerieGrid);
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

        /// <summary>
        /// Fast path for computing a data point position and stack height.
        /// All invariant axis/grid parameters are pre-computed outside the hot loop.
        /// Returns the axis value length (equivalent to AxisHelper.GetAxisValueLength),
        /// matching the original GetDataPoint return value.
        /// </summary>
        private float ComputeDataPointFast(bool isY, double xValue, double yValue, int dataIndex,
            float scaleWid, float dpGridXY,
            float relGridLength, float relMinValue, float relMinMaxRange, bool relHasRange,
            bool axisIsCategory, float axisGridXY, float axisGridLength,
            float axisMinValue, float axisMinMaxRange, bool axisHasRange, float boundaryOffset,
            bool isStack, int stackCount, ref Vector3 np)
        {
            // distance along the value axis
            float valueHig = 0f;
            if (relHasRange)
                valueHig = (float)((yValue - relMinValue) / relMinMaxRange * relGridLength);

            valueHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, dataIndex, valueHig);

            // axis value length (for stack height / return value, equivalent to GetAxisValueLength)
            float valueLength = 0f;
            if (relHasRange)
                valueLength = (float)(yValue / relMinMaxRange * relGridLength);

            float xPos, yPos;
            if (isY)
            {
                xPos = dpGridXY + valueHig;
                if (axisIsCategory)
                    yPos = axisGridXY + boundaryOffset + scaleWid * (float)xValue;
                else if (axisHasRange)
                    yPos = axisGridXY + (float)((xValue - axisMinValue) / axisMinMaxRange * axisGridLength);
                else
                    yPos = axisGridXY;

                if (isStack)
                {
                    for (int n = 0; n < stackCount; n++)
                        xPos += m_StackSerieData[n][dataIndex].context.stackHeight;
                }
            }
            else
            {
                yPos = dpGridXY + valueHig;
                if (axisIsCategory)
                    xPos = axisGridXY + boundaryOffset + scaleWid * (float)xValue;
                else if (axisHasRange)
                    xPos = axisGridXY + (float)((xValue - axisMinValue) / axisMinMaxRange * axisGridLength);
                else
                    xPos = axisGridXY;

                if (isStack)
                {
                    for (int n = 0; n < stackCount; n++)
                        yPos += m_StackSerieData[n][dataIndex].context.stackHeight;
                }
            }
            np = new Vector3(xPos, yPos);
            return valueLength;
        }
    }
}