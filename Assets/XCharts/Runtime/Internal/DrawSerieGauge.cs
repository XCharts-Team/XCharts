/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XUGL;
using System.Collections.Generic;

namespace XCharts
{
    internal class DrawSerieGauge : IDrawSerie
    {
        public BaseChart chart;

        private static readonly string s_SerieLabelObjectName = "label";
        private static readonly string s_AxisLabelObjectName = "axis_label";
        private bool m_UpdateTitleText = false;
        private bool m_UpdateLabelText = false;
        private Dictionary<int, int> m_LastSplitNumber = new Dictionary<int, int>();

        public DrawSerieGauge(BaseChart chart)
        {
            this.chart = chart;
        }

        public void InitComponent()
        {
            InitAxisLabel();
        }

        public void CheckComponent()
        {
        }

        public void Update()
        {
            if (m_UpdateTitleText)
            {
                m_UpdateTitleText = false;

                foreach (var serie in chart.series.list)
                {
                    if (serie.type == SerieType.Gauge)
                    {
                        TitleStyleHelper.UpdateTitleText(serie);
                    }
                }
            }
            if (m_UpdateLabelText)
            {
                m_UpdateLabelText = false;
                foreach (var serie in chart.series.list)
                {
                    if (serie.type == SerieType.Gauge)
                    {
                        SerieLabelHelper.SetGaugeLabelText(serie);
                        UpdateAxisLabel(serie);
                    }
                }
            }
            foreach (var serie in chart.series.list)
            {
                if (serie.type == SerieType.Gauge)
                {
                    if (!m_LastSplitNumber.TryGetValue(serie.index, out var lastSplitNumber))
                    {
                        m_LastSplitNumber[serie.index] = lastSplitNumber;
                    }
                    else if (serie.splitNumber != lastSplitNumber)
                    {
                        m_LastSplitNumber[serie.index] = serie.splitNumber;
                        InitAxisLabel();
                    }
                }
            }
        }

        public void DrawBase(VertexHelper vh)
        {
        }

        public void DrawSerie(VertexHelper vh, Serie serie)
        {
            if (serie.type != SerieType.Gauge) return;
            DrawGauge(vh, serie);
        }

        public void RefreshLabel()
        {
        }

        public bool CheckTootipArea(Vector2 local)
        {
            if (!chart.series.Contains(SerieType.Gauge)) return false;
            var serie = GetPointerInSerieIndex(chart.series, local);
            if (serie != null)
            {
                chart.tooltip.runtimeDataIndex.Clear();
                chart.tooltip.runtimeDataIndex.Add(serie.index);
                chart.tooltip.UpdateContentPos(local + chart.tooltip.offset);
                UpdateTooltip();
                return true;
            }
            else if (chart.tooltip.IsActive())
            {
                chart.tooltip.SetActive(false);
                chart.RefreshChart();
            }
            return false;
        }

        public bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            return false;
        }

        public bool OnLegendButtonEnter(int index, string legendName)
        {
            return false;
        }

