using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

namespace XCharts
{
    public enum Orient
    {
        Horizonal,
        Vertical
    }

    public class BaseChart : MaskableGraphic, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IScrollHandler
    {
        private static readonly string s_TitleObjectName = "title";
        private static readonly string s_LegendObjectName = "legend";

        [SerializeField] protected Theme m_Theme = Theme.Default;
        [SerializeField] protected ThemeInfo m_ThemeInfo;
        [SerializeField] protected Title m_Title = Title.defaultTitle;
        [SerializeField] protected Legend m_Legend = Legend.defaultLegend;
        [SerializeField] protected Tooltip m_Tooltip = Tooltip.defaultTooltip;
        [SerializeField] protected Series m_Series;

        [SerializeField] protected bool m_Large;
        [SerializeField] protected int m_MinShowDataNumber;
        [SerializeField] protected int m_MaxShowDataNumber;
        [SerializeField] protected int m_MaxCacheDataNumber;

        [NonSerialized] private Theme m_CheckTheme = 0;
        [NonSerialized] private Title m_CheckTitle = Title.defaultTitle;
        [NonSerialized] private Legend m_CheckLegend = Legend.defaultLegend;
        [NonSerialized] private float m_CheckWidth = 0;
        [NonSerialized] private float m_CheckHeight = 0;
        [NonSerialized] private bool m_RefreshChart = false;
        [NonSerialized] protected List<Text> m_LegendTextList = new List<Text>();

        protected float chartWidth { get { return rectTransform.sizeDelta.x; } }
        protected float chartHeight { get { return rectTransform.sizeDelta.y; } }
        protected Vector2 chartAnchorMax { get { return rectTransform.anchorMax; } }
        protected Vector2 chartAnchorMin { get { return rectTransform.anchorMin; } }
        protected Vector2 chartPivot { get { return rectTransform.pivot; } }

        public Title title { get { return m_Title; } }
        public Legend legend { get { return m_Legend; } }
        public Tooltip tooltip { get { return m_Tooltip; } }
        public Series series { get { return m_Series; } }

        public bool large { get { return m_Large; } set { m_Large = value; } }
        public int minShowDataNumber
        {
            get { return m_MinShowDataNumber; }
            set { m_MinShowDataNumber = value; if (m_MinShowDataNumber < 0) m_MinShowDataNumber = 0; }
        }
        public int maxShowDataNumber
        {
            get { return m_MaxShowDataNumber; }
            set { m_MaxShowDataNumber = value; if (m_MaxShowDataNumber < 0) m_MaxShowDataNumber = 0; }
        }
        public int maxCacheDataNumber
        {
            get { return m_MaxCacheDataNumber; }
            set { m_MaxCacheDataNumber = value; if (m_MaxCacheDataNumber < 0) m_MaxCacheDataNumber = 0; }
        }

        protected override void Awake()
        {
            if (m_ThemeInfo == null)
            {
                m_ThemeInfo = ThemeInfo.Default;
            }
            raycastTarget = false;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.pivot = Vector2.zero;
            m_CheckWidth = chartWidth;
            m_CheckHeight = chartHeight;
            m_CheckTheme = m_Theme;
            InitTitle();
            InitLegend();
            InitTooltip();
        }

        protected virtual void Update()
        {
            CheckSize();
            CheckTheme();
            CheckTile();
            CheckLegend();
            CheckTooltip();
            CheckRefreshChart();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            ChartHelper.DestoryAllChilds(transform);
            m_ThemeInfo = ThemeInfo.Default;
            m_Title = Title.defaultTitle;
            m_Legend = Legend.defaultLegend;
            m_Tooltip = Tooltip.defaultTooltip;
            m_Series = Series.defaultSeries;
            Awake();
        }
#endif

        protected override void OnDestroy()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public void ClearData()
        {
            m_Series.ClearData();
            m_Legend.ClearData();
        }

        public virtual void AddData(string legend, float value)
        {
            m_Legend.AddData(legend);
            m_Series.AddData(legend, value, m_MaxCacheDataNumber);
            RefreshChart();
        }

        public virtual void AddData(int legend, float value)
        {
            m_Series.AddData(legend, value, m_MaxCacheDataNumber);
        }

        public virtual void UpdateData(string legend, float value, int dataIndex = 0)
        {
            m_Series.UpdateData(legend, value, dataIndex);
            RefreshChart();
        }

        public virtual void UpdateData(int legendIndex, float value, int dataIndex = 0)
        {
            m_Series.UpdateData(legendIndex, value, dataIndex);
            RefreshChart();
        }

        public virtual void SetActive(string legend, bool active)
        {
            m_Legend.SetActive(legend, active);
            m_Series.SetActive(legend, active);
        }

        public virtual void SetActive(int index, bool active)
        {
            m_Legend.SetActive(index, active);
            m_Series.SetActive(index, active);
        }

