using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XCharts.Runtime
{
    /// <summary>
    /// The base class of all graphs or components.
    /// |所有图形的基类。
    /// </summary>
    public partial class BaseGraph
    {
        /// <summary>
        /// The x of graph.
        /// |图形的X
        /// </summary>
        public float graphX { get { return m_GraphX; } }
        /// <summary>
        /// The y of graph.
        /// |图形的Y
        /// </summary>
        public float graphY { get { return m_GraphY; } }
        /// <summary>
        /// The width of graph.
        /// |图形的宽
        /// </summary>
        public float graphWidth { get { return m_GraphWidth; } }
        /// <summary>
        /// The height of graph.
        /// |图形的高
        /// </summary>
        public float graphHeight { get { return m_GraphHeight; } }
        /// <summary>
        /// The position of graph.
        /// |图形的左下角起始坐标。
        /// </summary>
        public Vector3 graphPosition { get { return m_GraphPosition; } }
        public Rect graphRect { get { return m_GraphRect; } }
        public Vector2 graphSizeDelta { get { return m_GraphSizeDelta; } }
        public Vector2 graphPivot { get { return m_GraphPivot; } }
        public Vector2 graphMinAnchor { get { return m_GraphMinAnchor; } }
        public Vector2 graphMaxAnchor { get { return m_GraphMaxAnchor; } }
        public Vector2 graphAnchoredPosition { get { return m_GraphAnchoredPosition; } }
        /// <summary>
        /// The postion of pointer.
        /// |鼠标位置。
        /// </summary>
        public Vector2 pointerPos { get; protected set; }
        /// <summary>
        /// Whether the mouse pointer is in the chart.
        /// |鼠标是否在图表内。
        /// </summary>
        public bool isPointerInChart
        { get { return m_PointerEventData != null; } }
        /// <summary>
        /// 警告信息。
        /// </summary>
        public string warningInfo { get; protected set; }
        /// <summary>
        /// 强制开启鼠标事件检测。
        /// </summary>
        public bool forceOpenRaycastTarget { get { return m_ForceOpenRaycastTarget; } set { m_ForceOpenRaycastTarget = value; } }
        /// <summary>
        /// 鼠标点击回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onPointerClick { set { m_OnPointerClick = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标按下回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onPointerDown { set { m_OnPointerDown = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标弹起回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onPointerUp { set { m_OnPointerUp = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标进入回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onPointerEnter { set { m_OnPointerEnter = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标退出回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onPointerExit { set { m_OnPointerExit = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标开始拖拽回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onBeginDrag { set { m_OnBeginDrag = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标拖拽回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onDrag { set { m_OnDrag = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标结束拖拽回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onEndDrag { set { m_OnEndDrag = value; m_ForceOpenRaycastTarget = true; } }
        /// <summary>
        /// 鼠标滚动回调。
        /// </summary>
        public Action<PointerEventData, BaseGraph> onScroll { set { m_OnScroll = value; m_ForceOpenRaycastTarget = true; } }

        /// <summary>
        /// 设置图形的宽高（在非stretch pivot下才有效，其他情况需要自己调整RectTransform）
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void SetSize(float width, float height)
        {
            if (LayerHelper.IsFixedWidthHeight(rectTransform))
            {
                rectTransform.sizeDelta = new Vector2(width, height);
            }
            else
            {
                Debug.LogError("Can't set size on stretch pivot,you need to modify rectTransform by yourself.");
            }
        }

        /// <summary>
        /// 重新初始化Painter
        /// </summary>
        public void SetPainterDirty()
        {
            m_PainerDirty = true;
        }

        /// <summary>
        /// Redraw graph in next frame.
        /// |在下一帧刷新图形。
        /// </summary>
        public virtual void RefreshGraph()
        {
            m_RefreshChart = true;
        }

        public void RefreshAllComponent()
        {
            SetAllComponentDirty();
            RefreshGraph();
        }

        /// <summary>
        /// 检测警告信息。
        /// </summary>
        /// <returns></returns>
        public string CheckWarning()
        {
            warningInfo = CheckHelper.CheckChart(this);
            return warningInfo;
        }

        /// <summary>
        /// 移除并重新创建所有图表的Object。
        /// </summary>
        public void RebuildChartObject()
        {
            ChartHelper.DestroyAllChildren(transform);
            SetAllComponentDirty();
        }

        public bool ScreenPointToChartPoint(Vector2 screenPoint, out Vector2 chartPoint)
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            var relative = Display.RelativeMouseAt(screenPoint);
            if (relative != Vector3.zero)
                screenPoint = relative;
#endif
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                    screenPoint, cam, out chartPoint))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存图表为图片。
        /// </summary>
        /// <param name="imageType">type of image: png, jpg, exr</param>
        /// <param name="savePath">save path</param>
        public void SaveAsImage(string imageType = "png", string savePath = "")
        {
            StartCoroutine(SaveAsImageSync(imageType, savePath));
        }

        private IEnumerator SaveAsImageSync(string imageType, string path)
        {
            yield return new WaitForEndOfFrame();
            ChartHelper.SaveAsImage(rectTransform, canvas, imageType, path);
        }
    }
}