        public bool OnLegendButtonExit(int index, string legendName)
        {
            return false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        private void InitAxisLabel()
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Gauge)) return;
            var labelObject = ChartHelper.AddObject(s_AxisLabelObjectName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            labelObject.hideFlags = chart.chartHideFlags;
            SerieLabelPool.ReleaseAll(labelObject.transform);
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.series.list[i];
                if (serie.type != SerieType.Gauge) continue;
                var serieLabel = serie.gaugeAxis.axisLabel;
                var count = serie.splitNumber > 36 ? 36 : (serie.splitNumber + 1);
                var startAngle = serie.startAngle;
                var textColor = serieLabel.textStyle.GetColor(chart.theme.gauge.textColor);
                serie.gaugeAxis.ClearLabelObject();
                SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
                for (int j = 0; j < count; j++)
                {
                    var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, i, j);
                    var labelObj = SerieLabelPool.Get(textName, labelObject.transform, serieLabel, textColor, 100, 100, chart.theme);
                    var iconImage = labelObj.transform.Find("Icon").GetComponent<Image>();
                    var isAutoSize = serieLabel.backgroundWidth == 0 || serieLabel.backgroundHeight == 0;
                    var item = new ChartLabel();
                    item.SetLabel(labelObj, isAutoSize, serieLabel.paddingLeftRight, serieLabel.paddingTopBottom);
                    item.SetIcon(iconImage);
                    item.SetIconActive(false);
                    serie.gaugeAxis.AddLabelObject(item);
                }
                UpdateAxisLabel(serie);
            }
        }

        private void UpdateAxisLabel()
        {
            foreach (var serie in chart.series.list)
            {
                if (serie.type == SerieType.Gauge)
                {
                    UpdateAxisLabel(serie);
                }
            }
        }

        private void UpdateAxisLabel(Serie serie)
        {
            var show = serie.gaugeAxis.show && serie.gaugeAxis.axisLabel.show;
            serie.gaugeAxis.SetLabelObjectActive(show);
            if (!show)
            {
                return;
            }
            var count = serie.splitNumber > 36 ? 36 : serie.splitNumber;
            var startAngle = serie.startAngle;
            var totalAngle = serie.endAngle - serie.startAngle;
            var totalValue = serie.max - serie.min;
            var diffAngle = totalAngle / count;
            var diffValue = totalValue / count;
            var radius = serie.runtimeInsideRadius - serie.gaugeAxis.axisLabel.margin;
            var serieData = serie.GetSerieData(0);
            var customLabelText = serie.gaugeAxis.axisLabelText;
            for (int j = 0; j <= count; j++)
            {
                var angle = serie.startAngle + j * diffAngle;
                var value = serie.min + j * diffValue;
                var pos = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, radius);
                var text = customLabelText != null && j < customLabelText.Count ? customLabelText[j] :
                    SerieLabelHelper.GetFormatterContent(serie, serieData, value, totalValue,
                    serie.gaugeAxis.axisLabel, Color.clear);
                serie.gaugeAxis.SetLabelObjectText(j, text);
                serie.gaugeAxis.SetLabelObjectPosition(j, pos + serie.gaugeAxis.axisLabel.offset);
            }
        }

        private void DrawGauge(VertexHelper vh, Serie serie)
        {
            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            var destAngle = GetCurrAngle(serie, true);
            serie.animation.InitProgress(0, serie.startAngle, destAngle);
            var currAngle = serie.animation.IsFinish() ? GetCurrAngle(serie, false) : serie.animation.GetCurrDetail();
            DrawProgressBar(vh, serie, (float)currAngle);
            DrawStageColor(vh, serie);
            DrawLineStyle(vh, serie);
            DrawAxisTick(vh, serie);
            DrawPointer(vh, serie, (float)currAngle);
            TitleStyleHelper.CheckTitle(serie, ref chart.m_ReinitTitle, ref m_UpdateTitleText);
            SerieLabelHelper.CheckLabel(serie, ref chart.m_ReinitLabel, ref m_UpdateLabelText);
            CheckAnimation(serie);
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(destAngle - serie.startAngle);
                chart.RefreshPainter(serie);
            }
            else if (NeedRefresh(serie))
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawProgressBar(VertexHelper vh, Serie serie, float currAngle)
        {
            if (serie.gaugeType != GaugeType.ProgressBar) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisLine.show) return;
            var color = serie.gaugeAxis.GetAxisLineColor(chart.theme, serie.index);
            var backgroundColor = serie.gaugeAxis.GetAxisLineBackgroundColor(chart.theme, serie.index);
            var lineWidth = serie.gaugeAxis.axisLine.GetWidth(chart.theme.gauge.lineWidth);
            var outsideRadius = serie.runtimeInsideRadius + lineWidth;
            var borderWidth = serie.itemStyle.borderWidth;
            var borderColor = serie.itemStyle.borderColor;
            UGL.DrawDoughnut(vh, serie.runtimeCenterPos, serie.runtimeInsideRadius, outsideRadius,
                backgroundColor, backgroundColor, Color.clear, serie.startAngle, serie.endAngle, 0, Color.clear,
                0, chart.settings.cicleSmoothness, serie.roundCap);
            UGL.DrawDoughnut(vh, serie.runtimeCenterPos, serie.runtimeInsideRadius, outsideRadius,
                color, color, Color.clear, serie.startAngle, currAngle, 0, Color.clear,
                0, chart.settings.cicleSmoothness, serie.roundCap);
        }

        private void DrawStageColor(VertexHelper vh, Serie serie)
        {
            if (serie.gaugeType != GaugeType.Pointer) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisLine.show) return;
            var totalAngle = serie.endAngle - serie.startAngle;
            var tempStartAngle = serie.startAngle;
            var tempEndAngle = serie.startAngle;
            var lineWidth = serie.gaugeAxis.axisLine.GetWidth(chart.theme.gauge.lineWidth);
            var outsideRadius = serie.runtimeInsideRadius + lineWidth;
            serie.gaugeAxis.runtimeStageAngle.Clear();
            for (int i = 0; i < serie.gaugeAxis.axisLine.stageColor.Count; i++)
            {
                var stageColor = serie.gaugeAxis.axisLine.stageColor[i];
                tempEndAngle = serie.startAngle + totalAngle * stageColor.percent;
                serie.gaugeAxis.runtimeStageAngle.Add(tempEndAngle);
                UGL.DrawDoughnut(vh, serie.runtimeCenterPos, serie.runtimeInsideRadius, outsideRadius,
                    stageColor.color, stageColor.color, Color.clear, tempStartAngle, tempEndAngle, 0, Color.clear,
                    0, chart.settings.cicleSmoothness);
                tempStartAngle = tempEndAngle;
            }
        }

        private void DrawPointer(VertexHelper vh, Serie serie, float currAngle)
        {
            if (!serie.gaugePointer.show) return;
            var pointerColor = serie.gaugeAxis.GetPointerColor(chart.theme, serie.index, currAngle, serie.itemStyle);
            var pointerToColor = !ChartHelper.IsClearColor(serie.itemStyle.toColor) ? serie.itemStyle.toColor : pointerColor;
            var len = serie.gaugePointer.length < 1 && serie.gaugePointer.length > -1 ?
                serie.runtimeInsideRadius * serie.gaugePointer.length :
                serie.gaugePointer.length;
            var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle, len);
            var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle + 180, serie.gaugePointer.width);
            var p3 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle - 90, serie.gaugePointer.width / 2);
            var p4 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle + 90, serie.gaugePointer.width / 2);
            UGL.DrawTriangle(vh, p2, p3, p1, pointerColor, pointerColor, pointerToColor);
            UGL.DrawTriangle(vh, p4, p2, p1, pointerColor, pointerColor, pointerToColor);
        }

        private void DrawLineStyle(VertexHelper vh, Serie serie)
        {
            if (serie.gaugeType != GaugeType.Pointer) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.splitLine.show) return;
            if (serie.splitNumber <= 0) return;
            var splitLine = serie.gaugeAxis.splitLine;
            var totalAngle = serie.endAngle - serie.startAngle;
            var diffAngle = totalAngle / serie.splitNumber;
            var lineWidth = serie.gaugeAxis.axisLine.GetWidth(chart.theme.gauge.lineWidth);
            var splitLineWidth = splitLine.GetWidth(chart.theme.gauge.splitLineWidth);
            var splitLineLength = splitLine.GetLength(chart.theme.gauge.splitLineLength);
            var outsideRadius = serie.runtimeInsideRadius + lineWidth;
            var insideRadius = outsideRadius - splitLineLength;
            for (int i = 0; i < serie.splitNumber + 1; i++)
            {
                var angle = serie.startAngle + i * diffAngle;
                var lineColor = serie.gaugeAxis.GetSplitLineColor(chart.theme.gauge.splitLineColor, serie.index, angle);
                var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, insideRadius);
                var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, outsideRadius);
                UGL.DrawLine(vh, p1, p2, splitLineWidth, lineColor);
            }
        }

        private void DrawAxisTick(VertexHelper vh, Serie serie)
        {
            if (serie.gaugeType != GaugeType.Pointer) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisTick.show) return;
            if (serie.splitNumber <= 0) return;
            var axisTick = serie.gaugeAxis.axisTick;
            var totalAngle = serie.endAngle - serie.startAngle;
            var diffAngle = totalAngle / serie.splitNumber;
            var lineWidth = serie.gaugeAxis.axisLine.GetWidth(chart.theme.gauge.lineWidth);
            var tickWidth = axisTick.GetWidth(chart.theme.gauge.tickWidth);
            var tickLength = axisTick.GetLength(chart.theme.gauge.tickLength);
            var outsideRadius = serie.runtimeInsideRadius + lineWidth;

            var insideRadius = outsideRadius - (tickLength < 1 ? lineWidth * tickLength : tickLength);
            for (int i = 0; i < serie.splitNumber; i++)
            {
                for (int j = 1; j < axisTick.splitNumber; j++)
                {
                    var angle = serie.startAngle + i * diffAngle + j * (diffAngle / axisTick.splitNumber);
                    var lineColor = serie.gaugeAxis.GetSplitLineColor(chart.theme.gauge.tickColor, serie.index, angle);
                    var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, insideRadius);
                    var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, outsideRadius);
                    UGL.DrawLine(vh, p1, p2, tickWidth, lineColor);
                }
            }
        }

        private float GetCurrAngle(Serie serie, bool dest)
        {
            if (serie.animation.HasFadeOut())
            {
                return (float)serie.animation.GetCurrDetail();
            }
            float rangeValue = serie.max - serie.min;
            float rangeAngle = serie.endAngle - serie.startAngle;
            double value = 0;
            float angle = serie.startAngle;
            if (serie.dataCount > 0)
            {
                var serieData = serie.data[0];
                serieData.labelPosition = serie.runtimeCenterPos + serie.label.offset;
                value = dest ? serieData.GetData(1)
                    : serieData.GetCurrData(1, serie.animation.GetUpdateAnimationDuration());
                value = MathUtil.Clamp(value, serie.min, serie.max);
            }
            if (rangeValue > 0)
            {
                angle += rangeAngle * (float)(value - serie.min) / rangeValue;
            }
            return angle;
        }

        private void CheckAnimation(Serie serie)
        {
            var serieData = serie.GetSerieData(0);
            if (serieData != null)
            {
                var value = serieData.GetCurrData(1, serie.animation.GetUpdateAnimationDuration());
                var data = serieData.GetData(1);
                if (value != data) chart.RefreshPainter(serie);
            }
        }

        private bool NeedRefresh(Serie serie)
        {
            if (serie.type == SerieType.Gauge)
            {
                var serieData = serie.GetSerieData(0);
                if (serieData != null)
                {
                    var destValue = serieData.GetData(1);
                    var currValue = serieData.GetCurrData(1, serie.animation.GetUpdateAnimationDuration());
                    return destValue != currValue;
                }
            }
            return false;
        }

        private Serie GetPointerInSerieIndex(Series series, Vector2 local)
        {
            foreach (var serie in series.list)
            {
                if (serie.type != SerieType.Gauge) continue;
                if (!serie.gaugePointer.show) continue;
                var len = serie.gaugePointer.length < 1 && serie.gaugePointer.length > -1
                    ? serie.runtimeInsideRadius * serie.gaugePointer.length
                    : serie.gaugePointer.length;
                if (Vector3.Distance(local, serie.runtimeCenterPos) > len) continue;
                var currAngle = (float)(serie.animation.IsFinish() ? GetCurrAngle(serie, false) : serie.animation.GetCurrDetail());
                var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle, len);
                var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle + 180, serie.gaugePointer.width);
                var p3 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle - 90, serie.gaugePointer.width / 2);
                var p4 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle + 90, serie.gaugePointer.width / 2);
                if (ChartHelper.IsPointInQuadrilateral(local, p1, p3, p2, p4))
                {
                    return serie;
                }
            }
            return null;
        }

        private void UpdateTooltip()
        {
            int index = chart.tooltip.runtimeDataIndex[0];
            if (index < 0)
            {
                chart.tooltip.SetActive(false);
                return;
            }
            var content = TooltipHelper.GetFormatterContent(chart.tooltip, index, chart);
            TooltipHelper.SetContentAndPosition(chart.tooltip, content.TrimStart(), chart.chartRect);
            chart.tooltip.SetActive(true);
        }
    }
}