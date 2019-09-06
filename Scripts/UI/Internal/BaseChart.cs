using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

namespace XCharts
{
    /// <summary>
    /// the layout is horizontal or vertical.
    /// 垂直还是水平布局方式。
    /// </summary>
    public enum Orient
    {
        /// <summary>
        /// 水平
        /// </summary>
        Horizonal,
        /// <summary>
        /// 垂直
        /// </summary>
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
        [SerializeField] [Range(1, 8)] private float m_LineSmoothStyle = 2f;

        [NonSerialized] private Theme m_CheckTheme = 0;
        [NonSerialized] private Title m_CheckTitle = Title.defaultTitle;
        [NonSerialized] private Legend m_CheckLegend = Legend.defaultLegend;
        [NonSerialized] private float m_CheckWidth = 0;
        [NonSerialized] private float m_CheckHeight = 0;
        [NonSerialized] private float m_CheckSerieCount = 0;
        [NonSerialized] private List<string> m_CheckSerieName = new List<string>();
        [NonSerialized] private bool m_RefreshChart = false;
        [NonSerialized] private bool m_RefreshLabel = false;
        [NonSerialized] private bool m_ReinitLabel = false;
        [NonSerialized] private bool m_CheckAnimation = false;

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
            m_Series.AnimationStop();
            m_Series.AnimationStart();
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
            CheckAnimation();
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
            ChartHelper.HideAllObject(titleObject);

            Text titleText = ChartHelper.AddTextObject(s_TitleObjectName, titleObject.transform,
                        m_ThemeInfo.font, m_ThemeInfo.titleTextColor, anchor, anchorMin, anchorMax, pivot,
                        new Vector2(titleWid, m_Title.textFontSize), m_Title.textFontSize);

            titleText.alignment = anchor;
            titleText.gameObject.SetActive(m_Title.show);
            titleText.transform.localPosition = Vector2.zero;
            titleText.text = m_Title.text.Replace("\\n", "\n");

            Text subText = ChartHelper.AddTextObject(s_TitleObjectName + "_sub", titleObject.transform,
                        m_ThemeInfo.font, m_ThemeInfo.titleTextColor, anchor, anchorMin, anchorMax, pivot,
                        new Vector2(titleWid, m_Title.subTextFontSize), m_Title.subTextFontSize);

            subText.alignment = anchor;
            subText.gameObject.SetActive(m_Title.show && !string.IsNullOrEmpty(m_Title.subText));
            subText.transform.localPosition = subTitlePosition;
            subText.text = m_Title.subText.Replace("\\n", "\n");
        }

