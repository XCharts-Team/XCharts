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
    /// <summary>
    /// Settings related to gauge axis line.
    /// 仪表盘轴线相关设置。
    /// </summary>
    [System.Serializable]
    public class GaugeAxis : SubComponent
    {
        [System.Serializable]
        public class AxisLine
        {
            [System.Serializable]
            public class StageColor
            {
                [SerializeField] private float m_Percent;
                [SerializeField] private Color m_Color;
                /// <summary>
                /// 结束位置百分比。
                /// </summary>
                public float percent { get { return m_Percent; } set { m_Percent = value; } }
                /// <summary>
                /// 颜色。
                /// </summary>
                public Color color { get { return m_Color; } set { m_Color = value; } }

                public StageColor(float percent, Color color)
                {
                    m_Percent = percent;
                    m_Color = color;
                }
            }
            [SerializeField] private bool m_Show = true;
            [SerializeField] private float m_Width = 15f;
            [SerializeField] private float m_Opacity = 1f;
            [SerializeField] private Color m_BarColor;
            [SerializeField] private Color m_BarBackgroundColor = new Color32(200, 200, 200, 255);
            [SerializeField]
            private List<StageColor> m_StageColor = new List<StageColor>()
            {
                new StageColor(0.2f,new Color32(145,199,174,255)),
                new StageColor(0.8f,new Color32(99,134,158,255)),
                new StageColor(1.0f,new Color32(194,53,49,255)),
            };

            /// <summary>
            /// Set this to false to prevent the axis line from showing.
            /// 是否显示坐标轴轴线。
            /// </summary>
            public bool show { get { return m_Show; } set { m_Show = value; } }
            /// <summary>
            /// line style line width.
            /// 坐标轴线线宽。
            /// </summary>
            public float width { get { return m_Width; } set { m_Width = value; } }
            /// <summary>
            /// The opacity of axis line.
            /// 透明度。
            /// </summary>
            public float opacity { get { return m_Opacity; } set { m_Opacity = value; } }
            /// <summary>
            /// 进度条颜色。
            /// </summary>
            public Color barColor { get { return m_BarColor; } set { m_BarColor = value; } }
            /// <summary>
            /// 进度条背景颜色。
            /// </summary>
            public Color barBackgroundColor { get { return m_BarBackgroundColor; } set { m_BarBackgroundColor = value; } }
            /// <summary>
            /// 阶段颜色。
            /// </summary>
            public List<StageColor> stageColor { get { return m_StageColor; } set { m_StageColor = value; } }
        }
        /// <summary>
        /// 分割线
        /// </summary>
        [System.Serializable]
        public class SplitLine
        {
            [SerializeField] private bool m_Show = true;
            [SerializeField] private float m_Length = 15;
            [SerializeField]
            private LineStyle m_LineStyle = new LineStyle()
            {
                width = 1.5f,
                type = LineStyle.Type.Solid,
                color = new Color32(238, 238, 238, 255)
            };
            /// <summary>
            /// 是否显示分割线。
            /// </summary>
            public bool show { get { return m_Show; } set { m_Show = value; } }
            /// <summary>
            /// 分割线长度。
            /// </summary>
            public float length { get { return m_Length; } set { m_Length = value; } }
            /// <summary>
            /// 分割线样式。
            /// </summary>
            public LineStyle lineStyle { get { return m_LineStyle; } set { m_LineStyle = value; } }
        }
        /// <summary>
        /// 刻度
        /// </summary>
        [System.Serializable]
        public class AxisTick
        {
            [SerializeField] private bool m_Show = true;
            [SerializeField] private float m_Length = 5;
            [SerializeField] private float m_SplitNumber = 5;
            [SerializeField]
            private LineStyle m_LineStyle = new LineStyle()
            {
                width = 1f,
                type = LineStyle.Type.Solid,
                color = new Color32(238, 238, 238, 255)
            };
            /// <summary>
            /// 是否显示刻度。
            /// </summary>
            public bool show { get { return m_Show; } set { m_Show = value; } }
            /// <summary>
            /// 刻度长度。当为0-1的浮点数时表示半径的百分比。
            /// </summary>
            public float length { get { return m_Length; } set { m_Length = value; } }
            /// <summary>
            /// 分割线之间的分割段数。
            /// </summary>
            public float splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
            /// <summary>
            /// 刻度线样式。
            /// </summary>
            public LineStyle lineStyle { get { return m_LineStyle; } set { m_LineStyle = value; } }
        }
        [SerializeField] private bool m_Show = true;
        [SerializeField] private AxisLine m_AxisLine = new AxisLine();
        [SerializeField] private SplitLine m_SplitLine = new SplitLine();
        [SerializeField] private AxisTick m_AxisTick = new AxisTick();
        [SerializeField] private SerieLabel m_AxisLabel = new SerieLabel();
        [SerializeField] private List<string> m_AxisLabelText = new List<string>();

        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// 仪表盘轴线。
        /// </summary>
        public AxisLine axisLine { get { return m_AxisLine; } set { m_AxisLine = value; } }
        /// <summary>
        /// 分割线。
        /// </summary>
        public SplitLine splitLine { get { return m_SplitLine; } set { m_SplitLine = value; } }
        /// <summary>
        /// 刻度。
        /// </summary>
        public AxisTick axisTick { get { return m_AxisTick; } set { m_AxisTick = value; } }
        /// <summary>
        /// 文本标签。
        /// </summary>
        public SerieLabel axisLabel { get { return m_AxisLabel; } set { m_AxisLabel = value; } }
        /// <summary>
        /// 自定义Label的内容。
        /// </summary>
        public List<string> axisLabelText { get { return m_AxisLabelText; } set { m_AxisLabelText = value; } }

        public List<float> runtimeStageAngle = new List<float>();
        public List<Vector3> runtimeLabelPosition = new List<Vector3>();
        private List<LabelObject> m_RuntimeLabelList = new List<LabelObject>();

        internal Color GetAxisLineColor(ThemeInfo theme, int index)
        {
            var color = !ChartHelper.IsClearColor(axisLine.barColor) ? axisLine.barColor : (Color)theme.GetColor(index);
            color.a *= axisLine.opacity;
            return color;
        }

        internal Color GetAxisLineBackgroundColor(ThemeInfo theme, int index)
        {
            var color = !ChartHelper.IsClearColor(axisLine.barBackgroundColor) ? axisLine.barBackgroundColor : Color.grey;
            color.a *= axisLine.opacity;
            return color;
        }

        internal Color GetSplitLineColor(ThemeInfo theme, int serieIndex, float angle)
        {
            Color color;
            if (!ChartHelper.IsClearColor(splitLine.lineStyle.color))
            {
                color = splitLine.lineStyle.color;
                color.a *= splitLine.lineStyle.opacity;
                return color;
            }
            for (int i = 0; i < runtimeStageAngle.Count; i++)
            {
                if (angle < runtimeStageAngle[i])
                {
                    color = axisLine.stageColor[i].color;
                    color.a *= splitLine.lineStyle.opacity;
                    return color;
                }
            }
            color = theme.GetColor(serieIndex);
            color.a *= splitLine.lineStyle.opacity;
            return color;
        }

        internal Color GetAxisTickColor(ThemeInfo theme, int serieIndex, float angle)
        {
            Color color;
            if (!ChartHelper.IsClearColor(axisTick.lineStyle.color))
            {
                color = axisTick.lineStyle.color;
                color.a *= axisTick.lineStyle.opacity;
                return color;
            }
            for (int i = 0; i < runtimeStageAngle.Count; i++)
            {
                if (angle < runtimeStageAngle[i])
                {
                    color = axisLine.stageColor[i].color;
                    color.a *= axisTick.lineStyle.opacity;
                    return color;
                }
            }
            color = theme.GetColor(serieIndex);
            color.a *= axisTick.lineStyle.opacity;
            return color;
        }

        internal Color GetPointerColor(ThemeInfo theme, int serieIndex, float angle, ItemStyle itemStyle)
        {
            Color color;
            if (!ChartHelper.IsClearColor(itemStyle.color))
            {
                color = itemStyle.color;
                color.a *= itemStyle.opacity;
                return color;
            }
            for (int i = 0; i < runtimeStageAngle.Count; i++)
            {
                if (angle < runtimeStageAngle[i])
                {
                    color = axisLine.stageColor[i].color;
                    color.a *= itemStyle.opacity;
                    return color;
                }
            }
            color = theme.GetColor(serieIndex);
            color.a *= itemStyle.opacity;
            return color;
        }

        public void ClearLabelObject()
        {
            m_RuntimeLabelList.Clear();
        }

        public void AddLabelObject(LabelObject label)
        {
            m_RuntimeLabelList.Add(label);
        }

        public LabelObject GetLabelObject(int index)
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