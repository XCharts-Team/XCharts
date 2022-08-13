using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class SimplifiedBarHandler : SerieHandler<SimplifiedBar>
    {
        private GridCoord m_SerieGrid;

        public override void Update()
        {
            base.Update();
            UpdateSerieContext();
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
            DrawBarSerie(vh, serie, serie.context.colorIndex);
        }

        private void UpdateSerieContext()
        {
            if (m_SerieGrid == null)
                return;

            var needCheck = (chart.isPointerInChart && m_SerieGrid.IsPointerEnter()) || m_LegendEnter;
            var needInteract = false;
            Color32 color, toColor;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    foreach (var serieData in serie.data)
                    {
                        SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, SerieState.Normal);
                        serieData.interact.SetColor(ref needInteract, color, toColor);
                    }
                    if (needInteract)
                    {
                        chart.RefreshPainter(serie);
                    }
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            if (m_LegendEnter)
            {
                serie.context.pointerEnter = true;
                foreach (var serieData in serie.data)
                {
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, SerieState.Emphasis);
                    serieData.interact.SetColor(ref needInteract, color, toColor);
                }
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                foreach (var serieData in serie.data)
                {
                    if (serieData.context.rect.Contains(chart.pointerPos))
                    {
                        serie.context.pointerItemDataIndex = serieData.index;
                        serie.context.pointerEnter = true;
                        serieData.context.highlight = true;

                        SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, SerieState.Emphasis);
                        serieData.interact.SetColor(ref needInteract, color, toColor);
                    }
                    else
                    {
                        serieData.context.highlight = false;
                        SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, SerieState.Normal);
                        serieData.interact.SetColor(ref needInteract, color, toColor);
                    }
                }
            }
            if (needInteract)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawBarSerie(VertexHelper vh, SimplifiedBar serie, int colorIndex)
        {
            if (!serie.show || serie.animation.HasFadeOut())
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

            var axisLength = isY ? m_SerieGrid.context.height : m_SerieGrid.context.width;
            var axisXY = isY ? m_SerieGrid.context.y : m_SerieGrid.context.x;

            var barCount = chart.GetSerieBarRealCount<SimplifiedBar>();
            float categoryWidth = AxisHelper.GetDataWidth(axis, axisLength, showData.Count, dataZoom);
            float barGap = chart.GetSerieBarGap<SimplifiedBar>();
            float totalBarWidth = chart.GetSerieTotalWidth<SimplifiedBar>(categoryWidth, barGap, barCount);
            float barWidth = serie.GetBarWidth(categoryWidth, barCount);
            float offset = (categoryWidth - totalBarWidth) * 0.5f;
            float barGapWidth = barWidth + barWidth * barGap;
            float gap = serie.barGap == -1 ? offset : offset + serie.index * barGapWidth;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;

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

                var highlight = serieData.context.highlight || serie.highlight;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
                var value = axis.IsCategory() ? i : serieData.GetData(0, axis.inverse);
                var relativedValue = serieData.GetCurrData(1, dataChangeDuration, relativedAxis.inverse, yMinValue, yMaxValue);
                var borderWidth = relativedValue == 0 ? 0 : itemStyle.runtimeBorderWidth;

                if (!serieData.interact.TryGetColor(ref areaColor, ref areaToColor, ref interacting))
                {
                    SerieHelper.GetItemColor(out areaColor, out areaToColor, serie, serieData, chart.theme);
                    serieData.interact.SetColor(ref interacting, areaColor, areaToColor);
                }

                var pX = 0f;
                var pY = 0f;
                UpdateXYPosition(m_SerieGrid, isY, axis, relativedAxis, i, categoryWidth, barWidth, value, ref pX, ref pY);

                var barHig = AxisHelper.GetAxisValueLength(m_SerieGrid, relativedAxis, categoryWidth, relativedValue);
                var currHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, barHig);

                Vector3 plb, plt, prt, prb, top;
                UpdateRectPosition(m_SerieGrid, isY, relativedValue, pX, pY, gap, borderWidth, barWidth, currHig,
                    out plb, out plt, out prt, out prb, out top);
                serieData.context.stackHeight = barHig;
                serieData.context.position = top;
                serieData.context.rect = Rect.MinMaxRect(plb.x, plb.y, prb.x, prt.y);
                serie.context.dataPoints.Add(top);
                serie.context.dataIndexs.Add(serieData.index);
                DrawNormalBar(vh, serie, serieData, itemStyle, colorIndex, highlight, gap, barWidth,
                    pX, pY, plb, plt, prt, prb, false, m_SerieGrid, areaColor, areaToColor);

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

        private void UpdateXYPosition(GridCoord grid, bool isY, Axis axis, Axis relativedAxis, int i, float categoryWidth, float barWidth,
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
                    else pY = grid.context.y + (float) ((value - axis.context.minValue) / axis.context.minMaxRange) * (grid.context.height - barWidth);
                }
                pX = AxisHelper.GetAxisValuePosition(grid, relativedAxis, categoryWidth, 0);
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
                    else pX = grid.context.x + (float) ((value - axis.context.minValue) / axis.context.minMaxRange) * (grid.context.width - barWidth);
                }
                pY = AxisHelper.GetAxisValuePosition(grid, relativedAxis, categoryWidth, 0);
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
                    plt = new Vector3(pX - borderWidth, pY + gap + barWidth - borderWidth);
                    prt = new Vector3(pX + currHig + borderWidth, pY + gap + barWidth - borderWidth);
                    prb = new Vector3(pX + currHig + borderWidth, pY + gap + borderWidth);
                    plb = new Vector3(pX - borderWidth, pY + gap + borderWidth);
                }
                else
                {
                    plt = new Vector3(pX + borderWidth, pY + gap + barWidth - borderWidth);
                    prt = new Vector3(pX + currHig - borderWidth, pY + gap + barWidth - borderWidth);
                    prb = new Vector3(pX + currHig - borderWidth, pY + gap + borderWidth);
                    plb = new Vector3(pX + borderWidth, pY + gap + borderWidth);
                }
                top = new Vector3(pX + currHig - borderWidth, pY + gap + barWidth / 2);
            }
            else
            {
                if (yValue < 0)
                {
                    plb = new Vector3(pX + gap + borderWidth, pY - borderWidth);
                    plt = new Vector3(pX + gap + borderWidth, pY + currHig + borderWidth);
                    prt = new Vector3(pX + gap + barWidth - borderWidth, pY + currHig + borderWidth);
                    prb = new Vector3(pX + gap + barWidth - borderWidth, pY - borderWidth);
                }
                else
                {
                    plb = new Vector3(pX + gap + borderWidth, pY + borderWidth);
                    plt = new Vector3(pX + gap + borderWidth, pY + currHig - borderWidth);
                    prt = new Vector3(pX + gap + barWidth - borderWidth, pY + currHig - borderWidth);
                    prb = new Vector3(pX + gap + barWidth - borderWidth, pY + borderWidth);
                }
                top = new Vector3(pX + gap + barWidth / 2, pY + currHig - borderWidth);
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

        private void DrawNormalBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float gap, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis, GridCoord grid, Color32 areaColor, Color32 areaToColor)
        {

            var borderWidth = itemStyle.runtimeBorderWidth;
            if (isYAxis)
            {
                if (serie.clip)
                {
                    prb = chart.ClampInGrid(grid, prb);
                    plb = chart.ClampInGrid(grid, plb);
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                }
                var itemWidth = Mathf.Abs(prb.x - plt.x);
                var itemHeight = Mathf.Abs(prt.y - plb.y);
                var center = new Vector3((plt.x + prb.x) / 2, (prt.y + plb.y) / 2);
                if (itemWidth > 0 && itemHeight > 0)
                {
                    var invert = center.x < plb.x;
                    if (itemStyle.IsNeedCorner())
                    {
                        UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaToColor, 0,
                            itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                    }
                    else
                    {
                        chart.DrawClipPolygon(vh, plb, plt, prt, prb, areaColor, areaToColor, serie.clip, grid);
                    }
                    UGL.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, itemStyle.borderColor,
                        itemStyle.borderToColor, 0, itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                }
            }
            else
            {
                if (serie.clip)
                {
                    prb = chart.ClampInGrid(grid, prb);
                    plb = chart.ClampInGrid(grid, plb);
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                }
                var itemWidth = Mathf.Abs(prt.x - plb.x);
                var itemHeight = Mathf.Abs(plt.y - prb.y);
                var center = new Vector3((plb.x + prt.x) / 2, (plt.y + prb.y) / 2);
                if (itemWidth > 0 && itemHeight > 0)
                {
                    var invert = center.y < plb.y;
                    if (itemStyle.IsNeedCorner())
                    {
                        UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaToColor, 0,
                            itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                    }
                    else
                    {
                        chart.DrawClipPolygon(vh, ref prb, ref plb, ref plt, ref prt, areaColor, areaToColor,
                            serie.clip, grid);
                    }
                    UGL.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, itemStyle.borderColor,
                        itemStyle.borderToColor, 0, itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                }
            }
        }
    }
}