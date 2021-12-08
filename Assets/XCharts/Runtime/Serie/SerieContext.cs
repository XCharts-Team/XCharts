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
    public struct PointInfo
    {
        public Vector3 position;
        public bool isIgnoreBreak;

        public PointInfo(Vector3 pos, bool ignore)
        {
            this.position = pos;
            this.isIgnoreBreak = ignore;
        }
    }
    [System.Serializable]
    public class SerieContext
    {
        /// <summary>
        /// 鼠标是否进入serie
        /// </summary>
        public bool pointerEnter;
        /// <summary>
        /// 鼠标当前指示的数据项索引（单个）
        /// </summary>
        public int pointerItemDataIndex = -1;
        /// <summary>
        /// 鼠标所在轴线上的数据项索引（可能有多个）
        /// </summary>
        public List<int> pointerAxisDataIndexs = new List<int>();

        /// <summary>
        /// 中心点
        /// </summary>
        public Vector3 center { get; internal set; }
        /// <summary>
        /// 内半径
        /// </summary>
        public float insideRadius { get; internal set; }
        /// <summary>
        /// 外半径
        /// </summary>
        public float outsideRadius { get; internal set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public double dataMax { get; internal set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public double dataMin { get; internal set; }
        public double checkValue { get; set; }
        /// <summary>
        /// 左下角坐标X
        /// </summary>
        public float x { get; internal set; }
        /// <summary>
        /// 左下角坐标Y
        /// </summary>
        public float y { get; internal set; }
        /// <summary>
        /// 宽
        /// </summary>
        public float width { get; internal set; }
        /// <summary>
        /// 高
        /// </summary>
        public float height { get; internal set; }
        /// <summary>
        /// 矩形区域
        /// </summary>
        public Rect rect { get; internal set; }
        /// <summary>
        /// 绘制顶点数
        /// </summary>
        public int vertCount { get; internal set; }
        /// <summary>
        /// 数据对应的位置坐标。
        /// </summary>
        public List<Vector3> dataPoints = new List<Vector3>();
        /// <summary>
        /// 数据对应的位置坐标是否忽略（忽略时连线是透明的），dataIgnore 和 dataPoints 一一对应。
        /// </summary>
        public List<bool> dataIgnore = new List<bool>();
        /// <summary>
        /// 排序后的数据
        /// </summary>
        public List<SerieData> sortedData = new List<SerieData>();
        /// <summary>
        /// theme的颜色索引
        /// </summary>
        internal int colorIndex;
        /// <summary>
        /// 绘制点
        /// </summary>
        internal List<PointInfo> drawPoints = new List<PointInfo>();
    }
}