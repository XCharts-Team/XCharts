
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/GanttChart", 22)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public partial class GanttChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            title.text = "GanttChart";
            var xCount = 5;
            var yCount = 5;

            m_Grids[0].left = 60;
            m_Grids[0].right = 50;
            m_XAxes[0].type = Axis.AxisType.Time;
            m_XAxes[0].boundaryGap = false;
            m_XAxes[0].splitNumber = xCount;
            m_YAxes[0].type = Axis.AxisType.Category;
            m_YAxes[0].boundaryGap = true;
            m_YAxes[0].splitNumber = 0;

            RemoveData();
            SerieTemplate.AddDefaultTimeGanttSerie(this, "task", yCount);
        }
#endif
        protected override void GetSeriesMinMaxValue(Axis axis, int axisIndex, out float tempMinValue, out float tempMaxValue)
        {
            tempMinValue = int.MaxValue;
            tempMaxValue = int.MinValue;
            foreach (var serie in m_Series.list)
            {
                if (serie.type != SerieType.Gantt) continue;
                if (serie.xAxisIndex != axis.index) continue;
                foreach (var serieData in serie.data)
                {
                    if (serieData.data.Count >= 2)
                    {
                        var xData = serieData.data[0];
                        var yData = serieData.data[1];
                        if (xData < tempMinValue) tempMinValue = xData;
                        if (yData > tempMaxValue) tempMaxValue = yData;
                    }
                }
            }
            if (tempMinValue == int.MaxValue) tempMinValue = 0;
            if (tempMaxValue == int.MinValue) tempMaxValue = 0;
            //AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true, 60);
        }

        protected override void OnRefreshLabel()
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (serie.IsPerformanceMode()) continue;
                if (serie.type != SerieType.Gantt) continue;
                foreach (var serieData in serie.data)
                {
                    if (serieData.labelObject == null) continue;
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                    var labelShow = serie.show && serieLabel.show;
                    serieData.SetLabelActive(labelShow);
                    if (labelShow)
                    {
                        var labelColor = serieLabel.textStyle.GetColor(m_Theme.axis.textColor);
                        var labelPos = serieData.runtimePosition;
                        SerieLabelHelper.ResetLabel(serieData.labelObject.label, serieLabel, m_Theme, i);
                        serieData.labelObject.SetPosition(labelPos);
                        serieData.labelObject.SetLabelColor(labelColor);
                        serieData.labelObject.SetText(serieData.name);
                    }
                }
            }
        }

        protected override void UpdateTooltipValue(Vector2 local)
        {
            var grid = GetGrid(tooltip.runtimeGridIndex);
            if (grid == null) return;
            tooltip.runtimeDataIndex.Clear();
            foreach (var serie in m_Series.list)
            {
                var serieGrid = GetSerieGridOrDefault(serie);
                if (grid.index != serieGrid.index) continue;
                for (int i = 0; i < serie.data.Count; i++)
                {
                    var serieData = serie.GetSerieData(i);
                    var highlight = serieData.runtimeRect.Contains(local);
                    serieData.highlighted = highlight;
                    if (highlight)
                    {

                        tooltip.runtimeDataIndex.Add(serie.index);
                        tooltip.runtimeDataIndex.Add(i);
                        return;
                    }
                }
            }
        }

        protected override void UpdateTooltip()
        {
            if (tooltip.runtimeDataIndex.Count == 0)
            {
                if (tooltip.IsActive())
                {
                    tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            var serieIndex = tooltip.runtimeDataIndex[0];
            var dataIndex = tooltip.runtimeDataIndex[1];
            var serie = m_Series.GetSerie(serieIndex);
            if (serie == null) return;
            var serieData = serie.GetSerieData(dataIndex);
            var category = serieData == null ? serie.name : serieData.name;
            TooltipHelper.SetContentAndPosition(tooltip, category, chartRect);
            tooltip.SetActive(true);
        }
    }
}
