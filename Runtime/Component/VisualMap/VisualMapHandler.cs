using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class VisualMapHandler : MainComponentHandler<VisualMap>
    {
        public override void OnBeginDrag(PointerEventData eventData)
        {
            OnDragVisualMapStart(component);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            OnDragVisualMap(component);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            OnDragVisualMapEnd(component);
        }

        public override void Update()
        {
            CheckVisualMap(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            var visualMap = component;
            if (!visualMap.show || !visualMap.showUI) return;
            switch (visualMap.type)
            {
                case VisualMap.Type.Continuous:
                    DrawContinuousVisualMap(vh, visualMap);
                    break;
                case VisualMap.Type.Piecewise:
                    //DrawPiecewiseVisualMap(vh, visualMap);
                    break;
            }
        }

        private void CheckVisualMap(VisualMap visualMap)
        {
            if (visualMap == null || !visualMap.show)
                return;

            if (chart.canvas == null)
                return;

            Vector2 local;
            if (!chart.ScreenPointToChartPoint(Input.mousePosition, out local))
            {
                if (visualMap.context.pointerIndex >= 0)
                {
                    visualMap.context.pointerIndex = -1;
                    chart.RefreshChart();
                }
                return;
            }

            if (local.x < chart.chartX ||
                local.x > chart.chartX + chart.chartWidth ||
                local.y < chart.chartY ||
                local.y > chart.chartY + chart.chartHeight ||
                !visualMap.IsInRangeRect(local, chart.chartRect))
            {
                if (visualMap.context.pointerIndex >= 0)
                {
                    visualMap.context.pointerIndex = -1;
                    chart.RefreshChart();
                }
                return;
            }

            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var halfHig = visualMap.itemHeight / 2;
            var centerPos = chart.chartPosition + visualMap.location.GetPosition(chart.chartWidth, chart.chartHeight);
            var selectedIndex = -1;
            double value = 0;

            switch (visualMap.orient)
            {
                case Orient.Horizonal:
                    pos1 = centerPos + Vector3.left * halfHig;
                    pos2 = centerPos + Vector3.right * halfHig;
                    value = visualMap.min + (local.x - pos1.x) / (pos2.x - pos1.x) * (visualMap.max - visualMap.min);
                    selectedIndex = visualMap.GetIndex(value);
                    break;

                case Orient.Vertical:
                    pos1 = centerPos + Vector3.down * halfHig;
                    pos2 = centerPos + Vector3.up * halfHig;
                    value = visualMap.min + (local.y - pos1.y) / (pos2.y - pos1.y) * (visualMap.max - visualMap.min);
                    selectedIndex = visualMap.GetIndex(value);
                    break;
            }

            visualMap.context.pointerValue = value;
            visualMap.context.pointerIndex = selectedIndex;
            chart.RefreshChart();
        }

        private void DrawContinuousVisualMap(VertexHelper vh, VisualMap visualMap)
        {
            var centerPos = chart.chartPosition + visualMap.location.GetPosition(chart.chartWidth, chart.chartHeight);
            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var dir = Vector3.zero;
            var halfWid = visualMap.itemWidth / 2;
            var halfHig = visualMap.itemHeight / 2;
            var xRadius = 0f;
            var yRadius = 0f;
            var splitNum = visualMap.inRange.Count;
            var splitWid = visualMap.itemHeight / (splitNum - 1);
            var isVertical = false;
            var colors = visualMap.inRange;
            var triangeLen = chart.theme.visualMap.triangeLen;

            switch (visualMap.orient)
            {
                case Orient.Horizonal:
                    pos1 = centerPos + Vector3.left * halfHig;
                    pos2 = centerPos + Vector3.right * halfHig;
                    dir = Vector3.right;
                    xRadius = splitWid / 2;
                    yRadius = halfWid;
                    isVertical = false;
                    if (visualMap.calculable)
                    {
                        var p0 = pos1 + Vector3.right * visualMap.runtimeRangeMinHeight;
                        var p1 = p0 + Vector3.up * halfWid;
                        var p2 = p0 + Vector3.up * (halfWid + triangeLen);
                        var p3 = p2 + Vector3.left * triangeLen;
                        var color = visualMap.GetColor(visualMap.rangeMin);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                        p0 = pos1 + Vector3.right * visualMap.runtimeRangeMaxHeight;
                        p1 = p0 + Vector3.up * halfWid;
                        p2 = p0 + Vector3.up * (halfWid + triangeLen);
                        p3 = p2 + Vector3.right * triangeLen;
                        color = visualMap.GetColor(visualMap.rangeMax);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                    }
                    break;

                case Orient.Vertical:
                    pos1 = centerPos + Vector3.down * halfHig;
                    pos2 = centerPos + Vector3.up * halfHig;
                    dir = Vector3.up;
                    xRadius = halfWid;
                    yRadius = splitWid / 2;
                    isVertical = true;
                    if (visualMap.calculable)
                    {
                        var p0 = pos1 + Vector3.up * visualMap.runtimeRangeMinHeight;
                        var p1 = p0 + Vector3.right * halfWid;
                        var p2 = p0 + Vector3.right * (halfWid + triangeLen);
                        var p3 = p2 + Vector3.down * triangeLen;
                        var color = visualMap.GetColor(visualMap.rangeMin);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                        p0 = pos1 + Vector3.up * visualMap.runtimeRangeMaxHeight;
                        p1 = p0 + Vector3.right * halfWid;
                        p2 = p0 + Vector3.right * (halfWid + triangeLen);
                        p3 = p2 + Vector3.up * triangeLen;
                        color = visualMap.GetColor(visualMap.rangeMax);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                    }
                    break;
            }
            if (visualMap.calculable &&
                (visualMap.rangeMin > visualMap.min || visualMap.rangeMax < visualMap.max))
            {
                var rangeMin = visualMap.rangeMin;
                var rangeMax = visualMap.rangeMax;
                var diff = (visualMap.max - visualMap.min) / (splitNum - 1);
                for (int i = 1; i < splitNum; i++)
                {
                    var splitMin = visualMap.min + (i - 1) * diff;
                    var splitMax = splitMin + diff;
                    if (rangeMin > splitMax || rangeMax < splitMin)
                    {
                        continue;
                    }
                    else if (rangeMin <= splitMin && rangeMax >= splitMax)
                    {
                        var splitPos = pos1 + dir * (i - 1 + 0.5f) * splitWid;
                        var startColor = colors[i - 1].color;
                        var toColor = visualMap.IsPiecewise() ? startColor : colors[i].color;
                        UGL.DrawRectangle(vh, splitPos, xRadius, yRadius, startColor, toColor, isVertical);
                    }
                    else if (rangeMin > splitMin && rangeMax >= splitMax)
                    {
                        var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                        var splitMaxPos = pos1 + dir * i * splitWid;
                        var splitPos = p0 + (splitMaxPos - p0) / 2;
                        var startColor = visualMap.GetColor(visualMap.rangeMin);
                        var toColor = visualMap.IsPiecewise() ? startColor : colors[i].color;
                        var yRadius1 = Vector3.Distance(p0, splitMaxPos) / 2;

                        if (visualMap.orient == Orient.Vertical)
                            UGL.DrawRectangle(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            UGL.DrawRectangle(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                    else if (rangeMax < splitMax && rangeMin <= splitMin)
                    {
                        var p0 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                        var splitMinPos = pos1 + dir * (i - 1) * splitWid;
                        var splitPos = splitMinPos + (p0 - splitMinPos) / 2;
                        var startColor = colors[i - 1].color;
                        var toColor = visualMap.IsPiecewise() ? startColor : visualMap.GetColor(visualMap.rangeMax);
                        var yRadius1 = Vector3.Distance(p0, splitMinPos) / 2;

                        if (visualMap.orient == Orient.Vertical)
                            UGL.DrawRectangle(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            UGL.DrawRectangle(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                    else
                    {
                        var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                        var p1 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                        var splitPos = (p0 + p1) / 2;
                        var startColor = visualMap.GetColor(visualMap.rangeMin);
                        var toColor = visualMap.GetColor(visualMap.rangeMax);
                        var yRadius1 = Vector3.Distance(p0, p1) / 2;

                        if (visualMap.orient == Orient.Vertical)
                            UGL.DrawRectangle(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            UGL.DrawRectangle(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                }
            }
            else
            {
                for (int i = 1; i < splitNum; i++)
                {
                    var splitPos = pos1 + dir * (i - 1 + 0.5f) * splitWid;
                    var startColor = colors[i - 1].color;
                    var toColor = visualMap.IsPiecewise() ? startColor : colors[i].color;
                    UGL.DrawRectangle(vh, splitPos, xRadius, yRadius, startColor, toColor, isVertical);
                }
            }

            if (visualMap.rangeMin > visualMap.min)
            {
                var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                UGL.DrawRectangle(vh, pos1, p0, visualMap.itemWidth / 2, chart.theme.visualMap.backgroundColor);
            }
            if (visualMap.rangeMax < visualMap.max)
            {
                var p1 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                UGL.DrawRectangle(vh, p1, pos2, visualMap.itemWidth / 2, chart.theme.visualMap.backgroundColor);
            }

            if (visualMap.hoverLink)
            {
                if (visualMap.context.pointerIndex >= 0)
                {
                    var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                    var p1 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                    var pointerPos = chart.pointerPos;

                    if (visualMap.orient == Orient.Vertical)
                    {
                        var p2 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y + (triangeLen / 2), p0.y, p1.y));
                        var p3 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y - (triangeLen / 2), p0.y, p1.y));
                        var p4 = new Vector3(centerPos.x + halfWid + triangeLen / 2, pointerPos.y);
                        UGL.DrawTriangle(vh, p2, p3, p4, colors[visualMap.context.pointerIndex].color);
                    }
                    else
                    {
                        var p2 = new Vector3(Mathf.Clamp(pointerPos.x + (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                        var p3 = new Vector3(Mathf.Clamp(pointerPos.x - (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                        var p4 = new Vector3(pointerPos.x, centerPos.y + halfWid + triangeLen / 2);
                        UGL.DrawTriangle(vh, p2, p3, p4, colors[visualMap.context.pointerIndex].color);
                    }
                }
            }
        }

        private void DrawPiecewiseVisualMap(VertexHelper vh, VisualMap visualMap)
        {
            var centerPos = chart.chartPosition + visualMap.location.GetPosition(chart.chartWidth, chart.chartHeight);
            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var dir = Vector3.zero;
            var halfWid = visualMap.itemWidth / 2;
            var halfHig = visualMap.itemHeight / 2;

            switch (visualMap.orient)
            {
                case Orient.Horizonal:
                    for (int i = 0; i < visualMap.inRange.Count; i++)
                    {
                        var piece = visualMap.inRange[i];
                    }
                    break;

                case Orient.Vertical:
                    var each = visualMap.itemHeight + visualMap.itemGap;
                    for (int i = 0; i < visualMap.inRange.Count; i++)
                    {
                        var piece = visualMap.inRange[i];
                        var pos = new Vector3(centerPos.x, centerPos.y - each * i);
                        UGL.DrawRectangle(vh, pos, halfWid, halfHig, piece.color);
                    }
                    break;
            }
        }

        private void OnDragVisualMapStart(VisualMap visualMap)
        {
            if (!visualMap.show || !visualMap.showUI || !visualMap.calculable)
                return;

            var inMinRect = visualMap.IsInRangeMinRect(chart.pointerPos, chart.chartRect, chart.theme.visualMap.triangeLen);
            var inMaxRect = visualMap.IsInRangeMaxRect(chart.pointerPos, chart.chartRect, chart.theme.visualMap.triangeLen);

            if (inMinRect || inMaxRect)
            {
                if (inMinRect)
                {
                    visualMap.context.minDrag = true;
                }
                else
                {
                    visualMap.context.maxDrag = true;
                }
            }
        }

        private void OnDragVisualMap(VisualMap visualMap)
        {
            if (!visualMap.show || !visualMap.showUI || !visualMap.calculable)
                return;

            if (!visualMap.context.minDrag && !visualMap.context.maxDrag)
                return;

            var value = visualMap.GetValue(chart.pointerPos, chart.chartRect);
            if (visualMap.context.minDrag)
            {
                visualMap.rangeMin = value;
            }
            else
            {
                visualMap.rangeMax = value;
            }
            chart.RefreshChart();
        }

        private void OnDragVisualMapEnd(VisualMap visualMap)
        {
            if (!visualMap.show || !visualMap.showUI || !visualMap.calculable)
                return;

            if (visualMap.context.minDrag || visualMap.context.maxDrag)
            {
                chart.RefreshChart();
                visualMap.context.minDrag = false;
                visualMap.context.maxDrag = false;
            }
        }
    }
}