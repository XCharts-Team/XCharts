using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class ZAxis3DHander : AxisHandler<ZAxis3D>
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
            DrawZAxisSplit(vh, component.index, component);
            DrawZAxisLine(vh, component.index, component);
            DrawZAxisTick(vh, component.index, component);
        }

        private void UpdatePosition(ZAxis3D axis)
        {
            var grid = chart.GetChartComponent<GridCoord3D>(axis.gridIndex);
            if (grid != null)
            {
                if (grid.context.pointB.x < grid.context.pointA.x)
                {
                    axis.context.start = grid.context.pointD;
                    axis.context.end = grid.context.pointH;
                }
                else if (axis.position == Axis.AxisPosition.Center)
                {
                    axis.context.start = grid.context.pointB;
                    axis.context.end = grid.context.pointF;
                }
                else if (axis.position == Axis.AxisPosition.Right)
                {
                    axis.context.start = grid.context.pointC;
                    axis.context.end = grid.context.pointG;
                }
                else
                {
                    axis.context.start = grid.context.pointA;
                    axis.context.end = grid.context.pointE;
                }
                axis.context.x = axis.context.start.x;
                axis.context.y = axis.context.start.y;
                var vect = axis.context.end - axis.context.start;
                axis.context.dire = vect.normalized;
                axis.context.length = vect.magnitude;
            }
        }

        private void InitYAxis(ZAxis3D yAxis)
        {
            var theme = chart.theme;
            var yAxisIndex = yAxis.index;
            yAxis.painter = chart.painter;
            yAxis.refreshComponent = delegate ()
            {
                var grid = chart.GetChartComponent<GridCoord3D>(yAxis.gridIndex);
                if (grid != null)
                {
                    var relativedAxis = chart.GetChartComponent<ZAxis3D>(yAxis.index);
                    InitAxis3D(relativedAxis, orient);
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
            var grid = chart.GetChartComponent<GridCoord3D>(component.gridIndex);
            if (grid == null)
                return Vector3.zero;

            var yAxis = chart.GetChartComponent<XAxis3D>(component.index);
            return Axis3DHelper.GetLabelPosition(i, component, yAxis,
                chart.theme.axis,
                scaleWid);
        }

        private void DrawZAxisSplit(VertexHelper vh, int yAxisIndex, ZAxis3D yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = chart.GetChartComponent<GridCoord3D>(yAxis.gridIndex);
                if (grid == null)
                    return;

                var isLeft = grid.IsLeft();
                if (grid.xyExchanged)
                {
                    var relativedAxis = chart.GetChartComponent<XAxis3D>(yAxis.gridIndex);
                    var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                    Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                        isLeft ? grid.context.pointD : grid.context.pointA,
                        isLeft ? grid.context.pointH : grid.context.pointE,
                        relativedAxis);
                    if (yAxis.splitLine.showZLine)
                    {
                        var relativedAxis2 = chart.GetChartComponent<YAxis3D>(yAxis.gridIndex);
                        Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                            grid.context.pointB,
                            grid.context.pointF,
                            relativedAxis2);
                    }
                }
                else
                {
                    var relativedAxis = chart.GetChartComponent<YAxis3D>(yAxis.gridIndex);
                    var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                    Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                        isLeft ? grid.context.pointD : grid.context.pointA,
                        isLeft ? grid.context.pointH : grid.context.pointE,
                        relativedAxis);
                    if (yAxis.splitLine.showZLine)
                    {
                        var relativedAxis2 = chart.GetChartComponent<XAxis3D>(yAxis.gridIndex);
                        Axis3DHelper.DrawAxisSplit(vh, yAxis, chart.theme.axis, dataZoom,
                            grid.context.pointB,
                            grid.context.pointF,
                            relativedAxis2);
                    }
                }
            }
        }

        private void DrawZAxisTick(VertexHelper vh, int yAxisIndex, ZAxis3D zAxis)
        {
            if (AxisHelper.NeedShowSplit(zAxis))
            {
                var grid = chart.GetChartComponent<GridCoord3D>(zAxis.gridIndex);
                if (grid == null)
                    return;

                var dataZoom = chart.GetDataZoomOfAxis(zAxis);
                var relativedDire = grid.context.pointA - grid.context.pointB;
                Axis3DHelper.DrawAxisTick(vh, zAxis, chart.theme.axis, dataZoom,
                    zAxis.context.start,
                    zAxis.context.end,
                    relativedDire.normalized);
            }
        }

        private void DrawZAxisLine(VertexHelper vh, int axisIndex, ZAxis3D axis)
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