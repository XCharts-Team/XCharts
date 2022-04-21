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

namespace XCharts
{
    internal class DrawSerieRing : IDrawSerie
    {
        public BaseChart chart;
        private bool m_UpdateTitleText = false;
        private bool m_UpdateLabelText = false;
        private bool m_IsEnterLegendButtom;

        public DrawSerieRing(BaseChart chart)
        {
            this.chart = chart;
        }

        public void InitComponent()
        {
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
                    if (serie.type == SerieType.Ring)
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
                    if (serie.type == SerieType.Ring)
                    {
                        SerieLabelHelper.SetRingLabelText(serie, chart.theme);
                    }
                }
            }
        }

        public void DrawBase(VertexHelper vh)
        {
        }

        public void DrawSerie(VertexHelper vh, Serie serie)
        {
            if (serie.type != SerieType.Ring) return;
            if (!serie.show || serie.animation.HasFadeOut()) return;
            var data = serie.data;
            serie.animation.InitProgress(data.Count, serie.startAngle, serie.startAngle + 360);
            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            TitleStyleHelper.CheckTitle(serie, ref chart.m_ReinitTitle, ref m_UpdateTitleText);
            SerieLabelHelper.CheckLabel(serie, ref chart.m_ReinitLabel, ref m_UpdateLabelText);
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var ringWidth = serie.runtimeOutsideRadius - serie.runtimeInsideRadius;
            var dataChanging = false;
            for (int j = 0; j < data.Count; j++)
            {
                var serieData = data[j];
                if (!serieData.show) continue;
                if (serieData.IsDataChanged()) dataChanging = true;
                var value = serieData.GetFirstData(dataChangeDuration);
                var max = serieData.GetLastData();
                var degree = (float)(360 * value / max);
                var startDegree = GetStartAngle(serie);
                var toDegree = GetToAngle(serie, degree);
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, serieData.highlighted);
                var itemColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, j, serieData.highlighted);
                var itemToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, j, serieData.highlighted);
                var outsideRadius = serie.runtimeOutsideRadius - j * (ringWidth + serie.ringGap);
                var insideRadius = outsideRadius - ringWidth;
                var centerRadius = (outsideRadius + insideRadius) / 2;
                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;
                var roundCap = serie.roundCap && insideRadius > 0;

                serieData.runtimePieStartAngle = serie.clockwise ? startDegree : toDegree;
                serieData.runtimePieToAngle = serie.clockwise ? toDegree : startDegree;
                serieData.runtimePieInsideRadius = insideRadius;
                serieData.runtimePieOutsideRadius = outsideRadius;
                if (itemStyle.backgroundColor.a != 0)
                {
                    UGL.DrawDoughnut(vh, serie.runtimeCenterPos, insideRadius, outsideRadius, itemStyle.backgroundColor,
                        itemStyle.backgroundColor, Color.clear, 0, 360, borderWidth, borderColor, 0,
                        chart.settings.cicleSmoothness, false, serie.clockwise);
                }
                var isGradient = !UGLHelper.IsValueEqualsColor(itemColor, itemToColor);
                if (isGradient)
                {
                    if (serie.clockwise)
                        itemToColor = Color.Lerp(itemColor, itemToColor, toDegree / (startDegree + 360));
                    else
                        itemToColor = Color.Lerp(itemToColor, itemColor, toDegree / (startDegree + 360));
                }
                UGL.DrawDoughnut(vh, serie.runtimeCenterPos, insideRadius, outsideRadius, itemColor, itemToColor,
                    Color.clear, startDegree, toDegree, borderWidth, borderColor, 0, chart.settings.cicleSmoothness,
                    roundCap, serie.clockwise);
                DrawCenter(vh, serie, serieData, insideRadius, j == data.Count - 1);
                UpateLabelPosition(serie, serieData, j, startDegree, toDegree, centerRadius);
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(360);
                chart.RefreshChart();
            }
            if (dataChanging)
            {
                chart.RefreshChart();
            }
        }

        public void RefreshLabel()
        {
        }

        public bool CheckTootipArea(Vector2 local)
        {
            if (!chart.series.Contains(SerieType.Ring)) return false;
            if (m_IsEnterLegendButtom) return false;
            bool selected = false;
            chart.tooltip.runtimeDataIndex.Clear();
            foreach (var serie in chart.series.list)
            {
                int index = GetRingIndex(serie, local);
                chart.tooltip.runtimeDataIndex.Add(index);
                if (serie.type != SerieType.Ring) continue;
                bool refresh = false;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (serieData.highlighted != (j == index)) refresh = true;
                    serieData.highlighted = j == index;
                }
                if (index >= 0) selected = true;
                if (refresh) chart.RefreshChart();
            }
            if (selected)
            {
                chart.tooltip.UpdateContentPos(local + chart.tooltip.offset);
                UpdateTooltip();
            }
            else if (chart.tooltip.IsActive())
            {
                chart.tooltip.SetActive(false);
                chart.RefreshChart();
            }
            return true;
        }

        public bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Ring)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Ring)) return false;
            LegendHelper.CheckDataShow(chart.series, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshChart();
            return true;
        }

        public bool OnLegendButtonEnter(int index, string legendName)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Ring)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Ring)) return false;
            m_IsEnterLegendButtom = true;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, true);
            chart.RefreshChart();
            return true;
        }

        public bool OnLegendButtonExit(int index, string legendName)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Ring)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Ring)) return false;
            m_IsEnterLegendButtom = false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, false);
            chart.RefreshChart();
            return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        private float GetStartAngle(Serie serie)
        {
            return serie.clockwise ? serie.startAngle : 360 - serie.startAngle;
        }

        private float GetToAngle(Serie serie, float angle)
        {
            var toAngle = angle + serie.startAngle;
            if (!serie.clockwise)
            {
                toAngle = 360 - angle - serie.startAngle;
            }
            if (!serie.animation.IsFinish())
            {
                var currAngle = serie.animation.GetCurrDetail();
                if (serie.clockwise)
                {
                    toAngle = toAngle > currAngle ? currAngle : toAngle;
                }
                else
                {
                    toAngle = toAngle < 360 - currAngle ? 360 - currAngle : toAngle;
                }
            }
            return toAngle;
        }

        private void DrawCenter(VertexHelper vh, Serie serie, SerieData serieData, float insideRadius, bool last)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!ChartHelper.IsClearColor(itemStyle.centerColor) && last)
            {
                var radius = insideRadius - itemStyle.centerGap;
                var smoothness = chart.settings.cicleSmoothness;
                UGL.DrawCricle(vh, serie.runtimeCenterPos, radius, itemStyle.centerColor, smoothness);
            }
        }

        private void UpateLabelPosition(Serie serie, SerieData serieData, int index, float startAngle,
            float toAngle, float centerRadius)
        {
            if (!serie.label.show) return;
            if (serieData.labelObject == null) return;
            switch (serie.label.position)
            {
                case SerieLabel.Position.Center:
                    serieData.labelPosition = serie.runtimeCenterPos + serie.label.offset;
                    break;
                case SerieLabel.Position.Bottom:
                    var px1 = Mathf.Sin(startAngle * Mathf.Deg2Rad) * centerRadius;
                    var py1 = Mathf.Cos(startAngle * Mathf.Deg2Rad) * centerRadius;
                    var xDiff = serie.clockwise ? -serie.label.margin : serie.label.margin;
                    serieData.labelPosition = serie.runtimeCenterPos + new Vector3(px1 + xDiff, py1);
                    break;
                case SerieLabel.Position.Top:
                    startAngle += serie.clockwise ? -serie.label.margin : serie.label.margin;
                    toAngle += serie.clockwise ? serie.label.margin : -serie.label.margin;
                    var px2 = Mathf.Sin(toAngle * Mathf.Deg2Rad) * centerRadius;
                    var py2 = Mathf.Cos(toAngle * Mathf.Deg2Rad) * centerRadius;
                    serieData.labelPosition = serie.runtimeCenterPos + new Vector3(px2, py2);
                    break;
            }
            serieData.labelObject.SetLabelPosition(serieData.labelPosition);
        }

        private void DrawBackground(VertexHelper vh, Serie serie, SerieData serieData, int index, float insideRadius, float outsideRadius)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, index, false);
            if (itemStyle.backgroundWidth != 0)
            {
                var centerRadius = (outsideRadius + insideRadius) / 2;
                var inradius = centerRadius - itemStyle.backgroundWidth / 2;
                var outradius = centerRadius + itemStyle.backgroundWidth / 2;
                UGL.DrawDoughnut(vh, serie.runtimeCenterPos, inradius,
                    outradius, backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
            else
            {
                UGL.DrawDoughnut(vh, serie.runtimeCenterPos, insideRadius,
                    outsideRadius, backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
        }

        private void DrawBorder(VertexHelper vh, Serie serie, SerieData serieData, float insideRadius, float outsideRadius)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (itemStyle.show && itemStyle.borderWidth > 0 && !ChartHelper.IsClearColor(itemStyle.borderColor))
            {
                UGL.DrawDoughnut(vh, serie.runtimeCenterPos, outsideRadius,
                outsideRadius + itemStyle.borderWidth, itemStyle.borderColor,
                Color.clear, chart.settings.cicleSmoothness);
                UGL.DrawDoughnut(vh, serie.runtimeCenterPos, insideRadius,
                insideRadius + itemStyle.borderWidth, itemStyle.borderColor,
                Color.clear, chart.settings.cicleSmoothness);
            }
        }


        private void DrawRoundCap(VertexHelper vh, Serie serie, Vector3 centerPos, Color color,
            float insideRadius, float outsideRadius, ref float drawStartDegree, ref float drawEndDegree)
        {
            if (serie.roundCap && insideRadius > 0 && drawStartDegree != drawEndDegree)
            {
                var width = (outsideRadius - insideRadius) / 2;
                var radius = insideRadius + width;

                var diffDegree = Mathf.Asin(width / radius) * Mathf.Rad2Deg;
                drawStartDegree += serie.clockwise ? diffDegree : -diffDegree;
                drawEndDegree -= serie.clockwise ? diffDegree : -diffDegree;
                UGL.DrawRoundCap(vh, centerPos, width, radius, drawStartDegree, serie.clockwise, color, false);
                UGL.DrawRoundCap(vh, centerPos, width, radius, drawEndDegree, serie.clockwise, color, true);
            }
        }

        private int GetRingIndex(Serie serie, Vector2 local)
        {
            if (serie.type != SerieType.Ring) return -1;
            var dist = Vector2.Distance(local, serie.runtimeCenterPos);
            if (dist > serie.runtimeOutsideRadius) return -1;
            Vector2 dir = local - new Vector2(serie.runtimeCenterPos.x, serie.runtimeCenterPos.y);
            float angle = VectorAngle(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (dist >= serieData.runtimePieInsideRadius &&
                    dist <= serieData.runtimePieOutsideRadius &&
                    angle >= serieData.runtimePieStartAngle &&
                    angle <= serieData.runtimePieToAngle)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool PointerIsInRingSerie(Series series, Vector2 local)
        {
            foreach (var serie in series.list)
            {
                if (serie.type != SerieType.Ring) continue;
                if (GetRingIndex(serie, local) >= 0) return true;
            }
            return false;
        }

        private float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        private void UpdateTooltip()
        {
            bool showTooltip = false;
            foreach (var serie in chart.series.list)
            {
                int index = chart.tooltip.runtimeDataIndex[serie.index];
                if (index < 0) continue;
                showTooltip = true;
                var content = TooltipHelper.GetFormatterContent(chart.tooltip, index, chart);
                TooltipHelper.SetContentAndPosition(chart.tooltip, content.TrimStart(), chart.chartRect);
            }
            chart.tooltip.SetActive(showTooltip);
        }
    }
}