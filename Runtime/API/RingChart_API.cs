/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public partial class RingChart
    {
        /// <summary>
        /// 更新指定系列执行数据项的最大值
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateMax(int serieIndex, int dataIndex, float value)
        {
            var serie = m_Series.GetSerie(serieIndex);
            if (serie != null)
            {
                var serieData = serie.GetSerieData(dataIndex);
                if (serieData != null)
                {
                    return serieData.UpdateData(1, value);
                }
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列的所有数据项的最大值
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateMax(int serieIndex, float value)
        {
            var serie = m_Series.GetSerie(serieIndex);
            if (serie != null)
            {
                var flag = true;
                foreach (var serieData in serie.data)
                {
                    if (!serieData.UpdateData(1, value)) flag = false;
                }
                return flag;
            }
            return false;
        }

        /// <summary>
        /// 更新第一个系列第一个数据项的最大值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateMax(float value)
        {
            return UpdateMax(0, 0, value);
        }

        /// <summary>
        /// Adds the data with the specified maximum value to the specified serie.
        /// 添加指定最大值的数据到指定系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="value">the data</param>
        /// <param name="max">the max data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public override SerieData AddData(string serieName, float value, float max, string dataName = null)
        {
            return base.AddData(serieName, value, max, dataName);
        }

        /// <summary>
        /// Adds the data with the specified maximum value to the specified serie.
        /// 添加指定最大值的数据到指定系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="value">the data</param>
        /// <param name="max">the max data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public override SerieData AddData(int serieIndex, float value, float max, string dataName = null)
        {
            return base.AddData(serieIndex, value, max, dataName);
        }
    }
}