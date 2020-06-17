/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;

namespace XCharts
{
    public partial class CoordinateChart : BaseChart
    {
        private static readonly string s_DefaultDataZoom = "datazoom";
        private static readonly string s_DefaultAxisName = "name";

        [SerializeField] protected Grid m_Grid = Grid.defaultGrid;
        [SerializeField] protected List<XAxis> m_XAxises = new List<XAxis>();
        [SerializeField] protected List<YAxis> m_YAxises = new List<YAxis>();
        [SerializeField] protected DataZoom m_DataZoom = DataZoom.defaultDataZoom;
        [SerializeField] protected VisualMap m_VisualMap = new VisualMap();

        protected float m_CoordinateX;
        protected float m_CoordinateY;
        protected float m_CoordinateWidth;
        protected float m_CoordinateHeight;
        private bool m_DataZoomDrag;
        private bool m_DataZoomCoordinateDrag;
        private bool m_DataZoomStartDrag;
        private bool m_DataZoomEndDrag;
        private float m_DataZoomLastStartIndex;
        private float m_DataZoomLastEndIndex;
        private bool m_CheckMinMaxValue;
        private bool m_CheckDataZoomLabel;
        private bool m_XAxisesDirty;
        private bool m_YAxisesDirty;
        private Dictionary<int, List<Serie>> m_StackSeries = new Dictionary<int, List<Serie>>();
        private List<float> m_SeriesCurrHig = new List<float>();

        protected override void Awake()
        {
            base.Awake();
            m_CheckMinMaxValue = false;
            InitDefaultAxises();
            CheckMinMaxValue();
            InitDataZoom();
            InitAxisX();
            InitAxisY();
            m_Tooltip.UpdateToTop();
        }

        protected override void Update()
        {
            CheckMinMaxValue();
            CheckRaycastTarget();
            CheckDataZoom();
            CheckVisualMap();
            base.Update();
        }

