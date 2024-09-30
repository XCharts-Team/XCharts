using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class RadarCoordHandler : MainComponentHandler<RadarCoord>
    {
        private const string INDICATOR_TEXT = "indicator";

        public override void InitComponent()
        {
            InitRadarCoord(component);
        }

        public override void Update()
        {
            base.Update();
            if (!chart.isPointerInChart)
            {
                component.context.isPointerEnter = false;
                return;
            }
            var radar = component;
            radar.context.isPointerEnter = radar.show &&
                Vector3.Distance(radar.context.center, chart.pointerPos) <= radar.context.radius;
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawRadarCoord(vh, component);
        }

        private void InitRadarCoord(RadarCoord radar)
        {
            float txtHig = 20;
            radar.painter = chart.GetPainter(radar.index);
            radar.refreshComponent = delegate()
            {
                radar.UpdateRadarCenter(chart);
                var radarObject = ChartHelper.AddObject("Radar" + radar.index, chart.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                radar.gameObject = radarObject;
                radar.gameObject.hideFlags = chart.chartHideFlags;
                ChartHelper.HideAllObject(radarObject.transform, INDICATOR_TEXT);
                for (int i = 0; i < radar.indicatorList.Count; i++)
                {
                    var indicator = radar.indicatorList[i];
                    var pos = radar.GetIndicatorPosition(i);
                    var objName = INDICATOR_TEXT + "_" + i;

                    var label = ChartHelper.AddChartLabel(objName, radarObject.transform, radar.axisName.labelStyle,
                        chart.theme.common, radar.GetFormatterIndicatorContent(i), Color.clear, TextAnchor.MiddleCenter);
                    label.SetActive(radar.axisName.show && radar.indicator && radar.axisName.labelStyle.show, true);
                    AxisHelper.AdjustCircleLabelPos(label, pos, radar.context.center, txtHig, radar.axisName.labelStyle.offset);
                }
                chart.RefreshBasePainter();
            };
            radar.refreshComponent.Invoke();
        }

        private void DrawRadarCoord(VertexHelper vh, RadarCoord radar)
        {
            if (!radar.show) return;
            radar.UpdateRadarCenter(chart);
            if (radar.shape == RadarCoord.Shape.Circle)
            {
                DrawCricleRadar(vh, radar);
            }
            else
            {
                DrawPolygonRadar(vh, radar);
            }
        }

        private void DrawCricleRadar(VertexHelper vh, RadarCoord radar)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = radar.context.radius / radar.splitNumber;
            int indicatorNum = radar.indicatorList.Count;
            Vector3 p = radar.context.center;
            Vector3 p1;
            float angle = 2 * Mathf.PI / indicatorNum;
            var lineColor = radar.axisLine.GetColor(chart.theme.axis.splitLineColor);
            var lineWidth = radar.axisLine.GetWidth(chart.theme.axis.lineWidth);
            var lineType = radar.axisLine.GetType(chart.theme.axis.lineType);
            var splitLineColor = radar.splitLine.GetColor(chart.theme.axis.splitLineColor);
            var splitLineWidth = radar.splitLine.GetWidth(chart.theme.axis.splitLineWidth);
            splitLineWidth *= 2f;
            for (int i = 0; i < radar.splitNumber; i++)
            {
                var color = radar.splitArea.GetColor(i, chart.theme.axis);
                outsideRadius = insideRadius + block;
                if (radar.splitArea.show)
                {
                    UGL.DrawDoughnut(vh, p, insideRadius, outsideRadius, color, Color.clear,
                        0, 360, chart.settings.cicleSmoothness);
                }
                if (radar.splitLine.show)
                {
                    UGL.DrawEmptyCricle(vh, p, outsideRadius, splitLineWidth, splitLineColor,
                        Color.clear, chart.settings.cicleSmoothness);
                }
                insideRadius = outsideRadius;
            }
            if (radar.axisLine.show)
            {
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = j * angle;
                    p1 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, p, p1, lineColor);
                }
            }
        }

        private void DrawPolygonRadar(VertexHelper vh, RadarCoord radar)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = radar.context.radius / radar.splitNumber;
            int indicatorNum = radar.indicatorList.Count;
            Vector3 p1, p2, p3, p4;
            Vector3 p = radar.context.center;
            var startAngle = radar.startAngle * Mathf.PI / 180;
            var angle = 2 * Mathf.PI / indicatorNum;
            var lineColor = radar.axisLine.GetColor(chart.theme.axis.splitLineColor);
            var lineWidth = radar.axisLine.GetWidth(chart.theme.axis.lineWidth);
            var lineType = radar.axisLine.GetType(chart.theme.axis.lineType);
            var splitLineColor = radar.splitLine.GetColor(chart.theme.axis.splitLineColor);
            var splitLineWidth = radar.splitLine.GetWidth(chart.theme.axis.splitLineWidth);
            var splitLineType = radar.splitLine.GetType(chart.theme.axis.splitLineType);
            for (int i = 0; i < radar.splitNumber; i++)
            {
                var color = radar.splitArea.GetColor(i, chart.theme.axis);
                outsideRadius = insideRadius + block;
                p1 = new Vector3(p.x + insideRadius * Mathf.Sin(startAngle), p.y + insideRadius * Mathf.Cos(startAngle));
                p2 = new Vector3(p.x + outsideRadius * Mathf.Sin(startAngle), p.y + outsideRadius * Mathf.Cos(startAngle));
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = startAngle + j * angle;
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle),
                        p.y + insideRadius * Mathf.Cos(currAngle));
                    if (radar.splitArea.show)
                    {
                        UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, color);
                    }
                    if (radar.splitLine.NeedShow(i, radar.splitNumber))
                    {
                        ChartDrawer.DrawLineStyle(vh, splitLineType, splitLineWidth, p2, p3, splitLineColor);
                    }
                    p1 = p4;
                    p2 = p3;
                }
                insideRadius = outsideRadius;
            }
            if (radar.axisLine.show)
            {
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = startAngle + j * angle;
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, p, p3, lineColor);
                }
            }
        }
    }
}