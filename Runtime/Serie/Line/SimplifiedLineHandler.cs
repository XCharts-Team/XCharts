using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// For grid coord
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal sealed class SimplifiedLineHandler : SerieHandler<SimplifiedLine>
    {
        private GridCoord m_SerieGrid;
        private List<double> m_SampleSumPrefixCache;
        private int m_SampleSumPrefixMaxCount = -1;
        private bool m_SampleSumPrefixInverse = false;
        // cache for IsAnyDataChanged to avoid scanning all data every frame
        private int m_LastDataVersion = -1;
        private bool m_LastAnyDataChanged = false;

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            UpdateCoordSerieParams(ref paramList, ref title, dataIndex, showCategory, category,
                marker, itemFormatter, numericFormatter, ignoreDataDefaultContent);
        }

        public override void DrawSerie(VertexHelper vh)
        {
            DrawLineSerie(vh, serie);
        }

        public override void UpdateSerieContext()
        {
            if (m_SerieGrid == null)
                return;

            var needCheck = (chart.isPointerInChart && m_SerieGrid.IsPointerEnter()) || m_LegendEnter;
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
                    var symbolVisible1 = serie.symbol != null && serie.symbol.show && serie.symbol.type != SymbolType.None;
                    foreach (var serieData in serie.data)
                    {
                        serieData.context.highlight = false;
                        if (symbolVisible1)
                        {
                            var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                            var symbolSize = symbol.GetSize(serieData, chart.theme.serie.lineSymbolSize);
                            serieData.interact.SetValue(ref needAnimation1, symbolSize);
                        }
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
            var symbolVisible = serie.symbol != null && serie.symbol.show && serie.symbol.type != SymbolType.None;

            var needInteract = false;
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
                serie.context.pointerEnter = true;
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
                        highlight = pointerOffset.sqrMagnitude <= size * size;
                        var state = SerieHelper.GetSerieState(serie, serieData, true);
                        size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, state);
                        serieData.interact.SetValue(ref needInteract, size);
                    }
                    else
                    {
                        highlight = pointerOffset.sqrMagnitude <= themeSymbolSize * themeSymbolSize;
                    }
                    serieData.context.highlight = highlight;
                    if (highlight)
                    {
                        serie.context.pointerEnter = true;
                        serie.context.pointerItemDataIndex = serieData.index;
                        serie.interact.SetValue(ref needInteract, serie.animation.interaction.GetWidth(lineWidth));
                    }
                }
                if (lastIndex != serie.context.pointerItemDataIndex)
                    needInteract = true;
            }
            if (needInteract)
            {
                if (SeriesHelper.IsStack(chart.series))
                    chart.RefreshTopPainter();
                else
                    chart.RefreshPainter(serie);
            }
        }

        private void DrawLineSerie(VertexHelper vh, SimplifiedLine serie)
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

            var dataZoom = chart.GetDataZoomOfAxis(axis);
            var showData = serie.GetDataList(dataZoom);

            if (showData.Count <= 0)
                return;

            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;

            var axisLength = isY ? m_SerieGrid.context.height : m_SerieGrid.context.width;
            var axisRelativedLength = isY ? m_SerieGrid.context.width : m_SerieGrid.context.height;
            var scaleWid = AxisHelper.GetDataWidth(axis, axisLength, maxCount, dataZoom);
            var scaleRelativedWid = AxisHelper.GetDataWidth(relativedAxis, axisRelativedLength, maxCount, dataZoom);

            int rate = LineHelper.GetDataAverageRate(serie, axisLength, maxCount, false);
            var totalAverage = serie.sampleAverage > 0 ?
                serie.sampleAverage :
                DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetChangeDuration();
            var dataAddDuration = serie.animation.GetAdditionDuration();
            var unscaledTime = serie.animation.unscaledTime;
            var useCurrentData = false;
            List<double> sampleSumPrefix = null;
            if (serie.animation.enable)
            {
                // Only re-scan all data for changes when data version has changed
                // or when previously animating (to detect animation completion).
                if (serie.m_DataVersion != m_LastDataVersion || m_LastAnyDataChanged)
                {
                    m_LastDataVersion = serie.m_DataVersion;
                    m_LastAnyDataChanged = DataHelper.IsAnyDataChanged(ref showData, serie.minShow, maxCount);
                }
                useCurrentData = m_LastAnyDataChanged;
                dataChanging = useCurrentData;
            }
            if (!useCurrentData && rate > 1 &&
                (serie.sampleType == SampleType.Sum || serie.sampleType == SampleType.Average))
            {
                if (m_SampleSumPrefixCache == null || m_SampleSumPrefixMaxCount != maxCount ||
                    m_SampleSumPrefixInverse != relativedAxis.inverse)
                {
                    m_SampleSumPrefixCache = DataHelper.BuildSampleSumPrefix(ref showData, maxCount,
                        relativedAxis.inverse, m_SampleSumPrefixCache);
                    m_SampleSumPrefixMaxCount = maxCount;
                    m_SampleSumPrefixInverse = relativedAxis.inverse;
                }
                sampleSumPrefix = m_SampleSumPrefixCache;
            }

            var interacting = false;
            var lineWidth = LineHelper.GetLineWidth(ref interacting, serie, chart.theme.serie.lineWidth);

            axis.context.scaleWidth = scaleWid;
            relativedAxis.context.scaleWidth = scaleRelativedWid;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;

            var needDataAnimation = serie.animation.IsDataAnimation() && !serie.animation.IsFinish();

            // Pre-compute all invariant axis/grid values for the hot loop.
            // Each GetDataPoint call previously did 2x GetAxisPositionInternal which redundantly
            // checked axis types and read grid.context fields (all invariant within the loop).
            var gridX = m_SerieGrid.context.x;
            var gridY = m_SerieGrid.context.y;
            var gridWidth = m_SerieGrid.context.width;
            var gridHeight = m_SerieGrid.context.height;

            // relativedAxis: value axis for distance computation
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

            // Cache ignore-related fields to avoid repeated Serie.IsIgnoreValue()
            // method dispatch chain (3 dispatches per call) in the hot loop.
            var serieIgnore = serie.ignore;
            var serieIgnoreValue = serie.ignoreValue;

            // Cache axis.inverse to avoid repeated method calls in GetData(0, inverse).
            // Also allows direct data[index] access bypassing GetData() bounds checks.
            var axisInverse = axis.inverse;

            for (int i = serie.minShow; i < maxCount; i += rate)
            {
                var serieData = showData[i];

                // Inline ignore check: eliminates method dispatch + property getter overhead.
                // When serieIgnore=false, the outer if is skipped entirely (zero cost).
                bool isIgnore = false;
                if (serieIgnore)
                {
                    if (serieData.ignore)
                        isIgnore = true;
                    else
                    {
                        var dv = serieData.data[1];
                        isIgnore = serieIgnoreValue == 0 ? dv == 0 : MathUtil.Approximately(dv, serieIgnoreValue);
                    }
                }

                var np = Vector3.zero;
                var xValue = axisIsCategory ? i :
                    (axisInverse ? -serieData.data[0] : serieData.data[0]);
                var nextIndex = Mathf.Min(i + rate, maxCount);
                if (isIgnore)
                {
                    var relativedValue = 1d;
                    ComputeDataPointFast(isY, xValue, relativedValue, i,
                        scaleWid, needDataAnimation, dpGridXY,
                        relGridLength, relMinValue, relMinMaxRange, relHasRange,
                        axisIsCategory, axisGridXY, axisGridLength,
                        axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                        ref np);
                    serieData.context.stackHeight = 0;
                    serieData.context.position = np;
                    for (int j = i + 1; j < nextIndex; j++)
                    {
                        var skipData = showData[j];
                        var skipNp = Vector3.zero;
                        var skipXValue = axisIsCategory ? j :
                            (axisInverse ? -skipData.data[0] : skipData.data[0]);
                        var skipStackHeight = ComputeDataPointFast(isY, skipXValue, relativedValue, j,
                            scaleWid, needDataAnimation, dpGridXY,
                            relGridLength, relMinValue, relMinMaxRange, relHasRange,
                            axisIsCategory, axisGridXY, axisGridLength,
                            axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                            ref skipNp);

                        bool skipIgnored = false;
                        if (serieIgnore)
                        {
                            if (skipData.ignore)
                                skipIgnored = true;
                            else
                            {
                                var sv = skipData.data[1];
                                skipIgnored = serieIgnoreValue == 0 ? sv == 0 : MathUtil.Approximately(sv, serieIgnoreValue);
                            }
                        }
                        skipData.context.stackHeight = skipIgnored ? 0 : skipStackHeight;
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
                        scaleWid, needDataAnimation, dpGridXY,
                        relGridLength, relMinValue, relMinMaxRange, relHasRange,
                        axisIsCategory, axisGridXY, axisGridLength,
                        axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                        ref np);
                    serieData.context.position = np;
                    serie.context.dataPoints.Add(np);
                    serie.context.dataIndexs.Add(serieData.index);
                    serie.context.dataIgnores.Add(false);
                    for (int j = i + 1; j < nextIndex; j++)
                    {
                        var skipData = showData[j];
                        var skipNp = Vector3.zero;
                        var skipXValue = axisIsCategory ? j :
                            (axisInverse ? -skipData.data[0] : skipData.data[0]);
                        var skipStackHeight = ComputeDataPointFast(isY, skipXValue, relativedValue, j,
                            scaleWid, needDataAnimation, dpGridXY,
                            relGridLength, relMinValue, relMinMaxRange, relHasRange,
                            axisIsCategory, axisGridXY, axisGridLength,
                            axisMinValue, axisMinMaxRange, axisHasRange, boundaryOffset,
                            ref skipNp);

                        bool skipIgnored = false;
                        if (serieIgnore)
                        {
                            if (skipData.ignore)
                                skipIgnored = true;
                            else
                            {
                                var sv = skipData.data[1];
                                skipIgnored = serieIgnoreValue == 0 ? sv == 0 : MathUtil.Approximately(sv, serieIgnoreValue);
                            }
                        }
                        skipData.context.stackHeight = skipIgnored ? 0 : skipStackHeight;
                        skipData.context.position = skipNp;
                    }
                }
            }

            if (dataChanging || interacting)
                chart.RefreshPainter(serie);

            if (serie.context.dataPoints.Count <= 0)
                return;

            serie.animation.InitProgress(serie.context.dataPoints, isY);

            LineHelper.UpdateSerieDrawPoints(serie, chart.settings, chart.theme, null, lineWidth, isY, m_SerieGrid);
            LineHelper.DrawSerieLineArea(vh, serie, null, chart.theme, null, isY, axis, relativedAxis, m_SerieGrid);
            LineHelper.DrawSerieLine(vh, chart.theme, serie, null, m_SerieGrid, axis, relativedAxis, lineWidth);

            serie.context.vertCount = vh.currentVertCount;

            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
                chart.RefreshPainter(serie);
            }
        }

        private float ComputeDataPointFast(bool isY, double xValue, double yValue, int dataIndex,
            float scaleWid, bool needDataAnimation, float dpGridXY,
            float relGridLength, float relMinValue, float relMinMaxRange, bool relHasRange,
            bool axisIsCategory, float axisGridXY, float axisGridLength,
            float axisMinValue, float axisMinMaxRange, bool axisHasRange, float boundaryOffset,
            ref Vector3 np)
        {
            // distance along the value axis
            float valueHig = 0f;
            if (relHasRange)
                valueHig = (float)((yValue - relMinValue) / relMinMaxRange * relGridLength);

            if (needDataAnimation)
                valueHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, dataIndex, valueHig);

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
            }
            np = new Vector3(xPos, yPos);
            return yPos;
        }
    }
}