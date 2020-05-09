/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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

    public partial class BaseChart : MaskableGraphic, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IScrollHandler
    {
        protected static readonly string s_TitleObjectName = "title";
        protected static readonly string s_LegendObjectName = "legend";
        protected static readonly string s_SerieLabelObjectName = "label";
        protected static readonly string s_SerieTitleObjectName = "serie";

        [SerializeField] protected string m_ChartName;
        [SerializeField] protected string m_ChartUUID;
        [SerializeField] protected float m_ChartWidth;
        [SerializeField] protected float m_ChartHeight;
        [SerializeField] protected float m_ChartX;
        [SerializeField] protected float m_ChartY;
        [SerializeField] protected ThemeInfo m_ThemeInfo;
        [SerializeField] protected Title m_Title = Title.defaultTitle;
        [SerializeField] protected Legend m_Legend = Legend.defaultLegend;
        [SerializeField] protected Tooltip m_Tooltip = Tooltip.defaultTooltip;
        [SerializeField] protected Series m_Series = Series.defaultSeries;
        [SerializeField] protected Settings m_Settings = new Settings();
        [SerializeField] protected float m_Large = 1;
        [SerializeField] protected Action<VertexHelper> m_CustomDrawCallback;
        [SerializeField] protected string m_DebugInfo = "";

        [NonSerialized] private Theme m_CheckTheme = 0;
        [NonSerialized] protected Vector3 m_ChartPosition = Vector3.zero;
        [NonSerialized] protected Vector2 m_ChartMinAnchor;
        [NonSerialized] protected Vector2 m_ChartMaxAnchor;
        [NonSerialized] protected Vector2 m_ChartPivot;
        [NonSerialized] protected Vector2 m_ChartSizeDelta;
        [NonSerialized] protected Rect m_ChartRect = new Rect(0, 0, 0, 0);

        [NonSerialized] protected bool m_RefreshChart = false;
        [NonSerialized] protected bool m_RefreshLabel = false;
        [NonSerialized] protected bool m_ReinitLabel = false;
        [NonSerialized] protected bool m_ReinitTitle = false;
        [NonSerialized] protected bool m_CheckAnimation = false;
        [NonSerialized] protected bool m_IsPlayingAnimation = false;
        [NonSerialized] protected List<string> m_LegendRealShowName = new List<string>();

        protected Vector2 chartAnchorMax { get { return m_ChartMinAnchor; } }
        protected Vector2 chartAnchorMin { get { return m_ChartMaxAnchor; } }
        protected Vector2 chartPivot { get { return m_ChartPivot; } }

        protected virtual void InitComponent()
        {
            InitTitle();
            InitLegend();
            InitSerieLabel();
            InitSerieTitle();
            InitTooltip();
        }

        protected override void Awake()
        {
            if (m_ThemeInfo == null)
            {
                m_ThemeInfo = ThemeInfo.Default;
            }
            raycastTarget = false;
            m_ChartX = 100;
            m_ChartY = 100;
            m_ChartWidth = rectTransform.rect.width;
            m_ChartHeight = rectTransform.rect.height;
            m_CheckTheme = m_ThemeInfo.theme;
            InitComponent();
            m_Series.AnimationReset();
            m_Series.AnimationFadeIn();
            XChartsMgr.Instance.AddChart(this);
        }

        protected override void Start()
        {
            RefreshChart();
        }

        protected virtual void Update()
        {
            CheckSize();
            CheckLegend();
            CheckComponent();
            CheckPointerPos();
            CheckTooltip();
            CheckRefreshChart();
            CheckRefreshLabel();
            CheckAnimation();
        }

        protected virtual void CheckComponent()
        {
            if (m_ThemeInfo.anyDirty)
            {
                if (m_CheckTheme != m_ThemeInfo.theme)
                {
                    m_CheckTheme = m_ThemeInfo.theme;
                    m_ThemeInfo.Copy(m_CheckTheme);
                    OnThemeChanged();
                }
                if (m_ThemeInfo.componentDirty)
                {
                    m_Title.SetAllDirty();
                    m_Legend.SetAllDirty();
                    m_Tooltip.SetAllDirty();
                }
                if (m_ThemeInfo.vertsDirty) RefreshChart();
                m_ThemeInfo.ClearDirty();
            }
            if (m_Title.anyDirty)
            {
                if (m_Title.componentDirty) InitTitle();
                if (m_Title.vertsDirty) RefreshChart();
                m_Title.ClearDirty();
            }
            if (m_Legend.anyDirty)
            {
                if (m_Legend.componentDirty) InitLegend();
                if (m_Legend.vertsDirty) RefreshChart();
                m_Legend.ClearDirty();
            }
            if (m_Tooltip.anyDirty)
            {
                if (m_Tooltip.componentDirty) InitTooltip();
                if (m_Tooltip.vertsDirty) RefreshChart();
                m_Tooltip.ClearDirty();
            }
            if (m_Settings.vertsDirty)
            {
                RefreshChart();
                m_Settings.ClearDirty();
            }
            if (m_Series.anyDirty)
            {
                if (m_Series.vertsDirty) RefreshChart();
                if (m_Series.labelDirty) m_ReinitLabel = true;
                if (m_Series.labelUpdate && !m_RefreshChart) m_RefreshLabel = true;
                foreach (var serie in m_Series.list)
                {
                    if (serie.titleStyle.componentDirty) m_ReinitTitle = true;
                }
                m_Series.ClearDirty();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ChartHelper.ActiveAllObject(transform, true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ChartHelper.ActiveAllObject(transform, false);
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

        protected override void OnValidate()
        {
            m_ThemeInfo.SetAllDirty();
            m_Title.SetAllDirty();
            m_Legend.SetAllDirty();
            m_Tooltip.SetAllDirty();
            m_ReinitLabel = true;
            m_ReinitTitle = true;
        }
#endif

        protected override void OnDestroy()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private void InitTitle()
        {
            m_Title.OnChanged();
            TextAnchor anchor = m_Title.location.runtimeTextAnchor;
            Vector2 anchorMin = m_Title.location.runtimeAnchorMin;
            Vector2 anchorMax = m_Title.location.runtimeAnchorMax;
            Vector2 pivot = m_Title.location.runtimePivot;
            Vector3 titlePosition = GetTitlePosition();
            Vector3 subTitlePosition = -new Vector3(0, m_Title.textStyle.fontSize + m_Title.itemGap, 0);
            float titleWid = chartWidth;

            var titleObject = ChartHelper.AddObject(s_TitleObjectName, transform, anchorMin, anchorMax,
                pivot, new Vector2(chartWidth, chartHeight));
            titleObject.transform.localPosition = titlePosition;
            ChartHelper.HideAllObject(titleObject);

            var textFont = TitleHelper.GetTextFont(title, themeInfo);
            var textColor = TitleHelper.GetTextColor(title, themeInfo);
            Text titleText = ChartHelper.AddTextObject(s_TitleObjectName, titleObject.transform,
                        textFont, textColor, anchor, anchorMin, anchorMax, pivot,
                        new Vector2(titleWid, m_Title.textStyle.fontSize), m_Title.textStyle.fontSize,
                        m_Title.textStyle.rotate, m_Title.textStyle.fontStyle, m_Title.textStyle.lineSpacing);

            titleText.alignment = anchor;
            titleText.gameObject.SetActive(m_Title.show);
            titleText.transform.localPosition = Vector3.zero + m_Title.textStyle.offsetv3;
            titleText.text = m_Title.text.Replace("\\n", "\n");

            var subTextFont = TitleHelper.GetSubTextFont(title, themeInfo);
            var subTextColor = TitleHelper.GetSubTextColor(title, themeInfo);
            Text subText = ChartHelper.AddTextObject(s_TitleObjectName + "_sub", titleObject.transform,
                        subTextFont, subTextColor, anchor, anchorMin, anchorMax, pivot,
                        new Vector2(titleWid, m_Title.subTextStyle.fontSize), m_Title.subTextStyle.fontSize,
                        m_Title.subTextStyle.rotate, m_Title.subTextStyle.fontStyle, m_Title.subTextStyle.lineSpacing);

            subText.alignment = anchor;
            subText.gameObject.SetActive(m_Title.show && !string.IsNullOrEmpty(m_Title.subText));
            subText.transform.localPosition = subTitlePosition + m_Title.subTextStyle.offsetv3;
            subText.text = m_Title.subText.Replace("\\n", "\n");
        }

        private void InitLegend()
        {
            m_Legend.OnChanged();
            TextAnchor anchor = m_Legend.location.runtimeTextAnchor;
            Vector2 anchorMin = m_Legend.location.runtimeAnchorMin;
            Vector2 anchorMax = m_Legend.location.runtimeAnchorMax;
            Vector2 pivot = m_Legend.location.runtimePivot;

            var legendObject = ChartHelper.AddObject(s_LegendObjectName, transform, anchorMin, anchorMax,
                pivot, new Vector2(chartWidth, chartHeight));
            legendObject.transform.localPosition = GetLegendPosition();

            m_LegendRealShowName = m_Series.GetSerieNameList();
            List<string> datas;
            if (m_Legend.show && m_Legend.data.Count > 0)
            {
                datas = new List<string>();
                for (int i = 0; i < m_LegendRealShowName.Count; i++)
                {
                    if (m_Legend.data.Contains(m_LegendRealShowName[i])) datas.Add(m_LegendRealShowName[i]);
                }
            }
            else
            {
                datas = m_LegendRealShowName;
            }
            int totalLegend = 0;
            for (int i = 0; i < datas.Count; i++)
            {
                if (!m_Series.IsLegalLegendName(datas[i])) continue;
                totalLegend++;
            }
            m_Legend.RemoveButton();
            ChartHelper.HideAllObject(legendObject);
            if (!m_Legend.show) return;
            for (int i = 0; i < datas.Count; i++)
            {
                if (!m_Series.IsLegalLegendName(datas[i])) continue;
                string legendName = m_Legend.GetFormatterContent(datas[i]);
                var readIndex = m_LegendRealShowName.IndexOf(datas[i]);
                var active = IsActiveByLegend(datas[i]);
                var bgColor = LegendHelper.GetIconColor(m_Legend, readIndex, themeInfo, active);
                var item = LegendHelper.AddLegendItem(m_Legend, i, datas[i], legendObject.transform, m_ThemeInfo,
                    legendName, bgColor, active);
                m_Legend.SetButton(legendName, item, totalLegend);
                ChartHelper.ClearEventListener(item.button.gameObject);
                ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerDown, (data) =>
                {
                    if (data.selectedObject == null || m_Legend.selectedMode == Legend.SelectedMode.None) return;
                    var temp = data.selectedObject.name.Split('_');
                    string selectedName = temp[1];
                    int clickedIndex = int.Parse(temp[0]);
                    if (m_Legend.selectedMode == Legend.SelectedMode.Multiple)
                    {
                        OnLegendButtonClick(clickedIndex, selectedName, !IsActiveByLegend(selectedName));
                    }
                    else
                    {
                        var btnList = m_Legend.buttonList.Values.ToArray();
                        if (btnList.Length == 1)
                        {
                            OnLegendButtonClick(0, selectedName, !IsActiveByLegend(selectedName));
                        }
                        else
                        {
                            for (int n = 0; n < btnList.Length; n++)
                            {
                                temp = btnList[n].name.Split('_');
                                selectedName = btnList[n].legendName;
                                var index = btnList[n].index;
                                OnLegendButtonClick(n, selectedName, index == clickedIndex ? true : false);
                            }
                        }
                    }
                });
                ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerEnter, (data) =>
                {
                    if (item.button == null) return;
                    var temp = item.button.name.Split('_');
                    string selectedName = temp[1];
                    int index = int.Parse(temp[0]);
                    OnLegendButtonEnter(index, selectedName);
                });
                ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerExit, (data) =>
                {
                    if (item.button == null) return;
                    var temp = item.button.name.Split('_');
                    string selectedName = temp[1];
                    int index = int.Parse(temp[0]);
                    OnLegendButtonExit(index, selectedName);
                });
            }
            if (m_Legend.selectedMode == Legend.SelectedMode.Single)
            {
                for (int n = 0; n < m_LegendRealShowName.Count; n++)
                {
                    OnLegendButtonClick(n, m_LegendRealShowName[n], n == 0 ? true : false);
                }
            }
            LegendHelper.ResetItemPosition(m_Legend);
        }

        private void InitSerieLabel()
        {
            var labelObject = ChartHelper.AddObject(s_SerieLabelObjectName, transform, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
            SerieLabelPool.ReleaseAll(labelObject.transform);
            int count = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                SerieHelper.UpdateCenter(serie, chartPosition, chartWidth, chartHeight);
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                    if (!serieLabel.show && j > 100) continue;
                    var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, i, j);
                    var color = Color.grey;
                    if (serie.type == SerieType.Pie)
                    {
                        color = (serieLabel.position == SerieLabel.Position.Inside) ? Color.white :
                            (Color)m_ThemeInfo.GetColor(count);
                    }
                    else
                    {
                        color = serieLabel.color != Color.clear ? serieLabel.color :
                            (Color)m_ThemeInfo.GetColor(i);
                    }
                    var labelObj = SerieLabelPool.Get(textName, labelObject.transform, serieLabel, m_ThemeInfo.font, color,
                         serieData.iconStyle.width, serieData.iconStyle.height);
                    var iconImage = labelObj.transform.Find("Icon").GetComponent<Image>();
                    serieData.SetIconImage(iconImage);

                    var isAutoSize = serieLabel.backgroundWidth == 0 || serieLabel.backgroundHeight == 0;
                    serieData.InitLabel(labelObj, isAutoSize, serieLabel.paddingLeftRight, serieLabel.paddingTopBottom);
                    serieData.SetLabelActive(false);
                    count++;
                }
            }
            SerieLabelHelper.UpdateLabelText(m_Series, m_ThemeInfo);
        }

        private void InitSerieTitle()
        {
            var titleObject = ChartHelper.AddObject(s_SerieTitleObjectName, transform, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, new Vector2(chartWidth, chartHeight));
            ChartHelper.HideAllObject(titleObject);
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                var textStyle = serie.titleStyle.textStyle;
                var color = textStyle.color == Color.clear ? m_ThemeInfo.GetColor(i) : (Color32)textStyle.color;
                var anchorMin = new Vector2(0.5f, 0.5f);
                var anchorMax = new Vector2(0.5f, 0.5f);
                var pivot = new Vector2(0.5f, 0.5f);
                var fontSize = 10;
                var sizeDelta = new Vector2(50, fontSize + 2);
                var txt = ChartHelper.AddTextObject("title_" + i, titleObject.transform, m_ThemeInfo.font, color,
                    TextAnchor.MiddleCenter, anchorMin, anchorMax, pivot, sizeDelta, textStyle.fontSize, textStyle.rotate,
                    textStyle.fontStyle, textStyle.lineSpacing);
                txt.text = "";
                txt.transform.localPosition = new Vector2(0, 0);
                txt.transform.localEulerAngles = Vector2.zero;
                ChartHelper.SetActive(txt, serie.titleStyle.show);
                serie.titleStyle.runtimeText = txt;
                serie.titleStyle.UpdatePosition(serie.runtimeCenterPos);
                var serieData = serie.GetSerieData(0);
                if (serieData != null)
                {
                    txt.text = serieData.name;
                }
            }
        }

        private void InitTooltip()
        {
            var tooltipObject = ChartHelper.AddObject("tooltip", transform, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
            tooltipObject.transform.localPosition = Vector3.zero;
            DestroyImmediate(tooltipObject.GetComponent<Image>());
            var parent = tooltipObject.transform;
            var textStyle = m_Tooltip.textStyle;
            ChartHelper.HideAllObject(tooltipObject.transform);
            GameObject content = ChartHelper.AddTooltipContent("content", parent, m_ThemeInfo.font,
                textStyle.fontSize, textStyle.fontStyle, textStyle.lineSpacing);
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
            var currWidth = rectTransform.rect.width;
            var currHeight = rectTransform.rect.height;

            if (m_ChartWidth == 0 && m_ChartHeight == 0 && (currWidth != 0 || currHeight != 0))
            {
                Awake();
            }

            if (m_ChartWidth != currWidth || m_ChartHeight != currHeight ||
                m_ChartMinAnchor != rectTransform.anchorMin || m_ChartMaxAnchor != rectTransform.anchorMax)
            {
                UpdateSize();
            }
        }

        private void UpdateSize()
        {
            m_ChartWidth = rectTransform.rect.width;
            m_ChartHeight = rectTransform.rect.height;

            m_ChartMaxAnchor = rectTransform.anchorMax;
            m_ChartMinAnchor = rectTransform.anchorMin;
            m_ChartSizeDelta = rectTransform.sizeDelta;
            m_ReinitLabel = true;

            rectTransform.pivot = LayerHelper.ResetChartPositionAndPivot(m_ChartMinAnchor, m_ChartMaxAnchor,
               m_ChartWidth, m_ChartHeight, ref m_ChartX, ref m_ChartY);
            m_ChartPivot = rectTransform.pivot;

            m_ChartRect.x = m_ChartX;
            m_ChartRect.y = m_ChartY;
            m_ChartRect.width = m_ChartWidth;
            m_ChartRect.height = m_ChartHeight;
            m_ChartPosition.x = m_ChartX;
            m_ChartPosition.y = m_ChartY;

            OnSizeChanged();
        }

        private void CheckLegend()
        {
            if (m_Legend.show)
            {
                foreach (var serie in series.list)
                {
                    if (serie.nameDirty)
                    {
                        m_Legend.SetAllDirty();
                        serie.ClearNameDirty();
                    }
                }
            }
        }

        private void CheckPointerPos()
        {
            var needCheck = (m_Tooltip.show && m_Tooltip.runtimeInited)
                || raycastTarget;
            if (needCheck)
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

        private void CheckTooltip()
        {
            if (!m_Tooltip.show || !m_Tooltip.runtimeInited)
            {
                if (m_Tooltip.IsActive())
                {
                    m_Tooltip.ClearValue();
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            for (int i = 0; i < m_Tooltip.runtimeDataIndex.Count; i++)
            {
                m_Tooltip.runtimeDataIndex[i] = -1;
            }
            Vector2 local = pointerPos;
            if (canvas == null) return;

            if (local == Vector2.zero)
            {
                if (m_Tooltip.IsActive())
                {
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            if (!IsInChart(local))
            {
                if (m_Tooltip.IsActive())
                {
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            m_Tooltip.runtimePointerPos = local;
            CheckTootipArea(local);
        }

        protected virtual void CheckTootipArea(Vector2 localPostion)
        {
        }

        protected void CheckRefreshChart()
        {
            if (m_RefreshChart || m_Series.vertsDirty)
            {
                //SetAllDirty();
                SetVerticesDirty();
                m_RefreshChart = false;
                m_Series.ClearVerticesDirty();
            }
        }

        protected void CheckRefreshLabel()
        {
            if (m_ReinitLabel)
            {
                m_ReinitLabel = false;
                m_LegendRealShowName = m_Series.GetSerieNameList();
                InitSerieLabel();
            }
            if (m_ReinitTitle)
            {
                m_ReinitTitle = false;
                InitSerieTitle();
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
                m_Series.AnimationFadeIn();
            }
        }

        protected virtual void OnRefreshLabel()
        {
        }

        protected virtual void OnSizeChanged()
        {
            m_Title.SetAllDirty();
            m_Legend.SetAllDirty();
            m_Tooltip.SetAllDirty();
            m_Series.SetLabelDirty();
            RefreshChart();
        }

        protected virtual void OnThemeChanged()
        {
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
            foreach (var serie in m_Series.list)
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
            foreach (var serie in m_Series.list)
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

        protected virtual void UpdateTooltip()
        {
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            DrawBackground(vh);
            DrawChart(vh);
            if (m_CustomDrawCallback != null)
            {
                m_CustomDrawCallback(vh);
            }
            DrawTooltip(vh);
            m_RefreshLabel = true;
        }

        protected virtual void DrawChart(VertexHelper vh)
        {
        }

        protected virtual void DrawTooltip(VertexHelper vh)
        {
        }

        protected virtual void DrawBackground(VertexHelper vh)
        {
            Vector3 p1 = new Vector3(chartX, chartY + chartHeight);
            Vector3 p2 = new Vector3(chartX + chartWidth, chartY + chartHeight);
            Vector3 p3 = new Vector3(chartX + chartWidth, chartY);
            Vector3 p4 = new Vector3(chartX, chartY);
            ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.backgroundColor);
        }

        protected void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
            float tickness, Vector3 pos, Color color, Color toColor, float gap, float[] cornerRadius)
        {
            var backgroundColor = m_ThemeInfo.backgroundColor;
            var smoothness = m_Settings.cicleSmoothness;
            switch (type)
            {
                case SerieSymbolType.None:
                    break;
                case SerieSymbolType.Circle:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, color, toColor, smoothness);
                    }
                    else
                    {
                        ChartDrawer.DrawCricle(vh, pos, symbolSize, color, toColor, smoothness);
                    }
                    break;
                case SerieSymbolType.EmptyCircle:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawCricle(vh, pos, symbolSize + gap, backgroundColor, smoothness);
                        ChartDrawer.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, toColor, backgroundColor, smoothness);
                    }
                    else
                    {
                        ChartDrawer.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, toColor, backgroundColor, smoothness);
                    }
                    break;
                case SerieSymbolType.Rect:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawPolygon(vh, pos, symbolSize + gap, backgroundColor);
                        ChartDrawer.DrawPolygon(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        //ChartDrawer.DrawPolygon(vh, pos, symbolSize, color, toColor);
                        ChartDrawer.DrawRoundRectangle(vh, pos, symbolSize, symbolSize, color, 0, cornerRadius);
                    }
                    break;
                case SerieSymbolType.Triangle:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawTriangle(vh, pos, symbolSize + gap, backgroundColor);
                        ChartDrawer.DrawTriangle(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        ChartDrawer.DrawTriangle(vh, pos, symbolSize, color, toColor);
                    }
                    break;
                case SerieSymbolType.Diamond:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawDiamond(vh, pos, symbolSize + gap, backgroundColor);
                        ChartDrawer.DrawDiamond(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        ChartDrawer.DrawDiamond(vh, pos, symbolSize, color, toColor);
                    }
                    break;
            }
        }

        protected void DrawLineStyle(VertexHelper vh, LineStyle lineStyle,
            Vector3 startPos, Vector3 endPos, Color color)
        {
            var type = lineStyle.type;
            var width = lineStyle.width;
            switch (type)
            {
                case LineStyle.Type.Dashed:
                    ChartDrawer.DrawDashLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.Dotted:
                    ChartDrawer.DrawDotLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.Solid:
                    ChartDrawer.DrawLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.DashDot:
                    ChartDrawer.DrawDashDotLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.DashDotDot:
                    ChartDrawer.DrawDashDotDotLine(vh, startPos, endPos, width, color);
                    break;
            }
        }

        protected void DrawLabelBackground(VertexHelper vh, Serie serie, SerieData serieData)
        {
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (!serieLabel.show) return;
            var invert = serie.type == SerieType.Line
                && SerieHelper.IsDownPoint(serie, serieData.index)
                && !serie.areaStyle.show;
            var centerPos = serieData.labelPosition + serieLabel.offset * (invert ? -1 : 1);
            var labelHalfWid = serieData.GetLabelWidth() / 2;
            var labelHalfHig = serieData.GetLabelHeight() / 2;
            var p1 = new Vector3(centerPos.x - labelHalfWid, centerPos.y + labelHalfHig);
            var p2 = new Vector3(centerPos.x + labelHalfWid, centerPos.y + labelHalfHig);
            var p3 = new Vector3(centerPos.x + labelHalfWid, centerPos.y - labelHalfHig);
            var p4 = new Vector3(centerPos.x - labelHalfWid, centerPos.y - labelHalfHig);

            if (serieLabel.rotate > 0)
            {
                p1 = ChartHelper.RotateRound(p1, centerPos, Vector3.forward, serieLabel.rotate);
                p2 = ChartHelper.RotateRound(p2, centerPos, Vector3.forward, serieLabel.rotate);
                p3 = ChartHelper.RotateRound(p3, centerPos, Vector3.forward, serieLabel.rotate);
                p4 = ChartHelper.RotateRound(p4, centerPos, Vector3.forward, serieLabel.rotate);
            }

            ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, serieLabel.backgroundColor);

            if (serieLabel.border)
            {
                ChartDrawer.DrawBorder(vh, centerPos, serieData.GetLabelWidth(), serieData.GetLabelHeight(),
                    serieLabel.borderWidth, serieLabel.borderColor, serieLabel.rotate);
            }
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
