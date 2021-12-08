/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class RingHandler : SerieHandler<Ring>
    {
        private bool m_UpdateTitleText = false;
        private bool m_UpdateLabelText = false;

        public override void Update()
        {
            if (m_UpdateTitleText)
            {
                m_UpdateTitleText = false;
                foreach (var serie in chart.series)
                {
                    if (serie is Ring)
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
                    if (serie is Ring)
                    {
                        SerieLabelHelper.SetRingLabelText(serie, chart.theme);
                    }
                }
            }
        }

        public override bool SetDefaultTooltipContent(Tooltip tooltip, StringBuilder sb)
        {
            if (!serie.context.pointerEnter || serie.context.pointerItemDataIndex < 0) return false;
            var serieData = serie.GetSerieData(serie.context.pointerItemDataIndex);
            if (serieData == null) return false;
            var key = serieData.name;
            var numericFormatter = TooltipHelper.GetItemNumericFormatter(tooltip, serie, serieData);
            var value = serieData.GetData(1);
            if (!string.IsNullOrEmpty(serie.serieName))
            {
                sb.Append(serie.serieName).Append(FormatterHelper.PH_NN);
            }
            sb.Append("<color=#").Append(chart.theme.GetColorStr(serie.context.pointerItemDataIndex)).Append(">● </color>");
            if (!string.IsNullOrEmpty(key))
                sb.Append(key).Append(": ");
            sb.Append(ChartCached.FloatToStr(value, numericFormatter));
            return true;
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (!serie.show || serie.animation.HasFadeOut()) return;
            var data = serie.data;
            serie.animation.InitProgress(data.Count, serie.startAngle, serie.startAngle + 360);
            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var ringWidth = serie.context.outsideRadius - serie.context.insideRadius;
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
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, serieData.context.highlighted);
                var itemColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, j, serieData.context.highlighted);
                var itemToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, j, serieData.context.highlighted);
                var outsideRadius = serie.context.outsideRadius - j * (ringWidth + serie.ringGap);
                var insideRadius = outsideRadius - ringWidth;
                var centerRadius = (outsideRadius + insideRadius) / 2;
                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;
                var roundCap = serie.roundCap && insideRadius > 0;

                serieData.context.startAngle = serie.clockwise ? startDegree : toDegree;
                serieData.context.toAngle = serie.clockwise ? toDegree : startDegree;
                serieData.context.insideRadius = insideRadius;
                serieData.context.outsideRadius = serieData.radius > 0 ? serieData.radius : outsideRadius;
                if (itemStyle.backgroundColor.a != 0)
                {
                    UGL.DrawDoughnut(vh, serie.context.center, insideRadius, outsideRadius, itemStyle.backgroundColor,
                        itemStyle.backgroundColor, Color.clear, 0, 360, borderWidth, borderColor, 0,
                        chart.settings.cicleSmoothness, false, serie.clockwise);
                }
                UGL.DrawDoughnut(vh, serie.context.center, insideRadius, outsideRadius, itemColor, itemToColor,
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

        public override bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!chart.HasSerie<Ring>()) return false;
            if (!LegendHelper.IsSerieLegend<Ring>(chart, legendName)) return false;
            LegendHelper.CheckDataShow(chart.series, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshChart();
            return true;
        }

        public override bool OnLegendButtonEnter(int index, string legendName)
        {
            if (!chart.HasSerie<Ring>()) return false;
            if (!LegendHelper.IsSerieLegend<Ring>(chart, legendName)) return false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, true);
            chart.RefreshChart();
            return true;
        }

        public override bool OnLegendButtonExit(int index, string legendName)
        {
            if (!chart.HasSerie<Ring>()) return false;
            if (!LegendHelper.IsSerieLegend<Ring>(chart, legendName)) return false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, false);
            chart.RefreshChart();
            return true;
        }

        public override void OnPointerDown(PointerEventData eventData)
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
                UGL.DrawCricle(vh, serie.context.center, radius, itemStyle.centerColor, smoothness);
            }
        }

        private void UpateLabelPosition(Serie serie, SerieData serieData, int index, float startAngle,
            float toAngle, float centerRadius)
        {
            if (!serie.label.show) return;
            if (serieData.labelObject == null) return;
            switch (serie.label.position)
            {
                case LabelStyle.Position.Center:
                    serieData.context.labelPosition = serie.context.center + serie.label.offset;
                    break;
                case LabelStyle.Position.Bottom:
                    var px1 = Mathf.Sin(startAngle * Mathf.Deg2Rad) * centerRadius;
                    var py1 = Mathf.Cos(startAngle * Mathf.Deg2Rad) * centerRadius;
                    var xDiff = serie.clockwise ? -serie.label.margin : serie.label.margin;
                    serieData.context.labelPosition = serie.context.center + new Vector3(px1 + xDiff, py1);
                    break;
                case LabelStyle.Position.Top:
                    startAngle += serie.clockwise ? -serie.label.margin : serie.label.margin;
                    toAngle += serie.clockwise ? serie.label.margin : -serie.label.margin;
                    var px2 = Mathf.Sin(toAngle * Mathf.Deg2Rad) * centerRadius;
                    var py2 = Mathf.Cos(toAngle * Mathf.Deg2Rad) * centerRadius;
                    serieData.context.labelPosition = serie.context.center + new Vector3(px2, py2);
                    break;
            }
            serieData.labelObject.SetLabelPosition(serieData.context.labelPosition);
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
                UGL.DrawDoughnut(vh, serie.context.center, inradius,
                    outradius, backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
            else
            {
                UGL.DrawDoughnut(vh, serie.context.center, insideRadius,
                    outsideRadius, backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
        }

        private void DrawBorder(VertexHelper vh, Serie serie, SerieData serieData, float insideRadius, float outsideRadius)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (itemStyle.show && itemStyle.borderWidth > 0 && !ChartHelper.IsClearColor(itemStyle.borderColor))
            {
                UGL.DrawDoughnut(vh, serie.context.center, outsideRadius,
                outsideRadius + itemStyle.borderWidth, itemStyle.borderColor,
                Color.clear, chart.settings.cicleSmoothness);
                UGL.DrawDoughnut(vh, serie.context.center, insideRadius,
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
            if (!(serie is Ring)) return -1;
            var dist = Vector2.Distance(local, serie.context.center);
            if (dist > serie.context.outsideRadius) return -1;
            Vector2 dir = local - new Vector2(serie.context.center.x, serie.context.center.y);
            float angle = VectorAngle(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (dist >= serieData.context.insideRadius &&
                    dist <= serieData.context.outsideRadius &&
                    angle >= serieData.context.startAngle &&
                    angle <= serieData.context.toAngle)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool PointerIsInRingSerie(List<Serie> series, Vector2 local)
        {
            foreach (var serie in series)
            {
                if (!(serie is Ring)) continue;
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
    }
}