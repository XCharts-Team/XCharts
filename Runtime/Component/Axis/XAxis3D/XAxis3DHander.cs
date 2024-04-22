using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class XAxis3DHander : AxisHandler<XAxis3D>
    {
        protected override Orient orient { get { return Orient.Horizonal; } }

        public override void InitComponent()
        {
            InitXAxis(component);
        }

        public override void Update()
        {
            UpdateAxisMinMaxValue(component.index, component);
            if (!chart.isTriggerOnClick)
            {
                UpdatePointerValue(component);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (chart.isTriggerOnClick)
            {
                UpdatePointerValue(component);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (chart.isTriggerOnClick)
            {
                component.context.pointerValue = double.PositiveInfinity;
            }
        }

        public override void DrawBase(VertexHelper vh)
        {
            UpdatePosition(component);
            DrawXAxisSplit(vh, component);
            DrawXAxisLine(vh, component);
            DrawXAxisTick(vh, component);
        }

        private void UpdatePosition(XAxis3D axis)
        {
            var grid = chart.GetChartComponent<GridCoord3D>(axis.gridIndex);
            if (grid != null)
            {
                if (axis.position == Axis.AxisPosition.Right || axis.position == Axis.AxisPosition.Top)
                {
                    axis.context.start = grid.xyExchanged ? grid.context.pointD : grid.context.pointB;
                    axis.context.end = grid.context.pointC;
                }
                else
                {
                    axis.context.start = grid.context.pointA;
                    axis.context.end = grid.xyExchanged ? grid.context.pointB : grid.context.pointD;
                }
                var vect = axis.context.end - axis.context.start;
                axis.context.x = axis.context.start.x;
                axis.context.y = axis.context.start.y;
                axis.context.dire = vect.normalized;
                axis.context.length = vect.magnitude;
            }
        }

        private void InitXAxis(XAxis3D xAxis)
        {
            var theme = chart.theme;
            var xAxisIndex = xAxis.index;
            xAxis.painter = chart.painter;
            xAxis.refreshComponent = delegate ()
            {
                var yAxis = chart.GetChartComponent<YAxis3D>(xAxis.index);
                InitAxis3D(yAxis, orient);
            };
            xAxis.refreshComponent();
        }

        internal override void UpdateAxisLabelText(Axis axis)
        {
            base.UpdateAxisLabelText(axis);
            if (axis.IsTime() || axis.IsValue())
            {
                for (int i = 0; i < axis.context.labelObjectList.Count; i++)
                {
                    var label = axis.context.labelObjectList[i];
                    if (label != null)
                    {
                        var pos = GetLabelPosition(0, i);
                        label.SetPosition(pos);
                        CheckValueLabelActive(component, i, label, pos);
                    }
                }
            }
        }

        protected override Vector3 GetLabelPosition(float scaleWid, int i)
        {
            var yAxis = chart.GetChartComponent<YAxis3D>(component.index);
            return Axis3DHelper.GetLabelPosition(i, component, yAxis, chart.theme.axis, scaleWid);
        }

        private void DrawXAxisSplit(VertexHelper vh, XAxis3D xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var grid = chart.GetChartComponent<GridCoord3D>(xAxis.gridIndex);
                var relativedAxis = chart.GetChartComponent<YAxis3D>(xAxis.gridIndex);
                var dataZoom = chart.GetDataZoomOfAxis(xAxis);
                var isLeft = grid.IsLeft();
                if (grid.xyExchanged)
                {
                    Axis3DHelper.DrawAxisSplit(vh, xAxis, chart.theme.axis, dataZoom,
                        grid.context.pointA,
                        grid.context.pointB,
                        relativedAxis);
                    if (xAxis.splitLine.showZLine)
                    {
                        var relativedAxis2 = chart.GetChartComponent<ZAxis3D>(xAxis.gridIndex);
                        Axis3DHelper.DrawAxisSplit(vh, xAxis, chart.theme.axis, dataZoom,
                            isLeft ? grid.context.pointD : grid.context.pointA,
                            isLeft ? grid.context.pointC : grid.context.pointB,
                            relativedAxis2);
                    }
                }
                else
                {
                    Axis3DHelper.DrawAxisSplit(vh, xAxis, chart.theme.axis, dataZoom,
                        grid.context.pointA,
                        grid.context.pointD,
                        relativedAxis);
                    if (xAxis.splitLine.showZLine)
                    {
                        var relativedAxis2 = chart.GetChartComponent<ZAxis3D>(xAxis.gridIndex);
                        Axis3DHelper.DrawAxisSplit(vh, xAxis, chart.theme.axis, dataZoom,
                            grid.context.pointB,
                            grid.context.pointC,
                            relativedAxis2);
                    }
                }
            }
        }

        private void DrawXAxisTick(VertexHelper vh, XAxis3D xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var grid = chart.GetChartComponent<GridCoord3D>(xAxis.gridIndex);
                if (grid == null)
                    return;

                var dataZoom = chart.GetDataZoomOfAxis(xAxis);
                var relativedAxis = chart.GetChartComponent<YAxis3D>(xAxis.gridIndex);
                Axis3DHelper.DrawAxisTick(vh, xAxis, chart.theme.axis, dataZoom,
                    xAxis.context.start,
                    xAxis.context.end,
                    -relativedAxis.context.dire);
            }
        }

        private void DrawXAxisLine(VertexHelper vh, XAxis3D axis)
        {
            if (axis.show && axis.axisLine.show)
            {
                var theme = chart.theme.axis;
                var lineWidth = axis.axisLine.GetWidth(theme.lineWidth);
                var lineType = axis.axisLine.GetType(theme.lineType);
                var lineColor = axis.axisLine.GetColor(theme.lineColor);

                var start = axis.context.start;
                var end = axis.context.end;
                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, start, end, lineColor);
            }
        }

        internal override float GetAxisLineXOrY()
        {
            return component.context.y;
        }
    }
}