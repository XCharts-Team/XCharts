/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using System.Text;
using UnityEngine;

namespace XCharts
{
    public static class AxisHelper
    {
        public static float GetTickWidth(Axis axis)
        {
            return axis.axisTick.width != 0 ? axis.axisTick.width : axis.axisLine.width;
        }

        /// <summary>
        /// 包含箭头偏移的轴线长度
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static float GetAxisLineSymbolOffset(Axis axis)
        {
            if (axis.axisLine.show && axis.axisLine.symbol && axis.axisLine.symbolOffset > 0)
            {
                return axis.axisLine.symbolOffset;
            }
            return 0;
        }

        /// <summary>
        /// 获得分割段数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static int GetSplitNumber(Axis axis, float coordinateWid, DataZoom dataZoom)
        {
            if (axis.type == Axis.AxisType.Value)
            {
                if (axis.interval > 0)
                {
                    if (coordinateWid <= 0) return 0;
                    int num = Mathf.CeilToInt(axis.runtimeMinMaxRange / axis.interval) + 1;
                    int maxNum = Mathf.CeilToInt(coordinateWid / 15);
                    if (num > maxNum)
                    {
                        axis.interval *= 2;
                        num = Mathf.CeilToInt(axis.runtimeMinMaxRange / axis.interval) + 1;
                    }
                    return num;
                }
                else return axis.splitNumber;
            }
            else if (axis.type == Axis.AxisType.Log)
            {
                return axis.splitNumber;
            }
            int dataCount = axis.GetDataList(dataZoom).Count;
            if (axis.splitNumber <= 0) return dataCount;
            if (dataCount > 2 * axis.splitNumber || dataCount <= 0)
                return axis.splitNumber;
            else
                return dataCount;
        }

        /// <summary>
        /// 获得分割段的宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static float GetSplitWidth(Axis axis, float coordinateWidth, DataZoom dataZoom)
        {
            int split = GetSplitNumber(axis, coordinateWidth, dataZoom);
            int segment = (axis.boundaryGap ? split : split - 1);
            segment = segment <= 0 ? 1 : segment;
            return coordinateWidth / segment;
        }

        /// <summary>
        /// 获得一个类目数据在坐标系中代表的宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static float GetDataWidth(Axis axis, float coordinateWidth, int dataCount, DataZoom dataZoom)
        {
            if (dataCount < 1) dataCount = 1;
            var categoryCount = axis.GetDataNumber(dataZoom);
            int segment = (axis.boundaryGap ? categoryCount : categoryCount - 1);
            segment = segment <= 0 ? dataCount : segment;
            return coordinateWidth / segment;
        }

        /// <summary>
        /// 获得标签显示的名称
        /// </summary>
        /// <param name="index"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        internal static string GetLabelName(Axis axis, float coordinateWidth, int index, float minValue, float maxValue,
            DataZoom dataZoom, bool forcePercent)
        {
            int split = GetSplitNumber(axis, coordinateWidth, dataZoom);
            if (axis.type == Axis.AxisType.Value)
            {
                if (minValue == 0 && maxValue == 0) return string.Empty;
                float value = 0;
                if (forcePercent) maxValue = 100;
                if (axis.interval > 0)
                {
                    if (index == split - 1) value = maxValue;
                    else value = minValue + index * axis.interval;
                }
                else
                {
                    value = (minValue + (maxValue - minValue) * index / (split - 1));
                }
                if (axis.inverse)
                {
                    value = -value;
                    minValue = -minValue;
                    maxValue = -maxValue;
                }
                if (forcePercent) return string.Format("{0}%", (int)value);
                else return axis.axisLabel.GetFormatterContent(value, minValue, maxValue);
            }
            else if (axis.type == Axis.AxisType.Log)
            {
                float value = axis.logBaseE ? Mathf.Exp(axis.runtimeMinLogIndex + index) :
                    Mathf.Pow(axis.logBase, axis.runtimeMinLogIndex + index);
                if (axis.inverse)
                {
                    value = -value;
                    minValue = -minValue;
                    maxValue = -maxValue;
                }
                return axis.axisLabel.GetFormatterContent(value, minValue, maxValue, true);
            }
            var showData = axis.GetDataList(dataZoom);
            int dataCount = showData.Count;
            if (dataCount <= 0) return "";

            if (index == split - 1 && !axis.boundaryGap)
            {
                return axis.axisLabel.GetFormatterContent(showData[dataCount - 1]);
            }
            else
            {
                float rate = dataCount / split;
                if (rate < 1) rate = 1;
                int offset = axis.boundaryGap ? (int)(rate / 2) : 0;
                int newIndex = (int)(index * rate >= dataCount - 1 ?
                    dataCount - 1 : offset + index * rate);
                return axis.axisLabel.GetFormatterContent(showData[newIndex]);
            }
        }

