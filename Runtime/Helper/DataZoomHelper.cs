/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/
using System.Collections.Generic;

namespace XCharts
{
    public static class DataZoomHelper
    {
        public static DataZoom GetDataZoom(Serie serie, List<DataZoom> dataZooms)
        {
            if(serie == null) return null;
            foreach (var dataZoom in dataZooms)
            {
                if (!dataZoom.enable) continue;
                if (dataZoom.IsContainsXAxis(serie.xAxisIndex)
                    || dataZoom.IsContainsYAxis(serie.yAxisIndex))
                {
                    return dataZoom;
                }
            }
            return null;
        }

        public static void UpdateDataZoomRuntimeStartEndValue(DataZoom dataZoom, Serie serie){
            if(dataZoom == null || serie == null) return;
            float min = 0;
            float max = 0;
            SerieHelper.GetMinMaxData(serie, out min, out max, null);
            dataZoom.runtimeStartValue = min + (max-min) * dataZoom.start / 100;
            dataZoom.runtimeEndValue = min +(max-min) * dataZoom.end / 100;
        }
    }
}