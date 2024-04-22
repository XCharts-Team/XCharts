using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class XAxisHander : AxisHandler<XAxis>
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

        private void UpdatePosition(XAxis axis)
        {
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            if (grid != null)
            {
                var relativedAxis = chart.GetChartComponent<YAxis>(axis.gridIndex);
                axis.context.x = grid.context.x;
                axis.context.y = AxisHelper.GetXAxisXOrY(grid, axis, relativedAxis);
                axis.context.start = new Vector3(grid.context.x, axis.context.y);
                axis.context.end = new Vector3(grid.context.x + grid.context.width, axis.context.y);
                var vec = axis.context.end - axis.context.start;
                axis.context.dire = vec.normalized;
                axis.context.length = vec.magnitude;
                axis.context.zeroY = grid.context.y;
                axis.context.zeroX = grid.context.x + axis.context.offset;
            }
        }

        private void InitXAxis(XAxis xAxis)
        {
            var theme = chart.theme;
            var xAxisIndex = xAxis.index;
            xAxis.painter = chart.painter;
            xAxis.refreshComponent = delegate()
            {
                var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
                if (grid != null)
                {
                    var yAxis = chart.GetChartComponent<YAxis>(xAxis.index);
                    InitAxis(yAxis,
                        orient,
                        grid.context.x,
                        grid.context.y,
                        grid.context.width,
                        grid.context.height);
                }
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
            var grid = chart.GetChartComponent<GridCoord>(component.gridIndex);
            if (grid == null)
                return Vector3.zero;

            var yAxis = chart.GetChartComponent<YAxis>(component.index);
            return GetLabelPosition(i, Orient.Horizonal, component, yAxis,
                chart.theme.axis,
                scaleWid,
                grid.context.x,
                grid.context.y,
                grid.context.width,
                grid.context.height);
        }

        private void DrawXAxisSplit(VertexHelper vh, XAxis xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
                if (grid == null)
                    return;

                var relativedAxis = chart.GetChartComponent<YAxis>(xAxis.gridIndex);
                var dataZoom = chart.GetDataZoomOfAxis(xAxis);

                DrawAxisSplit(vh, chart.theme.axis, dataZoom,
                    Orient.Horizonal,
                    grid.context.x,
                    grid.context.y,
                    grid.context.width,
                    grid.context.height,
                    relativedAxis);
            }
        }

        private void DrawXAxisTick(VertexHelper vh, XAxis xAxis)
        {
            if (AxisHelper.NeedShowSplit(xAxis))
            {
                var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
                if (grid == null)
                    return;

                var dataZoom = chart.GetDataZoomOfAxis(xAxis);

                DrawAxisTick(vh, xAxis, chart.theme.axis, dataZoom,
                    Orient.Horizonal,
                    grid.context.x,
                    GetAxisLineXOrY(),
                    grid.context.width);
            }
        }

        private void DrawXAxisLine(VertexHelper vh, XAxis xAxis)
        {
            if (xAxis.show && xAxis.axisLine.show)
            {
                var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
                if (grid == null)
                    return;

                DrawAxisLine(vh, xAxis, chart.theme.axis,
                    Orient.Horizonal,
                    grid.context.x,
                    GetAxisLineXOrY(),
                    grid.context.width);
            }
        }

        internal override float GetAxisLineXOrY()
        {
            return component.context.y;
        }
    }
}