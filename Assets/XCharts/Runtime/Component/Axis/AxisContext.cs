/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public class AxisContext : MainComponentContext
    {
        /// <summary>
        /// the current minimun value.
        /// 当前最小值。
        /// </summary>
        public double minValue
        {
            get { return m_RuntimeMinValue; }
            internal set
            {
                m_RuntimeMinValue = value;
                m_RuntimeLastMinValue = value;
                m_RuntimeMinValueUpdateTime = Time.time;
                m_RuntimeMinValueChanged = true;
            }
        }
        /// <summary>
        /// the current maximum value.
        /// 当前最大值。
        /// </summary>
        public double maxValue
        {
            get { return m_RuntimeMaxValue; }
            internal set
            {
                m_RuntimeMaxValue = value;
                m_RuntimeLastMaxValue = value;
                m_RuntimeMaxValueUpdateTime = Time.time;
                m_RuntimeMaxValueChanged = false;
            }
        }
        /// <summary>
        /// the x offset of zero position.
        /// 坐标轴原点在X轴的偏移。
        /// </summary>
        public float xOffset { get; internal set; }
        /// <summary>
        /// the y offset of zero position.
        /// 坐标轴原点在Y轴的偏移。
        /// </summary>
        public float yOffset { get; internal set; }
        public double minMaxRange { get; internal set; }
        public float scaleWidth { get; internal set; }
        public float startAngle { get; set; }
        public double pointerValue { get; internal set; }
        public Vector3 pointerLabelPosition { get; internal set; }
        public double axisTooltipValue { get; internal set; }
        public List<string> runtimeData { get { return m_RuntimeData; } }
        public List<double> labelValueList { get { return m_LabelValueList; } }
        public List<ChartLabel> labelObjectList { get { return m_AxisLabelList; } }

        internal List<string> filterData;
        internal bool lastCheckInverse { get; set; }
        internal bool isNeedUpdateFilterData;

        private int filterStart;
        private int filterEnd;
        private int filterMinShow;
        private double m_RuntimeMinValue;
        private double m_RuntimeLastMinValue;
        private bool m_RuntimeMinValueChanged;
        private float m_RuntimeMinValueUpdateTime;
        private double m_RuntimeMaxValue;
        private double m_RuntimeLastMaxValue;
        private bool m_RuntimeMaxValueChanged;
        private float m_RuntimeMaxValueUpdateTime;
        private bool m_RuntimeMinValueFirstChanged = true;
        private bool m_RuntimeMaxValueFirstChanged = true;

        private List<ChartLabel> m_AxisLabelList = new List<ChartLabel>();
        private List<double> m_LabelValueList = new List<double>();
        private List<string> m_RuntimeData = new List<string>();


        internal void Clear()
        {
            m_RuntimeData.Clear();
        }

        internal bool UpdateMinValue(double value, bool check)
        {
            if (value != maxValue)
            {
                if (check && Application.isPlaying)
                {
                    if (m_RuntimeMinValueFirstChanged)
                    {
                        m_RuntimeMinValueFirstChanged = false;
                    }
                    else
                    {
                        m_RuntimeLastMinValue = minValue;
                        m_RuntimeMinValueChanged = true;
                        m_RuntimeMinValueUpdateTime = Time.time;
                    }
                    minValue = value;
                }
                else
                {
                    minValue = value;
                    m_RuntimeLastMinValue = value;
                    m_RuntimeMinValueUpdateTime = Time.time;
                    m_RuntimeMinValueChanged = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool UpdateMaxValue(double value, bool check)
        {
            if (value != maxValue)
            {
                if (check && Application.isPlaying)
                {
                    if (m_RuntimeMaxValueFirstChanged)
                    {
                        m_RuntimeMaxValueFirstChanged = false;
                    }
                    else
                    {
                        m_RuntimeLastMaxValue = maxValue;
                        m_RuntimeMaxValueChanged = true;
                        m_RuntimeMaxValueUpdateTime = Time.time;
                    }
                    maxValue = value;
                }
                else
                {
                    maxValue = value;
                    m_RuntimeLastMaxValue = value;
                    m_RuntimeMaxValueUpdateTime = Time.time;
                    m_RuntimeMaxValueChanged = false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public double GetCurrMinValue(float duration)
        {
            if (!Application.isPlaying || !m_RuntimeMinValueChanged)
                return minValue;

            if (minValue == 0 && maxValue == 0)
                return 0;

            var time = Time.time - m_RuntimeMinValueUpdateTime;
            if (time == 0)
                return minValue;

            var total = duration / 1000;
            if (duration > 0 && time <= total)
            {
                var curr = MathUtil.Lerp(m_RuntimeLastMinValue, minValue, time / total);
                return curr;
            }
            else
            {
                m_RuntimeMinValueChanged = false;
                return minValue;
            }
        }

        public double GetCurrMaxValue(float duration)
        {
            if (!Application.isPlaying || !m_RuntimeMaxValueChanged)
                return maxValue;

            if (minValue == 0 && maxValue == 0)
                return 0;

            var time = Time.time - m_RuntimeMaxValueUpdateTime;
            if (time == 0)
                return maxValue;

            var total = duration / 1000;
            if (duration > 0 && time < total)
            {
                var curr = MathUtil.Lerp(m_RuntimeLastMaxValue, maxValue, time / total);
                return curr;
            }
            else
            {
                m_RuntimeMaxValueChanged = false;
                return maxValue;
            }
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
                end = Mathf.CeilToInt(data.Count * dataZoom.end / 100);
                start = end - range;
                if (start < 0) start = 0;
            }
            else
            {
                start = Mathf.FloorToInt(data.Count * dataZoom.start / 100);
                end = start + range;
                if (end > data.Count) end = data.Count;
            }

            if (start != filterStart
                || end != filterEnd
                || dataZoom.minShowNum != filterMinShow
                || isNeedUpdateFilterData)
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
                    filterData = data.GetRange(start, range);
                }
                else
                {
                    filterData = data;
                }
            }
            else if (end == 0)
            {
                filterData = m_EmptyFliter;
            }
        }
    }
}