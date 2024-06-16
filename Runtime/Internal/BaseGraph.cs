﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif

namespace XCharts.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    public partial class BaseGraph : MaskableGraphic, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IPointerClickHandler,
        IDragHandler, IEndDragHandler, IScrollHandler
    {
        [SerializeField] protected bool m_EnableTextMeshPro = false;

        protected Painter m_Painter;
        protected int m_SiblingIndex;

        protected float m_GraphWidth;
        protected float m_GraphHeight;
        protected float m_GraphX;
        protected float m_GraphY;
        protected Vector3 m_GraphPosition = Vector3.zero;
        protected Vector2 m_GraphMinAnchor;
        protected Vector2 m_GraphMaxAnchor;
        protected Vector2 m_GraphPivot;
        protected Vector2 m_GraphSizeDelta;
        protected Vector2 m_GraphAnchoredPosition;
        protected Rect m_GraphRect = new Rect(0, 0, 0, 0);
        protected bool m_RefreshChart = false;
        protected bool m_ForceOpenRaycastTarget;
        protected bool m_IsControlledByLayout = false;
        protected bool m_PainerDirty = false;
        protected bool m_IsOnValidate = false;
        protected Vector3 m_LastLocalPosition;
        internal PointerEventData pointerMoveEventData;
        internal PointerEventData pointerClickEventData;
        internal bool isTriggerOnClick = false;

        protected Action<PointerEventData, BaseGraph> m_OnPointerClick;
        protected Action<PointerEventData, BaseGraph> m_OnPointerDown;
        protected Action<PointerEventData, BaseGraph> m_OnPointerUp;
        protected Action<PointerEventData, BaseGraph> m_OnPointerEnter;
        protected Action<PointerEventData, BaseGraph> m_OnPointerExit;
        protected Action<PointerEventData, BaseGraph> m_OnBeginDrag;
        protected Action<PointerEventData, BaseGraph> m_OnDrag;
        protected Action<PointerEventData, BaseGraph> m_OnEndDrag;
        protected Action<PointerEventData, BaseGraph> m_OnScroll;

        public virtual HideFlags chartHideFlags { get { return HideFlags.None; } }

        private ScrollRect m_ScrollRect;

        public Painter painter { get { return m_Painter; } }

        protected virtual void InitComponent()
        {
            InitPainter();
        }

        protected override void Awake()
        {
            CheckTextMeshPro();
            m_SiblingIndex = 0;
            m_LastLocalPosition = transform.localPosition;
            UpdateSize();
            InitComponent();
            CheckIsInScrollRect();
        }

        protected override void Start()
        {
            m_RefreshChart = true;
        }

        protected virtual void Update()
        {
            CheckSize();
            if (m_IsOnValidate)
            {
                m_IsOnValidate = false;
                CheckTextMeshPro();
                InitComponent();
                RefreshGraph();
            }
            else
            {
                CheckComponent();
            }
            CheckPointerPos();
            CheckRefreshChart();
            CheckRefreshPainter();
        }

        protected virtual void SetAllComponentDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                m_IsOnValidate = true;
            }
#endif
            m_PainerDirty = true;
        }

        protected virtual void CheckComponent()
        {
            if (m_PainerDirty)
            {
                InitPainter();
                m_PainerDirty = false;
            }
        }

        private void CheckTextMeshPro()
        {
#if dUI_TextMeshPro
                var enableTextMeshPro = true;
#else
            var enableTextMeshPro = false;
#endif
            if (m_EnableTextMeshPro != enableTextMeshPro)
            {
                m_EnableTextMeshPro = enableTextMeshPro;
                RebuildChartObject();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            m_IsOnValidate = true;
        }
#endif

        protected override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        protected virtual void InitPainter()
        {
            m_Painter = ChartHelper.AddPainterObject("painter_b", transform, m_GraphMinAnchor,
                m_GraphMaxAnchor, m_GraphPivot, new Vector2(m_GraphWidth, m_GraphHeight), chartHideFlags, 1);
            m_Painter.type = Painter.Type.Base;
            m_Painter.onPopulateMesh = OnDrawPainterBase;
            m_Painter.transform.SetSiblingIndex(0);
        }

        private void CheckSize()
        {
            var currWidth = rectTransform.rect.width;
            var currHeight = rectTransform.rect.height;

            if (m_GraphWidth == 0 && m_GraphHeight == 0 && (currWidth != 0 || currHeight != 0))
            {
                Awake();
            }

            if (m_GraphWidth != currWidth ||
                m_GraphHeight != currHeight ||
                m_GraphMinAnchor != rectTransform.anchorMin ||
                m_GraphMaxAnchor != rectTransform.anchorMax ||
                m_GraphAnchoredPosition != rectTransform.anchoredPosition)
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
            m_GraphAnchoredPosition = rectTransform.anchoredPosition;

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
            if (canvas == null) return;
            if (pointerMoveEventData != null)
            {
                pointerPos = MousePos2ChartPos(pointerMoveEventData.position);
            }
        }

        private Vector2 MousePos2ChartPos(Vector2 mousePos)
        {
            Vector2 local;
            if (!ScreenPointToChartPoint(mousePos, out local))
            {
                return Vector2.zero;
            }
            else
            {
                return local;
            }
        }

        protected virtual void CheckIsInScrollRect()
        {
            m_ScrollRect = GetComponentInParent<ScrollRect>();
        }

        protected virtual void CheckRefreshChart()
        {
            if (m_RefreshChart && m_Painter != null)
            {
                m_Painter.Refresh();
                m_RefreshChart = false;
            }
        }

        protected virtual void CheckRefreshPainter()
        {
            if (m_Painter == null) return;
            m_Painter.CheckRefresh();
        }

        internal virtual void RefreshPainter(Painter painter)
        {
            if (painter == null) return;
            painter.Refresh();
        }

        protected virtual void OnSizeChanged()
        {
            m_RefreshChart = true;
        }

        protected virtual void OnLocalPositionChanged() { }

        protected virtual void OnDrawPainterBase(VertexHelper vh, Painter painter)
        {
            DrawPainterBase(vh);
        }

        protected virtual void DrawPainterBase(VertexHelper vh) { }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            pointerClickEventData = eventData;
            clickPos = MousePos2ChartPos(pointerClickEventData.position);
            if (m_OnPointerClick != null) m_OnPointerClick(eventData, this);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (m_OnPointerDown != null) m_OnPointerDown(eventData, this);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (m_OnPointerUp != null) m_OnPointerUp(eventData, this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            pointerMoveEventData = eventData;
            if (m_OnPointerEnter != null) m_OnPointerEnter(eventData, this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            pointerMoveEventData = null;
            pointerClickEventData = null;
            if (m_OnPointerExit != null) m_OnPointerExit(eventData, this);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (m_ScrollRect != null) m_ScrollRect.OnBeginDrag(eventData);
            if (m_OnBeginDrag != null) m_OnBeginDrag(eventData, this);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (m_ScrollRect != null) m_ScrollRect.OnEndDrag(eventData);
            if (m_OnEndDrag != null) m_OnEndDrag(eventData, this);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (m_ScrollRect != null) m_ScrollRect.OnDrag(eventData);
            if (m_OnDrag != null) m_OnDrag(eventData, this);
        }

        public virtual void OnScroll(PointerEventData eventData)
        {
            if (m_ScrollRect != null) m_ScrollRect.OnScroll(eventData);
            if (m_OnScroll != null) m_OnScroll(eventData, this);
        }
    }
}