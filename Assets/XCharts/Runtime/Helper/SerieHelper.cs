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

        /// <summary>
        /// 更新运行时中心点和半径
        /// </summary>
        /// <param name="chartWidth"></param>
        /// <param name="chartHeight"></param>
        public static void UpdateCenter(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (serie.center.Length < 2) return;
            var centerX = serie.center[0] <= 1 ? chartWidth * serie.center[0] : serie.center[0];
            var centerY = serie.center[1] <= 1 ? chartHeight * serie.center[1] : serie.center[1];
            serie.runtimeCenterPos = chartPosition + new Vector3(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            serie.runtimeInsideRadius = serie.radius[0] <= 1 ? minWidth * serie.radius[0] : serie.radius[0];
            serie.runtimeOutsideRadius = serie.radius[1] <= 1 ? minWidth * serie.radius[1] : serie.radius[1];
        }

        public static void UpdateRect(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (serie.left != 0 || serie.right != 0 || serie.top != 0 || serie.bottom != 0)
            {
                var runtimeLeft = serie.left <= 1 ? serie.left * chartWidth : serie.left;
                var runtimeBottom = serie.bottom <= 1 ? serie.bottom * chartHeight : serie.bottom;
                var runtimeTop = serie.top <= 1 ? serie.top * chartHeight : serie.top;
                var runtimeRight = serie.right <= 1 ? serie.right * chartWidth : serie.right;

                serie.runtimeX = chartPosition.x + runtimeLeft;
                serie.runtimeY = chartPosition.y + runtimeBottom;
                serie.runtimeWidth = chartWidth - runtimeLeft - runtimeRight;
                serie.runtimeHeight = chartHeight - runtimeTop - runtimeBottom;
                serie.runtimeCenterPos = new Vector3(serie.runtimeX + serie.runtimeWidth / 2,
                    serie.runtimeY + serie.runtimeHeight / 2);
            }
            else
            {
                serie.runtimeX = chartPosition.x;
                serie.runtimeY = chartPosition.y;
                serie.runtimeWidth = chartWidth;
                serie.runtimeHeight = chartHeight;
                serie.runtimeCenterPos = chartPosition + new Vector3(chartWidth / 2, chartHeight / 2);
            }
        }
    }
}