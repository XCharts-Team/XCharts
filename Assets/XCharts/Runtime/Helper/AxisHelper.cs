/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using UnityEngine;

namespace XCharts
{
    public static class AxisHelper
    {

        /// <summary>
        /// 包含箭头偏移的轴线长度
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static float GetAxisLineSymbolOffset(Axis axis)
        {
            if (axis.axisLine.show && axis.axisLine.showArrow && axis.axisLine.arrow.offset > 0)
            {
                return axis.axisLine.arrow.offset;
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
                    int num = (int)(axis.runtimeMinMaxRange / axis.interval);
                    int maxNum = Mathf.CeilToInt(coordinateWid / 15);
                    if (num > maxNum)
                    {
                        axis.interval *= 2;
                        num = (int)(axis.runtimeMinMaxRange / axis.interval);
                    }
                    return num;
                }
                else
                {
                    return axis.splitNumber > 0 ? axis.splitNumber : 4;
                }
            }
            else if (axis.type == Axis.AxisType.Time)
            {
                if (axis.interval > 0)
                {
                    if (coordinateWid <= 0) return 0;
                    int num = (int)(axis.runtimeMinMaxRange / axis.interval);
                    int maxNum = Mathf.CeilToInt(coordinateWid / 15);
                    if (num > maxNum)
                    {
                        axis.interval *= 2;
                        num = (int)(axis.runtimeMinMaxRange / axis.interval);
                    }
                    return num;
                }
                else
                {
                    return axis.splitNumber > 0 ? axis.splitNumber : 4;
                }
            }
            else if (axis.type == Axis.AxisType.Log)
            {
                return axis.splitNumber > 0 ? axis.splitNumber : 4;
            }
            else if (axis.type == Axis.AxisType.Category)
            {
                int dataCount = axis.GetDataList(dataZoom).Count;
                if (!axis.boundaryGap)
                {
                    dataCount -= 1;
                }
                if (axis.splitNumber <= 0 || axis.splitNumber > dataCount) return dataCount;
                if (dataCount >= axis.splitNumber * 2) return axis.splitNumber;
                else return dataCount;
            }
            return 0;
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
            if (segment <= 0) segment = 1;
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
        public static string GetLabelName(Axis axis, float coordinateWidth, int index, double minValue, double maxValue,
            DataZoom dataZoom, bool forcePercent)
        {
            int split = GetSplitNumber(axis, coordinateWidth, dataZoom);
            if (axis.type == Axis.AxisType.Value)
            {
                if (minValue == 0 && maxValue == 0)
                    maxValue = axis.max != 0 ? axis.max : 1;
                double value = 0;
                if (forcePercent) maxValue = 100;
                if (axis.interval > 0)
                {
                    if (index == split) value = maxValue;
                    else value = minValue + index * axis.interval;
                }
                else
                {
                    value = minValue + (maxValue - minValue) * index / split;
                    if (!axis.clockwise && value != minValue) value = maxValue - value;
                }
                if (axis.inverse)
                {
                    value = -value;
                    minValue = -minValue;
                    maxValue = -maxValue;
                }
                if (forcePercent) return string.Format("{0}%", (int)value);
                else return axis.axisLabel.GetFormatterContent(index, value, minValue, maxValue);
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
                return axis.axisLabel.GetFormatterContent(index, value, minValue, maxValue, true);
            }
            else if (axis.type == Axis.AxisType.Time)
            {
                if (minValue == 0 && maxValue == 0) return string.Empty;
                double value = 0;
                if (axis.interval > 0)
                {
                    if (index == split) value = maxValue;
                    else value = minValue + index * axis.interval;
                }
                else
                {
                    value = minValue + (maxValue - minValue) * index / split;
                }
                return axis.axisLabel.GetFormatterDateTime(index, value);
            }
            var showData = axis.GetDataList(dataZoom);
            int dataCount = showData.Count;
            if (dataCount <= 0) return "";
            int rate = axis.boundaryGap ? (dataCount / split) : (dataCount - 1) / split;
            if (rate == 0) rate = 1;
            if (axis.insertDataToHead)
            {
                if (index > 0)
                {
                    var residue = (dataCount - 1) - split * rate;
                    var newIndex = residue + (index - 1) * rate;
                    if (newIndex < 0) newIndex = 0;
                    return axis.axisLabel.GetFormatterContent(newIndex, showData[newIndex]);
                }
                else
                {
                    if (axis.boundaryGap && coordinateWidth / dataCount > 5) return string.Empty;
                    else return axis.axisLabel.GetFormatterContent(0, showData[0]);
                }
            }
            else
            {
                int newIndex = index * rate;
                if (newIndex < dataCount)
                {
                    return axis.axisLabel.GetFormatterContent(newIndex, showData[newIndex]);
                }
                else
                {
                    if (axis.boundaryGap && coordinateWidth / dataCount > 5) return string.Empty;
                    else return axis.axisLabel.GetFormatterContent(dataCount - 1, showData[dataCount - 1]);
                }
            }
        }

