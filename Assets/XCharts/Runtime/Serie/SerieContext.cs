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

        public int vertCount;

        internal int colorIndex;
        internal List<PointInfo> drawPoints = new List<PointInfo>();
    }
}