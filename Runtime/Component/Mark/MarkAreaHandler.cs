using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class MarkAreaHandler : MainComponentHandler<MarkArea>
    {
        private GameObject m_MarkLineLabelRoot;
        private bool m_NeedUpdateLabelPosition;

        public override void InitComponent()
        {
            m_MarkLineLabelRoot = ChartHelper.AddObject("markarea" + component.index, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            m_MarkLineLabelRoot.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(m_MarkLineLabelRoot);
            InitMarkArea(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawMarkArea(vh, component);
        }

        public override void Update()
        {
            if (m_NeedUpdateLabelPosition)
            {
                m_NeedUpdateLabelPosition = false;
                if (component.runtimeLabel != null)
                {
                    component.runtimeLabel.SetPosition(component.runtimeLabelPosition);
                }
            }
        }

        private void InitMarkArea(MarkArea markArea)
        {
            markArea.painter = chart.m_PainterUpper;
            markArea.refreshComponent = delegate()
            {
                var label = ChartHelper.AddChartLabel("label", m_MarkLineLabelRoot.transform, markArea.label, chart.theme.axis,
                    component.text, Color.clear, TextAnchor.MiddleCenter);
                UpdateRuntimeData(component);
                label.SetActive(markArea.label.show);
                label.SetPosition(component.runtimeLabelPosition);
                label.SetText(component.text);
                markArea.runtimeLabel = label;
            };
            markArea.refreshComponent();
        }

        private void DrawMarkArea(VertexHelper vh, MarkArea markArea)
        {
            if (!markArea.show) return;
            var serie = chart.GetSerie(markArea.serieIndex);
            if (serie == null || !serie.show || !markArea.show) return;

            UpdateRuntimeData(markArea);

            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
            var serieColor = SerieHelper.GetLineColor(serie, null, chart.theme, colorIndex, SerieState.Normal);
            var areaColor = markArea.itemStyle.GetColor(serieColor);
            UGL.DrawRectangle(vh, markArea.runtimeRect, areaColor, areaColor);
        }

        private void UpdateRuntimeData(MarkArea markArea)
        {
            var serie = chart.GetSerie(markArea.serieIndex);
            if (serie == null || !serie.show || !markArea.show) return;
            var yAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
            var xAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
            var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
            var dataZoom = chart.GetDataZoomOfAxis(xAxis);
            var showData = serie.GetDataList(dataZoom);

            var lt = GetPosition(markArea.start, serie, dataZoom, xAxis, yAxis, grid, showData, true);
            var rb = GetPosition(markArea.end, serie, dataZoom, xAxis, yAxis, grid, showData, false);
            var lb = new Vector3(lt.x, rb.y);

            markArea.runtimeRect = new Rect(lb.x, lb.y, rb.x - lb.x, lt.y - lb.y);
            UpdateLabelPosition(markArea);
        }

        private void UpdateLabelPosition(MarkArea markArea)
        {
            if (!markArea.label.show) return;
            m_NeedUpdateLabelPosition = true;
            var rect = markArea.runtimeRect;
            switch (markArea.label.position)
            {
                case LabelStyle.Position.Center:
                    markArea.runtimeLabelPosition = rect.center;
                    break;
                case LabelStyle.Position.Left:
                    markArea.runtimeLabelPosition = rect.center + new Vector2(rect.width / 2, 0);
                    break;
                case LabelStyle.Position.Right:
                    markArea.runtimeLabelPosition = rect.center - new Vector2(rect.width / 2, 0);
                    break;
                case LabelStyle.Position.Top:
                    markArea.runtimeLabelPosition = rect.center + new Vector2(0, rect.height / 2);
                    break;
                case LabelStyle.Position.Bottom:
                    markArea.runtimeLabelPosition = rect.center - new Vector2(0, rect.height / 2);
                    break;
                default:
                    markArea.runtimeLabelPosition = rect.center + new Vector2(0, rect.height / 2);
                    break;
            }
            markArea.runtimeLabelPosition += markArea.label.offset;
        }

        private Vector3 GetPosition(MarkAreaData data, Serie serie, DataZoom dataZoom, XAxis xAxis, YAxis yAxis,
            GridCoord grid, List<SerieData> showData, bool start)
        {
            var pos = Vector3.zero;
            switch (data.type)
            {
                case MarkAreaType.Min:
                    data.runtimeValue = SerieHelper.GetMinData(serie, data.dimension, dataZoom);
                    return GetPosition(xAxis, yAxis, grid, data.runtimeValue, start);
                case MarkAreaType.Max:
                    data.runtimeValue = SerieHelper.GetMaxData(serie, data.dimension, dataZoom);
                    return GetPosition(xAxis, yAxis, grid, data.runtimeValue, start);
                case MarkAreaType.Average:
                    data.runtimeValue = SerieHelper.GetAverageData(serie, data.dimension, dataZoom);
                    return GetPosition(xAxis, yAxis, grid, data.runtimeValue, start);
                case MarkAreaType.Median:
                    data.runtimeValue = SerieHelper.GetMedianData(serie, data.dimension, dataZoom);
                    return GetPosition(xAxis, yAxis, grid, data.runtimeValue, start);
                case MarkAreaType.None:
                    if (data.xPosition != 0 || data.yPosition != 0)
                    {
                        var pX = grid.context.x + data.xPosition;
                        var pY = grid.context.y + data.yPosition;
                        return new Vector3(pX, pY);
                    }
                    else if (data.yValue != 0)
                    {
                        data.runtimeValue = data.yValue;
                        if (yAxis.IsCategory())
                        {
                            var pY = AxisHelper.GetAxisPosition(grid, yAxis, data.yValue, showData.Count, dataZoom);
                            return start ?
                                new Vector3(grid.context.x, pY) :
                                new Vector3(grid.context.x + grid.context.width, pY);
                        }
                        else
                        {
                            return GetPosition(xAxis, yAxis, grid, data.runtimeValue, start);
                        }
                    }
                    else
                    {
                        data.runtimeValue = data.xValue;
                        if (xAxis.IsCategory())
                        {
                            var pX = AxisHelper.GetAxisPosition(grid, xAxis, data.xValue, showData.Count, dataZoom);
                            return start ? new Vector3(pX, grid.context.y + grid.context.height) :
                                new Vector3(pX, grid.context.y);
                        }
                        else
                        {
                            return GetPosition(xAxis, yAxis, grid, data.xValue, start);
                        }
                    }
                default:
                    break;
            }
            return pos;
        }

        private Vector3 GetPosition(Axis xAxis, Axis yAxis, GridCoord grid, double value, bool start)
        {
            if (yAxis.IsCategory())
            {
                var pX = AxisHelper.GetAxisPosition(grid, xAxis, value);
                return start ?
                    new Vector3(pX, grid.context.y + grid.context.height) :
                    new Vector3(pX, grid.context.y);
            }
            else
            {
                var pY = AxisHelper.GetAxisPosition(grid, yAxis, value);
                return start ?
                    new Vector3(grid.context.x, pY + grid.context.height) :
                    new Vector3(grid.context.x + grid.context.width, pY);
            }
        }
    }
}