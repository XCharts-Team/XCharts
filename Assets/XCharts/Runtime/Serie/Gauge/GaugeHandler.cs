/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/



using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class GaugeHandler : SerieHandler<Gauge>
    {
        private static readonly string s_SerieLabelObjectName = "label";
        private static readonly string s_AxisLabelObjectName = "axis_label";
        private bool m_UpdateTitleText = false;
        private bool m_UpdateLabelText = false;

        public override void Update()
        {
            if (m_UpdateTitleText)
            {
                m_UpdateTitleText = false;

                foreach (var serie in chart.series)
                {
                    if (serie is Gauge)
                    {
                        serie.titleStyle.SetText(serie.serieName);
                    }
                }
            }
            if (m_UpdateLabelText)
            {
                m_UpdateLabelText = false;
                foreach (var serie in chart.series)
                {
                    if (serie is Gauge)
                    {
                        SerieLabelHelper.SetGaugeLabelText(serie);
                        UpdateAxisLabel(serie as Gauge);
                    }
                }
            }
            serie.context.pointerEnter = false;
            serie.context.pointerItemDataIndex = -1;
            foreach (var serieData in serie.data)
            {
                if (serieData.IsInPolygon(chart.pointerPos))
                {
                    serie.context.pointerEnter = true;
                    serie.context.pointerItemDataIndex = serieData.index;
                }
            }
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter,
            ref List<SerieParams> paramList, ref string title)
        {
            UpdateItemSerieParams(ref paramList, ref title, dataIndex, category, 
                marker, itemFormatter, numericFormatter);
        }

        public override void DrawSerie(VertexHelper vh)
        {
            DrawGauge(vh, serie);
        }

        private void InitAxisLabel()
        {
            if (!chart.HasSerie<Gauge>())
                return;

            var labelObject = ChartHelper.AddObject(s_AxisLabelObjectName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            labelObject.hideFlags = chart.chartHideFlags;
            SerieLabelPool.ReleaseAll(labelObject.transform);

            for (int i = 0; i < chart.series.Count; i++)
            {
                if (!(chart.series[i] is Gauge))
                    continue;

                var serie = chart.series[i] as Gauge;
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
                    var item = ChartHelper.GetOrAddComponent<ChartLabel>(labelObj);
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
            foreach (var serie in chart.series)
            {
                if (serie is Gauge)
                {
                    UpdateAxisLabel(serie as Gauge);
                }
            }
        }

        private void UpdateAxisLabel(Gauge serie)
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
            var radius = serie.context.insideRadius - serie.gaugeAxis.axisLabel.margin;
            var serieData = serie.GetSerieData(0);
            var customLabelText = serie.gaugeAxis.axisLabelText;
            for (int j = 0; j <= count; j++)
            {
                var angle = serie.startAngle + j * diffAngle;
                var value = serie.min + j * diffValue;
                var pos = ChartHelper.GetPosition(serie.context.center, angle, radius);
                var text = customLabelText != null && j < customLabelText.Count ? customLabelText[j] :
                    SerieLabelHelper.GetFormatterContent(serie, serieData, value, totalValue,
                    serie.gaugeAxis.axisLabel, Color.clear);
                serie.gaugeAxis.SetLabelObjectText(j, text);
                serie.gaugeAxis.SetLabelObjectPosition(j, pos + serie.gaugeAxis.axisLabel.offset);
            }
        }

        private void DrawGauge(VertexHelper vh, Gauge serie)
        {
            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            var destAngle = GetCurrAngle(serie, true);
            serie.animation.InitProgress(serie.startAngle, destAngle);
            var currAngle = serie.animation.IsFinish() ? GetCurrAngle(serie, false) : serie.animation.GetCurrDetail();
            DrawProgressBar(vh, serie, (float)currAngle);
            DrawStageColor(vh, serie);
            DrawLineStyle(vh, serie);
            DrawAxisTick(vh, serie);
            DrawPointer(vh, serie, (float)currAngle);
            //TitleStyleHelper.CheckTitle(serie, ref chart.m_ReinitTitle, ref m_UpdateTitleText);
            //SerieLabelHelper.CheckLabel(serie, ref chart.m_ReinitLabel, ref m_UpdateLabelText);
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

        private void DrawProgressBar(VertexHelper vh, Gauge serie, float currAngle)
        {
            if (serie.gaugeType != GaugeType.ProgressBar) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisLine.show) return;
            var color = serie.gaugeAxis.GetAxisLineColor(chart.theme, serie.index);
            var backgroundColor = serie.gaugeAxis.GetAxisLineBackgroundColor(chart.theme, serie.index);
            var lineWidth = serie.gaugeAxis.axisLine.GetWidth(chart.theme.gauge.lineWidth);
            var outsideRadius = serie.context.insideRadius + lineWidth;
            var borderWidth = serie.itemStyle.borderWidth;
            var borderColor = serie.itemStyle.borderColor;
            UGL.DrawDoughnut(vh, serie.context.center, serie.context.insideRadius, outsideRadius,
                backgroundColor, backgroundColor, Color.clear, serie.startAngle, serie.endAngle, 0, Color.clear,
                0, chart.settings.cicleSmoothness, serie.roundCap);
            UGL.DrawDoughnut(vh, serie.context.center, serie.context.insideRadius, outsideRadius,
                color, color, Color.clear, serie.startAngle, currAngle, 0, Color.clear,
                0, chart.settings.cicleSmoothness, serie.roundCap);
        }

        private void DrawStageColor(VertexHelper vh, Gauge serie)
        {
            if (serie.gaugeType != GaugeType.Pointer) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisLine.show) return;
            var totalAngle = serie.endAngle - serie.startAngle;
            var tempStartAngle = serie.startAngle;
            var tempEndAngle = serie.startAngle;
            var lineWidth = serie.gaugeAxis.axisLine.GetWidth(chart.theme.gauge.lineWidth);
            var outsideRadius = serie.context.insideRadius + lineWidth;
            serie.gaugeAxis.runtimeStageAngle.Clear();
            for (int i = 0; i < serie.gaugeAxis.axisLine.stageColor.Count; i++)
            {
                var stageColor = serie.gaugeAxis.axisLine.stageColor[i];
                tempEndAngle = serie.startAngle + totalAngle * stageColor.percent;
                serie.gaugeAxis.runtimeStageAngle.Add(tempEndAngle);
                UGL.DrawDoughnut(vh, serie.context.center, serie.context.insideRadius, outsideRadius,
                    stageColor.color, stageColor.color, Color.clear, tempStartAngle, tempEndAngle, 0, Color.clear,
                    0, chart.settings.cicleSmoothness);
                tempStartAngle = tempEndAngle;
            }
        }

        private void DrawPointer(VertexHelper vh, Gauge serie, float currAngle)
        {
            if (!serie.gaugePointer.show) return;
            var pointerColor = serie.gaugeAxis.GetPointerColor(chart.theme, serie.index, currAngle, serie.itemStyle);
            var pointerToColor = !ChartHelper.IsClearColor(serie.itemStyle.toColor) ? serie.itemStyle.toColor : pointerColor;
            var len = serie.gaugePointer.length < 1 && serie.gaugePointer.length > -1 ?
                serie.context.insideRadius * serie.gaugePointer.length :
                serie.gaugePointer.length;
            var p1 = ChartHelper.GetPosition(serie.context.center, currAngle, len);
            var p2 = ChartHelper.GetPosition(serie.context.center, currAngle + 180, serie.gaugePointer.width);
            var p3 = ChartHelper.GetPosition(serie.context.center, currAngle - 90, serie.gaugePointer.width / 2);
            var p4 = ChartHelper.GetPosition(serie.context.center, currAngle + 90, serie.gaugePointer.width / 2);
            UGL.DrawTriangle(vh, p2, p3, p1, pointerColor, pointerColor, pointerToColor);
            UGL.DrawTriangle(vh, p4, p2, p1, pointerColor, pointerColor, pointerToColor);
            if (serie.data.Count > 0)
                serie.data[0].SetPolygon(p1, p2, p3, p4);
        }

        private void DrawLineStyle(VertexHelper vh, Gauge serie)
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
            var outsideRadius = serie.context.insideRadius + lineWidth;
            var insideRadius = outsideRadius - splitLineLength;
            for (int i = 0; i < serie.splitNumber + 1; i++)
            {
                var angle = serie.startAngle + i * diffAngle;
                var lineColor = serie.gaugeAxis.GetSplitLineColor(chart.theme.gauge.splitLineColor, serie.index, angle);
                var p1 = ChartHelper.GetPosition(serie.context.center, angle, insideRadius);
                var p2 = ChartHelper.GetPosition(serie.context.center, angle, outsideRadius);
                UGL.DrawLine(vh, p1, p2, splitLineWidth, lineColor);
            }
        }

        private void DrawAxisTick(VertexHelper vh, Gauge serie)
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
            var outsideRadius = serie.context.insideRadius + lineWidth;

            var insideRadius = outsideRadius - (tickLength < 1 ? lineWidth * tickLength : tickLength);
            for (int i = 0; i < serie.splitNumber; i++)
            {
                for (int j = 1; j < axisTick.splitNumber; j++)
                {
                    var angle = serie.startAngle + i * diffAngle + j * (diffAngle / axisTick.splitNumber);
                    var lineColor = serie.gaugeAxis.GetSplitLineColor(chart.theme.gauge.tickColor, serie.index, angle);
                    var p1 = ChartHelper.GetPosition(serie.context.center, angle, insideRadius);
                    var p2 = ChartHelper.GetPosition(serie.context.center, angle, outsideRadius);
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
                serieData.context.labelPosition = serie.context.center + serie.label.offset;
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
            if (serie is Gauge)
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

        private Serie GetPointerInSerieIndex(List<Serie> series, Vector2 local)
        {
            foreach (var gauge in series)
            {
                if (!(gauge is Gauge))
                    continue;

                var serie = gauge as Gauge;
                if (!serie.gaugePointer.show)
                    continue;

                var len = serie.gaugePointer.length < 1 && serie.gaugePointer.length > -1
                    ? serie.context.insideRadius * serie.gaugePointer.length
                    : serie.gaugePointer.length;
                if (Vector3.Distance(local, serie.context.center) > len) continue;
                var currAngle = (float)(serie.animation.IsFinish() ? GetCurrAngle(serie, false) : serie.animation.GetCurrDetail());
                var p1 = ChartHelper.GetPosition(serie.context.center, currAngle, len);
                var p2 = ChartHelper.GetPosition(serie.context.center, currAngle + 180, serie.gaugePointer.width);
                var p3 = ChartHelper.GetPosition(serie.context.center, currAngle - 90, serie.gaugePointer.width / 2);
                var p4 = ChartHelper.GetPosition(serie.context.center, currAngle + 90, serie.gaugePointer.width / 2);
                if (ChartHelper.IsPointInQuadrilateral(local, p1, p3, p2, p4))
                {
                    return serie;
                }
            }
            return null;
        }
    }
}