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

    public partial class BaseChart : Graphic, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IScrollHandler
    {
        private static readonly string s_TitleObjectName = "title";
        private static readonly string s_LegendObjectName = "legend";
        private static readonly string s_SerieLabelObjectName = "label";

        [SerializeField] protected float m_ChartWidth;
        [SerializeField] protected float m_ChartHeight;
        [SerializeField] protected ThemeInfo m_ThemeInfo;
        [SerializeField] protected Title m_Title = Title.defaultTitle;
        [SerializeField] protected Legend m_Legend = Legend.defaultLegend;
        [SerializeField] protected Tooltip m_Tooltip = Tooltip.defaultTooltip;
        [SerializeField] protected Series m_Series = Series.defaultSeries;
        [SerializeField] protected float m_Large = 1;
        [SerializeField] protected int m_MinShowDataNumber;
        [SerializeField] protected int m_MaxShowDataNumber;
        [SerializeField] protected int m_MaxCacheDataNumber;

        [NonSerialized] private Theme m_CheckTheme = 0;
        [NonSerialized] private Title m_CheckTitle = Title.defaultTitle;
        [NonSerialized] private Legend m_CheckLegend = Legend.defaultLegend;
        [NonSerialized] private float m_CheckWidth = 0;
        [NonSerialized] private float m_CheckHeight = 0;
        [NonSerialized] private float m_CheckSerieCount = 0;
        [NonSerialized] private List<string> m_CheckSerieName = new List<string>();
        [NonSerialized] private bool m_RefreshChart = false;
        [NonSerialized] private bool m_RefreshLabel = false;

        protected Vector2 chartAnchorMax { get { return rectTransform.anchorMax; } }
        protected Vector2 chartAnchorMin { get { return rectTransform.anchorMin; } }
        protected Vector2 chartPivot { get { return rectTransform.pivot; } }

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
            m_ChartWidth = rectTransform.sizeDelta.x;
            m_ChartHeight = rectTransform.sizeDelta.y;
            m_CheckWidth = m_ChartWidth;
            m_CheckHeight = m_ChartHeight;
            m_CheckTheme = m_ThemeInfo.theme;
            InitTitle();
            InitLegend();
            InitSerieLabel();
            InitTooltip();
            TransferOldVersionData();
        }

        protected override void Start()
        {
            RefreshChart();
        }

        protected virtual void Update()
        {
            CheckSize();
            CheckTheme();
            CheckTile();
            CheckLegend();
            CheckTooltip();
            CheckRefreshChart();
            CheckRefreshLabel();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Awake();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ChartHelper.HideAllObject(transform);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            var sizeDelta = rectTransform.sizeDelta;
            if (sizeDelta.x < 580 && sizeDelta.y < 300)
            {
                rectTransform.sizeDelta = new Vector2(580, 300);
            }
            ChartHelper.HideAllObject(transform);
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

        private void TransferOldVersionData()
        {
            foreach (var serie in m_Series.series)
            {
                if (serie.yData.Count <= 0) continue;
                bool needTransfer = true;
                foreach (var sd in serie.data)
                {
                    foreach (var value in sd.data)
                    {
                        if (value != 0) needTransfer = false;
                    }
                }
                if (needTransfer)
                {
                    serie.data.Clear();
                    for (int i = 0; i < serie.yData.Count; i++)
                    {
                        float xvalue = i < serie.xData.Count ? serie.xData[i] : i;
                        float yvalue = serie.yData[i];
                        var serieData = new SerieData();
                        serieData.data = new List<float>() { xvalue, yvalue };
                        serie.data.Add(serieData);
                    }
                }
            }
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

            var serieNameList = m_Series.GetSerieNameList();
            List<string> datas;
            if (m_Legend.data.Count > 0)
            {
                datas = new List<string>();
                for (int i = 0; i < m_Legend.data.Count; i++)
                {
                    var category = m_Legend.data[i];
                    if (serieNameList.Contains(category)) datas.Add(category);
                }
            }
            else
            {
                datas = serieNameList;
            }
            m_Legend.RemoveButton();
            for (int i = 0; i < datas.Count; i++)
            {
                string legendName = datas[i];
                Button btn = ChartHelper.AddButtonObject(s_LegendObjectName + "_" + i + "_" + legendName, legendObject.transform,
                    m_ThemeInfo.font, m_Legend.itemFontSize, m_ThemeInfo.legendTextColor, anchor,
                    anchorMin, anchorMax, pivot, new Vector2(m_Legend.itemWidth, m_Legend.itemHeight));
                var bgColor = IsLegendActive(legendName) ? m_ThemeInfo.GetColor(i) : m_ThemeInfo.legendUnableColor;
                m_Legend.SetButton(legendName, btn, datas.Count);
                m_Legend.UpdateButtonColor(legendName, bgColor);
                btn.GetComponentInChildren<Text>().text = legendName;
                ChartHelper.ClearEventListener(btn.gameObject);
                ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerDown, (data) =>
                {
                    if (data.selectedObject == null) return;
                    var temp = data.selectedObject.name.Split('_');
                    string selectedName = temp[2];
                    int index = int.Parse(temp[1]);
                    OnLegendButtonClick(index, selectedName);
                });
                ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerEnter, (data) =>
                {
                    if (btn == null) return;
                    var temp = btn.name.Split('_');
                    string selectedName = temp[2];
                    int index = int.Parse(temp[1]);
                    OnLegendButtonEnter(index, selectedName);
                });
                ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerExit, (data) =>
                {
                    if (btn == null) return;
                    var temp = btn.name.Split('_');
                    string selectedName = temp[2];
                    int index = int.Parse(temp[1]);
                    OnLegendButtonExit(index, selectedName);
                });
            }
        }

        private void InitSerieLabel()
        {
            ChartHelper.HideAllObject(transform, s_SerieLabelObjectName);
            var labelObject = ChartHelper.AddObject(s_SerieLabelObjectName, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            int count = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                if (serie.type != SerieType.Pie) continue;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    var textName = s_SerieLabelObjectName + "_" + i + "_" + j + "_" + serieData.name;
                    var color = (serie.label.position == SerieLabel.Position.Inside) ? Color.white :
                        (Color)m_ThemeInfo.GetColor(count++);
                    var anchorMin = new Vector2(0.5f, 0.5f);
                    var anchorMax = new Vector2(0.5f, 0.5f);
                    var pivot = new Vector2(0.5f, 0.5f);
                    serieData.label = ChartHelper.AddTextObject(textName, labelObject.transform,
                        m_ThemeInfo.font, color, TextAnchor.MiddleCenter, anchorMin, anchorMax, pivot,
                        new Vector2(50, serie.label.fontSize), serie.label.fontSize);
                    serieData.label.text = serieData.name;
                    serieData.label.gameObject.SetActive(true);
                }
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
            m_Tooltip.SetObj(tooltipObject);
            m_Tooltip.SetContentObj(content);
            m_Tooltip.SetContentBackgroundColor(m_ThemeInfo.tooltipBackgroundColor);
            m_Tooltip.SetContentTextColor(m_ThemeInfo.tooltipTextColor);
            m_Tooltip.SetActive(false);
        }

        private Vector3 GetLegendPosition(int i)
        {
            return m_Legend.location.GetPosition(chartWidth, chartHeight);
        }

        protected float GetMaxValue(int index)
        {
            return m_Series.GetMaxValue(index);
        }

        private void CheckSize()
        {
            if (m_CheckWidth != chartWidth || m_CheckHeight != chartHeight)
            {
                SetSize(chartWidth, chartHeight);
            }
            var sizeDelta = rectTransform.sizeDelta;
            if (m_CheckWidth != sizeDelta.x || m_CheckHeight != sizeDelta.y)
            {
                SetSize(sizeDelta.x, sizeDelta.y);
            }
        }

        private void CheckTheme()
        {
            if (m_CheckTheme != m_ThemeInfo.theme)
            {
                m_CheckTheme = m_ThemeInfo.theme;
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
            else if (m_Legend.show)
            {
                if (m_CheckSerieCount != m_Series.Count)
                {
                    m_CheckSerieCount = m_Series.Count;
                    m_CheckSerieName.Clear();
                    var serieNames = m_Series.GetSerieNameList();
                    foreach (var name in serieNames) m_CheckSerieName.Add(name);
                    OnLegendChanged();
                }
                else if (!ChartHelper.IsValueEqualsList(m_CheckSerieName, m_Series.GetSerieNameList()))
                {
                    var serieNames = m_Series.GetSerieNameList();
                    foreach (var name in serieNames) m_CheckSerieName.Add(name);
                    OnLegendChanged();
                }
            }
        }

        private void CheckTooltip()
        {
            if (!m_Tooltip.show || !m_Tooltip.isInited)
            {

                if (m_Tooltip.dataIndex[0] != 0 || m_Tooltip.dataIndex[1] != 0)
                {
                    m_Tooltip.dataIndex[0] = m_Tooltip.dataIndex[1] = -1;
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            for (int i = 0; i < m_Tooltip.dataIndex.Count; i++)
            {
                m_Tooltip.dataIndex[i] = -1;
            }
            Vector2 local;
            if (canvas == null) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                Input.mousePosition, canvas.worldCamera, out local))
            {
                if (m_Tooltip.IsActive()) RefreshChart();
                m_Tooltip.SetActive(false);
                return;
            }
            if (local.x < 0 || local.x > chartWidth ||
                local.y < 0 || local.y > chartHeight)
            {
                if (m_Tooltip.IsActive()) RefreshChart();
                m_Tooltip.SetActive(false);
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

        protected void CheckRefreshLabel()
        {
            if (m_RefreshLabel)
            {
                m_RefreshLabel = false;
                OnRefreshLabel();
            }
        }

        protected virtual void OnRefreshLabel()
        {

        }

        protected virtual void OnSizeChanged()
        {
            InitTitle();
            InitLegend();
            InitTooltip();
        }

        protected virtual void OnThemeChanged()
        {
            switch (m_ThemeInfo.theme)
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

        protected virtual void OnLegendButtonClick(int index, string legendName)
        {
            foreach (var serie in m_Series.GetSeries(legendName))
            {
                SetActive(serie.index, !serie.show);
            }
            OnYMaxValueChanged();
            RefreshChart();
        }

        protected virtual void OnLegendButtonEnter(int index, string legendName)
        {
            var serie = m_Series.GetSerie(index);
            serie.highlighted = true;
        }

        protected virtual void OnLegendButtonExit(int index, string legendName)
        {
            var serie = m_Series.GetSerie(index);
            serie.highlighted = false;
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
            m_RefreshLabel = true;
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

        protected void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
            float tickness, Vector3 pos, Color color)
        {
            switch (type)
            {
                case SerieSymbolType.None:
                    break;
                case SerieSymbolType.Circle:
                    ChartHelper.DrawCricle(vh, pos, symbolSize, color, GetSymbolCricleSegment(symbolSize));
                    break;
                case SerieSymbolType.EmptyCircle:
                    int segment = GetSymbolCricleSegment(symbolSize);
                    ChartHelper.DrawCricle(vh, pos, symbolSize, m_ThemeInfo.backgroundColor, segment);
                    ChartHelper.DrawDoughnut(vh, pos, symbolSize - tickness, symbolSize, 0, 360, color, segment);
                    break;
                case SerieSymbolType.Rect:
                    ChartHelper.DrawPolygon(vh, pos, symbolSize, color);
                    break;
                case SerieSymbolType.Triangle:
                    var x = symbolSize * Mathf.Cos(30 * Mathf.PI / 180);
                    var y = symbolSize * Mathf.Sin(30 * Mathf.PI / 180);
                    var p1 = new Vector2(pos.x - x, pos.y - y);
                    var p2 = new Vector2(pos.x, pos.y + symbolSize);
                    var p3 = new Vector2(pos.x + x, pos.y - y);
                    ChartHelper.DrawTriangle(vh, p1, p2, p3, color);
                    break;
                case SerieSymbolType.Diamond:
                    p1 = new Vector2(pos.x - symbolSize, pos.y);
                    p2 = new Vector2(pos.x, pos.y + symbolSize);
                    p3 = new Vector2(pos.x + symbolSize, pos.y);
                    var p4 = new Vector2(pos.x, pos.y - symbolSize);
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                    break;
            }
        }

        private int GetSymbolCricleSegment(float radiu)
        {
            int max = 50;
            int segent = (int)(2 * Mathf.PI * radiu / ChartHelper.CRICLE_SMOOTHNESS);
            if (segent > max) segent = max;
            segent = (int)(segent / (1 + (m_Large - 1) / 10));
            return segent;
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
