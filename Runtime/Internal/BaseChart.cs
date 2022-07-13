/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using XUGL;

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

    public partial class BaseChart : BaseGraph
    {
        protected static readonly string s_TitleObjectName = "title";
        protected static readonly string s_SubTitleObjectName = "title_sub";
        protected static readonly string s_LegendObjectName = "legend";
        protected static readonly string s_SerieLabelObjectName = "label";
        protected static readonly string s_SerieTitleObjectName = "serie";

        [SerializeField] protected string m_ChartName;
        [SerializeField] protected ChartTheme m_Theme;
        [SerializeField] protected Settings m_Settings;
        [SerializeField] protected List<Title> m_Titles = new List<Title>() { Title.defaultTitle };
        [SerializeField] protected List<Legend> m_Legends = new List<Legend>() { Legend.defaultLegend };
        [SerializeField] protected List<Tooltip> m_Tooltips = new List<Tooltip>() { Tooltip.defaultTooltip };

        [SerializeField] protected List<Grid> m_Grids = new List<Grid>();
        [SerializeField] protected List<XAxis> m_XAxes = new List<XAxis>();
        [SerializeField] protected List<YAxis> m_YAxes = new List<YAxis>();
        [SerializeField] protected List<DataZoom> m_DataZooms = new List<DataZoom>();
        [SerializeField] protected List<VisualMap> m_VisualMaps = new List<VisualMap>();
        [SerializeField] protected List<Vessel> m_Vessels = new List<Vessel>();
        [SerializeField] protected List<Polar> m_Polars = new List<Polar>();
        [SerializeField] protected List<RadiusAxis> m_RadiusAxes = new List<RadiusAxis>();
        [SerializeField] protected List<AngleAxis> m_AngleAxes = new List<AngleAxis>();
        [SerializeField] protected List<Radar> m_Radars = new List<Radar>();

        [SerializeField] protected Series m_Series = Series.defaultSeries;

        protected float m_ChartWidth;
        protected float m_ChartHeight;
        protected float m_ChartX;
        protected float m_ChartY;
        protected Vector3 m_ChartPosition = Vector3.zero;
        protected Vector2 m_ChartMinAnchor;
        protected Vector2 m_ChartMaxAnchor;
        protected Vector2 m_ChartPivot;
        protected Vector2 m_ChartSizeDelta;
        protected Rect m_ChartRect = new Rect(0, 0, 0, 0);
        protected Action<VertexHelper> m_OnCustomDrawBaseCallback;
        protected Action<VertexHelper> m_OnCustomDrawTopCallback;
        protected Action<VertexHelper, Serie> m_OnCustomDrawSerieBeforeCallback;
        protected Action<VertexHelper, Serie> m_OnCustomDrawSerieAfterCallback;
        protected Action<PointerEventData, int, int> m_OnPointerClickPie;
        protected Action<int, string, bool> m_OnLegendClick;
        protected Action<int, string> m_OnLegendEnter;
        protected Action<int, string> m_OnLegendExit;

        protected bool m_RefreshLabel = false;
        internal bool m_ReinitLabel = false;
        internal bool m_ReinitTitle = false;
        internal bool m_CheckAnimation = false;
        internal bool m_IsPlayingAnimation = false;
        internal int m_BasePainterVertCount;
        internal int m_TopPainterVertCount;
        internal protected List<string> m_LegendRealShowName = new List<string>();
        protected List<Painter> m_PainterList = new List<Painter>();
        internal Painter m_PainterTop;
        protected GameObject m_SerieLabelRoot;
        private Theme m_CheckTheme = 0;

        protected List<IDrawSerie> m_DrawSeries = new List<IDrawSerie>();
        protected List<IComponentHandler> m_ComponentHandlers = new List<IComponentHandler>();

        protected override void InitComponent()
        {
            base.InitComponent();
            InitTitles();
            InitLegends();
            InitSerieLabel();
            InitSerieTitle();
            InitTooltip();
            m_DrawSeries.Clear();
            m_DrawSeries.Add(new DrawSeriePie(this));
            m_DrawSeries.Add(new DrawSerieRing(this));
            m_DrawSeries.Add(new DrawSerieGauge(this));
            m_DrawSeries.Add(new DrawSerieLiquid(this));
            m_DrawSeries.Add(new DrawSerieRadar(this));
            foreach (var draw in m_DrawSeries) draw.InitComponent();

            m_ComponentHandlers.Clear();
            m_ComponentHandlers.Add(new VisualMapHandler(this));
            m_ComponentHandlers.Add(new DataZoomHandler(this));
            foreach (var draw in m_ComponentHandlers) draw.Init();

            m_DebugInfo.Init(this);
        }

        protected override void Awake()
        {
            if (m_Settings == null) m_Settings = Settings.DefaultSettings;
            if (m_Series == null) m_Series = Series.defaultSeries; ;
            if (m_Titles.Count == 0) m_Titles = new List<Title>() { Title.defaultTitle };
            if (m_Legends.Count == 0) m_Legends = new List<Legend>() { Legend.defaultLegend };
            if (m_Tooltips.Count == 0) m_Tooltips = new List<Tooltip>() { Tooltip.defaultTooltip };
            CheckTheme();
            base.Awake();
            AnimationReset();
            AnimationFadeIn();
            XChartsMgr.Instance.AddChart(this);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Theme = null;
            m_Settings = null;
            m_Series = null;
            m_Titles.Clear();
            m_Legends.Clear();
            m_Tooltips.Clear();
            var sizeDelta = rectTransform.sizeDelta;
            if (sizeDelta.x < 580 && sizeDelta.y < 300)
            {
                rectTransform.sizeDelta = new Vector2(580, 300);
            }
            ChartHelper.HideAllObject(transform);
            Awake();
        }
#endif

        protected override void Start()
        {
            RefreshChart();
        }

        protected override void Update()
        {
            CheckTheme();
            base.Update();
            CheckPainter();
            CheckTooltip();
            CheckRefreshChart();
            CheckRefreshLabel();
            Internal_CheckAnimation();
            foreach (var draw in m_DrawSeries) draw.Update();
            foreach (var draw in m_ComponentHandlers) draw.Update();
            m_DebugInfo.Update();
        }

        public Painter GetPainter(int index)
        {
            if (index >= 0 && index < m_PainterList.Count)
            {
                return m_PainterList[index];
            }
            return null;
        }

        public void RefreshBasePainter()
        {
            m_Painter.Refresh();
        }
        public void RefreshTopPainter()
        {
            m_PainterTop.Refresh();
        }

        public void RefreshPainter(int index)
        {
            var painter = GetPainter(index);
            RefreshPainter(painter);
        }

        public void RefreshPainter(Serie serie)
        {
            RefreshPainter(GetPainterIndexBySerie(serie));
        }

        internal override void RefreshPainter(Painter painter)
        {
            base.RefreshPainter(painter);
            if (painter != null && painter.type == Painter.Type.Serie)
            {
                m_PainterTop.Refresh();
            }
        }

        public void SetPainterActive(int index, bool flag)
        {
            var painter = GetPainter(index);
            if (painter == null) return;
            painter.SetActive(flag, m_DebugMode);
        }

        protected virtual void CheckTheme()
        {
            if (m_Theme == null)
            {
                m_Theme = ChartTheme.Default;
            }
            else
            {
                if (m_Theme.colorPalette.Count == 0)
                {
                    m_Theme.ResetTheme();
                }
                if (m_CheckTheme != m_Theme.theme)
                {
                    m_CheckTheme = m_Theme.theme;
                    m_Theme.CopyTheme(m_CheckTheme);
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                    SetAllComponentDirty();
                    OnThemeChanged();
                }
            }
        }
        protected override void CheckComponent()
        {
            base.CheckComponent();
            if (m_Series.anyDirty)
            {
                if (SeriesHelper.IsLabelDirty(m_Series)) m_ReinitLabel = true;
                if (SeriesHelper.IsNeedLabelUpdate(m_Series) && !m_RefreshChart) m_RefreshLabel = true;
                foreach (var serie in m_Series.list)
                {
                    if (serie.titleStyle.componentDirty) m_ReinitTitle = true;
                    if (serie.nameDirty)
                    {
                        foreach (var legend in m_Legends) legend.SetAllDirty();
                        RefreshChart();
                        serie.ClearNameDirty();
                    }
                    if (serie.vertsDirty)
                    {
                        RefreshPainter(serie);
                    }
                }
                m_Series.ClearDirty();
            }
            if (m_Theme.anyDirty)
            {
                if (m_Theme.componentDirty)
                {
                    foreach (var title in m_Titles) title.SetAllDirty();
                    foreach (var legend in m_Legends) legend.SetAllDirty();
                    tooltip.SetAllDirty();
                }
                if (m_Theme.vertsDirty) RefreshChart();
                m_Theme.ClearDirty();
            }
            CheckComponentDirty(tooltip);
            foreach (var component in m_Titles) CheckComponentDirty(component);
            foreach (var component in m_Legends) CheckComponentDirty(component);
            foreach (var component in m_Tooltips) CheckComponentDirty(component);
            foreach (var component in m_DataZooms) CheckComponentDirty(component);
            foreach (var component in m_VisualMaps) CheckComponentDirty(component);
            foreach (var component in m_Grids) CheckComponentDirty(component);
            foreach (var component in m_XAxes) CheckComponentDirty(component);
            foreach (var component in m_YAxes) CheckComponentDirty(component);
            foreach (var component in m_Vessels) CheckComponentDirty(component);
            foreach (var component in m_Polars) CheckComponentDirty(component);
            foreach (var component in m_AngleAxes) CheckComponentDirty(component);
            foreach (var component in m_RadiusAxes) CheckComponentDirty(component);
            foreach (var component in m_Radars) CheckComponentDirty(component);
            foreach (var drawSerie in m_DrawSeries) drawSerie.CheckComponent();
        }

        protected override void SetAllComponentDirty()
        {
            base.SetAllComponentDirty();
            m_Theme.SetAllDirty();
            foreach (var component in m_Titles) component.SetAllDirty();
            foreach (var component in m_Legends) component.SetAllDirty();
            foreach (var component in m_Tooltips) component.SetAllDirty();
            foreach (var component in m_Grids) component.SetAllDirty();
            foreach (var component in m_XAxes) component.SetAllDirty();
            foreach (var component in m_YAxes) component.SetAllDirty();
            foreach (var component in m_DataZooms) component.SetAllDirty();
            foreach (var component in m_VisualMaps) component.SetAllDirty();
            foreach (var component in m_Vessels) component.SetAllDirty();
            foreach (var component in m_Polars) component.SetAllDirty();
            foreach (var component in m_RadiusAxes) component.SetAllDirty();
            foreach (var component in m_AngleAxes) component.SetAllDirty();
            foreach (var component in m_Radars) component.SetAllDirty();
            m_ReinitLabel = true;
            m_ReinitTitle = true;
            m_RefreshChart = true;
        }

        protected override void OnDestroy()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        protected virtual void CheckPainter()
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                serie.index = i;
                SetPainterActive(i, true);
            }
        }

        protected override void InitPainter()
        {
            base.InitPainter();
            m_Painter.material = settings.basePainterMaterial;
            m_PainterList.Clear();
            if (settings == null) return;
            var sizeDelta = new Vector2(m_GraphWidth, m_GraphHeight);
            for (int i = 0; i < settings.maxPainter; i++)
            {
                var index = settings.reversePainter ? settings.maxPainter - 1 - i : i;
                var painter = ChartHelper.AddPainterObject("painter_" + index, transform, m_GraphMinAnchor,
                    m_GraphMaxAnchor, m_GraphPivot, sizeDelta, chartHideFlags, 2 + index);
                painter.index = m_PainterList.Count;
                painter.type = Painter.Type.Serie;
                painter.onPopulateMesh = OnDrawPainterSerie;
                painter.SetActive(false, m_DebugMode);
                painter.material = settings.seriePainterMaterial;
                m_PainterList.Add(painter);
            }
            m_PainterTop = ChartHelper.AddPainterObject("painter_t", transform, m_GraphMinAnchor,
                    m_GraphMaxAnchor, m_GraphPivot, sizeDelta, chartHideFlags, 2 + settings.maxPainter);
            m_PainterTop.type = Painter.Type.Top;
            m_PainterTop.onPopulateMesh = OnDrawPainterTop;
            m_PainterTop.SetActive(true, m_DebugMode);
            m_PainterTop.material = settings.topPainterMaterial;
        }

        private void InitTitles()
        {
            for (int i = 0; i < m_Titles.Count; i++)
            {
                var title = m_Titles[i];
                title.index = i;
                InitTitle(title);
            }
        }
        private void InitTitle(Title title)
        {
            title.painter = null;
            title.refreshComponent = delegate ()
            {
                title.OnChanged();
                var anchorMin = title.location.runtimeAnchorMin;
                var anchorMax = title.location.runtimeAnchorMax;
                var pivot = title.location.runtimePivot;
                var titleObject = ChartHelper.AddObject(s_TitleObjectName + title.index, transform, anchorMin, anchorMax,
                    pivot, m_ChartSizeDelta);
                title.gameObject = titleObject;
                anchorMin = title.location.runtimeAnchorMin;
                anchorMax = title.location.runtimeAnchorMax;
                pivot = title.location.runtimePivot;
                title.textStyle.UpdateAlignmentByLocation(title.location);
                title.subTextStyle.UpdateAlignmentByLocation(title.location);
                var fontSize = title.textStyle.GetFontSize(theme.title);
                ChartHelper.UpdateRectTransform(titleObject, anchorMin, anchorMax, pivot, new Vector2(chartWidth, chartHeight));
                var titlePosition = GetTitlePosition(title);
                var subTitlePosition = -new Vector3(0, fontSize + title.itemGap, 0);
                var titleWid = chartWidth;

                titleObject.transform.localPosition = titlePosition;
                titleObject.hideFlags = chartHideFlags;
                ChartHelper.HideAllObject(titleObject);

                var titleText = ChartHelper.AddTextObject(s_TitleObjectName, titleObject.transform, anchorMin, anchorMax,
                    pivot, new Vector2(titleWid, fontSize), title.textStyle, theme.title);
                titleText.SetActive(title.show);
                titleText.SetLocalPosition(Vector3.zero + title.textStyle.offsetv3);
                titleText.SetText(title.text);

                var subText = ChartHelper.AddTextObject(s_SubTitleObjectName, titleObject.transform, anchorMin, anchorMax,
                    pivot, new Vector2(titleWid, title.subTextStyle.GetFontSize(theme.subTitle)), title.subTextStyle,
                    theme.subTitle);
                subText.SetActive(title.show && !string.IsNullOrEmpty(title.subText));
                subText.SetLocalPosition(subTitlePosition + title.subTextStyle.offsetv3);
                subText.SetText(title.subText);
            };
            title.refreshComponent();
        }

        private void InitLegends()
        {
            for (int i = 0; i < m_Legends.Count; i++)
            {
                var legend = m_Legends[i];
                legend.index = i;
                InitLegend(legend);
            }
        }

        private void InitLegend(Legend legend)
        {
            legend.painter = null; // legend component does not need to paint
            legend.refreshComponent = delegate ()
            {
                legend.OnChanged();
                var legendObject = ChartHelper.AddObject(s_LegendObjectName + legend.index, transform, m_ChartMinAnchor,
                     m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
                legend.gameObject = legendObject;
                legendObject.hideFlags = chartHideFlags;
                SeriesHelper.UpdateSerieNameList(this, ref m_LegendRealShowName);
                List<string> datas;
                if (legend.show && legend.data.Count > 0)
                {
                    datas = new List<string>();
                    for (int i = 0; i < m_LegendRealShowName.Count; i++)
                    {
                        if (legend.data.Contains(m_LegendRealShowName[i])) datas.Add(m_LegendRealShowName[i]);
                    }
                }
                else
                {
                    datas = m_LegendRealShowName;
                }
                int totalLegend = 0;
                for (int i = 0; i < datas.Count; i++)
                {
                    if (!SeriesHelper.IsLegalLegendName(datas[i])) continue;
                    totalLegend++;
                }
                legend.RemoveButton();
                ChartHelper.HideAllObject(legendObject);
                if (!legend.show) return;
                for (int i = 0; i < datas.Count; i++)
                {
                    if (!SeriesHelper.IsLegalLegendName(datas[i])) continue;
                    string legendName = legend.GetFormatterContent(datas[i]);
                    var readIndex = m_LegendRealShowName.IndexOf(datas[i]);
                    var active = IsActiveByLegend(datas[i]);
                    var bgColor = LegendHelper.GetIconColor(this, readIndex, datas[i], active);
                    var item = LegendHelper.AddLegendItem(legend, i, datas[i], legendObject.transform, m_Theme,
                        legendName, bgColor, active, readIndex);
                    legend.SetButton(legendName, item, totalLegend);
                    ChartHelper.ClearEventListener(item.button.gameObject);
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerDown, (data) =>
                    {
                        if (data.selectedObject == null || legend.selectedMode == Legend.SelectedMode.None) return;
                        var temp = data.selectedObject.name.Split('_');
                        string selectedName = temp[1];
                        int clickedIndex = int.Parse(temp[0]);
                        if (legend.selectedMode == Legend.SelectedMode.Multiple)
                        {
                            OnLegendButtonClick(clickedIndex, selectedName, !IsActiveByLegend(selectedName));
                        }
                        else
                        {
                            var btnList = legend.buttonList.Values.ToArray();
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
                if (legend.selectedMode == Legend.SelectedMode.Single)
                {
                    for (int n = 0; n < m_LegendRealShowName.Count; n++)
                    {
                        OnLegendButtonClick(n, m_LegendRealShowName[n], n == 0 ? true : false);
                    }
                }
                LegendHelper.ResetItemPosition(legend, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            };
            legend.refreshComponent();
        }

        private void InitSerieLabel()
        {
            m_SerieLabelRoot = ChartHelper.AddObject(s_SerieLabelObjectName, transform, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
            m_SerieLabelRoot.hideFlags = chartHideFlags;
            SerieLabelPool.ReleaseAll(m_SerieLabelRoot.transform);
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                serie.index = i;
                SerieHelper.UpdateCenter(serie, chartPosition, chartWidth, chartHeight);
                int count = 0;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    serieData.index = count;
                    serieData.labelObject = null;
                    AddSerieLabel(serie, serieData, ref count);
                    count++;
                }
            }
            SerieLabelHelper.UpdateLabelText(m_Series, m_Theme, m_LegendRealShowName);
        }


        protected void AddSerieLabel(Serie serie, SerieData serieData, ref int count)
        {
            if (m_SerieLabelRoot == null) return;
            if (count == -1) count = serie.dataCount;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            var serieEmphasisLable = SerieHelper.GetSerieEmphasisLabel(serie, serieData);
            var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
            if (serie.IsPerformanceMode()) return;
            if (!serieLabel.show && (serieEmphasisLable == null || !serieEmphasisLable.show) && !iconStyle.show) return;
            if (serie.animation.enable && serie.animation.HasFadeOut()) return;
            var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, serie.index, serieData.index);
            var color = Color.grey;
            if (serie.type == SerieType.Pie)
            {
                color = (serieLabel.position == SerieLabel.Position.Inside) ? Color.white :
                    (Color)m_Theme.GetColor(count);
            }
            else
            {
                color = !ChartHelper.IsClearColor(serieLabel.textStyle.color) ? serieLabel.textStyle.color :
                    (Color)m_Theme.GetColor(serie.index);
            }
            var labelObj = SerieLabelPool.Get(textName, m_SerieLabelRoot.transform, serieLabel, color,
                       iconStyle.width, iconStyle.height, theme);
            var iconImage = labelObj.transform.Find("Icon").GetComponent<Image>();
            var isAutoSize = serieLabel.backgroundWidth == 0 || serieLabel.backgroundHeight == 0;
            var item = new ChartLabel();
            item.SetLabel(labelObj, isAutoSize, serieLabel.paddingLeftRight, serieLabel.paddingTopBottom);
            item.SetIcon(iconImage);
            item.SetIconActive(iconStyle.show);
            serieData.labelObject = item;

            foreach (var dataIndex in serieData.children)
            {
                AddSerieLabel(serie, serie.GetSerieData(dataIndex), ref count);
                count++;
            }
        }

        private void InitSerieTitle()
        {
            var titleObject = ChartHelper.AddObject(s_SerieTitleObjectName, transform, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, new Vector2(chartWidth, chartHeight));
            titleObject.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(titleObject);
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                var textStyle = serie.titleStyle.textStyle;
                var titleColor = ChartHelper.IsClearColor(textStyle.color) ? m_Theme.GetColor(i) : (Color32)textStyle.color;
                var anchorMin = new Vector2(0.5f, 0.5f);
                var anchorMax = new Vector2(0.5f, 0.5f);
                var pivot = new Vector2(0.5f, 0.5f);
                var fontSize = 10;
                var sizeDelta = new Vector2(50, fontSize + 2);
                var txt = ChartHelper.AddTextObject("title_" + i, titleObject.transform, anchorMin, anchorMax,
                    pivot, sizeDelta, textStyle, theme.common);
                txt.SetText("");
                txt.SetColor(titleColor);
                txt.SetLocalPosition(Vector2.zero);
                txt.SetLocalEulerAngles(Vector2.zero);
                txt.SetActive(serie.titleStyle.show);
                serie.titleStyle.runtimeText = txt;
                serie.titleStyle.UpdatePosition(serie.runtimeCenterPos);
                var serieData = serie.GetSerieData(0);
                if (serieData != null)
                {
                    txt.SetText(serieData.name);
                }
            }
        }

        private void InitTooltip()
        {
            tooltip.painter = m_PainterTop;
            tooltip.refreshComponent = delegate ()
            {
                tooltip.gameObject = ChartHelper.AddObject("tooltip", transform, m_ChartMinAnchor,
                    m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
                var tooltipObject = tooltip.gameObject;
                tooltipObject.transform.localPosition = Vector3.zero;
                tooltipObject.hideFlags = chartHideFlags;
                DestroyImmediate(tooltipObject.GetComponent<Image>());
                var parent = tooltipObject.transform;
                var textStyle = tooltip.textStyle;
                ChartHelper.HideAllObject(tooltipObject.transform);
                GameObject content = ChartHelper.AddTooltipContent("content", parent, textStyle, m_Theme);
                tooltip.SetObj(tooltipObject);
                tooltip.SetContentObj(content);
                tooltip.SetContentBackgroundColor(TooltipHelper.GetTexBackgroundColor(tooltip, m_Theme.tooltip));
                tooltip.SetContentTextColor(TooltipHelper.GetTexColor(tooltip, m_Theme.tooltip));
                tooltip.SetActive(false);
            };
            tooltip.refreshComponent();
        }

        private Vector3 GetLegendPosition(Legend legend, int i)
        {
            return legend.location.GetPosition(chartWidth, chartHeight);
        }

        protected override bool IsNeedCheckPointerPos()
        {
            return (tooltip.show && tooltip.runtimeInited)
                || raycastTarget;
        }

        private void CheckTooltip()
        {
            if (!isPointerInChart || !tooltip.show || !tooltip.runtimeInited)
            {
                if (tooltip.IsActive())
                {
                    tooltip.ClearValue();
                    tooltip.SetActive(false);
                    m_PainterTop.Refresh();
                }
                return;
            }
            for (int i = 0; i < tooltip.runtimeDataIndex.Count; i++)
            {
                tooltip.runtimeDataIndex[i] = -1;
            }
            Vector2 local = pointerPos;
            if (canvas == null) return;

            if (local == Vector2.zero)
            {
                if (tooltip.IsActive())
                {
                    tooltip.SetActive(false);
                    m_PainterTop.Refresh();
                }
                return;
            }
            if (!IsInChart(local))
            {
                if (tooltip.IsActive())
                {
                    tooltip.SetActive(false);
                    m_PainterTop.Refresh();
                }
                return;
            }
            tooltip.runtimePointerPos = local;
            CheckAllTooptip(local);
        }

        private void CheckAllTooptip(Vector2 localPostion)
        {
            tooltip.runtimeGridIndex = -1;
            var actived = false;
            foreach (var draw in m_DrawSeries)
                actived = actived || draw.CheckTootipArea(localPostion);
            CheckTootipArea(localPostion, actived);
        }

        protected virtual void CheckTootipArea(Vector2 localPostion, bool isActivedOther)
        {
        }

        protected override void CheckRefreshChart()
        {
            if (m_Painter == null) return;
            if (m_RefreshChart)
            {
                m_Painter.Refresh();
                foreach (var painter in m_PainterList) painter.Refresh();
                if (m_PainterTop != null) m_PainterTop.Refresh();
                m_RefreshChart = false;
            }
        }

        protected override void CheckRefreshPainter()
        {
            if (m_Painter == null) return;
            m_Painter.CheckRefresh();
            foreach (var painter in m_PainterList) painter.CheckRefresh();
            if (m_PainterTop != null) m_PainterTop.CheckRefresh();
        }

        protected void CheckRefreshLabel()
        {
            if (m_ReinitLabel)
            {
                m_ReinitLabel = false;
                SeriesHelper.UpdateSerieNameList(this, ref m_LegendRealShowName);
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

        public void Internal_CheckAnimation()
        {
            if (!m_CheckAnimation)
            {
                m_CheckAnimation = true;
                AnimationFadeIn();
            }
        }

        protected virtual void OnRefreshLabel()
        {
            foreach (var drawSerie in m_DrawSeries) drawSerie.RefreshLabel();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            m_ChartWidth = m_GraphWidth;
            m_ChartHeight = m_GraphHeight;
            m_ChartX = m_GraphX;
            m_ChartY = m_GraphY;
            m_ChartPosition = m_GraphPosition;
            m_ChartMinAnchor = m_GraphMinAnchor;
            m_ChartMaxAnchor = m_GraphMaxAnchor;
            m_ChartPivot = m_GraphPivot;
            m_ChartSizeDelta = m_GraphSizeDelta;
            m_ChartRect = m_GraphRect;

            SetAllComponentDirty();
            m_Series.SetLabelDirty();
            m_ReinitLabel = true;
            RefreshChart();
        }

        protected override void OnLocalPositionChanged()
        {
            m_Background.SetAllDirty();
        }

        protected virtual void OnThemeChanged()
        {
        }

        protected virtual void OnYMaxValueChanged()
        {
        }

        public virtual void OnDataZoomRangeChanged(DataZoom dataZoom)
        {
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            m_DebugInfo.clickChartCount++;
            base.OnPointerClick(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            foreach (var drawSerie in m_DrawSeries) drawSerie.OnPointerDown(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnPointerDown(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnEndDrag(eventData);
        }

        public override void OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnScroll(eventData);
        }

        protected virtual void OnLegendButtonClick(int index, string legendName, bool show)
        {
            var clicked = false;
            foreach (var drawSerie in m_DrawSeries)
                clicked = clicked || drawSerie.OnLegendButtonClick(index, legendName, show);
            if (!clicked)
            {
                foreach (var serie in m_Series.GetSeries(legendName))
                {
                    SetActive(serie.index, show);
                    RefreshPainter(serie);
                }
                OnYMaxValueChanged();
            }
            if(m_OnLegendClick != null)
                m_OnLegendClick(index, legendName, show);
        }

        protected virtual void OnLegendButtonEnter(int index, string legendName)
        {
            var enter = false;
            foreach (var drawSerie in m_DrawSeries)
                enter = enter || drawSerie.OnLegendButtonEnter(index, legendName);
            if (!enter)
            {
                foreach (var serie in m_Series.GetSeries(legendName))
                {
                    serie.highlighted = true;
                    RefreshPainter(serie);
                }
            }
            if(m_OnLegendEnter != null)
                m_OnLegendEnter(index, legendName);
        }

        protected virtual void OnLegendButtonExit(int index, string legendName)
        {
            var exit = false;
            foreach (var drawSerie in m_DrawSeries)
                exit = exit || drawSerie.OnLegendButtonExit(index, legendName);
            if (!exit)
            {
                foreach (var serie in m_Series.GetSeries(legendName))
                {
                    serie.highlighted = false;
                    RefreshPainter(serie);
                }
            }
            if(m_OnLegendExit != null)
                m_OnLegendExit(index, legendName);
        }

        protected virtual void UpdateTooltip()
        {
        }

        protected override void OnDrawPainterBase(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            DrawBackground(vh);
            DrawPainterBase(vh);
            foreach (var draw in m_ComponentHandlers) draw.DrawBase(vh);
            foreach (var draw in m_DrawSeries) draw.DrawBase(vh);
            if (m_OnCustomDrawBaseCallback != null)
            {
                m_OnCustomDrawBaseCallback(vh);
            }
            m_BasePainterVertCount = vh.currentVertCount;
        }

        protected virtual void OnDrawPainterSerie(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            var maxPainter = settings.maxPainter;
            var maxSeries = m_Series.Count;
            var rate = Mathf.CeilToInt(maxSeries * 1.0f / maxPainter);
            m_PainterTop.Refresh();
            m_DebugInfo.refreshCount++;
            for (int i = painter.index * rate; i < (painter.index + 1) * rate && i < maxSeries; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (m_OnCustomDrawSerieBeforeCallback != null)
                {
                    m_OnCustomDrawSerieBeforeCallback.Invoke(vh, serie);
                }
                DrawPainterSerie(vh, serie);
                if (m_OnCustomDrawSerieAfterCallback != null)
                {
                    m_OnCustomDrawSerieAfterCallback(vh, serie);
                }
                serie.runtimeVertCount = vh.currentVertCount;
            }
            m_RefreshLabel = true;
        }

        protected virtual void OnDrawPainterTop(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            DrawLegend(vh);
            DrawPainterTop(vh);
            foreach (var draw in m_ComponentHandlers) draw.DrawTop(vh);
            if (m_OnCustomDrawTopCallback != null)
            {
                m_OnCustomDrawTopCallback(vh);
            }
            DrawTooltip(vh);
            m_TopPainterVertCount = vh.currentVertCount;
        }

        protected virtual void DrawPainterSerie(VertexHelper vh, Serie serie)
        {
            foreach (var drawSerie in m_DrawSeries)
            {
                drawSerie.DrawSerie(vh, serie);
            }
        }

        protected virtual void DrawPainterTop(VertexHelper vh)
        {
        }

        protected virtual void DrawTooltip(VertexHelper vh)
        {
        }

        protected override void DrawBackground(VertexHelper vh)
        {
            Vector3 p1 = new Vector3(chartX, chartY + chartHeight);
            Vector3 p2 = new Vector3(chartX + chartWidth, chartY + chartHeight);
            Vector3 p3 = new Vector3(chartX + chartWidth, chartY);
            Vector3 p4 = new Vector3(chartX, chartY);
            var backgroundColor = ThemeHelper.GetBackgroundColor(m_Theme, m_Background);
            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, backgroundColor);
        }

        protected virtual void DrawLegend(VertexHelper vh)
        {
            if (m_Series.Count == 0) return;
            foreach (var legend in m_Legends)
            {
                if (!legend.show) continue;
                if (legend.iconType == Legend.Type.Custom) continue;
                foreach (var kv in legend.buttonList)
                {
                    var item = kv.Value;
                    var rect = item.GetIconRect();
                    var radius = Mathf.Min(rect.width, rect.height) / 2;
                    var color = item.GetIconColor();
                    var iconType = legend.iconType;
                    if (legend.iconType == Legend.Type.Auto)
                    {
                        var serie = m_Series.GetSerie(item.legendName);
                        if (serie != null && serie.type == SerieType.Line)
                        {
                            var sp = new Vector3(rect.center.x - rect.width / 2, rect.center.y);
                            var ep = new Vector3(rect.center.x + rect.width / 2, rect.center.y);
                            UGL.DrawLine(vh, sp, ep, m_Settings.legendIconLineWidth, color);
                            if (!serie.symbol.show) continue;
                            switch (serie.symbol.type)
                            {
                                case SerieSymbolType.None:
                                    continue;
                                case SerieSymbolType.Circle:
                                    iconType = Legend.Type.Circle;
                                    break;
                                case SerieSymbolType.Diamond:
                                    iconType = Legend.Type.Diamond;
                                    break;
                                case SerieSymbolType.EmptyCircle:
                                    iconType = Legend.Type.EmptyCircle;
                                    break;
                                case SerieSymbolType.Rect:
                                    iconType = Legend.Type.Rect;
                                    break;
                                case SerieSymbolType.Triangle:
                                    iconType = Legend.Type.Triangle;
                                    break;
                            }
                        }
                        else
                        {
                            iconType = Legend.Type.Rect;
                        }
                    }
                    switch (iconType)
                    {
                        case Legend.Type.Rect:
                            var cornerRadius = m_Settings.legendIconCornerRadius;
                            UGL.DrawRoundRectangle(vh, rect.center, rect.width, rect.height, color, color,
                                0, cornerRadius, false, 0.5f);
                            break;
                        case Legend.Type.Circle:
                            UGL.DrawCricle(vh, rect.center, radius, color);
                            break;
                        case Legend.Type.Diamond:
                            UGL.DrawDiamond(vh, rect.center, radius, color);
                            break;
                        case Legend.Type.EmptyCircle:
                            var backgroundColor = ThemeHelper.GetBackgroundColor(m_Theme, m_Background);
                            UGL.DrawEmptyCricle(vh, rect.center, radius, 2 * m_Settings.legendIconLineWidth,
                                color, color, backgroundColor, 1f);
                            break;
                        case Legend.Type.Triangle:
                            UGL.DrawTriangle(vh, rect.center, 1.2f * radius, color);
                            break;
                    }
                }
            }
        }

        public void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
          float tickness, Vector3 pos, Color32 color, Color32 toColor, Color32 fillColor, float gap, float[] cornerRadius)
        {
            DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, fillColor, gap, cornerRadius, Vector3.zero);
        }

        public void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
          float tickness, Vector3 pos, Color32 color, Color32 toColor, Color32 fillColor, float gap, float[] cornerRadius, Vector3 startPos)
        {
            var backgroundColor = ThemeHelper.GetBackgroundColor(m_Theme, m_Background);
            if (ChartHelper.IsClearColor(fillColor))
                fillColor = backgroundColor;
            var smoothness = settings.cicleSmoothness;
            ChartDrawer.DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, gap,
                cornerRadius, fillColor, backgroundColor, smoothness, startPos);
        }

        public void DrawLabelBackground(VertexHelper vh, Serie serie, SerieData serieData)
        {
            if (serieData == null || serieData.labelObject == null) return;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (!serieLabel.show) return;
            var invert = serieLabel.autoOffset
                && serie.type == SerieType.Line
                && SerieHelper.IsDownPoint(serie, serieData.index)
                && !serie.areaStyle.show;
            var centerPos = Vector3.zero;
            if (serie.type == SerieType.Pie)
                centerPos = SerieLabelHelper.GetRealLabelPosition(serieData, serieLabel);
            else
                centerPos = serieData.labelPosition + serieLabel.offset * (invert ? -1 : 1);
            var labelHalfWid = serieData.labelObject.GetLabelWidth() / 2;
            var labelHalfHig = serieData.GetLabelHeight() / 2;
            var p1 = new Vector3(centerPos.x - labelHalfWid, centerPos.y + labelHalfHig);
            var p2 = new Vector3(centerPos.x + labelHalfWid, centerPos.y + labelHalfHig);
            var p3 = new Vector3(centerPos.x + labelHalfWid, centerPos.y - labelHalfHig);
            var p4 = new Vector3(centerPos.x - labelHalfWid, centerPos.y - labelHalfHig);

            if (serieLabel.textStyle.rotate > 0)
            {
                p1 = ChartHelper.RotateRound(p1, centerPos, Vector3.forward, serieLabel.textStyle.rotate);
                p2 = ChartHelper.RotateRound(p2, centerPos, Vector3.forward, serieLabel.textStyle.rotate);
                p3 = ChartHelper.RotateRound(p3, centerPos, Vector3.forward, serieLabel.textStyle.rotate);
                p4 = ChartHelper.RotateRound(p4, centerPos, Vector3.forward, serieLabel.textStyle.rotate);
            }

            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, serieLabel.textStyle.backgroundColor);

            if (serieLabel.border)
            {
                UGL.DrawBorder(vh, centerPos, serieData.GetLabelWidth(), serieData.GetLabelHeight(),
                    serieLabel.borderWidth, serieLabel.borderColor, serieLabel.textStyle.rotate);
            }
        }

        protected int GetPainterIndexBySerie(Serie serie)
        {
            var maxPainter = settings.maxPainter;
            var maxSeries = m_Series.Count;
            if (maxPainter >= maxSeries) return serie.index;
            else
            {
                var rate = Mathf.CeilToInt(maxSeries * 1.0f / maxPainter);
                return serie.index / rate;
            }
        }
    }
}