        /// <summary>
        /// 获得分割线条数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static int GetScaleNumber(Axis axis, float coordinateWidth, DataZoom dataZoom = null)
        {
            int splitNum = GetSplitNumber(axis, coordinateWidth, dataZoom);
            if (splitNum == 0) return 0;
            if (axis.IsCategory())
            {
                var dataCount = axis.GetDataList(dataZoom).Count;
                var scaleNum = 0;

                if (axis.boundaryGap)
                {
                    scaleNum = dataCount > 2 && dataCount % splitNum == 0
                        ? splitNum + 1
                        : splitNum + 2;
                }
                else
                {
                    if (dataCount < splitNum) scaleNum = splitNum;
                    else scaleNum = dataCount > 2 && dataCount % splitNum == 0
                        ? splitNum
                        : splitNum + 1;
                }
                return scaleNum;
            }
            else
            {
                return splitNum + 1;
            }
        }

        /// <summary>
        /// 获得分割段宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static float GetScaleWidth(Axis axis, float coordinateWidth, int index, DataZoom dataZoom = null)
        {
            if (index < 0) return 0;
            int num = GetScaleNumber(axis, coordinateWidth, dataZoom);
            int splitNum = GetSplitNumber(axis, coordinateWidth, dataZoom);
            if (num <= 0) num = 1;
            if (axis.type == Axis.AxisType.Value && axis.interval > 0)
            {
                if (axis.runtimeMinMaxRange <= 0) return 0;
                if (index >= splitNum)
                {
                    return (float)(coordinateWidth - (index - 1) * axis.interval * coordinateWidth / axis.runtimeMinMaxRange);
                }
                else
                {
                    return (float)(axis.interval * coordinateWidth / axis.runtimeMinMaxRange);
                }
            }
            else
            {
                var data = axis.GetDataList(dataZoom);
                if (axis.IsCategory() && data.Count > 0)
                {
                    var count = axis.boundaryGap ? data.Count : data.Count - 1;
                    int tick = count / splitNum;
                    if (count <= 0) return 0;
                    var each = coordinateWidth / count;
                    if (axis.insertDataToHead)
                    {
                        var max = axis.boundaryGap ? splitNum : splitNum - 1;
                        if (index == 1)
                        {
                            if (axis.axisTick.alignWithLabel) return each * tick;
                            else return coordinateWidth - each * tick * max;
                        }
                        else
                        {
                            if (count < splitNum) return each;
                            else return each * (count / splitNum);
                        }
                    }
                    else
                    {
                        var max = axis.boundaryGap ? num - 1 : num;
                        if (index >= max)
                        {
                            if (axis.axisTick.alignWithLabel) return each * tick;
                            else return coordinateWidth - each * tick * (index - 1);
                        }
                        else
                        {
                            if (count < splitNum) return each;
                            else return each * (count / splitNum);
                        }
                    }
                }
                else
                {
                    if (splitNum <= 0) return 0;
                    else return coordinateWidth / splitNum;
                }
            }
        }

        public static float GetEachWidth(Axis axis, float coordinateWidth, DataZoom dataZoom = null)
        {
            var data = axis.GetDataList(dataZoom);
            if (data.Count > 0)
            {
                var count = axis.boundaryGap ? data.Count : data.Count - 1;
                return count > 0 ? coordinateWidth / count : coordinateWidth;
            }
            else
            {
                int num = GetScaleNumber(axis, coordinateWidth, dataZoom) - 1;
                return num > 0 ? coordinateWidth / num : coordinateWidth;
            }
        }

