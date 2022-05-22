namespace XCharts.Runtime
{
    public static class DataZoomHelper
    {
        public static void UpdateDataZoomRuntimeStartEndValue(DataZoom dataZoom, Serie serie)
        {
            if (dataZoom == null || serie == null)
                return;

            double min = 0;
            double max = 0;
            SerieHelper.GetMinMaxData(serie, out min, out max, null);
            dataZoom.context.startValue = min + (max - min) * dataZoom.start / 100;
            dataZoom.context.endValue = min + (max - min) * dataZoom.end / 100;
        }

        public static void UpdateDataZoomRuntimeStartEndValue<T>(BaseChart chart) where T : Serie
        {
            foreach (var component in chart.components)
            {
                if (component is DataZoom)
                {
                    var dataZoom = component as DataZoom;
                    if (!dataZoom.enable)
                        continue;

                    double min = double.MaxValue;
                    double max = double.MinValue;
                    foreach (var serie in chart.series)
                    {
                        if (!serie.show || !(serie is T))
                            continue;
                        if (!dataZoom.IsContainsXAxis(serie.xAxisIndex))
                            continue;

                        var axis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);

                        if (axis.minMaxType == Axis.AxisMinMaxType.Custom)
                        {
                            if (axis.min < min)
                                min = axis.min;
                            if (axis.max > max)
                                max = axis.max;
                        }
                        else
                        {
                            double serieMinValue = 0;
                            double serieMaxValue = 0;
                            SerieHelper.GetMinMaxData(serie, out serieMinValue, out serieMaxValue, null, 2);
                            if (serieMinValue < min)
                                min = serieMinValue;
                            if (serieMaxValue > max)
                                max = serieMaxValue;
                        }
                    }
                    dataZoom.context.startValue = min + (max - min) * dataZoom.start / 100;
                    dataZoom.context.endValue = min + (max - min) * dataZoom.end / 100;
                }
            }
        }
    }
}