/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using XUGL;
using System;

namespace XCharts
{
    public partial class CoordinateChart : BaseChart
    {
        private static readonly string s_DefaultDataZoom = "datazoom";
        private static readonly string s_DefaultAxisName = "name";

        protected override void InitComponent()
        {
            base.InitComponent();
            if (m_Grids.Count == 0) m_Grids = new List<Grid>() { Grid.defaultGrid };
            if (m_DataZooms.Count == 0) m_DataZooms = new List<DataZoom>() { DataZoom.defaultDataZoom };
            if (m_VisualMaps.Count == 0) m_VisualMaps = new List<VisualMap> { new VisualMap() };
            InitDefaultAxes();
            CheckMinMaxValue();
            InitGrid();
            InitDataZoom();
            InitAxisX();
            InitAxisY();
            tooltip.UpdateToTop();

            var handler = new MarkLineHandler(this);
            m_ComponentHandlers.Add(handler);
            handler.Init();
        }

        protected override void Update()
        {
            CheckMinMaxValue();
            CheckRaycastTarget();
            base.Update();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Grids.Clear();
            m_DataZooms.Clear();
            m_VisualMaps.Clear();
            m_XAxes.Clear();
            m_YAxes.Clear();
            Awake();
        }
#endif

        private void RefreshSeriePainterByGridIndex(int gridIndex)
        {
            foreach (var serie in m_Series.list)
            {
                var axis = GetXAxis(serie.xAxisIndex);
                if (axis == null) continue;
                var grid = GetGrid(axis.index);
                if (grid == null) continue;
                if (grid.index == gridIndex)
                {
                    RefreshPainter(serie);
                }
            }
        }

        private void RefreshSeriePainterByAxisIndex(Axis axis)
        {
            var grid = GetGrid(axis.index);
            if (grid == null) return;
            RefreshSeriePainterByGridIndex(grid.index);
        }

        public override void OnDataZoomRangeChanged(DataZoom dataZoom)
        {
            foreach (var index in dataZoom.xAxisIndexs)
            {
                var axis = GetXAxis(index);
                if (axis != null && axis.show) axis.SetAllDirty();
            }
            foreach (var index in dataZoom.yAxisIndexs)
            {
                var axis = GetYAxis(index);
                if (axis != null && axis.show) axis.SetAllDirty();
            }
        }

        protected override void DrawPainterBase(VertexHelper vh)
        {
            base.DrawPainterBase(vh);
            if (!SeriesHelper.IsAnyClipSerie(m_Series))
            {
                DrawCoordinate(vh);
            }
        }

        protected override void DrawBackground(VertexHelper vh)
        {
            if (SeriesHelper.IsAnyClipSerie(m_Series))
            {
                var xLineDiff = xAxis0.axisLine.GetWidth(m_Theme.axis.lineWidth);
                var yLineDiff = yAxis0.axisLine.GetWidth(m_Theme.axis.lineWidth);
                var xSplitDiff = xAxis0.splitLine.GetWidth(m_Theme.axis.splitLineWidth);
                var ySplitDiff = yAxis0.splitLine.GetWidth(m_Theme.axis.splitLineWidth);
                foreach (var grid in m_Grids)
                {
                    var cpty = grid.runtimeY + grid.runtimeHeight + ySplitDiff;
                    var cp1 = new Vector3(grid.runtimeX - yLineDiff, grid.runtimeY - xLineDiff);
                    var cp2 = new Vector3(grid.runtimeX - yLineDiff, cpty);
                    var cp3 = new Vector3(grid.runtimeX + grid.runtimeWidth + xSplitDiff, cpty);
                    var cp4 = new Vector3(grid.runtimeX + grid.runtimeWidth + xSplitDiff, grid.runtimeY - xLineDiff);
                    var backgroundColor = ThemeHelper.GetBackgroundColor(m_Theme, m_Background);
                    UGL.DrawQuadrilateral(vh, cp1, cp2, cp3, cp4, backgroundColor);
                }
            }
            else
            {
                base.DrawBackground(vh);
            }
        }

        protected void DrawClip(VertexHelper vh)
        {
            if (!SeriesHelper.IsAnyClipSerie(m_Series)) return;
            var xLineDiff = xAxis0.axisLine.GetWidth(m_Theme.axis.lineWidth);
            var yLineDiff = yAxis0.axisLine.GetWidth(m_Theme.axis.lineWidth);
            var xSplitDiff = xAxis0.splitLine.GetWidth(m_Theme.axis.splitLineWidth);
            var ySplitDiff = yAxis0.splitLine.GetWidth(m_Theme.axis.splitLineWidth);
            var backgroundColor = ThemeHelper.GetBackgroundColor(m_Theme, m_Background);
            var lp1 = new Vector3(m_ChartX, m_ChartY);
            var lp2 = new Vector3(m_ChartX, m_ChartY + chartHeight);
            var lp3 = new Vector3(grid.runtimeX - yLineDiff, m_ChartY + chartHeight);
            var lp4 = new Vector3(grid.runtimeX - yLineDiff, m_ChartY);
            UGL.DrawQuadrilateral(vh, lp1, lp2, lp3, lp4, backgroundColor);
            var rp1 = new Vector3(grid.runtimeX + grid.runtimeWidth, m_ChartY);
            var rp2 = new Vector3(grid.runtimeX + grid.runtimeWidth, m_ChartY + chartHeight);
            var rp3 = new Vector3(m_ChartX + chartWidth, m_ChartY + chartHeight);
            var rp4 = new Vector3(m_ChartX + chartWidth, m_ChartY);
            UGL.DrawQuadrilateral(vh, rp1, rp2, rp3, rp4, backgroundColor);
            var up1 = new Vector3(grid.runtimeX - yLineDiff, grid.runtimeY + grid.runtimeHeight + ySplitDiff);
            var up2 = new Vector3(grid.runtimeX - yLineDiff, m_ChartY + chartHeight);
            var up3 = new Vector3(grid.runtimeX + grid.runtimeWidth + xSplitDiff, m_ChartY + chartHeight);
            var up4 = new Vector3(grid.runtimeX + grid.runtimeWidth + xSplitDiff,
                grid.runtimeY + grid.runtimeHeight + ySplitDiff);
            UGL.DrawQuadrilateral(vh, up1, up2, up3, up4, backgroundColor);
            var dp1 = new Vector3(grid.runtimeX - yLineDiff, m_ChartY);
            var dp2 = new Vector3(grid.runtimeX - yLineDiff, grid.runtimeY - xLineDiff);
            var dp3 = new Vector3(grid.runtimeX + grid.runtimeWidth + xSplitDiff, grid.runtimeY - xLineDiff);
            var dp4 = new Vector3(grid.runtimeX + grid.runtimeWidth + xSplitDiff, m_ChartY);
            UGL.DrawQuadrilateral(vh, dp1, dp2, dp3, dp4, backgroundColor);
        }

        protected override void DrawPainterSerie(VertexHelper vh, Serie serie)
        {
            base.DrawPainterSerie(vh, serie);
            var colorIndex = m_LegendRealShowName.IndexOf(serie.legendName);
            bool yCategory = IsAnyYAxisIsCategory();
            switch (serie.type)
            {
                case SerieType.Line:
                    serie.dataPoints.Clear();
                    if (yCategory) DrawYLineSerie(vh, serie, colorIndex);
                    else DrawXLineSerie(vh, serie, colorIndex);
                    if (!SeriesHelper.IsStack(m_Series))
                    {
                        DrawLinePoint(vh, serie);
                        DrawLineArrow(vh, serie);
                    }
                    break;
                case SerieType.Bar:
                    serie.dataPoints.Clear();
                    if (yCategory) DrawYBarSerie(vh, serie, colorIndex);
                    else DrawXBarSerie(vh, serie, colorIndex);
                    break;
                case SerieType.Scatter:
                case SerieType.EffectScatter:
                    serie.dataPoints.Clear();
                    DrawScatterSerie(vh, colorIndex, serie);
                    break;
                case SerieType.Heatmap:
                    serie.dataPoints.Clear();
                    DrawHeatmapSerie(vh, colorIndex, serie);
                    break;
                case SerieType.Candlestick:
                    serie.dataPoints.Clear();
                    DrawCandlestickSerie(vh, colorIndex, serie);
                    break;
            }
        }

