using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public abstract class SerieHandler
    {
        public BaseChart chart { get; internal set; }
        public SerieHandlerAttribute attribute { get; internal set; }
        public bool inited { get; internal set; }
        public virtual int defaultDimension { get; internal set; }

        public virtual void InitComponent() { }
        public virtual void RemoveComponent() { }
        public virtual void CheckComponent(StringBuilder sb) { }
        public virtual void BeforeUpdate() { }
        public virtual void Update() { }
        public virtual void AfterUpdate() { }
        public virtual void DrawBase(VertexHelper vh) { }
        public virtual void DrawSerie(VertexHelper vh) { }
        public virtual void DrawUpper(VertexHelper vh) { }
        public virtual void DrawTop(VertexHelper vh) { }
        public virtual void OnPointerClick(PointerEventData eventData) { }
        public virtual void OnPointerDown(PointerEventData eventData) { }
        public virtual void OnPointerUp(PointerEventData eventData) { }
        public virtual void OnPointerEnter(PointerEventData eventData) { }
        public virtual void OnPointerExit(PointerEventData eventData) { }
        public virtual void OnDrag(PointerEventData eventData) { }
        public virtual void OnBeginDrag(PointerEventData eventData) { }
        public virtual void OnEndDrag(PointerEventData eventData) { }
        public virtual void OnScroll(PointerEventData eventData) { }
        public virtual void OnDataUpdate() { }
        public virtual void RefreshLabelNextFrame() { }
        public virtual void RefreshLabelInternal() { }
        public virtual void ForceUpdateSerieContext() { }
        public virtual void UpdateSerieContext() { }
        public virtual void UpdateTooltipSerieParams(int dataIndex, bool showCategory,
            string category, string marker,
            string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        { }
        public virtual void OnLegendButtonClick(int index, string legendName, bool show) { }
        public virtual void OnLegendButtonEnter(int index, string legendName) { }
        public virtual void OnLegendButtonExit(int index, string legendName) { }
        internal abstract void SetSerie(Serie serie);
        public virtual int GetPointerItemDataIndex() { return -1; }
        public virtual int GetPointerItemDataDimension() { return 1; }
    }

    public abstract class SerieHandler<T> : SerieHandler where T : Serie
    {
        private static readonly string s_SerieLabelObjectName = "label";
        private static readonly string s_SerieTitleObjectName = "title";
        private static readonly string s_SerieRootObjectName = "serie";
        private static readonly string s_SerieEndLabelObjectName = "end_label";
        protected GameObject m_SerieRoot;
        protected GameObject m_SerieLabelRoot;
        protected bool m_InitedLabel;
        protected bool m_NeedInitComponent;
        protected bool m_RefreshLabel;
        protected bool m_LastCheckContextFlag = false;
        protected bool m_LegendEnter = false;
        protected bool m_LegendExiting = false;
        protected bool m_ForceUpdateSerieContext = false;
        protected int m_LegendEnterIndex;
        protected ChartLabel m_EndLabel;

        private float[] m_LastRadius = new float[2] { 0, 0 };
        private float[] m_LastCenter = new float[2] { 0, 0 };
        private bool m_LastPointerEnter;
        private int m_LastPointerDataIndex;
        private int m_LastPointerDataDimension;

        public T serie { get; internal set; }
        public GameObject labelObject { get { return m_SerieLabelRoot; } }

        internal override void SetSerie(Serie serie)
        {
            this.serie = (T)serie;
            this.serie.context.param.serieType = typeof(T);
            m_NeedInitComponent = true;
            AnimationStyleHelper.UpdateSerieAnimation(serie);
        }

        public override void BeforeUpdate()
        {
            m_LastPointerEnter = serie.context.pointerEnter;
            m_LastPointerDataIndex = serie.context.pointerItemDataIndex;
            m_LastPointerDataDimension = GetPointerItemDataDimension();
            serie.context.pointerEnter = false;
            serie.context.pointerItemDataIndex = -1;
        }

        public override void Update()
        {
            CheckConfigurationChanged();
            if (m_NeedInitComponent)
            {
                m_NeedInitComponent = false;
                InitComponent();
            }
            if (m_RefreshLabel)
            {
                m_RefreshLabel = false;
                RefreshLabelInternal();
                RefreshEndLabelInternal();
            }
            if (serie.dataDirty)
            {
                OnDataUpdate();
                SeriesHelper.UpdateSerieNameList(chart, ref chart.m_LegendRealShowName);
                chart.OnSerieDataUpdate(serie.index);
                serie.OnDataUpdate();
                serie.dataDirty = false;
            }
            if (serie.label != null && (serie.labelDirty || serie.label.componentDirty))
            {
                serie.labelDirty = false;
                serie.label.ClearComponentDirty();
                InitSerieLabel();
                InitSerieEndLabel();
            }
            if (serie.endLabel != null && serie.endLabel.componentDirty)
            {
                serie.endLabel.ClearComponentDirty();
                InitSerieEndLabel();
            }
            if (serie.titleStyle != null && (serie.titleDirty || serie.titleStyle.componentDirty))
            {
                serie.titleDirty = false;
                serie.titleStyle.ClearComponentDirty();
                InitSerieTitle();
            }
            if (serie.nameDirty)
            {
                foreach (var component in chart.components)
                {
                    if (component is Legend)
                        component.SetAllDirty();
                }
                chart.RefreshChart();
                serie.ClearSerieNameDirty();
            }
            if (serie.vertsDirty)
            {
                chart.RefreshPainter(serie);
                serie.ClearVerticesDirty();
            }
            if (serie.interactDirty)
            {
                if (serie.animation.enable && serie.animation.interaction.enable)
                {
                    Color32 color1, toColor1;
                    bool needInteract = false;
                    serie.context.colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
                    foreach (var serieData in serie.data)
                    {
                        var state = SerieHelper.GetSerieState(serie, serieData, true);
                        SerieHelper.GetItemColor(out color1, out toColor1, serie, serieData, chart.theme, state);
                        serieData.interact.SetColor(ref needInteract, color1, toColor1);
                    }
                }
                chart.RefreshChart();
                serie.interactDirty = false;
                m_ForceUpdateSerieContext = true;
            }
            UpdateSerieContextInternal();
        }

        public override void AfterUpdate()
        {
            if (m_LastPointerEnter != serie.context.pointerEnter || m_LastPointerDataIndex != serie.context.pointerItemDataIndex)
            {
                if (chart.onSerieEnter != null || chart.onSerieExit != null || serie.onEnter != null || serie.onExit != null)
                {
                    if (serie.context.pointerEnter)
                    {
                        if ((serie.onExit != null || chart.onSerieExit != null) && m_LastPointerDataIndex >= 0)
                        {
                            var dataValue = serie.GetData(m_LastPointerDataIndex, m_LastPointerDataDimension);
                            var exitEventData = SerieEventDataPool.Get(chart.pointerPos, serie.index, m_LastPointerDataIndex, m_LastPointerDataDimension, dataValue);
                            if (serie.onExit != null) serie.onExit(exitEventData);
                            if (chart.onSerieExit != null) chart.onSerieExit(exitEventData);
                            SerieEventDataPool.Release(exitEventData);
                        }
                        var dataIndex = GetPointerItemDataIndex();
                        var dimension = GetPointerItemDataDimension();
                        var value = serie.GetData(dataIndex, dimension);
                        var enterEventData = SerieEventDataPool.Get(chart.pointerPos, serie.index, dataIndex, dimension, value);
                        if (serie.onEnter != null) serie.onEnter(enterEventData);
                        if (chart.onSerieEnter != null) chart.onSerieEnter(enterEventData);
                        SerieEventDataPool.Release(enterEventData);
                    }
                    else if (m_LastPointerDataIndex >= 0)
                    {
                        var dataValue = serie.GetData(m_LastPointerDataIndex, m_LastPointerDataDimension);
                        var exitEventData = SerieEventDataPool.Get(chart.pointerPos, serie.index, m_LastPointerDataIndex, m_LastPointerDataDimension, dataValue);
                        if (serie.onExit != null) serie.onExit(exitEventData);
                        if (chart.onSerieExit != null) chart.onSerieExit(exitEventData);
                        SerieEventDataPool.Release(exitEventData);
                    }
                }
            }
        }

        public override void ForceUpdateSerieContext()
        {
            m_ForceUpdateSerieContext = true;
        }

        private void CheckConfigurationChanged()
        {
            if (m_LastRadius[0] != serie.radius[0] || m_LastRadius[1] != serie.radius[1])
            {
                m_LastRadius[0] = serie.radius[0];
                m_LastRadius[1] = serie.radius[1];
                serie.SetVerticesDirty();
            }
            if (m_LastCenter[0] != serie.center[0] || m_LastCenter[1] != serie.center[1])
            {
                m_LastCenter[0] = serie.center[0];
                m_LastCenter[1] = serie.center[1];
                serie.SetVerticesDirty();
            }
        }

        private void UpdateSerieContextInternal()
        {
            UpdateSerieContext();
            m_ForceUpdateSerieContext = false;
        }

        public override void RefreshLabelNextFrame()
        {
            m_RefreshLabel = true;
        }

        public override void InitComponent()
        {
            m_InitedLabel = false;
            serie.context.totalDataIndex = serie.dataCount - 1;
            InitRoot();
            InitSerieLabel();
            InitSerieTitle();
            InitSerieEndLabel();
        }

        public override void RemoveComponent()
        {
            ChartHelper.SetActive(m_SerieRoot, false);
        }

        public override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (serie.colorByData && serie.IsSerieDataLegendName(legendName))
            {
                LegendHelper.CheckDataShow(serie, legendName, show);
                chart.UpdateLegendColor(legendName, show);
                chart.RefreshPainter(serie);
            }
            else if (serie.IsLegendName(legendName))
            {
                chart.SetSerieActive(serie, show);
                chart.RefreshPainter(serie);
            }
        }

        public override void OnLegendButtonEnter(int index, string legendName)
        {
            if (serie.colorByData && serie.IsSerieDataLegendName(legendName))
            {
                m_LegendEnterIndex = LegendHelper.CheckDataHighlighted(serie, legendName, true);
                m_LegendEnter = true;
                chart.RefreshPainter(serie);
            }
            else if (serie.IsLegendName(legendName))
            {
                m_LegendEnter = true;
                chart.RefreshPainter(serie);
            }
        }

        public override void OnLegendButtonExit(int index, string legendName)
        {
            if (serie.colorByData && serie.IsSerieDataLegendName(legendName))
            {
                LegendHelper.CheckDataHighlighted(serie, legendName, false);
                m_LegendEnter = false;
                m_LegendExiting = true;
                chart.RefreshPainter(serie);
            }
            else if (serie.IsLegendName(legendName))
            {
                m_LegendEnter = false;
                m_LegendExiting = true;
                chart.RefreshPainter(serie);
            }
        }

        private void InitRoot()
        {
            if (m_SerieRoot != null)
            {
                var rect = ChartHelper.EnsureComponent<RectTransform>(m_SerieRoot);
                rect.localPosition = Vector3.zero;
                rect.sizeDelta = chart.chartSizeDelta;
                rect.anchorMin = chart.chartMinAnchor;
                rect.anchorMax = chart.chartMaxAnchor;
                rect.pivot = chart.chartPivot;
                return;
            }
            var objName = s_SerieRootObjectName + "_" + serie.index;
            m_SerieRoot = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            m_SerieRoot.hideFlags = chart.chartHideFlags;
            ChartHelper.SetActive(m_SerieRoot, true);
            ChartHelper.HideAllObject(m_SerieRoot);
        }

        private void InitSerieLabel()
        {
            InitRoot();
            m_SerieLabelRoot = ChartHelper.AddObject(s_SerieLabelObjectName, m_SerieRoot.transform,
                chart.chartMinAnchor, chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            m_SerieLabelRoot.hideFlags = chart.chartHideFlags;
            SerieLabelPool.ReleaseAll(m_SerieLabelRoot.transform);
            int count = 0;
            SerieHelper.UpdateCenter(serie, chart);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                serieData.index = j;
                serieData.labelObject = null;
                if (AddSerieLabel(m_SerieLabelRoot, serieData, ref count))
                {
                    m_InitedLabel = true;
                    count++;
                }
            }
            RefreshLabelInternal();
        }

        protected bool AddSerieLabel(GameObject serieLabelRoot, SerieData serieData, ref int count)
        {
            if (serieData == null)
                return false;
            if (serieLabelRoot == null)
                return false;
            if (serie.IsPerformanceMode())
                return false;

            if (count == -1) count = serie.dataCount;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (serieLabel == null)
            {
                return false;
            }

            var dataAutoColor = GetSerieDataAutoColor(serieData);
            serieData.context.dataLabels.Clear();
            if (serie.multiDimensionLabel)
            {
                for (int i = 0; i < serieData.data.Count; i++)
                {
                    var textName = string.Format("{0}_{1}_{2}_{3}", s_SerieLabelObjectName, serie.index, serieData.index, i);
                    var label = ChartHelper.AddChartLabel(textName, serieLabelRoot.transform, serieLabel, chart.theme.common,
                        "", dataAutoColor, TextAnchor.MiddleCenter);
                    label.SetActive(false, true);
                    serieData.context.dataLabels.Add(label);
                }
            }
            else
            {
                var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, serie.index, serieData.index);
                var label = ChartHelper.AddChartLabel(textName, serieLabelRoot.transform, serieLabel, chart.theme.common,
                    "", dataAutoColor, TextAnchor.MiddleCenter);
                label.SetActive(false, true);
                serieData.labelObject = label;
            }

            if (serieData.context.children.Count > 0)
            {
                foreach (var childSerieData in serieData.context.children)
                {
                    AddSerieLabel(serieLabelRoot, childSerieData, ref count);
                    count++;
                }
            }
            return true;
        }

        private void InitSerieEndLabel()
        {
            if (serie.endLabel == null)
            {
                if (m_EndLabel != null)
                {
                    m_EndLabel.SetActive(false);
                    m_EndLabel = null;
                }
                return;
            }
            InitRoot();
            var dataAutoColor = (Color)chart.GetLegendRealShowNameColor(serie.legendName);
            m_EndLabel = ChartHelper.AddChartLabel(s_SerieEndLabelObjectName, m_SerieRoot.transform, serie.endLabel,
                chart.theme.common, "", dataAutoColor, TextAnchor.MiddleLeft);
            m_EndLabel.SetActive(serie.endLabel.show);
            RefreshEndLabelInternal();
        }

        private void InitSerieTitle()
        {
            InitRoot();
            var serieTitleRoot = ChartHelper.AddObject(s_SerieTitleObjectName, m_SerieRoot.transform,
                chart.chartMinAnchor, chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            serieTitleRoot.hideFlags = chart.chartHideFlags;
            SerieLabelPool.ReleaseAll(serieTitleRoot.transform);
            ChartHelper.RemoveComponent<Text>(serieTitleRoot);

            SerieHelper.UpdateCenter(serie, chart);

            if (serie.titleJustForSerie)
            {
                var titleStyle = SerieHelper.GetTitleStyle(serie, null);
                if (titleStyle != null)
                {
                    var color = chart.GetItemColor(serie, null);
                    var content = string.Empty;
                    if (string.IsNullOrEmpty(titleStyle.formatter))
                    {
                        content = serie.serieName;
                    }
                    else
                    {
                        content = titleStyle.formatter;
                        FormatterHelper.ReplaceContent(ref content, -1, titleStyle.numericFormatter, serie, chart);
                    }
                    var label = ChartHelper.AddChartLabel("title_" + 0, serieTitleRoot.transform, titleStyle, chart.theme.common,
                        content, color, TextAnchor.MiddleCenter);
                    serie.context.titleObject = label;
                    label.SetActive(titleStyle.show, true);
                    var labelPosition = GetSerieDataTitlePosition(null, titleStyle);
                    var offset = titleStyle.GetOffset(serie.context.insideRadius);
                    label.SetPosition(labelPosition + offset);
                }
            }
            else
            {
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.data[i];
                    var titleStyle = SerieHelper.GetTitleStyle(serie, serieData);
                    if (titleStyle == null) continue;
                    var color = chart.GetItemColor(serie, serieData);
                    var content = string.Empty;
                    if (string.IsNullOrEmpty(titleStyle.formatter))
                    {
                        content = serieData.name;
                    }
                    else
                    {
                        content = titleStyle.formatter;
                        FormatterHelper.ReplaceContent(ref content, i, titleStyle.numericFormatter, serie, chart);
                    }
                    var label = ChartHelper.AddChartLabel("title_" + i, serieTitleRoot.transform, titleStyle, chart.theme.common,
                        content, color, TextAnchor.MiddleCenter);
                    serieData.titleObject = label;
                    label.SetActive(titleStyle.show, true);
                    var labelPosition = GetSerieDataTitlePosition(serieData, titleStyle);
                    var offset = titleStyle.GetOffset(serie.context.insideRadius);
                    label.SetPosition(labelPosition + offset);
                }
            }
        }

        public override void RefreshLabelInternal()
        {
            if (!m_InitedLabel)
                return;

            var dataChangeDuration = serie.animation.GetChangeDuration();
            var dataAddDuration = serie.animation.GetAdditionDuration();
            var unscaledTime = serie.animation.unscaledTime;
            var needCheck = serie.context.dataIndexs.Count > 0;
            var allLabelZeroPosition = true;
            var anyLabelActive = false;
            foreach (var serieData in serie.data)
            {
                if (serieData.labelObject == null && serieData.context.dataLabels.Count <= 0)
                {
                    continue;
                }
                if (needCheck && !serie.context.dataIndexs.Contains(serieData.index))
                {
                    serieData.SetLabelActive(false);
                    continue;
                };
                var currLabel = SerieHelper.GetSerieLabel(serie, serieData);
                var isIgnore = serie.IsIgnoreIndex(serieData.index, defaultDimension);
                if (serie.show &&
                    currLabel != null &&
                    currLabel.show &&
                    serieData.context.canShowLabel &&
                    !serieData.context.isClip &&
                    !isIgnore)
                {
                    if (serie.multiDimensionLabel)
                    {
                        var total = serieData.GetTotalData();
                        var color = chart.GetItemColor(serie, serieData);
                        for (int i = 0; i < serieData.context.dataLabels.Count; i++)
                        {
                            if (i >= serieData.context.dataPoints.Count) continue;
                            var labelObject = serieData.context.dataLabels[i];
                            var value = serieData.GetCurrData(i, dataAddDuration, dataChangeDuration, unscaledTime);
                            var content = string.IsNullOrEmpty(currLabel.formatter) ?
                                ChartCached.NumberToStr(value, currLabel.numericFormatter) :
                                SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                                    currLabel, color, chart);
                            var offset = GetSerieDataLabelOffset(serieData, currLabel);
                            var active = currLabel.show && !isIgnore && !serie.IsMinShowLabelValue(value);
                            if (active)
                            {
                                anyLabelActive = true;
                                if (!ChartHelper.IsZeroVector(serieData.context.dataPoints[i]))
                                {
                                    allLabelZeroPosition = false;
                                }
                            }
                            labelObject.SetActive(active);
                            labelObject.SetText(content);
                            labelObject.SetPosition(serieData.context.dataPoints[i] + offset);
                            labelObject.UpdateIcon(currLabel.icon);
                            if (currLabel.textStyle.autoColor)
                            {
                                var dataAutoColor = GetSerieDataAutoColor(serieData);
                                if (!ChartHelper.IsClearColor(dataAutoColor))
                                    labelObject.SetTextColor(dataAutoColor);
                            }
                        }
                    }
                    else
                    {
                        var value = serieData.GetCurrData(defaultDimension, dataAddDuration, dataChangeDuration, unscaledTime);
                        var total = serie.GetDataTotal(defaultDimension, serieData);
                        var color = chart.GetItemColor(serie, serieData);
                        var content = string.IsNullOrEmpty(currLabel.formatter) ?
                            ChartCached.NumberToStr(value, currLabel.numericFormatter) :
                            SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                                currLabel, color, chart);
                        var labelPos = UpdateLabelPosition(serieData, currLabel);
                        var active = currLabel.show && !isIgnore && !serie.IsMinShowLabelValue(value);
                        if (active)
                        {
                            anyLabelActive = true;
                            if (!ChartHelper.IsZeroVector(labelPos))
                            {
                                allLabelZeroPosition = false;
                            }
                        }
                        serieData.SetLabelActive(active);
                        serieData.labelObject.UpdateIcon(currLabel.icon);
                        serieData.labelObject.SetText(content);
                        if (currLabel.textStyle.autoColor)
                        {
                            var dataAutoColor = GetSerieDataAutoColor(serieData);
                            if (!ChartHelper.IsClearColor(dataAutoColor))
                                serieData.labelObject.SetTextColor(dataAutoColor);
                        }
                    }
                }
                else
                {
                    serieData.SetLabelActive(false);
                }
            }
            if (anyLabelActive && allLabelZeroPosition)
            {
                foreach (var serieData in serie.data)
                {
                    serieData.SetLabelActive(false);
                }
            }
        }

        public virtual void RefreshEndLabelInternal()
        {
            if (m_EndLabel == null)
                return;
            var endLabelStyle = serie.endLabel;
            if (endLabelStyle == null)
                return;
            var dataCount = serie.context.dataPoints.Count;
            var active = endLabelStyle.show && dataCount > 0;
            m_EndLabel.SetActive(active);
            if (active)
            {
                var value = serie.context.lineEndValueY;
                var content = SerieLabelHelper.GetFormatterContent(serie, null, value, 0,
                    endLabelStyle, Color.clear);
                m_EndLabel.SetText(content);
                m_EndLabel.SetPosition(serie.context.lineEndPostion + endLabelStyle.offset);
            }
            m_EndLabel.isAnimationEnd = serie.animation.IsFinish();
        }

        protected Vector3 UpdateLabelPosition(SerieData serieData, LabelStyle currLabel)
        {
            var labelPosition = GetSerieDataLabelPosition(serieData, currLabel);
            var offset = GetSerieDataLabelOffset(serieData, currLabel);
            serieData.labelObject.SetPosition(labelPosition + offset);
            if (currLabel.autoRotate && serieData.context.angle != 0)
            {
                if (serieData.context.angle > 90 && serieData.context.angle < 270)
                    serieData.labelObject.SetRotate(180 - serieData.context.angle + currLabel.rotate);
                else
                    serieData.labelObject.SetRotate(-serieData.context.angle + currLabel.rotate);
            }
            return labelPosition;
        }

        public virtual Vector3 GetSerieDataLabelPosition(SerieData serieData, LabelStyle label)
        {
            return ChartHelper.IsZeroVector(serieData.context.labelPosition) ?
                serieData.context.position :
                serieData.context.labelPosition;
        }

        public virtual Vector3 GetSerieDataLabelOffset(SerieData serieData, LabelStyle label)
        {
            return label.GetOffset(serie.context.insideRadius);
        }

        public virtual Vector3 GetSerieDataTitlePosition(SerieData serieData, TitleStyle titleStyle)
        {
            return serieData.context.position;
        }

        public virtual Color GetSerieDataAutoColor(SerieData serieData)
        {
            var colorIndex = serie.colorByData ? serieData.index : serie.index;
            Color32 color, toColor;
            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Normal, false);
            return (Color)color;
        }

        protected void UpdateCoordSerieParams(ref List<SerieParams> paramList, ref string title,
            int dataIndex, bool showCategory, string category, string marker,
            string itemFormatter, string numericFormatter, string ignoreDataDefaultContent)
        {
            var dimension = 1;
            if (dataIndex < 0)
                dataIndex = serie.context.pointerItemDataIndex;

            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            var ignore = serie.IsIgnoreValue(serieData, dimension);
            if (ignore && string.IsNullOrEmpty(ignoreDataDefaultContent))
                return;

            itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            if (serie.placeHolder || TooltipHelper.IsIgnoreFormatter(itemFormatter))
                return;

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = dimension;
            param.serieData = serieData;
            param.dataCount = serie.dataCount;
            param.value = serieData.GetData(dimension);
            param.ignore = ignore;
            param.total = serie.yTotal;
            param.color = chart.GetMarkColor(serie, serieData);
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = itemFormatter;
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(showCategory ? category : serie.serieName);
            param.columns.Add(ignore ? ignoreDataDefaultContent : ChartCached.NumberToStr(param.value, param.numericFormatter));

            paramList.Add(param);
        }

        protected void UpdateItemSerieParams(ref List<SerieParams> paramList, ref string title,
            int dataIndex, string category, string marker,
            string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            int dimension = 1, int colorIndex = -1)
        {
            if (dataIndex < 0)
                dataIndex = serie.context.pointerItemDataIndex;

            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            var ignore = serie.IsIgnoreValue(serieData, dimension);
            if (ignore && string.IsNullOrEmpty(ignoreDataDefaultContent))
                return;

            itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            if (serie.placeHolder || TooltipHelper.IsIgnoreFormatter(itemFormatter))
                return;

            if (colorIndex < 0)
                colorIndex = serie.colorByData ? dataIndex : chart.GetLegendRealShowNameIndex(serieData.name);

            Color32 color, toColor;
            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Normal);
            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;

            param.category = category;
            param.dimension = dimension;
            param.serieData = serieData;
            param.dataCount = serie.dataCount;
            param.value = serieData.GetData(param.dimension);
            param.ignore = ignore;
            param.total = serie.multiDimensionLabel ? serieData.GetTotalData() : serie.GetDataTotal(defaultDimension);
            param.color = color;
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = itemFormatter;
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(serieData.name);

            param.columns.Add(ignore ? ignoreDataDefaultContent : ChartCached.NumberToStr(param.value, param.numericFormatter));

            paramList.Add(param);
        }

        public void DrawLabelLineSymbol(VertexHelper vh, LabelLine labelLine, Vector3 startPos, Vector3 endPos, Color32 defaultColor)
        {
            if (labelLine.startSymbol != null && labelLine.startSymbol.show)
            {
                DrawSymbol(vh, labelLine.startSymbol, startPos, defaultColor);
            }
            if (labelLine.endSymbol != null && labelLine.endSymbol.show)
            {
                DrawSymbol(vh, labelLine.endSymbol, endPos, defaultColor);
            }
        }

        private void DrawSymbol(VertexHelper vh, SymbolStyle symbol, Vector3 pos, Color32 defaultColor)
        {
            var color = symbol.GetColor(defaultColor);
            chart.DrawSymbol(vh, symbol.type, symbol.size, 1, pos,
                color, color, ColorUtil.clearColor32, color, symbol.gap, null);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (serie.onDown == null && chart.onSerieDown == null) return;
            if (!serie.context.pointerEnter) return;
            var dataIndex = GetPointerItemDataIndex();
            if (dataIndex < 0) return;
            var dimension = GetPointerItemDataDimension();
            var value = serie.GetData(dataIndex, dimension);
            var data = SerieEventDataPool.Get(chart.pointerPos, serie.index, dataIndex, dimension, value);
            if (chart.onSerieDown != null)
                chart.onSerieDown(data);
            if (serie.onDown != null)
                serie.onDown(data);
            SerieEventDataPool.Release(data);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            serie.context.clickTotalDataIndex = serie.context.totalDataIndex;
            if (serie.onClick == null && chart.onSerieClick == null) return;
            if (!serie.context.pointerEnter) return;
            var dataIndex = GetPointerItemDataIndex();
            if (dataIndex < 0) return;
            var dimension = GetPointerItemDataDimension();
            var value = serie.GetData(dataIndex, dimension);
            var data = SerieEventDataPool.Get(chart.pointerPos, serie.index, dataIndex, dimension, value);
            if (chart.onSerieClick != null)
                chart.onSerieClick(data);
            if (serie.onClick != null)
                serie.onClick(data);
            SerieEventDataPool.Release(data);
        }

        public override int GetPointerItemDataIndex()
        {
            return serie.context.pointerItemDataIndex;
        }

        public override int GetPointerItemDataDimension()
        {
            return serie.context.pointerItemDataDimension;
        }
    }
}