using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public class SerieDataContext
    {
        public Vector3 labelPosition;
        public Vector3 labelLinePosition;
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
        /// |该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlight
        {
            get { return m_Highligth; }
            set
            {
                m_Highligth = value;
            }
        }
        private bool m_Highligth;
        public bool selected;

        public void Reset()
        {
            canShowLabel = true;
            highlight = false;
            parent = null;
            symbol = null;
            rect = Rect.zero;
            subRect = Rect.zero;
            children.Clear();
            dataPoints.Clear();
            dataLabels.Clear();
        }
    }
}