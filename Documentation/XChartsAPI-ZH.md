# API

[XCharts主页](https://github.com/XCharts-Team/XCharts)<br/>
[XCharts配置项手册](XChartsConfiguration-ZH.md)<br/>
[XCharts问答](XChartsFAQ-ZH.md)

## All Class

- [AnimationStyleHelper](#AnimationStyleHelper)
- [AxisContext](#AxisContext)
- [AxisHandler&lt;T&gt;](#AxisHandler&lt;T&gt;)
- [AxisHelper](#AxisHelper)
- [BarChart](#BarChart)
- [BaseChart](#BaseChart)
- [BaseGraph](#BaseGraph)
- [CandlestickChart](#CandlestickChart)
- [ChartCached](#ChartCached)
- [ChartConst](#ChartConst)
- [ChartDrawer](#ChartDrawer)
- [ChartHelper](#ChartHelper)
- [ChartLabel](#ChartLabel)
- [ChartObject](#ChartObject)
- [CheckHelper](#CheckHelper)
- [ColorUtil](#ColorUtil)
- [ComponentHandlerAttribute](#ComponentHandlerAttribute)
- [ComponentHelper](#ComponentHelper)
- [CoordOptionsAttribute](#CoordOptionsAttribute)
- [DataZoomContext](#DataZoomContext)
- [DataZoomHelper](#DataZoomHelper)
- [DateTimeUtil](#DateTimeUtil)
- [DefaultAnimationAttribute](#DefaultAnimationAttribute)
- [DefineSymbolsUtil](#DefineSymbolsUtil)
- [FormatterHelper](#FormatterHelper)
- [GridCoordContext](#GridCoordContext)
- [HeatmapChart](#HeatmapChart)
- [IgnoreDoc](#IgnoreDoc)
- [InteractData](#InteractData)
- [LayerHelper](#LayerHelper)
- [LegendContext](#LegendContext)
- [LegendHelper](#LegendHelper)
- [LegendItem](#LegendItem)
- [LineChart](#LineChart)
- [ListFor](#ListFor)
- [ListForComponent](#ListForComponent)
- [ListForSerie](#ListForSerie)
- [MainComponentContext](#MainComponentContext)
- [MainComponentHandler](#MainComponentHandler)
- [MainComponentHandler&lt;T&gt;](#MainComponentHandler&lt;T&gt;)
- [MathUtil](#MathUtil)
- [Painter](#Painter)
- [ParallelChart](#ParallelChart)
- [ParallelCoordContext](#ParallelCoordContext)
- [PieChart](#PieChart)
- [PolarChart](#PolarChart)
- [PolarCoordContext](#PolarCoordContext)
- [ProgressBar](#ProgressBar)
- [PropertyUtil](#PropertyUtil)
- [RadarChart](#RadarChart)
- [RadarCoordContext](#RadarCoordContext)
- [ReflectionUtil](#ReflectionUtil)
- [RequireChartComponentAttribute](#RequireChartComponentAttribute)
- [RingChart](#RingChart)
- [RuntimeUtil](#RuntimeUtil)
- [ScatterChart](#ScatterChart)
- [SerieContext](#SerieContext)
- [SerieConvertAttribute](#SerieConvertAttribute)
- [SerieDataContext](#SerieDataContext)
- [SerieDataExtraComponentAttribute](#SerieDataExtraComponentAttribute)
- [SerieDataExtraFieldAttribute](#SerieDataExtraFieldAttribute)
- [SerieExtraComponentAttribute](#SerieExtraComponentAttribute)
- [SerieHandler](#SerieHandler)
- [SerieHandler&lt;T&gt;](#SerieHandler&lt;T&gt;)
- [SerieHandlerAttribute](#SerieHandlerAttribute)
- [SerieHelper](#SerieHelper)
- [SerieLabelHelper](#SerieLabelHelper)
- [SerieLabelPool](#SerieLabelPool)
- [SerieParams](#SerieParams)
- [SeriesHelper](#SeriesHelper)
- [SimplifiedBarChart](#SimplifiedBarChart)
- [SimplifiedCandlestickChart](#SimplifiedCandlestickChart)
- [SimplifiedLineChart](#SimplifiedLineChart)
- [Since](#Since)
- [SVG](#SVG)
- [SVGImage](#SVGImage)
- [SVGPath](#SVGPath)
- [SVGPathSeg](#SVGPathSeg)
- [TooltipContext](#TooltipContext)
- [TooltipData](#TooltipData)
- [TooltipHelper](#TooltipHelper)
- [TooltipView](#TooltipView)
- [TooltipViewItem](#TooltipViewItem)
- [UGL](#UGL)
- [UGLExample](#UGLExample)
- [UGLHelper](#UGLHelper)
- [VisualMapContext](#VisualMapContext)
- [VisualMapHelper](#VisualMapHelper)
- [XChartsMgr](#XChartsMgr)
- [XCResourceImporterWindow](#XCResourceImporterWindow)
- [XCThemeMgr](#XCThemeMgr)

## AnimationStyleHelper

|public method|description|
|--|--|
| CheckDataAnimation() |public static float CheckDataAnimation(BaseChart chart, Serie serie, int dataIndex, float destProgress, float startPorgress = 0)|
| GetAnimationPosition() |public static bool GetAnimationPosition(AnimationStyle animation, bool isY, Vector3 lp, Vector3 cp, float progress, ref Vector3 ip)|
| UpdateAnimationType() |public static void UpdateAnimationType(AnimationStyle animation, AnimationType defaultType)|
| UpdateSerieAnimation() |public static void UpdateSerieAnimation(Serie serie)|

## AxisContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## AxisHandler&lt;T&gt;

Inherits or Implemented: [MainComponentHandler](#MainComponentHandler)

## AxisHelper

|public method|description|
|--|--|
| AdjustCircleLabelPos() |public static void AdjustCircleLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)|
| AdjustMinMaxValue() |public static void AdjustMinMaxValue(Axis axis, ref double minValue, ref double maxValue, bool needFormat, double ceilRate = 0)<br/>调整最大最小值 |
| AdjustRadiusAxisLabelPos() |public static void AdjustRadiusAxisLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)|
| GetAxisLineArrowOffset() |public static float GetAxisLineArrowOffset(Axis axis)<br/>包含箭头偏移的轴线长度 |
| GetAxisPosition() |public static float GetAxisPosition(GridCoord grid, Axis axis, double value, int dataCount = 0, DataZoom dataZoom = null)|
| GetAxisPositionValue() |public static double GetAxisPositionValue(float xy, float axisLength, double axisRange, float axisStart, float axisOffset)|
| GetAxisPositionValue() |public static double GetAxisPositionValue(GridCoord grid, Axis axis, Vector3 pos)|
| GetAxisValueDistance() |public static float GetAxisValueDistance(GridCoord grid, Axis axis, float scaleWidth, double value)<br/>获得数值value在坐标轴上相对起点的距离 |
| GetAxisValueLength() |public static float GetAxisValueLength(GridCoord grid, Axis axis, float scaleWidth, double value)<br/>获得数值value在坐标轴上对应的长度 |
| GetAxisValuePosition() |public static float GetAxisValuePosition(GridCoord grid, Axis axis, float scaleWidth, double value)<br/>获得数值value在坐标轴上的坐标位置 |
| GetAxisValueSplitIndex() |public static int GetAxisValueSplitIndex(Axis axis, double value, int totalSplitNumber = -1)<br/>获得数值value在坐标轴上对应的split索引 |
| GetAxisXOrY() |public static float GetAxisXOrY(GridCoord grid, Axis axis, Axis relativedAxis)|
| GetDataWidth() |public static float GetDataWidth(Axis axis, float coordinateWidth, int dataCount, DataZoom dataZoom)<br/>获得一个类目数据在坐标系中代表的宽度 |
| GetEachWidth() |public static float GetEachWidth(Axis axis, float coordinateWidth, DataZoom dataZoom = null)|
| GetScaleNumber() |public static int GetScaleNumber(Axis axis, float coordinateWidth, DataZoom dataZoom = null)<br/>获得分割线条数 |
| GetScaleWidth() |public static float GetScaleWidth(Axis axis, float coordinateWidth, int index, DataZoom dataZoom = null)<br/>获得分割段宽度 |
| GetSplitNumber() |public static int GetSplitNumber(Axis axis, float coordinateWid, DataZoom dataZoom)<br/>获得分割段数 |
| GetTotalSplitGridNum() |public static int GetTotalSplitGridNum(Axis axis)<br/>获得分割网格个数，包含次刻度 |
| GetXAxisXOrY() |public static float GetXAxisXOrY(GridCoord grid, Axis xAxis, Axis relativedAxis)|
| GetYAxisXOrY() |public static float GetYAxisXOrY(GridCoord grid, Axis yAxis, Axis relativedAxis)|
| NeedShowSplit() |public static bool NeedShowSplit(Axis axis)|

## BarChart

Inherits or Implemented: [BaseChart](#BaseChart)

## BaseChart

Inherits or Implemented: [BaseGraph](#BaseGraph),[ISerializationCallbackReceiver](#ISerializationCallbackReceiver)

|public method|description|
|--|--|
| AddChartComponent() |public MainComponent AddChartComponent(Type type)|
| AddChartComponent&lt;T&gt;() |public T AddChartComponent&lt;T&gt;() where T : MainComponent|
| AddChartComponentWhenNoExist&lt;T&gt;() |public T AddChartComponentWhenNoExist&lt;T&gt;() where T : MainComponent|
| AddData() |public SerieData AddData(int serieIndex, DateTime time, double yValue, string dataName = null, string dataId = null)<br/>添加（time,y）数据到指定的系列中。 |
| AddData() |public SerieData AddData(int serieIndex, double data, string dataName = null, string dataId = null)<br/>添加一个数据到指定的系列中。 |
| AddData() |public SerieData AddData(int serieIndex, double indexOrTimestamp, double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)|
| AddData() |public SerieData AddData(int serieIndex, double xValue, double yValue, string dataName = null, string dataId = null)<br/>添加（x,y）数据到指定系列中。 |
| AddData() |public SerieData AddData(int serieIndex, List&lt;double&gt; multidimensionalData, string dataName = null, string dataId = null)<br/>添加多维数据（x,y,z...）到指定的系列中。 |
| AddData() |public SerieData AddData(int serieIndex, params double[] multidimensionalData)<br/>添加多维数据（x,y,z...）到指定的系列中。 |
| AddData() |public SerieData AddData(string serieName, DateTime time, double yValue, string dataName = null, string dataId = null)<br/>添加（time,y）数据到指定的系列中。 |
| AddData() |public SerieData AddData(string serieName, double data, string dataName = null, string dataId = null)<br/>If serieName doesn't exist in legend,will be add to legend. |
| AddData() |public SerieData AddData(string serieName, double indexOrTimestamp, double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)|
| AddData() |public SerieData AddData(string serieName, double xValue, double yValue, string dataName = null, string dataId = null)<br/>添加（x,y）数据到指定系列中。 |
| AddData() |public SerieData AddData(string serieName, List&lt;double&gt; multidimensionalData, string dataName = null, string dataId = null)<br/>添加多维数据（x,y,z...）到指定的系列中。 |
| AddData() |public SerieData AddData(string serieName, params double[] multidimensionalData)<br/>添加多维数据（x,y,z...）到指定的系列中。 |
| AddSerie&lt;T&gt;() |public T AddSerie&lt;T&gt;(string serieName = null, bool show = true, bool addToHead = false) where T : Serie|
| AddXAxisData() |public void AddXAxisData(string category, int xAxisIndex = 0)<br/>添加一个类目数据到指定的x轴。 |
| AddXAxisIcon() |public void AddXAxisIcon(Sprite icon, int xAxisIndex = 0)<br/>添加一个图标到指定的x轴。 |
| AddYAxisData() |public void AddYAxisData(string category, int yAxisIndex = 0)<br/>添加一个类目数据到指定的y轴。 |
| AddYAxisIcon() |public void AddYAxisIcon(Sprite icon, int yAxisIndex = 0)<br/>添加一个图标到指定的y轴。 |
| AnimationEnable() |public void AnimationEnable(bool flag)<br/>启用或关闭起始动画。 |
| AnimationFadeIn() |public void AnimationFadeIn()<br/>开始渐入动画。 |
| AnimationFadeOut() |public void AnimationFadeOut()<br/>开始渐出动画。 |
| AnimationPause() |public void AnimationPause()<br/>暂停动画。 |
| AnimationReset() |public void AnimationReset()<br/>重置动画。 |
| AnimationResume() |public void AnimationResume()<br/>继续动画。 |
| CanAddChartComponent() |public bool CanAddChartComponent(Type type)|
| CanAddSerie() |public bool CanAddSerie(Type type)|
| CanAddSerie&lt;T&gt;() |public bool CanAddSerie&lt;T&gt;() where T : Serie|
| CanMultipleComponent() |public bool CanMultipleComponent(Type type)|
| ClampInChart() |public void ClampInChart(ref Vector3 pos)|
| ClampInGrid() |public Vector3 ClampInGrid(GridCoord grid, Vector3 pos)|
| ClearComponentData() |public virtual void ClearComponentData()<br/>清空所有组件的数据。 |
| ClearData() |public virtual void ClearData()<br/>清空所有组件和Serie的数据。注意：Serie只是清空数据，不会移除Serie。 |
| ClearSerieData() |public virtual void ClearSerieData()<br/>清空所有serie的数据。 |
| ClickLegendButton() |public void ClickLegendButton(int legendIndex, string legendName, bool show)<br/>点击图例按钮 |
| CovertSerie() |public bool CovertSerie(Serie serie, Type type)|
| CovertSerie&lt;T&gt;() |public bool CovertSerie&lt;T&gt;(Serie serie) where T : Serie|
| CovertXYAxis() |public void CovertXYAxis(int index)<br/>转换X轴和Y轴的配置 |
| GenerateDefaultSerieName() |public string GenerateDefaultSerieName()|
| GetAllSerieDataCount() |public int GetAllSerieDataCount()|
| GetChartBackgroundColor() |public Color32 GetChartBackgroundColor()|
| GetChartComponent&lt;T&gt;() |public T GetChartComponent&lt;T&gt;(int index = 0) where T : MainComponent|
| GetChartComponentNum() |public int GetChartComponentNum(Type type)|
| GetChartComponentNum&lt;T&gt;() |public int GetChartComponentNum&lt;T&gt;() where T : MainComponent|
| GetChartComponents&lt;T&gt;() |public List&lt;MainComponent&gt; GetChartComponents&lt;T&gt;() where T : MainComponent|
| GetData() |public double GetData(int serieIndex, int dataIndex, int dimension = 1)|
| GetData() |public double GetData(string serieName, int dataIndex, int dimension = 1)|
| GetDataZoomOfAxis() |public DataZoom GetDataZoomOfAxis(Axis axis)|
| GetDataZoomOfSerie() |public void GetDataZoomOfSerie(Serie serie, out DataZoom xDataZoom, out DataZoom yDataZoom)|
| GetGrid() |public GridCoord GetGrid(Vector2 local)|
| GetGridOfDataZoom() |public GridCoord GetGridOfDataZoom(DataZoom dataZoom)|
| GetItemColor() |public Color32 GetItemColor(Serie serie)|
| GetItemColor() |public Color32 GetItemColor(Serie serie, SerieData serieData)|
| GetItemColor() |public Color32 GetItemColor(Serie serie, SerieData serieData, int colorIndex)|
| GetLegendRealShowNameColor() |public Color32 GetLegendRealShowNameColor(string name)|
| GetLegendRealShowNameIndex() |public int GetLegendRealShowNameIndex(string name)|
| GetMarkColor() |public Color32 GetMarkColor(Serie serie, SerieData serieData)<br/>获得Serie的标识颜色。 |
| GetOrAddChartComponent&lt;T&gt;() |public T GetOrAddChartComponent&lt;T&gt;() where T : MainComponent|
| GetPainter() |public Painter GetPainter(int index)|
| GetSerie() |public Serie GetSerie(int serieIndex)|
| GetSerie() |public Serie GetSerie(string serieName)|
| GetSerie&lt;T&gt;() |public T GetSerie&lt;T&gt;() where T : Serie|
| GetSerie&lt;T&gt;() |public T GetSerie&lt;T&gt;(int serieIndex) where T : Serie|
| GetSerieBarGap&lt;T&gt;() |public float GetSerieBarGap&lt;T&gt;() where T : Serie|
| GetSerieBarRealCount&lt;T&gt;() |public int GetSerieBarRealCount&lt;T&gt;() where T : Serie|
| GetSerieIndexIfStack&lt;T&gt;() |public int GetSerieIndexIfStack&lt;T&gt;(Serie currSerie) where T : Serie|
| GetSerieSameStackTotalValue&lt;T&gt;() |public double GetSerieSameStackTotalValue&lt;T&gt;(string stack, int dataIndex) where T : Serie|
| GetSeriesMinMaxValue() |public virtual void GetSeriesMinMaxValue(Axis axis, int axisIndex, out double tempMinValue, out double tempMaxValue)|
| GetSerieTotalGap&lt;T&gt;() |public float GetSerieTotalGap&lt;T&gt;(float categoryWidth, float gap, int index) where T : Serie|
| GetSerieTotalWidth&lt;T&gt;() |public float GetSerieTotalWidth&lt;T&gt;(float categoryWidth, float gap, int realBarCount) where T : Serie|
| GetTitlePosition() |public Vector3 GetTitlePosition(Title title)|
| GetVisualMapOfSerie() |public VisualMap GetVisualMapOfSerie(Serie serie)|
| GetXDataZoomOfSerie() |public DataZoom GetXDataZoomOfSerie(Serie serie)|
| GetXLerpColor() |public Color32 GetXLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, GridCoord grid)|
| GetYLerpColor() |public Color32 GetYLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, GridCoord grid)|
| HasChartComponent() |public bool HasChartComponent(Type type)|
| HasChartComponent&lt;T&gt;() |public bool HasChartComponent&lt;T&gt;()|
| HasSerie() |public bool HasSerie(Type type)|
| HasSerie&lt;T&gt;() |public bool HasSerie&lt;T&gt;() where T : Serie|
| Init() |public void Init(bool defaultChart = true)|
| InitAxisRuntimeData() |public virtual void InitAxisRuntimeData(Axis axis) { }|
| InsertSerie() |public void InsertSerie(Serie serie, int index = -1, bool addToHead = false)|
| InsertSerie&lt;T&gt;() |public T InsertSerie&lt;T&gt;(int index, string serieName = null, bool show = true) where T : Serie|
| Internal_CheckAnimation() |public void Internal_CheckAnimation()|
| IsActiveByLegend() |public virtual bool IsActiveByLegend(string legendName)<br/>获得指定图例名字的系列是否显示。 |
| IsAllAxisCategory() |public bool IsAllAxisCategory()<br/>纯类目轴。 |
| IsAllAxisValue() |public bool IsAllAxisValue()<br/>纯数值坐标轴（数值轴或对数轴）。 |
| IsInAnyGrid() |public bool IsInAnyGrid(Vector2 local)|
| IsInChart() |public bool IsInChart(float x, float y)|
| IsInChart() |public bool IsInChart(Vector2 local)<br/>坐标是否在图表范围内 |
| IsSerieName() |public bool IsSerieName(string name)|
| MoveDownSerie() |public bool MoveDownSerie(int serieIndex)|
| MoveUpSerie() |public bool MoveUpSerie(int serieIndex)|
| OnAfterDeserialize() |public void OnAfterDeserialize()|
| OnBeforeSerialize() |public void OnBeforeSerialize()|
| OnBeginDrag() |public override void OnBeginDrag(PointerEventData eventData)|
| OnDataZoomRangeChanged() |public virtual void OnDataZoomRangeChanged(DataZoom dataZoom)|
| OnDrag() |public override void OnDrag(PointerEventData eventData)|
| OnEndDrag() |public override void OnEndDrag(PointerEventData eventData)|
| OnLegendButtonClick() |public virtual void OnLegendButtonClick(int index, string legendName, bool show)|
| OnLegendButtonEnter() |public virtual void OnLegendButtonEnter(int index, string legendName)|
| OnLegendButtonExit() |public virtual void OnLegendButtonExit(int index, string legendName)|
| OnPointerClick() |public override void OnPointerClick(PointerEventData eventData)|
| OnPointerDown() |public override void OnPointerDown(PointerEventData eventData)|
| OnPointerEnter() |public override void OnPointerEnter(PointerEventData eventData)|
| OnPointerExit() |public override void OnPointerExit(PointerEventData eventData)|
| OnPointerUp() |public override void OnPointerUp(PointerEventData eventData)|
| OnScroll() |public override void OnScroll(PointerEventData eventData)|
| RefreshBasePainter() |public void RefreshBasePainter()|
| RefreshChart() |public void RefreshChart()<br/>在下一帧刷新整个图表。 |
| RefreshChart() |public void RefreshChart(int serieIndex)<br/>在下一帧刷新图表的指定serie。 |
| RefreshChart() |public void RefreshChart(Serie serie)<br/>在下一帧刷新图表的指定serie。 |
| RefreshDataZoom() |public void RefreshDataZoom()<br/>在下一帧刷新DataZoom |
| RefreshGraph() |public override void RefreshGraph()|
| RefreshPainter() |public void RefreshPainter(int index)|
| RefreshPainter() |public void RefreshPainter(Serie serie)|
| RefreshTopPainter() |public void RefreshTopPainter()|
| RefreshUpperPainter() |public void RefreshUpperPainter()|
| RemoveAllChartComponent() |public void RemoveAllChartComponent()|
| RemoveAllSerie() |public virtual void RemoveAllSerie()<br/>移除所有的Serie。当确认只需要移除Serie时使用该接口，其他情况下一般用RemoveData()。 |
| RemoveChartComponent() |public bool RemoveChartComponent(MainComponent component)|
| RemoveChartComponent() |public bool RemoveChartComponent(Type type, int index = 0)|
| RemoveChartComponent&lt;T&gt;() |public bool RemoveChartComponent&lt;T&gt;(int index = 0)|
| RemoveChartComponents() |public int RemoveChartComponents(Type type)|
| RemoveChartComponents&lt;T&gt;() |public int RemoveChartComponents&lt;T&gt;()|
| RemoveData() |public virtual void RemoveData()<br/>清空所有组件数据，并移除所有Serie。一般在图表重新初始化时使用。 注意：组件只清空数据部分，参数会保留不会被重置。 |
| RemoveData() |public virtual void RemoveData(string serieName)<br/>清除指定系列名称的数据。 |
| RemoveSerie() |public void RemoveSerie(int serieIndex)|
| RemoveSerie() |public void RemoveSerie(Serie serie)|
| RemoveSerie() |public void RemoveSerie(string serieName)|
| RemoveSerie&lt;T&gt;() |public void RemoveSerie&lt;T&gt;() where T : Serie|
| ReplaceSerie() |public bool ReplaceSerie(Serie oldSerie, Serie newSerie)|
| ResetDataIndex() |public bool ResetDataIndex(int serieIndex)<br/>重置serie的数据项索引。避免数据项索引异常。 |
| SaveAsImage() |public void SaveAsImage(string imageType = "png", string savePath = "")<br/>保存图表为图片。 |
| SetBasePainterMaterial() |public void SetBasePainterMaterial(Material material)<br/>设置Base Painter的材质球 |
| SetMaxCache() |public void SetMaxCache(int maxCache)<br/>设置可缓存的最大数据量。当数据量超过该值时，会自动删除第一个值再加入最新值。 |
| SetPainterActive() |public void SetPainterActive(int index, bool flag)|
| SetSerieActive() |public void SetSerieActive(int serieIndex, bool active)<br/>设置指定系列是否显示。 |
| SetSerieActive() |public void SetSerieActive(Serie serie, bool active)|
| SetSerieActive() |public void SetSerieActive(string serieName, bool active)<br/>设置指定系列是否显示。 |
| SetSeriePainterMaterial() |public void SetSeriePainterMaterial(Material material)<br/>设置Serie Painter的材质球 |
| SetTopPainterMaterial() |public void SetTopPainterMaterial(Material material)<br/>设置Top Painter的材质球 |
| SetUpperPainterMaterial() |public void SetUpperPainterMaterial(Material material)<br/>设置Upper Painter的材质球 |
| TryAddChartComponent() |public bool TryAddChartComponent(Type type)|
| TryAddChartComponent&lt;T&gt;() |public bool TryAddChartComponent&lt;T&gt;() where T : MainComponent|
| TryAddChartComponent&lt;T&gt;() |public bool TryAddChartComponent&lt;T&gt;(out T component) where T : MainComponent|
| TryGetChartComponent&lt;T&gt;() |public bool TryGetChartComponent&lt;T&gt;(out T component, int index = 0)|
| UdpateXAxisIcon() |public void UdpateXAxisIcon(int index, Sprite icon, int xAxisIndex = 0)<br/>更新X轴图标。 |
| UpdateData() |public bool UpdateData(int serieIndex, int dataIndex, double value)<br/>更新指定系列中的指定索引数据。 |
| UpdateData() |public bool UpdateData(int serieIndex, int dataIndex, int dimension, double value)<br/>更新指定系列指定索引指定维数的数据。维数从0开始。 |
| UpdateData() |public bool UpdateData(int serieIndex, int dataIndex, List&lt;double&gt; multidimensionalData)<br/>更新指定系列指定索引的数据项的多维数据。 |
| UpdateData() |public bool UpdateData(string serieName, int dataIndex, double value)<br/>更新指定系列中的指定索引数据。 |
| UpdateData() |public bool UpdateData(string serieName, int dataIndex, int dimension, double value)<br/>更新指定系列指定索引指定维数的数据。维数从0开始。 |
| UpdateData() |public bool UpdateData(string serieName, int dataIndex, List&lt;double&gt; multidimensionalData)<br/>更新指定系列指定索引的数据项的多维数据。 |
| UpdateDataName() |public bool UpdateDataName(int serieIndex, int dataIndex, string dataName)<br/>更新指定系列中的指定索引数据名称。 |
| UpdateDataName() |public bool UpdateDataName(string serieName, int dataIndex, string dataName)<br/>更新指定系列中的指定索引数据名称。 |
| UpdateLegendColor() |public virtual void UpdateLegendColor(string legendName, bool active)|
| UpdateTheme() |public bool UpdateTheme(ThemeType theme)<br/>切换内置主题。 |
| UpdateTheme() |public void UpdateTheme(Theme theme)<br/>切换图表主题。 |
| UpdateXAxisData() |public void UpdateXAxisData(int index, string category, int xAxisIndex = 0)<br/>更新X轴类目数据。 |
| UpdateYAxisData() |public void UpdateYAxisData(int index, string category, int yAxisIndex = 0)<br/>更新Y轴类目数据。 |
| UpdateYAxisIcon() |public void UpdateYAxisIcon(int index, Sprite icon, int yAxisIndex = 0)<br/>更新Y轴图标。 |

## BaseGraph

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic),[IPointerDownHandler](#IPointerDownHandler),[IPointerUpHandler](#IPointerUpHandler),[](#)

|public method|description|
|--|--|
| CheckWarning() |public string CheckWarning()<br/>检测警告信息。 |
| OnBeginDrag() |public virtual void OnBeginDrag(PointerEventData eventData)|
| OnDrag() |public virtual void OnDrag(PointerEventData eventData)|
| OnEndDrag() |public virtual void OnEndDrag(PointerEventData eventData)|
| OnPointerClick() |public virtual void OnPointerClick(PointerEventData eventData)|
| OnPointerDown() |public virtual void OnPointerDown(PointerEventData eventData)|
| OnPointerEnter() |public virtual void OnPointerEnter(PointerEventData eventData)|
| OnPointerExit() |public virtual void OnPointerExit(PointerEventData eventData)|
| OnPointerUp() |public virtual void OnPointerUp(PointerEventData eventData)|
| OnScroll() |public virtual void OnScroll(PointerEventData eventData)|
| RebuildChartObject() |public void RebuildChartObject()<br/>移除并重新创建所有图表的Object。 |
| RefreshAllComponent() |public void RefreshAllComponent()|
| RefreshGraph() |public virtual void RefreshGraph()<br/>在下一帧刷新图形。 |
| ScreenPointToChartPoint() |public bool ScreenPointToChartPoint(Vector2 screenPoint, out Vector2 chartPoint)|
| SetPainterDirty() |public void SetPainterDirty()<br/>重新初始化Painter |
| SetSize() |public virtual void SetSize(float width, float height)<br/>设置图形的宽高（在非stretch pivot下才有效，其他情况需要自己调整RectTransform） |

## CandlestickChart

Inherits or Implemented: [BaseChart](#BaseChart)

## ChartCached

|public method|description|
|--|--|
| ColorToDotStr() |public static string ColorToDotStr(Color color)|
| ColorToStr() |public static string ColorToStr(Color color)|
| FloatToStr() |public static string FloatToStr(double value, string numericFormatter = "F", int precision = 0)|
| GetSerieLabelName() |public static string GetSerieLabelName(string prefix, int i, int j)|
| IntToStr() |public static string IntToStr(int value, string numericFormatter = "")|
| NumberToStr() |public static string NumberToStr(double value, string formatter)|

## ChartConst

## ChartDrawer

## ChartHelper

|public method|description|
|--|--|
| ActiveAllObject() |public static void ActiveAllObject(Transform parent, bool active, string match = null)|
| AddIcon() |public static Image AddIcon(string name, Transform parent, IconStyle iconStyle)|
| Cancat() |public static string Cancat(string str1, int i)|
| Cancat() |public static string Cancat(string str1, string str2)|
| ClearEventListener() |public static void ClearEventListener(GameObject obj)|
| CopyArray&lt;T&gt;() |public static bool CopyArray&lt;T&gt;(T[] toList, T[] fromList)|
| CopyList&lt;T&gt;() |public static bool CopyList&lt;T&gt;(List&lt;T&gt; toList, List&lt;T&gt; fromList)|
| DestoryGameObject() |public static void DestoryGameObject(GameObject go)|
| DestoryGameObject() |public static void DestoryGameObject(Transform parent, string childName)|
| DestoryGameObjectByMatch() |public static void DestoryGameObjectByMatch(Transform parent, string match)|
| DestroyAllChildren() |public static void DestroyAllChildren(Transform parent)|
| GetActualValue() |public static float GetActualValue(float valueOrRate, float total, float maxRate = 1.5f)|
| GetAngle360() |public static float GetAngle360(Vector2 from, Vector2 to)<br/>获得0-360的角度（12点钟方向为0度） |
| GetBlurColor() |public static Color32 GetBlurColor(Color32 color, float a = 0.3f)|
| GetColor() |public static Color32 GetColor(string hexColorStr)|
| GetDire() |public static Vector3 GetDire(float angle, bool isDegree = false)|
| GetFloatAccuracy() |public static int GetFloatAccuracy(double value)|
| GetFullName() |public static string GetFullName(Transform transform)|
| GetHighlightColor() |public static Color32 GetHighlightColor(Color32 color, float rate = 0.8f)|
| GetLastValue() |public static Vector3 GetLastValue(List&lt;Vector3&gt; list)|
| GetMaxDivisibleValue() |public static double GetMaxDivisibleValue(double max, double ceilRate)|
| GetMaxLogValue() |public static double GetMaxLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)|
| GetMinDivisibleValue() |public static double GetMinDivisibleValue(double min, double ceilRate)|
| GetMinLogValue() |public static double GetMinLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)|
| GetOrAddComponent&lt;T&gt;() |public static T GetOrAddComponent&lt;T&gt;(GameObject gameObject) where T : Component|
| GetOrAddComponent&lt;T&gt;() |public static T GetOrAddComponent&lt;T&gt;(Transform transform) where T : Component|
| GetPointList() |public static void GetPointList(ref List&lt;Vector3&gt; posList, Vector3 sp, Vector3 ep, float k = 30f)|
| GetPos() |public static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)|
| GetPosition() |public static Vector3 GetPosition(Vector3 center, float angle, float radius)|
| GetSelectColor() |public static Color32 GetSelectColor(Color32 color, float rate = 0.8f)|
| GetVertialDire() |public static Vector3 GetVertialDire(Vector3 dire)|
| HideAllObject() |public static void HideAllObject(GameObject obj, string match = null)|
| HideAllObject() |public static void HideAllObject(Transform parent, string match = null)|
| IsClearColor() |public static bool IsClearColor(Color color)|
| IsClearColor() |public static bool IsClearColor(Color32 color)|
| IsColorAlphaZero() |public static bool IsColorAlphaZero(Color color)|
| IsEquals() |public static bool IsEquals(double d1, double d2)|
| IsEquals() |public static bool IsEquals(float d1, float d2)|
| IsIngore() |public static bool IsIngore(Vector3 pos)|
| IsInRect() |public static bool IsInRect(Vector3 pos, float xMin, float xMax, float yMin, float yMax)|
| IsPointInQuadrilateral() |public static bool IsPointInQuadrilateral(Vector3 P, Vector3 A, Vector3 B, Vector3 C, Vector3 D)|
| IsValueEqualsColor() |public static bool IsValueEqualsColor(Color color1, Color color2)|
| IsValueEqualsColor() |public static bool IsValueEqualsColor(Color32 color1, Color32 color2)|
| IsValueEqualsList&lt;T&gt;() |public static bool IsValueEqualsList&lt;T&gt;(List&lt;T&gt; list1, List&lt;T&gt; list2)|
| IsValueEqualsString() |public static bool IsValueEqualsString(string str1, string str2)|
| IsValueEqualsVector2() |public static bool IsValueEqualsVector2(Vector2 v1, Vector2 v2)|
| IsValueEqualsVector3() |public static bool IsValueEqualsVector3(Vector3 v1, Vector3 v2)|
| IsZeroVector() |public static bool IsZeroVector(Vector3 pos)|
| ParseFloatFromString() |public static List&lt;float&gt; ParseFloatFromString(string jsonData)|
| ParseStringFromString() |public static List&lt;string&gt; ParseStringFromString(string jsonData)|
| RemoveComponent&lt;T&gt;() |public static void RemoveComponent&lt;T&gt;(GameObject gameObject)|
| RotateRound() |public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)|
| SaveAsImage() |public static Texture2D SaveAsImage(RectTransform rectTransform, Canvas canvas, string imageType = "png", string path = "")|
| SetActive() |public static void SetActive(GameObject gameObject, bool active)|
| SetActive() |public static void SetActive(Image image, bool active)|
| SetActive() |public static void SetActive(Text text, bool active)|
| SetActive() |public static void SetActive(Transform transform, bool active)<br/>通过设置scale实现是否显示，优化性能，减少GC |
| SetBackground() |public static void SetBackground(Image background, ImageStyle imageStyle)|
| SetColorOpacity() |public static void SetColorOpacity(ref Color32 color, float opacity)|

## ChartLabel

Inherits or Implemented: [Image](#Image)

|public method|description|
|--|--|
| GetHeight() |public float GetHeight()|
| GetPosition() |public Vector3 GetPosition()|
| GetTextHeight() |public float GetTextHeight()|
| GetTextWidth() |public float GetTextWidth()|
| GetWidth() |public float GetWidth()|
| SetActive() |public void SetActive(bool flag)|
| SetIcon() |public void SetIcon(Image image)|
| SetIconActive() |public void SetIconActive(bool flag)|
| SetIconSize() |public void SetIconSize(float width, float height)|
| SetIconSprite() |public void SetIconSprite(Sprite sprite)|
| SetPadding() |public void SetPadding(float[] padding)|
| SetPosition() |public void SetPosition(Vector3 position)|
| SetRectPosition() |public void SetRectPosition(Vector3 position)|
| SetSize() |public void SetSize(float width, float height)|
| SetText() |public bool SetText(string text)|
| SetTextActive() |public void SetTextActive(bool flag)|
| SetTextColor() |public void SetTextColor(Color color)|
| SetTextPadding() |public void SetTextPadding(TextPadding padding)|
| SetTextRotate() |public void SetTextRotate(float rotate)|
| UpdateIcon() |public void UpdateIcon(IconStyle iconStyle, Sprite sprite = null)|

## ChartObject

|public method|description|
|--|--|
| Destroy() |public virtual void Destroy()|

## CheckHelper

|public method|description|
|--|--|
| CheckChart() |public static string CheckChart(BaseChart chart)|
| CheckChart() |public static string CheckChart(BaseGraph chart)|

## ColorUtil

|public method|description|
|--|--|
| GetColor() |public static Color32 GetColor(string hexColorStr)<br/>将字符串颜色值转成Color。 |

## ComponentHandlerAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| ComponentHandlerAttribute() |public ComponentHandlerAttribute(Type handler)|
| ComponentHandlerAttribute() |public ComponentHandlerAttribute(Type handler, bool allowMultiple)|

## ComponentHelper

|public method|description|
|--|--|
| GetAngleAxis() |public static AngleAxis GetAngleAxis(List&lt;MainComponent&gt; components, int polarIndex)|
| GetRadiusAxis() |public static RadiusAxis GetRadiusAxis(List&lt;MainComponent&gt; components, int polarIndex)|
| GetXAxisOnZeroOffset() |public static float GetXAxisOnZeroOffset(List&lt;MainComponent&gt; components, XAxis axis)|
| GetYAxisOnZeroOffset() |public static float GetYAxisOnZeroOffset(List&lt;MainComponent&gt; components, YAxis axis)|
| IsAnyCategoryOfYAxis() |public static bool IsAnyCategoryOfYAxis(List&lt;MainComponent&gt; components)|

## CoordOptionsAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| Contains&lt;T&gt;() |public bool Contains&lt;T&gt;() where T : CoordSystem|
| CoordOptionsAttribute() |public CoordOptionsAttribute(Type coord)|
| CoordOptionsAttribute() |public CoordOptionsAttribute(Type coord, Type coord2)|
| CoordOptionsAttribute() |public CoordOptionsAttribute(Type coord, Type coord2, Type coord3)|
| CoordOptionsAttribute() |public CoordOptionsAttribute(Type coord, Type coord2, Type coord3, Type coord4)|

## DataZoomContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## DataZoomHelper

|public method|description|
|--|--|
| UpdateDataZoomRuntimeStartEndValue() |public static void UpdateDataZoomRuntimeStartEndValue(DataZoom dataZoom, Serie serie)|
| UpdateDataZoomRuntimeStartEndValue&lt;T&gt;() |public static void UpdateDataZoomRuntimeStartEndValue&lt;T&gt;(BaseChart chart) where T : Serie|

## DateTimeUtil

|public method|description|
|--|--|
| GetDateTime() |public static DateTime GetDateTime(int timestamp)|
| GetTimestamp() |public static int GetTimestamp()|
| GetTimestamp() |public static int GetTimestamp(DateTime time)|

## DefaultAnimationAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| DefaultAnimationAttribute() |public DefaultAnimationAttribute(AnimationType handler)|

## DefineSymbolsUtil

|public method|description|
|--|--|
| AddGlobalDefine() |public static void AddGlobalDefine(string symbol)|
| RemoveGlobalDefine() |public static void RemoveGlobalDefine(string symbol)|

## FormatterHelper

|public method|description|
|--|--|
| NeedFormat() |public static bool NeedFormat(string content)|
| ReplaceAxisLabelContent() |public static void ReplaceAxisLabelContent(ref string content, string numericFormatter, double value)|
| ReplaceAxisLabelContent() |public static void ReplaceAxisLabelContent(ref string content, string value)|
| TrimAndReplaceLine() |public static string TrimAndReplaceLine(string content)|
| TrimAndReplaceLine() |public static string TrimAndReplaceLine(StringBuilder sb)|

## GridCoordContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## HeatmapChart

Inherits or Implemented: [BaseChart](#BaseChart)

## IgnoreDoc

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| IgnoreDoc() |public IgnoreDoc()|

## InteractData

|public method|description|
|--|--|
| Reset() |public void Reset()|
| SetColor() |public void SetColor(ref bool needInteract, Color32 color)|
| SetColor() |public void SetColor(ref bool needInteract, Color32 color, Color32 toColor)|
| SetValue() |public void SetValue(ref bool needInteract, float size)|
| SetValue() |public void SetValue(ref bool needInteract, float size, bool highlight, float rate = 1.3f)|
| SetValueAndColor() |public void SetValueAndColor(ref bool needInteract, float value, Color32 color)|
| SetValueAndColor() |public void SetValueAndColor(ref bool needInteract, float value, Color32 color, Color32 toColor)|
| TryGetColor() |public bool TryGetColor(ref Color32 color, ref bool interacting, float animationDuration = 250)|
| TryGetColor() |public bool TryGetColor(ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)|
| TryGetValue() |public bool TryGetValue(ref float value, ref bool interacting, float animationDuration = 250)|
| TryGetValueAndColor() |public bool TryGetValueAndColor(ref float value, ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)|

## LayerHelper

|public method|description|
|--|--|
| IsFixedWidthHeight() |public static bool IsFixedWidthHeight(RectTransform rt)|
| IsStretchPivot() |public static bool IsStretchPivot(RectTransform rt)|

## LegendContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## LegendHelper

|public method|description|
|--|--|
| CheckDataHighlighted() |public static bool CheckDataHighlighted(Serie serie, string legendName, bool heighlight)|
| CheckDataShow() |public static bool CheckDataShow(Serie serie, string legendName, bool show)|
| GetContentColor() |public static Color GetContentColor(BaseChart chart, int legendIndex, string legendName, Legend legend, ThemeStyle theme, bool active)|
| GetIconColor() |public static Color GetIconColor(BaseChart chart, Legend legend, int readIndex, string legendName, bool active)|
| ResetItemPosition() |public static void ResetItemPosition(Legend legend, Vector3 chartPos, float chartWidth, float chartHeight)|
| SetLegendBackground() |public static void SetLegendBackground(Legend legend, ImageStyle style)|

## LegendItem

|public method|description|
|--|--|
| GetIconColor() |public Color GetIconColor()|
| GetIconRect() |public Rect GetIconRect()|
| SetActive() |public void SetActive(bool active)|
| SetBackground() |public void SetBackground(ImageStyle imageStyle)|
| SetButton() |public void SetButton(Button button)|
| SetContent() |public bool SetContent(string content)|
| SetContentBackgroundColor() |public void SetContentBackgroundColor(Color color)|
| SetContentColor() |public void SetContentColor(Color color)|
| SetContentPosition() |public void SetContentPosition(Vector3 offset)|
| SetIcon() |public void SetIcon(Image icon)|
| SetIconActive() |public void SetIconActive(bool active)|
| SetIconColor() |public void SetIconColor(Color color)|
| SetIconImage() |public void SetIconImage(Sprite image)|
| SetIconSize() |public void SetIconSize(float width, float height)|
| SetObject() |public void SetObject(GameObject obj)|
| SetPosition() |public void SetPosition(Vector3 position)|
| SetText() |public void SetText(ChartText text)|
| SetTextBackground() |public void SetTextBackground(Image image)|

## LineChart

Inherits or Implemented: [BaseChart](#BaseChart)

## ListFor

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| ListFor() |public ListFor(Type type)|

## ListForComponent

Inherits or Implemented: [ListFor](#ListFor)

|public method|description|
|--|--|
| ListForComponent() |public ListForComponent(Type type) : base(type)|

## ListForSerie

Inherits or Implemented: [ListFor](#ListFor)

|public method|description|
|--|--|
| ListForSerie() |public ListForSerie(Type type) : base(type)|

## MainComponentContext

## MainComponentHandler

|public method|description|
|--|--|
| CheckComponent() |public virtual void CheckComponent(StringBuilder sb) { }|
| DrawBase() |public virtual void DrawBase(VertexHelper vh) { }|
| DrawTop() |public virtual void DrawTop(VertexHelper vh) { }|
| DrawUpper() |public virtual void DrawUpper(VertexHelper vh) { }|
| InitComponent() |public virtual void InitComponent() { }|
| OnBeginDrag() |public virtual void OnBeginDrag(PointerEventData eventData) { }|
| OnDrag() |public virtual void OnDrag(PointerEventData eventData) { }|
| OnEndDrag() |public virtual void OnEndDrag(PointerEventData eventData) { }|
| OnPointerClick() |public virtual void OnPointerClick(PointerEventData eventData) { }|
| OnPointerDown() |public virtual void OnPointerDown(PointerEventData eventData) { }|
| OnPointerEnter() |public virtual void OnPointerEnter(PointerEventData eventData) { }|
| OnPointerExit() |public virtual void OnPointerExit(PointerEventData eventData) { }|
| OnPointerUp() |public virtual void OnPointerUp(PointerEventData eventData) { }|
| OnScroll() |public virtual void OnScroll(PointerEventData eventData) { }|
| OnSerieDataUpdate() |public virtual void OnSerieDataUpdate(int serieIndex) { }|
| RemoveComponent() |public virtual void RemoveComponent() { }|
| Update() |public virtual void Update() { }|

## MainComponentHandler&lt;T&gt;

Inherits or Implemented: [MainComponentHandler](#MainComponentHandler)

## MathUtil

|public method|description|
|--|--|
| Abs() |public static double Abs(double d)|
| Approximately() |public static bool Approximately(double a, double b)|
| Clamp() |public static double Clamp(double d, double min, double max)|
| Clamp01() |public static double Clamp01(double value)|
| Lerp() |public static double Lerp(double a, double b, double t)|

## ObjectPool&lt;T&gt; where T

Inherits or Implemented: [new()](#new())

|public method|description|
|--|--|
| ClearAll() |public void ClearAll()|
| Get() |public T Get()|
| new() |public class ObjectPool&lt;T&gt; where T : new()|
| ObjectPool() |public ObjectPool(UnityAction&lt;T&gt; actionOnGet, UnityAction&lt;T&gt; actionOnRelease, bool newIfEmpty = true)|
| Release() |public void Release(T element)|

## Painter

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic)

|public method|description|
|--|--|
| Init() |public void Init()|
| Refresh() |public void Refresh()|
| SetActive() |public void SetActive(bool flag, bool isDebugMode = false)|

## ParallelChart

Inherits or Implemented: [BaseChart](#BaseChart)

## ParallelCoordContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## PieChart

Inherits or Implemented: [BaseChart](#BaseChart)

## PolarChart

Inherits or Implemented: [BaseChart](#BaseChart)

## PolarCoordContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## ProgressBar

Inherits or Implemented: [BaseChart](#BaseChart)

## PropertyUtil

|public method|description|
|--|--|
| SetClass&lt;T&gt;() |public static bool SetClass&lt;T&gt;(ref T currentValue, T newValue, bool notNull = false) where T : class|
| SetColor() |public static bool SetColor(ref Color currentValue, Color newValue)|
| SetColor() |public static bool SetColor(ref Color32 currentValue, Color32 newValue)|
| SetStruct&lt;T&gt;() |public static bool SetStruct&lt;T&gt;(ref T currentValue, T newValue) where T : struct|

## RadarChart

Inherits or Implemented: [BaseChart](#BaseChart)

## RadarCoordContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## ReflectionUtil

|public method|description|
|--|--|
| DeepCloneSerializeField() |public static object DeepCloneSerializeField(object obj)|
| InvokeListAdd() |public static void InvokeListAdd(object obj, FieldInfo field, object item)|
| InvokeListAddTo&lt;T&gt;() |public static void InvokeListAddTo&lt;T&gt;(object obj, FieldInfo field, Action&lt;T&gt; callback)|
| InvokeListClear() |public static void InvokeListClear(object obj, FieldInfo field)|
| InvokeListCount() |public static int InvokeListCount(object obj, FieldInfo field)|
| InvokeListGet&lt;T&gt;() |public static T InvokeListGet&lt;T&gt;(object obj, FieldInfo field, int i)|

## RequireChartComponentAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| RequireChartComponentAttribute() |public RequireChartComponentAttribute(Type requiredComponent)|
| RequireChartComponentAttribute() |public RequireChartComponentAttribute(Type requiredComponent, Type requiredComponent2)|
| RequireChartComponentAttribute() |public RequireChartComponentAttribute(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)|

## RingChart

Inherits or Implemented: [BaseChart](#BaseChart)

## RuntimeUtil

|public method|description|
|--|--|
| GetAllAssemblyTypes() |public static IEnumerable&lt;Type&gt; GetAllAssemblyTypes()|
| GetAllTypesDerivedFrom() |public static IEnumerable&lt;Type&gt; GetAllTypesDerivedFrom(Type type)|
| GetAllTypesDerivedFrom&lt;T&gt;() |public static IEnumerable&lt;Type&gt; GetAllTypesDerivedFrom&lt;T&gt;()|
| GetAttribute&lt;T&gt;() |public static T GetAttribute&lt;T&gt;(this MemberInfo type, bool check = true) where T : Attribute|
| GetAttribute&lt;T&gt;() |public static T GetAttribute&lt;T&gt;(this Type type, bool check = true) where T : Attribute|
| HasSubclass() |public static bool HasSubclass(Type type)|

## ScatterChart

Inherits or Implemented: [BaseChart](#BaseChart)

## SerieContext

## SerieConvertAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| Contains() |public bool Contains(Type type)|
| Contains&lt;T&gt;() |public bool Contains&lt;T&gt;() where T : Serie|
| SerieConvertAttribute() |public SerieConvertAttribute(Type serie)|
| SerieConvertAttribute() |public SerieConvertAttribute(Type serie, Type serie2)|
| SerieConvertAttribute() |public SerieConvertAttribute(Type serie, Type serie2, Type serie3)|
| SerieConvertAttribute() |public SerieConvertAttribute(Type serie, Type serie2, Type serie3, Type serie4)|

## SerieDataContext

|public method|description|
|--|--|
| Reset() |public void Reset()|

## SerieDataExtraComponentAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| Contains() |public bool Contains(Type type)|
| Contains&lt;T&gt;() |public bool Contains&lt;T&gt;() where T : ISerieExtraComponent|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute()|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1)|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1, Type type2)|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3)|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4)|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)|
| SerieDataExtraComponentAttribute() |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)|

## SerieDataExtraFieldAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| Contains() |public bool Contains(string field)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute()|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1, string field2)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1, string field2, string field3)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5, string field6)|
| SerieDataExtraFieldAttribute() |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5, string field6, string field7)|

## SerieExtraComponentAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| Contains() |public bool Contains(Type type)|
| Contains&lt;T&gt;() |public bool Contains&lt;T&gt;() where T : ISerieExtraComponent|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute()|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1)|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1, Type type2)|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3)|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4)|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)|
| SerieExtraComponentAttribute() |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)|

## SerieHandler

|public method|description|
|--|--|
| CheckComponent() |public virtual void CheckComponent(StringBuilder sb) { }|
| DrawBase() |public virtual void DrawBase(VertexHelper vh) { }|
| DrawSerie() |public virtual void DrawSerie(VertexHelper vh) { }|
| DrawTop() |public virtual void DrawTop(VertexHelper vh) { }|
| DrawUpper() |public virtual void DrawUpper(VertexHelper vh) { }|
| InitComponent() |public virtual void InitComponent() { }|
| OnBeginDrag() |public virtual void OnBeginDrag(PointerEventData eventData) { }|
| OnDrag() |public virtual void OnDrag(PointerEventData eventData) { }|
| OnEndDrag() |public virtual void OnEndDrag(PointerEventData eventData) { }|
| OnLegendButtonClick() |public virtual void OnLegendButtonClick(int index, string legendName, bool show) { }|
| OnLegendButtonEnter() |public virtual void OnLegendButtonEnter(int index, string legendName) { }|
| OnLegendButtonExit() |public virtual void OnLegendButtonExit(int index, string legendName) { }|
| OnPointerClick() |public virtual void OnPointerClick(PointerEventData eventData) { }|
| OnPointerDown() |public virtual void OnPointerDown(PointerEventData eventData) { }|
| OnPointerEnter() |public virtual void OnPointerEnter(PointerEventData eventData) { }|
| OnPointerExit() |public virtual void OnPointerExit(PointerEventData eventData) { }|
| OnPointerUp() |public virtual void OnPointerUp(PointerEventData eventData) { }|
| OnScroll() |public virtual void OnScroll(PointerEventData eventData) { }|
| RefreshLabelInternal() |public virtual void RefreshLabelInternal() { }|
| RefreshLabelNextFrame() |public virtual void RefreshLabelNextFrame() { }|
| RemoveComponent() |public virtual void RemoveComponent() { }|
| Update() |public virtual void Update() { }|

## SerieHandler&lt;T&gt;

Inherits or Implemented: [SerieHandler where T](#SerieHandler where T),[Serie](#Serie)

|public method|description|
|--|--|
| GetSerieDataAutoColor() |public virtual Color GetSerieDataAutoColor(SerieData serieData)|
| GetSerieDataLabelOffset() |public virtual Vector3 GetSerieDataLabelOffset(SerieData serieData, LabelStyle label)|
| GetSerieDataLabelPosition() |public virtual Vector3 GetSerieDataLabelPosition(SerieData serieData, LabelStyle label)|
| GetSerieDataTitlePosition() |public virtual Vector3 GetSerieDataTitlePosition(SerieData serieData, TitleStyle titleStyle)|
| InitComponent() |public override void InitComponent()|
| OnLegendButtonClick() |public override void OnLegendButtonClick(int index, string legendName, bool show)|
| OnLegendButtonEnter() |public override void OnLegendButtonEnter(int index, string legendName)|
| OnLegendButtonExit() |public override void OnLegendButtonExit(int index, string legendName)|
| RefreshEndLabelInternal() |public virtual void RefreshEndLabelInternal()|
| RefreshLabelInternal() |public override void RefreshLabelInternal()|
| RefreshLabelNextFrame() |public override void RefreshLabelNextFrame()|
| RemoveComponent() |public override void RemoveComponent()|
| Update() |public override void Update()|

## SerieHandlerAttribute

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| SerieHandlerAttribute() |public SerieHandlerAttribute(Type handler)|
| SerieHandlerAttribute() |public SerieHandlerAttribute(Type handler, bool allowMultiple)|

## SerieHelper

|public method|description|
|--|--|
| CloneSerie&lt;T&gt;() |public static T CloneSerie&lt;T&gt;(Serie serie) where T : Serie|
| CopySerie() |public static void CopySerie(Serie oldSerie, Serie newSerie)|
| GetAllMinMaxData() |public static void GetAllMinMaxData(Serie serie, double ceilRate = 0, DataZoom dataZoom = null)|
| GetAreaStyle() |public static AreaStyle GetAreaStyle(Serie serie, SerieData serieData)|
| GetAverageData() |public static double GetAverageData(Serie serie, int dimension = 1, DataZoom dataZoom = null)|
| GetBlurStyle() |public static BlurStyle GetBlurStyle(Serie serie, SerieData serieData)|
| GetEmphasisStyle() |public static EmphasisStyle GetEmphasisStyle(Serie serie, SerieData serieData)|
| GetItemColor() |public static Color32 GetItemColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, SerieState state = SerieState.Auto, bool opacity = true)|
| GetItemFormatter() |public static string GetItemFormatter(Serie serie, SerieData serieData, string defaultFormatter = null)|
| GetItemMarker() |public static string GetItemMarker(Serie serie, SerieData serieData, string defaultMarker = null)|
| GetItemStyle() |public static ItemStyle GetItemStyle(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)|
| GetLineColor() |public static Color32 GetLineColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, SerieState state = SerieState.Auto)|
| GetLineStyle() |public static LineStyle GetLineStyle(Serie serie, SerieData serieData)|
| GetMaxData() |public static double GetMaxData(Serie serie, int dimension = 1, DataZoom dataZoom = null)|
| GetMaxSerieData() |public static SerieData GetMaxSerieData(Serie serie, int dimension = 1, DataZoom dataZoom = null)|
| GetMedianData() |public static double GetMedianData(Serie serie, int dimension = 1, DataZoom dataZoom = null)|
| GetMinData() |public static double GetMinData(Serie serie, int dimension = 1, DataZoom dataZoom = null)|
| GetMinMaxData() |public static void GetMinMaxData(Serie serie, out double min, out double max, DataZoom dataZoom = null, int dimension = 0)<br/>获得系列所有数据的最大最小值。 |
| GetMinSerieData() |public static SerieData GetMinSerieData(Serie serie, int dimension = 1, DataZoom dataZoom = null)|
| GetNumericFormatter() |public static string GetNumericFormatter(Serie serie, SerieData serieData, string defaultFormatter = null)|
| GetSelectStyle() |public static SelectStyle GetSelectStyle(Serie serie, SerieData serieData)|
| GetSerieLabel() |public static LabelStyle GetSerieLabel(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)|
| GetSerieLabelLine() |public static LabelLine GetSerieLabelLine(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)|
| GetSerieState() |public static SerieState GetSerieState(Serie serie)|
| GetSerieState() |public static SerieState GetSerieState(Serie serie, SerieData serieData, bool defaultSerieState = false)|
| GetSerieState() |public static SerieState GetSerieState(SerieData serieData)|
| GetSerieSymbol() |public static SerieSymbol GetSerieSymbol(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)|
| GetStateStyle() |public static StateStyle GetStateStyle(Serie serie, SerieData serieData, SerieState state)|
| GetSysmbolSize() |public static float GetSysmbolSize(Serie serie, SerieData serieData, ThemeStyle theme, float defaultSize, SerieState state = SerieState.Auto)|
| GetTitleStyle() |public static TitleStyle GetTitleStyle(Serie serie, SerieData serieData)|
| IsAllZeroValue() |public static bool IsAllZeroValue(Serie serie, int dimension = 1)<br/>系列指定维数的数据是否全部为0。 |
| IsDownPoint() |public static bool IsDownPoint(Serie serie, int index)|
| UpdateCenter() |public static void UpdateCenter(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)<br/>更新运行时中心点和半径 |
| UpdateFilterData() |public static void UpdateFilterData(Serie serie, DataZoom dataZoom)<br/>根据dataZoom更新数据列表缓存 |
| UpdateMinMaxData() |public static void UpdateMinMaxData(Serie serie, int dimension, double ceilRate = 0, DataZoom dataZoom = null)<br/>获得指定维数的最大最小值 |
| UpdateRect() |public static void UpdateRect(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)|
| UpdateSerieRuntimeFilterData() |public static void UpdateSerieRuntimeFilterData(Serie serie, bool filterInvisible = true)|

## SerieLabelHelper

|public method|description|
|--|--|
| AvoidLabelOverlap() |public static void AvoidLabelOverlap(Serie serie, ComponentTheme theme)|
| CanShowLabel() |public static bool CanShowLabel(Serie serie, SerieData serieData, LabelStyle label, int dimesion)|
| GetLabelColor() |public static Color GetLabelColor(Serie serie, ThemeStyle theme, int index)|
| GetRealLabelPosition() |public static Vector3 GetRealLabelPosition(Serie serie, SerieData serieData, LabelStyle label, LabelLine labelLine)|
| SetGaugeLabelText() |public static void SetGaugeLabelText(Serie serie)|
| UpdatePieLabelPosition() |public static void UpdatePieLabelPosition(Serie serie, SerieData serieData)|

## SerieLabelPool

|public method|description|
|--|--|
| ClearAll() |public static void ClearAll()|
| Release() |public static void Release(GameObject element)|
| ReleaseAll() |public static void ReleaseAll(Transform parent)|

## SerieParams

## SeriesHelper

|public method|description|
|--|--|
| GetLastStackSerie() |public static Serie GetLastStackSerie(List&lt;Serie&gt; series, Serie serie)<br/>获得上一个同堆叠且显示的serie。 |
| GetLegalSerieNameList() |public static List&lt;string&gt; GetLegalSerieNameList(List&lt;Serie&gt; series)|
| GetMaxSerieDataCount() |public static int GetMaxSerieDataCount(List&lt;Serie&gt; series)|
| GetNameColor() |public static Color GetNameColor(BaseChart chart, int index, string name)|
| GetStackSeries() |public static void GetStackSeries(List&lt;Serie&gt; series, ref Dictionary&lt;int, List&lt;Serie&gt;&gt; stackSeries)<br/>获得堆叠系列列表 |
| IsAnyClipSerie() |public static bool IsAnyClipSerie(List&lt;Serie&gt; series)<br/>是否有需裁剪的serie。 |
| IsLegalLegendName() |public static bool IsLegalLegendName(string name)|
| IsPercentStack&lt;T&gt;() |public static bool IsPercentStack&lt;T&gt;(List&lt;Serie&gt; series) where T : Serie<br/>是否时百分比堆叠 |
| IsPercentStack&lt;T&gt;() |public static bool IsPercentStack&lt;T&gt;(List&lt;Serie&gt; series, string stackName) where T : Serie<br/>是否时百分比堆叠 |
| IsStack() |public static bool IsStack(List&lt;Serie&gt; series)<br/>是否由数据堆叠 |
| IsStack&lt;T&gt;() |public static bool IsStack&lt;T&gt;(List&lt;Serie&gt; series, string stackName) where T : Serie<br/>是否堆叠 |
| UpdateSerieNameList() |public static void UpdateSerieNameList(BaseChart chart, ref List&lt;string&gt; serieNameList)<br/>获得所有系列名，不包含空名字。 |
| UpdateStackDataList() |public static void UpdateStackDataList(List&lt;Serie&gt; series, Serie currSerie, DataZoom dataZoom, List&lt;List&lt;SerieData&gt;&gt; dataList)|

## SimplifiedBarChart

Inherits or Implemented: [BaseChart](#BaseChart)

## SimplifiedCandlestickChart

Inherits or Implemented: [BaseChart](#BaseChart)

## SimplifiedLineChart

Inherits or Implemented: [BaseChart](#BaseChart)

## Since

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| Since() |public Since(string version)|

## SVG

|public method|description|
|--|--|
| DrawPath() |public static void DrawPath(VertexHelper vh, string path)|
| DrawPath() |public static void DrawPath(VertexHelper vh, SVGPath path)|
| Test() |public static void Test(VertexHelper vh)|

## SVGImage

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic)

## SVGPath

|public method|description|
|--|--|
| AddSegment() |public void AddSegment(SVGPathSeg seg)|
| Draw() |public void Draw(VertexHelper vh)|
| Parse() |public static SVGPath Parse(string path)|

## SVGPathSeg

|public method|description|
|--|--|
| SVGPathSeg() |public SVGPathSeg(SVGPathSegType type)|

## TooltipContext

## TooltipData

## TooltipHelper

|public method|description|
|--|--|
| GetItemNumericFormatter() |public static string GetItemNumericFormatter(Tooltip tooltip, Serie serie, SerieData serieData)|
| GetLineColor() |public static Color32 GetLineColor(Tooltip tooltip, ThemeStyle theme)|
| IsIgnoreFormatter() |public static bool IsIgnoreFormatter(string itemFormatter)|
| LimitInRect() |public static void LimitInRect(Tooltip tooltip, Rect chartRect)|

## TooltipView

|public method|description|
|--|--|
| CreateView() |public static TooltipView CreateView(Tooltip tooltip, ThemeStyle theme, Transform parent)|
| GetCurrentPos() |public Vector3 GetCurrentPos()|
| GetTargetPos() |public Vector3 GetTargetPos()|
| Refresh() |public void Refresh()|
| SetActive() |public void SetActive(bool flag)|
| Update() |public void Update()|
| UpdatePosition() |public void UpdatePosition(Vector3 pos)|

## TooltipViewItem

## UGL

|public method|description|
|--|--|
| DrawDiamond() |public static void DrawDiamond(VertexHelper vh, Vector3 center, float size, Color32 color)<br/>Draw a diamond. 画菱形（钻石形状） |
| DrawDiamond() |public static void DrawDiamond(VertexHelper vh, Vector3 center, float size, Color32 color, Color32 toColor)<br/>Draw a diamond. 画菱形（钻石形状） |
| DrawEllipse() |public static void DrawEllipse(VertexHelper vh, Vector3 center, float w, float h, Color32 color, float smoothness = 1)|
| DrawLine() |public static void DrawLine(VertexHelper vh, List&lt;Vector3&gt; points, float width, Color32 color, bool smooth, bool closepath = false)|
| DrawLine() |public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width, Color32 color)<br/>Draw a line. 画直线 |
| DrawLine() |public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width, Color32 color, Color32 toColor)<br/>Draw a line. 画直线 |
| DrawPolygon() |public static void DrawPolygon(VertexHelper vh, List&lt;Vector3&gt; points, Color32 color)<br/>填充任意多边形（目前只支持凸多边形） |
| DrawRectangle() |public static void DrawRectangle(VertexHelper vh, Rect rect, Color32 color)|
| DrawRectangle() |public static void DrawRectangle(VertexHelper vh, Rect rect, Color32 color, Color32 toColor)|
| DrawRectangle() |public static void DrawRectangle(VertexHelper vh, Rect rect, float border, Color32 color)|
| DrawRectangle() |public static void DrawRectangle(VertexHelper vh, Rect rect, float border, Color32 color, Color32 toColor)|
| DrawRectangle() |public static void DrawRectangle(VertexHelper vh, Vector3 p1, Vector3 p2, float radius, Color32 color)<br/>Draw a rectangle. 画带长方形 |
| DrawSquare() |public static void DrawSquare(VertexHelper vh, Vector3 center, float radius, Color32 color)<br/>Draw a square. 画正方形 |
| DrawSvgPath() |public static void DrawSvgPath(VertexHelper vh, string path)|
| DrawTriangle() |public static void DrawTriangle(VertexHelper vh, Vector3 pos, float size, Color32 color)|
| DrawTriangle() |public static void DrawTriangle(VertexHelper vh, Vector3 pos, float size, Color32 color, Color32 toColor)|

## UGLExample

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic)

## UGLHelper

|public method|description|
|--|--|
| GetAngle360() |public static float GetAngle360(Vector2 from, Vector2 to)<br/>获得0-360的角度（12点钟方向为0度） |
| GetBezier() |public static Vector3 GetBezier(float t, Vector3 sp, Vector3 cp, Vector3 ep)|
| GetBezier2() |public static Vector3 GetBezier2(float t, Vector3 sp, Vector3 p1, Vector3 p2, Vector3 ep)|
| GetBezierList() |public static List&lt;Vector3&gt; GetBezierList(Vector3 sp, Vector3 ep, int segment, Vector3 cp)|
| GetDire() |public static Vector3 GetDire(float angle, bool isDegree = false)|
| GetIntersection() |public static bool GetIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, ref Vector3 intersection)<br/>获得两直线的交点 |
| GetPos() |public static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)|
| GetVertialDire() |public static Vector3 GetVertialDire(Vector3 dire)|
| IsClearColor() |public static bool IsClearColor(Color color)|
| IsClearColor() |public static bool IsClearColor(Color32 color)|
| IsPointInPolygon() |public static bool IsPointInPolygon(Vector3 p, List&lt;Vector2&gt; polyons)|
| IsPointInPolygon() |public static bool IsPointInPolygon(Vector3 p, List&lt;Vector3&gt; polyons)|
| IsPointInTriangle() |public static bool IsPointInTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 check)|
| IsValueEqualsColor() |public static bool IsValueEqualsColor(Color color1, Color color2)|
| IsValueEqualsColor() |public static bool IsValueEqualsColor(Color32 color1, Color32 color2)|
| IsValueEqualsList&lt;T&gt;() |public static bool IsValueEqualsList&lt;T&gt;(List&lt;T&gt; list1, List&lt;T&gt; list2)|
| IsValueEqualsString() |public static bool IsValueEqualsString(string str1, string str2)|
| IsValueEqualsVector2() |public static bool IsValueEqualsVector2(Vector2 v1, Vector2 v2)|
| IsValueEqualsVector3() |public static bool IsValueEqualsVector3(Vector3 v1, Vector2 v2)|
| IsValueEqualsVector3() |public static bool IsValueEqualsVector3(Vector3 v1, Vector3 v2)|
| IsZeroVector() |public static bool IsZeroVector(Vector3 pos)|
| RotateRound() |public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)|

## VisualMapContext

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## VisualMapHelper

|public method|description|
|--|--|
| AutoSetLineMinMax() |public static void AutoSetLineMinMax(VisualMap visualMap, Serie serie, bool isY, Axis axis, Axis relativedAxis)|
| GetDimension() |public static int GetDimension(VisualMap visualMap, int defaultDimension)|
| IsNeedAreaGradient() |public static bool IsNeedAreaGradient(VisualMap visualMap)|
| IsNeedGradient() |public static bool IsNeedGradient(VisualMap visualMap)|
| IsNeedLineGradient() |public static bool IsNeedLineGradient(VisualMap visualMap)|
| SetMinMax() |public static void SetMinMax(VisualMap visualMap, double min, double max)|

## XChartsMgr

|public method|description|
|--|--|
| AddChart() |public static void AddChart(BaseChart chart)|
| ContainsChart() |public static bool ContainsChart(BaseChart chart)|
| ContainsChart() |public static bool ContainsChart(string chartName)|
| DisableTextMeshPro() |public static void DisableTextMeshPro()|
| EnableTextMeshPro() |public static void EnableTextMeshPro()|
| GetChart() |public static BaseChart GetChart(string chartName)|
| GetCharts() |public static List&lt;BaseChart&gt; GetCharts(string chartName)|
| GetPackageFullPath() |public static string GetPackageFullPath()|
| GetRepeatChartNameInfo() |public static string GetRepeatChartNameInfo(BaseChart chart, string chartName)|
| IsExistTMPAssembly() |public static bool IsExistTMPAssembly()|
| IsRepeatChartName() |public static bool IsRepeatChartName(BaseChart chart, string chartName = null)|
| ModifyTMPRefence() |public static bool ModifyTMPRefence(bool removeTMP = false)|
| RemoveAllChartObject() |public static void RemoveAllChartObject()|
| RemoveChart() |public static void RemoveChart(string chartName)|

## XCResourceImporterWindow

Inherits or Implemented: [UnityEditor.EditorWindow](#UnityEditor.EditorWindow)

|public method|description|
|--|--|
| ShowPackageImporterWindow() |public static void ShowPackageImporterWindow()|

## XCThemeMgr

|public method|description|
|--|--|
| AddTheme() |public static void AddTheme(Theme theme)|
| CheckReloadTheme() |public static void CheckReloadTheme()|
| ContainsTheme() |public static bool ContainsTheme(string themeName)|
| ExportTheme() |public static bool ExportTheme(Theme theme)|
| ExportTheme() |public static bool ExportTheme(Theme theme, string themeNewName)|
| GetAllThemeNames() |public static List&lt;string&gt; GetAllThemeNames()|
| GetTheme() |public static Theme GetTheme(string themeName)|
| GetTheme() |public static Theme GetTheme(ThemeType type)|
| GetThemeAssetPath() |public static string GetThemeAssetPath(string themeName)|
| GetThemeList() |public static List&lt;Theme&gt; GetThemeList()|
| LoadTheme() |public static Theme LoadTheme(string themeName)|
| LoadTheme() |public static Theme LoadTheme(ThemeType type)|
| ReloadThemeList() |public static void ReloadThemeList()<br/>重新加载主题列表 |
| SwitchTheme() |public static void SwitchTheme(BaseChart chart, string themeName)|

[XCharts主页](https://github.com/XCharts-Team/XCharts)<br/>
[XCharts配置项手册](XChartsConfiguration-ZH.md)<br/>
[XCharts问答](XChartsFAQ-ZH.md)
