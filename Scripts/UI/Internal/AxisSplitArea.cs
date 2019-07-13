using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Split area of axis in grid area, not shown by default.
    /// </summary>
    [Serializable]
    public class AxisSplitArea
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private List<Color> m_Color;

        /// <summary>
        /// Set this to true to show the splitArea.
        /// </summary>
        /// <value>false</value>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Color of split area. SplitArea color could also be set in color array,
        /// which the split lines would take as their colors in turns. 
        /// Dark and light colors in turns are used by default.
        /// </summary>
        /// <value>['rgba(250,250,250,0.3)','rgba(200,200,200,0.3)']</value>
        public List<Color> color { get { return m_Color; } set { m_Color = value; } }

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

        public Color getColor(int index)
        {
            var i = index % color.Count;
            return color[i];
        }
    }
}