        /// <summary>
        /// 调整最大最小值
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public static void AdjustMinMaxValue(Axis axis, ref double minValue, ref double maxValue, bool needFormat, int ceilRate = 0)
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
                if (ceilRate == 0) ceilRate = axis.ceilRate;
                switch (axis.minMaxType)
                {
                    case Axis.AxisMinMaxType.Default:
                        if (minValue == 0 && maxValue == 0)
                        {
                        }
                        else if (minValue > 0 && maxValue > 0)
                        {
                            minValue = 0;
                            maxValue = needFormat ? ChartHelper.GetMaxDivisibleValue(maxValue, ceilRate) : maxValue;
                        }
                        else if (minValue < 0 && maxValue < 0)
                        {
                            minValue = needFormat ? ChartHelper.GetMinDivisibleValue(minValue, ceilRate) : minValue;
                            maxValue = 0;
                        }
                        else
                        {
                            minValue = needFormat ? ChartHelper.GetMinDivisibleValue(minValue, ceilRate) : minValue;
                            maxValue = needFormat ? ChartHelper.GetMaxDivisibleValue(maxValue, ceilRate) : maxValue;
                        }
                        break;
                    case Axis.AxisMinMaxType.MinMax:
                        minValue = needFormat ? ChartHelper.GetMinDivisibleValue(minValue, ceilRate) : minValue;
                        maxValue = needFormat ? ChartHelper.GetMaxDivisibleValue(maxValue, ceilRate) : maxValue;
                        break;
                }
            }
            double tempRange = maxValue - minValue;
            if (axis.runtimeMinMaxRange != tempRange)
            {
                axis.runtimeMinMaxRange = tempRange;
                if (axis.type == Axis.AxisType.Value && axis.interval > 0)
                {
                    axis.SetComponentDirty();
                }
            }
        }

        public static bool NeedShowSplit(Axis axis)
        {
            if (!axis.show) return false;
            if (axis.IsCategory() && axis.GetDataList().Count <= 0) return false;
            else return true;
        }

        public static void AdjustCircleLabelPos(ChartText txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)
        {
            var txtWidth = txt.GetPreferredWidth();
            var sizeDelta = new Vector2(txtWidth, txt.GetPreferredHeight());
            txt.SetSizeDelta(sizeDelta);
            var diff = pos.x - cenPos.x;
            if (diff < -1f) //left
            {
                pos = new Vector3(pos.x - txtWidth / 2, pos.y);
            }
            else if (diff > 1f) //right
            {
                pos = new Vector3(pos.x + txtWidth / 2, pos.y);
            }
            else
            {
                float y = pos.y > cenPos.y ? pos.y + txtHig / 2 : pos.y - txtHig / 2;
                pos = new Vector3(pos.x, y);
            }
            txt.SetLocalPosition(pos + offset);
        }

        public static void AdjustCircleLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)
        {
            var txtWidth = txt.label.GetPreferredWidth();
            var sizeDelta = new Vector2(txtWidth, txt.label.GetPreferredHeight());
            txt.label.SetSizeDelta(sizeDelta);
            var diff = pos.x - cenPos.x;
            if (diff < -1f) //left
            {
                pos = new Vector3(pos.x - txtWidth / 2, pos.y);
            }
            else if (diff > 1f) //right
            {
                pos = new Vector3(pos.x + txtWidth / 2, pos.y);
            }
            else
            {
                float y = pos.y > cenPos.y ? pos.y + txtHig / 2 : pos.y - txtHig / 2;
                pos = new Vector3(pos.x, y);
            }
            txt.SetPosition(pos + offset);
        }

        public static void AdjustRadiusAxisLabelPos(ChartText txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)
        {
            var txtWidth = txt.GetPreferredWidth();
            var sizeDelta = new Vector2(txtWidth, txt.GetPreferredHeight());
            txt.SetSizeDelta(sizeDelta);
            var diff = pos.y - cenPos.y;
            if (diff > 20f) //left
            {
                pos = new Vector3(pos.x - txtWidth / 2, pos.y);
            }
            else if (diff < -20f) //right
            {
                pos = new Vector3(pos.x + txtWidth / 2, pos.y);
            }
            else
            {
                float y = pos.y > cenPos.y ? pos.y + txtHig / 2 : pos.y - txtHig / 2;
                pos = new Vector3(pos.x, y);
            }
            txt.SetLocalPosition(pos);
        }

        public static float GetAxisPosition(Grid grid, Axis axis, double value, int dataCount = 0, DataZoom dataZoom = null)
        {
            var gridHeight = axis is YAxis ? grid.runtimeHeight : grid.runtimeWidth;
            var gridXY = axis is YAxis ? grid.runtimeY : grid.runtimeX;
            if (axis.IsCategory())
            {
                if (dataCount == 0) dataCount = axis.data.Count;
                var categoryIndex = (int)value;
                var scaleWid = AxisHelper.GetDataWidth(axis, grid.runtimeHeight, dataCount, dataZoom);
                float startY = gridXY + (axis.boundaryGap ? scaleWid / 2 : 0);
                return startY + scaleWid * categoryIndex;
            }
            else
            {
                var yDataHig = (axis.runtimeMinMaxRange == 0) ? 0f :
                    (float)((value - axis.runtimeMinValue) / axis.runtimeMinMaxRange * gridHeight);
                return gridXY + yDataHig;

            }
        }
    }
}