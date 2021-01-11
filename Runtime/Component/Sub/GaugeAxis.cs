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
    /// <summary>
    /// Settings related to gauge axis line.
    /// 仪表盘轴线相关设置。
    /// </summary>
    [System.Serializable]
    public class GaugeAxis : SubComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private GaugeAxisLine m_AxisLine = new GaugeAxisLine(true);
        [SerializeField] private GaugeAxisSplitLine m_SplitLine = new GaugeAxisSplitLine(true);
        [SerializeField] private GaugeAxisTick m_AxisTick = new GaugeAxisTick(true);
        [SerializeField] private SerieLabel m_AxisLabel = new SerieLabel();
        [SerializeField] private List<string> m_AxisLabelText = new List<string>();

        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// axis line style.
        /// 仪表盘轴线样式。
        /// </summary>
        public GaugeAxisLine axisLine { get { return m_AxisLine; } set { m_AxisLine = value; } }
        /// <summary>
        /// slit line style.
        /// 分割线。
        /// </summary>
        public GaugeAxisSplitLine splitLine { get { return m_SplitLine; } set { m_SplitLine = value; } }
        /// <summary>
        /// axis tick style.
        /// 刻度。
        /// </summary>
        public GaugeAxisTick axisTick { get { return m_AxisTick; } set { m_AxisTick = value; } }
        /// <summary>
        /// axis label style.
        /// 文本标签。
        /// </summary>
        public SerieLabel axisLabel { get { return m_AxisLabel; } set { m_AxisLabel = value; } }
        /// <summary>
        /// Coordinate axis scale label custom content. When the content is empty, 
        /// `axisLabel` automatically displays the content according to the scale; otherwise, 
        /// the content is taken from the list definition.
        /// 
        /// 自定义Label的内容。
        /// </summary>
        public List<string> axisLabelText { get { return m_AxisLabelText; } set { m_AxisLabelText = value; } }

        public List<float> runtimeStageAngle = new List<float>();
        public List<Vector3> runtimeLabelPosition = new List<Vector3>();
        private List<ChartLabel> m_RuntimeLabelList = new List<ChartLabel>();

        internal Color32 GetAxisLineColor(ChartTheme theme, int index)
        {
            var color = !ChartHelper.IsClearColor(axisLine.barColor) ? axisLine.barColor : theme.GetColor(index);
            ChartHelper.SetColorOpacity(ref color, axisLine.lineStyle.opacity);
            return color;
        }

        internal Color32 GetAxisLineBackgroundColor(ChartTheme theme, int index)
        {
            var color = !ChartHelper.IsClearColor(axisLine.barBackgroundColor)
                ? axisLine.barBackgroundColor : ChartConst.greyColor32;
            ChartHelper.SetColorOpacity(ref color, axisLine.lineStyle.opacity);
            return color;
        }

        internal Color32 GetSplitLineColor(Color32 themeColor, int serieIndex, float angle)
        {
            Color32 color;
            if (!ChartHelper.IsClearColor(splitLine.lineStyle.color))
            {
                color = splitLine.lineStyle.color;
                ChartHelper.SetColorOpacity(ref color, splitLine.lineStyle.opacity);
                return color;
            }
            color = themeColor;
            ChartHelper.SetColorOpacity(ref color, splitLine.lineStyle.opacity);
            return color;
        }

        internal Color32 GetAxisTickColor(Color32 themeColor, int serieIndex, float angle)
        {
            Color32 color;
            if (!ChartHelper.IsClearColor(axisTick.lineStyle.color))
            {
                color = axisTick.lineStyle.color;
                ChartHelper.SetColorOpacity(ref color, axisTick.lineStyle.opacity);
                return color;
            }
            color = themeColor;
            ChartHelper.SetColorOpacity(ref color, axisTick.lineStyle.opacity);
            return color;
        }

        internal Color32 GetPointerColor(ChartTheme theme, int serieIndex, float angle, ItemStyle itemStyle)
        {
            Color32 color;
            if (!ChartHelper.IsClearColor(itemStyle.color))
            {
                return itemStyle.GetColor();
            }
            for (int i = 0; i < runtimeStageAngle.Count; i++)
            {
                if (angle < runtimeStageAngle[i])
                {
                    color = axisLine.stageColor[i].color;
                    ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                    return color;
                }
            }
            color = theme.GetColor(serieIndex);
            ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
            return color;
        }

        public void ClearLabelObject()
        {
            m_RuntimeLabelList.Clear();
        }

        public void AddLabelObject(ChartLabel label)
        {
            m_RuntimeLabelList.Add(label);
        }

        public ChartLabel GetLabelObject(int index)
        {
            if (index >= 0 && index < m_RuntimeLabelList.Count)
            {
                return m_RuntimeLabelList[index];
            }
            return null;
        }

        public void SetLabelObjectPosition(int index, Vector3 pos)
        {
            if (index >= 0 && index < m_RuntimeLabelList.Count)
            {
                m_RuntimeLabelList[index].SetPosition(pos);
            }
        }

        public void SetLabelObjectText(int index, string text)
        {
            if (index >= 0 && index < m_RuntimeLabelList.Count)
            {
                m_RuntimeLabelList[index].SetText(text);
            }
        }

        public void SetLabelObjectActive(bool flag)
        {
            foreach (var label in m_RuntimeLabelList)
            {
                label.SetActive(flag);
            }
        }
    }
}