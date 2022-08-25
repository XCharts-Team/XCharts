using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class MarkLineHandler : MainComponentHandler<MarkLine>
    {
        private GameObject m_MarkLineLabelRoot;
        private bool m_RefreshLabel = false;

        public override void InitComponent()
        {
            m_MarkLineLabelRoot = ChartHelper.AddObject("markline", chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            m_MarkLineLabelRoot.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(m_MarkLineLabelRoot);
            InitMarkLine(component);
        }

        public override void DrawUpper(VertexHelper vh)
        {
            DrawMarkLine(vh, component);
        }

        public override void Update()
        {
            if (m_RefreshLabel)
            {
                m_RefreshLabel = false;
                var serie = chart.GetSerie(component.serieIndex);
                if (!serie.show || !component.show) return;
                foreach (var data in component.data)
                {
                    if (data.runtimeLabel != null)
                    {
                        var pos = MarkLineHelper.GetLabelPosition(data);
                        data.runtimeLabel.SetActive(data.label.show && pos != Vector3.zero);
                        data.runtimeLabel.SetPosition(pos);
                        data.runtimeLabel.SetText(MarkLineHelper.GetFormatterContent(serie, data));
                    }
                }
            }
        }

        private void InitMarkLine(MarkLine markLine)
        {
            var serie = chart.GetSerie(markLine.serieIndex);
            if (!serie.show || !markLine.show) return;
            ResetTempMarkLineGroupData(markLine);
            var serieColor = (Color) chart.GetItemColor(serie);
            if (m_TempGroupData.Count > 0)
            {
                foreach (var kv in m_TempGroupData)
                {
                    if (kv.Value.Count >= 2)
                    {
                        var data = kv.Value[0];
                        InitMarkLineLabel(serie, data, serieColor);
                    }
                }
            }
            foreach (var data in markLine.data)
            {
                if (data.group != 0) continue;
                InitMarkLineLabel(serie, data, serieColor);
            }
        }

        private void InitMarkLineLabel(Serie serie, MarkLineData data, Color serieColor)
        {
            data.painter = chart.m_PainterUpper;
            data.refreshComponent = delegate()
            {
                var textName = string.Format("markLine_{0}_{1}", serie.index, data.index);
                var content = MarkLineHelper.GetFormatterContent(serie, data);
                var label = ChartHelper.AddChartLabel(textName, m_MarkLineLabelRoot.transform, data.label, chart.theme.axis,
                    content, Color.clear, TextAnchor.MiddleCenter);
                var pos = MarkLineHelper.GetLabelPosition(data);
                label.SetIconActive(false);
                label.SetActive(data.label.show && pos != Vector3.zero);
                label.SetPosition(pos);
                data.runtimeLabel = label;
            };
            data.refreshComponent();
        }

        private Dictionary<int, List<MarkLineData>> m_TempGroupData = new Dictionary<int, List<MarkLineData>>();
        private void DrawMarkLine(VertexHelper vh, MarkLine markLine)
        {
            var serie = chart.GetSerie(markLine.serieIndex);
            if (!serie.show || !markLine.show) return;
            if (markLine.data.Count == 0) return;
            var yAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
            var xAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
            var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
            var dataZoom = chart.GetDataZoomOfAxis(xAxis);
            var animation = markLine.animation;
            var showData = serie.GetDataList(dataZoom);
            var sp = Vector3.zero;
            var ep = Vector3.zero;
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.serieName);
            var serieColor = SerieHelper.GetLineColor(serie, null, chart.theme, colorIndex, SerieState.Normal);
            animation.InitProgress(0, 1f);
            ResetTempMarkLineGroupData(markLine);
            if (m_TempGroupData.Count > 0)
            {
                foreach (var kv in m_TempGroupData)
                {
                    if (kv.Value.Count >= 2)
                    {
                        sp = GetSinglePos(xAxis, yAxis, grid, serie, dataZoom, kv.Value[0], showData.Count);
                        ep = GetSinglePos(xAxis, yAxis, grid, serie, dataZoom, kv.Value[1], showData.Count);
                        kv.Value[0].runtimeStartPosition = sp;
                        kv.Value[1].runtimeEndPosition = ep;
                        DrawMakLineData(vh, kv.Value[0], animation, serie, grid, serieColor, sp, ep);
                    }
                }
            }
            foreach (var data in markLine.data)
            {
                if (data.group != 0) continue;
                switch (data.type)
                {
                    case MarkLineType.Min:
                        data.runtimeValue = SerieHelper.GetMinData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.Max:
                        data.runtimeValue = SerieHelper.GetMaxData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.Average:
                        data.runtimeValue = SerieHelper.GetAverageData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.Median:
                        data.runtimeValue = SerieHelper.GetMedianData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.None:
                        if (data.xPosition != 0)
                        {
                            data.runtimeValue = data.xPosition;
                            var pX = grid.context.x + data.xPosition;
                            sp = new Vector3(pX, grid.context.y);
                            ep = new Vector3(pX, grid.context.y + grid.context.height);
                        }
                        else if (data.yPosition != 0)
                        {
                            data.runtimeValue = data.yPosition;
                            var pY = grid.context.y + data.yPosition;
                            sp = new Vector3(grid.context.x, pY);
                            ep = new Vector3(grid.context.x + grid.context.width, pY);
                        }
                        else if (data.yValue != 0)
                        {
                            data.runtimeValue = data.yValue;
                            if (yAxis.IsCategory())
                            {
                                var pY = AxisHelper.GetAxisPosition(grid, yAxis, data.yValue, showData.Count, dataZoom);
                                sp = new Vector3(grid.context.x, pY);
                                ep = new Vector3(grid.context.x + grid.context.width, pY);
                            }
                            else
                            {
                                GetStartEndPos(xAxis, yAxis, grid, data.yValue, ref sp, ref ep);
                            }
                        }
                        else
                        {
                            data.runtimeValue = data.xValue;
                            if (xAxis.IsCategory())
                            {
                                var pX = AxisHelper.GetAxisPosition(grid, xAxis, data.xValue, showData.Count, dataZoom);
                                sp = new Vector3(pX, grid.context.y);
                                ep = new Vector3(pX, grid.context.y + grid.context.height);
                            }
                            else
                            {
                                GetStartEndPos(xAxis, yAxis, grid, data.xValue, ref sp, ref ep);
                            }
                        }
                        break;
                    default:
                        break;
                }
                data.runtimeStartPosition = sp;
                data.runtimeEndPosition = ep;
                DrawMakLineData(vh, data, animation, serie, grid, serieColor, sp, ep);
            }
            if (!animation.IsFinish())
            {
                animation.CheckProgress(1f);
                chart.RefreshTopPainter();
            }
        }

        private void ResetTempMarkLineGroupData(MarkLine markLine)
        {
            m_TempGroupData.Clear();
            for (int i = 0; i < markLine.data.Count; i++)
            {
                var data = markLine.data[i];
                data.index = i;
                if (data.group == 0) continue;
                if (!m_TempGroupData.ContainsKey(data.group))
                {
                    m_TempGroupData[data.group] = new List<MarkLineData>();
                }
                m_TempGroupData[data.group].Add(data);
            }
        }

        private void DrawMakLineData(VertexHelper vh, MarkLineData data, AnimationStyle animation, Serie serie,
            GridCoord grid, Color32 serieColor, Vector3 sp, Vector3 ep)
        {
            if (!animation.IsFinish())
                ep = Vector3.Lerp(sp, ep, animation.GetCurrDetail());
            data.runtimeCurrentEndPosition = ep;
            if (sp != Vector3.zero || ep != Vector3.zero)
            {
                m_RefreshLabel = true;
                chart.ClampInChart(ref sp);
                chart.ClampInChart(ref ep);
                var theme = chart.theme.axis;
                var lineColor = ChartHelper.IsClearColor(data.lineStyle.color) ? serieColor : data.lineStyle.color;
                var lineWidth = data.lineStyle.width == 0 ? theme.lineWidth : data.lineStyle.width;
                ChartDrawer.DrawLineStyle(vh, data.lineStyle, sp, ep, lineWidth, LineStyle.Type.Dashed, lineColor, lineColor);
                if (data.startSymbol != null && data.startSymbol.show)
                {
                    DrawMarkLineSymbol(vh, data.startSymbol, serie, grid, chart.theme, sp, sp, lineColor);
                }
                if (data.endSymbol != null && data.endSymbol.show)
                {
                    DrawMarkLineSymbol(vh, data.endSymbol, serie, grid, chart.theme, ep, sp, lineColor);
                }
            }
        }

        private void DrawMarkLineSymbol(VertexHelper vh, SymbolStyle symbol, Serie serie, GridCoord grid, ThemeStyle theme,
            Vector3 pos, Vector3 startPos, Color32 lineColor)
        {
            float tickness = 0f;
            float[] cornerRadius = null;
            Color32 borderColor;
            SerieHelper.GetSymbolInfo(out borderColor, out tickness, out cornerRadius, serie, null, chart.theme);
            chart.DrawClipSymbol(vh, symbol.type, symbol.size, tickness, pos, lineColor, lineColor,
                ColorUtil.clearColor32, borderColor, symbol.gap, true, cornerRadius, grid, startPos);
        }

        private void GetStartEndPos(Axis xAxis, Axis yAxis, GridCoord grid, double value, ref Vector3 sp, ref Vector3 ep)
        {
            if (xAxis.IsCategory())
            {
                var pY = AxisHelper.GetAxisPosition(grid, yAxis, value);
                sp = new Vector3(grid.context.x, pY);
                ep = new Vector3(grid.context.x + grid.context.width, pY);
            }
            else
            {
                var pX = AxisHelper.GetAxisPosition(grid, xAxis, value);
                sp = new Vector3(pX, grid.context.y);
                ep = new Vector3(pX, grid.context.y + grid.context.height);
            }
        }

        private float GetAxisPosition(GridCoord grid, Axis axis, DataZoom dataZoom, int dataCount, double value)
        {
            return AxisHelper.GetAxisPosition(grid, axis, value, dataCount, dataZoom);
        }

        private Vector3 GetSinglePos(Axis xAxis, Axis yAxis, GridCoord grid, Serie serie, DataZoom dataZoom, MarkLineData data,
            int serieDataCount)
        {
            switch (data.type)
            {
                case MarkLineType.Min:
                    var serieData = SerieHelper.GetMinSerieData(serie, data.dimension, dataZoom);
                    data.runtimeValue = serieData.GetData(data.dimension);
                    var pX = GetAxisPosition(grid, xAxis, dataZoom, serieDataCount, serieData.index);
                    var pY = GetAxisPosition(grid, yAxis, dataZoom, serieDataCount, data.runtimeValue);
                    return new Vector3(pX, pY);
                case MarkLineType.Max:
                    serieData = SerieHelper.GetMaxSerieData(serie, data.dimension, dataZoom);
                    data.runtimeValue = serieData.GetData(data.dimension);
                    pX = GetAxisPosition(grid, xAxis, dataZoom, serieDataCount, serieData.index);
                    pY = GetAxisPosition(grid, yAxis, dataZoom, serieDataCount, data.runtimeValue);
                    return new Vector3(pX, pY);
                case MarkLineType.None:
                    if (data.zeroPosition)
                    {
                        data.runtimeValue = 0;
                        return grid.context.position;
                    }
                    else
                    {
                        pX = data.xPosition != 0 ? grid.context.x + data.xPosition :
                            GetAxisPosition(grid, xAxis, dataZoom, serieDataCount, data.xValue);
                        pY = data.yPosition != 0 ? grid.context.y + data.yPosition :
                            GetAxisPosition(grid, yAxis, dataZoom, serieDataCount, data.yValue);
                        data.runtimeValue = data.yValue;
                        return new Vector3(pX, pY);
                    }
                default:
                    return grid.context.position;
            }
        }
    }
}