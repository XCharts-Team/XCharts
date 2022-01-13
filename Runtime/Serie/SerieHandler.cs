
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public abstract class SerieHandler
    {
        public BaseChart chart { get; internal set; }
        public SerieHandlerAttribute attribute { get; internal set; }

        public virtual void InitComponent() { }
        public virtual void RemoveComponent() { }
        public virtual void CheckComponent(StringBuilder sb) { }
        public virtual void Update() { }
        public virtual void DrawBase(VertexHelper vh) { }
        public virtual void DrawSerie(VertexHelper vh) { }
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
        public virtual void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category, string marker,
            string itemFormatter, string numericFormatter, ref List<SerieParams> paramList, ref string title)
        { }
        public virtual void OnLegendButtonClick(int index, string legendName, bool show) { }
        public virtual void OnLegendButtonEnter(int index, string legendName) { }
        public virtual void OnLegendButtonExit(int index, string legendName) { }
        internal abstract void SetSerie(Serie serie);
    }

    public abstract class SerieHandler<T> : SerieHandler where T : Serie
    {
        private static readonly string s_SerieLabelObjectName = "label";
        private static readonly string s_SerieTitleObjectName = "serie";
        protected GameObject m_SerieRoot;
        protected bool m_InitedLabel;
        protected bool m_NeedInitComponent;
        protected bool m_RefreshLabel;
        protected bool m_LastCheckContextFlag = false;
        protected bool m_LegendEnter = false;
        protected int m_LegendEnterIndex;

        public T serie { get; internal set; }

        internal override void SetSerie(Serie serie)
        {
            this.serie = (T)serie;
            this.serie.context.param.serieType = typeof(T);
            m_NeedInitComponent = true;
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
                if (m_InitedLabel)
                    RefreshLabelInternal();
            }
            if (serie.label != null && (serie.labelDirty || serie.label.componentDirty))
            {
                serie.labelDirty = false;
                serie.label.ClearComponentDirty();
                InitSerieLabel();
            }
            if (serie.titleDirty || serie.titleStyle.componentDirty)
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
        }

        public override void RemoveComponent()
        {
            ChartHelper.SetActive(m_SerieRoot, false);
        }

        public override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (serie.IsLegendName(legendName))
            {
                chart.SetSerieActive(serie, show);
                chart.RefreshPainter(serie);
            }
        }

        public override void OnLegendButtonEnter(int index, string legendName)
        {
            if (serie.IsLegendName(legendName))
            {
                m_LegendEnter = true;
                chart.RefreshPainter(serie);
            }
        }

        public override void OnLegendButtonExit(int index, string legendName)
        {
            if (serie.IsLegendName(legendName))
            {
                m_LegendEnter = false;
                chart.RefreshPainter(serie);
            }
        }

        private void InitRoot()
        {
            if (m_SerieRoot != null) return;
            var objName = s_SerieTitleObjectName + "_" + serie.index;
            m_SerieRoot = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            m_SerieRoot.hideFlags = chart.chartHideFlags;
            ChartHelper.SetActive(m_SerieRoot, true);
            ChartHelper.HideAllObject(m_SerieRoot);
        }

        private void InitSerieLabel()
        {
            if (m_SerieRoot == null)
                InitRoot();
            var serieLabelRoot = ChartHelper.AddObject(s_SerieLabelObjectName, m_SerieRoot.transform,
                chart.chartMinAnchor, chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            serieLabelRoot.hideFlags = chart.chartHideFlags;
            SerieLabelPool.ReleaseAll(serieLabelRoot.transform);
            int count = 0;
            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                serieData.index = count;
                serieData.labelObject = null;
                if (AddSerieLabel(serieLabelRoot, serie, serieData, ref count))
                {
                    m_InitedLabel = true;
                    count++;
                }
            }
        }

        protected bool AddSerieLabel(GameObject serieLabelRoot, Serie serie, SerieData serieData, ref int count)
        {
            if (serieLabelRoot == null)
                return false;
            if (serie.IsPerformanceMode())
                return false;

            if (count == -1) count = serie.dataCount;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (serieLabel == null)
                return false;

            var serieEmphasisLabel = SerieHelper.GetSerieEmphasisLabel(serie, serieData);
            var iconStyle = SerieHelper.GetIconStyle(serie, serieData);

            if (!serieLabel.show && (serieEmphasisLabel == null || !serieEmphasisLabel.show)
                && (iconStyle != null && !iconStyle.show))
                return false;

            var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, serie.index, serieData.index);
            var color = chart.theme.common.textColor;
            var iconWidth = iconStyle != null ? iconStyle.width : 20;
            var iconHeight = iconStyle != null ? iconStyle.height : 20;
            var labelObj = SerieLabelPool.Get(textName, serieLabelRoot.transform, serieLabel, color,
                       iconWidth, iconHeight, chart.theme);
            var iconImage = labelObj.transform.Find("Icon").GetComponent<Image>();
            var isAutoSize = serieLabel.backgroundWidth == 0 || serieLabel.backgroundHeight == 0;
            var item = ChartHelper.GetOrAddComponent<ChartLabel>(labelObj);
            item.SetLabel(labelObj, isAutoSize, serieLabel.paddingLeftRight, serieLabel.paddingTopBottom);
            item.SetIcon(iconImage);
            item.SetIconActive(iconStyle != null && iconStyle.show);
            item.color = serieLabel.textStyle.backgroundColor;
            serieData.labelObject = item;

            foreach (var data in serieData.children)
            {
                AddSerieLabel(serieLabelRoot, serie, serie.GetSerieData(data), ref count);
                count++;
            }

            return true;
        }

        private void InitSerieTitle()
        {
            if (m_SerieRoot == null)
                InitRoot();
            var textStyle = serie.titleStyle.textStyle;
            var titleColor = ChartHelper.IsClearColor(textStyle.color)
                ? chart.theme.GetColor(serie.index)
                : (Color32)textStyle.color;
            var anchorMin = new Vector2(0.5f, 0.5f);
            var anchorMax = new Vector2(0.5f, 0.5f);
            var pivot = new Vector2(0.5f, 0.5f);
            var fontSize = 10;
            var sizeDelta = new Vector2(50, fontSize + 2);
            var txt = ChartHelper.AddTextObject("title", m_SerieRoot.transform, anchorMin, anchorMax,
                pivot, sizeDelta, textStyle, chart.theme.common);
            txt.SetText("");
            txt.SetColor(titleColor);
            txt.SetLocalPosition(Vector2.zero);
            txt.SetLocalEulerAngles(Vector2.zero);
            txt.SetActive(serie.titleStyle.show);
            serie.titleStyle.runtimeText = txt;
            serie.titleStyle.UpdatePosition(serie.context.center);
            var serieData = serie.GetSerieData(0);
            if (serieData != null)
            {
                txt.SetText(serieData.name);
            }
        }

        public override void RefreshLabelInternal()
        {
            if (!m_InitedLabel)
                return;
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
            var total = serie.yTotal;
            foreach (var serieData in serie.data)
            {
                if (serieData.labelObject == null)
                    continue;
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
                var isIgnore = serie.IsIgnoreIndex(serieData.index);
                serieData.labelObject.SetPosition(serieData.context.position);
                serieData.labelObject.UpdateIcon(iconStyle);
                if (serie.show && serieLabel != null
                    && serieLabel.show && serieData.context.canShowLabel && !isIgnore)
                {
                    var value = serieData.GetData(1);
                    var content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                        serieLabel, chart.theme.GetColor(colorIndex));
                    var invert = serieLabel.autoOffset
                        && serie is Line
                        && SerieHelper.IsDownPoint(serie, serieData.index)
                        && (serie.areaStyle == null || !serie.areaStyle.show);
                    var labelPosition = GetSerieDataLabelPosition(serieData, serieLabel);
                    SerieLabelHelper.ResetLabel(serieData.labelObject.label, serieLabel, chart.theme);
                    serieData.SetLabelActive(!isIgnore);
                    serieData.labelObject.SetPosition(labelPosition
                        + (invert ? -serieLabel.offset : serieLabel.offset));
                    serieData.labelObject.SetText(content);
                }
                else
                {
                    serieData.SetLabelActive(false);
                }
            }
        }

        public virtual Vector3 GetSerieDataLabelPosition(SerieData serieData, LabelStyle label)
        {
            return serieData.context.position;
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
            if (TooltipHelper.IsIgnoreItemFormatter(itemFormatter))
                return;

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = 1;
            param.serieData = serieData;
            param.value = serieData.GetData(1);
            param.total = serie.yTotal;
            param.color = chart.GetLegendRealShowNameColor(serie.serieName);
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
            if (TooltipHelper.IsIgnoreItemFormatter(itemFormatter))
                return;

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = dimension;
            param.serieData = serieData;
            param.value = serieData.GetData(param.dimension);
            param.total = SerieHelper.GetMaxData(serie, dimension);
            param.color = chart.theme.GetColor(dataIndex);
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = itemFormatter;
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter); ;
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(serieData.name);
            param.columns.Add(ChartCached.NumberToStr(param.value, param.numericFormatter));

            paramList.Add(param);
        }
    }
}