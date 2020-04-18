/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Split area of axis in grid area, not shown by default.
    /// 坐标轴在 grid 区域中的分隔区域，默认不显示。
    /// </summary>
    [Serializable]
    public class AxisSplitArea : SubComponent
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private List<Color> m_Color;

        /// <summary>
        /// Set this to true to show the splitArea.
        /// 是否显示分隔区域。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Color of split area. SplitArea color could also be set in color array,
        /// which the split lines would take as their colors in turns. 
        /// Dark and light colors in turns are used by default.
        /// 分隔区域颜色。分隔区域会按数组中颜色的顺序依次循环设置颜色。默认是一个深浅的间隔色。
        /// </summary>
        public List<Color> color
        {
            get { return m_Color; }
            set { if (value != null) { m_Color = value; SetVerticesDirty(); } }
        }

        public static AxisSplitArea defaultSplitArea
        {
            get
            {
                return new AxisSplitArea()
                {
                    m_Show = false,
                    m_Color = new List<Color>(){
                            new Color32(250,250,250,77),
                            new Color32(200,200,200,77)
                        }
                };
            }
        }

        public AxisSplitArea Clone()
        {
            var axisSplitArea = new AxisSplitArea();
            axisSplitArea.show = show;
            axisSplitArea.color = new List<Color>();
            ChartHelper.CopyList(axisSplitArea.color, color);
            return axisSplitArea;
        }

        public void Copy(AxisSplitArea splitArea)
        {
            show = splitArea.show;
            color.Clear();
            ChartHelper.CopyList(color, splitArea.color);
        }

        public Color getColor(int index)
        {
            var i = index % color.Count;
            return color[i];
        }
    }
}