        protected override void DrawPainterTop(VertexHelper vh)
        {
            DrawClip(vh);
            if (SeriesHelper.IsAnyClipSerie(m_Series))
            {
                DrawCoordinate(vh);
            }
            DrawAxisTick(vh);
            DrawLabelBackground(vh);
            if (SeriesHelper.IsStack(m_Series))
            {
                foreach (var serie in m_Series.list)
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
            bool yCategory = IsAnyYAxisIsCategory();
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }

        protected override void CheckTootipArea(Vector2 local, bool isActivedOther)
        {
            if (isActivedOther)
            {
                foreach (var axis in m_XAxes) axis.SetTooltipLabelActive(false);
                foreach (var axis in m_YAxes) axis.SetTooltipLabelActive(false);
                return;
            }
            var grid = GetGrid(local);
            if (grid == null)
            {
                tooltip.runtimeGridIndex = -1;
                tooltip.ClearValue();
                UpdateTooltip();
            }
            else
            {
                if (tooltip.runtimeGridIndex != grid.index)
                {
                    tooltip.runtimeGridIndex = grid.index;
                    RefreshSeriePainterByGridIndex(grid.index);
                }
                UpdateTooltipValue(local);
            }
            if (tooltip.IsSelected())
            {
                tooltip.UpdateContentPos(local + tooltip.offset);
                UpdateTooltip();
                if (tooltip.IsDataIndexChanged() || tooltip.type == Tooltip.Type.Corss)
                {
                    tooltip.UpdateLastDataIndex();
                    m_PainterTop.Refresh();
                }
            }
            else if (tooltip.IsActive())
            {
                tooltip.SetActive(false);
                m_PainterTop.Refresh();
            }
        }

        protected virtual void UpdateTooltipValue(Vector2 local)
        {
            var isCartesian = IsValue();
            var grid = GetGrid(tooltip.runtimeGridIndex);
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                var xAxis = m_XAxes[i];
                var yAxis = m_YAxes[i];
                if (!xAxis.show && !yAxis.show) continue;
                if (isCartesian && xAxis.show && yAxis.show)
                {
                    var yRate = (yAxis.runtimeMaxValue - yAxis.runtimeMinValue) / grid.runtimeHeight;
                    var xRate = (xAxis.runtimeMaxValue - xAxis.runtimeMinValue) / grid.runtimeWidth;
                    var yValue = yRate * (local.y - grid.runtimeY - yAxis.runtimeZeroYOffset);
                    if (yAxis.runtimeMinValue > 0) yValue += yAxis.runtimeMinValue;
                    tooltip.runtimeYValues[i] = yValue;
                    var xValue = xRate * (local.x - grid.runtimeX - xAxis.runtimeZeroXOffset);
                    if (xAxis.runtimeMinValue > 0) xValue += xAxis.runtimeMinValue;
                    tooltip.runtimeXValues[i] = xValue;

                    for (int j = 0; j < m_Series.Count; j++)
                    {
                        var serie = m_Series.GetSerie(j);
                        for (int n = 0; n < serie.data.Count; n++)
                        {
                            var serieData = serie.data[n];
                            var xdata = serieData.GetData(0, xAxis.inverse);
                            var ydata = serieData.GetData(1, yAxis.inverse);
                            var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                            var symbolSize = symbol.GetSize(serieData == null ? null : serieData.data,
                                m_Theme.serie.lineSymbolSize);
                            if (Math.Abs(xValue - xdata) / xRate < symbolSize
                                && Math.Abs(yValue - ydata) / yRate < symbolSize)
                            {
                                tooltip.runtimeDataIndex[i] = n;
                                RefreshPainter(serie);
                                serieData.highlighted = true;
                            }
                            else
                            {
                                serieData.highlighted = false;
                            }
                        }
                    }
                }
                else if (IsCategory())
                {
                    var dataZoomX = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
                    var dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoomX).Count : 0;
                    for (int j = 0; j < xAxis.GetDataNumber(dataZoomX); j++)
                    {
                        float splitWid = AxisHelper.GetDataWidth(xAxis, grid.runtimeWidth, dataCount, dataZoomX);
                        float pX = grid.runtimeX + j * splitWid;
                        if ((xAxis.boundaryGap && (local.x > pX && local.x <= pX + splitWid)) ||
                            (!xAxis.boundaryGap && (local.x > pX - splitWid / 2 && local.x <= pX + splitWid / 2)))
                        {
                            tooltip.runtimeXValues[i] = j;
                            tooltip.runtimeDataIndex[i] = j;
                            break;
                        }
                    }
                    var dataZoomY = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
                    dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoomY).Count : 0;
                    for (int j = 0; j < yAxis.GetDataNumber(dataZoomY); j++)
                    {
                        float splitWid = AxisHelper.GetDataWidth(yAxis, grid.runtimeHeight, dataCount, dataZoomY);
                        float pY = grid.runtimeY + j * splitWid;
                        if ((yAxis.boundaryGap && (local.y > pY && local.y <= pY + splitWid)) ||
                            (!yAxis.boundaryGap && (local.y > pY - splitWid / 2 && local.y <= pY + splitWid / 2)))
                        {
                            tooltip.runtimeYValues[i] = j;
                            break;
                        }
                    }
                }
                else if (xAxis.IsCategory())
                {
                    var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
                    var dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
                    var value = (yAxis.runtimeMaxValue - yAxis.runtimeMinValue) * (local.y - grid.runtimeY - yAxis.runtimeZeroYOffset) / grid.runtimeHeight;
                    if (yAxis.runtimeMinValue > 0) value += yAxis.runtimeMinValue;
                    tooltip.runtimeYValues[i] = value;
                    for (int j = 0; j < xAxis.GetDataNumber(dataZoom); j++)
                    {
                        float splitWid = AxisHelper.GetDataWidth(xAxis, grid.runtimeWidth, dataCount, dataZoom);
                        float pX = grid.runtimeX + j * splitWid;
                        if ((xAxis.boundaryGap && (local.x > pX && local.x <= pX + splitWid)) ||
                            (!xAxis.boundaryGap && (local.x > pX - splitWid / 2 && local.x <= pX + splitWid / 2)))
                        {
                            tooltip.runtimeXValues[i] = j;
                            tooltip.runtimeDataIndex[i] = j;
                            RefreshSeriePainterByAxisIndex(xAxis);
                            break;
                        }
                    }
                }
                else if (yAxis.IsCategory())
                {
                    var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
                    var dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
                    var value = (xAxis.runtimeMaxValue - xAxis.runtimeMinValue) * (local.x - grid.runtimeX - xAxis.runtimeZeroXOffset) / grid.runtimeWidth;
                    if (xAxis.runtimeMinValue > 0) value += xAxis.runtimeMinValue;
                    tooltip.runtimeXValues[i] = value;
                    for (int j = 0; j < yAxis.GetDataNumber(dataZoom); j++)
                    {
                        float splitWid = AxisHelper.GetDataWidth(yAxis, grid.runtimeHeight, dataCount, dataZoom);
                        float pY = grid.runtimeY + j * splitWid;
                        if ((yAxis.boundaryGap && (local.y > pY && local.y <= pY + splitWid)) ||
                            (!yAxis.boundaryGap && (local.y > pY - splitWid / 2 && local.y <= pY + splitWid / 2)))
                        {
                            tooltip.runtimeYValues[i] = j;
                            tooltip.runtimeDataIndex[i] = j;
                            RefreshSeriePainterByAxisIndex(yAxis);
                            break;
                        }
                    }
                }
            }
        }

        protected StringBuilder sb = new StringBuilder(100);
        protected override void UpdateTooltip()
        {
            base.UpdateTooltip();
            int index;
            Axis tempAxis;
            bool isCartesian = IsValue();
            if (isCartesian)
            {
                index = tooltip.runtimeDataIndex[0];
                tempAxis = m_XAxes[0];
            }
            else if (m_XAxes[0].type == Axis.AxisType.Value)
            {
                index = (int)tooltip.runtimeYValues[0];
                tempAxis = m_YAxes[0];
            }
            else
            {
                index = (int)tooltip.runtimeXValues[0];
                tempAxis = m_XAxes[0];
            }
            if (index < 0)
            {
                if (tooltip.IsActive())
                {
                    tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            UpdateSerieGridIndex();
            RefreshSeriePainterByGridIndex(grid.index);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(tempAxis, dataZooms);
            var content = TooltipHelper.GetFormatterContent(tooltip, index, this, dataZoom, isCartesian);
            TooltipHelper.SetContentAndPosition(tooltip, content, chartRect);
            tooltip.SetActive(true);

            for (int i = 0; i < m_XAxes.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_XAxes[i]);
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_YAxes[i]);
            }
        }

        internal string GetTooltipCategory(int dataIndex, DataZoom dataZoom = null)
        {
            bool isCartesian = IsValue();
            var index = -1;
            Axis tempAxis;
            if (isCartesian)
            {
                index = tooltip.runtimeDataIndex[0];
                tempAxis = m_XAxes[0];
            }
            else if (m_XAxes[0].type == Axis.AxisType.Value)
            {
                index = (int)tooltip.runtimeYValues[0];
                tempAxis = m_YAxes[0];
            }
            else
            {
                index = (int)tooltip.runtimeXValues[0];
                tempAxis = m_XAxes[0];
            }
            return tempAxis.GetData(index, dataZoom);
        }
        internal string GetTooltipCategory(int dataIndex, Serie serie, DataZoom dataZoom = null)
        {
            bool isCartesian = IsValue();
            var index = -1;
            Axis tempAxis;
            if (isCartesian)
            {
                index = tooltip.runtimeDataIndex[0];
                tempAxis = GetXAxis(serie.xAxisIndex);
            }
            else if (m_XAxes[0].type == Axis.AxisType.Value)
            {
                index = (int)tooltip.runtimeYValues[0];
                tempAxis = GetYAxis(serie.yAxisIndex);
            }
            else
            {
                index = (int)tooltip.runtimeXValues[0];
                tempAxis = GetXAxis(serie.xAxisIndex);
            }
            return tempAxis == null ? "" : tempAxis.GetData(index, dataZoom);
        }

        protected void UpdateAxisTooltipLabel(int axisIndex, Axis axis)
        {
            var showTooltipLabel = axis.show && tooltip.type == Tooltip.Type.Corss;
            axis.SetTooltipLabelActive(showTooltipLabel);
            if (!showTooltipLabel) return;
            var labelText = "";
            var labelPos = Vector2.zero;
            var grid = GetAxisGridOrDefault(axis);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(axis, dataZooms);
            if (axis is XAxis)
            {
                var posY = axisIndex > 0 ? grid.runtimeY + grid.runtimeHeight : grid.runtimeY;
                var diff = axisIndex > 0
                    ? -axis.axisLabel.textStyle.GetFontSize(m_Theme.tooltip) - axis.axisLabel.margin - 3.5f
                    : axis.axisLabel.margin / 2 + 1;
                if (axis.IsValue())
                {
                    labelText = ChartCached.NumberToStr(tooltip.runtimeXValues[axisIndex], axis.axisLabel.numericFormatter);
                    labelPos = new Vector2(tooltip.runtimePointerPos.x, posY - diff);
                }
                else
                {
                    labelText = axis.GetData((int)tooltip.runtimeXValues[axisIndex], dataZoom);
                    var splitWidth = AxisHelper.GetDataWidth(axis, grid.runtimeWidth, 0, dataZoom);
                    var index = (int)tooltip.runtimeXValues[axisIndex];
                    var px = grid.runtimeX + index * splitWidth + (axis.boundaryGap ? splitWidth / 2 : 0);
                    labelPos = new Vector2(px, posY - diff);
                }
            }
            else if (axis is YAxis)
            {
                var posX = axisIndex > 0 ? grid.runtimeX + grid.runtimeWidth : grid.runtimeX;
                var diff = axisIndex > 0 ? -axis.axisLabel.margin + 3 : axis.axisLabel.margin - 3;
                if (axis.IsValue())
                {
                    labelText = ChartCached.NumberToStr(tooltip.runtimeYValues[axisIndex], axis.axisLabel.numericFormatter);
                    labelPos = new Vector2(posX - diff, tooltip.runtimePointerPos.y);
                }
                else
                {
                    labelText = axis.GetData((int)tooltip.runtimeYValues[axisIndex], dataZoom);
                    var splitWidth = AxisHelper.GetDataWidth(axis, grid.runtimeHeight, 0, dataZoom);
                    var index = (int)tooltip.runtimeYValues[axisIndex];
                    var py = grid.runtimeY + index * splitWidth + (axis.boundaryGap ? splitWidth / 2 : 0);
                    labelPos = new Vector2(posX - diff, py);
                }
            }
            axis.UpdateTooptipLabelText(labelText);
            axis.UpdateTooltipLabelPos(labelPos);
        }

        private void InitDefaultAxes()
        {
            if (m_XAxes.Count <= 0)
            {
                var axis1 = XAxis.defaultXAxis;
                var axis2 = XAxis.defaultXAxis;
                axis1.show = true;
                axis2.show = false;
                axis1.position = Axis.AxisPosition.Bottom;
                axis2.position = Axis.AxisPosition.Top;
                m_XAxes.Add(axis1);
                m_XAxes.Add(axis2);
            }
            if (m_YAxes.Count <= 0)
            {
                var axis1 = YAxis.defaultYAxis;
                var axis2 = YAxis.defaultYAxis;
                axis1.show = true;
                axis1.splitNumber = 5;
                axis1.boundaryGap = false;
                axis2.show = false;
                axis1.position = Axis.AxisPosition.Left;
                axis2.position = Axis.AxisPosition.Right;
                m_YAxes.Add(axis1);
                m_YAxes.Add(axis2);
            }
            foreach (var axis in m_XAxes) axis.runtimeMinValue = axis.runtimeMaxValue = 0;
            foreach (var axis in m_YAxes) axis.runtimeMinValue = axis.runtimeMaxValue = 0;
        }

        private void InitAxisY()
        {
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                InitYAxis(i, m_YAxes[i]);
            }
        }

        private void InitYAxis(int yAxisIndex, YAxis yAxis)
        {
            yAxis.painter = m_Painter;
            yAxis.refreshComponent = delegate ()
            {
                InitAxisRuntimeData(yAxis);
                string objName = ChartCached.GetYAxisName(yAxisIndex);
                var axisObj = ChartHelper.AddObject(objName, transform, graphAnchorMin,
                    graphAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
                yAxis.gameObject = axisObj;
                yAxis.runtimeAxisLabelList.Clear();
                axisObj.SetActive(yAxis.show);
                axisObj.hideFlags = chartHideFlags;
                ChartHelper.HideAllObject(axisObj);
                var grid = GetAxisGridOrDefault(yAxis);
                if (grid == null) return;
                if (!yAxis.show) return;
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
                var axisLabelTextStyle = yAxis.axisLabel.textStyle;
                int splitNumber = AxisHelper.GetScaleNumber(yAxis, grid.runtimeHeight, dataZoom);
                float totalWidth = 0;
                float eachWidth = AxisHelper.GetEachWidth(yAxis, grid.runtimeHeight, dataZoom);
                float gapWidth = yAxis.boundaryGap ? eachWidth / 2 : 0;
                float textWidth = yAxis.axisLabel.width > 0
                    ? yAxis.axisLabel.width
                    : grid.left;
                float textHeight = yAxis.axisLabel.height > 0
                    ? yAxis.axisLabel.height
                    : 20f;
                if (yAxis.IsCategory() && yAxis.boundaryGap)
                {
                    splitNumber -= 1;
                }
                for (int i = 0; i < splitNumber; i++)
                {
                    ChartLabel txt;
                    var inside = yAxis.axisLabel.inside;
                    var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                    var labelName = AxisHelper.GetLabelName(yAxis, grid.runtimeHeight, i, yAxis.runtimeMinValue,
                        yAxis.runtimeMaxValue, dataZoom, isPercentStack);
                    if ((inside && yAxis.IsLeft()) || (!inside && yAxis.IsRight()))
                    {
                        txt = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform, Vector2.zero,
                            Vector2.zero, new Vector2(0, 0.5f), new Vector2(textWidth, textHeight), yAxis, theme.axis,
                            labelName);
                        txt.label.SetAlignment(axisLabelTextStyle.GetAlignment(TextAnchor.MiddleLeft));
                    }
                    else
                    {
                        txt = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform, Vector2.zero,
                            Vector2.zero, new Vector2(1, 0.5f), new Vector2(textWidth, textHeight), yAxis, theme.axis,
                            labelName);
                        txt.label.SetAlignment(axisLabelTextStyle.GetAlignment(TextAnchor.MiddleRight));
                    }
                    var labelWidth = AxisHelper.GetScaleWidth(yAxis, grid.runtimeHeight, i + 1, dataZoom);
                    if (i == 0) yAxis.axisLabel.SetRelatedText(txt.label, labelWidth);
                    txt.SetPosition(GetLabelYPosition(totalWidth + gapWidth, i, yAxisIndex, yAxis));
                    yAxis.runtimeAxisLabelList.Add(txt);
                    totalWidth += labelWidth;
                }
                if (yAxis.axisName.show)
                {
                    var axisNameTextStyle = yAxis.axisName.textStyle;
                    var offset = axisNameTextStyle.offset;
                    ChartText axisName = null;
                    var xAxis = GetXAxis(yAxisIndex);
                    var zeroPos = new Vector3(grid.runtimeX + (xAxis == null ? 0 : xAxis.runtimeZeroXOffset), grid.runtimeY);
                    switch (yAxis.axisName.location)
                    {
                        case AxisName.Location.Start:
                            axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform, new Vector2(0.5f, 0.5f),
                                 new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), axisNameTextStyle,
                                 m_Theme.axis);
                            axisName.SetAlignment(axisNameTextStyle.GetAlignment(TextAnchor.MiddleCenter));
                            axisName.SetLocalPosition(yAxis.position == Axis.AxisPosition.Right ?
                                new Vector2(grid.runtimeX + grid.runtimeWidth + offset.x + yAxis.offset, grid.runtimeY - offset.y) :
                                new Vector2(zeroPos.x + offset.x + yAxis.offset, grid.runtimeY - offset.y));
                            break;
                        case AxisName.Location.Middle:
                            axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform, new Vector2(1, 0.5f),
                                new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 20), axisNameTextStyle,
                                m_Theme.axis);
                            axisName.SetAlignment(axisNameTextStyle.GetAlignment(TextAnchor.MiddleRight));
                            axisName.SetLocalPosition(yAxis.position == Axis.AxisPosition.Right ?
                            new Vector2(grid.runtimeX + grid.runtimeWidth - offset.x + yAxis.offset, grid.runtimeY + grid.runtimeHeight / 2 + offset.y) :
                            new Vector2(grid.runtimeX - offset.x + yAxis.offset, grid.runtimeY + grid.runtimeHeight / 2 + offset.y));
                            break;
                        case AxisName.Location.End:
                            axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform, new Vector2(0.5f, 0.5f),
                                 new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), axisNameTextStyle,
                                 m_Theme.axis);
                            axisName.SetAlignment(axisNameTextStyle.GetAlignment(TextAnchor.MiddleCenter));
                            axisName.SetLocalPosition(yAxis.position == Axis.AxisPosition.Right ?
                                new Vector2(grid.runtimeX + grid.runtimeWidth + offset.x + yAxis.offset, grid.runtimeY + grid.runtimeHeight + offset.y) :
                                new Vector2(zeroPos.x + offset.x + yAxis.offset, grid.runtimeY + grid.runtimeHeight + offset.y));
                            break;
                    }
                    axisName.SetText(yAxis.axisName.name);
                }
                //init tooltip label
                if (tooltip.runtimeGameObject)
                {
                    Vector2 privot = yAxis.position == Axis.AxisPosition.Right ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
                    var labelParent = tooltip.runtimeGameObject.transform;
                    var labelName = ChartCached.GetAxisTooltipLabel(objName);
                    GameObject labelObj = ChartHelper.AddTooltipLabel(labelName, labelParent, m_Theme, privot);
                    yAxis.SetTooltipLabel(labelObj);
                    yAxis.SetTooltipLabelColor(m_Theme.tooltip.labelBackgroundColor, m_Theme.tooltip.labelTextColor);
                    yAxis.SetTooltipLabelActive(yAxis.show && tooltip.show && tooltip.type == Tooltip.Type.Corss);
                }
            };
            yAxis.refreshComponent();
        }

        protected virtual void InitAxisRuntimeData(Axis axis)
        {
        }

        internal void InitAxisX()
        {
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                InitXAxis(i, m_XAxes[i]);
            }
        }

        private void InitXAxis(int xAxisIndex, XAxis xAxis)
        {
            xAxis.painter = m_Painter;
            xAxis.refreshComponent = delegate ()
            {
                string objName = ChartCached.GetXAxisName(xAxisIndex);
                var axisObj = ChartHelper.AddObject(objName, transform, graphAnchorMin,
                    graphAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
                xAxis.gameObject = axisObj;
                xAxis.runtimeAxisLabelList.Clear();
                axisObj.SetActive(xAxis.show);
                axisObj.hideFlags = chartHideFlags;
                ChartHelper.HideAllObject(axisObj);
                var grid = GetAxisGridOrDefault(xAxis);
                if (grid == null) return;
                if (!xAxis.show) return;
                var axisLabelTextStyle = xAxis.axisLabel.textStyle;
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
                int splitNumber = AxisHelper.GetScaleNumber(xAxis, grid.runtimeWidth, dataZoom);
                float totalWidth = 0;
                float eachWidth = AxisHelper.GetEachWidth(xAxis, grid.runtimeWidth, dataZoom);
                float gapWidth = xAxis.boundaryGap ? eachWidth / 2 : 0;
                float textWidth = xAxis.axisLabel.width > 0
                    ? xAxis.axisLabel.width
                    : AxisHelper.GetScaleWidth(xAxis, grid.runtimeWidth, 0, dataZoom);
                float textHeight = xAxis.axisLabel.height > 0
                    ? xAxis.axisLabel.height
                    : 20f;
                if (xAxis.IsCategory() && xAxis.boundaryGap)
                {
                    splitNumber -= 1;
                }
                for (int i = 0; i < splitNumber; i++)
                {
                    var labelWidth = AxisHelper.GetScaleWidth(xAxis, grid.runtimeWidth, i + 1, dataZoom);
                    var inside = xAxis.axisLabel.inside;
                    var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                    var labelName = AxisHelper.GetLabelName(xAxis, grid.runtimeWidth, i, xAxis.runtimeMinValue,
                        xAxis.runtimeMaxValue, dataZoom, isPercentStack);
                    var label = ChartHelper.AddAxisLabelObject(splitNumber, i, ChartCached.GetXAxisName(xAxisIndex, i),
                        axisObj.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                        new Vector2(textWidth, textHeight), xAxis, theme.axis, labelName);

                    if (i == 0) xAxis.axisLabel.SetRelatedText(label.label, labelWidth);
                    label.label.SetAlignment(axisLabelTextStyle.GetAlignment(TextAnchor.MiddleCenter));
                    label.SetPosition(GetLabelXPosition(totalWidth + gapWidth, i, xAxisIndex, xAxis));
                    xAxis.runtimeAxisLabelList.Add(label);
                    totalWidth += labelWidth;
                }
                if (xAxis.axisName.show)
                {
                    var axisNameTextStyle = xAxis.axisName.textStyle;
                    var offset = axisNameTextStyle.offset;
                    ChartText axisName = null;
                    var yAxis = GetYAxis(xAxisIndex);
                    var zeroPos = new Vector3(grid.runtimeX, grid.runtimeY + (yAxis == null ? 0 : yAxis.runtimeZeroYOffset));
                    switch (xAxis.axisName.location)
                    {
                        case AxisName.Location.Start:
                            axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform, new Vector2(1, 0.5f),
                                new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 20), axisNameTextStyle, theme.axis);
                            axisName.SetAlignment(axisNameTextStyle.GetAlignment(TextAnchor.MiddleRight));
                            axisName.SetLocalPosition(xAxis.position == Axis.AxisPosition.Top ?
                                new Vector2(zeroPos.x - offset.x, grid.runtimeY + grid.runtimeHeight + offset.y + xAxis.offset) :
                                new Vector2(zeroPos.x - offset.x, zeroPos.y + offset.y + xAxis.offset));
                            break;
                        case AxisName.Location.Middle:
                            axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform, new Vector2(0.5f, 0.5f),
                                 new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), axisNameTextStyle, theme.axis);
                            axisName.SetAlignment(axisNameTextStyle.GetAlignment(TextAnchor.MiddleCenter));
                            axisName.SetLocalPosition(xAxis.position == Axis.AxisPosition.Top ?
                                new Vector2(grid.runtimeX + grid.runtimeWidth / 2 + offset.x, grid.runtimeY + grid.runtimeHeight - offset.y + xAxis.offset) :
                                new Vector2(grid.runtimeX + grid.runtimeWidth / 2 + offset.x, grid.runtimeY - offset.y + xAxis.offset));
                            break;
                        case AxisName.Location.End:
                            axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform, new Vector2(0, 0.5f),
                                 new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(100, 20), axisNameTextStyle, theme.axis);
                            axisName.SetAlignment(axisNameTextStyle.GetAlignment(TextAnchor.MiddleLeft));
                            axisName.SetLocalPosition(xAxis.position == Axis.AxisPosition.Top ?
                                new Vector2(grid.runtimeX + grid.runtimeWidth + offset.x, grid.runtimeY + grid.runtimeHeight + offset.y + xAxis.offset) :
                                new Vector2(grid.runtimeX + grid.runtimeWidth + offset.x, zeroPos.y + offset.y + xAxis.offset));
                            break;
                    }
                    axisName.SetText(xAxis.axisName.name);
                }
                if (tooltip.runtimeGameObject)
                {
                    Vector2 privot = xAxis.position != Axis.AxisPosition.Top ? new Vector2(0.5f, 1) : new Vector2(0.5f, 1);
                    var labelParent = tooltip.runtimeGameObject.transform;
                    var labelName = ChartCached.GetAxisTooltipLabel(objName);
                    GameObject labelObj = ChartHelper.AddTooltipLabel(labelName, labelParent, theme, privot);
                    xAxis.SetTooltipLabel(labelObj);
                    xAxis.SetTooltipLabelColor(theme.tooltip.labelBackgroundColor, theme.tooltip.labelTextColor);
                    xAxis.SetTooltipLabelActive(xAxis.show && tooltip.show && tooltip.type == Tooltip.Type.Corss);
                }
            };
            xAxis.refreshComponent();
        }

        private void InitGrid()
        {
            for (int i = 0; i < m_Grids.Count; i++)
            {
                var grid = m_Grids[i];
                grid.index = i;
                grid.painter = m_Painter;
                grid.refreshComponent = delegate ()
                {
                    OnCoordinateChanged();
                };
                grid.refreshComponent();
            }
        }

        private void InitDataZoom()
        {
            for (int i = 0; i < m_DataZooms.Count; i++)
            {
                var dataZoom = m_DataZooms[i];
                dataZoom.index = i;
                dataZoom.painter = m_PainterTop;
                dataZoom.refreshComponent = delegate ()
                {
                    var dataZoomObject = ChartHelper.AddObject(s_DefaultDataZoom + dataZoom.index, transform, graphAnchorMin,
                    graphAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
                    dataZoom.gameObject = dataZoomObject;
                    dataZoomObject.hideFlags = chartHideFlags;
                    ChartHelper.HideAllObject(dataZoomObject);
                    var startLabel = ChartHelper.AddTextObject(s_DefaultDataZoom + "start", dataZoomObject.transform,
                        Vector2.zero, Vector2.zero, new Vector2(1, 0.5f), new Vector2(200, 20), dataZoom.textStyle,
                        m_Theme.dataZoom);
                    startLabel.SetAlignment(TextAnchor.MiddleRight);
                    var endLabel = ChartHelper.AddTextObject(s_DefaultDataZoom + "end", dataZoomObject.transform,
                        Vector2.zero, Vector2.zero, new Vector2(0, 0.5f), new Vector2(200, 20), dataZoom.textStyle,
                        m_Theme.dataZoom);
                    endLabel.SetAlignment(TextAnchor.MiddleLeft);
                    dataZoom.SetStartLabel(startLabel);
                    dataZoom.SetEndLabel(endLabel);
                    dataZoom.SetLabelActive(false);
                    CheckRaycastTarget();
                    foreach (var index in dataZoom.xAxisIndexs)
                    {
                        var xAxis = m_XAxes[index];
                        if (xAxis != null)
                        {
                            xAxis.UpdateFilterData(dataZoom);
                        }
                    }
                    if (m_Series != null)
                    {
                        m_Series.UpdateFilterData(dataZoom);
                    }
                };
                dataZoom.refreshComponent();
            }
        }

        private Vector3 GetLabelYPosition(float scaleWid, int i, int yAxisIndex, YAxis yAxis)
        {
            var grid = GetAxisGridOrDefault(yAxis);
            var startX = grid.runtimeX + (yAxis.axisLabel.onZero ? m_XAxes[yAxisIndex].runtimeZeroXOffset : 0);
            if (yAxis.IsRight()) startX += grid.runtimeWidth;
            var posX = 0f;
            var inside = yAxis.axisLabel.inside;
            if ((inside && yAxis.IsLeft()) || (!inside && yAxis.IsRight()))
            {
                posX = startX + yAxis.axisLabel.margin;
            }
            else
            {
                posX = startX - yAxis.axisLabel.margin;
            }
            posX += yAxis.offset;
            return new Vector3(posX, grid.runtimeY + scaleWid, 0) + yAxis.axisLabel.textStyle.offsetv3;
        }

        private Vector3 GetLabelXPosition(float scaleWid, int i, int xAxisIndex, XAxis xAxis)
        {
            var grid = GetAxisGridOrDefault(xAxis);
            var startY = grid.runtimeY + (xAxis.axisLabel.onZero ? m_YAxes[xAxisIndex].runtimeZeroYOffset : 0);
            if (xAxis.IsTop()) startY += grid.runtimeHeight;
            var posY = 0f;
            var inside = xAxis.axisLabel.inside;
            var fontSize = xAxis.axisLabel.textStyle.GetFontSize(m_Theme.axis);
            if ((inside && xAxis.IsBottom()) || (!inside && xAxis.IsTop()))
            {
                posY = startY + xAxis.axisLabel.margin + fontSize / 2;
            }
            else
            {
                posY = startY - xAxis.axisLabel.margin - fontSize / 2;
            }
            posY += xAxis.offset;
            return new Vector3(grid.runtimeX + scaleWid, posY) + xAxis.axisLabel.textStyle.offsetv3;
        }

        protected virtual void CheckMinMaxValue()
        {
            if (m_XAxes == null || m_YAxes == null) return;
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                UpdateAxisMinMaxValue(i, m_XAxes[i]);
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                UpdateAxisMinMaxValue(i, m_YAxes[i]);
            }
        }

        private void UpdateAxisMinMaxValue(int axisIndex, Axis axis, bool updateChart = true)
        {
            if (!axis.show) return;
            if (axis.IsCategory())
            {
                axis.runtimeMinValue = 0;
                axis.runtimeMaxValue = SeriesHelper.GetMaxSerieDataCount(m_Series) - 1;
                return;
            }
            double tempMinValue = 0;
            double tempMaxValue = 0;
            GetSeriesMinMaxValue(axis, axisIndex, out tempMinValue, out tempMaxValue);
            if (tempMinValue != axis.runtimeMinValue || tempMaxValue != axis.runtimeMaxValue)
            {
                m_IsPlayingAnimation = true;
                var needCheck = !m_IsPlayingAnimation && axis.runtimeLastCheckInverse == axis.inverse;
                axis.UpdateMinValue(tempMinValue, needCheck);
                axis.UpdateMaxValue(tempMaxValue, needCheck);
                axis.runtimeZeroXOffset = 0;
                axis.runtimeZeroYOffset = 0;
                axis.runtimeLastCheckInverse = axis.inverse;
                if (tempMinValue != 0 || tempMaxValue != 0)
                {
                    var grid = GetAxisGridOrDefault(axis);
                    if (grid != null && axis is XAxis && axis.IsValue())
                    {
                        axis.runtimeZeroXOffset = axis.runtimeMinValue > 0 ? 0 :
                            axis.runtimeMaxValue < 0 ? grid.runtimeWidth :
                            (float)(Math.Abs(axis.runtimeMinValue) * (grid.runtimeWidth / (Math.Abs(axis.runtimeMinValue) + Math.Abs(axis.runtimeMaxValue))));
                    }
                    if (grid != null && axis is YAxis && axis.IsValue())
                    {
                        axis.runtimeZeroYOffset = axis.runtimeMinValue > 0 ? 0 :
                            axis.runtimeMaxValue < 0 ? grid.runtimeHeight :
                            (float)(Math.Abs(axis.runtimeMinValue) * (grid.runtimeHeight / (Math.Abs(axis.runtimeMinValue) + Math.Abs(axis.runtimeMaxValue))));
                    }
                }
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(axis, dataZooms);
                if (dataZoom != null && dataZoom.enable)
                {
                    if (axis is XAxis) dataZoom.SetXAxisIndexValueInfo(axisIndex, tempMinValue, tempMaxValue);
                    else dataZoom.SetYAxisIndexValueInfo(axisIndex, tempMinValue, tempMaxValue);
                }
                if (updateChart)
                {
                    UpdateAxisLabelText(axis);
                    RefreshChart();
                }
            }
            if (axis.IsValueChanging(500) && !m_IsPlayingAnimation)
            {
                UpdateAxisLabelText(axis);
                RefreshChart();
            }
        }

        protected virtual void GetSeriesMinMaxValue(Axis axis, int axisIndex, out double tempMinValue, out double tempMaxValue)
        {
            if (IsValue())
            {
                if (axis is XAxis)
                {
                    SeriesHelper.GetXMinMaxValue(m_Series, null, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
                }
                else
                {
                    SeriesHelper.GetYMinMaxValue(m_Series, null, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
                }
            }
            else
            {
                SeriesHelper.GetYMinMaxValue(m_Series, null, axisIndex, false, axis.inverse, out tempMinValue, out tempMaxValue);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
        }

        protected void UpdateAxisLabelText(Axis axis)
        {
            var grid = GetAxisGridOrDefault(axis);
            if (grid == null || axis == null) return;
            float runtimeWidth = axis is XAxis ? grid.runtimeWidth : grid.runtimeHeight;
            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(axis, dataZooms);
            axis.UpdateLabelText(runtimeWidth, dataZoom, isPercentStack, 500);
        }

        protected virtual void OnCoordinateChanged()
        {
            UpdateCoordinate();
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                m_XAxes[i].SetAllDirty();
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                m_YAxes[i].SetAllDirty();
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            OnCoordinateChanged();
        }

        private void DrawCoordinate(VertexHelper vh)
        {
            DrawGrid(vh);
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                m_XAxes[i].index = i;
                DrawXAxisSplit(vh, i, m_XAxes[i]);
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                m_YAxes[i].index = i;
                DrawYAxisSplit(vh, i, m_YAxes[i]);
            }
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                DrawXAxisLine(vh, i, m_XAxes[i]);
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                DrawYAxisLine(vh, i, m_YAxes[i]);
            }
        }

        private void DrawAxisTick(VertexHelper vh)
        {
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                DrawXAxisTick(vh, i, m_XAxes[i]);
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                DrawYAxisTick(vh, i, m_YAxes[i]);
            }
        }

        private void DrawGrid(VertexHelper vh)
        {
            foreach (var grid in m_Grids)
            {
                if (grid.show && !ChartHelper.IsClearColor(grid.backgroundColor))
                {
                    var p1 = new Vector2(grid.runtimeX, grid.runtimeY);
                    var p2 = new Vector2(grid.runtimeX, grid.runtimeY + grid.runtimeHeight);
                    var p3 = new Vector2(grid.runtimeX + grid.runtimeWidth, grid.runtimeY + grid.runtimeHeight);
                    var p4 = new Vector2(grid.runtimeX + grid.runtimeWidth, grid.runtimeY);
                    UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, grid.backgroundColor);
                }
            }
        }

        private void DrawYAxisSplit(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = GetAxisGridOrDefault(yAxis);
                if (grid == null) return;
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
                var size = AxisHelper.GetScaleNumber(yAxis, grid.runtimeWidth, dataZoom);
                var totalWidth = grid.runtimeY;
                var xAxis = GetRelatedXAxis(yAxis);
                var zeroPos = new Vector3(grid.runtimeX + xAxis.runtimeZeroXOffset, grid.runtimeY + yAxis.runtimeZeroYOffset);
                var lineColor = yAxis.splitLine.GetColor(m_Theme.axis.splitLineColor);
                var lineWidth = yAxis.splitLine.GetWidth(m_Theme.axis.lineWidth);
                var lineType = yAxis.splitLine.GetType(m_Theme.axis.splitLineType);
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(yAxis, grid.runtimeHeight, i + 1, dataZoom);
                    float pY = totalWidth;
                    if (yAxis.boundaryGap && yAxis.axisTick.alignWithLabel)
                    {
                        pY -= scaleWidth / 2;
                    }
                    if (yAxis.splitArea.show && i < size - 1)
                    {
                        UGL.DrawQuadrilateral(vh, new Vector2(grid.runtimeX, pY),
                            new Vector2(grid.runtimeX + grid.runtimeWidth, pY),
                            new Vector2(grid.runtimeX + grid.runtimeWidth, pY + scaleWidth),
                            new Vector2(grid.runtimeX, pY + scaleWidth),
                            yAxis.splitArea.GetColor(i, m_Theme.axis));
                    }
                    if (yAxis.splitLine.show && pY >= grid.runtimeY)
                    {
                        if (!xAxis.axisLine.show || !xAxis.axisLine.onZero || zeroPos.y != pY)
                        {
                            if (yAxis.splitLine.NeedShow(i))
                            {
                                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, new Vector3(grid.runtimeX, pY),
                                    new Vector3(grid.runtimeX + grid.runtimeWidth, pY), lineColor);
                            }
                        }
                    }
                    totalWidth += scaleWidth;
                }
            }
        }

        private void DrawYAxisTick(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = GetAxisGridOrDefault(yAxis);
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
                var size = AxisHelper.GetScaleNumber(yAxis, grid.runtimeWidth, dataZoom);
                var totalWidth = grid.runtimeY;
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(yAxis, grid.runtimeHeight, i + 1, dataZoom);
                    if (i == 0 && (!yAxis.axisTick.showStartTick || yAxis.axisTick.alignWithLabel))
                    {
                        totalWidth += scaleWidth;
                        continue;
                    }
                    if (i == size - 1 && !yAxis.axisTick.showEndTick)
                    {
                        totalWidth += scaleWidth;
                        continue;
                    }
                    if (yAxis.axisTick.show)
                    {
                        float pX = 0;
                        float pY = totalWidth;
                        if (yAxis.boundaryGap && yAxis.axisTick.alignWithLabel) pY -= scaleWidth / 2;
                        var startX = grid.runtimeX + GetYAxisOnZeroOffset(yAxis) + yAxis.offset;
                        startX -= yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                        if (yAxis.IsValue() && yAxis.IsRight()) startX += grid.runtimeWidth;
                        bool inside = yAxis.axisTick.inside;
                        if ((inside && yAxis.IsLeft()) || (!inside && yAxis.IsRight()))
                        {
                            pX += startX + yAxis.axisTick.GetLength(m_Theme.axis.tickLength);
                        }
                        else
                        {
                            pX += startX - yAxis.axisTick.GetLength(m_Theme.axis.tickLength);
                        }
                        UGL.DrawLine(vh, new Vector3(startX, pY), new Vector3(pX, pY),
                            yAxis.axisTick.GetWidth(m_Theme.axis.tickWidth),
                            yAxis.axisTick.GetColor(m_Theme.axis.tickColor));
                    }
                    totalWidth += scaleWidth;
                }
            }
            if (yAxis.show && yAxis.axisLine.show && yAxis.axisLine.showArrow)
            {
                var grid = GetAxisGridOrDefault(yAxis);
                var lineX = grid.runtimeX + GetYAxisOnZeroOffset(yAxis);
                if (yAxis.IsValue() && yAxis.IsRight()) lineX += grid.runtimeWidth;
                var inverse = yAxis.IsValue() && yAxis.inverse;
                var axisArrow = yAxis.axisLine.arrow;
                if (inverse)
                {
                    var startPos = new Vector3(lineX, grid.runtimeY + grid.runtimeHeight);
                    var arrowPos = new Vector3(lineX, grid.runtimeY);
                    UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                        axisArrow.offset, axisArrow.dent,
                        axisArrow.GetColor(yAxis.axisLine.GetColor(m_Theme.axis.lineColor)));
                }
                else
                {
                    var lineWidth = yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                    var startPos = new Vector3(lineX, grid.runtimeX);
                    var arrowPos = new Vector3(lineX, grid.runtimeY + grid.runtimeHeight + lineWidth);
                    UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                        axisArrow.offset, axisArrow.dent,
                        axisArrow.GetColor(yAxis.axisLine.GetColor(m_Theme.axis.lineColor)));
                }
            }
        }

        private void DrawXAxisSplit(VertexHelper vh, int xAxisIndex, XAxis xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var grid = GetAxisGridOrDefault(xAxis);
                if (grid == null) return;
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
                var size = AxisHelper.GetScaleNumber(xAxis, grid.runtimeWidth, dataZoom);
                var totalWidth = grid.runtimeX;
                var yAxis = m_YAxes[xAxisIndex];
                var zeroPos = new Vector3(grid.runtimeX, grid.runtimeY + yAxis.runtimeZeroYOffset);
                var lineColor = xAxis.splitLine.GetColor(m_Theme.axis.splitLineColor);
                var lineWidth = xAxis.splitLine.GetWidth(m_Theme.axis.lineWidth);
                var lineType = xAxis.splitLine.GetType(m_Theme.axis.splitLineType);
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(xAxis, grid.runtimeWidth, i + 1, dataZoom);
                    float pX = totalWidth;
                    if (xAxis.boundaryGap && xAxis.axisTick.alignWithLabel)
                    {
                        pX -= scaleWidth / 2;
                    }
                    if (xAxis.splitArea.show && i < size - 1)
                    {
                        UGL.DrawQuadrilateral(vh, new Vector2(pX, grid.runtimeY),
                            new Vector2(pX, grid.runtimeY + grid.runtimeHeight),
                            new Vector2(pX + scaleWidth, grid.runtimeY + grid.runtimeHeight),
                            new Vector2(pX + scaleWidth, grid.runtimeY),
                            xAxis.splitArea.GetColor(i, m_Theme.axis));
                    }
                    if (xAxis.splitLine.show && pX >= grid.runtimeX)
                    {
                        if (!yAxis.axisLine.show || !yAxis.axisLine.onZero || zeroPos.x != pX)
                        {
                            if (xAxis.splitLine.NeedShow(i))
                            {
                                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, new Vector3(pX, grid.runtimeY),
                                    new Vector3(pX, grid.runtimeY + grid.runtimeHeight), lineColor);
                            }
                        }
                    }
                    totalWidth += AxisHelper.GetScaleWidth(xAxis, grid.runtimeWidth, i + 1, dataZoom);
                }
            }
        }

        private void DrawXAxisTick(VertexHelper vh, int xAxisIndex, XAxis xAxis)
        {
            var grid = GetAxisGridOrDefault(xAxis);
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
                var size = AxisHelper.GetScaleNumber(xAxis, grid.runtimeWidth, dataZoom);
                var totalWidth = grid.runtimeX;
                var yAxis = m_YAxes[xAxisIndex];
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(xAxis, grid.runtimeWidth, i + 1, dataZoom);
                    if (i == 0 && (!xAxis.axisTick.showStartTick || xAxis.axisTick.alignWithLabel))
                    {
                        totalWidth += scaleWidth;
                        continue;
                    }
                    if (i == size - 1 && !xAxis.axisTick.showEndTick)
                    {
                        totalWidth += scaleWidth;
                        continue;
                    }
                    if (xAxis.axisTick.show)
                    {
                        float pX = totalWidth;
                        float pY = 0;
                        if (xAxis.boundaryGap && xAxis.axisTick.alignWithLabel) pX -= scaleWidth / 2;
                        var startY = grid.runtimeY + xAxis.offset - xAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                        if (xAxis.IsTop()) startY += grid.runtimeHeight;
                        else startY += GetXAxisOnZeroOffset(xAxis);
                        bool inside = xAxis.axisTick.inside;
                        if ((inside && xAxis.IsBottom()) || (!inside && xAxis.IsTop()))
                        {
                            pY += startY + xAxis.axisTick.GetLength(m_Theme.axis.tickLength);
                        }
                        else
                        {
                            pY += startY - xAxis.axisTick.GetLength(m_Theme.axis.tickLength);
                        }
                        UGL.DrawLine(vh, new Vector3(pX, startY), new Vector3(pX, pY),
                            xAxis.axisTick.GetWidth(m_Theme.axis.tickWidth),
                            xAxis.axisTick.GetColor(m_Theme.axis.tickColor));
                    }
                    totalWidth += scaleWidth;
                }
            }
            if (xAxis.show && xAxis.axisLine.show && xAxis.axisLine.showArrow)
            {
                var lineY = grid.runtimeY + xAxis.offset;
                if (xAxis.IsTop()) lineY += grid.runtimeHeight;
                else lineY += GetXAxisOnZeroOffset(xAxis);
                var inverse = xAxis.IsValue() && xAxis.inverse;
                var axisArrow = xAxis.axisLine.arrow;
                if (inverse)
                {
                    var startPos = new Vector3(grid.runtimeX + grid.runtimeWidth, lineY);
                    var arrowPos = new Vector3(grid.runtimeX, lineY);
                    UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                        axisArrow.offset, axisArrow.dent,
                        axisArrow.GetColor(xAxis.axisLine.GetColor(m_Theme.axis.lineColor)));
                }
                else
                {
                    var startPos = new Vector3(grid.runtimeX, lineY);
                    var arrowPos = new Vector3(grid.runtimeX + grid.runtimeWidth + xAxis.axisLine.GetWidth(m_Theme.axis.lineWidth), lineY);
                    UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                        axisArrow.offset, axisArrow.dent,
                        axisArrow.GetColor(xAxis.axisLine.GetColor(m_Theme.axis.lineColor)));
                }
            }
        }

        private void DrawXAxisLine(VertexHelper vh, int xAxisIndex, XAxis xAxis)
        {
            if (xAxis.show && xAxis.axisLine.show)
            {
                var grid = GetAxisGridOrDefault(xAxis);
                var inverse = xAxis.IsValue() && xAxis.inverse;
                var offset = AxisHelper.GetAxisLineSymbolOffset(xAxis);
                var lineY = grid.runtimeY + xAxis.offset;
                if (xAxis.IsTop()) lineY += grid.runtimeHeight;
                else lineY += GetXAxisOnZeroOffset(xAxis);
                var lineWidth = xAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                var lineType = xAxis.axisLine.GetType(m_Theme.axis.lineType);
                var lineColor = xAxis.axisLine.GetColor(m_Theme.axis.lineColor);
                var left = new Vector3(grid.runtimeX - lineWidth - (inverse ? offset : 0), lineY);
                var right = new Vector3(grid.runtimeX + grid.runtimeWidth + lineWidth + (!inverse ? offset : 0), lineY);
                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, left, right, lineColor);
            }
        }

        private void DrawYAxisLine(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (yAxis.show && yAxis.axisLine.show)
            {
                var grid = GetAxisGridOrDefault(yAxis);
                var offset = AxisHelper.GetAxisLineSymbolOffset(yAxis);
                var inverse = yAxis.IsValue() && yAxis.inverse;
                var lineX = grid.runtimeX + yAxis.offset;
                if (yAxis.IsRight()) lineX += grid.runtimeWidth;
                else lineX += GetYAxisOnZeroOffset(yAxis);
                var lineWidth = yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                var lineType = yAxis.axisLine.GetType(m_Theme.axis.lineType);
                var lineColor = yAxis.axisLine.GetColor(m_Theme.axis.lineColor);
                var bottom = new Vector3(lineX, grid.runtimeY - lineWidth - (inverse ? offset : 0));
                var top = new Vector3(lineX, grid.runtimeY + grid.runtimeHeight + lineWidth + (!inverse ? offset : 0));
                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, bottom, top, lineColor);
            }
        }



        protected void DrawXTooltipIndicator(VertexHelper vh)
        {
            if (!tooltip.show || !tooltip.IsSelected()) return;
            if (tooltip.type == Tooltip.Type.None) return;
            if (tooltip.runtimeGridIndex < 0) return;

            var grid = GetGrid(tooltip.runtimeGridIndex);
            if (grid == null) return;
            var lineType = tooltip.lineStyle.GetType(m_Theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(m_Theme.tooltip.lineWidth);
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                var xAxis = m_XAxes[i];
                if (!xAxis.show) continue;
                if (tooltip.runtimeXValues[i] < 0) continue;
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
                int dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
                float splitWidth = AxisHelper.GetDataWidth(xAxis, grid.runtimeWidth, dataCount, dataZoom);
                switch (tooltip.type)
                {
                    case Tooltip.Type.Corss:
                    case Tooltip.Type.Line:
                        float pX = (float)(grid.runtimeX + tooltip.runtimeXValues[i] * splitWidth
                            + (xAxis.boundaryGap ? splitWidth / 2 : 0));
                        if (xAxis.IsValue()) pX = tooltip.runtimePointerPos.x;
                        Vector2 sp = new Vector2(pX, grid.runtimeY);
                        Vector2 ep = new Vector2(pX, grid.runtimeY + grid.runtimeHeight);
                        var lineColor = TooltipHelper.GetLineColor(tooltip, m_Theme);
                        ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                        if (tooltip.type == Tooltip.Type.Corss)
                        {
                            sp = new Vector2(grid.runtimeX, tooltip.runtimePointerPos.y);
                            ep = new Vector2(grid.runtimeX + grid.runtimeWidth, tooltip.runtimePointerPos.y);
                            ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                        }
                        break;
                    case Tooltip.Type.Shadow:
                        float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                        pX = (float)(grid.runtimeX + splitWidth * tooltip.runtimeXValues[i] -
                            (xAxis.boundaryGap ? 0 : splitWidth / 2));
                        if (xAxis.IsValue()) pX = (float)tooltip.runtimeXValues[i];
                        float pY = grid.runtimeY + grid.runtimeHeight;
                        Vector3 p1 = new Vector3(pX, grid.runtimeY);
                        Vector3 p2 = new Vector3(pX, pY);
                        Vector3 p3 = new Vector3(pX + tooltipSplitWid, pY);
                        Vector3 p4 = new Vector3(pX + tooltipSplitWid, grid.runtimeY);
                        UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, m_Theme.tooltip.areaColor);
                        break;
                }
            }
        }

        protected void DrawYTooltipIndicator(VertexHelper vh)
        {
            if (!tooltip.show || !tooltip.IsSelected()) return;
            if (tooltip.type == Tooltip.Type.None) return;
            if (tooltip.runtimeGridIndex < 0) return;

            var grid = GetGrid(tooltip.runtimeGridIndex);
            if (grid == null) return;
            var lineType = tooltip.lineStyle.GetType(m_Theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(m_Theme.tooltip.lineWidth);
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                var yAxis = m_YAxes[i];
                if (!yAxis.show) continue;
                if (tooltip.runtimeYValues[i] < 0) continue;
                var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
                int dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
                float splitWidth = AxisHelper.GetDataWidth(yAxis, grid.runtimeHeight, dataCount, dataZoom);
                switch (tooltip.type)
                {
                    case Tooltip.Type.Corss:
                    case Tooltip.Type.Line:
                        float pY = (float)(grid.runtimeY + tooltip.runtimeYValues[i] * splitWidth + (yAxis.boundaryGap ? splitWidth / 2 : 0));
                        Vector2 sp = new Vector2(grid.runtimeX, pY);
                        Vector2 ep = new Vector2(grid.runtimeX + grid.runtimeWidth, pY);
                        var lineColor = TooltipHelper.GetLineColor(tooltip, m_Theme);
                        ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                        if (tooltip.type == Tooltip.Type.Corss)
                        {
                            sp = new Vector2(grid.runtimeX, tooltip.runtimePointerPos.y);
                            ep = new Vector2(grid.runtimeX + grid.runtimeWidth, tooltip.runtimePointerPos.y);
                            ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                        }
                        break;
                    case Tooltip.Type.Shadow:
                        float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                        float pX = grid.runtimeX + grid.runtimeWidth;
                        pY = (float)(grid.runtimeY + splitWidth * tooltip.runtimeYValues[i] -
                            (yAxis.boundaryGap ? 0 : splitWidth / 2));
                        Vector3 p1 = new Vector3(grid.runtimeX, pY);
                        Vector3 p2 = new Vector3(grid.runtimeX, pY + tooltipSplitWid);
                        Vector3 p3 = new Vector3(pX, pY + tooltipSplitWid);
                        Vector3 p4 = new Vector3(pX, pY);
                        UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, m_Theme.tooltip.areaColor);
                        break;
                }
            }
        }

        private void CheckRaycastTarget()
        {
            var anyDataZoom = false;
            foreach (var dataZoom in dataZooms)
            {
                if (dataZoom.enable) anyDataZoom = true;
            }
            var ray = anyDataZoom
                || (visualMap != null && visualMap.enable && visualMap.show && visualMap.calculable);
            if (raycastTarget != ray)
            {
                raycastTarget = ray;
            }
        }

        public bool IsAnyYAxisIsCategory()
        {
            foreach (var yAxis in m_YAxes)
            {
                if (yAxis.type == Axis.AxisType.Category)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void DrawLabelBackground(VertexHelper vh)
        {
            var isYAxis = IsAnyYAxisIsCategory();
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (!serie.show) continue;
                if (serie.IsPerformanceMode()) continue;
                if (!serie.IsCoordinateSerie()) continue;
                DataZoomHelper.GetSerieRelatedDataZoom(serie, dataZooms, out var xDataZoom, out var yDataZoom);
                var showData = serie.GetDataList(xDataZoom);
                for (int j = 0; j < showData.Count; j++)
                {
                    var serieData = showData[j];
                    if (serieData.labelObject == null) continue;
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData, serieData.highlighted);
                    serieData.index = j;
                    if (serieLabel.show && j < serie.dataPoints.Count)
                    {
                        var pos = serie.dataPoints[j];

                        var isIngore = ChartHelper.IsIngore(pos);
                        if (isIngore)
                        {
                            serieData.SetLabelActive(false);
                        }
                        else
                        {
                            Internal_RefreshLabelPosition(serie, serieData, serieLabel, pos, serie.type == SerieType.Bar, isYAxis);
                            m_RefreshLabel = true;
                            if (serieLabel.show) DrawLabelBackground(vh, serie, serieData);
                        }
                    }
                    else
                    {
                        serieData.SetLabelActive(false);
                    }
                }
            }
        }

        public void Internal_RefreshLabelPosition(Serie serie, SerieData serieData, SerieLabel serieLabel, Vector3 pos, bool IsNeedCheckPosition, bool isYAxis)
        {
            if (IsNeedCheckPosition)
            {
                var value = serieData.data[1];
                var zeroPos = Vector3.zero;
                var lastStackSerie = SeriesHelper.GetLastStackSerie(m_Series, serie.index);
                if (serieLabel.position == SerieLabel.Position.Bottom || serieLabel.position == SerieLabel.Position.Center)
                {
                    if (isYAxis)
                    {
                        var xAxis = m_XAxes[serie.xAxisIndex];
                        var grid = GetAxisGridOrDefault(xAxis);
                        zeroPos = new Vector3(grid.runtimeX + xAxis.runtimeZeroXOffset, grid.runtimeY);
                    }
                    else
                    {
                        var yAxis = m_YAxes[serie.yAxisIndex];
                        var grid = GetAxisGridOrDefault(yAxis);
                        zeroPos = new Vector3(grid.runtimeX, grid.runtimeY + yAxis.runtimeZeroYOffset);
                    }
                }
                var bottomPos = lastStackSerie == null ? zeroPos : lastStackSerie.dataPoints[serieData.index];
                switch (serieLabel.position)
                {
                    case SerieLabel.Position.Center:

                        pos = isYAxis ? new Vector3(bottomPos.x + (pos.x - bottomPos.x) / 2, pos.y) :
                            new Vector3(pos.x, bottomPos.y + (pos.y - bottomPos.y) / 2);
                        break;
                    case SerieLabel.Position.Bottom:
                        pos = isYAxis ? new Vector3(bottomPos.x, pos.y) : new Vector3(pos.x, bottomPos.y);
                        break;
                }
            }
            serieData.labelPosition = pos;
        }

        protected override void OnRefreshLabel()
        {
            base.OnRefreshLabel();
            var isYAxis = IsAnyYAxisIsCategory();
            var anyPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);

            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (serie.IsPerformanceMode()) continue;
                if (!serie.IsCoordinateSerie()) continue;
                var total = serie.yTotal;
                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, serie.stack, SerieType.Bar);
                DataZoomHelper.GetSerieRelatedDataZoom(serie, dataZooms, out var xDataZoom, out var yDataZoom);
                var showData = serie.GetDataList(xDataZoom);
                if (xDataZoom != null)
                    ChartHelper.HideAllObject(m_SerieLabelRoot, "label_" + i);
                for (int j = 0; j < showData.Count; j++)
                {
                    var serieData = showData[j];
                    if (serieData.labelObject == null) continue;
                    if (j >= serie.dataPoints.Count)
                    {
                        serieData.SetLabelActive(false);
                        serieData.SetIconActive(false);
                        continue;
                    }
                    serieData.labelObject.SetActive(true);
                    var pos = serie.dataPoints[j];
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                    var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
                    var dimension = 1;
                    var isIgnore = serie.IsIgnoreIndex(j);
                    Internal_RefreshLabelPosition(serie, serieData, serieLabel, pos, serie.type == SerieType.Bar, isYAxis);
                    serieData.labelObject.SetPosition(serieData.labelPosition);
                    serieData.labelObject.UpdateIcon(iconStyle);
                    if (serie.show && serieLabel.show && serieData.canShowLabel && !isIgnore)
                    {
                        double value = 0;

                        if (serie.type == SerieType.Heatmap)
                        {
                            dimension = VisualMapHelper.GetDimension(visualMap, serieData.data.Count);
                        }

                        SerieLabelHelper.ResetLabel(serieData.labelObject.label, serieLabel, theme, i);

                        value = serieData.data[dimension];
                        var content = "";
                        if (anyPercentStack && isPercentStack)
                        {
                            var tempTotal = Internal_GetBarSameStackTotalValue(serie.stack, j, SerieType.Custom);
                            content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, tempTotal,
                                serieLabel, theme.GetColor(i));
                        }
                        else
                        {
                            content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                                serieLabel, theme.GetColor(i));
                        }
                        serieData.SetLabelActive(serieData.labelPosition != Vector3.zero);
                        var invert = serieLabel.autoOffset
                            && serie.type == SerieType.Line
                            && SerieHelper.IsDownPoint(serie, j)
                            && !serie.areaStyle.show;
                        serieData.labelObject.SetLabelPosition(invert ? -serieLabel.offset : serieLabel.offset);
                        if (serieData.labelObject.SetText(content)) RefreshChart();
                    }
                    else
                    {
                        serieData.SetLabelActive(false);
                    }
                }
            }
        }




        public void Internal_CheckClipAndDrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 color, bool clip, Grid grid)
        {
            Internal_CheckClipAndDrawPolygon(vh, p1, p2, p3, p4, color, color, clip, grid);
        }

        public void Internal_CheckClipAndDrawPolygon(VertexHelper vh, Vector3 p, float radius, Color32 color,
            bool clip, bool vertical, Grid grid)
        {
            if (!IsInChart(p)) return;
            if (!clip || (clip && (IsInGrid(grid, p))))
                UGL.DrawSquare(vh, p, radius, color);
        }

        public void Internal_CheckClipAndDrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 startColor, Color32 toColor, bool clip, Grid grid)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ClampInChart(ref p3);
            ClampInChart(ref p4);
            if (clip)
            {
                p1 = ClampInGrid(grid, p1);
                p2 = ClampInGrid(grid, p2);
                p3 = ClampInGrid(grid, p3);
                p4 = ClampInGrid(grid, p4);
            }
            if (!clip || (clip && (IsInGrid(grid, p1) && IsInGrid(grid, p2) && IsInGrid(grid, p3) && IsInGrid(grid, p4))))
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, startColor, toColor);
        }

        public void Internal_CheckClipAndDrawPolygon(VertexHelper vh, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, ref Vector3 p4,
           Color32 startColor, Color32 toColor, bool clip, Grid grid)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ClampInChart(ref p3);
            ClampInChart(ref p4);
            if (clip)
            {
                p1 = ClampInGrid(grid, p1);
                p2 = ClampInGrid(grid, p2);
                p3 = ClampInGrid(grid, p3);
                p4 = ClampInGrid(grid, p4);
            }
            if (!clip
                || (clip && (IsInGrid(grid, p1) && IsInGrid(grid, p2) && IsInGrid(grid, p3) && IsInGrid(grid, p4))))
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, startColor, toColor);
        }

        public void Internal_CheckClipAndDrawTriangle(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Color32 color,
            bool clip, Grid grid)
        {
            Internal_CheckClipAndDrawTriangle(vh, p1, p2, p3, color, color, color, clip, grid);
        }

        public void Internal_CheckClipAndDrawTriangle(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Color32 color,
            Color32 color2, Color32 color3, bool clip, Grid grid)
        {
            if (!IsInChart(p1) || !IsInChart(p2) || !IsInChart(p3)) return;
            if (!clip || (clip && (IsInGrid(grid, p1) || IsInGrid(grid, p2) || IsInGrid(grid, p3))))
                UGL.DrawTriangle(vh, p1, p2, p3, color, color2, color3);
        }

        public void Internal_CheckClipAndDrawLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
            bool clip, Grid grid)
        {
            if (!IsInChart(p1) || !IsInChart(p2)) return;
            if (!clip || (clip && (IsInGrid(grid, p1) || IsInGrid(grid, p2))))
                UGL.DrawLine(vh, p1, p2, size, color);
        }

        public void Internal_CheckClipAndDrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize, float tickness,
            Vector3 pos, Color32 color, Color32 toColor, Color32 backgroundColor, float gap, bool clip, float[] cornerRadius,
            Grid grid, Vector3 startPos)
        {
            if (!IsInChart(pos)) return;
            if (!clip || (clip && (IsInGrid(grid, pos))))
                DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, backgroundColor, gap, cornerRadius, startPos);
        }

        public void Internal_CheckClipAndDrawZebraLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, float zebraWidth,
            float zebraGap, Color32 color, Color32 toColor, bool clip, Grid grid, float maxDistance)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            UGL.DrawZebraLine(vh, p1, p2, size, zebraWidth, zebraGap, color, toColor, maxDistance);
        }

        protected Color32 GetXLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, Grid grid)
        {
            if (ChartHelper.IsValueEqualsColor(areaColor, areaToColor)) return areaColor;
            return Color32.Lerp(areaToColor, areaColor, (pos.y - grid.runtimeY) / grid.runtimeHeight);
        }

        protected Color32 GetYLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, Grid grid)
        {
            if (ChartHelper.IsValueEqualsColor(areaColor, areaToColor)) return areaColor;
            return Color32.Lerp(areaToColor, areaColor, (pos.x - grid.runtimeX) / grid.runtimeWidth);
        }

        public Grid GetAxisGridOrDefault(Axis axis)
        {
            var index = axis.gridIndex;
            if (index >= 0 && index < m_Grids.Count)
            {
                return m_Grids[index];
            }
            else if (m_Grids.Count > 0)
            {
                return m_Grids[0];
            }
            else
            {
                return null;
            }
        }

        public Grid GetDataZoomGridOrDefault(DataZoom dataZoom)
        {
            Grid grid = null;
            if (dataZoom.xAxisIndexs != null && dataZoom.xAxisIndexs.Count > 0)
            {
                var xAxis = GetXAxis(dataZoom.xAxisIndexs[0]);
                grid = GetGrid(xAxis.gridIndex);
            }
            else if (dataZoom.yAxisIndexs != null && dataZoom.yAxisIndexs.Count > 0)
            {
                var yAxis = GetYAxis(dataZoom.yAxisIndexs[0]);
                grid = GetGrid(yAxis.gridIndex);
            }
            if (grid == null) return m_Grids[0];
            else return grid;
        }

        public Grid GetSerieGridOrDefault(Serie serie)
        {
            var xAxis = GetSerieXAxisOrDefault(serie);
            var yAxis = GetSerieYAxisOrDefault(serie);
            Grid grid = GetGrid(xAxis.gridIndex);
            if (xAxis.gridIndex != yAxis.gridIndex)
            {
                Debug.LogErrorFormat("serie {0}:{1} xAxisIndex:{2} and yAxisIndex:{3} not in the same grid.",
                    serie.index, serie.name, serie.xAxisIndex, serie.yAxisIndex);
            }
            if (grid == null)
            {
                grid = m_Grids[0];
                grid.index = 0;
            }
            else
            {
                grid.index = xAxis.gridIndex;
            }
            return grid;
        }

        public XAxis GetSerieXAxisOrDefault(Serie serie)
        {
            var axis = GetXAxis(serie.xAxisIndex);
            if (axis == null)
            {
                Debug.LogErrorFormat("serie {0}:{1} xAxisIndex:{2} not exist.", serie.index, serie.name, serie.xAxisIndex);
                axis = m_XAxes[0];
            }
            return axis;
        }

        public YAxis GetSerieYAxisOrDefault(Serie serie)
        {
            var axis = GetYAxis(serie.yAxisIndex);
            if (axis == null)
            {
                Debug.LogErrorFormat("serie {0}:{1} yAxisIndex:{2} not exist.", serie.index, serie.name, serie.xAxisIndex);
                return m_YAxes[0];
            }
            return axis;
        }

        protected void UpdateSerieGridIndex()
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                serie.index = i;
                var grid = GetSerieGridOrDefault(serie);
                serie.runtimeGridIndex = grid.index;
            }
        }

        public float GetXAxisOnZeroOffset(XAxis axis)
        {
            if (!axis.axisLine.onZero) return 0;
            foreach (var yAxis in m_YAxes)
            {
                if (yAxis.IsValue() && yAxis.gridIndex == axis.gridIndex) return yAxis.runtimeZeroYOffset;
            }
            return 0;
        }

        public float GetYAxisOnZeroOffset(YAxis axis)
        {
            if (!axis.axisLine.onZero) return 0;
            foreach (var xAxis in m_XAxes)
            {
                if (xAxis.IsValue() && xAxis.gridIndex == axis.gridIndex) return xAxis.runtimeZeroXOffset;
            }
            return 0;
        }

        public YAxis GetRelatedYAxis(XAxis axis)
        {
            foreach (var yAxis in m_YAxes)
            {
                if (yAxis.gridIndex == axis.gridIndex) return yAxis;
            }
            return m_YAxes[0];
        }

        public XAxis GetRelatedXAxis(YAxis axis)
        {
            foreach (var xAxis in m_XAxes)
            {
                if (xAxis.gridIndex == axis.gridIndex) return xAxis;
            }
            return m_XAxes[0];
        }
    }
}

