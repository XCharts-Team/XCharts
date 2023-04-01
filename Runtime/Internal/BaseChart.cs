using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/EmptyChart", 10)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform),typeof(CanvasRenderer))]
    [DisallowMultipleComponent]
    public partial class BaseChart : BaseGraph, ISerializationCallbackReceiver
    {
        [SerializeField] protected string m_ChartName;
        [SerializeField] protected ThemeStyle m_Theme = new ThemeStyle();
        [SerializeField] protected Settings m_Settings;
        [SerializeField] protected DebugInfo m_DebugInfo = new DebugInfo();

#pragma warning disable 0414
        [SerializeField][ListForComponent(typeof(AngleAxis))] private List<AngleAxis> m_AngleAxes = new List<AngleAxis>();
        [SerializeField][ListForComponent(typeof(Background))] private List<Background> m_Backgrounds = new List<Background>();
        [SerializeField][ListForComponent(typeof(DataZoom))] private List<DataZoom> m_DataZooms = new List<DataZoom>();
        [SerializeField][ListForComponent(typeof(GridCoord))] private List<GridCoord> m_Grids = new List<GridCoord>();
        [SerializeField][ListForComponent(typeof(Legend))] private List<Legend> m_Legends = new List<Legend>();
        [SerializeField][ListForComponent(typeof(MarkLine))] private List<MarkLine> m_MarkLines = new List<MarkLine>();
        [SerializeField][ListForComponent(typeof(MarkArea))] private List<MarkArea> m_MarkAreas = new List<MarkArea>();
        [SerializeField][ListForComponent(typeof(PolarCoord))] private List<PolarCoord> m_Polars = new List<PolarCoord>();
        [SerializeField][ListForComponent(typeof(RadarCoord))] private List<RadarCoord> m_Radars = new List<RadarCoord>();
        [SerializeField][ListForComponent(typeof(RadiusAxis))] private List<RadiusAxis> m_RadiusAxes = new List<RadiusAxis>();
        [SerializeField][ListForComponent(typeof(Title))] private List<Title> m_Titles = new List<Title>();
        [SerializeField][ListForComponent(typeof(Tooltip))] private List<Tooltip> m_Tooltips = new List<Tooltip>();
        [SerializeField][ListForComponent(typeof(VisualMap))] private List<VisualMap> m_VisualMaps = new List<VisualMap>();
        [SerializeField][ListForComponent(typeof(XAxis))] private List<XAxis> m_XAxes = new List<XAxis>();
        [SerializeField][ListForComponent(typeof(YAxis))] private List<YAxis> m_YAxes = new List<YAxis>();
        [SerializeField][ListForComponent(typeof(SingleAxis))] private List<SingleAxis> m_SingleAxes = new List<SingleAxis>();
        [SerializeField][ListForComponent(typeof(ParallelCoord))] private List<ParallelCoord> m_Parallels = new List<ParallelCoord>();
        [SerializeField][ListForComponent(typeof(ParallelAxis))] private List<ParallelAxis> m_ParallelAxes = new List<ParallelAxis>();
        [SerializeField][ListForComponent(typeof(Comment))] private List<Comment> m_Comments = new List<Comment>();

        [SerializeField][ListForSerie(typeof(Bar))] private List<Bar> m_SerieBars = new List<Bar>();
        [SerializeField][ListForSerie(typeof(Candlestick))] private List<Candlestick> m_SerieCandlesticks = new List<Candlestick>();
        [SerializeField][ListForSerie(typeof(EffectScatter))] private List<EffectScatter> m_SerieEffectScatters = new List<EffectScatter>();
        [SerializeField][ListForSerie(typeof(Heatmap))] private List<Heatmap> m_SerieHeatmaps = new List<Heatmap>();
        [SerializeField][ListForSerie(typeof(Line))] private List<Line> m_SerieLines = new List<Line>();
        [SerializeField][ListForSerie(typeof(Pie))] private List<Pie> m_SeriePies = new List<Pie>();
        [SerializeField][ListForSerie(typeof(Radar))] private List<Radar> m_SerieRadars = new List<Radar>();
        [SerializeField][ListForSerie(typeof(Ring))] private List<Ring> m_SerieRings = new List<Ring>();
        [SerializeField][ListForSerie(typeof(Scatter))] private List<Scatter> m_SerieScatters = new List<Scatter>();
        [SerializeField][ListForSerie(typeof(Parallel))] private List<Parallel> m_SerieParallels = new List<Parallel>();
        [SerializeField][ListForSerie(typeof(SimplifiedLine))] private List<SimplifiedLine> m_SerieSimplifiedLines = new List<SimplifiedLine>();
        [SerializeField][ListForSerie(typeof(SimplifiedBar))] private List<SimplifiedBar> m_SerieSimplifiedBars = new List<SimplifiedBar>();
        [SerializeField][ListForSerie(typeof(SimplifiedCandlestick))] private List<SimplifiedCandlestick> m_SerieSimplifiedCandlesticks = new List<SimplifiedCandlestick>();
#pragma warning restore 0414
        protected List<Serie> m_Series = new List<Serie>();
        protected List<MainComponent> m_Components = new List<MainComponent>();

        protected Dictionary<Type, FieldInfo> m_TypeListForComponent = new Dictionary<Type, FieldInfo>();
        protected Dictionary<Type, FieldInfo> m_TypeListForSerie = new Dictionary<Type, FieldInfo>();

        protected Dictionary<Type, List<MainComponent>> m_ComponentMaps = new Dictionary<Type, List<MainComponent>>();

        public Dictionary<Type, FieldInfo> typeListForComponent { get { return m_TypeListForComponent; } }
        public Dictionary<Type, FieldInfo> typeListForSerie { get { return m_TypeListForSerie; } }
        public List<MainComponent> components { get { return m_Components; } }

        public List<Serie> series { get { return m_Series; } }
        public DebugInfo debug { get { return m_DebugInfo; } }
        public override HideFlags chartHideFlags { get { return m_DebugInfo.showAllChartObject ? HideFlags.None : HideFlags.HideInHierarchy; } }

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
        protected Action m_OnInit;
        protected Action m_OnUpdate;
        protected Action<VertexHelper> m_OnDrawBase;
        protected Action<VertexHelper> m_OnDrawUpper;
        protected Action<VertexHelper> m_OnDrawTop;
        protected Action<VertexHelper, Serie> m_OnDrawSerieBefore;
        protected Action<VertexHelper, Serie> m_OnDrawSerieAfter;
        protected Action<SerieEventData> m_OnSerieClick;
        protected Action<SerieEventData> m_OnSerieDown;
        protected Action<SerieEventData> m_OnSerieEnter;
        protected Action<SerieEventData> m_OnSerieExit;
        protected Action<int, int> m_OnPointerEnterPie;
        protected Action<Axis, double> m_OnAxisPointerValueChanged;
        protected Action<Legend, int, string, bool> m_OnLegendClick;
        protected Action<Legend, int, string> m_OnLegendEnter;
        protected Action<Legend, int, string> m_OnLegendExit;

        protected CustomDrawGaugePointerFunction m_CustomDrawGaugePointerFunction;

        internal bool m_CheckAnimation = false;
        internal protected List<string> m_LegendRealShowName = new List<string>();
        protected List<Painter> m_PainterList = new List<Painter>();
        internal Painter m_PainterUpper;
        internal Painter m_PainterTop;
        internal int m_BasePainterVertCount;
        internal int m_UpperPainterVertCount;
        internal int m_TopPainterVertCount;

        private ThemeType m_CheckTheme = 0;
        protected List<MainComponentHandler> m_ComponentHandlers = new List<MainComponentHandler>();
        protected List<SerieHandler> m_SerieHandlers = new List<SerieHandler>();

        protected virtual void DefaultChart() { }

        protected override void InitComponent()
        {
            base.InitComponent();
            SeriesHelper.UpdateSerieNameList(this, ref m_LegendRealShowName);
            foreach (var handler in m_ComponentHandlers)
                handler.InitComponent();
            foreach (var handler in m_SerieHandlers)
                handler.InitComponent();
            m_DebugInfo.Init(this);
        }

        protected override void Awake()
        {
            if (m_Settings == null)
                m_Settings = Settings.DefaultSettings;
            CheckTheme();
            base.Awake();
            InitComponentHandlers();
            InitSerieHandlers();
            AnimationReset();
            AnimationFadeIn();
            XChartsMgr.AddChart(this);
        }

        protected void OnInit()
        {
            RemoveAllChartComponent();
            OnBeforeSerialize();
            AddChartComponentWhenNoExist<Title>();
            AddChartComponentWhenNoExist<Tooltip>();

            GetChartComponent<Title>().text = GetType().Name;

            if (m_Theme.sharedTheme != null)
                m_Theme.sharedTheme.CopyTheme(ThemeType.Default);
            else
                m_Theme.sharedTheme = XCThemeMgr.GetTheme(ThemeType.Default);

            var sizeDelta = rectTransform.sizeDelta;
            if (sizeDelta.x < 580 && sizeDelta.y < 300)
            {
                rectTransform.sizeDelta = new Vector2(580, 300);
            }
            ChartHelper.HideAllObject(transform);
            if (m_OnInit != null)
                m_OnInit();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            OnInit();
            DefaultChart();
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
            CheckRefreshChart();
            Internal_CheckAnimation();
            foreach (var handler in m_SerieHandlers) handler.Update();
            foreach (var handler in m_ComponentHandlers) handler.Update();
            m_DebugInfo.Update();
            if (m_OnUpdate != null)
                m_OnUpdate();
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

        public void RefreshUpperPainter()
        {
            m_PainterUpper.Refresh();
        }

        public void RefreshPainter(int index)
        {
            var painter = GetPainter(index);
            RefreshPainter(painter);
        }

        public void RefreshPainter(Serie serie)
        {
            if (serie == null) return;
            RefreshPainter(GetPainterIndexBySerie(serie));
        }

        internal override void RefreshPainter(Painter painter)
        {
            base.RefreshPainter(painter);
            if (painter != null && painter.type == Painter.Type.Serie)
            {
                m_PainterUpper.Refresh();
            }
        }

        public void SetPainterActive(int index, bool flag)
        {
            var painter = GetPainter(index);
            if (painter == null) return;
            painter.SetActive(flag, m_DebugInfo.showAllChartObject);
        }

        protected virtual void CheckTheme()
        {
            if (m_Theme.sharedTheme == null)
            {
                m_Theme.sharedTheme = XCThemeMgr.GetTheme(ThemeType.Default);
            }
            if (m_Theme.sharedTheme != null && m_CheckTheme != m_Theme.themeType)
            {
                m_CheckTheme = m_Theme.themeType;
                m_Theme.sharedTheme.CopyTheme(m_CheckTheme);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
                SetAllComponentDirty();
                OnThemeChanged();
            }
        }
        protected override void CheckComponent()
        {
            base.CheckComponent();
            if (m_Theme.anyDirty)
            {
                if (m_Theme.componentDirty)
                {
                    SetAllComponentDirty();
                }
                if (m_Theme.vertsDirty) RefreshChart();
                m_Theme.ClearDirty();
            }
            foreach (var com in m_Components)
                CheckComponentDirty(com);
        }

        protected void CheckComponentDirty(MainComponent component)
        {
            if (component == null) return;
            if (component.anyDirty)
            {
                if (component.componentDirty)
                {
                    if (component.refreshComponent != null)
                        component.refreshComponent.Invoke();
                    else
                        component.handler.InitComponent();
                }
                if (component.vertsDirty)
                {
                    if (component.painter != null)
                    {
                        RefreshPainter(component.painter);
                    }
                }
                component.ClearDirty();
            }
        }

        protected override void SetAllComponentDirty()
        {
            base.SetAllComponentDirty();
            m_Theme.SetAllDirty();
            foreach (var com in m_Components) com.SetAllDirty();
            foreach (var handler in m_SerieHandlers) handler.InitComponent();
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
                var serie = m_Series[i];
                serie.index = i;
                SetPainterActive(i, true);
            }
            if (transform.childCount - 3 != m_PainterTop.transform.GetSiblingIndex())
            {
                m_PainterTop.transform.SetSiblingIndex(transform.childCount - 3);
            }
        }

        protected override void InitPainter()
        {
            base.InitPainter();
            if (settings == null) return;
            m_Painter.material = settings.basePainterMaterial;
            m_PainterList.Clear();
            var sizeDelta = new Vector2(m_GraphWidth, m_GraphHeight);
            for (int i = 0; i < settings.maxPainter; i++)
            {
                var index = settings.reversePainter ? settings.maxPainter - 1 - i : i;
                var painter = ChartHelper.AddPainterObject("painter_" + index, transform, m_GraphMinAnchor,
                    m_GraphMaxAnchor, m_GraphPivot, sizeDelta, chartHideFlags, 2 + index);
                painter.index = m_PainterList.Count;
                painter.type = Painter.Type.Serie;
                painter.onPopulateMesh = OnDrawPainterSerie;
                painter.SetActive(false, m_DebugInfo.showAllChartObject);
                painter.material = settings.seriePainterMaterial;
                painter.transform.SetSiblingIndex(index + 1);
                m_PainterList.Add(painter);
            }
            m_PainterUpper = ChartHelper.AddPainterObject("painter_u", transform, m_GraphMinAnchor,
                m_GraphMaxAnchor, m_GraphPivot, sizeDelta, chartHideFlags, 2 + settings.maxPainter);
            m_PainterUpper.type = Painter.Type.Top;
            m_PainterUpper.onPopulateMesh = OnDrawPainterUpper;
            m_PainterUpper.SetActive(true, m_DebugInfo.showAllChartObject);
            m_PainterUpper.material = settings.topPainterMaterial;
            m_PainterUpper.transform.SetSiblingIndex(settings.maxPainter + 1);

            m_PainterTop = ChartHelper.AddPainterObject("painter_t", transform, m_GraphMinAnchor,
                m_GraphMaxAnchor, m_GraphPivot, sizeDelta, chartHideFlags, 2 + settings.maxPainter);
            m_PainterTop.type = Painter.Type.Top;
            m_PainterTop.onPopulateMesh = OnDrawPainterTop;
            m_PainterTop.SetActive(true, m_DebugInfo.showAllChartObject);
            m_PainterTop.material = settings.topPainterMaterial;
            m_PainterTop.transform.SetSiblingIndex(settings.maxPainter + 1);
        }

        internal void InitComponentHandlers()
        {
            m_ComponentHandlers.Clear();
            m_Components.Sort();
            m_ComponentMaps.Clear();
            foreach (var component in m_Components)
            {
                var type = component.GetType();
                List<MainComponent> list;
                if (!m_ComponentMaps.TryGetValue(type, out list))
                {
                    list = new List<MainComponent>();
                    m_ComponentMaps[type] = list;
                }
                component.index = list.Count;
                list.Add(component);
                CreateComponentHandler(component);
            }
        }

        protected override void CheckRefreshChart()
        {
            if (m_Painter == null) return;
            if (m_RefreshChart)
            {
                CheckRefreshPainter();
                m_RefreshChart = false;
            }
        }

        protected override void CheckRefreshPainter()
        {
            if (m_Painter == null) return;
            m_Painter.CheckRefresh();
            foreach (var painter in m_PainterList) painter.CheckRefresh();
            if (m_PainterUpper != null) m_PainterUpper.CheckRefresh();
            if (m_PainterTop != null) m_PainterTop.CheckRefresh();
        }

        public void Internal_CheckAnimation()
        {
            if (!m_CheckAnimation)
            {
                m_CheckAnimation = true;
                AnimationFadeIn();
            }
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
            OnCoordinateChanged();
            RefreshChart();
        }

        internal virtual void OnSerieDataUpdate(int serieIndex)
        {
            foreach (var handler in m_ComponentHandlers) handler.OnSerieDataUpdate(serieIndex);
        }

        internal virtual void OnCoordinateChanged()
        {
            foreach (var component in m_Components)
            {
                if (component is Axis)
                    component.SetAllDirty();
                if (component is IUpdateRuntimeData)
                    (component as IUpdateRuntimeData).UpdateRuntimeData(m_ChartX, m_ChartY, m_ChartWidth, m_ChartHeight);
            }
        }

        protected override void OnLocalPositionChanged()
        {
            Background background;
            if (TryGetChartComponent<Background>(out background))
                background.SetAllDirty();
        }

        protected virtual void OnThemeChanged() { }

        public virtual void OnDataZoomRangeChanged(DataZoom dataZoom)
        {
            foreach (var index in dataZoom.xAxisIndexs)
            {
                var axis = GetChartComponent<XAxis>(index);
                if (axis != null && axis.show) axis.SetAllDirty();
            }
            foreach (var index in dataZoom.yAxisIndexs)
            {
                var axis = GetChartComponent<YAxis>(index);
                if (axis != null && axis.show) axis.SetAllDirty();
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            m_DebugInfo.clickChartCount++;
            base.OnPointerClick(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnPointerClick(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnPointerClick(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnPointerDown(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnPointerUp(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnPointerUp(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnPointerEnter(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnPointerExit(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnPointerExit(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnBeginDrag(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnDrag(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnEndDrag(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnEndDrag(eventData);
        }

        public override void OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
            foreach (var handler in m_SerieHandlers) handler.OnScroll(eventData);
            foreach (var handler in m_ComponentHandlers) handler.OnScroll(eventData);
        }

        public virtual void OnLegendButtonClick(int index, string legendName, bool show)
        {
            foreach (var handler in m_SerieHandlers)
                handler.OnLegendButtonClick(index, legendName, show);
        }

        public virtual void OnLegendButtonEnter(int index, string legendName)
        {
            foreach (var handler in m_SerieHandlers)
                handler.OnLegendButtonEnter(index, legendName);
        }

        public virtual void OnLegendButtonExit(int index, string legendName)
        {
            foreach (var handler in m_SerieHandlers)
                handler.OnLegendButtonExit(index, legendName);
        }

        protected override void OnDrawPainterBase(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            DrawBackground(vh);
            DrawPainterBase(vh);
            foreach (var handler in m_ComponentHandlers) handler.DrawBase(vh);
            foreach (var handler in m_SerieHandlers) handler.DrawBase(vh);
            if (m_OnDrawBase != null)
            {
                m_OnDrawBase(vh);
            }
            m_BasePainterVertCount = vh.currentVertCount;
        }

        protected virtual void OnDrawPainterSerie(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            var maxPainter = settings.maxPainter;
            var maxSeries = m_Series.Count;
            var rate = Mathf.CeilToInt(maxSeries * 1.0f / maxPainter);
            m_PainterUpper.Refresh();
            m_PainterTop.Refresh();
            m_DebugInfo.refreshCount++;
            for (int i = painter.index * rate; i < (painter.index + 1) * rate && i < maxSeries; i++)
            {
                var serie = m_Series[i];
                serie.context.colorIndex = GetLegendRealShowNameIndex(serie.legendName);
                serie.context.dataPoints.Clear();
                serie.context.dataIndexs.Clear();
                serie.context.dataIgnores.Clear();
                serie.animation.context.isAllItemAnimationEnd = true;
                if (serie.show && !serie.animation.HasFadeOut())
                {
                    if (!serie.context.pointerEnter)
                        serie.ResetInteract();
                    if (m_OnDrawSerieBefore != null)
                    {
                        m_OnDrawSerieBefore.Invoke(vh, serie);
                    }
                    DrawPainterSerie(vh, serie);
                    if (i >= 0 && i < m_SerieHandlers.Count)
                    {
                        var handler = m_SerieHandlers[i];
                        handler.DrawSerie(vh);
                        handler.RefreshLabelNextFrame();
                    }
                    if (m_OnDrawSerieAfter != null)
                    {
                        m_OnDrawSerieAfter(vh, serie);
                    }
                }
                serie.context.vertCount = vh.currentVertCount;
            }
        }

        protected virtual void OnDrawPainterUpper(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            DrawPainterUpper(vh);
            foreach (var draw in m_ComponentHandlers) draw.DrawUpper(vh);
            if (m_OnDrawUpper != null)
            {
                m_OnDrawUpper(vh);
            }
            m_UpperPainterVertCount = vh.currentVertCount;
        }

        protected virtual void OnDrawPainterTop(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            DrawPainterTop(vh);
            foreach (var draw in m_ComponentHandlers) draw.DrawTop(vh);
            if (m_OnDrawTop != null)
            {
                m_OnDrawTop(vh);
            }
            m_TopPainterVertCount = vh.currentVertCount;
        }

        protected virtual void DrawPainterSerie(VertexHelper vh, Serie serie) { }

        protected virtual void DrawPainterUpper(VertexHelper vh)
        {
            foreach (var handler in m_SerieHandlers)
                handler.DrawUpper(vh);
        }

        protected virtual void DrawPainterTop(VertexHelper vh)
        {
            foreach (var handler in m_SerieHandlers)
                handler.DrawTop(vh);
        }

        protected virtual void DrawBackground(VertexHelper vh)
        {
            var background = GetChartComponent<Background>();
            if (background != null && background.show)
                return;
            Vector3 p1 = new Vector3(chartX, chartY + chartHeight);
            Vector3 p2 = new Vector3(chartX + chartWidth, chartY + chartHeight);
            Vector3 p3 = new Vector3(chartX + chartWidth, chartY);
            Vector3 p4 = new Vector3(chartX, chartY);
            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, theme.backgroundColor);
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

        private void InitListForFieldInfos()
        {
            if (m_TypeListForSerie.Count != 0) return;
            m_TypeListForComponent.Clear();
            m_TypeListForSerie.Clear();
            var fileds1 = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var fileds2 = GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var list = ListPool<FieldInfo>.Get();
            list.AddRange(fileds1);
            list.AddRange(fileds2);
            foreach (var field in list)
            {
                var attribute1 = field.GetAttribute<ListForSerie>(false);
                if (attribute1 != null)
                    m_TypeListForSerie.Add(attribute1.type, field);

                var attribute2 = field.GetAttribute<ListForComponent>(false);
                if (attribute2 != null)
                    m_TypeListForComponent.Add(attribute2.type, field);
            }
            ListPool<FieldInfo>.Release(list);
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
            if (!UnityEditor.EditorUtility.IsDirty(this))
                return;
            UnityEditor.EditorUtility.ClearDirty(this);
#endif
            InitListForFieldInfos();
            foreach (var kv in m_TypeListForSerie)
            {
                ReflectionUtil.InvokeListClear(this, kv.Value);
            }
            foreach (var kv in m_TypeListForComponent)
            {
                ReflectionUtil.InvokeListClear(this, kv.Value);
            }
            foreach (var component in m_Components)
            {
                FieldInfo field;
                if (m_TypeListForComponent.TryGetValue(component.GetType(), out field))
                    ReflectionUtil.InvokeListAdd(this, field, component);
                else
                    Debug.LogError("No ListForComponent:" + component.GetType());
            }
            foreach (var serie in m_Series)
            {
                FieldInfo field;
                serie.OnBeforeSerialize();
                if (m_TypeListForSerie.TryGetValue(serie.GetType(), out field))
                    ReflectionUtil.InvokeListAdd(this, field, serie);
                else
                    Debug.LogError("No ListForSerie:" + serie.GetType());
            }
        }

        public void OnAfterDeserialize()
        {
            InitListForFieldInfos();
            m_Components.Clear();
            m_Series.Clear();
            foreach (var kv in m_TypeListForComponent)
            {
                ReflectionUtil.InvokeListAddTo<MainComponent>(this, kv.Value, AddComponent);
            }
            foreach (var kv in m_TypeListForSerie)
            {
                ReflectionUtil.InvokeListAddTo<Serie>(this, kv.Value, AddSerieAfterDeserialize);
            }
            m_Series.Sort();
            m_Components.Sort();
            InitComponentHandlers();
            InitSerieHandlers();
        }
    }
}