        /// <summary>
        /// 获得分割线条数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        internal static int GetScaleNumber(Axis axis, float coordinateWidth, DataZoom dataZoom)
        {
            if (axis.type == Axis.AxisType.Value || axis.type == Axis.AxisType.Log)
            {
                int splitNum = GetSplitNumber(axis, coordinateWidth, dataZoom);
                return axis.boundaryGap ? splitNum + 1 : splitNum;
            }
            else
            {
                var showData = axis.GetDataList(dataZoom);
                int dataCount = showData.Count;
                if (axis.splitNumber <= 0) return axis.boundaryGap ? dataCount + 1 : dataCount;
                if (dataCount > 2 * axis.splitNumber || dataCount <= 0)
                    return axis.boundaryGap ? axis.splitNumber + 1 : axis.splitNumber;
                else
                    return axis.boundaryGap ? dataCount + 1 : dataCount;
            }
        }

        /// <summary>
        /// 获得分割段宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        internal static float GetScaleWidth(Axis axis, float coordinateWidth, int index, DataZoom dataZoom)
        {
            int num = GetScaleNumber(axis, coordinateWidth, dataZoom) - 1;
            if (num <= 0) num = 1;
            if (axis.type == Axis.AxisType.Value && axis.interval > 0)
            {
                if (index == num - 1) return coordinateWidth - (num - 1) * axis.interval * coordinateWidth / axis.runtimeMinMaxRange;
                else return axis.interval * coordinateWidth / axis.runtimeMinMaxRange;
            }
            else
            {
                return coordinateWidth / num;
            }
        }

        /// <summary>
        /// 调整最大最小值
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        internal static void AdjustMinMaxValue(Axis axis, ref float minValue, ref float maxValue, bool needFormat)
        {
            if (axis.type == Axis.AxisType.Log)
            {
                int minSplit = 0;
                int maxSplit = 0;
                maxValue = ChartHelper.GetMaxLogValue(maxValue, axis.logBase, axis.logBaseE, out maxSplit);
                minValue = ChartHelper.GetMinLogValue(minValue, axis.logBase, axis.logBaseE, out minSplit);
                axis.splitNumber = (minSplit > 0 && maxSplit > 0) ? (maxSplit + minSplit - 1) : (maxSplit + minSplit);
                return;
            }
            if (axis.minMaxType == Axis.AxisMinMaxType.Custom)
            {
                if (axis.min != 0 || axis.max != 0)
                {
                    if (axis.inverse)
                    {
                        minValue = -axis.max;
                        maxValue = -axis.min;
                    }
                    else
                    {
                        minValue = axis.min;
                        maxValue = axis.max;
                    }
                }
            }
            else
            {
                switch (axis.minMaxType)
                {
                    case Axis.AxisMinMaxType.Default:
                        if (minValue == 0 && maxValue == 0)
                        {
                        }
                        else if (minValue > 0 && maxValue > 0)
                        {
                            minValue = 0;
                            maxValue = needFormat ? ChartHelper.GetMaxDivisibleValue(maxValue, axis.ceilRate) : maxValue;
                        }
                        else if (minValue < 0 && maxValue < 0)
                        {
                            minValue = needFormat ? ChartHelper.GetMinDivisibleValue(minValue, axis.ceilRate) : minValue;
                            maxValue = 0;
                        }
                        else
                        {
                            minValue = needFormat ? ChartHelper.GetMinDivisibleValue(minValue, axis.ceilRate) : minValue;
                            maxValue = needFormat ? ChartHelper.GetMaxDivisibleValue(maxValue, axis.ceilRate) : maxValue;
                        }
                        break;
                    case Axis.AxisMinMaxType.MinMax:
                        minValue = needFormat ? ChartHelper.GetMinDivisibleValue(minValue, axis.ceilRate) : minValue;
                        maxValue = needFormat ? ChartHelper.GetMaxDivisibleValue(maxValue, axis.ceilRate) : maxValue;
                        break;
                }
            }
            var tempRange = maxValue - minValue;
            if (axis.runtimeMinMaxRange != tempRange)
            {
                axis.runtimeMinMaxRange = tempRange;
                if (axis.type == Axis.AxisType.Value && axis.interval > 0)
                {
                    axis.SetComponentDirty();
                }
            }
        }

        internal static bool NeedShowSplit(Axis axis)
        {
            if (!axis.show) return false;
            if (axis.IsCategory() && axis.data.Count <= 0) return false;
            else if (axis.IsValue() && axis.runtimeMinValue == 0 && axis.runtimeMaxValue == 0) return false;
            else return true;
        }
    }
}