        public virtual bool IsActive(string name)
        {
            return m_Legend.IsActive(name) || m_Series.IsActive(name);
        }

        public virtual bool IsActive(int index)
        {
            return m_Legend.IsActive(index) || m_Series.IsActive(index);
        }

        public virtual void RemoveData(string legend)
        {
            m_Legend.RemoveData(legend);
            m_Series.RemoveData(legend);
            RefreshChart();
        }

        public void UpdateTheme(Theme theme)
        {
            this.m_Theme = theme;
            OnThemeChanged();
            SetAllDirty();
        }

        private void InitTitle()
        {
            m_Title.OnChanged();
            TextAnchor anchor = m_Title.location.textAnchor;
            Vector2 anchorMin = m_Title.location.anchorMin;
            Vector2 anchorMax = m_Title.location.anchorMax;
            Vector2 pivot = m_Title.location.pivot;
            Vector3 titlePosition = m_Title.location.GetPosition(chartWidth, chartHeight);
            Vector3 subTitlePosition = -new Vector3(0, m_Title.textFontSize + m_Title.itemGap, 0);
            float titleWid = chartWidth;

            var titleObject = ChartHelper.AddObject(s_TitleObjectName, transform, anchorMin, anchorMax,
                pivot, new Vector2(chartWidth, chartHeight));
            titleObject.transform.localPosition = titlePosition;
            ChartHelper.HideAllObject(titleObject, s_TitleObjectName);

            Text titleText = ChartHelper.AddTextObject(s_TitleObjectName, titleObject.transform,
                        m_ThemeInfo.font, m_ThemeInfo.textColor, anchor, anchorMin, anchorMax, pivot,
                        new Vector2(titleWid, m_Title.textFontSize), m_Title.textFontSize);

            titleText.alignment = anchor;
            titleText.gameObject.SetActive(m_Title.show);
            titleText.transform.localPosition = Vector2.zero;
            titleText.text = m_Title.text;

            Text subText = ChartHelper.AddTextObject(s_TitleObjectName + "_sub", titleObject.transform,
                        m_ThemeInfo.font, m_ThemeInfo.textColor, anchor, anchorMin, anchorMax, pivot,
                        new Vector2(titleWid, m_Title.subTextFontSize), m_Title.subTextFontSize);

            subText.alignment = anchor;
            subText.gameObject.SetActive(m_Title.show && !string.IsNullOrEmpty(m_Title.subText));
            subText.transform.localPosition = subTitlePosition;
            subText.text = m_Title.subText;
        }

        private void InitLegend()
        {
            m_Legend.OnChanged();
            ChartHelper.HideAllObject(transform, s_LegendObjectName);
            TextAnchor anchor = m_Legend.location.textAnchor;
            Vector2 anchorMin = m_Legend.location.anchorMin;
            Vector2 anchorMax = m_Legend.location.anchorMax;
            Vector2 pivot = m_Legend.location.pivot;

            var legendObject = ChartHelper.AddObject(s_LegendObjectName, transform, anchorMin, anchorMax,
                pivot, new Vector2(chartWidth, chartHeight));
            legendObject.transform.localPosition = m_Legend.location.GetPosition(chartWidth, chartHeight);
            ChartHelper.HideAllObject(legendObject, s_LegendObjectName);

            for (int i = 0; i < m_Legend.data.Count; i++)
            {
                Button btn = ChartHelper.AddButtonObject(s_LegendObjectName + "_" + i, legendObject.transform,
                    m_ThemeInfo.font, m_Legend.itemFontSize, m_ThemeInfo.legendTextColor, anchor,
                    anchorMin, anchorMax, pivot, new Vector2(m_Legend.itemWidth, m_Legend.itemHeight));

                m_Legend.SetButton(i, btn);
                m_Legend.SetActive(i, IsActive(i));
                m_Legend.UpdateButtonColor(i, m_ThemeInfo.GetColor(i), m_ThemeInfo.legendUnableColor);
                btn.GetComponentInChildren<Text>().text = m_Legend.data[i];
                ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerDown, (data) =>
                {
                    int count = (data as PointerEventData).clickCount;
                    int index = int.Parse(data.selectedObject.name.Split('_')[1]);
                    SetActive(index, !m_Legend.IsActive(index));
                    m_Legend.UpdateButtonColor(index, m_ThemeInfo.GetColor(index),
                        m_ThemeInfo.legendUnableColor);
                    OnYMaxValueChanged();
                    OnLegendButtonClicked();
                    RefreshChart();
                });
            }
        }

