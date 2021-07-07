/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static class DataZoomHelper
    {
        public static DataZoom GetAxisRelatedDataZoom(Axis axis, List<DataZoom> dataZooms)
        {
            foreach (var dataZoom in dataZooms)
            {
                if (!dataZoom.enable) continue;
                if (dataZoom.IsContainsAxis(axis)) return dataZoom;
            }
            return null;
        }
        public static void GetSerieRelatedDataZoom(Serie serie, List<DataZoom> dataZooms,
            out DataZoom xDataZoom, out DataZoom yDataZoom)
        {
            xDataZoom = null;
            yDataZoom = null;
            if (serie == null) return;
            foreach (var dataZoom in dataZooms)
            {
                if (!dataZoom.enable) continue;
                if (dataZoom.IsContainsXAxis(serie.xAxisIndex))
                {
                    xDataZoom = dataZoom;
                }
                if (dataZoom.IsContainsYAxis(serie.yAxisIndex))
                {
                    yDataZoom = dataZoom;
                }
            }
        }

        public static void UpdateDataZoomRuntimeStartEndValue(DataZoom dataZoom, Serie serie)
        {
            if (dataZoom == null || serie == null) return;
            double min = 0;
            double max = 0;
            SerieHelper.GetMinMaxData(serie, out min, out max, null);
            dataZoom.runtimeStartValue = min + (max - min) * dataZoom.start / 100;
            dataZoom.runtimeEndValue = min + (max - min) * dataZoom.end / 100;
        }

        public static void UpdateDataZoomRuntimeStartEndValue(CoordinateChart chart, SerieType serieType)
        {
            foreach (var dataZoom in chart.dataZooms)
            {
                if (!dataZoom.enable) continue;
                double min = double.MaxValue;
                double max = double.MinValue;
                foreach (var serie in chart.series.list)
                {
                    if (!serie.show || serie.type != serieType) continue;
                    if (!dataZoom.IsContainsXAxis(serie.xAxisIndex)) continue;
                    var axis = chart.GetXAxis(serie.xAxisIndex);
                    
                    if (axis.minMaxType == Axis.AxisMinMaxType.Custom)
                    {
                        if (axis.min < min) min = axis.min;
                        if (axis.max > max) max = axis.max;
                    }
                    else
                    {
                        double serieMinValue = 0;
                        double serieMaxValue = 0;
                        SerieHelper.GetMinMaxData(serie, out serieMinValue, out serieMaxValue, null, 2);
                        if (serieMinValue < min) min = serieMinValue;
                        if (serieMaxValue > max) max = serieMaxValue;
                    }
                }
                dataZoom.runtimeStartValue = min + (max - min) * dataZoom.start / 100;
                dataZoom.runtimeEndValue = min + (max - min) * dataZoom.end / 100;
            }
        }
    }
}