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

    public partial class BaseChart : BaseGraph
    {
        protected static readonly string s_BackgroundObjectName = "background";
        protected static readonly string s_TitleObjectName = "title";
        protected static readonly string s_SubTitleObjectName = "title_sub";
        protected static readonly string s_LegendObjectName = "legend";
        protected static readonly string s_SerieLabelObjectName = "label";
        protected static readonly string s_SerieTitleObjectName = "serie";

        [SerializeField] protected string m_ChartName;

        [SerializeField] protected ThemeInfo m_ThemeInfo;
        [SerializeField] protected Title m_Title = Title.defaultTitle;
        [SerializeField] protected Background m_Background = Background.defaultBackground;
        [SerializeField] protected Legend m_Legend = Legend.defaultLegend;
        [SerializeField] protected Tooltip m_Tooltip = Tooltip.defaultTooltip;
        [SerializeField] protected Series m_Series = Series.defaultSeries;
        [SerializeField] protected Settings m_Settings = new Settings();

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
        protected Action<VertexHelper> m_OnCustomDrawCallback;

        protected bool m_RefreshLabel = false;
        protected bool m_ReinitLabel = false;
        protected bool m_ReinitTitle = false;
        protected bool m_CheckAnimation = false;
        protected bool m_IsPlayingAnimation = false;
        protected List<string> m_LegendRealShowName = new List<string>();
        protected GameObject m_SerieLabelRoot;
        protected GameObject m_BackgroundRoot;

        private Theme m_CheckTheme = 0;

        protected override void InitComponent()
        {
            base.InitComponent();
            InitBackground();
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
            if (transform.parent != null)
            {
                m_IsControlledByLayout = transform.parent.GetComponent<LayoutGroup>() != null;
            }
            raycastTarget = false;
            m_CheckTheme = m_ThemeInfo.theme;
            m_LastLocalPosition = transform.localPosition;
            UpdateSize();
            InitComponent();
            m_Series.AnimationReset();
            m_Series.AnimationFadeIn();
            XChartsMgr.Instance.AddChart(this);
        }

        protected override void Start()
        {
            RefreshChart();
        }

        protected override void Update()
        {
            base.Update();
            CheckTooltip();
            CheckRefreshChart();
            CheckRefreshLabel();
            CheckAnimation();
        }

        protected override void CheckComponent()
        {
            if (m_Series.anyDirty)
            {
                if (m_Series.vertsDirty) RefreshChart();
                if (SeriesHelper.IsLabelDirty(m_Series)) m_ReinitLabel = true;
                if (SeriesHelper.IsNeedLabelUpdate(m_Series) && !m_RefreshChart) m_RefreshLabel = true;
                if (SeriesHelper.IsLabelDirty(m_Series)) m_ReinitLabel = true;
                foreach (var serie in m_Series.list)
                {
                    if (serie.titleStyle.componentDirty) m_ReinitTitle = true;
                    if (serie.nameDirty)
                    {
                        m_Legend.SetAllDirty();
                        RefreshChart();
                        serie.ClearNameDirty();
                    }
                }
                m_Series.ClearDirty();
            }
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
            if (m_Background.anyDirty)
            {
                if (m_Background.componentDirty) InitBackground();
                if (m_Background.vertsDirty) RefreshChart();
                m_Background.ClearDirty();
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
            m_Background.SetAllDirty();
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

        private void InitBackground()
        {
            if (!transform.parent) return;
            int childCount = transform.parent.childCount;
            if (childCount > 2) m_Background.runtimeActive = false;
            else if (childCount == 1) m_Background.runtimeActive = true;
            else
            {
                m_Background.runtimeActive = false;
                for (int i = 0; i < childCount; i++)
                {
                    if (transform.parent.GetChild(i).name.StartsWith(s_BackgroundObjectName))
                    {
                        m_Background.runtimeActive = true;
                        break;
                    }
                }
            }
            if (!m_Background.runtimeActive || m_IsControlledByLayout)
            {
                //find old gameobject and delete
                var objName = s_BackgroundObjectName + GetInstanceID();
                ChartHelper.DestoryGameObject(transform.parent, objName);
                ChartHelper.DestoryGameObject(m_BackgroundRoot);
                return;
            }
            if (!m_Background.show)
            {
                ChartHelper.DestoryGameObject(m_BackgroundRoot);
                return;
            }
            var backgroundName = s_BackgroundObjectName;
            m_BackgroundRoot = ChartHelper.AddObject(backgroundName, transform.parent, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
            m_BackgroundRoot.hideFlags = chartHideFlags;
            var backgroundImage = ChartHelper.GetOrAddComponent<Image>(m_BackgroundRoot);
            var backgroundRect = m_BackgroundRoot.GetComponent<RectTransform>();
            backgroundRect.position = rectTransform.position;
            backgroundImage.sprite = m_Background.image;
            backgroundImage.type = m_Background.imageType;
            backgroundImage.color = m_Background.imageColor;
            m_BackgroundRoot.SetActive(m_Background.show);
            var siblindIndex = rectTransform.GetSiblingIndex();
            if (siblindIndex == 0)
            {
                backgroundRect.SetSiblingIndex(0);
            }
            else
            {
                backgroundRect.SetSiblingIndex(rectTransform.GetSiblingIndex() - 1);
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
            titleObject.hideFlags = chartHideFlags;
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
            Text subText = ChartHelper.AddTextObject(s_SubTitleObjectName, titleObject.transform,
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
            var legendObject = ChartHelper.AddObject(s_LegendObjectName, transform, m_ChartMinAnchor,
                 m_ChartMaxAnchor, m_ChartPivot, new Vector2(chartWidth, chartHeight));
            legendObject.hideFlags = chartHideFlags;
            SeriesHelper.UpdateSerieNameList(m_Series, ref m_LegendRealShowName);
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
                if (!SeriesHelper.IsLegalLegendName(datas[i])) continue;
                totalLegend++;
            }
            m_Legend.RemoveButton();
            ChartHelper.HideAllObject(legendObject);
            if (!m_Legend.show) return;
            for (int i = 0; i < datas.Count; i++)
            {
                if (!SeriesHelper.IsLegalLegendName(datas[i])) continue;
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
            LegendHelper.ResetItemPosition(m_Legend, m_ChartPosition, m_ChartWidth, m_ChartHeight);
        }

        private void InitSerieLabel()
        {
            m_SerieLabelRoot = ChartHelper.AddObject(s_SerieLabelObjectName, transform, m_ChartMinAnchor,
                m_ChartMaxAnchor, m_ChartPivot, m_ChartSizeDelta);
            m_SerieLabelRoot.hideFlags = chartHideFlags;
            SerieLabelPool.ReleaseAll(m_SerieLabelRoot.transform);
            int count = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                serie.index = i;
                SerieHelper.UpdateCenter(serie, chartPosition, chartWidth, chartHeight);
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    serieData.index = j;
                    serieData.labelObject = null;
                    AddSerieLabel(serie, serieData, count);
                    count++;
                }
            }
            SerieLabelHelper.UpdateLabelText(m_Series, m_ThemeInfo);
        }

        protected void AddSerieLabel(Serie serie, SerieData serieData, int count = -1)
        {
            if (m_SerieLabelRoot == null) return;
            if (count == -1) count = serie.dataCount;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (serie.IsPerformanceMode()) return;
            if (!serieLabel.show) return;
            var textName = ChartCached.GetSerieLabelName(s_SerieLabelObjectName, serie.index, serieData.index);
            var color = Color.grey;
            if (serie.type == SerieType.Pie)
            {
                color = (serieLabel.position == SerieLabel.Position.Inside) ? Color.white :
                    (Color)m_ThemeInfo.GetColor(count);
            }
            else
            {
                color = !ChartHelper.IsClearColor(serieLabel.color) ? serieLabel.color :
                    (Color)m_ThemeInfo.GetColor(serie.index);
            }
            var labelObj = SerieLabelPool.Get(textName, m_SerieLabelRoot.transform, serieLabel, m_ThemeInfo.font, color,
                       serieData.iconStyle.width, serieData.iconStyle.height);
            var iconImage = labelObj.transform.Find("Icon").GetComponent<Image>();
            var isAutoSize = serieLabel.backgroundWidth == 0 || serieLabel.backgroundHeight == 0;
            var item = new LabelObject();
            item.SetLabel(labelObj, isAutoSize, serieLabel.paddingLeftRight, serieLabel.paddingTopBottom);
            item.SetIcon(iconImage);
            item.SetIconActive(false);
            serieData.labelObject = item;
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
                var color = ChartHelper.IsClearColor(textStyle.color) ? m_ThemeInfo.GetColor(i) : (Color32)textStyle.color;
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
            tooltipObject.hideFlags = chartHideFlags;
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

        protected override bool IsNeedCheckPointerPos()
        {
            return (m_Tooltip.show && m_Tooltip.runtimeInited)
                || raycastTarget;
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

        protected override void CheckRefreshChart()
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
                SeriesHelper.UpdateSerieNameList(m_Series, ref m_LegendRealShowName);
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

            m_Background.SetAllDirty();
            m_Title.SetAllDirty();
            m_Legend.SetAllDirty();
            m_Tooltip.SetAllDirty();
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

        protected virtual void UpdateTooltip()
        {
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            DrawBackground(vh);
            DrawChart(vh);
            if (m_OnCustomDrawCallback != null)
            {
                m_OnCustomDrawCallback(vh);
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

        protected override void DrawBackground(VertexHelper vh)
        {
            Vector3 p1 = new Vector3(chartX, chartY + chartHeight);
            Vector3 p2 = new Vector3(chartX + chartWidth, chartY + chartHeight);
            Vector3 p3 = new Vector3(chartX + chartWidth, chartY);
            Vector3 p4 = new Vector3(chartX, chartY);
            var backgroundColor = ThemeHelper.GetBackgroundColor(m_ThemeInfo, m_Background);
            ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, backgroundColor);
        }

        public void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
          float tickness, Vector3 pos, Color color, Color toColor, float gap, float[] cornerRadius)
        {
            var backgroundColor = ThemeHelper.GetBackgroundColor(m_ThemeInfo, m_Background);
            var smoothness = m_Settings.cicleSmoothness;
            ChartDrawer.DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, gap,
                cornerRadius, backgroundColor, smoothness);
        }

        protected void DrawLabelBackground(VertexHelper vh, Serie serie, SerieData serieData)
        {
            if (serieData == null || serieData.labelObject == null) return;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (!serieLabel.show) return;
            var invert = serieLabel.autoOffset
                && serie.type == SerieType.Line
                && SerieHelper.IsDownPoint(serie, serieData.index)
                && !serie.areaStyle.show;
            var centerPos = serieData.labelPosition + serieLabel.offset * (invert ? -1 : 1);
            var labelHalfWid = serieData.labelObject.GetLabelWidth() / 2;
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
    }
}
