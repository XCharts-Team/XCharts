/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public class SerieDataContext
    {
        public Vector3 labelPosition { get; set; }
        /// <summary>
        /// 开始角度
        /// </summary>
        public float startAngle { get; internal set; }
        /// <summary>
        /// 结束角度
        /// </summary>
        public float toAngle { get; internal set; }
        /// <summary>
        /// 一半时的角度
        /// </summary>
        public float halfAngle { get; internal set; }
        /// <summary>
        /// 当前角度
        /// </summary>
        public float currentAngle { get; internal set; }
        /// <summary>
        /// 饼图数据项的内半径
        /// </summary>
        public float insideRadius { get; internal set; }
        /// <summary>
        /// 饼图数据项的偏移半径
        /// </summary>
        public float offsetRadius { get; internal set; }
        public float outsideRadius { get; set; }
        public Vector3 position { get; set; }
        /// <summary>
        /// 绘制区域。
        /// </summary>
        public Rect rect { get; set; }
        public Rect subRect { get; set; }
        public int level { get; set; }
        public SerieData parent { get; set; }
        public Color32 color { get; set; }
        public double area { get; set; }
        public float angle { get; set; }
        public Vector3 offsetCenter { get; set; }
        public float stackHeight { get; set; }

        public bool canShowLabel { get; set; }
        public Image symbol { get; set; }
        /// <summary>
        /// Whether the data item is highlighted.
        /// 该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlighted { get; set; }

    }
}