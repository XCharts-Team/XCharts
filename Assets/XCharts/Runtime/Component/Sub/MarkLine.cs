/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// 标线类型
    /// </summary>
    public enum MarkLineType
    {
        None,
        /// <summary>
        /// 最小值。
        /// </summary>
        Min,
        /// <summary>
        /// 最大值。
        /// </summary>
        Max,
        /// <summary>
        /// 平均值。
        /// </summary>
        Average,
        /// <summary>
        /// 中位数。
        /// </summary>
        Median
    }
    /// <summary>
    /// Data of marking line. 
    /// 图表标线的数据。
    /// </summary>
    [System.Serializable]
    public class MarkLineData : SubComponent
    {
        [SerializeField] private string m_Name;
        [SerializeField] private MarkLineType m_Type = MarkLineType.None;
        [SerializeField] private int m_Dimension = 1;
        [SerializeField] private float m_XPosition;
        [SerializeField] private float m_YPosition;
        [SerializeField] private double m_XValue;
        [SerializeField] private double m_YValue;
        [SerializeField] private int m_Group = 0;
        [SerializeField] private bool m_ZeroPosition = false;

        [SerializeField] private SerieSymbol m_StartSymbol = new SerieSymbol();
        [SerializeField] private SerieSymbol m_EndSymbol = new SerieSymbol();
        [SerializeField] private LineStyle m_LineStyle = new LineStyle();
        [SerializeField] private SerieLabel m_Label = new SerieLabel();
        //[SerializeField] private Emphasis m_Emphasis = new Emphasis();

        public int index { get; set; }
        public Vector3 runtimeStartPosition { get; internal set; }
        public Vector3 runtimeEndPosition { get; internal set; }
        public Vector3 runtimeCurrentEndPosition { get; internal set; }
        public ChartLabel runtimeLabel { get; internal set; }
        public double runtimeValue { get; internal set; }

        /// <summary>
        /// Name of the marker, which will display as a label.
        /// 标线名称，将会作为文字显示。label的formatter可通过{b}显示名称，通过{c}显示数值。
        /// </summary>
        public string name
        {
            get { return m_Name; }
            set { if (PropertyUtil.SetClass(ref m_Name, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Special label types, are used to label maximum value, minimum value and so on.
        /// 特殊的标线类型，用于标注最大值最小值等。
        /// </summary>
        public MarkLineType type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// From which dimension of data to calculate the maximum and minimum value and so on.
        /// 从哪个维度的数据计算最大最小值等。
        /// </summary>
        public int dimension
        {
            get { return m_Dimension; }
            set { if (PropertyUtil.SetStruct(ref m_Dimension, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The x coordinate relative to the origin, in pixels.
        /// 相对原点的 x 坐标，单位像素。当type为None时有效。
        /// </summary>
        public float xPosition
        {
            get { return m_XPosition; }
            set { if (PropertyUtil.SetStruct(ref m_XPosition, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The y coordinate relative to the origin, in pixels.
        /// 相对原点的 y 坐标，单位像素。当type为None时有效。
        /// </summary>
        public float yPosition
        {
            get { return m_YPosition; }
            set { if (PropertyUtil.SetStruct(ref m_YPosition, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The value specified on the X-axis. A value specified when the X-axis is the category axis represents the index of the category axis data, otherwise a specific value.
        /// X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
        /// </summary>
        public double xValue
        {
            get { return m_XValue; }
            set { if (PropertyUtil.SetStruct(ref m_XValue, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// That's the value on the Y-axis. The value specified when the Y axis is the category axis represents the index of the category axis data, otherwise the specific value.
        /// Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
        /// </summary>
        public double yValue
        {
            get { return m_YValue; }
            set { if (PropertyUtil.SetStruct(ref m_YValue, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Grouping. When the group is not 0, it means that this data is the starting point or end point of the marking line. Data consistent with the group form a marking line.
        /// 分组。当group不为0时，表示这个data是标线的起点或终点，group一致的data组成一条标线。
        /// </summary>
        public int group
        {
            get { return m_Group; }
            set { if (PropertyUtil.SetStruct(ref m_Group, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Is the origin of the coordinate system.
        /// 是否为坐标系原点。
        /// </summary>
        public bool zeroPosition
        {
            get { return m_ZeroPosition; }
            set { if (PropertyUtil.SetStruct(ref m_ZeroPosition, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The symbol of the start point of markline.
        /// 起始点的图形标记。
        /// </summary>
        public SerieSymbol startSymbol
        {
            get { return m_StartSymbol; }
            set { if (PropertyUtil.SetClass(ref m_StartSymbol, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The symbol of the end point of markline.
        /// 结束点的图形标记。
        /// </summary>
        public SerieSymbol endSymbol
        {
            get { return m_EndSymbol; }
            set { if (PropertyUtil.SetClass(ref m_EndSymbol, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The line style of markline.
        /// 标线样式。
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (PropertyUtil.SetClass(ref m_LineStyle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Text styles of label. You can set position to Start, Middle, and End to display text in different locations.
        /// 文本样式。可设置position为Start、Middle和End在不同的位置显示文本。
        /// </summary>
        public SerieLabel label
        {
            get { return m_Label; }
            set { if (PropertyUtil.SetClass(ref m_Label, value)) SetVerticesDirty(); }
        }
        // public Emphasis emphasis
        // {
        //     get { return m_Emphasis; }
        //     set { if (PropertyUtil.SetClass(ref m_Emphasis, value)) SetVerticesDirty(); }
        // }
    }

    /// <summary>
    /// Use a line in the chart to illustrate.
    /// 图表标线。
    /// </summary>
    [System.Serializable]
    public class MarkLine : SubComponent
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private SerieAnimation m_Animation = new SerieAnimation();
        [SerializeField] private List<MarkLineData> m_Data = new List<MarkLineData>();

        /// <summary>
        /// Whether to display the marking line.
        /// 是否显示标线。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The animation of markline.
        /// 标线的动画样式。
        /// </summary>
        public SerieAnimation animation
        {
            get { return m_Animation; }
            set { if (PropertyUtil.SetClass(ref m_Animation, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// A list of marked data. When the group of data item is 0, each data item represents a line; 
        /// When the group is not 0, two data items of the same group represent the starting point and 
        /// the ending point of the line respectively to form a line. In this case, the relevant style 
        /// parameters of the line are the parameters of the starting point.
        /// 标线的数据列表。当数据项的group为0时，每个数据项表示一条标线；当group不为0时，相同group的两个数据项分别表
        /// 示标线的起始点和终止点来组成一条标线，此时标线的相关样式参数取起始点的参数。
        /// </summary>
        public List<MarkLineData> data
        {
            get { return m_Data; }
            set { if (PropertyUtil.SetClass(ref m_Data, value)) SetVerticesDirty(); }
        }
        public static MarkLine defaultMarkLine
        {
            get
            {
                var markLine = new MarkLine
                {
                    m_Show = false,
                    m_Data = new List<MarkLineData>()
                };
                var data = new MarkLineData();
                data.name = "average";
                data.type = MarkLineType.Average;
                data.lineStyle.type = LineStyle.Type.Dashed;
                data.lineStyle.color = Color.blue;
                data.startSymbol.show = true;
                data.startSymbol.type = SerieSymbolType.Circle;
                data.endSymbol.show = true;
                data.endSymbol.type = SerieSymbolType.Arrow;
                data.label.show = true;
                data.label.numericFormatter = "f1";
                data.label.formatter = "{c}";
                markLine.data.Add(data);
                return markLine;
            }
        }
    }

    internal class MarkLineHandler : IComponentHandler
    {
        public CoordinateChart chart;
        private GameObject m_MarkLineLabelRoot;

        public MarkLineHandler(CoordinateChart chart)
        {
            this.chart = chart;
        }

        public void DrawBase(VertexHelper vh)
        {
        }

        public void DrawTop(VertexHelper vh)
        {
            DrawMarkLine(vh);
        }

        public void Init()
        {
            m_MarkLineLabelRoot = ChartHelper.AddObject("markline", chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            m_MarkLineLabelRoot.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(m_MarkLineLabelRoot);
            foreach (var serie in chart.series.list) InitMarkLine(serie);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnScroll(PointerEventData eventData)
        {
        }

        public void Update()
        {
            foreach (var serie in chart.series.list)
            {
                var show = serie.show && serie.markLine.show;
                foreach (var data in serie.markLine.data)
                {
                    if (data.runtimeLabel != null)
                    {
                        if (data.runtimeLabel.gameObject.activeSelf != show)
                            data.runtimeLabel.gameObject.SetActive(show);
                        if (show)
                        {
                            data.runtimeLabel.SetPosition(MarkLineHelper.GetLabelPosition(data));
                            data.runtimeLabel.SetText(MarkLineHelper.GetFormatterContent(serie, data));
                        }
                    }
                }
            }
        }

        private void InitMarkLine(Serie serie)
        {
            if (!serie.show || !serie.markLine.show) return;
            ResetTempMarkLineGroupData(serie.markLine);
            var serieColor = (Color)chart.theme.GetColor(chart.GetLegendRealShowNameIndex(serie.name));
            if (m_TempGroupData.Count > 0)
            {
                foreach (var kv in m_TempGroupData)
                {
                    if (kv.Value.Count >= 2)
                    {
                        var data = kv.Value[0];
                        InitMarkLineLabel(serie, data, serieColor);
                    }
                }
            }
            foreach (var data in serie.markLine.data)
            {
                if (data.group != 0) continue;
                InitMarkLineLabel(serie, data, serieColor);
            }
        }

        private void InitMarkLineLabel(Serie serie, MarkLineData data, Color serieColor)
        {
            data.painter = chart.m_PainterTop;
            data.refreshComponent = delegate ()
            {
                var label = data.label;
                var textName = string.Format("markLine_{0}_{1}", serie.index, data.index);
                var color = !ChartHelper.IsClearColor(label.textStyle.color) ? label.textStyle.color : chart.theme.axis.textColor;
                var element = ChartHelper.AddSerieLabel(textName, m_MarkLineLabelRoot.transform, label.backgroundWidth,
                    label.backgroundHeight, color, label.textStyle, chart.theme);
                var isAutoSize = label.backgroundWidth == 0 || label.backgroundHeight == 0;
                var item = new ChartLabel();
                item.SetLabel(element, isAutoSize, label.paddingLeftRight, label.paddingTopBottom);
                item.SetIconActive(false);
                item.SetActive(data.label.show);
                item.SetPosition(MarkLineHelper.GetLabelPosition(data));
                item.SetText(MarkLineHelper.GetFormatterContent(serie, data));
                data.runtimeLabel = item;
            };
            data.refreshComponent();
        }

        private void DrawMarkLine(VertexHelper vh)
        {
            foreach (var serie in chart.series.list)
            {
                DrawMarkLine(vh, serie);
            }
        }

        private Dictionary<int, List<MarkLineData>> m_TempGroupData = new Dictionary<int, List<MarkLineData>>();
        private void DrawMarkLine(VertexHelper vh, Serie serie)
        {
            if (!serie.show || !serie.markLine.show) return;
            if (serie.markLine.data.Count == 0) return;
            var yAxis = chart.GetSerieYAxisOrDefault(serie);
            var xAxis = chart.GetSerieXAxisOrDefault(serie);
            var grid = chart.GetSerieGridOrDefault(serie);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, chart.dataZooms);
            var animation = serie.markLine.animation;
            var showData = serie.GetDataList(dataZoom);
            var sp = Vector3.zero;
            var ep = Vector3.zero;
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.name);
            var serieColor = SerieHelper.GetLineColor(serie, chart.theme, colorIndex, false);
            animation.InitProgress(1, 0, 1f);
            ResetTempMarkLineGroupData(serie.markLine);
            if (m_TempGroupData.Count > 0)
            {
                foreach (var kv in m_TempGroupData)
                {
                    if (kv.Value.Count >= 2)
                    {
                        sp = GetSinglePos(xAxis, yAxis, grid, serie, dataZoom, kv.Value[0], showData.Count);
                        ep = GetSinglePos(xAxis, yAxis, grid, serie, dataZoom, kv.Value[1], showData.Count);
                        kv.Value[0].runtimeStartPosition = sp;
                        kv.Value[1].runtimeEndPosition = ep;
                        DrawMakLineData(vh, kv.Value[0], animation, serie, grid, serieColor, sp, ep);
                    }
                }
            }
            foreach (var data in serie.markLine.data)
            {
                if (data.group != 0) continue;
                switch (data.type)
                {
                    case MarkLineType.Min:
                        data.runtimeValue = SerieHelper.GetMinData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.Max:
                        data.runtimeValue = SerieHelper.GetMaxData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.Average:
                        data.runtimeValue = SerieHelper.GetAverageData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.Median:
                        data.runtimeValue = SerieHelper.GetMedianData(serie, data.dimension, dataZoom);
                        GetStartEndPos(xAxis, yAxis, grid, data.runtimeValue, ref sp, ref ep);
                        break;
                    case MarkLineType.None:
                        if (data.xPosition != 0)
                        {
                            data.runtimeValue = data.xPosition;
                            var pX = grid.runtimeX + data.xPosition;
                            sp = new Vector3(pX, grid.runtimeY);
                            ep = new Vector3(pX, grid.runtimeY + grid.runtimeHeight);
                        }
                        else if (data.yPosition != 0)
                        {
                            data.runtimeValue = data.yPosition;
                            var pY = grid.runtimeY + data.yPosition;
                            sp = new Vector3(grid.runtimeX, pY);
                            ep = new Vector3(grid.runtimeX + grid.runtimeWidth, pY);
                        }
                        else if (data.yValue != 0)
                        {
                            data.runtimeValue = data.yValue;
                            if (yAxis.IsCategory())
                            {
                                var pY = AxisHelper.GetAxisPosition(grid, yAxis, data.yValue, showData.Count, dataZoom);
                                sp = new Vector3(grid.runtimeX, pY);
                                ep = new Vector3(grid.runtimeX + grid.runtimeWidth, pY);
                            }
                            else
                            {
                                GetStartEndPos(xAxis, yAxis, grid, data.yValue, ref sp, ref ep);
                            }
                        }
                        else
                        {
                            data.runtimeValue = data.xValue;
                            if (xAxis.IsCategory())
                            {
                                var pX = AxisHelper.GetAxisPosition(grid, xAxis, data.xValue, showData.Count, dataZoom);
                                sp = new Vector3(pX, grid.runtimeY);
                                ep = new Vector3(pX, grid.runtimeY + grid.runtimeHeight);
                            }
                            else
                            {
                                GetStartEndPos(xAxis, yAxis, grid, data.xValue, ref sp, ref ep);
                            }
                        }
                        break;
                    default:
                        break;
                }
                data.runtimeStartPosition = sp;
                data.runtimeEndPosition = ep;
                DrawMakLineData(vh, data, animation, serie, grid, serieColor, sp, ep);
            }
            if (!animation.IsFinish())
            {
                animation.CheckProgress(1f);
                chart.RefreshTopPainter();
            }
        }

        private void ResetTempMarkLineGroupData(MarkLine markLine)
        {
            m_TempGroupData.Clear();
            for (int i = 0; i < markLine.data.Count; i++)
            {
                var data = markLine.data[i];
                data.index = i;
                if (data.group == 0) continue;
                if (!m_TempGroupData.ContainsKey(data.group))
                {
                    m_TempGroupData[data.group] = new List<MarkLineData>();
                }
                m_TempGroupData[data.group].Add(data);
            }
        }

        private void DrawMakLineData(VertexHelper vh, MarkLineData data, SerieAnimation animation, Serie serie,
            Grid grid, Color32 serieColor, Vector3 sp, Vector3 ep)
        {
            if (!animation.IsFinish())
                ep = Vector3.Lerp(sp, ep, animation.GetCurrDetail());
            data.runtimeCurrentEndPosition = ep;
            if (sp != Vector3.zero || ep != Vector3.zero)
            {
                chart.ClampInChart(ref sp);
                chart.ClampInChart(ref ep);
                var theme = chart.theme.axis;
                var lineColor = ChartHelper.IsClearColor(data.lineStyle.color) ? serieColor : data.lineStyle.color;
                var lineWidth = data.lineStyle.width == 0 ? theme.lineWidth : data.lineStyle.width;
                ChartDrawer.DrawLineStyle(vh, data.lineStyle, sp, ep, lineWidth, LineStyle.Type.Dashed, lineColor, lineColor);
                if (data.startSymbol != null && data.startSymbol.show)
                {
                    DrawMarkLineSymbol(vh, data.startSymbol, serie, grid, chart.theme, sp, sp, lineColor);
                }
                if (data.endSymbol != null && data.endSymbol.show)
                {
                    DrawMarkLineSymbol(vh, data.endSymbol, serie, grid, chart.theme, ep, sp, lineColor);
                }
            }
        }

        private void DrawMarkLineSymbol(VertexHelper vh, SerieSymbol symbol, Serie serie, Grid grid, ChartTheme theme,
            Vector3 pos, Vector3 startPos, Color32 lineColor)
        {
            var symbolSize = symbol.GetSize(null, theme.serie.lineSymbolSize);
            var tickness = SerieHelper.GetSymbolBorder(serie, null, theme, false);
            var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, null, false);
            chart.Internal_CheckClipAndDrawSymbol(vh, symbol.type, symbolSize, tickness, pos, lineColor, lineColor,
                ColorUtil.clearColor32, symbol.gap, true, cornerRadius, grid, startPos);
        }

        private void GetStartEndPos(Axis xAxis, Axis yAxis, Grid grid, double value, ref Vector3 sp, ref Vector3 ep)
        {
            if (xAxis.IsCategory())
            {
                var pY = AxisHelper.GetAxisPosition(grid, yAxis, value);
                sp = new Vector3(grid.runtimeX, pY);
                ep = new Vector3(grid.runtimeX + grid.runtimeWidth, pY);
            }
            else
            {
                var pX = AxisHelper.GetAxisPosition(grid, xAxis, value);
                sp = new Vector3(pX, grid.runtimeY);
                ep = new Vector3(pX, grid.runtimeY + grid.runtimeHeight);
            }
        }

        private float GetAxisPosition(Grid grid, Axis axis, DataZoom dataZoom, int dataCount, double value)
        {
            return AxisHelper.GetAxisPosition(grid, axis, value, dataCount, dataZoom);
        }

        private Vector3 GetSinglePos(Axis xAxis, Axis yAxis, Grid grid, Serie serie, DataZoom dataZoom, MarkLineData data,
            int serieDataCount)
        {
            switch (data.type)
            {
                case MarkLineType.Min:
                    var serieData = SerieHelper.GetMinSerieData(serie, data.dimension, dataZoom);
                    data.runtimeValue = serieData.GetData(data.dimension);
                    var pX = GetAxisPosition(grid, xAxis, dataZoom, serieDataCount, serieData.index);
                    var pY = GetAxisPosition(grid, yAxis, dataZoom, serieDataCount, data.runtimeValue);
                    return new Vector3(pX, pY);
                case MarkLineType.Max:
                    serieData = SerieHelper.GetMaxSerieData(serie, data.dimension, dataZoom);
                    data.runtimeValue = serieData.GetData(data.dimension);
                    pX = GetAxisPosition(grid, xAxis, dataZoom, serieDataCount, serieData.index);
                    pY = GetAxisPosition(grid, yAxis, dataZoom, serieDataCount, data.runtimeValue);
                    return new Vector3(pX, pY);
                case MarkLineType.None:
                    if (data.zeroPosition)
                    {
                        data.runtimeValue = 0;
                        return grid.runtimePosition;
                    }
                    else
                    {
                        pX = data.xPosition != 0 ? grid.runtimeX + data.xPosition :
                            GetAxisPosition(grid, xAxis, dataZoom, serieDataCount, data.xValue);
                        pY = data.yPosition != 0 ? grid.runtimeY + data.yPosition :
                            GetAxisPosition(grid, yAxis, dataZoom, serieDataCount, data.yValue);
                        data.runtimeValue = data.yValue;
                        return new Vector3(pX, pY);
                    }
                default:
                    return grid.runtimePosition;
            }
        }
    }

    internal static class MarkLineHelper
    {
        public static string GetFormatterContent(Serie serie, MarkLineData data)
        {
            var serieLabel = data.label;
            var numericFormatter = serieLabel.numericFormatter;
            if (serieLabel.formatterFunction != null)
            {
                return serieLabel.formatterFunction(data.index, data.runtimeValue);
            }
            if (string.IsNullOrEmpty(serieLabel.formatter))
                return ChartCached.NumberToStr(data.runtimeValue, numericFormatter);
            else
            {
                var content = serieLabel.formatter;
                FormatterHelper.ReplaceSerieLabelContent(ref content, numericFormatter, data.runtimeValue,
                    0, serie.name, data.name, Color.clear);
                return content;
            }
        }

        public static Vector3 GetLabelPosition(MarkLineData data)
        {
            if (!data.label.show) return Vector3.zero;
            var dir = (data.runtimeEndPosition - data.runtimeStartPosition).normalized;
            var horizontal = Mathf.Abs(Vector3.Dot(dir, Vector3.right)) == 1;
            var labelWidth = data.runtimeLabel == null ? 50 : data.runtimeLabel.GetLabelWidth();
            var labelHeight = data.runtimeLabel == null ? 20 : data.runtimeLabel.GetLabelHeight();
            switch (data.label.position)
            {
                case SerieLabel.Position.Start:
                    if (horizontal) return data.runtimeStartPosition + data.label.offset + labelWidth / 2 * Vector3.left;
                    else return data.runtimeStartPosition + data.label.offset + labelHeight / 2 * Vector3.down;
                case SerieLabel.Position.Middle:
                    var center = (data.runtimeStartPosition + data.runtimeCurrentEndPosition) / 2;
                    if (horizontal) return center + data.label.offset + labelHeight / 2 * Vector3.up;
                    else return center + data.label.offset + labelWidth / 2 * Vector3.right;
                default:
                    if (horizontal) return data.runtimeCurrentEndPosition + data.label.offset + labelWidth / 2 * Vector3.right;
                    else return data.runtimeCurrentEndPosition + data.label.offset + labelHeight / 2 * Vector3.up;
            }
        }
    }
}