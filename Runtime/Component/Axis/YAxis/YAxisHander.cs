using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class YAxisHander : AxisHandler<YAxis>
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

        private void UpdatePosition(YAxis axis)
        {
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            if (grid != null)
            {
                var relativedAxis = chart.GetChartComponent<XAxis>(axis.gridIndex);
                axis.context.x = AxisHelper.GetYAxisXOrY(grid, axis, relativedAxis);
                axis.context.y = grid.context.y;
                axis.context.zeroX = axis.context.x;
                axis.context.zeroY = axis.context.y + axis.context.offset;
            }
        }

        private void InitYAxis(YAxis yAxis)
        {
            var theme = chart.theme;
            var yAxisIndex = yAxis.index;
            yAxis.painter = chart.painter;
            yAxis.refreshComponent = delegate()
            {
                var grid = chart.GetChartComponent<GridCoord>(yAxis.gridIndex);
                if (grid != null)
                {
                    var xAxis = chart.GetChartComponent<YAxis>(yAxis.index);
                    InitAxis(xAxis,
                        orient,
                        grid.context.x,
                        grid.context.y,
                        grid.context.height,
                        grid.context.width);
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
            var grid = chart.GetChartComponent<GridCoord>(component.gridIndex);
            if (grid == null)
                return Vector3.zero;

            var xAxis = chart.GetChartComponent<XAxis>(component.index);
            return GetLabelPosition(i, Orient.Vertical, component, xAxis,
                chart.theme.axis,
                scaleWid,
                grid.context.x,
                grid.context.y,
                grid.context.height,
                grid.context.width);
        }

        private void DrawYAxisSplit(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = chart.GetChartComponent<GridCoord>(yAxis.gridIndex);
                if (grid == null)
                    return;
                var relativedAxis = chart.GetChartComponent<XAxis>(yAxis.gridIndex);
                var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                DrawAxisSplit(vh, chart.theme.axis, dataZoom,
                    Orient.Vertical,
                    grid.context.x,
                    grid.context.y,
                    grid.context.height,
                    grid.context.width,
                    relativedAxis);
            }
        }

        private void DrawYAxisTick(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (AxisHelper.NeedShowSplit(yAxis))
            {
                var grid = chart.GetChartComponent<GridCoord>(yAxis.gridIndex);
                if (grid == null)
                    return;

                var dataZoom = chart.GetDataZoomOfAxis(yAxis);

                DrawAxisTick(vh, yAxis, chart.theme.axis, dataZoom,
                    Orient.Vertical,
                    GetAxisLineXOrY(),
                    grid.context.y,
                    grid.context.height);
            }
        }

        private void DrawYAxisLine(VertexHelper vh, int yAxisIndex, YAxis yAxis)
        {
            if (yAxis.show && yAxis.axisLine.show)
            {
                var grid = chart.GetChartComponent<GridCoord>(yAxis.gridIndex);
                if (grid == null)
                    return;

                DrawAxisLine(vh, yAxis, chart.theme.axis,
                    Orient.Vertical,
                    GetAxisLineXOrY(),
                    grid.context.y,
                    grid.context.height);
            }
        }

        internal override float GetAxisLineXOrY()
        {
            return component.context.x;
        }
    }
}