using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{

    public class LegendContext : MainComponentContext
    {
        /// <summary>
        /// 运行时图例的总宽度
        /// </summary>
        public float width { get; internal set; }
        /// <summary>
        /// 运行时图例的总高度
        /// </summary>
        public float height { get; internal set; }
        public Vector2 center { get; internal set; }
        /// <summary>
        /// the button list of legend.
        /// ||图例按钮列表。
        /// </summary>
        internal Dictionary<string, LegendItem> buttonList = new Dictionary<string, LegendItem>();
        /// <summary>
        /// 多列时每列的宽度
        /// </summary>
        internal Dictionary<int, float> eachWidthDict = new Dictionary<int, float>();
        /// <summary>
        /// 单列高度
        /// </summary>
        internal float eachHeight { get; set; }
        public Image background { get; set; }
    }
}