        private void InitLegend()
        {
            m_Legend.OnChanged();
            TextAnchor anchor = m_Legend.location.textAnchor;
            Vector2 anchorMin = m_Legend.location.anchorMin;
            Vector2 anchorMax = m_Legend.location.anchorMax;
            Vector2 pivot = m_Legend.location.pivot;

            var legendObject = ChartHelper.AddObject(s_LegendObjectName, transform, anchorMin, anchorMax,
                pivot, new Vector2(chartWidth, chartHeight));
            legendObject.transform.localPosition = m_Legend.location.GetPosition(chartWidth, chartHeight);
            ChartHelper.DestoryAllChilds(legendObject.transform);
            if (!m_Legend.show) return;
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
                var bgColor = IsActiveByLegend(legendName) ? m_ThemeInfo.GetColor(i) : m_ThemeInfo.legendUnableColor;
                m_Legend.SetButton(legendName, btn, datas.Count);
                m_Legend.UpdateButtonColor(legendName, bgColor);
                btn.GetComponentInChildren<Text>().text = legendName;
                ChartHelper.ClearEventListener(btn.gameObject);
                ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerDown, (data) =>
                {
                    if (data.selectedObject == null || m_Legend.selectedMode == Legend.SelectedMode.None) return;
                    var temp = data.selectedObject.name.Split('_');
                    string selectedName = temp[2];
                    int clickedIndex = int.Parse(temp[1]);
                    if (m_Legend.selectedMode == Legend.SelectedMode.Multiple)
                    {
                        OnLegendButtonClick(clickedIndex, selectedName, !IsActiveByLegend(selectedName));
                    }
                    else
                    {
                        var btnList = m_Legend.buttonList.Values.ToArray();
                        for (int n = 0; n < btnList.Length; n++)
                        {
                            temp = btnList[n].name.Split('_');
                            selectedName = temp[2];
                            var index = int.Parse(temp[1]);
                            OnLegendButtonClick(n, selectedName, index == clickedIndex ? true : false);
                        }
                    }
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
            if (m_Legend.selectedMode == Legend.SelectedMode.Single)
            {
                for (int n = 0; n < datas.Count; n++)
                {
                    OnLegendButtonClick(n, datas[n], n == 0 ? true : false);
                }
            }
        }

        private void InitSerieLabel()
        {
            var labelObject = ChartHelper.AddObject(s_SerieLabelObjectName, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            ChartHelper.DestoryAllChilds(labelObject.transform);
            int count = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                if (serie.type != SerieType.Pie && !serie.label.show) continue;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    var textName = s_SerieLabelObjectName + "_" + i + "_" + j + "_" + serieData.name;
                    var color = Color.grey;
                    if (serie.type == SerieType.Pie)
                    {
                        color = (serie.label.position == SerieLabel.Position.Inside) ? Color.white :
                            (Color)m_ThemeInfo.GetColor(count);
                    }
                    else
                    {
                        color = serie.label.color != Color.clear ? serie.label.color :
                            (Color)m_ThemeInfo.GetColor(i);
                    }
                    var backgroundColor = serie.label.backgroundColor;
                    var labelObj = ChartHelper.AddSerieLabel(textName, labelObject.transform, m_ThemeInfo.font,
                        color, backgroundColor, serie.label.fontSize, serie.label.fontStyle, serie.label.rotate,
                        serie.label.backgroundWidth, serie.label.backgroundHeight);
                    var isAutoSize = serie.label.backgroundWidth == 0 || serie.label.backgroundHeight == 0;
                    serieData.InitLabel(labelObj, isAutoSize, serie.label.paddingLeftRight, serie.label.paddingTopBottom);
                    serieData.SetLabelActive(false);
                    serieData.SetLabelText(serieData.name);
                    count++;
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

        private void CheckSize()
        {
            var sizeDelta = rectTransform.sizeDelta;
            if (m_CheckWidth == 0 && m_CheckHeight == 0 && (sizeDelta.x != 0 || sizeDelta.y != 0))
            {
                Awake();
            }
            else if (m_CheckWidth != sizeDelta.x || m_CheckHeight != sizeDelta.y)
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
            if (!m_Tooltip.show || !m_Tooltip.inited)
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
            if (m_ReinitLabel)
            {
                m_ReinitLabel = false;
                InitSerieLabel();
            }
            if (m_RefreshLabel)
            {
                m_RefreshLabel = false;
                OnRefreshLabel();
            }
        }

        protected void CheckAnimation()
        {
            if (!m_CheckAnimation)
            {
                m_CheckAnimation = true;
                m_Series.AnimationStart();
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

        protected virtual void OnLegendButtonClick(int index, string legendName, bool show)
        {
            foreach (var serie in m_Series.GetSeries(legendName))
            {
                SetActive(serie.index, show);
            }
            OnYMaxValueChanged();
            RefreshChart();
        }

        protected virtual void OnLegendButtonEnter(int index, string legendName)
        {
            var serie = m_Series.GetSerie(index);
            serie.highlighted = true;
            RefreshChart();
        }

        protected virtual void OnLegendButtonExit(int index, string legendName)
        {
            var serie = m_Series.GetSerie(index);
            serie.highlighted = false;
            RefreshChart();
        }

        protected bool CheckDataShow(string legendName, bool show)
        {
            bool needShow = false;
            foreach (var serie in m_Series.series)
            {
                if (legendName.Equals(serie.name))
                {
                    serie.show = show;
                    serie.highlighted = false;
                    if (serie.show) needShow = true;
                }
                else
                {
                    foreach (var data in serie.data)
                    {
                        if (legendName.Equals(data.name))
                        {
                            data.show = show;
                            data.highlighted = false;
                            if (data.show) needShow = true;
                        }
                    }
                }
            }
            return needShow;
        }

        protected bool CheckDataHighlighted(string legendName, bool heighlight)
        {
            bool show = false;
            foreach (var serie in m_Series.series)
            {
                if (legendName.Equals(serie.name))
                {
                    serie.highlighted = heighlight;
                }
                else
                {
                    foreach (var data in serie.data)
                    {
                        if (legendName.Equals(data.name))
                        {
                            data.highlighted = heighlight;
                            if (data.highlighted) show = true;
                        }
                    }
                }
            }
            return show;
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

        protected void DrawLabelBackground(VertexHelper vh, Serie serie, SerieData serieData)
        {
            var labelHalfWid = serieData.GetLabelWidth() / 2;
            var labelHalfHig = serieData.GetLabelHeight() / 2;
            var centerPos = serieData.labelPosition;
            var p1 = new Vector3(centerPos.x - labelHalfWid, centerPos.y + labelHalfHig);
            var p2 = new Vector3(centerPos.x + labelHalfWid, centerPos.y + labelHalfHig);
            var p3 = new Vector3(centerPos.x + labelHalfWid, centerPos.y - labelHalfHig);
            var p4 = new Vector3(centerPos.x - labelHalfWid, centerPos.y - labelHalfHig);

            if (serie.label.rotate > 0)
            {
                p1 = ChartHelper.RotateRound(p1, centerPos, Vector3.forward, serie.label.rotate);
                p2 = ChartHelper.RotateRound(p2, centerPos, Vector3.forward, serie.label.rotate);
                p3 = ChartHelper.RotateRound(p3, centerPos, Vector3.forward, serie.label.rotate);
                p4 = ChartHelper.RotateRound(p4, centerPos, Vector3.forward, serie.label.rotate);
            }

            ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, serie.label.backgroundColor);

            if (serie.label.border)
            {
                var borderWid = serie.label.borderWidth;
                p1 = new Vector3(centerPos.x - labelHalfWid, centerPos.y + labelHalfHig + borderWid);
                p2 = new Vector3(centerPos.x + labelHalfWid + 2 * borderWid, centerPos.y + labelHalfHig + borderWid);
                p3 = new Vector3(centerPos.x + labelHalfWid + borderWid, centerPos.y + labelHalfHig);
                p4 = new Vector3(centerPos.x + labelHalfWid + borderWid, centerPos.y - labelHalfHig - 2 * borderWid);
                var p5 = new Vector3(centerPos.x + labelHalfWid, centerPos.y - labelHalfHig - borderWid);
                var p6 = new Vector3(centerPos.x - labelHalfWid - 2 * borderWid, centerPos.y - labelHalfHig - borderWid);
                var p7 = new Vector3(centerPos.x - labelHalfWid - borderWid, centerPos.y - labelHalfHig);
                var p8 = new Vector3(centerPos.x - labelHalfWid - borderWid, centerPos.y + labelHalfHig + 2 * borderWid);
                if (serie.label.rotate > 0)
                {
                    p1 = ChartHelper.RotateRound(p1, centerPos, Vector3.forward, serie.label.rotate);
                    p2 = ChartHelper.RotateRound(p2, centerPos, Vector3.forward, serie.label.rotate);
                    p3 = ChartHelper.RotateRound(p3, centerPos, Vector3.forward, serie.label.rotate);
                    p4 = ChartHelper.RotateRound(p4, centerPos, Vector3.forward, serie.label.rotate);
                    p5 = ChartHelper.RotateRound(p5, centerPos, Vector3.forward, serie.label.rotate);
                    p6 = ChartHelper.RotateRound(p6, centerPos, Vector3.forward, serie.label.rotate);
                    p7 = ChartHelper.RotateRound(p7, centerPos, Vector3.forward, serie.label.rotate);
                    p8 = ChartHelper.RotateRound(p8, centerPos, Vector3.forward, serie.label.rotate);
                }
                ChartHelper.DrawLine(vh, p1, p2, borderWid, serie.label.borderColor);
                ChartHelper.DrawLine(vh, p3, p4, borderWid, serie.label.borderColor);
                ChartHelper.DrawLine(vh, p5, p6, borderWid, serie.label.borderColor);
                ChartHelper.DrawLine(vh, p7, p8, borderWid, serie.label.borderColor);
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
