using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class AxisContext : MainComponentContext
    {
        public Orient orient;
        /// <summary>
        /// 坐标轴的起点X
        /// </summary>
        public float x;
        /// <summary>
        /// 坐标轴的起点Y
        /// </summary>
        public float y;
        /// <summary>
        /// 坐标轴原点X
        /// </summary>
        public float zeroX;
        /// <summary>
        /// 坐标轴原点Y
        /// </summary>
        public float zeroY;
        public float width;
        public float height;
        public Vector3 position;
        public float left;
        public float right;
        public float bottom;
        public float top;
        /// <summary>
        /// the current minimun value.
        /// |当前最小值。
        /// </summary>
        public double minValue;
        /// <summary>
        /// the current maximum value.
        /// |当前最大值。
        /// </summary>
        public double maxValue;
        /// <summary>
        /// the offset of zero position.
        /// |坐标轴原点在坐标轴的偏移。
        /// </summary>
        public float offset;
        public double minMaxRange;
        /// <summary>
        /// the tick value of value axis.
        /// |数值轴时每个tick的数值。
        /// </summary>
        public double tickValue;
        public float scaleWidth;
        public float startAngle;
        public double pointerValue;
        public Vector3 pointerLabelPosition;
        public double axisTooltipValue;
        public TextAnchor aligment;
        public List<string> runtimeData { get { return m_RuntimeData; } }
        public List<double> labelValueList { get { return m_LabelValueList; } }
        public List<ChartLabel> labelObjectList { get { return m_AxisLabelList; } }
        public int dataZoomStartIndex;

        internal List<string> filterData;
        internal bool lastCheckInverse;
        internal bool isNeedUpdateFilterData;

        private int filterStart;
        private int filterEnd;
        private int filterMinShow;

        private List<ChartLabel> m_AxisLabelList = new List<ChartLabel>();
        private List<double> m_LabelValueList = new List<double>();
        private List<string> m_RuntimeData = new List<string>();

        internal void Clear()
        {
            m_RuntimeData.Clear();
        }

        private List<string> m_EmptyFliter = new List<string>();
        /// <summary>
        /// 更新dataZoom对应的类目数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateFilterData(List<string> data, DataZoom dataZoom)
        {
            int start = 0, end = 0;
            var range = Mathf.RoundToInt(data.Count * (dataZoom.end - dataZoom.start) / 100);
            if (range <= 0)
                range = 1;

            if (dataZoom.context.invert)
            {
                end = Mathf.RoundToInt(data.Count * dataZoom.end / 100);
                start = end - range;
                if (start < 0) start = 0;
            }
            else
            {
                start = Mathf.RoundToInt(data.Count * dataZoom.start / 100);
                end = start + range;
                if (end > data.Count) end = data.Count;
            }

            if (start != filterStart ||
                end != filterEnd ||
                dataZoom.minShowNum != filterMinShow ||
                isNeedUpdateFilterData)
            {
                filterStart = start;
                filterEnd = end;
                filterMinShow = dataZoom.minShowNum;
                isNeedUpdateFilterData = false;

                if (data.Count > 0)
                {
                    if (range < dataZoom.minShowNum)
                    {
                        if (dataZoom.minShowNum > data.Count)
                            range = data.Count;
                        else
                            range = dataZoom.minShowNum;
                    }
                    if (range > data.Count - start)
                        start = data.Count - range;
                    if (start >= 0)
                    {
                        dataZoomStartIndex = start;
                        filterData = data.GetRange(start, range);
                    }
                    else
                    {
                        dataZoomStartIndex = 0;
                        filterData = data;
                    }
                }
                else
                {
                    dataZoomStartIndex = 0;
                    filterData = data;
                }
            }
            else if (end == 0)
            {
                dataZoomStartIndex = 0;
                filterData = m_EmptyFliter;
            }
        }
    }
}