        private void InitTooltip()
        {
            var tooltipObject = ChartHelper.AddObject("tooltip", transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            tooltipObject.transform.localPosition = Vector3.zero;
            DestroyImmediate(tooltipObject.GetComponent<Image>());
            var parent = tooltipObject.transform;
            ChartHelper.HideAllObject(tooltipObject.transform);
            GameObject content = ChartHelper.AddTooltipContent("content", parent, m_ThemeInfo.font);
            GameObject labelX = ChartHelper.AddTooltipLabel("label_x", parent, m_ThemeInfo.font, new Vector2(0.5f, 1));
            GameObject labelY = ChartHelper.AddTooltipLabel("label_y", parent, m_ThemeInfo.font, new Vector2(1, 0.5f));
            m_Tooltip.SetObj(tooltipObject);
            m_Tooltip.SetContentObj(content);
            m_Tooltip.SetLabelObj(labelX, labelY);
            m_Tooltip.SetContentBackgroundColor(m_ThemeInfo.tooltipBackgroundColor);
            m_Tooltip.SetContentTextColor(m_ThemeInfo.tooltipTextColor);
            m_Tooltip.SetLabelBackgroundColor(m_ThemeInfo.tooltipLabelColor);
            m_Tooltip.SetLabelTextColor(m_ThemeInfo.tooltipTextColor);
            m_Tooltip.SetActive(false);
        }

        private Vector3 GetLegendPosition(int i)
        {
            return m_Legend.location.GetPosition(chartWidth, chartHeight);
        }

        protected float GetMaxValue(int index)
        {
            if (m_Series == null) return 100;
            else return m_Series.GetMaxValue(index);
        }

        private void CheckSize()
        {
            if (m_CheckWidth != chartWidth || m_CheckHeight != chartHeight)
            {
                m_CheckWidth = chartWidth;
                m_CheckHeight = chartHeight;
                OnSizeChanged();
            }
        }

        private void CheckTheme()
        {
            if (m_CheckTheme != m_Theme)
            {
                m_CheckTheme = m_Theme;
                OnThemeChanged();
            }
        }

        private void CheckTile()
        {
            if (!m_CheckTitle.Equals(m_Title))
            {
                m_CheckTitle.Copy(m_Title);
                OnTitleChanged();
            }
        }

        private void CheckLegend()
        {
            if (m_CheckLegend != m_Legend)
            {
                m_CheckLegend.Copy(m_Legend);
                OnLegendChanged();
            }
        }

        private void CheckTooltip()
        {
            if (!m_Tooltip.show || !m_Tooltip.isInited)
            {
                if (m_Tooltip.dataIndex != 0)
                {
                    m_Tooltip.dataIndex = 0;
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            m_Tooltip.SetLabelActive(m_Tooltip.crossLabel);
            m_Tooltip.dataIndex = 0;

            Vector2 local;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                Input.mousePosition, null, out local))
            {
                m_Tooltip.SetActive(false);
                RefreshChart();
                return;
            }
            if (local.x < 0 || local.x > chartWidth ||
                local.y < 0 || local.y > chartHeight)
            {
                m_Tooltip.SetActive(false);
                RefreshChart();
                return;
            }
            m_Tooltip.pointerPos = local;
            CheckTootipArea(local);
        }

        protected virtual void CheckTootipArea(Vector2 localPostion)
        {
        }

        protected void CheckRefreshChart()
        {
            if (m_RefreshChart)
            {
                int tempWid = (int)chartWidth;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempWid - 1);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempWid);
                m_RefreshChart = false;
            }
        }

        protected virtual void OnSizeChanged()
        {
            InitTitle();
            InitLegend();
        }

        protected virtual void OnThemeChanged()
        {
            switch (m_Theme)
            {
                case Theme.Dark:
                    m_ThemeInfo.Copy(ThemeInfo.Dark);
                    break;
                case Theme.Default:
                    m_ThemeInfo.Copy(ThemeInfo.Default);
                    break;
                case Theme.Light:
                    m_ThemeInfo.Copy(ThemeInfo.Light);
                    break;
            }
            InitTitle();
            InitLegend();
            InitTooltip();
        }

        protected virtual void OnTitleChanged()
        {
            InitTitle();
        }

        protected virtual void OnLegendChanged()
        {
            InitLegend();
        }

        protected virtual void OnYMaxValueChanged()
        {
        }

        protected virtual void OnLegendButtonClicked()
        {
        }

        public void RefreshChart()
        {
            m_RefreshChart = true;
        }

        protected virtual void RefreshTooltip()
        {
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            DrawBackground(vh);
            DrawChart(vh);
            DrawTooltip(vh);
        }

        protected virtual void DrawChart(VertexHelper vh)
        {
        }

        protected virtual void DrawTooltip(VertexHelper vh)
        {
        }

        private void DrawBackground(VertexHelper vh)
        {
            // draw bg
            Vector3 p1 = new Vector3(0, chartHeight);
            Vector3 p2 = new Vector3(chartWidth, chartHeight);
            Vector3 p3 = new Vector3(chartWidth, 0);
            Vector3 p4 = new Vector3(0, 0);
            ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.backgroundColor);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {

        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
        }

        public virtual void OnScroll(PointerEventData eventData)
        {
        }
    }
}
