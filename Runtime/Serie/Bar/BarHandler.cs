using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed partial class BarHandler : SerieHandler<Bar>
    {
        List<List<SerieData>> m_StackSerieData = new List<List<SerieData>>();
        private GridCoord m_SerieGrid;
        private float[] m_CapusleDefaultCornerRadius = new float[] { 1, 1, 1, 1 };

        public override void Update()
        {
            base.Update();
            if (serie.IsUseCoord<GridCoord>())
                UpdateSerieGridContext();
            else if (serie.IsUseCoord<PolarCoord>())
                UpdateSeriePolarContext();
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
            if (serie.IsUseCoord<PolarCoord>())
            {
                DrawPolarBar(vh, serie);
            }
            else if (serie.IsUseCoord<GridCoord>())
            {
                DrawBarSerie(vh, serie);
            }
        }

        public override Vector3 GetSerieDataLabelPosition(SerieData serieData, LabelStyle label)
        {
            if (serie.IsUseCoord<PolarCoord>())
            {
                switch (label.position)
                {
                    case LabelStyle.Position.Bottom:
                        var center = serieData.context.areaCenter;
                        var angle = serieData.context.halfAngle;
                        var radius = serieData.context.insideRadius;
                        return ChartHelper.GetPosition(center, angle, radius);
                    case LabelStyle.Position.Top:
                        center = serieData.context.areaCenter;
                        angle = serieData.context.halfAngle;
                        radius = serieData.context.outsideRadius;
                        return ChartHelper.GetPosition(center, angle, radius);
                    default:
                        return serieData.context.position;
                }
            }
            else
            {
                switch (label.position)
                {
                    case LabelStyle.Position.Bottom:
                        var center = serieData.context.rect.center;
                        return new Vector3(center.x, center.y - serieData.context.rect.height / 2);
                    case LabelStyle.Position.Center:
                    case LabelStyle.Position.Inside:
                        return serieData.context.rect.center;
                    default:
                        return serieData.context.position;
                }
            }
        }

        private void UpdateSerieGridContext()
        {
            if (m_SerieGrid == null)
                return;

            var needCheck = (chart.isPointerInChart && m_SerieGrid.IsPointerEnter() && !serie.placeHolder) || m_LegendEnter;
            var needInteract = false;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    foreach (var serieData in serie.data)
                    {
                        serieData.context.highlight = false;
                        serieData.interact.Reset();
                    }
                    chart.RefreshPainter(serie);
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            Color32 color, toColor;
            if (m_LegendEnter)
            {
                serie.context.pointerEnter = true;
                foreach (var serieData in serie.data)
                {
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme);
                    serieData.interact.SetColor(ref needInteract, color, toColor);
                }
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                foreach (var serieData in serie.data)
                {
                    if (serie.context.pointerAxisDataIndexs.Contains(serieData.index) ||
                        serieData.context.rect.Contains(chart.pointerPos))
                    {
                        serie.context.pointerItemDataIndex = serieData.index;
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
                chart.RefreshPainter(serie);
            }
        }

        private void DrawBarSerie(VertexHelper vh, Bar serie)
        {
            if (!serie.show || serie.animation.HasFadeOut())
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

            var axisLength = isY ? m_SerieGrid.context.height : m_SerieGrid.context.width;
            var relativedAxisLength = isY ? m_SerieGrid.context.width : m_SerieGrid.context.height;
            var axisXY = isY ? m_SerieGrid.context.y : m_SerieGrid.context.x;

            var isStack = SeriesHelper.IsStack<Bar>(chart.series, serie.stack);
            if (isStack)
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);

            var barCount = chart.GetSerieBarRealCount<Bar>();
            float categoryWidth = AxisHelper.GetDataWidth(axis, axisLength, showData.Count, dataZoom);
            float barGap = chart.GetSerieBarGap<Bar>();
            float totalBarWidth = chart.GetSerieTotalWidth<Bar>(categoryWidth, barGap, barCount);
            float barWidth = serie.GetBarWidth(categoryWidth, barCount);
            float offset = (categoryWidth - totalBarWidth) * 0.5f;
            var serieReadIndex = chart.GetSerieIndexIfStack<Bar>(serie);
            float gap = serie.barGap == -1 ? offset : offset + chart.GetSerieTotalGap<Bar>(categoryWidth, barGap, serieReadIndex);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;
            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series, serie.stack);
            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double yMinValue = relativedAxis.context.minValue;
            double yMaxValue = relativedAxis.context.maxValue;

            var areaColor = ColorUtil.clearColor32;
            var areaToColor = ColorUtil.clearColor32;
            var interacting = false;

            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;
            serie.animation.InitProgress(axisXY, axisXY + axisLength);
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (!serieData.show || serie.IsIgnoreValue(serieData))
                {
                    serie.context.dataPoints.Add(Vector3.zero);
                    serie.context.dataIndexs.Add(serieData.index);
                    continue;
                }

                if (serieData.IsDataChanged())
                    dataChanging = true;

                var state = SerieHelper.GetSerieState(serie, serieData);
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, state);
                var value = axis.IsCategory() ? i : serieData.GetData(0, axis.inverse);
                var relativedValue = serieData.GetCurrData(1, dataChangeDuration, relativedAxis.inverse, yMinValue, yMaxValue, serie.animation.unscaledTime);
                var borderWidth = relativedValue == 0 ? 0 : itemStyle.runtimeBorderWidth;
                var borderGap = relativedValue == 0 ? 0 : itemStyle.borderGap;
                var borderGapAndWidth = borderWidth + borderGap;
                var backgroundColor = itemStyle.backgroundColor;

                if (!serieData.interact.TryGetColor(ref areaColor, ref areaToColor, ref interacting))
                {
                    SerieHelper.GetItemColor(out areaColor, out areaToColor, serie, serieData, chart.theme);
                    serieData.interact.SetColor(ref interacting, areaColor, areaToColor);
                }

                var pX = 0f;
                var pY = 0f;
                UpdateXYPosition(m_SerieGrid, isY, axis, relativedAxis, i, categoryWidth, barWidth, isStack, value, ref pX, ref pY);
                var barHig = 0f;
                if (isPercentStack)
                {
                    var valueTotal = chart.GetSerieSameStackTotalValue<Bar>(serie.stack, i);
                    barHig = valueTotal != 0 ? (float)(relativedValue / valueTotal * relativedAxisLength) : 0;
                }
                else
                {
                    barHig = AxisHelper.GetAxisValueLength(m_SerieGrid, relativedAxis, categoryWidth, relativedValue);
                }

                float currHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, barHig);
                Vector3 plb, plt, prt, prb, top;
                UpdateRectPosition(m_SerieGrid, isY, relativedValue, pX, pY, gap, borderWidth, barWidth, currHig,
                    out plb, out plt, out prt, out prb, out top);
                serieData.context.stackHeight = barHig;
                serieData.context.position = top;
                serieData.context.rect = Rect.MinMaxRect(plb.x + borderGapAndWidth, plb.y + borderGapAndWidth,
                    prt.x - borderGapAndWidth, prt.y - borderGapAndWidth);
                serieData.context.backgroundRect = isY ?
                    Rect.MinMaxRect(m_SerieGrid.context.x, plb.y, m_SerieGrid.context.x + relativedAxisLength, prt.y) :
                    Rect.MinMaxRect(plb.x, m_SerieGrid.context.y, prb.x, m_SerieGrid.context.y + relativedAxisLength);

                if (!serie.clip || (serie.clip && m_SerieGrid.Contains(top)))
                {
                    serie.context.dataPoints.Add(top);
                    serie.context.dataIndexs.Add(serieData.index);
                }
                else
                {
                    continue;
                }

                if (serie.show && !serie.placeHolder)
                {
                    switch (serie.barType)
                    {
                        case BarType.Normal:
                        case BarType.Capsule:
                            DrawNormalBar(vh, serie, serieData, itemStyle, backgroundColor, gap, barWidth,
                                pX, pY, plb, plt, prt, prb, isY, m_SerieGrid, axis, areaColor, areaToColor, relativedValue);
                            break;
                        case BarType.Zebra:
                            DrawZebraBar(vh, serie, serieData, itemStyle, backgroundColor, gap, barWidth,
                                pX, pY, plb, plt, prt, prb, isY, m_SerieGrid, axis, areaColor, areaToColor);
                            break;
                    }
                }
                if (serie.animation.CheckDetailBreak(top, isY))
                {
                    break;
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
                chart.RefreshPainter(serie);
            }
            if (dataChanging || interacting)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void UpdateXYPosition(GridCoord grid, bool isY, Axis axis, Axis relativedAxis, int i, float categoryWidth, float barWidth, bool isStack,
            double value, ref float pX, ref float pY)
        {
            if (isY)
            {
                if (axis.IsCategory())
                {
                    pY = grid.context.y + i * categoryWidth + (axis.boundaryGap ? 0 : -categoryWidth * 0.5f);
                }
                else
                {
                    if (axis.context.minMaxRange <= 0) pY = grid.context.y;
                    else
                    {
                        var valueLen = (float)((value - axis.context.minValue) / axis.context.minMaxRange) * grid.context.height;
                        pY = grid.context.y + valueLen - categoryWidth * 0.5f;
                    }
                }
                pX = AxisHelper.GetAxisValuePosition(grid, relativedAxis, categoryWidth, 0);
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        pX += m_StackSerieData[n][i].context.stackHeight;
                }
            }
            else
            {
                if (axis.IsCategory())
                {
                    pX = grid.context.x + i * categoryWidth + (axis.boundaryGap ? 0 : -categoryWidth * 0.5f);
                }
                else
                {
                    if (axis.context.minMaxRange <= 0) pX = grid.context.x;
                    else
                    {
                        var valueLen = (float)((value - axis.context.minValue) / axis.context.minMaxRange) * grid.context.width;
                        pX = grid.context.x + valueLen - categoryWidth * 0.5f;
                    }
                }
                pY = AxisHelper.GetAxisValuePosition(grid, relativedAxis, categoryWidth, 0);
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        pY += m_StackSerieData[n][i].context.stackHeight;
                }
            }
        }

        private void UpdateRectPosition(GridCoord grid, bool isY, double yValue, float pX, float pY, float gap, float borderWidth,
            float barWidth, float currHig,
            out Vector3 plb, out Vector3 plt, out Vector3 prt, out Vector3 prb, out Vector3 top)
        {
            if (isY)
            {
                if (yValue < 0)
                {
                    plt = new Vector3(pX + currHig, pY + gap + barWidth);
                    prt = new Vector3(pX, pY + gap + barWidth);
                    prb = new Vector3(pX, pY + gap);
                    plb = new Vector3(pX + currHig, pY + gap);
                }
                else
                {
                    plt = new Vector3(pX, pY + gap + barWidth);
                    prt = new Vector3(pX + currHig, pY + gap + barWidth);
                    prb = new Vector3(pX + currHig, pY + gap);
                    plb = new Vector3(pX, pY + gap);
                }
                top = new Vector3(pX + currHig, pY + gap + barWidth / 2);
            }
            else
            {
                if (yValue < 0)
                {
                    plb = new Vector3(pX + gap, pY + currHig);
                    plt = new Vector3(pX + gap, pY);
                    prt = new Vector3(pX + gap + barWidth, pY);
                    prb = new Vector3(pX + gap + barWidth, pY + currHig);
                }
                else
                {
                    plb = new Vector3(pX + gap, pY);
                    plt = new Vector3(pX + gap, pY + currHig);
                    prt = new Vector3(pX + gap + barWidth, pY + currHig);
                    prb = new Vector3(pX + gap + barWidth, pY);
                }
                top = new Vector3(pX + gap + barWidth / 2, pY + currHig);
            }
            if (serie.clip)
            {
                plb = chart.ClampInGrid(grid, plb);
                plt = chart.ClampInGrid(grid, plt);
                prt = chart.ClampInGrid(grid, prt);
                prb = chart.ClampInGrid(grid, prb);
                top = chart.ClampInGrid(grid, top);
            }
        }

        private void DrawNormalBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, Color32 backgroundColor,
            float gap, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis, GridCoord grid, Axis axis, Color32 areaColor, Color32 areaToColor, double value)
        {
            var borderWidth = itemStyle.runtimeBorderWidth;
            var cornerRadius = serie.barType == BarType.Capsule && !itemStyle.IsNeedCorner() ?
                m_CapusleDefaultCornerRadius :
                itemStyle.cornerRadius;
            var invert = value < 0;
            if (!ChartHelper.IsClearColor(backgroundColor))
            {
                UGL.DrawRoundRectangle(vh, serieData.context.backgroundRect, backgroundColor, backgroundColor, 0,
                    cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
            }
            UGL.DrawRoundRectangle(vh, serieData.context.rect, areaColor, areaToColor, 0,
                cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
            if (serie.barType == BarType.Capsule)
            {
                UGL.DrawBorder(vh, serieData.context.backgroundRect, borderWidth, itemStyle.borderColor,
                    0, cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert, -borderWidth);
            }
            else
            {
                UGL.DrawBorder(vh, serieData.context.rect, borderWidth, itemStyle.borderColor,
                    0, cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert, itemStyle.borderGap);
            }
        }

        private void DrawZebraBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, Color32 backgroundColor,
            float gap, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis, GridCoord grid, Axis axis, Color32 barColor, Color32 barToColor)
        {
            if (!ChartHelper.IsClearColor(backgroundColor))
            {
                UGL.DrawRoundRectangle(vh, serieData.context.backgroundRect, backgroundColor, backgroundColor, 0,
                    null, isYAxis, chart.settings.cicleSmoothness, false);
            }
            if (isYAxis)
            {
                plt = (plb + plt) / 2;
                prt = (prt + prb) / 2;
                chart.DrawClipZebraLine(vh, plt, prt, barWidth / 2, serie.barZebraWidth, serie.barZebraGap,
                    barColor, barToColor, serie.clip, grid, grid.context.width);
            }
            else
            {
                plb = (prb + plb) / 2;
                plt = (plt + prt) / 2;
                chart.DrawClipZebraLine(vh, plb, plt, barWidth / 2, serie.barZebraWidth, serie.barZebraGap,
                    barColor, barToColor, serie.clip, grid, grid.context.height);
            }
        }
    }
}