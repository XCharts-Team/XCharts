/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using UnityEngine;

namespace XCharts
{
    public static partial class SerieHelper
    {
        /// <summary>
        /// Gets the maximum and minimum values of the specified dimension of a serie.
        /// 获得系列指定维数的最大最小值。
        /// </summary>
        /// <param name="serie">指定系列</param>
        /// <param name="dimension">指定维数</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="dataZoom">缩放组件，默认null</param>
        public static void GetMinMaxData(Serie serie, int dimension, out float min, out float max,
            DataZoom dataZoom = null)
        {
            max = float.MinValue;
            min = float.MaxValue;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value > max) max = value;
                    if (value < min) min = value;
                }
            }
        }

        /// <summary>
        /// Gets the maximum and minimum values of all data in the serie.
        /// 获得系列所有数据的最大最小值。
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="dataZoom"></param>
        public static void GetMinMaxData(Serie serie, out float min, out float max, DataZoom dataZoom = null)
        {
            max = float.MinValue;
            min = float.MaxValue;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show)
                {
                    var count = serie.showDataDimension > serieData.data.Count
                        ? serieData.data.Count
                        : serie.showDataDimension;
                    for (int j = 0; j < count; j++)
                    {
                        var value = serieData.data[j];
                        if (value > max) max = value;
                        if (value < min) min = value;
                    }
                }
            }
        }

        /// <summary>
        /// Whether the data for the specified dimension of serie are all 0.
        /// 系列指定维数的数据是否全部为0。
        /// </summary>
        /// <param name="serie">系列</param>
        /// <param name="dimension">指定维数</param>
        /// <returns></returns>
        public static bool IsAllZeroValue(Serie serie, int dimension = 1)
        {
            foreach (var serieData in serie.data)
            {
                if (serieData.GetData(dimension) != 0) return false;
            }
            return true;
        }
    }
}