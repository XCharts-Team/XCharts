using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class ParallelCoordHandler : MainComponentHandler<ParallelCoord>
    {
        private Dictionary<int, double> m_SerieDimMin = new Dictionary<int, double>();
        private Dictionary<int, double> m_SerieDimMax = new Dictionary<int, double>();
        private double m_LastInterval;
        private int m_LastSplitNumber;

        public override void InitComponent()
        {
            var grid = component;
            grid.painter = chart.painter;
            grid.refreshComponent = delegate()
            {
                grid.UpdateRuntimeData(chart);
                chart.OnCoordinateChanged();
            };
            grid.refreshComponent();
        }

        public override void CheckComponent(StringBuilder sb)
        {
            var grid = component;
            if (grid.left >= chart.chartWidth)
                sb.Append("warning:grid->left > chartWidth\n");
            if (grid.right >= chart.chartWidth)
                sb.Append("warning:grid->right > chartWidth\n");
            if (grid.top >= chart.chartHeight)
                sb.Append("warning:grid->top > chartHeight\n");
            if (grid.bottom >= chart.chartHeight)
                sb.Append("warning:grid->bottom > chartHeight\n");
            if (grid.left + grid.right >= chart.chartWidth)
                sb.Append("warning:grid.left + grid.right > chartWidth\n");
            if (grid.top + grid.bottom >= chart.chartHeight)
                sb.Append("warning:grid.top + grid.bottom > chartHeight\n");
        }

        public override void Update()
        {
            UpdatePointerEnter();
            UpdateParallelAxisMinMaxValue();
        }

        public override void DrawBase(VertexHelper vh)
        {
            if (!SeriesHelper.IsAnyClipSerie(chart.series))
            {
                DrawCoord(vh);
            }
        }
        public override void DrawUpper(VertexHelper vh)
        {
            if (SeriesHelper.IsAnyClipSerie(chart.series))
            {
                DrawCoord(vh);
            }
        }

        private void DrawCoord(VertexHelper vh)
        {
            var grid = component;
            if (grid.show && !ChartHelper.IsClearColor(grid.backgroundColor))
            {
                var p1 = new Vector2(grid.context.x, grid.context.y);
                var p2 = new Vector2(grid.context.x, grid.context.y + grid.context.height);
                var p3 = new Vector2(grid.context.x + grid.context.width, grid.context.y + grid.context.height);
                var p4 = new Vector2(grid.context.x + grid.context.width, grid.context.y);
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, grid.backgroundColor);
            }
        }

        private void UpdatePointerEnter()
        {
            if (chart.isPointerInChart)
                component.context.runtimeIsPointerEnter = component.Contains(chart.pointerPos);
            else
                component.context.runtimeIsPointerEnter = false;
        }

        private void UpdateParallelAxisMinMaxValue()
        {
            var list = chart.GetChartComponents<ParallelAxis>();
            if (list.Count != component.context.parallelAxes.Count)
            {
                component.context.parallelAxes.Clear();
                foreach (var com in chart.GetChartComponents<ParallelAxis>())
                {
                    var axis = com as ParallelAxis;
                    if (axis.parallelIndex == component.index)
                        component.context.parallelAxes.Add(axis);
                }
            }
            m_SerieDimMin.Clear();
            m_SerieDimMax.Clear();
            foreach (var serie in chart.series)
            {
                if ((serie is Parallel) && serie.parallelIndex == component.index)
                {
                    foreach (var serieData in serie.data)
                    {
                        for (int i = 0; i < serieData.data.Count; i++)
                        {
                            var value = serieData.data[i];
                            if (!m_SerieDimMin.ContainsKey(i))
                                m_SerieDimMin[i] = value;
                            else if (m_SerieDimMin[i] > value)
                                m_SerieDimMin[i] = value;

                            if (!m_SerieDimMax.ContainsKey(i))
                                m_SerieDimMax[i] = value;
                            else if (m_SerieDimMax[i] < value)
                                m_SerieDimMax[i] = value;
                        }
                    }
                }
            }
            for (int i = 0; i < component.context.parallelAxes.Count; i++)
            {
                var axis = component.context.parallelAxes[i];
                if (axis.IsCategory())
                {
                    m_SerieDimMax[i] = axis.data.Count > 0 ? axis.data.Count - 1 : 0;
                    m_SerieDimMin[i] = 0;
                }
                else if (axis.minMaxType == Axis.AxisMinMaxType.Custom)
                {
                    m_SerieDimMin[i] = axis.min;
                    m_SerieDimMax[i] = axis.max;
                }
                else if (m_SerieDimMax.ContainsKey(i))
                {

                    var tempMinValue = m_SerieDimMin[i];
                    var tempMaxValue = m_SerieDimMax[i];
                    AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
                    m_SerieDimMin[i] = tempMinValue;
                    m_SerieDimMax[i] = tempMaxValue;
                }
            }
            for (int i = 0; i < component.context.parallelAxes.Count; i++)
            {
                if (m_SerieDimMax.ContainsKey(i))
                {
                    var axis = component.context.parallelAxes[i];
                    var tempMinValue = m_SerieDimMin[i];
                    var tempMaxValue = m_SerieDimMax[i];

                    if (tempMinValue != axis.context.minValue ||
                        tempMaxValue != axis.context.maxValue ||
                        m_LastInterval != axis.interval ||
                        m_LastSplitNumber != axis.splitNumber)
                    {
                        m_LastSplitNumber = axis.splitNumber;
                        m_LastInterval = axis.interval;

                        axis.UpdateMinMaxValue(tempMinValue, tempMaxValue);
                        axis.context.offset = 0;
                        axis.context.lastCheckInverse = axis.inverse;

                        (axis.handler as ParallelAxisHander).UpdateAxisTickValueList(axis);
                        (axis.handler as ParallelAxisHander).UpdateAxisLabelText(axis);
                        chart.RefreshChart();
                    }
                }
            }
        }
    }
}