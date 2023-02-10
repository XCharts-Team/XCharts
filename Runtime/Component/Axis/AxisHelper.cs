using UnityEngine;

namespace XCharts.Runtime
{
    public static class AxisHelper
    {

        /// <summary>
        /// 包含箭头偏移的轴线长度
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static float GetAxisLineArrowOffset(Axis axis)
        {
            if (axis.axisLine.show && axis.axisLine.showArrow && axis.axisLine.arrow.offset > 0)
            {
                return axis.axisLine.arrow.offset;
            }
            return 0;
        }

        /// <summary>
        /// 获得分割网格个数，包含次刻度
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static int GetTotalSplitGridNum(Axis axis)
        {
            if (axis.IsCategory())
                return axis.data.Count;
            else
            {
                var splitNum = axis.splitNumber <= 0 ? GetSplitNumber(axis, 0, null) : axis.splitNumber;
                return splitNum * axis.minorTick.splitNumber;
            }
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
                return axis.context.labelValueList.Count - 1;
            }
            else if (axis.type == Axis.AxisType.Time)
            {
                return axis.context.labelValueList.Count;
            }
            else if (axis.type == Axis.AxisType.Log)
            {
                return axis.splitNumber > 0 ? axis.splitNumber : 4;
            }
            else if (axis.type == Axis.AxisType.Category)
            {
                int dataCount = axis.GetDataList(dataZoom).Count;
                if (!axis.boundaryGap)
                    dataCount -= 1;
                if (dataCount <= 0)
                    dataCount = 1;

                if (axis.splitNumber <= 0)
                {
                    var eachWid = coordinateWid / dataCount;
                    var min = axis is YAxis ? 20 : 80;
                    if (eachWid > min) return dataCount;
                    var tick = Mathf.CeilToInt(min / eachWid);
                    return (int)(dataCount / tick);
                }
                else
                {
                    if (axis.splitNumber <= 0 || axis.splitNumber > dataCount)
                        return dataCount;
                    if (dataCount >= axis.splitNumber * 2)
                        return axis.splitNumber;
                    else
                        return dataCount;
                }
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
            if (dataCount < 1)
                dataCount = 1;
            if (axis.IsValue())
                return dataCount > 1 ? coordinateWidth / (dataCount - 1) : coordinateWidth;
            var categoryCount = axis.GetDataCount(dataZoom);
            int segment = (axis.boundaryGap ? categoryCount : categoryCount - 1);
            segment = segment <= 0 ? dataCount : segment;
            if (segment <= 0)
                segment = 1;

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
                if (forcePercent)
                    maxValue = 100;

                value = axis.GetLabelValue(index);
                if (axis.inverse)
                {
                    value = -value;
                    minValue = -minValue;
                    maxValue = -maxValue;
                }
                if (forcePercent)
                    return string.Format("{0}%", (int)value);
                else
                    return axis.axisLabel.GetFormatterContent(index, value, minValue, maxValue);
            }
            else if (axis.type == Axis.AxisType.Log)
            {
                double value = axis.logBaseE ?
                    System.Math.Exp(axis.GetLogMinIndex() + index) :
                    System.Math.Pow(axis.logBase, axis.GetLogMinIndex() + index);
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
                if (minValue == 0 && maxValue == 0)
                    return string.Empty;
                if (index > axis.context.labelValueList.Count - 1)
                    return string.Empty;

                var value = axis.GetLabelValue(index);
                return axis.axisLabel.GetFormatterDateTime(index, value, minValue, maxValue);
            }
            var showData = axis.GetDataList(dataZoom);
            int dataCount = showData.Count;
            if (dataCount <= 0)
                return "";
            int rate = axis.boundaryGap ? (dataCount / split) : (dataCount - 1) / split;
            if (rate == 0) rate = 1;
            if (axis.insertDataToHead)
            {
                if (index > 0)
                {
                    var residue = (dataCount - 1) - split * rate;
                    var newIndex = residue + (index - 1) * rate;
                    if (newIndex < 0)
                        newIndex = 0;
                    return axis.axisLabel.GetFormatterContent(newIndex, showData[newIndex]);
                }
                else
                {
                    if (axis.boundaryGap && coordinateWidth / dataCount > 5)
                        return string.Empty;
                    else
                        return axis.axisLabel.GetFormatterContent(0, showData[0]);
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
                    var diff = newIndex - dataCount;
                    if (axis.boundaryGap && ((diff > 0 && diff / rate < 0.4f) || dataCount >= axis.data.Count))
                        return string.Empty;
                    else
                        return axis.axisLabel.GetFormatterContent(dataCount - 1, showData[dataCount - 1]);
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
            if (splitNum == 0)
                return 0;

            if (axis.IsCategory())
            {
                var dataCount = axis.GetDataList(dataZoom).Count;
                var scaleNum = 0;

                if (axis.boundaryGap)
                {
                    scaleNum = dataCount > 1 && dataCount % splitNum == 0 ?
                        splitNum + 1 :
                        splitNum + 2;
                }
                else
                {
                    scaleNum = splitNum + 1;
                }
                return scaleNum;
            }
            else if (axis.IsTime())
                return splitNum;
            else
                return splitNum + 1;
        }

        /// <summary>
        /// 获得分割段宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static float GetScaleWidth(Axis axis, float coordinateWidth, int index, DataZoom dataZoom = null)
        {
            if (index < 0)
                return 0;

            int num = GetScaleNumber(axis, coordinateWidth, dataZoom);
            int splitNum = GetSplitNumber(axis, coordinateWidth, dataZoom);
            if (num <= 0)
                num = 1;

            if (axis.IsTime() || axis.IsValue())
            {
                var value = axis.GetLabelValue(index);
                var lastValue = axis.GetLabelValue(index - 1);
                return axis.context.minMaxRange == 0 ? 0 :
                    (float)(coordinateWidth * (value - lastValue) / axis.context.minMaxRange);
            }
            else
            {
                var data = axis.GetDataList(dataZoom);
                if (axis.IsCategory() && data.Count > 0 && splitNum > 0)
                {
                    var count = axis.boundaryGap ? data.Count : data.Count - 1;
                    int tick = count / splitNum;
                    if (count <= 0)
                        return 0;

                    var each = coordinateWidth / count;
                    if (axis.insertDataToHead)
                    {
                        var max = axis.boundaryGap ? splitNum : splitNum - 1;
                        if (index == 1)
                        {
                            if (axis.axisTick.alignWithLabel)
                                return each * tick;
                            else
                                return coordinateWidth - each * tick * max;
                        }
                        else
                        {
                            if (count < splitNum)
                                return each;
                            else
                                return each * (count / splitNum);
                        }
                    }
                    else
                    {
                        var max = axis.boundaryGap ? num - 1 : num;
                        if (index >= max)
                        {
                            if (axis.axisTick.alignWithLabel)
                                return each * tick;
                            else
                                return coordinateWidth - each * tick * (index - 1);
                        }
                        else
                        {
                            if (count < splitNum)
                                return each;
                            else
                                return each * (count / splitNum);
                        }
                    }
                }
                else
                {
                    if (splitNum <= 0)
                        return 0;
                    else
                        return coordinateWidth / splitNum;
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
        public static void AdjustMinMaxValue(Axis axis, ref double minValue, ref double maxValue, bool needFormat, double ceilRate = 0)
        {
            if (axis.type == Axis.AxisType.Log)
            {
                int minSplit = 0;
                int maxSplit = 0;
                maxValue = ChartHelper.GetMaxLogValue(maxValue, axis.logBase, axis.logBaseE, out maxSplit);
                minValue = ChartHelper.GetMinLogValue(minValue, axis.logBase, axis.logBaseE, out minSplit);
                var splitNumber = (minSplit > 0 && maxSplit > 0) ? (maxSplit + minSplit - 1) : (maxSplit + minSplit);
                if (splitNumber > 15)
                    splitNumber = 15;
                axis.splitNumber = splitNumber;
                return;
            }
            if (axis.type == Axis.AxisType.Time) { }
            else if (axis.minMaxType == Axis.AxisMinMaxType.Custom)
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

                        if (minValue == 0 && maxValue == 0) { }
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

                        minValue = ceilRate != 0 ? ChartHelper.GetMinDivisibleValue(minValue, ceilRate) : minValue;
                        maxValue = ceilRate != 0 ? ChartHelper.GetMaxDivisibleValue(maxValue, ceilRate) : maxValue;
                        break;
                }
            }
        }

        public static bool NeedShowSplit(Axis axis)
        {
            if (!axis.show)
                return false;
            if (axis.IsCategory() && axis.GetDataList().Count <= 0)
                return false;
            else
                return true;
        }

        public static void AdjustCircleLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)
        {
            var txtWidth = txt.text.GetPreferredWidth();
            var sizeDelta = new Vector2(txtWidth, txt.text.GetPreferredHeight());
            txt.text.SetSizeDelta(sizeDelta);
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

        public static void AdjustRadiusAxisLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)
        {
            var txtWidth = txt.text.GetPreferredWidth();
            var sizeDelta = new Vector2(txtWidth, txt.text.GetPreferredHeight());
            txt.text.SetSizeDelta(sizeDelta);
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
            txt.SetPosition(pos);
        }

        public static float GetAxisPosition(GridCoord grid, Axis axis, double value, int dataCount = 0, DataZoom dataZoom = null)
        {
            var gridHeight = axis is YAxis ? grid.context.height : grid.context.width;
            var gridXY = axis is YAxis ? grid.context.y : grid.context.x;
            if (axis.IsCategory())
            {
                if (dataCount == 0) dataCount = axis.data.Count;
                var categoryIndex = (int)value;
                var scaleWid = AxisHelper.GetDataWidth(axis, gridHeight, dataCount, dataZoom);
                float startY = gridXY + (axis.boundaryGap ? scaleWid / 2 : 0);
                return startY + scaleWid * categoryIndex;
            }
            else
            {
                var yDataHig = (axis.context.minMaxRange == 0) ? 0f :
                    (float)((value - axis.context.minValue) / axis.context.minMaxRange * gridHeight);
                return gridXY + yDataHig;
            }
        }

        public static double GetAxisPositionValue(GridCoord grid, Axis axis, Vector3 pos)
        {
            if (axis is YAxis)
                return GetAxisPositionValue(pos.y, grid.context.height, axis.context.minMaxRange, grid.context.y, axis.context.offset);
            else if (axis is XAxis)
                return GetAxisPositionValue(pos.x, grid.context.width, axis.context.minMaxRange, grid.context.x, axis.context.offset);
            else
                return 0;
        }

        public static double GetAxisPositionValue(float xy, float axisLength, double axisRange, float axisStart, float axisOffset)
        {
            var yRate = axisRange / axisLength;
            return yRate * (xy - axisStart - axisOffset);
        }

        /// <summary>
        /// 获得数值value在坐标轴上的坐标位置
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="axis"></param>
        /// <param name="scaleWidth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetAxisValuePosition(GridCoord grid, Axis axis, float scaleWidth, double value)
        {
            return GetAxisPositionInternal(grid, axis, scaleWidth, value, true, false);
        }

        /// <summary>
        /// 获得数值value在坐标轴上相对起点的距离
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="axis"></param>
        /// <param name="scaleWidth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetAxisValueDistance(GridCoord grid, Axis axis, float scaleWidth, double value)
        {
            return GetAxisPositionInternal(grid, axis, scaleWidth, value, false, false);
        }

        /// <summary>
        /// 获得数值value在坐标轴上对应的长度
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="axis"></param>
        /// <param name="scaleWidth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetAxisValueLength(GridCoord grid, Axis axis, float scaleWidth, double value)
        {
            return GetAxisPositionInternal(grid, axis, scaleWidth, value, false, true);
        }

        /// <summary>
        /// 获得数值value在坐标轴上对应的split索引
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetAxisValueSplitIndex(Axis axis, double value, int totalSplitNumber = -1)
        {
            if (axis.IsCategory())
            {
                return (int)value;
            }
            else
            {
                if (value == axis.context.minValue)
                    return 0;
                else
                {
                    if (totalSplitNumber == -1)
                        totalSplitNumber = GetTotalSplitGridNum(axis);
                    if (axis.minMaxType == Axis.AxisMinMaxType.Custom)
                        return Mathf.CeilToInt(((float)((value - axis.min) / axis.max) * totalSplitNumber) - 1);
                    else
                        return Mathf.CeilToInt(((float)((value - axis.context.minValue) / axis.context.minMaxRange) * totalSplitNumber) - 1);
                }
            }
        }

        private static float GetAxisPositionInternal(GridCoord grid, Axis axis, float scaleWidth, double value, bool includeGridXY, bool realLength)
        {
            var isY = axis is YAxis;
            var gridHeight = isY ? grid.context.height : grid.context.width;
            var gridXY = isY ? grid.context.y : grid.context.x;

            if (axis.IsLog())
            {
                var minIndex = axis.GetLogMinIndex();
                var nowIndex = axis.GetLogValue(value);
                return includeGridXY ?
                    (float)(gridXY + (nowIndex - minIndex) / axis.splitNumber * gridHeight) :
                    (float)((nowIndex - minIndex) / axis.splitNumber * gridHeight);
            }
            else if (axis.IsCategory())
            {
                var categoryIndex = (int)value;
                return includeGridXY ?
                    gridXY + (axis.boundaryGap ? scaleWidth / 2 : 0) + scaleWidth * categoryIndex :
                    (axis.boundaryGap ? scaleWidth / 2 : 0) + scaleWidth * categoryIndex;
            }
            else
            {
                var yDataHig = 0f;
                if (axis.context.minMaxRange != 0)
                {
                    if (realLength)
                        yDataHig = (float)(value * gridHeight / axis.context.minMaxRange);
                    else
                        yDataHig = (float)((value - axis.context.minValue) / axis.context.minMaxRange * gridHeight);
                }
                return includeGridXY ?
                    gridXY + yDataHig :
                    yDataHig;
            }
        }

        public static float GetAxisXOrY(GridCoord grid, Axis axis, Axis relativedAxis)
        {
            if (axis is XAxis)
                return GetXAxisXOrY(grid, axis, relativedAxis);
            else if (axis is YAxis)
                return GetYAxisXOrY(grid, axis, relativedAxis);
            else if (axis is SingleAxis)
                return axis.context.y + axis.offset;
            else if (axis is ParallelAxis)
                return axis.context.y;
            else
                return axis.context.x;
        }

        public static float GetXAxisXOrY(GridCoord grid, Axis xAxis, Axis relativedAxis)
        {
            var startY = grid.context.y + xAxis.offset;
            if (xAxis.IsTop())
                startY += grid.context.height;
            else if (xAxis.axisLine.onZero && relativedAxis.IsValue() && relativedAxis.gridIndex == xAxis.gridIndex)
                startY += relativedAxis.context.offset;
            return startY;
        }

        public static float GetYAxisXOrY(GridCoord grid, Axis yAxis, Axis relativedAxis)
        {
            var startX = grid.context.x + yAxis.offset;
            if (yAxis.IsRight())
                startX += grid.context.width;
            else if (yAxis.axisLine.onZero && relativedAxis.IsValue() && relativedAxis.gridIndex == yAxis.gridIndex)
                startX += relativedAxis.context.offset;
            return startX;
        }
    }
}