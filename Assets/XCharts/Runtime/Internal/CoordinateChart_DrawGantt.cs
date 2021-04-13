/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected void DrawGanttSerie(VertexHelper vh, int colorIndex, Serie serie)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            var showData = serie.GetDataList(null);
            var yAxis = m_YAxes[serie.yAxisIndex];
            var xAxis = m_XAxes[serie.xAxisIndex];
            var grid = GetSerieGridOrDefault(serie);
            var xCategoryWidth = AxisHelper.GetDataWidth(xAxis, grid.runtimeWidth, showData.Count, dataZoom);
            var yCategoryWidth = AxisHelper.GetDataWidth(yAxis, grid.runtimeHeight, showData.Count, dataZoom);
            var barWidth = serie.GetBarWidth(yCategoryWidth);
            var space = (yCategoryWidth - barWidth) / 2;
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var minValue = xAxis.GetCurrMinValue(dataChangeDuration);
            var maxValue = xAxis.GetCurrMaxValue(dataChangeDuration);
            var pX = grid.runtimeX + (xAxis.boundaryGap ? xCategoryWidth / 2 : 0);
            var pY = 0f;
            var startY = grid.runtimeY - (yAxis.boundaryGap ? 0 : yCategoryWidth / 2);
            var isTime = xAxis.type == Axis.AxisType.Time;

            var categoryIndex = GetGanttSerieCategoryIndex(serie, grid.index);
            var dataCount = serie.data.Count;
            for (int i = 0; i < dataCount; i++)
            {
                var serieData = serie.data[i];
                pY = startY + (categoryIndex - 1 - i) * yCategoryWidth;
                DrawSerieData(vh, grid, serie, serieData, colorIndex, pX, pY, space, barWidth, isTime, minValue,
                    maxValue, xCategoryWidth);
            }
            if (dataChanging)
            {
                RefreshPainter(serie);
            }
        }

        private void DrawSerieData(VertexHelper vh, Grid grid, Serie serie, SerieData serieData, int colorIndex,
            float pX, float pY, float space, float barWidth, bool isTime, float minValue, float maxValue,
            float xCategoryWidth)
        {
            var xStart = 0f;
            var xEnd = 0f;
            var xActualStart = 0f;
            var xActualEnd = 0f;
            var start = (int)serieData.GetData(0);
            var end = (int)serieData.GetData(1);
            var actualStart = (int)serieData.GetData(2);
            var actualEnd = (int)serieData.GetData(3);
            var enableActual = actualStart > 0 && actualEnd > 0;
            if (isTime)
            {
                var valueTotal = maxValue - minValue;
                xStart = pX + (start - minValue) / valueTotal * grid.runtimeWidth;
                xEnd = pX + (end - minValue) / valueTotal * grid.runtimeWidth;
                if (enableActual)
                {
                    xActualStart = pX + (actualStart - minValue) / valueTotal * grid.runtimeWidth;
                    xActualEnd = pX + (actualEnd - minValue) / valueTotal * grid.runtimeWidth;
                }
            }
            else
            {
                xStart = pX + start * xCategoryWidth;
                xEnd = pX + end * xCategoryWidth;
                if (enableActual)
                {
                    xActualStart = pX + actualStart * xCategoryWidth;
                    xActualEnd = pX + actualEnd * xCategoryWidth;
                }
            }
            var highlight = (serieData != null && serieData.highlighted)
                    || serie.highlighted;
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);
            var color = SerieHelper.GetItemColor(serie, serieData, m_Theme, colorIndex, highlight);
            var borderWidth = itemStyle.borderWidth;

            var rect = DrawGanttBar(vh, grid, serie, serieData, itemStyle, color, pY, pY, space, barWidth, xStart,
                xEnd);
            if (enableActual)
            {
                var defaultActualColor = SerieHelper.GetItemColor(serie, serieData, m_Theme, colorIndex, true);
                var actualColor = SerieHelper.GetItemColor0(serie, serieData, m_Theme, highlight, defaultActualColor);
                var rect2 = DrawGanttBar(vh, grid, serie, serieData, itemStyle, actualColor, pY, pY, space, barWidth,
                    xActualStart, xActualEnd);
                var rect3X = Mathf.Min(rect.x, rect2.x);
                var rect3Width = Mathf.Max(rect.x + rect.width, rect2.x + rect2.width) - rect3X;
                var rect3 = new Rect(rect3X, rect.y, rect3Width, rect.height);
                serie.dataPoints.Add(rect3.center);
                serieData.runtimePosition = rect3.center;
                serieData.labelPosition = rect3.center;
                serieData.runtimeRect = rect3;
            }
            else
            {
                serie.dataPoints.Add(rect.center);
                serieData.runtimePosition = rect.center;
                serieData.labelPosition = rect.center;
                serieData.runtimeRect = rect;
            }
        }

        private Rect DrawGanttBar(VertexHelper vh, Grid grid, Serie serie, SerieData serieData, ItemStyle itemStyle,
            Color32 color, float pX, float pY, float space, float barWidth, float xStart, float xEnd)
        {

            var borderWidth = itemStyle.borderWidth;
            var plb = new Vector3(xStart + borderWidth, pY + space + borderWidth);
            var plt = new Vector3(xStart + borderWidth, pY + space + barWidth - borderWidth);
            var prt = new Vector3(xEnd - borderWidth, pY + space + barWidth - borderWidth);
            var prb = new Vector3(xEnd - borderWidth, pY + space + borderWidth);
            var center = new Vector3((plb.x + prt.x) / 2, (plt.y + prb.y) / 2);
            var itemWidth = Mathf.Abs(prt.x - plb.x);
            var itemHeight = Mathf.Abs(plt.y - prb.y);
            if (serie.clip)
            {
                plb = ClampInGrid(grid, plb);
                plt = ClampInGrid(grid, plt);
                prt = ClampInGrid(grid, prt);
                prb = ClampInGrid(grid, prb);
                center = ClampInGrid(grid, center);
            }
            if (ItemStyleHelper.IsNeedCorner(itemStyle))
            {
                UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, color, color, 0,
                    itemStyle.cornerRadius, true, 0.5f);
            }
            else
            {
                CheckClipAndDrawPolygon(vh, ref prb, ref plb, ref plt, ref prt, color, color,
                    serie.clip, grid);
            }
            if (borderWidth != 0)
            {
                UGL.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, itemStyle.borderColor, 0,
                              itemStyle.cornerRadius, true, 0.5f);
            }
            return new Rect(plb.x, plb.y, xEnd - xStart, barWidth);
        }

        private int GetGanttSerieCategoryIndex(Serie currSerie, int gridIndex)
        {
            var count = m_Series.Count;
            var index = 0;
            for (int i = 0; i < count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (serie.type != SerieType.Gantt) continue;
                var grid = GetSerieGridOrDefault(serie);
                if (grid.index != gridIndex) continue;
                foreach (var serieData in serie.data)
                {
                    index++;
                }
                if (serie.index == currSerie.index)
                {
                    return index;
                }
            }
            return index;
        }
    }
}