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
        public virtual int defaultDimension { get; internal set; }

        public virtual void InitComponent() { }
        public virtual void RemoveComponent() { }
        public virtual void CheckComponent(StringBuilder sb) { }
        public virtual void Update() { }
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
        public virtual void RefreshLabelNextFrame() { }
        public virtual void RefreshLabelInternal() { }
        public virtual void UpdateTooltipSerieParams(int dataIndex, bool showCategory,
            string category, string marker,
            string itemFormatter, string numericFormatter,
            ref List<SerieParams> paramList, ref string title) { }
        public virtual void OnLegendButtonClick(int index, string legendName, bool show) { }
        public virtual void OnLegendButtonEnter(int index, string legendName) { }
        public virtual void OnLegendButtonExit(int index, string legendName) { }
        internal abstract void SetSerie(Serie serie);
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
        protected int m_LegendEnterIndex;
        protected ChartLabel m_EndLabel;

        public T serie { get; internal set; }
        public GameObject labelObject { get { return m_SerieLabelRoot; } }

        internal override void SetSerie(Serie serie)
        {
            this.serie = (T) serie;
            this.serie.context.param.serieType = typeof(T);
            m_NeedInitComponent = true;
            AnimationStyleHelper.UpdateSerieAnimation(serie);
        }

        public override void Update()
        {
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
                SeriesHelper.UpdateSerieNameList(chart, ref chart.m_LegendRealShowName);
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
                serie.ResetInteract();
                serie.ClearVerticesDirty();
            }
        }

        public override void RefreshLabelNextFrame()
        {
            m_RefreshLabel = true;
        }

        public override void InitComponent()
        {
            m_InitedLabel = false;
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
            if (serie.useDataNameForColor && serie.IsSerieDataLegendName(legendName))
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
            if (serie.useDataNameForColor && serie.IsSerieDataLegendName(legendName))
            {
                LegendHelper.CheckDataHighlighted(serie, legendName, true);
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
            if (serie.useDataNameForColor && serie.IsSerieDataLegendName(legendName))
            {
                LegendHelper.CheckDataHighlighted(serie, legendName, false);
                chart.RefreshPainter(serie);
            }
            else if (serie.IsLegendName(legendName))
            {
                m_LegendEnter = false;
                chart.RefreshPainter(serie);
            }
        }

        private void InitRoot()
        {
            if (m_SerieRoot != null)
            {
                var rect = ChartHelper.GetOrAddComponent<RectTransform>(m_SerieRoot);
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
            //ChartHelper.DestroyAllChildren(m_SerieLabelRoot.transform);
            int count = 0;
            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                serieData.index = count;
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
                serieLabel = SerieHelper.GetSerieEmphasisLabel(serie, serieData);
                if (serieLabel == null || !serieLabel.show)
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
                    label.SetActive(serieLabel.show);
                    serieData.context.dataLabels.Add(label);
                }
            }
            else
            {
                var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, serie.index, serieData.index);
                var label = ChartHelper.AddChartLabel(textName, serieLabelRoot.transform, serieLabel, chart.theme.common,
                    "", dataAutoColor, TextAnchor.MiddleCenter);
                label.SetActive(serieLabel.show);
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
            var dataAutoColor = (Color) chart.GetLegendRealShowNameColor(serie.legendName);
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

            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);

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
                        FormatterHelper.ReplaceContent(ref content, 0, titleStyle.numericFormatter, serie, chart);
                    }
                    var label = ChartHelper.AddChartLabel("title_" + 0, serieTitleRoot.transform, titleStyle, chart.theme.common,
                        content, color, TextAnchor.MiddleCenter);
                    serie.context.titleObject = label;
                    label.SetActive(titleStyle.show);
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
                        FormatterHelper.ReplaceContent(ref content, 0, titleStyle.numericFormatter, serie, chart);
                    }
                    FormatterHelper.ReplaceContent(ref content, i, titleStyle.numericFormatter, serie, chart);
                    var label = ChartHelper.AddChartLabel("title_" + i, serieTitleRoot.transform, titleStyle, chart.theme.common,
                        content, color, TextAnchor.MiddleCenter);
                    serieData.titleObject = label;
                    label.SetActive(titleStyle.show);
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

            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            foreach (var serieData in serie.data)
            {
                if (serieData.labelObject == null && serieData.context.dataLabels.Count <= 0)
                    continue;
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                var emphasisLabel = SerieHelper.GetSerieEmphasisLabel(serie, serieData);
                var isHighlight = (serieData.context.highlight && emphasisLabel != null && emphasisLabel.show);
                var isIgnore = serie.IsIgnoreIndex(serieData.index, defaultDimension);
                var currLabel = isHighlight && emphasisLabel != null ? emphasisLabel : serieLabel;
                if (serie.show &&
                    currLabel != null &&
                    (currLabel.show || isHighlight) &&
                    serieData.context.canShowLabel &&
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
                            var value = serieData.GetCurrData(i, dataChangeDuration);
                            var content = string.IsNullOrEmpty(currLabel.formatter) ?
                                ChartCached.NumberToStr(value, serieLabel.numericFormatter) :
                                SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                                    currLabel, color);
                            var offset = GetSerieDataLabelOffset(serieData, currLabel);
                            labelObject.SetActive(!isIgnore);
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
                        var value = serieData.GetCurrData(defaultDimension, dataChangeDuration);
                        var total = serie.GetDataTotal(defaultDimension, serieData);
                        var color = chart.GetItemColor(serie, serieData);
                        var content = string.IsNullOrEmpty(currLabel.formatter) ?
                            ChartCached.NumberToStr(value, serieLabel.numericFormatter) :
                            SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                                currLabel, color);
                        serieData.SetLabelActive(!isIgnore);
                        serieData.labelObject.UpdateIcon(currLabel.icon);
                        serieData.labelObject.SetText(content);
                        UpdateLabelPosition(serieData, currLabel);
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
                    foreach (var labelObject in serieData.context.dataLabels)
                    {
                        labelObject.SetActive(false);
                    }
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
            var dataCount = serie.context.drawPoints.Count;
            var active = endLabelStyle.show && dataCount > 0;
            m_EndLabel.SetActive(active);
            if (active)
            {
                var value = serie.context.lineEndValue;
                var content = SerieLabelHelper.GetFormatterContent(serie, null, value, 0,
                    endLabelStyle, Color.clear);
                m_EndLabel.SetText(content);
                m_EndLabel.SetPosition(serie.context.lineEndPostion + endLabelStyle.offset);
            }
            m_EndLabel.isAnimationEnd = serie.animation.IsFinish();
        }

        private void UpdateLabelPosition(SerieData serieData, LabelStyle currLabel)
        {
            var labelPosition = GetSerieDataLabelPosition(serieData, currLabel);
            var offset = GetSerieDataLabelOffset(serieData, currLabel);
            serieData.labelObject.SetPosition(labelPosition + offset);
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
            var colorIndex = serie.useDataNameForColor ? serieData.index : serie.index;
            return (Color) SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, false, false);
        }

        protected void UpdateCoordSerieParams(ref List<SerieParams> paramList, ref string title,
            int dataIndex, bool showCategory, string category, string marker,
            string itemFormatter, string numericFormatter)
        {
            if (dataIndex < 0)
                dataIndex = serie.context.pointerItemDataIndex;

            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            if (serie.placeHolder || TooltipHelper.IsIgnoreFormatter(itemFormatter))
                return;

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = 1;
            param.serieData = serieData;
            param.dataCount = serie.dataCount;
            param.value = serieData.GetData(1);
            param.total = serie.yTotal;
            param.color = SerieHelper.GetItemColor(serie, serieData, chart.theme, serie.context.colorIndex, false);
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = itemFormatter;
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(showCategory ? category : serie.serieName);
            param.columns.Add(ChartCached.NumberToStr(param.value, param.numericFormatter));

            paramList.Add(param);
        }

        protected void UpdateItemSerieParams(ref List<SerieParams> paramList, ref string title,
            int dataIndex, string category, string marker,
            string itemFormatter, string numericFormatter, int dimension = 1)
        {
            if (dataIndex < 0)
                dataIndex = serie.context.pointerItemDataIndex;

            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            if (serie.placeHolder || TooltipHelper.IsIgnoreFormatter(itemFormatter))
                return;

            var colorIndex = chart.GetLegendRealShowNameIndex(serieData.name);

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = dimension;
            param.serieData = serieData;
            param.dataCount = serie.dataCount;
            param.value = serieData.GetData(param.dimension);
            param.total = SerieHelper.GetMaxData(serie, dimension);
            param.color = SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, false);
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = itemFormatter;
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(serieData.name);
            param.columns.Add(ChartCached.NumberToStr(param.value, param.numericFormatter));

            paramList.Add(param);
        }
    }
}