        protected override void CheckComponent()
        {
            if (m_DataZoom.anyDirty)
            {
                if (m_DataZoom.componentDirty) InitDataZoom();
                if (m_DataZoom.vertsDirty) RefreshChart();
                m_DataZoom.ClearDirty();
            }
            if (m_VisualMap.anyDirty)
            {
                if (m_VisualMap.vertsDirty) RefreshChart();
                m_VisualMap.ClearDirty();
            }
            if (m_Grid.anyDirty)
            {
                if (m_Grid.componentDirty)
                {
                    m_XAxisesDirty = true;
                    m_YAxisesDirty = true;
                    OnCoordinateChanged();
                }
                if (m_Grid.vertsDirty) RefreshChart();
                m_Grid.ClearDirty();
            }
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                var axis = m_XAxises[i];
                if (m_XAxisesDirty || axis.anyDirty)
                {
                    if (axis.componentDirty || m_XAxisesDirty) InitXAxis(i, axis);
                    if (axis.vertsDirty || m_XAxisesDirty) RefreshChart();
                    axis.ClearDirty();
                }
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                var axis = m_YAxises[i];
                if (m_YAxisesDirty || axis.anyDirty)
                {
                    if (axis.componentDirty || m_YAxisesDirty) InitYAxis(i, axis);
                    if (axis.vertsDirty || m_YAxisesDirty) RefreshChart();
                    axis.ClearDirty();
                }
            }
            m_XAxisesDirty = false;
            m_YAxisesDirty = false;
            base.CheckComponent();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Grid = Grid.defaultGrid;
            m_XAxises.Clear();
            m_YAxises.Clear();
            Awake();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            m_XAxisesDirty = true;
            m_YAxisesDirty = true;
            m_Grid.SetAllDirty();
            m_DataZoom.SetAllDirty();
            m_VisualMap.SetAllDirty();
        }
#endif

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            DrawCoordinate(vh);
            DrawSerie(vh);
            DrawAxisTick(vh);
            DrawDataZoomSlider(vh);
            DrawVisualMap(vh);
        }

        protected override void DrawBackground(VertexHelper vh)
        {
            if (SeriesHelper.IsAnyClipSerie(m_Series))
            {
                var xLineDiff = xAxis0.axisLine.width;
                var yLineDiff = yAxis0.axisLine.width;
                var xSplitDiff = xAxis0.splitLine.lineStyle.width;
                var ySplitDiff = yAxis0.splitLine.lineStyle.width;

                var cpty = m_CoordinateY + m_CoordinateHeight + ySplitDiff;
                var cp1 = new Vector3(m_CoordinateX - yLineDiff, m_CoordinateY - xLineDiff);
                var cp2 = new Vector3(m_CoordinateX - yLineDiff, cpty);
                var cp3 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, cpty);
                var cp4 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_CoordinateY - xLineDiff);
                var backgroundColor = ThemeHelper.GetBackgroundColor(m_ThemeInfo, m_Background);
                ChartDrawer.DrawPolygon(vh, cp1, cp2, cp3, cp4, backgroundColor);
            }
            else
            {
                base.DrawBackground(vh);
            }
        }

        protected void DrawClip(VertexHelper vh)
        {
            if (!SeriesHelper.IsAnyClipSerie(m_Series)) return;
            var xLineDiff = xAxis0.axisLine.width;
            var yLineDiff = yAxis0.axisLine.width;
            var xSplitDiff = xAxis0.splitLine.lineStyle.width;
            var ySplitDiff = yAxis0.splitLine.lineStyle.width;
            var backgroundColor = ThemeHelper.GetBackgroundColor(m_ThemeInfo, m_Background);
            var lp1 = new Vector3(m_ChartX, m_ChartY);
            var lp2 = new Vector3(m_ChartX, m_ChartY + chartHeight);
            var lp3 = new Vector3(m_CoordinateX - yLineDiff, m_ChartY + chartHeight);
            var lp4 = new Vector3(m_CoordinateX - yLineDiff, m_ChartY);
            ChartDrawer.DrawPolygon(vh, lp1, lp2, lp3, lp4, backgroundColor);
            var rp1 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_ChartY);
            var rp2 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_ChartY + chartHeight);
            var rp3 = new Vector3(m_ChartX + chartWidth, m_ChartY + chartHeight);
            var rp4 = new Vector3(m_ChartX + chartWidth, m_ChartY);
            ChartDrawer.DrawPolygon(vh, rp1, rp2, rp3, rp4, backgroundColor);
            var up1 = new Vector3(m_CoordinateX - yLineDiff, m_CoordinateY + m_CoordinateHeight + ySplitDiff);
            var up2 = new Vector3(m_CoordinateX - yLineDiff, m_ChartY + chartHeight);
            var up3 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_ChartY + chartHeight);
            var up4 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_CoordinateY + m_CoordinateHeight + ySplitDiff);
            ChartDrawer.DrawPolygon(vh, up1, up2, up3, up4, backgroundColor);
            var dp1 = new Vector3(m_CoordinateX - yLineDiff, m_ChartY);
            var dp2 = new Vector3(m_CoordinateX - yLineDiff, m_CoordinateY - xLineDiff);
            var dp3 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_CoordinateY - xLineDiff);
            var dp4 = new Vector3(m_CoordinateX + m_CoordinateWidth + xSplitDiff, m_ChartY);
            ChartDrawer.DrawPolygon(vh, dp1, dp2, dp3, dp4, backgroundColor);
        }

        protected virtual void DrawSerie(VertexHelper vh)
        {
            base.DrawChart(vh);
            if (!m_CheckMinMaxValue) return;
            m_IsPlayingAnimation = false;
            bool yCategory = m_YAxises[0].IsCategory() || m_YAxises[1].IsCategory();
            SeriesHelper.GetStackSeries(m_Series, ref m_StackSeries);
            int seriesCount = m_StackSeries.Count;
            m_BarLastOffset = 0;
            for (int j = 0; j < seriesCount; j++)
            {
                var serieList = m_StackSeries[j];
                m_SeriesCurrHig.Clear();
                for (int n = 0; n < serieList.Count; n++)
                {
                    Serie serie = serieList[n];
                    serie.dataPoints.Clear();
                    var colorIndex = m_LegendRealShowName.IndexOf(serie.legendName);
                    switch (serie.type)
                    {
                        case SerieType.Line:
                            if (yCategory) DrawYLineSerie(vh, serie, colorIndex, ref m_SeriesCurrHig);
                            else DrawXLineSerie(vh, serie, colorIndex, ref m_SeriesCurrHig);
                            break;
                        case SerieType.Bar:
                            if (yCategory) DrawYBarSerie(vh, serie, colorIndex, ref m_SeriesCurrHig);
                            else DrawXBarSerie(vh, serie, colorIndex, ref m_SeriesCurrHig);
                            break;
                        case SerieType.Scatter:
                        case SerieType.EffectScatter:
                            DrawScatterSerie(vh, colorIndex, serie);
                            break;
                        case SerieType.Heatmap:
                            DrawHeatmapSerie(vh, colorIndex, serie);
                            break;
                    }
                }
            }
            DrawClip(vh);
            DrawLabelBackground(vh);
            DrawLinePoint(vh);
            DrawLineArrow(vh);
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (!IsInCooridate(local))
            {
                m_Tooltip.ClearValue();
                UpdateTooltip();
            }
            else
            {
                var isCartesian = IsValue();
                int dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
                for (int i = 0; i < m_XAxises.Count; i++)
                {
                    var xAxis = m_XAxises[i];
                    var yAxis = m_YAxises[i];
                    if (!xAxis.show && !yAxis.show) continue;
                    if (isCartesian && xAxis.show && yAxis.show)
                    {
                        var yRate = (yAxis.runtimeMaxValue - yAxis.runtimeMinValue) / m_CoordinateHeight;
                        var xRate = (xAxis.runtimeMaxValue - xAxis.runtimeMinValue) / m_CoordinateWidth;
                        var yValue = yRate * (local.y - m_CoordinateY - yAxis.runtimeZeroYOffset);
                        if (yAxis.runtimeMinValue > 0) yValue += yAxis.runtimeMinValue;
                        m_Tooltip.runtimeYValues[i] = yValue;
                        var xValue = xRate * (local.x - m_CoordinateX - xAxis.runtimeZeroXOffset);
                        if (xAxis.runtimeMinValue > 0) xValue += xAxis.runtimeMinValue;
                        m_Tooltip.runtimeXValues[i] = xValue;

                        for (int j = 0; j < m_Series.Count; j++)
                        {
                            var serie = m_Series.GetSerie(j);
                            for (int n = 0; n < serie.data.Count; n++)
                            {
                                var serieData = serie.data[n];
                                var xdata = serieData.GetData(0, xAxis.inverse);
                                var ydata = serieData.GetData(1, yAxis.inverse);
                                var symbol = SerieHelper.GetSerieSymbol(serie,serieData);
                                var symbolSize = symbol.GetSize(serieData == null ? null : serieData.data);
                                if (Mathf.Abs(xValue - xdata) / xRate < symbolSize
                                    && Mathf.Abs(yValue - ydata) / yRate < symbolSize)
                                {
                                    m_Tooltip.runtimeDataIndex[i] = n;
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

                        for (int j = 0; j < xAxis.GetDataNumber(m_DataZoom); j++)
                        {
                            float splitWid = AxisHelper.GetDataWidth(xAxis, m_CoordinateWidth, dataCount, m_DataZoom);
                            float pX = m_CoordinateX + j * splitWid;
                            if ((xAxis.boundaryGap && (local.x > pX && local.x <= pX + splitWid)) ||
                                (!xAxis.boundaryGap && (local.x > pX - splitWid / 2 && local.x <= pX + splitWid / 2)))
                            {
                                m_Tooltip.runtimeXValues[i] = j;
                                m_Tooltip.runtimeDataIndex[i] = j;
                                break;
                            }
                        }
                        for (int j = 0; j < yAxis.GetDataNumber(m_DataZoom); j++)
                        {
                            float splitWid = AxisHelper.GetDataWidth(yAxis, m_CoordinateHeight, dataCount, m_DataZoom);
                            float pY = m_CoordinateY + j * splitWid;
                            if ((yAxis.boundaryGap && (local.y > pY && local.y <= pY + splitWid)) ||
                                (!yAxis.boundaryGap && (local.y > pY - splitWid / 2 && local.y <= pY + splitWid / 2)))
                            {
                                m_Tooltip.runtimeYValues[i] = j;
                                break;
                            }
                        }
                    }
                    else if (xAxis.IsCategory())
                    {
                        var value = (yAxis.runtimeMaxValue - yAxis.runtimeMinValue) * (local.y - m_CoordinateY - yAxis.runtimeZeroYOffset) / m_CoordinateHeight;
                        if (yAxis.runtimeMinValue > 0) value += yAxis.runtimeMinValue;
                        m_Tooltip.runtimeYValues[i] = value;
                        for (int j = 0; j < xAxis.GetDataNumber(m_DataZoom); j++)
                        {
                            float splitWid = AxisHelper.GetDataWidth(xAxis, m_CoordinateWidth, dataCount, m_DataZoom);
                            float pX = m_CoordinateX + j * splitWid;
                            if ((xAxis.boundaryGap && (local.x > pX && local.x <= pX + splitWid)) ||
                                (!xAxis.boundaryGap && (local.x > pX - splitWid / 2 && local.x <= pX + splitWid / 2)))
                            {
                                m_Tooltip.runtimeXValues[i] = j;
                                m_Tooltip.runtimeDataIndex[i] = j;
                                break;
                            }
                        }
                    }
                    else if (yAxis.IsCategory())
                    {
                        var value = (xAxis.runtimeMaxValue - xAxis.runtimeMinValue) * (local.x - m_CoordinateX - xAxis.runtimeZeroXOffset) / m_CoordinateWidth;
                        if (xAxis.runtimeMinValue > 0) value += xAxis.runtimeMinValue;
                        m_Tooltip.runtimeXValues[i] = value;
                        for (int j = 0; j < yAxis.GetDataNumber(m_DataZoom); j++)
                        {
                            float splitWid = AxisHelper.GetDataWidth(yAxis, m_CoordinateHeight, dataCount, m_DataZoom);
                            float pY = m_CoordinateY + j * splitWid;
                            if ((yAxis.boundaryGap && (local.y > pY && local.y <= pY + splitWid)) ||
                                (!yAxis.boundaryGap && (local.y > pY - splitWid / 2 && local.y <= pY + splitWid / 2)))
                            {
                                m_Tooltip.runtimeYValues[i] = j;
                                m_Tooltip.runtimeDataIndex[i] = j;
                                break;
                            }
                        }
                    }
                }
            }
            if (m_Tooltip.IsSelected())
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                UpdateTooltip();
                if (m_Tooltip.IsDataIndexChanged() || m_Tooltip.type == Tooltip.Type.Corss)
                {
                    m_Tooltip.UpdateLastDataIndex();
                    RefreshChart();
                }
            }
            else if (m_Tooltip.IsActive())
            {
                m_Tooltip.SetActive(false);
                RefreshChart();
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
                index = m_Tooltip.runtimeDataIndex[0];
                tempAxis = m_XAxises[0];
            }
            else if (m_XAxises[0].type == Axis.AxisType.Value)
            {
                index = (int)m_Tooltip.runtimeYValues[0];
                tempAxis = m_YAxises[0];
            }
            else
            {
                index = (int)m_Tooltip.runtimeXValues[0];
                tempAxis = m_XAxises[0];
            }
            if (index < 0)
            {
                if (m_Tooltip.IsActive())
                {
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            var category = tempAxis.GetData(index, m_DataZoom);
            var content = TooltipHelper.GetFormatterContent(m_Tooltip, index, m_Series, m_ThemeInfo, category,
                m_DataZoom, isCartesian);
            TooltipHelper.SetContentAndPosition(m_Tooltip, content, chartRect);
            m_Tooltip.SetActive(true);

            for (int i = 0; i < m_XAxises.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_XAxises[i]);
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_YAxises[i]);
            }
        }

        protected void UpdateAxisTooltipLabel(int axisIndex, Axis axis)
        {
            var showTooltipLabel = axis.show && m_Tooltip.type == Tooltip.Type.Corss;
            axis.SetTooltipLabelActive(showTooltipLabel);
            if (!showTooltipLabel) return;
            string labelText = "";
            Vector2 labelPos = Vector2.zero;
            if (axis is XAxis)
            {
                var posY = axisIndex > 0 ? m_CoordinateY + m_CoordinateHeight : m_CoordinateY;
                var diff = axisIndex > 0 ? -axis.axisLabel.fontSize - axis.axisLabel.margin - 3.5f : axis.axisLabel.margin / 2 + 1;
                if (axis.IsValue())
                {
                    labelText = ChartCached.NumberToStr(m_Tooltip.runtimeXValues[axisIndex], axis.axisLabel.numericFormatter);
                    labelPos = new Vector2(m_Tooltip.runtimePointerPos.x, posY - diff);
                }
                else
                {
                    labelText = axis.GetData((int)m_Tooltip.runtimeXValues[axisIndex], m_DataZoom);
                    float splitWidth = AxisHelper.GetSplitWidth(axis, m_CoordinateWidth, m_DataZoom);
                    int index = (int)m_Tooltip.runtimeXValues[axisIndex];
                    float px = m_CoordinateX + index * splitWidth + (axis.boundaryGap ? splitWidth / 2 : 0) + 0.5f;
                    labelPos = new Vector2(px, posY - diff);
                }
            }
            else if (axis is YAxis)
            {
                var posX = axisIndex > 0 ? m_CoordinateX + m_CoordinateWidth : m_CoordinateX;
                var diff = axisIndex > 0 ? -axis.axisLabel.margin + 3 : axis.axisLabel.margin - 3;
                if (axis.IsValue())
                {
                    labelText = ChartCached.NumberToStr(m_Tooltip.runtimeYValues[axisIndex], axis.axisLabel.numericFormatter);
                    labelPos = new Vector2(posX - diff, m_Tooltip.runtimePointerPos.y);
                }
                else
                {
                    labelText = axis.GetData((int)m_Tooltip.runtimeYValues[axisIndex], m_DataZoom);
                    float splitWidth = AxisHelper.GetSplitWidth(axis, m_CoordinateHeight, m_DataZoom);
                    int index = (int)m_Tooltip.runtimeYValues[axisIndex];
                    float py = m_CoordinateY + index * splitWidth + (axis.boundaryGap ? splitWidth / 2 : 0);
                    labelPos = new Vector2(posX - diff, py);
                }
            }
            axis.UpdateTooptipLabelText(labelText);
            axis.UpdateTooltipLabelPos(labelPos);
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            m_DataZoom.SetAllDirty();
            m_XAxisesDirty = true;
            m_YAxisesDirty = true;
        }

        private void InitDefaultAxises()
        {
            if (m_XAxises.Count <= 0)
            {
                var axis1 = XAxis.defaultXAxis;
                var axis2 = XAxis.defaultXAxis;
                axis1.show = true;
                axis2.show = false;
                m_XAxises.Add(axis1);
                m_XAxises.Add(axis2);
            }
            if (m_YAxises.Count <= 0)
            {
                var axis1 = YAxis.defaultYAxis;
                var axis2 = YAxis.defaultYAxis;
                axis1.show = true;
                axis1.splitNumber = 5;
                axis1.boundaryGap = false;
                axis2.show = false;
                m_YAxises.Add(axis1);
                m_YAxises.Add(axis2);
            }
            foreach (var axis in m_XAxises) axis.runtimeMinValue = axis.runtimeMaxValue = 0;
            foreach (var axis in m_YAxises) axis.runtimeMinValue = axis.runtimeMaxValue = 0;
        }

        private void InitAxisY()
        {
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                InitYAxis(i, m_YAxises[i]);
            }
        }

        private void InitYAxis(int yAxisIndex, YAxis yAxis)
        {
            yAxis.axisLabelTextList.Clear();
            string objName = ChartCached.GetYAxisName(yAxisIndex);
            var axisObj = ChartHelper.AddObject(objName, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(yAxis.show && yAxis.axisLabel.show);
            axisObj.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var labelColor = ChartHelper.IsClearColor(yAxis.axisLabel.color) ?
                (Color)m_ThemeInfo.axisTextColor :
                yAxis.axisLabel.color;
            int splitNumber = AxisHelper.GetSplitNumber(yAxis, m_CoordinateHeight, m_DataZoom);
            float totalWidth = 0;
            for (int i = 0; i < splitNumber; i++)
            {
                Text txt;
                bool inside = yAxis.axisLabel.inside;
                if ((inside && yAxisIndex == 0) || (!inside && yAxisIndex == 1))
                {
                    txt = ChartHelper.AddTextObject(objName + i, axisObj.transform,
                        m_ThemeInfo.font, labelColor, TextAnchor.MiddleLeft, Vector2.zero,
                        Vector2.zero, new Vector2(0, 0.5f), new Vector2(m_Grid.left, 20),
                        yAxis.axisLabel.fontSize, yAxis.axisLabel.rotate, yAxis.axisLabel.fontStyle);
                }
                else
                {
                    txt = ChartHelper.AddTextObject(objName + i, axisObj.transform,
                        m_ThemeInfo.font, labelColor, TextAnchor.MiddleRight, Vector2.zero,
                        Vector2.zero, new Vector2(1, 0.5f), new Vector2(m_Grid.left, 20),
                        yAxis.axisLabel.fontSize, yAxis.axisLabel.rotate, yAxis.axisLabel.fontStyle);
                }

                float labelWidth = AxisHelper.GetScaleWidth(yAxis, m_CoordinateHeight, i, m_DataZoom);
                if (i == 0) yAxis.axisLabel.SetRelatedText(txt, labelWidth);
                txt.transform.localPosition = GetLabelYPosition(totalWidth + (yAxis.boundaryGap ? labelWidth / 2 : 0), i, yAxisIndex, yAxis);

                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                txt.text = AxisHelper.GetLabelName(yAxis, m_CoordinateHeight, i, yAxis.runtimeMinValue, yAxis.runtimeMaxValue, m_DataZoom, isPercentStack);
                txt.gameObject.SetActive(yAxis.show &&
                    (yAxis.axisLabel.interval == 0 || i % (yAxis.axisLabel.interval + 1) == 0));
                yAxis.axisLabelTextList.Add(txt);
                totalWidth += labelWidth;
            }
            if (yAxis.axisName.show)
            {
                var color = ChartHelper.IsClearColor(yAxis.axisName.color) ? (Color)m_ThemeInfo.axisTextColor :
                    yAxis.axisName.color;
                var fontSize = yAxis.axisName.fontSize;
                var offset = yAxis.axisName.offset;
                Text axisName = null;
                var zeroPos = new Vector3(m_CoordinateX + m_XAxises[yAxisIndex].runtimeZeroXOffset, m_CoordinateY);
                switch (yAxis.axisName.location)
                {
                    case AxisName.Location.Start:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                             new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), fontSize,
                             yAxis.axisName.rotate, yAxis.axisName.fontStyle);
                        axisName.transform.localPosition = yAxisIndex > 0 ?
                            new Vector2(m_CoordinateX + m_CoordinateWidth + offset.x, m_CoordinateY - offset.y) :
                            new Vector2(zeroPos.x + offset.x, m_CoordinateY - offset.y);
                        break;
                    case AxisName.Location.Middle:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform,
                            m_ThemeInfo.font, color, TextAnchor.MiddleRight, new Vector2(1, 0.5f),
                            new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 20), fontSize,
                            yAxis.axisName.rotate, yAxis.axisName.fontStyle);
                        axisName.transform.localPosition = yAxisIndex > 0 ?
                        new Vector2(m_CoordinateX + m_CoordinateWidth - offset.x, m_CoordinateY + m_CoordinateHeight / 2 + offset.y) :
                        new Vector2(m_CoordinateX - offset.x, m_CoordinateY + m_CoordinateHeight / 2 + offset.y);
                        break;
                    case AxisName.Location.End:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                             new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), fontSize,
                             yAxis.axisName.rotate, yAxis.axisName.fontStyle);
                        axisName.transform.localPosition = yAxisIndex > 0 ?
                            new Vector2(m_CoordinateX + m_CoordinateWidth + offset.x, m_CoordinateY + m_CoordinateHeight + offset.y) :
                            new Vector2(zeroPos.x + offset.x, m_CoordinateY + m_CoordinateHeight + offset.y);
                        break;
                }
                axisName.text = yAxis.axisName.name;
            }
            //init tooltip label
            if (m_Tooltip.runtimeGameObject)
            {
                Vector2 privot = yAxisIndex > 0 ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
                var labelParent = m_Tooltip.runtimeGameObject.transform;
                GameObject labelObj = ChartHelper.AddTooltipLabel(ChartCached.GetAxisTooltipLabel(objName), labelParent, m_ThemeInfo.font, privot);
                yAxis.SetTooltipLabel(labelObj);
                yAxis.SetTooltipLabelColor(m_ThemeInfo.tooltipBackgroundColor, m_ThemeInfo.tooltipTextColor);
                yAxis.SetTooltipLabelActive(yAxis.show && m_Tooltip.show && m_Tooltip.type == Tooltip.Type.Corss);
            }
        }

        private void InitAxisX()
        {
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                InitXAxis(i, m_XAxises[i]);
            }
        }

        private void InitXAxis(int xAxisIndex, XAxis xAxis)
        {
            xAxis.axisLabelTextList.Clear();

            string objName = ChartCached.GetXAxisName(xAxisIndex);
            var axisObj = ChartHelper.AddObject(objName, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(xAxis.show && xAxis.axisLabel.show);
            axisObj.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var labelColor = ChartHelper.IsClearColor(xAxis.axisLabel.color) ?
                (Color)m_ThemeInfo.axisTextColor :
                xAxis.axisLabel.color;
            int splitNumber = AxisHelper.GetSplitNumber(xAxis, m_CoordinateWidth, m_DataZoom);
            float totalWidth = 0;
            for (int i = 0; i < splitNumber; i++)
            {
                float labelWidth = AxisHelper.GetScaleWidth(xAxis, m_CoordinateWidth, i, m_DataZoom);
                bool inside = xAxis.axisLabel.inside;
                Text txt = ChartHelper.AddTextObject(ChartCached.GetXAxisName(xAxisIndex, i), axisObj.transform,
                    m_ThemeInfo.font, labelColor, TextAnchor.MiddleCenter, new Vector2(0, 1),
                    new Vector2(0, 1), new Vector2(1, 0.5f), new Vector2(labelWidth, 20),
                    xAxis.axisLabel.fontSize, xAxis.axisLabel.rotate, xAxis.axisLabel.fontStyle);
                if (i == 0) xAxis.axisLabel.SetRelatedText(txt, labelWidth);
                txt.transform.localPosition = GetLabelXPosition(totalWidth + (xAxis.boundaryGap ? labelWidth : labelWidth / 2),
                    i, xAxisIndex, xAxis);
                totalWidth += labelWidth;
                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                txt.text = AxisHelper.GetLabelName(xAxis, m_CoordinateWidth, i, xAxis.runtimeMinValue, xAxis.runtimeMaxValue, m_DataZoom,
                    isPercentStack);
                txt.gameObject.SetActive(xAxis.show &&
                    (xAxis.axisLabel.interval == 0 || i % (xAxis.axisLabel.interval + 1) == 0));
                xAxis.axisLabelTextList.Add(txt);
            }
            if (xAxis.axisName.show)
            {
                var color = ChartHelper.IsClearColor(xAxis.axisName.color) ? (Color)m_ThemeInfo.axisTextColor :
                    xAxis.axisName.color;
                var fontSize = xAxis.axisName.fontSize;
                var offset = xAxis.axisName.offset;
                Text axisName = null;
                var zeroPos = new Vector3(m_CoordinateX, m_CoordinateY + m_YAxises[xAxisIndex].runtimeZeroYOffset);
                switch (xAxis.axisName.location)
                {
                    case AxisName.Location.Start:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform,
                            m_ThemeInfo.font, color, TextAnchor.MiddleRight, new Vector2(1, 0.5f),
                            new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 20), fontSize,
                            xAxis.axisName.rotate, xAxis.axisName.fontStyle);
                        axisName.transform.localPosition = xAxisIndex > 0 ?
                            new Vector2(zeroPos.x - offset.x, m_CoordinateY + m_CoordinateHeight + offset.y) :
                            new Vector2(zeroPos.x - offset.x, zeroPos.y + offset.y);
                        break;
                    case AxisName.Location.Middle:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                             new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 20), fontSize,
                             xAxis.axisName.rotate, xAxis.axisName.fontStyle);
                        axisName.transform.localPosition = xAxisIndex > 0 ?
                            new Vector2(m_CoordinateX + m_CoordinateWidth / 2 + offset.x, m_CoordinateY + m_CoordinateHeight - offset.y) :
                            new Vector2(m_CoordinateX + m_CoordinateWidth / 2 + offset.x, m_CoordinateY - offset.y);
                        break;
                    case AxisName.Location.End:
                        axisName = ChartHelper.AddTextObject(s_DefaultAxisName, axisObj.transform,
                             m_ThemeInfo.font, color, TextAnchor.MiddleLeft, new Vector2(0, 0.5f),
                             new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(100, 20), fontSize,
                             xAxis.axisName.rotate, xAxis.axisName.fontStyle);
                        axisName.transform.localPosition = xAxisIndex > 0 ?
                            new Vector2(m_CoordinateX + m_CoordinateWidth + offset.x, m_CoordinateY + m_CoordinateHeight + offset.y) :
                            new Vector2(m_CoordinateX + m_CoordinateWidth + offset.x, zeroPos.y + offset.y);
                        break;
                }
                axisName.text = xAxis.axisName.name;
            }
            if (m_Tooltip.runtimeGameObject)
            {
                Vector2 privot = xAxisIndex > 0 ? new Vector2(0.5f, 1) : new Vector2(0.5f, 1);
                var labelParent = m_Tooltip.runtimeGameObject.transform;
                GameObject labelObj = ChartHelper.AddTooltipLabel(ChartCached.GetAxisTooltipLabel(objName), labelParent, m_ThemeInfo.font, privot);
                xAxis.SetTooltipLabel(labelObj);
                xAxis.SetTooltipLabelColor(m_ThemeInfo.tooltipBackgroundColor, m_ThemeInfo.tooltipTextColor);
                xAxis.SetTooltipLabelActive(xAxis.show && m_Tooltip.show && m_Tooltip.type == Tooltip.Type.Corss);
            }
        }

        private void InitDataZoom()
        {
            var dataZoomObject = ChartHelper.AddObject(s_DefaultDataZoom, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            dataZoomObject.transform.localPosition = Vector3.zero;
            dataZoomObject.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(dataZoomObject);
            var startLabel = ChartHelper.AddTextObject(s_DefaultDataZoom + "start",
                dataZoomObject.transform, m_ThemeInfo.font, m_ThemeInfo.dataZoomTextColor, TextAnchor.MiddleRight,
                Vector2.zero, Vector2.zero, new Vector2(1, 0.5f), new Vector2(200, 20), m_DataZoom.fontSize, 0, m_DataZoom.fontStyle);
            var endLabel = ChartHelper.AddTextObject(s_DefaultDataZoom + "end",
                dataZoomObject.transform, m_ThemeInfo.font, m_ThemeInfo.dataZoomTextColor, TextAnchor.MiddleLeft,
                Vector2.zero, Vector2.zero, new Vector2(0, 0.5f), new Vector2(200, 20), m_DataZoom.fontSize, 0, m_DataZoom.fontStyle);
            m_DataZoom.SetStartLabel(startLabel);
            m_DataZoom.SetEndLabel(endLabel);
            m_DataZoom.SetLabelActive(false);
            CheckRaycastTarget();
            var xAxis = m_XAxises[m_DataZoom.xAxisIndex];
            if (xAxis != null)
            {
                xAxis.UpdateFilterData(m_DataZoom);
            }
            if (m_Series != null)
            {
                m_Series.UpdateFilterData(m_DataZoom);
            }
        }

        private Vector3 GetLabelYPosition(float scaleWid, int i, int yAxisIndex, YAxis yAxis)
        {
            var startX = yAxisIndex == 0 ? m_CoordinateX : m_CoordinateX + m_CoordinateWidth;
            var posX = 0f;
            var inside = yAxis.axisLabel.inside;
            if ((inside && yAxisIndex == 0) || (!inside && yAxisIndex == 1))
            {
                posX = startX + yAxis.axisLabel.margin;
            }
            else
            {
                posX = startX - yAxis.axisLabel.margin;
            }
            return new Vector3(posX, m_CoordinateY + scaleWid, 0);
        }

        private Vector3 GetLabelXPosition(float scaleWid, int i, int xAxisIndex, XAxis xAxis)
        {
            var startY = m_CoordinateY + (xAxis.axisLabel.onZero ? m_YAxises[xAxisIndex].runtimeZeroYOffset : 0);
            if (xAxisIndex > 0) startY += m_CoordinateHeight;
            var posY = 0f;
            var inside = xAxis.axisLabel.inside;
            if ((inside && xAxisIndex == 0) || (!inside && xAxisIndex == 1))
            {
                posY = startY + xAxis.axisLabel.margin + xAxis.axisLabel.fontSize / 2;
            }
            else
            {
                posY = startY - xAxis.axisLabel.margin - xAxis.axisLabel.fontSize / 2;
            }
            return new Vector3(m_CoordinateX + scaleWid, posY);
        }

        private void CheckMinMaxValue()
        {
            if (m_XAxises == null || m_YAxises == null) return;
            if (IsCategory())
            {
                m_CheckMinMaxValue = true;
                return;
            }
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                UpdateAxisMinMaxValue(i, m_XAxises[i]);
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                UpdateAxisMinMaxValue(i, m_YAxises[i]);
            }
        }

        private void UpdateAxisMinMaxValue(int axisIndex, Axis axis, bool updateChart = true)
        {
            if (axis.IsCategory() || !axis.show) return;
            float tempMinValue = 0;
            float tempMaxValue = 0;

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
            if (tempMinValue != axis.runtimeMinValue || tempMaxValue != axis.runtimeMaxValue)
            {
                m_CheckMinMaxValue = true;
                m_IsPlayingAnimation = true;
                var needCheck = !m_IsPlayingAnimation && axis.runtimeLastCheckInverse == axis.inverse;
                axis.UpdateMinValue(tempMinValue, needCheck);
                axis.UpdateMaxValue(tempMaxValue, needCheck);
                axis.runtimeZeroXOffset = 0;
                axis.runtimeZeroYOffset = 0;
                axis.runtimeLastCheckInverse = axis.inverse;
                if (tempMinValue != 0 || tempMaxValue != 0)
                {
                    if (axis is XAxis && axis.IsValue())
                    {
                        axis.runtimeZeroXOffset = axis.runtimeMinValue > 0 ? 0 :
                            axis.runtimeMaxValue < 0 ? this.m_CoordinateWidth :
                            Mathf.Abs(axis.runtimeMinValue) * (this.m_CoordinateWidth / (Mathf.Abs(axis.runtimeMinValue) + Mathf.Abs(axis.runtimeMaxValue)));
                    }
                    if (axis is YAxis && axis.IsValue())
                    {
                        axis.runtimeZeroYOffset = axis.runtimeMinValue > 0 ? 0 :
                            axis.runtimeMaxValue < 0 ? m_CoordinateHeight :
                            Mathf.Abs(axis.runtimeMinValue) * (m_CoordinateHeight / (Mathf.Abs(axis.runtimeMinValue) + Mathf.Abs(axis.runtimeMaxValue)));
                    }
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

        protected void UpdateAxisLabelText(Axis axis)
        {
            float m_CoordinateWidth = axis is XAxis ? this.m_CoordinateWidth : m_CoordinateHeight;
            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
            axis.UpdateLabelText(m_CoordinateWidth, m_DataZoom, isPercentStack, 500);
        }

        protected virtual void OnCoordinateChanged()
        {
            UpdateCoordinate();
            m_XAxisesDirty = true;
            m_YAxisesDirty = true;
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            OnCoordinateChanged();
        }

        private void DrawCoordinate(VertexHelper vh)
        {
            DrawGrid(vh);
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                DrawXAxisSplit(vh, i, m_XAxises[i]);
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                DrawYAxisSplit(vh, i, m_YAxises[i]);
            }
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                DrawXAxisLine(vh, i, m_XAxises[i]);
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                DrawYAxisLine(vh, i, m_YAxises[i]);
            }
        }

        private void DrawAxisTick(VertexHelper vh)
        {
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                DrawXAxisTick(vh, i, m_XAxises[i]);
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                DrawYAxisTick(vh, i, m_YAxises[i]);
            }
        }

        private void DrawGrid(VertexHelper vh)
        {
            if (m_Grid.show && !ChartHelper.IsClearColor(m_Grid.backgroundColor))
            {
                var p1 = new Vector2(m_CoordinateX, m_CoordinateY);
                var p2 = new Vector2(m_CoordinateX, m_CoordinateY + m_CoordinateHeight);
                var p3 = new Vector2(m_CoordinateX + m_CoordinateWidth, m_CoordinateY + m_CoordinateHeight);
                var p4 = new Vector2(m_CoordinateX + m_CoordinateWidth, m_CoordinateY);
                ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, m_Grid.backgroundColor);
            }
        }

        private void DrawYAxisSplit(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var size = AxisHelper.GetScaleNumber(yAxis, m_CoordinateWidth, m_DataZoom);
                var totalWidth = m_CoordinateY;
                var xAxis = m_XAxises[yAxisIndex];
                var zeroPos = new Vector3(m_CoordinateX + xAxis.runtimeZeroXOffset, m_CoordinateY + yAxis.runtimeZeroYOffset);
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(yAxis, m_CoordinateHeight, i, m_DataZoom);
                    float pY = totalWidth;
                    if (yAxis.boundaryGap && yAxis.axisTick.alignWithLabel)
                    {
                        pY -= scaleWidth / 2;
                    }
                    if (yAxis.splitArea.show && i < size - 1)
                    {
                        ChartDrawer.DrawPolygon(vh, new Vector2(m_CoordinateX, pY),
                            new Vector2(m_CoordinateX + m_CoordinateWidth, pY),
                            new Vector2(m_CoordinateX + m_CoordinateWidth, pY + scaleWidth),
                            new Vector2(m_CoordinateX, pY + scaleWidth),
                            yAxis.splitArea.getColor(i));
                    }
                    if (yAxis.splitLine.show)
                    {
                        if (!xAxis.axisLine.show || !xAxis.axisLine.onZero || zeroPos.y != pY)
                        {
                            if (yAxis.splitLine.NeedShow(i))
                            {
                                ChartDrawer.DrawLineStyle(vh, yAxis.splitLine.lineStyle, new Vector3(m_CoordinateX, pY),
                                    new Vector3(m_CoordinateX + m_CoordinateWidth, pY), yAxis.splitLine.GetColor(m_ThemeInfo));
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
                var size = AxisHelper.GetScaleNumber(yAxis, m_CoordinateWidth, m_DataZoom);
                var totalWidth = m_CoordinateY;
                var xAxis = m_XAxises[yAxisIndex];
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(yAxis, m_CoordinateHeight, i, m_DataZoom);
                    float pX = 0;
                    float pY = totalWidth;
                    if (yAxis.boundaryGap && yAxis.axisTick.alignWithLabel)
                    {
                        pY -= scaleWidth / 2;
                    }
                    if (yAxis.axisTick.show && i > 0)
                    {
                        var startX = m_CoordinateX + (yAxis.axisLine.onZero ? m_XAxises[yAxisIndex].runtimeZeroXOffset : 0);
                        startX -= yAxis.axisLine.width;
                        if (yAxis.IsValue() && yAxisIndex > 0) startX += m_CoordinateWidth;
                        bool inside = yAxis.axisTick.inside;
                        if ((inside && yAxisIndex == 0) || (!inside && yAxisIndex == 1))
                        {
                            pX += startX + yAxis.axisTick.length;
                        }
                        else
                        {
                            pX += startX - yAxis.axisTick.length;
                        }
                        ChartDrawer.DrawLine(vh, new Vector3(startX, pY), new Vector3(pX, pY),
                            AxisHelper.GetTickWidth(yAxis), m_ThemeInfo.axisLineColor);
                    }
                    totalWidth += scaleWidth;
                }
            }
            if (yAxis.show && yAxis.axisLine.show && yAxis.axisLine.symbol)
            {
                var lineX = m_CoordinateX + (yAxis.axisLine.onZero ? m_XAxises[yAxisIndex].runtimeZeroXOffset : 0);
                if (yAxis.IsValue() && yAxisIndex > 0) lineX += m_CoordinateWidth;
                var inverse = yAxis.IsValue() && yAxis.inverse;
                if (inverse)
                {
                    var startPos = new Vector3(lineX, m_CoordinateY + m_CoordinateHeight);
                    var arrowPos = new Vector3(lineX, m_CoordinateY);
                    var axisLine = yAxis.axisLine;
                    ChartDrawer.DrawArrow(vh, startPos, arrowPos, axisLine.symbolWidth, axisLine.symbolHeight,
                        axisLine.symbolOffset, axisLine.symbolDent, m_ThemeInfo.axisLineColor);
                }
                else
                {
                    var startPos = new Vector3(lineX, m_CoordinateX);
                    var arrowPos = new Vector3(lineX, m_CoordinateY + m_CoordinateHeight + yAxis.axisLine.width);
                    var axisLine = yAxis.axisLine;
                    ChartDrawer.DrawArrow(vh, startPos, arrowPos, axisLine.symbolWidth, axisLine.symbolHeight,
                        axisLine.symbolOffset, axisLine.symbolDent, m_ThemeInfo.axisLineColor);
                }
            }
        }

        private void DrawXAxisSplit(VertexHelper vh, int xAxisIndex, XAxis xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var size = AxisHelper.GetScaleNumber(xAxis, m_CoordinateWidth, m_DataZoom);
                var totalWidth = m_CoordinateX;
                var yAxis = m_YAxises[xAxisIndex];
                var zeroPos = new Vector3(m_CoordinateX, m_CoordinateY + yAxis.runtimeZeroYOffset);
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(xAxis, m_CoordinateWidth, i, m_DataZoom);
                    float pX = totalWidth;
                    if (xAxis.boundaryGap && xAxis.axisTick.alignWithLabel)
                    {
                        pX -= scaleWidth / 2;
                    }
                    if (xAxis.splitArea.show && i < size - 1)
                    {
                        ChartDrawer.DrawPolygon(vh, new Vector2(pX, m_CoordinateY),
                            new Vector2(pX, m_CoordinateY + m_CoordinateHeight),
                            new Vector2(pX + scaleWidth, m_CoordinateY + m_CoordinateHeight),
                            new Vector2(pX + scaleWidth, m_CoordinateY),
                            xAxis.splitArea.getColor(i));
                    }
                    if (xAxis.splitLine.show)
                    {
                        if (!yAxis.axisLine.show || !yAxis.axisLine.onZero || zeroPos.x != pX)
                        {
                            if (xAxis.splitLine.NeedShow(i))
                            {
                                ChartDrawer.DrawLineStyle(vh, xAxis.splitLine.lineStyle, new Vector3(pX, m_CoordinateY),
                                    new Vector3(pX, m_CoordinateY + m_CoordinateHeight), xAxis.splitLine.GetColor(m_ThemeInfo));
                            }
                        }
                    }
                    totalWidth += scaleWidth;
                }
            }
        }

        private void DrawXAxisTick(VertexHelper vh, int xAxisIndex, XAxis xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var size = AxisHelper.GetScaleNumber(xAxis, m_CoordinateWidth, m_DataZoom);
                var totalWidth = m_CoordinateX;
                var yAxis = m_YAxises[xAxisIndex];
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(xAxis, m_CoordinateWidth, i, m_DataZoom);
                    float pX = totalWidth;
                    float pY = 0;
                    if (xAxis.boundaryGap && xAxis.axisTick.alignWithLabel)
                    {
                        pX -= scaleWidth / 2;
                    }
                    if (xAxis.axisTick.show && i > 0)
                    {
                        var startY = m_CoordinateY + (xAxis.axisLine.onZero ? m_YAxises[xAxisIndex].runtimeZeroYOffset : 0);
                        startY -= xAxis.axisLine.width;
                        if (xAxis.IsValue() && xAxisIndex > 0) startY += m_CoordinateHeight;
                        bool inside = xAxis.axisTick.inside;
                        if ((inside && xAxisIndex == 0) || (!inside && xAxisIndex == 1))
                        {
                            pY += startY + xAxis.axisTick.length;
                        }
                        else
                        {
                            pY += startY - xAxis.axisTick.length;
                        }
                        ChartDrawer.DrawLine(vh, new Vector3(pX, startY), new Vector3(pX, pY),
                            AxisHelper.GetTickWidth(xAxis), m_ThemeInfo.axisLineColor);
                    }
                    totalWidth += scaleWidth;
                }
            }
            if (xAxis.show && xAxis.axisLine.show && xAxis.axisLine.symbol)
            {
                var lineY = m_CoordinateY + (xAxis.axisLine.onZero ? m_YAxises[xAxisIndex].runtimeZeroYOffset : 0);
                if (xAxis.IsValue() && xAxisIndex > 0) lineY += m_CoordinateHeight;
                var inverse = xAxis.IsValue() && xAxis.inverse;
                if (inverse)
                {
                    var startPos = new Vector3(m_CoordinateX + m_CoordinateWidth, lineY);
                    var arrowPos = new Vector3(m_CoordinateX, lineY);
                    var axisLine = xAxis.axisLine;
                    ChartDrawer.DrawArrow(vh, startPos, arrowPos, axisLine.symbolWidth, axisLine.symbolHeight,
                        axisLine.symbolOffset, axisLine.symbolDent, m_ThemeInfo.axisLineColor);
                }
                else
                {
                    var startPos = new Vector3(m_CoordinateX, lineY);
                    var arrowPos = new Vector3(m_CoordinateX + m_CoordinateWidth + xAxis.axisLine.width, lineY);
                    var axisLine = xAxis.axisLine;
                    ChartDrawer.DrawArrow(vh, startPos, arrowPos, axisLine.symbolWidth, axisLine.symbolHeight,
                        axisLine.symbolOffset, axisLine.symbolDent, m_ThemeInfo.axisLineColor);
                }
            }
        }

        private void DrawXAxisLine(VertexHelper vh, int xAxisIndex, XAxis xAxis)
        {
            if (xAxis.show && xAxis.axisLine.show)
            {
                var inverse = xAxis.IsValue() && xAxis.inverse;
                var offset = AxisHelper.GetAxisLineSymbolOffset(xAxis);
                var lineY = m_CoordinateY + (xAxis.axisLine.onZero ? m_YAxises[xAxisIndex].runtimeZeroYOffset : 0);
                if (xAxis.IsValue() && xAxisIndex > 0) lineY += m_CoordinateHeight;
                var left = new Vector3(m_CoordinateX - xAxis.axisLine.width - (inverse ? offset : 0), lineY);
                var right = new Vector3(m_CoordinateX + m_CoordinateWidth + xAxis.axisLine.width + (!inverse ? offset : 0), lineY);
                ChartDrawer.DrawLine(vh, left, right, xAxis.axisLine.width, m_ThemeInfo.axisLineColor);
            }
        }

        private void DrawYAxisLine(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (yAxis.show && yAxis.axisLine.show)
            {
                var offset = AxisHelper.GetAxisLineSymbolOffset(yAxis);
                var inverse = yAxis.IsValue() && yAxis.inverse;
                var lineX = m_CoordinateX + (yAxis.axisLine.onZero ? m_XAxises[yAxisIndex].runtimeZeroXOffset : 0);
                if (yAxis.IsValue() && yAxisIndex > 0) lineX += m_CoordinateWidth;
                var bottom = new Vector3(lineX, m_CoordinateY - yAxis.axisLine.width - (inverse ? offset : 0));
                var top = new Vector3(lineX, m_CoordinateY + m_CoordinateHeight + yAxis.axisLine.width + (!inverse ? offset : 0));
                ChartDrawer.DrawLine(vh, bottom, top, yAxis.axisLine.width, m_ThemeInfo.axisLineColor);
            }
        }

        private void DrawDataZoomSlider(VertexHelper vh)
        {
            if (!m_DataZoom.enable || !m_DataZoom.supportSlider) return;
            var hig = m_DataZoom.GetHeight(grid.bottom);
            var p1 = new Vector3(m_CoordinateX, m_ChartY + m_DataZoom.bottom);
            var p2 = new Vector3(m_CoordinateX, m_ChartY + m_DataZoom.bottom + hig);
            var p3 = new Vector3(m_CoordinateX + m_CoordinateWidth, m_ChartY + m_DataZoom.bottom + hig);
            var p4 = new Vector3(m_CoordinateX + m_CoordinateWidth, m_ChartY + m_DataZoom.bottom);
            var xAxis = xAxises[0];
            ChartDrawer.DrawLine(vh, p1, p2, xAxis.axisLine.width, m_ThemeInfo.dataZoomLineColor);
            ChartDrawer.DrawLine(vh, p2, p3, xAxis.axisLine.width, m_ThemeInfo.dataZoomLineColor);
            ChartDrawer.DrawLine(vh, p3, p4, xAxis.axisLine.width, m_ThemeInfo.dataZoomLineColor);
            ChartDrawer.DrawLine(vh, p4, p1, xAxis.axisLine.width, m_ThemeInfo.dataZoomLineColor);
            if (m_DataZoom.showDataShadow && m_Series.Count > 0)
            {
                Serie serie = m_Series.list[0];
                Axis axis = yAxises[0];
                var showData = serie.GetDataList(null);
                float scaleWid = m_CoordinateWidth / (showData.Count - 1);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                float minValue = 0;
                float maxValue = 0;
                SeriesHelper.GetYMinMaxValue(m_Series, null, 0, IsValue(), axis.inverse, out minValue, out maxValue);
                AxisHelper.AdjustMinMaxValue(axis, ref minValue, ref maxValue, true);

                int rate = 1;
                var sampleDist = serie.sampleDist < 2 ? 2 : serie.sampleDist;
                var maxCount = showData.Count;
                if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (m_CoordinateWidth / sampleDist));
                if (rate < 1) rate = 1;
                var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                    DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
                var dataChanging = false;
                for (int i = 0; i < maxCount; i += rate)
                {
                    float value = SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage, i,
                    serie.animation.GetUpdateAnimationDuration(), ref dataChanging, axis.inverse);
                    float pX = m_CoordinateX + i * scaleWid;
                    float dataHig = (maxValue - minValue) == 0 ? 0 :
                        (value - minValue) / (maxValue - minValue) * hig;
                    np = new Vector3(pX, m_ChartY + m_DataZoom.bottom + dataHig);
                    if (i > 0)
                    {
                        Color color = m_ThemeInfo.dataZoomLineColor;
                        ChartDrawer.DrawLine(vh, lp, np, xAxis.axisLine.width, color);
                        Vector3 alp = new Vector3(lp.x, lp.y - xAxis.axisLine.width);
                        Vector3 anp = new Vector3(np.x, np.y - xAxis.axisLine.width);
                        Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                        Vector3 tnp = new Vector3(np.x, m_ChartY + m_DataZoom.bottom + xAxis.axisLine.width);
                        Vector3 tlp = new Vector3(lp.x, m_ChartY + m_DataZoom.bottom + xAxis.axisLine.width);
                        ChartDrawer.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                    }
                    lp = np;
                }
                if (dataChanging)
                {
                    RefreshChart();
                }
            }
            switch (m_DataZoom.rangeMode)
            {
                case DataZoom.RangeMode.Percent:
                    var start = m_CoordinateX + m_CoordinateWidth * m_DataZoom.start / 100;
                    var end = m_CoordinateX + m_CoordinateWidth * m_DataZoom.end / 100;
                    p1 = new Vector2(start, m_ChartY + m_DataZoom.bottom);
                    p2 = new Vector2(start, m_ChartY + m_DataZoom.bottom + hig);
                    p3 = new Vector2(end, m_ChartY + m_DataZoom.bottom + hig);
                    p4 = new Vector2(end, m_ChartY + m_DataZoom.bottom);
                    ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.dataZoomSelectedColor);
                    ChartDrawer.DrawLine(vh, p1, p2, xAxis.axisLine.width, m_ThemeInfo.dataZoomSelectedColor);
                    ChartDrawer.DrawLine(vh, p3, p4, xAxis.axisLine.width, m_ThemeInfo.dataZoomSelectedColor);
                    break;
            }
        }

        protected void DrawXTooltipIndicator(VertexHelper vh)
        {
            if (!m_Tooltip.show || !m_Tooltip.IsSelected()) return;
            if (m_Tooltip.type == Tooltip.Type.None) return;
            int dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
            for (int i = 0; i < m_XAxises.Count; i++)
            {
                var xAxis = m_XAxises[i];
                var yAxis = m_YAxises[i];
                if (!xAxis.show) continue;
                float splitWidth = AxisHelper.GetDataWidth(xAxis, m_CoordinateWidth, dataCount, m_DataZoom);
                switch (m_Tooltip.type)
                {
                    case Tooltip.Type.Corss:
                    case Tooltip.Type.Line:
                        float pX = m_CoordinateX + m_Tooltip.runtimeXValues[i] * splitWidth
                            + (xAxis.boundaryGap ? splitWidth / 2 : 0);
                        if (xAxis.IsValue()) pX = m_Tooltip.runtimePointerPos.x;
                        Vector2 sp = new Vector2(pX, m_CoordinateY);
                        Vector2 ep = new Vector2(pX, m_CoordinateY + m_CoordinateHeight);
                        var lineColor = TooltipHelper.GetLineColor(tooltip, m_ThemeInfo);
                        ChartDrawer.DrawLineStyle(vh, m_Tooltip.lineStyle, sp, ep, lineColor);
                        if (m_Tooltip.type == Tooltip.Type.Corss)
                        {
                            sp = new Vector2(m_CoordinateX, m_Tooltip.runtimePointerPos.y);
                            ep = new Vector2(m_CoordinateX + m_CoordinateWidth, m_Tooltip.runtimePointerPos.y);
                            ChartDrawer.DrawLineStyle(vh, m_Tooltip.lineStyle, sp, ep, lineColor);
                        }
                        break;
                    case Tooltip.Type.Shadow:
                        float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                        pX = m_CoordinateX + splitWidth * m_Tooltip.runtimeXValues[i] -
                            (xAxis.boundaryGap ? 0 : splitWidth / 2);
                        if (xAxis.IsValue()) pX = m_Tooltip.runtimeXValues[i];
                        float pY = m_CoordinateY + m_CoordinateHeight;
                        Vector3 p1 = new Vector3(pX, m_CoordinateY);
                        Vector3 p2 = new Vector3(pX, pY);
                        Vector3 p3 = new Vector3(pX + tooltipSplitWid, pY);
                        Vector3 p4 = new Vector3(pX + tooltipSplitWid, m_CoordinateY);
                        ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                        break;
                }
            }
        }

        protected void DrawYTooltipIndicator(VertexHelper vh)
        {
            if (!m_Tooltip.show || !m_Tooltip.IsSelected()) return;
            if (m_Tooltip.type == Tooltip.Type.None) return;
            int dataCount = m_Series.list.Count > 0 ? m_Series.list[0].GetDataList(dataZoom).Count : 0;
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                var yAxis = m_YAxises[i];
                var xAxis = m_XAxises[i];
                if (!yAxis.show) continue;
                float splitWidth = AxisHelper.GetDataWidth(yAxis, m_CoordinateHeight, dataCount, m_DataZoom);
                switch (m_Tooltip.type)
                {
                    case Tooltip.Type.Corss:
                    case Tooltip.Type.Line:
                        float pY = m_CoordinateY + m_Tooltip.runtimeYValues[i] * splitWidth + (yAxis.boundaryGap ? splitWidth / 2 : 0);
                        Vector2 sp = new Vector2(m_CoordinateX, pY);
                        Vector2 ep = new Vector2(m_CoordinateX + m_CoordinateWidth, pY);
                        var lineColor = TooltipHelper.GetLineColor(tooltip, m_ThemeInfo);
                        ChartDrawer.DrawLineStyle(vh, m_Tooltip.lineStyle, sp, ep, lineColor);
                        if (m_Tooltip.type == Tooltip.Type.Corss)
                        {
                            sp = new Vector2(m_CoordinateX, m_Tooltip.runtimePointerPos.y);
                            ep = new Vector2(m_CoordinateX + m_CoordinateWidth, m_Tooltip.runtimePointerPos.y);
                            ChartDrawer.DrawLineStyle(vh, m_Tooltip.lineStyle, sp, ep, lineColor);
                        }
                        break;
                    case Tooltip.Type.Shadow:
                        float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                        float pX = m_CoordinateX + m_CoordinateWidth;
                        pY = m_CoordinateY + splitWidth * m_Tooltip.runtimeYValues[i] -
                            (yAxis.boundaryGap ? 0 : splitWidth / 2);
                        Vector3 p1 = new Vector3(m_CoordinateX, pY);
                        Vector3 p2 = new Vector3(m_CoordinateX, pY + tooltipSplitWid);
                        Vector3 p3 = new Vector3(pX, pY + tooltipSplitWid);
                        Vector3 p4 = new Vector3(pX, pY);
                        ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                        break;
                }
            }
        }

        private void CheckRaycastTarget()
        {
            var ray = m_DataZoom.enable || (m_VisualMap.enable && m_VisualMap.show && m_VisualMap.calculable);
            if (raycastTarget != ray)
            {
                raycastTarget = ray;
            }
        }

        private void CheckDataZoom()
        {
            if (!m_DataZoom.enable) return;
            CheckDataZoomScale();
            CheckDataZoomLabel();
        }

        private bool m_IsSingleTouch;
        private Vector2 m_LastSingleTouchPos;
        private Vector2 m_LastTouchPos0;
        private Vector2 m_LastTouchPos1;
        private void CheckDataZoomScale()
        {
            if (!m_DataZoom.enable || m_DataZoom.zoomLock || !m_DataZoom.supportInside) return;

            if (Input.touchCount == 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began)
                {
                    m_LastTouchPos0 = touch0.position;
                    m_LastTouchPos1 = touch1.position;
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    var tempPos0 = touch0.position;
                    var tempPos1 = touch1.position;
                    var currDist = Vector2.Distance(tempPos0, tempPos1);
                    var lastDist = Vector2.Distance(m_LastTouchPos0, m_LastTouchPos1);
                    var delta = (currDist - lastDist);
                    ScaleDataZoom(delta / m_DataZoom.scrollSensitivity);
                    m_LastTouchPos0 = tempPos0;
                    m_LastTouchPos1 = tempPos1;
                }
            }
        }

        private void CheckDataZoomLabel()
        {
            if (m_DataZoom.supportSlider && m_DataZoom.showDetail)
            {
                Vector2 local;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                    Input.mousePosition, canvas.worldCamera, out local))
                {
                    m_DataZoom.SetLabelActive(false);
                    return;
                }
                if (m_DataZoom.IsInSelectedZoom(local, m_CoordinateX, chartY, m_CoordinateWidth)
                    || m_DataZoom.IsInStartZoom(local, m_CoordinateX, chartY, m_CoordinateWidth)
                    || m_DataZoom.IsInEndZoom(local, m_CoordinateX, chartY, m_CoordinateWidth))
                {
                    m_DataZoom.SetLabelActive(true);
                    RefreshDataZoomLabel();
                }
                else
                {

                    m_DataZoom.SetLabelActive(false);
                }
            }
            if (m_CheckDataZoomLabel)
            {
                m_CheckDataZoomLabel = false;
                var xAxis = m_XAxises[m_DataZoom.xAxisIndex];
                var startIndex = (int)((xAxis.data.Count - 1) * m_DataZoom.start / 100);
                var endIndex = (int)((xAxis.data.Count - 1) * m_DataZoom.end / 100);
                if (m_DataZoomLastStartIndex != startIndex || m_DataZoomLastEndIndex != endIndex)
                {
                    m_DataZoomLastStartIndex = startIndex;
                    m_DataZoomLastEndIndex = endIndex;
                    if (xAxis.data.Count > 0)
                    {
                        m_DataZoom.SetStartLabelText(xAxis.data[startIndex]);
                        m_DataZoom.SetEndLabelText(xAxis.data[endIndex]);
                    }
                    InitAxisX();
                }
                var start = m_CoordinateX + m_CoordinateWidth * m_DataZoom.start / 100;
                var end = m_CoordinateX + m_CoordinateWidth * m_DataZoom.end / 100;
                var hig = m_DataZoom.GetHeight(grid.bottom);
                m_DataZoom.UpdateStartLabelPosition(new Vector3(start - 10, chartY + m_DataZoom.bottom + hig / 2));
                m_DataZoom.UpdateEndLabelPosition(new Vector3(end + 10, chartY + m_DataZoom.bottom + hig / 2));
            }
        }

        protected void DrawLabelBackground(VertexHelper vh)
        {
            var isYAxis = m_YAxises[0].type == Axis.AxisType.Category
                || m_YAxises[1].type == Axis.AxisType.Category;
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (!serie.show) continue;

                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (serieData.labelObject == null) continue;
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData, serieData.highlighted);
                    serieData.index = j;
                    if ((serieLabel.show || serieData.iconStyle.show))
                    {
                        var pos = serie.dataPoints[j];

                        var isIngore = ChartHelper.IsIngore(pos);
                        if (isIngore)
                        {
                            serieData.SetLabelActive(false);
                        }
                        else
                        {
                            var value = serieData.data[1];
                            switch (serie.type)
                            {
                                case SerieType.Line:
                                    break;
                                case SerieType.Bar:
                                    var zeroPos = Vector3.zero;
                                    var lastStackSerie = SeriesHelper.GetLastStackSerie(m_Series, n);
                                    if (serie.type == SerieType.Bar)
                                    {
                                        if (serieLabel.position == SerieLabel.Position.Bottom || serieLabel.position == SerieLabel.Position.Center)
                                        {
                                            if (isYAxis)
                                            {
                                                var xAxis = m_XAxises[serie.axisIndex];
                                                zeroPos = new Vector3(m_CoordinateX + xAxis.runtimeZeroXOffset, m_CoordinateY);
                                            }
                                            else
                                            {
                                                var yAxis = m_YAxises[serie.axisIndex];
                                                zeroPos = new Vector3(m_CoordinateX, m_CoordinateY + yAxis.runtimeZeroYOffset);
                                            }
                                        }
                                    }
                                    var bottomPos = lastStackSerie == null ? zeroPos : lastStackSerie.dataPoints[j];
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
                                    break;
                            }
                            m_RefreshLabel = true;
                            serieData.labelPosition = pos;
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

        protected override void OnRefreshLabel()
        {
            var anyPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (serie.IsPerformanceMode()) continue;
                var total = serie.yTotal;
                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, serie.stack, SerieType.Bar);
                for (int j = 0; j < serie.data.Count; j++)
                {
                    if (j >= serie.dataPoints.Count) break;
                    var serieData = serie.data[j];
                    if (serieData.labelObject == null) continue;
                    var pos = serie.dataPoints[j];
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                    var dimension = 1;
                    var isIgnore = serie.IsIgnoreIndex(j, 1);
                    serieData.labelObject.SetPosition(serieData.labelPosition);
                    serieData.labelObject.UpdateIcon(serieData.iconStyle);
                    if (serie.show && serieLabel.show && serieData.canShowLabel && !isIgnore)
                    {
                        float value = 0f;

                        if (serie.type == SerieType.Heatmap)
                        {
                            dimension = m_VisualMap.enable && m_VisualMap.dimension > 0 ? m_VisualMap.dimension - 1 :
                            serieData.data.Count - 1;
                        }

                        SerieLabelHelper.ResetLabel(serieData, serieLabel, themeInfo, i);

                        value = serieData.data[dimension];
                        var content = "";
                        if (anyPercentStack && isPercentStack)
                        {
                            var tempTotal = GetSameStackTotalValue(serie.stack, j);
                            content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, tempTotal, serieLabel);
                        }
                        else
                        {
                            content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total, serieLabel);
                        }
                        serieData.SetLabelActive(value != 0 && serieData.labelPosition != Vector3.zero);
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

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (Input.touchCount > 1) return;
            Vector2 pos;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out pos))
            {
                return;
            }
            if (m_DataZoom.supportInside)
            {
                if (IsInCooridate(pos))
                {
                    m_DataZoomCoordinateDrag = true;
                }
            }
            if (m_DataZoom.supportSlider)
            {
                if (m_DataZoom.IsInStartZoom(pos, m_CoordinateX, chartY, m_CoordinateWidth))
                {
                    m_DataZoomStartDrag = true;
                }
                else if (m_DataZoom.IsInEndZoom(pos, m_CoordinateX, chartY, m_CoordinateWidth))
                {
                    m_DataZoomEndDrag = true;
                }
                else if (m_DataZoom.IsInSelectedZoom(pos, m_CoordinateX, chartY, m_CoordinateWidth))
                {
                    m_DataZoomDrag = true;
                }
            }
            OnDragVisualMapStart();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (Input.touchCount > 1) return;
            float deltaX = eventData.delta.x;
            float deltaPercent = deltaX / m_CoordinateWidth * 100;
            OnDragInside(deltaPercent);
            OnDragSlider(deltaPercent);
            OnDragVisualMap();
        }

        private void OnDragInside(float deltaPercent)
        {
            if (Input.touchCount > 1) return;
            if (!m_DataZoom.supportInside) return;
            if (!m_DataZoomCoordinateDrag) return;
            var diff = m_DataZoom.end - m_DataZoom.start;
            if (deltaPercent > 0)
            {
                m_DataZoom.start -= deltaPercent;
                m_DataZoom.end = m_DataZoom.start + diff;
            }
            else
            {
                m_DataZoom.end += -deltaPercent;
                m_DataZoom.start = m_DataZoom.end - diff;
            }
            RefreshDataZoomLabel();
            RefreshChart();
        }

        private void OnDragSlider(float deltaPercent)
        {
            if (Input.touchCount > 1) return;
            if (!m_DataZoom.supportSlider) return;
            if (m_DataZoomStartDrag)
            {
                m_DataZoom.start += deltaPercent;
                if (m_DataZoom.start > m_DataZoom.end)
                {
                    m_DataZoom.start = m_DataZoom.end;
                    m_DataZoomEndDrag = true;
                    m_DataZoomStartDrag = false;
                }
                if (m_DataZoom.realtime)
                {
                    RefreshDataZoomLabel();
                    RefreshChart();
                }
            }
            else if (m_DataZoomEndDrag)
            {
                m_DataZoom.end += deltaPercent;
                if (m_DataZoom.end < m_DataZoom.start)
                {
                    m_DataZoom.end = m_DataZoom.start;
                    m_DataZoomStartDrag = true;
                    m_DataZoomEndDrag = false;
                }
                if (m_DataZoom.realtime)
                {
                    RefreshDataZoomLabel();
                    RefreshChart();
                }
            }
            else if (m_DataZoomDrag)
            {
                if (deltaPercent > 0)
                {
                    if (m_DataZoom.end + deltaPercent > 100)
                    {
                        deltaPercent = 100 - m_DataZoom.end;
                    }
                }
                else
                {
                    if (m_DataZoom.start + deltaPercent < 0)
                    {
                        deltaPercent = -m_DataZoom.start;
                    }
                }
                m_DataZoom.start += deltaPercent;
                m_DataZoom.end += deltaPercent;
                if (m_DataZoom.realtime)
                {
                    RefreshDataZoomLabel();
                    RefreshChart();
                }
            }
        }

        private void RefreshDataZoomLabel()
        {
            m_CheckDataZoomLabel = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (m_DataZoomDrag || m_DataZoomStartDrag || m_DataZoomEndDrag || m_DataZoomCoordinateDrag)
            {
                RefreshChart();
            }
            m_DataZoomDrag = false;
            m_DataZoomCoordinateDrag = false;
            m_DataZoomStartDrag = false;
            m_DataZoomEndDrag = false;
            OnDragVisualMapEnd();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (Input.touchCount > 1) return;
            Vector2 localPos;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out localPos))
            {
                return;
            }

            if (m_DataZoom.IsInStartZoom(localPos, m_CoordinateX, chartY, m_CoordinateWidth) ||
                m_DataZoom.IsInEndZoom(localPos, m_CoordinateX, chartY, m_CoordinateWidth))
            {
                return;
            }
            if (m_DataZoom.IsInZoom(localPos, m_CoordinateX, chartY, m_CoordinateWidth)
                && !m_DataZoom.IsInSelectedZoom(localPos, m_CoordinateX, chartY, m_CoordinateWidth))
            {
                var pointerX = localPos.x;
                var selectWidth = m_CoordinateWidth * (m_DataZoom.end - m_DataZoom.start) / 100;
                var startX = pointerX - selectWidth / 2;
                var endX = pointerX + selectWidth / 2;
                if (startX < m_CoordinateX)
                {
                    startX = m_CoordinateX;
                    endX = m_CoordinateX + selectWidth;
                }
                else if (endX > m_CoordinateX + m_CoordinateWidth)
                {
                    endX = m_CoordinateX + m_CoordinateWidth;
                    startX = m_CoordinateX + m_CoordinateWidth - selectWidth;
                }
                m_DataZoom.start = (startX - m_CoordinateX) / m_CoordinateWidth * 100;
                m_DataZoom.end = (endX - m_CoordinateX) / m_CoordinateWidth * 100;
                RefreshDataZoomLabel();
                RefreshChart();
            }
        }

        public override void OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
            if (Input.touchCount > 1) return;
            if (!m_DataZoom.enable || m_DataZoom.zoomLock) return;
            Vector2 pos;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out pos))
            {
                return;
            }
            if (!IsInCooridate(pos) && !m_DataZoom.IsInSelectedZoom(pos, m_CoordinateX, chartY, m_CoordinateWidth))
            {
                return;
            }
            ScaleDataZoom(eventData.scrollDelta.y * m_DataZoom.scrollSensitivity);
        }

        private void ScaleDataZoom(float delta)
        {
            float deltaPercent = Mathf.Abs(delta / m_CoordinateWidth * 100);
            if (delta > 0)
            {
                if (m_DataZoom.end <= m_DataZoom.start) return;
                m_DataZoom.end -= deltaPercent;
                m_DataZoom.start += deltaPercent;
                if (m_DataZoom.end <= m_DataZoom.start)
                {
                    m_DataZoom.end = m_DataZoom.start;
                }
            }
            else
            {
                m_DataZoom.end += deltaPercent;
                m_DataZoom.start -= deltaPercent;
                if (m_DataZoom.end > 100) m_DataZoom.end = 100;
                if (m_DataZoom.start < 0) m_DataZoom.start = 0;
            }
            RefreshDataZoomLabel();
            RefreshChart();
        }

        protected void CheckClipAndDrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 color, bool clip)
        {
            CheckClipAndDrawPolygon(vh, p1, p2, p3, p4, color, color, clip);
        }

        protected void CheckClipAndDrawPolygon(VertexHelper vh, Vector3 p, float radius, Color32 color,
            bool clip, bool vertical = true)
        {
            if (!IsInChart(p)) return;
            if (!clip || (clip && (IsInCooridate(p))))
                ChartDrawer.DrawPolygon(vh, p, radius, color, vertical);
        }

        protected void CheckClipAndDrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 startColor, Color32 toColor, bool clip)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ClampInChart(ref p3);
            ClampInChart(ref p4);
            if (clip)
            {
                p1 = ClampInCoordinate(p1);
                p2 = ClampInCoordinate(p2);
                p3 = ClampInCoordinate(p3);
                p4 = ClampInCoordinate(p4);
            }
            if (!clip || (clip && (IsInCooridate(p1) && IsInCooridate(p2) && IsInCooridate(p3) && IsInCooridate(p4))))
                ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, startColor, toColor);
        }

        protected void CheckClipAndDrawPolygon(VertexHelper vh, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, ref Vector3 p4,
           Color32 startColor, Color32 toColor, bool clip)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ClampInChart(ref p3);
            ClampInChart(ref p4);
            if (clip)
            {
                p1 = ClampInCoordinate(p1);
                p2 = ClampInCoordinate(p2);
                p3 = ClampInCoordinate(p3);
                p4 = ClampInCoordinate(p4);
            }
            if (!clip || (clip && (IsInCooridate(p1) && IsInCooridate(p2) && IsInCooridate(p3) && IsInCooridate(p4))))
                ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, startColor, toColor);
        }

        protected void CheckClipAndDrawTriangle(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3,
            Color32 color, bool clip)
        {
            CheckClipAndDrawTriangle(vh, p1, p2, p3, color, color, color, clip);
        }

        protected void CheckClipAndDrawTriangle(VertexHelper vh, Vector3 p1,
           Vector3 p2, Vector3 p3, Color32 color, Color32 color2, Color32 color3, bool clip)
        {
            if (!IsInChart(p1) || !IsInChart(p2) || !IsInChart(p3)) return;
            if (!clip || (clip && (IsInCooridate(p1) || IsInCooridate(p2) || IsInCooridate(p3))))
                ChartDrawer.DrawTriangle(vh, p1, p2, p3, color, color2, color3);
        }

        protected void CheckClipAndDrawLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size,
            Color32 color, bool clip)
        {
            if (!IsInChart(p1) || !IsInChart(p2)) return;
            if (!clip || (clip && (IsInCooridate(p1) || IsInCooridate(p2))))
                ChartDrawer.DrawLine(vh, p1, p2, size, color);
        }

        protected void CheckClipAndDrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
            float tickness, Vector3 pos, Color color, Color toColor, float gap, bool clip, float[] cornerRadius)
        {
            if (!IsInChart(pos)) return;
            if (!clip || (clip && (IsInCooridate(pos))))
                DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, gap, cornerRadius);
        }

        protected void CheckClipAndDrawZebraLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size,
            float zebraWidth, float zebraGap, Color32 color, bool clip)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ChartDrawer.DrawZebraLine(vh, p1, p2, size, zebraWidth, zebraGap, color);
        }

        protected Color GetXLerpColor(Color areaColor, Color areaToColor, Vector3 pos)
        {
            if (areaColor == areaToColor) return areaColor;
            return Color.Lerp(areaToColor, areaColor, (pos.y - m_CoordinateY) / m_CoordinateHeight);
        }

        protected Color GetYLerpColor(Color areaColor, Color areaToColor, Vector3 pos)
        {
            if (areaColor == areaToColor) return areaColor;
            return Color.Lerp(areaToColor, areaColor, (pos.x - m_CoordinateX) / m_CoordinateWidth);
        }
    }
}

