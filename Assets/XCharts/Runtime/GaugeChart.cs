using System;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/GaugeChart", 19)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class GaugeChart : BaseChart
    {
        private static readonly string s_AxisLabelObjectName = "axis_label";
        private bool m_UpdateTitleText = false;
        private bool m_UpdateLabelText = false;

        protected override void Awake()
        {
            base.Awake();
            InitAxisLabel();
        }

        protected override void Start()
        {
            base.Start();
            foreach (var serie in m_Series.list)
            {
                UpdateTitle(serie);
                UpdateLabel(serie);
            }
            UpdateTitleAndLabelText();
        }

        protected override void Update()
        {
            base.Update();
            if (m_UpdateLabelText || m_UpdateTitleText)
            {
                m_UpdateTitleText = false;
                m_UpdateLabelText = false;
                UpdateTitleAndLabelText();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            RemoveData();
            m_Title.text = "GuageChart";
            var serie = AddSerie(SerieType.Gauge, "serie1");
            serie.min = 0;
            serie.max = 100;
            serie.startAngle = -125;
            serie.endAngle = 125;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.5f;
            serie.radius[0] = 80;
            serie.splitNumber = 5;
            serie.animation.updateAnimation = true;
            serie.titleStyle.show = true;
            serie.titleStyle.textStyle.offset = new Vector2(0, 20);
            serie.label.show = true;
            serie.label.offset = new Vector3(0, -30);
            serie.itemStyle.show = true;
            serie.gaugeAxis.axisLabel.show = true;
            serie.gaugeAxis.axisLabel.margin = 18;
            AddData(0, UnityEngine.Random.Range(10, 90), "title");
            InitAxisLabel();
        }
#endif

        private void InitAxisLabel()
        {
            Debug.LogError("InitAxisLabel");
            var labelObject = ChartHelper.AddObject(s_AxisLabelObjectName, transform, Vector2.zero,
                Vector2.zero, Vector2.zero, new Vector2(chartWidth, chartHeight));
            SerieLabelPool.ReleaseAll(labelObject.transform);
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                
                var serieLabel = serie.gaugeAxis.axisLabel;
                serie.gaugeAxis.ClearLabelObject();
                var count = serie.splitNumber > 36 ? 36 : (serie.splitNumber + 1);
                var startAngle = serie.startAngle;
                Debug.LogError("InitAxisLabel:"+serie.name+","+count);
                for (int j = 0; j < count; j++)
                {
                    var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, i, j);
                    var color = Color.grey;
                    var labelObj = SerieLabelPool.Get(textName, labelObject.transform, serieLabel, m_ThemeInfo.font, color, 100, 100);
                    var iconImage = labelObj.transform.Find("Icon").GetComponent<Image>();
                    var isAutoSize = serieLabel.backgroundWidth == 0 || serieLabel.backgroundHeight == 0;
                    var item = new LabelObject();
                    item.SetLabel(labelObj, isAutoSize, serieLabel.paddingLeftRight, serieLabel.paddingTopBottom);
                    item.SetIcon(iconImage);
                    item.SetIconActive(false);
                    serie.gaugeAxis.AddLabelObject(item);
                }
                 UpdateAxisLabel(serie);
            }
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            DrawData(vh);
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
        }

        private void DrawData(VertexHelper vh)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type != SerieType.Gauge) continue;
                DrawGauge(vh, serie);
            }
        }

        private void DrawGauge(VertexHelper vh, Serie serie)
        {
            serie.UpdateCenter(chartWidth, chartHeight);
            var destAngle = GetCurrAngle(serie, true);
            serie.animation.InitProgress(0, serie.startAngle, destAngle);
            var currAngle = serie.animation.IsFinish() ? GetCurrAngle(serie, false) : serie.animation.GetCurrDetail();

            DrawProgressBar(vh, serie, currAngle);
            DrawStageColor(vh, serie);
            DrawSplitLine(vh, serie);
            DrawAxisTick(vh, serie);
            DrawPointer(vh, serie, currAngle);
            UpdateTitle(serie);
            // UpdateAxisLabel(serie);
            UpdateLabel(serie);

            CheckAnimation(serie);
            if (!serie.animation.IsFinish())
            {
                float duration = serie.animation.duration > 0 ? (float)serie.animation.duration / 1000 : 1;
                float speed = (destAngle - serie.startAngle) / duration;
                serie.animation.CheckProgress(Time.deltaTime * speed);
                RefreshChart();
            }
            else if (NeedRefresh(serie))
            {
                RefreshChart();
            }
        }

        private void DrawProgressBar(VertexHelper vh, Serie serie, float currAngle)
        {
            if (serie.gaugeType != GaugeType.ProgressBar) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisLine.show) return;
            var color = serie.gaugeAxis.GetAxisLineColor(m_ThemeInfo, serie.index);
            var backgroundColor = serie.gaugeAxis.GetAxisLineBackgroundColor(m_ThemeInfo, serie.index);
            var outsideRadius = serie.runtimeInsideRadius + serie.gaugeAxis.axisLine.width;
            ChartDrawer.DrawDoughnut(vh, serie.runtimeCenterPos, serie.runtimeInsideRadius, outsideRadius,
                backgroundColor, m_ThemeInfo.backgroundColor, m_Settings.cicleSmoothness, serie.startAngle, serie.endAngle);
            if (serie.arcShaped)
            {
                DrawArcShape(vh, serie, serie.startAngle, backgroundColor, true);
                DrawArcShape(vh, serie, serie.endAngle, backgroundColor);
            }
            ChartDrawer.DrawDoughnut(vh, serie.runtimeCenterPos, serie.runtimeInsideRadius, outsideRadius,
                color, m_ThemeInfo.backgroundColor, m_Settings.cicleSmoothness, serie.startAngle, currAngle);
            if (serie.arcShaped && currAngle != serie.startAngle)
            {
                DrawArcShape(vh, serie, currAngle, color);
                DrawArcShape(vh, serie, serie.startAngle, color, true);
            }
        }

        private void DrawStageColor(VertexHelper vh, Serie serie)
        {
            if (serie.gaugeType != GaugeType.Pointer) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.axisLine.show) return;
            var totalAngle = serie.endAngle - serie.startAngle;
            var tempStartAngle = serie.startAngle;
            var tempEndAngle = serie.startAngle;
            var outsideRadius = serie.runtimeInsideRadius + serie.gaugeAxis.axisLine.width;
            serie.gaugeAxis.runtimeStageAngle.Clear();
            for (int i = 0; i < serie.gaugeAxis.axisLine.stageColor.Count; i++)
            {
                var stageColor = serie.gaugeAxis.axisLine.stageColor[i];
                tempEndAngle = serie.startAngle + totalAngle * stageColor.percent;
                serie.gaugeAxis.runtimeStageAngle.Add(tempEndAngle);
                ChartDrawer.DrawDoughnut(vh, serie.runtimeCenterPos, serie.runtimeInsideRadius, outsideRadius,
                    stageColor.color, m_ThemeInfo.backgroundColor, m_Settings.cicleSmoothness, tempStartAngle, tempEndAngle);
                tempStartAngle = tempEndAngle;
            }
        }

        private void DrawPointer(VertexHelper vh, Serie serie, float currAngle)
        {
            if (!serie.gaugePointer.show) return;
            var pointerColor = serie.gaugeAxis.GetPointerColor(m_ThemeInfo, serie.index, currAngle, serie.itemStyle);
            var len = serie.gaugePointer.length < 1 && serie.gaugePointer.length > -1 ?
                serie.runtimeInsideRadius * serie.gaugePointer.length :
                serie.gaugePointer.length;
            var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle, len);
            var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle + 180, serie.gaugePointer.width);
            var p3 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle - 90, serie.gaugePointer.width / 2);
            var p4 = ChartHelper.GetPosition(serie.runtimeCenterPos, currAngle + 90, serie.gaugePointer.width / 2);
            ChartDrawer.DrawPolygon(vh, p1, p3, p2, p4, pointerColor);
        }

        private void DrawSplitLine(VertexHelper vh, Serie serie)
        {
            if (serie.gaugeType != GaugeType.Pointer) return;
            if (!serie.gaugeAxis.show || !serie.gaugeAxis.splitLine.show) return;
            if (serie.splitNumber <= 0) return;
            var splitLine = serie.gaugeAxis.splitLine;
            var totalAngle = serie.endAngle - serie.startAngle;
            var diffAngle = totalAngle / serie.splitNumber;
            var outsideRadius = serie.runtimeInsideRadius + serie.gaugeAxis.axisLine.width;
            var insideRadius = outsideRadius - splitLine.length;
            var lineWidth = splitLine.lineStyle.width;
            for (int i = 0; i < serie.splitNumber + 1; i++)
            {
                var angle = serie.startAngle + i * diffAngle;
                var lineColor = serie.gaugeAxis.GetSplitLineColor(m_ThemeInfo, serie.index, angle);
                var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, insideRadius);
                var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, outsideRadius);
                ChartDrawer.DrawLine(vh, p1, p2, lineWidth, lineColor);
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
            var outsideRadius = serie.runtimeInsideRadius + serie.gaugeAxis.axisLine.width;
            var insideRadius = outsideRadius - (axisTick.length < 1 ? serie.gaugeAxis.axisLine.width * axisTick.length : axisTick.length);
            var lineWidth = axisTick.lineStyle.width;
            for (int i = 0; i < serie.splitNumber; i++)
            {
                for (int j = 1; j < axisTick.splitNumber; j++)
                {
                    var angle = serie.startAngle + i * diffAngle + j * (diffAngle / axisTick.splitNumber);
                    var lineColor = serie.gaugeAxis.GetSplitLineColor(m_ThemeInfo, serie.index, angle);
                    var p1 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, insideRadius);
                    var p2 = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, outsideRadius);
                    ChartDrawer.DrawLine(vh, p1, p2, lineWidth, lineColor);
                }
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

        private void DrawArcShape(VertexHelper vh, Serie serie, float angle, Color color, bool invert = false)
        {
            var radius = serie.gaugeAxis.axisLine.width / 2;
            var len = serie.runtimeInsideRadius + radius;
            var pos = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, len);
            var startAngle = invert ? angle + 180 : angle;
            var endAngle = invert ? angle + 360 : angle + 180;
            ChartDrawer.DrawSector(vh, pos, radius, color, startAngle, endAngle);
        }

        private void CheckAnimation(Serie serie)
        {
            var serieData = serie.GetSerieData(0);
            if (serieData != null)
            {
                var value = serieData.GetCurrData(1, serie.animation.GetUpdateAnimationDuration());
                var data = serieData.GetData(1);
                if (value != data) m_RefreshChart = true;
            }
        }

        private void UpdateTitle(Serie serie)
        {
            if (serie.titleStyle.show)
            {
                if (serie.titleStyle.IsInited())
                {
                    serie.titleStyle.SetActive(true);
                    serie.titleStyle.UpdatePosition(serie.runtimeCenterPos);
                    m_UpdateTitleText = true;
                }
                else
                {
                    m_ReinitTitle = true;
                }
            }
            else
            {
                serie.titleStyle.SetActive(false);
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
            var dataName = serieData != null ? serieData.name : null;
            var customLabelText = serie.gaugeAxis.axisLabelText;
            for (int j = 0; j <= count; j++)
            {
                var angle = serie.startAngle + j * diffAngle;
                var value = serie.min + j * diffValue;
                var pos = ChartHelper.GetPosition(serie.runtimeCenterPos, angle, radius);
                var text = customLabelText != null && j < customLabelText.Count ? customLabelText[j] :
                    serie.gaugeAxis.axisLabel.GetFormatterContent(serie.name, dataName, value, totalValue);
                serie.gaugeAxis.SetLabelObjectText(j, text);
                serie.gaugeAxis.SetLabelObjectPosition(j, pos);
            }
        }

        private void UpdateLabel(Serie serie)
        {
            var serieData = serie.GetSerieData(0);
            if (serieData != null)
            {
                if (serie.label.show)
                {
                    if (serieData.IsInitLabel())
                    {
                        serieData.SetLabelActive(true);
                        serieData.SetLabelPosition(serie.runtimeCenterPos + serie.label.offset);
                        m_UpdateLabelText = true;
                    }
                    else
                    {
                        m_ReinitLabel = true;
                    }
                }
                else
                {
                    serieData.SetLabelActive(false);
                }
            }
        }

        private void UpdateTitleAndLabelText()
        {
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.Gauge)
                {
                    var serieData = serie.GetSerieData(0);
                    if (serieData != null)
                    {
                        if (serie.label.show && serieData.IsInitLabel())
                        {
                            var value = serieData.GetData(1);
                            var total = serie.max;
                            var content = serie.label.GetFormatterContent(serie.name, serieData.name, value, total);
                            serieData.SetLabelText(content);
                            if (serie.label.color != Color.clear)
                            {
                                serieData.SetLabelColor(serie.label.color);
                            }
                        }
                        if (serie.titleStyle.show && serie.titleStyle.IsInited())
                        {
                            serie.titleStyle.SetText(serieData.name);
                        }
                    }
                    UpdateAxisLabel(serie);
                }
            }
        }

        private float GetCurrAngle(Serie serie, bool dest)
        {
            float rangeValue = serie.max - serie.min;
            float rangeAngle = serie.endAngle - serie.startAngle;
            float value = 0;
            float angle = serie.startAngle;
            if (serie.dataCount > 0)
            {
                var serieData = serie.data[0];
                value = dest ? serieData.GetData(1) : serieData.GetCurrData(1, serie.animation.GetUpdateAnimationDuration());
                value = Mathf.Clamp(value, serie.min, serie.max);
            }
            if (rangeValue > 0)
            {
                angle += rangeAngle * value / rangeValue;
            }
            return angle;
        }

        protected override void OnRefreshLabel()
        {
            //TODO:
        }
    }
}
