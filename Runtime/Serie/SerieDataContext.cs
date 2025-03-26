using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public class SerieDataContext
    {
        public Vector3 labelPosition;
        public Vector3 labelLinePosition;
        public Vector3 labelLinePosition2;
        /// <summary>
        /// 开始角度
        /// </summary>
        public float startAngle;
        /// <summary>
        /// 结束角度
        /// </summary>
        public float toAngle;
        /// <summary>
        /// 一半时的角度
        /// </summary>
        public float halfAngle;
        /// <summary>
        /// 当前角度
        /// </summary>
        public float currentAngle;
        /// <summary>
        /// 饼图数据项的内半径
        /// </summary>
        public float insideRadius;
        /// <summary>
        /// 饼图数据项的偏移半径
        /// </summary>
        public float offsetRadius;
        public float outsideRadius;
        public Vector3 position;
        /// <summary>
        /// is the exchange animation end.
        /// ||交换动画是否结束。
        /// </summary> 
        public bool exchangeEnd;
        /// <summary>
        /// the current position of the exchange animation.
        /// ||交换动画的当前位置。
        /// </summary>
        public Vector3 exchangePosition;
        private float exchangeStartTime;
        private Vector3 exchangeStartPosition;
        private Vector3 exchangeEndPosition;
        public List<Vector3> dataPoints = new List<Vector3>();
        public List<ChartLabel> dataLabels = new List<ChartLabel>();
        public List<SerieData> children = new List<SerieData>();
        /// <summary>
        /// 绘制区域。
        /// </summary>
        public Rect rect;
        public Rect backgroundRect;
        public Rect subRect;
        public int level;
        public SerieData parent;
        public Color32 color;
        public double area;
        public float angle;
        public Vector3 offsetCenter;
        public Vector3 areaCenter;
        public float stackHeight;
        public bool isClip;
        public bool canShowLabel = true;
        public Image symbol;
        /// <summary>
        /// Whether the data item is highlighted.
        /// ||该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlight;
        public bool selected;
        /// <summary>
        /// the id of the node in the graph.
        /// ||图中节点的id。
        /// </summary> 
        public string graphNodeId;
        public double inTotalValue;
        public double outTotalValue;

        public void Reset()
        {
            canShowLabel = true;
            highlight = false;
            parent = null;
            symbol = null;
            rect = Rect.zero;
            subRect = Rect.zero;
            exchangeEnd = true;
            exchangeStartPosition = Vector3.zero;
            exchangePosition = Vector3.zero;
            exchangeEndPosition = Vector3.zero;
            children.Clear();
            dataPoints.Clear();
            dataLabels.Clear();
        }

        public void UpdateExchangePosition(ref float x, ref float y, float totalTime)
        {
            if (exchangeEndPosition.x != x || exchangeEndPosition.y != y)
            {
                if (exchangeStartPosition == Vector3.zero || Time.time - exchangeStartTime < 0.1f)
                {
                    exchangeEnd = true;
                    exchangeStartTime = Time.time;
                    exchangeEndPosition.x = x;
                    exchangeEndPosition.y = y;
                    exchangeStartPosition = exchangeEndPosition;
                    exchangePosition = exchangeEndPosition;
                    return;
                }
                else
                {
                    exchangeEnd = false;
                    exchangeStartTime = Time.time;
                    exchangeStartPosition = exchangePosition;
                    exchangeEndPosition.x = x;
                    exchangeEndPosition.y = y;
                }
            }
            if (exchangeStartPosition == exchangeEndPosition)
            {
                exchangeEnd = true;
                exchangePosition = exchangeEndPosition;
                x = exchangePosition.x;
                y = exchangePosition.y;
                return;
            }
            var spendTime = Time.time - exchangeStartTime;
            totalTime /= 1000;
            if (spendTime >= totalTime)
            {
                exchangeEnd = true;
                exchangeStartPosition = exchangeEndPosition;
                exchangePosition = exchangeEndPosition;
                x = exchangePosition.x;
                y = exchangePosition.y;
                return;
            }
            exchangePosition = Vector3.Lerp(exchangeStartPosition, exchangeEndPosition, spendTime / totalTime);
            x = exchangePosition.x;
            y = exchangePosition.y;
            return;
        }
    }
}