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
            else if (serie.context.isTriggerByAxis)
            {
                serie.context.pointerEnter = true;
                serie.interact.SetValue(ref needInteract, serie.animation.interaction.GetWidth(lineWidth));
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var highlight = i == serie.context.pointerItemDataIndex;
                    serieData.context.highlight = highlight;
                    var state = SerieHelper.GetSerieState(serie, serieData, true);
                    var size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, state);
                    serieData.interact.SetValue(ref needInteract, size);
                    if (highlight)
                    {
                        serie.context.pointerEnter = true;
                        serie.context.pointerItemDataIndex = i;
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
                    var dist = Vector3.Distance(chart.pointerPos, serieData.context.position);
                    var size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize);
                    var highlight = dist <= size;
                    serieData.context.highlight = highlight;
                    var state = SerieHelper.GetSerieState(serie, serieData, true);
                    size = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, state);
                    serieData.interact.SetValue(ref needInteract, size);
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

            m_SerieGrid = chart.GetChartComponent<GridCoord>(axis.gridIndex);

            if (axis == null)
                return;
            if (relativedAxis == null)
                return;
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

            var interacting = false;
            var lineWidth = LineHelper.GetLineWidth(ref interacting, serie, chart.theme.serie.lineWidth);

            axis.context.scaleWidth = scaleWid;
            relativedAxis.context.scaleWidth = scaleRelativedWid;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;

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
                        maxCount, totalAverage, i, dataAddDuration, dataChangeDuration, ref dataChanging, relativedAxis, unscaledTime);

                    serieData.context.stackHeight = GetDataPoint(isY, axis, relativedAxis, m_SerieGrid, xValue, relativedValue,
                        i, scaleWid, scaleRelativedWid, false, ref np);

                    serieData.context.position = np;

                    serie.context.dataPoints.Add(np);
                    serie.context.dataIndexs.Add(serieData.index);
                    serie.context.dataIgnores.Add(false);
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

        private float GetDataPoint(bool isY, Axis axis, Axis relativedAxis, GridCoord grid, double xValue,
            double yValue, int i, float scaleWid, float scaleRelativedWid, bool isStack, ref Vector3 np)
        {
            float xPos, yPos;
            var gridXY = isY ? grid.context.x : grid.context.y;

            if (isY)
            {
                var valueHig = AxisHelper.GetAxisValueDistance(grid, relativedAxis, scaleRelativedWid, yValue);
                valueHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, valueHig);

                xPos = gridXY + valueHig;
                yPos = AxisHelper.GetAxisValuePosition(grid, axis, scaleWid, xValue);
            }
            else
            {

                var valueHig = AxisHelper.GetAxisValueDistance(grid, relativedAxis, scaleRelativedWid, yValue);
                valueHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, valueHig);

                yPos = gridXY + valueHig;
                xPos = AxisHelper.GetAxisValuePosition(grid, axis, scaleWid, xValue);
            }
            np = new Vector3(xPos, yPos);
            return yPos;
        }
    }
}