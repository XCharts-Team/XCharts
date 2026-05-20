using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public struct PointInfo
    {
        public Vector3 position;
        public bool isIgnoreBreak;
        public double xValue;
        public double yValue;
        public double zValue;

        // public PointInfo(Vector3 pos, bool ignore)
        // {
        //     this.position = pos;
        //     this.isIgnoreBreak = ignore;
        // }

        public PointInfo(Vector3 pos, bool ignore, double x = 0, double y = 0, double z = 0)
        {
            this.position = pos;
            this.isIgnoreBreak = ignore;
            this.xValue = x;
            this.yValue = y;
            this.zValue = z;
        }
    }

    public class SerieContext
    {
        [System.NonSerialized] internal double[] cachedMin = new double[3] { double.MaxValue, double.MaxValue, double.MaxValue };
        [System.NonSerialized] internal double[] cachedMax = new double[3] { double.MinValue, double.MinValue, double.MinValue };
        [System.NonSerialized] internal bool[] cacheValid = new bool[3] { false, false, false };
        [System.NonSerialized] internal Dictionary<string, double[]> dataZoomMinMaxCache = new Dictionary<string, double[]>();

        internal void InvalidateMinMaxCache()
        {
            for (int i = 0; i < cacheValid.Length; i++)
                cacheValid[i] = false;
            cachedMin[0] = cachedMin[1] = cachedMin[2] = double.MaxValue;
            cachedMax[0] = cachedMax[1] = cachedMax[2] = double.MinValue;
            dataZoomMinMaxCache.Clear();
        }

        internal bool TryGetCachedMinMax(int dimension, out double minValue, out double maxValue)
        {
            minValue = 0; maxValue = 0;
            if (dimension < 0 || dimension > 2) return false;
            if (cacheValid[dimension])
            {
                minValue = cachedMin[dimension];
                maxValue = cachedMax[dimension];
                return true;
            }
            return false;
        }

        internal void SetCachedMinMax(int dimension, double minValue, double maxValue)
        {
            if (dimension < 0 || dimension > 2) return;
            cachedMin[dimension] = minValue;
            cachedMax[dimension] = maxValue;
            cacheValid[dimension] = true;
        }

        internal bool TryGetDataZoomCachedMinMax(string key, int dimension, out double minValue, out double maxValue)
        {
            minValue = 0; maxValue = 0;
            if (string.IsNullOrEmpty(key)) return false;
            double[] arr;
            if (!dataZoomMinMaxCache.TryGetValue(key, out arr) || arr == null || arr.Length < 6) return false;
            int mi = dimension * 2;
            minValue = arr[mi];
            maxValue = arr[mi + 1];
            return true;
        }

        internal void SetDataZoomCachedMinMax(string key, int dimension, double minValue, double maxValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            double[] arr;
            if (!dataZoomMinMaxCache.TryGetValue(key, out arr) || arr == null || arr.Length < 6)
            {
                arr = new double[6];
                for (int i = 0; i < 6; i++) arr[i] = 0;
                dataZoomMinMaxCache[key] = arr;
            }
            int mi = dimension * 2;
            arr[mi] = minValue;
            arr[mi + 1] = maxValue;
        }
        /// <summary>
        /// 鼠标是否进入serie
        /// </summary>
        public bool pointerEnter;
        /// <summary>
        /// 鼠标当前指示的数据项索引（单个）
        /// </summary>
        public int pointerItemDataIndex = -1;
        /// <summary>
        /// 鼠标当前指示的数据项维度
        /// </summary>
        public int pointerItemDataDimension = 1;
        /// <summary>
        /// 鼠标所在轴线上的数据项索引（可能有多个）
        /// </summary>
        public List<int> pointerAxisDataIndexs = new List<int>();
        public bool isTriggerByAxis = false;
        public int dataZoomStartIndex = 0;
        public int dataZoomStartIndexOffset = 0;

        /// <summary>
        /// 中心点
        /// </summary>
        public Vector3 center;
        /// <summary>
        /// 线段终点
        /// </summary>
        public Vector3 lineEndPostion;
        public double lineEndValueX;
        public double lineEndValueY;
        public double lineEndValueZ;
        /// <summary>
        /// 内半径
        /// </summary>
        public float insideRadius;
        /// <summary>
        /// 外半径
        /// </summary>
        public float outsideRadius;
        public float startAngle;
        /// <summary>
        /// 最大值
        /// </summary>
        public double dataMax;
        /// <summary>
        /// 最小值
        /// </summary>
        public double dataMin;
        public double checkValue;
        /// <summary>
        /// 左下角坐标X
        /// </summary>
        public float x;
        /// <summary>
        /// 左下角坐标Y
        /// </summary>
        public float y;
        /// <summary>
        /// 宽
        /// </summary>
        public float width;
        /// <summary>
        /// 高
        /// </summary>
        public float height;
        /// <summary>
        /// 矩形区域
        /// </summary>
        public Rect rect;
        /// <summary>
        /// 绘制顶点数
        /// </summary>
        public int vertCount;
        /// <summary>
        /// theme的颜色索引
        /// </summary>
        public int colorIndex;
        /// <summary>
        /// 数据对应的位置坐标。
        /// </summary>
        public List<Vector3> dataPoints = new List<Vector3>();
        /// <summary>
        /// 数据对应的位置坐标是否忽略（忽略时连线是透明的），dataIgnore 和 dataPoints 一一对应。
        /// </summary>
        public List<bool> dataIgnores = new List<bool>();
        /// <summary>
        /// 数据对应的index索引。dataIndexs 和 dataPoints 一一对应。
        /// </summary>
        public List<int> dataIndexs = new List<int>();
        /// <summary>
        /// 排序后的数据
        /// </summary>
        public List<SerieData> sortedData = new List<SerieData>();
        public List<SerieData> rootData = new List<SerieData>();
        /// <summary>
        /// 绘制点
        /// </summary>
        public List<PointInfo> drawPoints = new List<PointInfo>();
        public SerieParams param = new SerieParams();
        public ChartLabel titleObject { get; set; }

        public Tooltip.Type tooltipType;
        public Tooltip.Trigger tooltipTrigger;
        public int totalDataIndex;
        public int clickTotalDataIndex;
        /// <summary>
        /// 水平方向的
        /// </summary>
        public bool isHorizontal;
    }
}