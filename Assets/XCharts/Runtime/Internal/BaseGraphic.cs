/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace XCharts
{
    public partial class BaseGraph : MaskableGraphic, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IPointerClickHandler,
        IDragHandler, IEndDragHandler, IScrollHandler
    {
        [SerializeField] protected bool m_DebugMode = false;

        protected float m_GraphWidth;
        protected float m_GraphHeight;
        protected float m_GraphX;
        protected float m_GraphY;
        protected Vector3 m_GraphPosition = Vector3.zero;
        protected Vector2 m_GraphMinAnchor;
        protected Vector2 m_GraphMaxAnchor;
        protected Vector2 m_GraphPivot;
        protected Vector2 m_GraphSizeDelta;
        protected Rect m_GraphRect = new Rect(0, 0, 0, 0);
        protected bool m_RefreshChart = false;
        protected bool m_ForceOpenRaycastTarget;
        protected bool m_IsControlledByLayout = false;
        protected Vector3 m_LastLocalPosition;

        protected Action<BaseGraph, PointerEventData> m_OnPointerClick;
        protected Action<BaseGraph, PointerEventData> m_OnPointerDown;
        protected Action<BaseGraph, PointerEventData> m_OnPointerUp;
        protected Action<BaseGraph, PointerEventData> m_OnPointerEnter;
        protected Action<BaseGraph, PointerEventData> m_OnPointerExit;
        protected Action<BaseGraph, PointerEventData> m_OnBeginDrag;
        protected Action<BaseGraph, PointerEventData> m_OnDrag;
        protected Action<BaseGraph, PointerEventData> m_OnEndDrag;
        protected Action<BaseGraph, PointerEventData> m_OnScroll;

        protected Vector2 chartAnchorMax { get { return m_GraphMinAnchor; } }
        protected Vector2 chartAnchorMin { get { return m_GraphMaxAnchor; } }
        protected Vector2 chartPivot { get { return m_GraphPivot; } }
        protected HideFlags chartHideFlags { get { return m_DebugMode ? HideFlags.None : HideFlags.HideInHierarchy; } }


        protected virtual void InitComponent()
        {
        }

        protected override void Awake()
        {
            if (transform.parent != null)
            {
                m_IsControlledByLayout = transform.parent.GetComponent<LayoutGroup>() != null;
            }
            raycastTarget = false;
            m_LastLocalPosition = transform.localPosition;
            UpdateSize();
            InitComponent();
        }

        protected override void Start()
        {
            m_RefreshChart = true;
        }

        protected virtual void Update()
        {
            CheckSize();
            CheckComponent();
            CheckPointerPos();
            CheckRefreshChart();
        }

        protected virtual void CheckComponent()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
        }

        protected override void OnValidate()
        {
        }
#endif

        protected override void OnDestroy()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private void CheckSize()
        {
            var currWidth = rectTransform.rect.width;
            var currHeight = rectTransform.rect.height;

            if (m_GraphWidth == 0 && m_GraphHeight == 0 && (currWidth != 0 || currHeight != 0))
            {
                Awake();
            }

            if (m_GraphWidth != currWidth || m_GraphHeight != currHeight ||
                m_GraphMinAnchor != rectTransform.anchorMin || m_GraphMaxAnchor != rectTransform.anchorMax)
            {
                UpdateSize();
            }
            if (!ChartHelper.IsValueEqualsVector3(m_LastLocalPosition, transform.localPosition))
            {
                m_LastLocalPosition = transform.localPosition;
                OnLocalPositionChanged();
            }
        }

        protected void UpdateSize()
        {
            m_GraphWidth = rectTransform.rect.width;
            m_GraphHeight = rectTransform.rect.height;

            m_GraphMaxAnchor = rectTransform.anchorMax;
            m_GraphMinAnchor = rectTransform.anchorMin;
            m_GraphSizeDelta = rectTransform.sizeDelta;

            rectTransform.pivot = LayerHelper.ResetChartPositionAndPivot(m_GraphMinAnchor, m_GraphMaxAnchor,
               m_GraphWidth, m_GraphHeight, ref m_GraphX, ref m_GraphY);
            m_GraphPivot = rectTransform.pivot;

            m_GraphRect.x = m_GraphX;
            m_GraphRect.y = m_GraphY;
            m_GraphRect.width = m_GraphWidth;
            m_GraphRect.height = m_GraphHeight;
            m_GraphPosition.x = m_GraphX;
            m_GraphPosition.y = m_GraphY;

            OnSizeChanged();
        }

        private void CheckPointerPos()
        {
            if (m_ForceOpenRaycastTarget) raycastTarget = true;
            if (IsNeedCheckPointerPos())
            {
                if (canvas == null) return;
                Vector2 local;
                var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                    Input.mousePosition, cam, out local))
                {
                    pointerPos = Vector2.zero;
                }
                else
                {
                    pointerPos = local;
                }
            }
        }

        protected virtual bool IsNeedCheckPointerPos()
        {
            return raycastTarget;
        }

        protected virtual void CheckRefreshChart()
        {
            if (m_RefreshChart)
            {
                SetVerticesDirty();
                m_RefreshChart = false;
            }
        }

        protected virtual void OnSizeChanged()
        {
            m_RefreshChart = true;
        }

        protected virtual void OnLocalPositionChanged()
        {
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            DrawBackground(vh);
            DrawGraphic(vh);
        }

        protected virtual void DrawGraphic(VertexHelper vh)
        {
        }

        protected virtual void DrawBackground(VertexHelper vh)
        {
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (m_OnPointerClick != null) m_OnPointerClick(this, eventData);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (m_OnPointerDown != null) m_OnPointerDown(this, eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (m_OnPointerUp != null) m_OnPointerUp(this, eventData);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (m_OnPointerEnter != null) m_OnPointerEnter(this, eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (m_OnPointerExit != null) m_OnPointerExit(this, eventData);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (m_OnBeginDrag != null) m_OnBeginDrag(this, eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (m_OnEndDrag != null) m_OnEndDrag(this, eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (m_OnDrag != null) m_OnDrag(this, eventData);
        }

        public virtual void OnScroll(PointerEventData eventData)
        {
            if (m_OnScroll != null) m_OnScroll(this, eventData);
        }
    }
}
