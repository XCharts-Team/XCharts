using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class YAxis3DHander : AxisHandler<YAxis3D>
    {
        protected override Orient orient { get { return Orient.Vertical; } }

        public override void InitComponent()
        {
            InitYAxis(component);
        }

        public override void Update()
        {
            UpdateAxisMinMaxValue(component.index, component);
            UpdatePointerValue(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            UpdatePosition(component);
            DrawYAxisSplit(vh, component.index, component);
            DrawYAxisLine(vh, component.index, component);
            DrawYAxisTick(vh, component.index, component);
        }

        private void UpdatePosition(YAxis3D axis)
        {
            var grid = chart.GetChartComponent<GridCoord3D>(axis.gridIndex);
            if (grid != null)
            {
                if (axis.position == Axis.AxisPosition.Right)
                {
                    axis.context.start = grid.xyExchanged ? grid.context.pointB : grid.context.pointD;
                    axis.context.end = grid.context.pointC;
                }
                else
                {
                    axis.context.start = grid.context.pointA;
                    axis.context.end = grid.xyExchanged ? grid.context.pointD : grid.context.pointB;
                }
                axis.context.x = axis.context.start.x;
                axis.context.y = axis.context.start.y;
                var vect = axis.context.end - axis.context.start;
                axis.context.dire = vect.normalized;
                axis.context.length = vect.magnitude;
            }
        }

        private void InitYAxis(YAxis3D yAxis)
        {
            var theme = chart.theme;
            var yAxisIndex = yAxis.index;
            yAxis.painter = chart.painter;
            yAxis.refreshComponent = delegate ()
            {
                var grid = chart.GetChartComponent<GridCoord3D>(yAxis.gridIndex);
                if (grid != null)
                {
                    var xAxis = chart.GetChartComponent<YAxis3D>(yAxis.index);
                    InitAxis3D(xAxis, orient);
                }
            };
            yAxis.refreshComponent();
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
                        CheckValueLabelActive(axis, i, label, pos);
                    }
                }
            }
        }

        protected override Vector3 GetLabelPosition(float scaleWid, int i)
        {
            var xAxis = chart.GetChartComponent<XAxis3D>(component.index);
            return Axis3DHelper.GetLabelPosition(i, component, xAxis, chart.theme.axis, scaleWid);
        }

        private void DrawYAxisSplit(VertexHelper vh, int yAxisIndex, YAxis3D yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = chart.GetChartComponent<GridCoord3D>(yAxis.gridIndex);
                var relativedAxis = chart.GetChartComponent<XAxis3D>(yAxis.gridIndex);
                var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                var isLeft = grid.IsLeft();
                if (grid.xyExchanged)
                {
                    Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                        grid.context.pointA,
                        grid.context.pointD,
                        relativedAxis);
                    if (yAxis.splitLine.showZLine)
                    {
                        var relativedAxis2 = chart.GetChartComponent<ZAxis3D>(yAxis.gridIndex);
                        Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                            grid.context.pointB, grid.context.pointC, relativedAxis2);
                    }
                }
                else
                {
                    Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                        grid.context.pointA,
                        grid.context.pointB,
                        relativedAxis);
                    if (yAxis.splitLine.showZLine)
                    {
                        var relativedAxis2 = chart.GetChartComponent<ZAxis3D>(yAxis.gridIndex);
                        Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                            isLeft ? grid.context.pointD : grid.context.pointA,
                            isLeft ? grid.context.pointC : grid.context.pointB,
                            relativedAxis2);
                    }
                }
            }
        }

        private void DrawYAxisTick(VertexHelper vh, int yAxisIndex, YAxis3D yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = chart.GetChartComponent<GridCoord3D>(yAxis.gridIndex);
                if (grid == null)
                    return;

                var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                var relativedAxis = chart.GetChartComponent<XAxis3D>(yAxis.gridIndex);

                Axis3DHelper.DrawAxisTick(vh, yAxis, chart.theme.axis, dataZoom,
                    yAxis.context.start,
                    yAxis.context.end,
                    -relativedAxis.context.dire);
            }
        }

        private void DrawYAxisLine(VertexHelper vh, int axisIndex, YAxis3D axis)
        {
            if (axis.show && axis.axisLine.show)
            {
                var grid = chart.GetChartComponent<GridCoord3D>(axis.gridIndex);
                if (grid == null)
                    return;

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
            return component.context.x;
        }
    }
}