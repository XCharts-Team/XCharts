# API

[XCharts主页](https://github.com/XCharts-Team/XCharts)</br>
[XCharts配置项手册](XChartsConfiguration-ZH.md)</br>
[XCharts问答](XChartsFAQ-ZH.md)

## Class

- [AnimationStyleHelper](#AnimationStyleHelper)
- [AxisContext](#AxisContext)
- [AxisHandler<T>](#AxisHandler<T>)
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
- [MainComponentHandler<T>](#MainComponentHandler<T>)
- [MathUtil](#MathUtil)
- [ObjectPool<T> where T](#ObjectPool<T> where T)
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
- [SerieHandler<T>](#SerieHandler<T>)
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

## `AnimationStyleHelper`

|public method|description|
|--|--|
| `CheckDataAnimation()` |public static float CheckDataAnimation(BaseChart chart, Serie serie, int dataIndex, float destProgress, float startPorgress = 0)</br> |
| `GetAnimationPosition()` |public static bool GetAnimationPosition(AnimationStyle animation, bool isY, Vector3 lp, Vector3 cp, float progress, ref Vector3 ip)</br> |
| `UpdateAnimationType()` |public static void UpdateAnimationType(AnimationStyle animation, AnimationType defaultType)</br> |
| `UpdateSerieAnimation()` |public static void UpdateSerieAnimation(Serie serie)</br> |

## `AxisContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `AxisHandler<T>`

Inherits or Implemented: [MainComponentHandler](#MainComponentHandler)

## `AxisHelper`

|public method|description|
|--|--|
| `AdjustCircleLabelPos()` |public static void AdjustCircleLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)</br> |
| `AdjustMinMaxValue()` |public static void AdjustMinMaxValue(Axis axis, ref double minValue, ref double maxValue, bool needFormat, double ceilRate = 0)</br>调整最大最小值 |
| `AdjustRadiusAxisLabelPos()` |public static void AdjustRadiusAxisLabelPos(ChartLabel txt, Vector3 pos, Vector3 cenPos, float txtHig, Vector3 offset)</br> |
| `GetAxisLineArrowOffset()` |public static float GetAxisLineArrowOffset(Axis axis)</br>包含箭头偏移的轴线长度 |
| `GetAxisPosition()` |public static float GetAxisPosition(GridCoord grid, Axis axis, double value, int dataCount = 0, DataZoom dataZoom = null)</br> |
| `GetAxisPositionValue()` |public static double GetAxisPositionValue(float xy, float axisLength, double axisRange, float axisStart, float axisOffset)</br> |
| `GetAxisPositionValue()` |public static double GetAxisPositionValue(GridCoord grid, Axis axis, Vector3 pos)</br> |
| `GetAxisValueDistance()` |public static float GetAxisValueDistance(GridCoord grid, Axis axis, float scaleWidth, double value)</br>获得数值value在坐标轴上相对起点的距离 |
| `GetAxisValueLength()` |public static float GetAxisValueLength(GridCoord grid, Axis axis, float scaleWidth, double value)</br>获得数值value在坐标轴上对应的长度 |
| `GetAxisValuePosition()` |public static float GetAxisValuePosition(GridCoord grid, Axis axis, float scaleWidth, double value)</br>获得数值value在坐标轴上的坐标位置 |
| `GetDataWidth()` |public static float GetDataWidth(Axis axis, float coordinateWidth, int dataCount, DataZoom dataZoom)</br>获得一个类目数据在坐标系中代表的宽度 |
| `GetEachWidth()` |public static float GetEachWidth(Axis axis, float coordinateWidth, DataZoom dataZoom = null)</br> |
| `GetScaleNumber()` |public static int GetScaleNumber(Axis axis, float coordinateWidth, DataZoom dataZoom = null)</br>获得分割线条数 |
| `GetScaleWidth()` |public static float GetScaleWidth(Axis axis, float coordinateWidth, int index, DataZoom dataZoom = null)</br>获得分割段宽度 |
| `GetSplitNumber()` |public static int GetSplitNumber(Axis axis, float coordinateWid, DataZoom dataZoom)</br>获得分割段数 |
| `NeedShowSplit()` |public static bool NeedShowSplit(Axis axis)</br> |

## `BarChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `BaseChart`

Inherits or Implemented: [BaseGraph](#BaseGraph),[ISerializationCallbackReceiver](#ISerializationCallbackReceiver)

|public method|description|
|--|--|
| `AddChartComponent()` |public MainComponent AddChartComponent(Type type)</br> |
| `AddData()` |public SerieData AddData(int serieIndex, DateTime time, double yValue, string dataName = null, string dataId = null)</br>添加（time,y）数据到指定的系列中。 |
| `AddData()` |public SerieData AddData(int serieIndex, double data, string dataName = null, string dataId = null)</br>添加一个数据到指定的系列中。 |
| `AddData()` |public SerieData AddData(int serieIndex, double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)</br> |
| `AddData()` |public SerieData AddData(int serieIndex, double xValue, double yValue, string dataName = null, string dataId = null)</br>添加（x,y）数据到指定系列中。 |
| `AddData()` |public SerieData AddData(int serieIndex, List<double> multidimensionalData, string dataName = null, string dataId = null)</br>添加多维数据（x,y,z...）到指定的系列中。 |
| `AddData()` |public SerieData AddData(string serieName, DateTime time, double yValue, string dataName = null, string dataId = null)</br>添加（time,y）数据到指定的系列中。 |
| `AddData()` |public SerieData AddData(string serieName, double data, string dataName = null, string dataId = null)</br>If serieName doesn't exist in legend,will be add to legend. |
| `AddData()` |public SerieData AddData(string serieName, double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)</br> |
| `AddData()` |public SerieData AddData(string serieName, double xValue, double yValue, string dataName = null, string dataId = null)</br>添加（x,y）数据到指定系列中。 |
| `AddData()` |public SerieData AddData(string serieName, List<double> multidimensionalData, string dataName = null, string dataId = null)</br>添加多维数据（x,y,z...）到指定的系列中。 |
| `AddXAxisData()` |public void AddXAxisData(string category, int xAxisIndex = 0)</br>添加一个类目数据到指定的x轴。 |
| `AddXAxisIcon()` |public void AddXAxisIcon(Sprite icon, int xAxisIndex = 0)</br>添加一个图标到指定的x轴。 |
| `AddYAxisData()` |public void AddYAxisData(string category, int yAxisIndex = 0)</br>添加一个类目数据到指定的y轴。 |
| `AddYAxisIcon()` |public void AddYAxisIcon(Sprite icon, int yAxisIndex = 0)</br>添加一个图标到指定的y轴。 |
| `AnimationEnable()` |public void AnimationEnable(bool flag)</br>启用或关闭起始动画。 |
| `AnimationFadeIn()` |public void AnimationFadeIn()</br>开始渐入动画。 |
| `AnimationFadeOut()` |public void AnimationFadeOut()</br>开始渐出动画。 |
| `AnimationPause()` |public void AnimationPause()</br>暂停动画。 |
| `AnimationReset()` |public void AnimationReset()</br>重置动画。 |
| `AnimationResume()` |public void AnimationResume()</br>继续动画。 |
| `CanAddChartComponent()` |public bool CanAddChartComponent(Type type)</br> |
| `CanAddSerie()` |public bool CanAddSerie(Type type)</br> |
| `CanMultipleComponent()` |public bool CanMultipleComponent(Type type)</br> |
| `ClampInChart()` |public void ClampInChart(ref Vector3 pos)</br> |
| `ClampInGrid()` |public Vector3 ClampInGrid(GridCoord grid, Vector3 pos)</br> |
| `ClearData()` |public virtual void ClearData()</br>It just emptying all of serie's data without emptying the list of series. |
| `ClickLegendButton()` |public void ClickLegendButton(int legendIndex, string legendName, bool show)</br>点击图例按钮 |
| `CovertSerie()` |public bool CovertSerie(Serie serie, Type type)</br> |
| `CovertXYAxis()` |public void CovertXYAxis(int index)</br>转换X轴和Y轴的配置 |
| `GenerateDefaultSerieName()` |public string GenerateDefaultSerieName()</br> |
| `GetAllSerieDataCount()` |public int GetAllSerieDataCount()</br> |
| `GetChartBackgroundColor()` |public Color32 GetChartBackgroundColor()</br> |
| `GetChartComponentNum()` |public int GetChartComponentNum(Type type)</br> |
| `GetData()` |public double GetData(int serieIndex, int dataIndex, int dimension = 1)</br> |
| `GetData()` |public double GetData(string serieName, int dataIndex, int dimension = 1)</br> |
| `GetDataZoomOfAxis()` |public DataZoom GetDataZoomOfAxis(Axis axis)</br> |
| `GetDataZoomOfSerie()` |public void GetDataZoomOfSerie(Serie serie, out DataZoom xDataZoom, out DataZoom yDataZoom)</br> |
| `GetGrid()` |public GridCoord GetGrid(Vector2 local)</br> |
| `GetGridOfDataZoom()` |public GridCoord GetGridOfDataZoom(DataZoom dataZoom)</br> |
| `GetItemColor()` |public Color32 GetItemColor(Serie serie, bool highlight = false)</br> |
| `GetItemColor()` |public Color32 GetItemColor(Serie serie, SerieData serieData, bool highlight = false)</br> |
| `GetLegendRealShowNameColor()` |public Color32 GetLegendRealShowNameColor(string name)</br> |
| `GetLegendRealShowNameIndex()` |public int GetLegendRealShowNameIndex(string name)</br> |
| `GetPainter()` |public Painter GetPainter(int index)</br> |
| `GetSerie()` |public Serie GetSerie(int serieIndex)</br> |
| `GetSerie()` |public Serie GetSerie(string serieName)</br> |
| `GetSeriesMinMaxValue()` |public virtual void GetSeriesMinMaxValue(Axis axis, int axisIndex, out double tempMinValue, out double tempMaxValue)</br> |
| `GetTitlePosition()` |public Vector3 GetTitlePosition(Title title)</br> |
| `GetVisualMapOfSerie()` |public VisualMap GetVisualMapOfSerie(Serie serie)</br> |
| `GetXLerpColor()` |public Color32 GetXLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, GridCoord grid)</br> |
| `GetYLerpColor()` |public Color32 GetYLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, GridCoord grid)</br> |
| `HasChartComponent()` |public bool HasChartComponent(Type type)</br> |
| `HasChartComponent<T>()` |public bool HasChartComponent<T>()</br> |
| `HasSerie()` |public bool HasSerie(Type type)</br> |
| `Init()` |public void Init(bool defaultChart = true)</br> |
| `InitAxisRuntimeData()` |public virtual void InitAxisRuntimeData(Axis axis)</br> |
| `InsertSerie()` |public void InsertSerie(Serie serie, int index = -1, bool addToHead = false)</br> |
| `Internal_CheckAnimation()` |public void Internal_CheckAnimation()</br> |
| `IsActiveByLegend()` |public virtual bool IsActiveByLegend(string legendName)</br>获得指定图例名字的系列是否显示。 |
| `IsAllAxisCategory()` |public bool IsAllAxisCategory()</br>纯类目轴。 |
| `IsAllAxisValue()` |public bool IsAllAxisValue()</br>纯数值坐标轴（数值轴或对数轴）。 |
| `IsInAnyGrid()` |public bool IsInAnyGrid(Vector2 local)</br> |
| `IsInChart()` |public bool IsInChart(float x, float y)</br> |
| `IsInChart()` |public bool IsInChart(Vector2 local)</br>坐标是否在图表范围内 |
| `IsSerieName()` |public bool IsSerieName(string name)</br> |
| `MoveDownSerie()` |public bool MoveDownSerie(int serieIndex)</br> |
| `MoveUpSerie()` |public bool MoveUpSerie(int serieIndex)</br> |
| `OnAfterDeserialize()` |public void OnAfterDeserialize()</br> |
| `OnBeforeSerialize()` |public void OnBeforeSerialize()</br> |
| `OnBeginDrag()` |public override void OnBeginDrag(PointerEventData eventData)</br> |
| `OnDataZoomRangeChanged()` |public virtual void OnDataZoomRangeChanged(DataZoom dataZoom)</br> |
| `OnDrag()` |public override void OnDrag(PointerEventData eventData)</br> |
| `OnEndDrag()` |public override void OnEndDrag(PointerEventData eventData)</br> |
| `OnLegendButtonClick()` |public virtual void OnLegendButtonClick(int index, string legendName, bool show)</br> |
| `OnLegendButtonEnter()` |public virtual void OnLegendButtonEnter(int index, string legendName)</br> |
| `OnLegendButtonExit()` |public virtual void OnLegendButtonExit(int index, string legendName)</br> |
| `OnPointerClick()` |public override void OnPointerClick(PointerEventData eventData)</br> |
| `OnPointerDown()` |public override void OnPointerDown(PointerEventData eventData)</br> |
| `OnPointerEnter()` |public override void OnPointerEnter(PointerEventData eventData)</br> |
| `OnPointerExit()` |public override void OnPointerExit(PointerEventData eventData)</br> |
| `OnPointerUp()` |public override void OnPointerUp(PointerEventData eventData)</br> |
| `OnScroll()` |public override void OnScroll(PointerEventData eventData)</br> |
| `RefreshBasePainter()` |public void RefreshBasePainter()</br> |
| `RefreshChart()` |public void RefreshChart()</br>在下一帧刷新整个图表。 |
| `RefreshChart()` |public void RefreshChart(int serieIndex)</br>在下一帧刷新图表的指定serie。 |
| `RefreshChart()` |public void RefreshChart(Serie serie)</br>在下一帧刷新图表的指定serie。 |
| `RefreshDataZoom()` |public void RefreshDataZoom()</br>在下一帧刷新DataZoom |
| `RefreshPainter()` |public void RefreshPainter(int index)</br> |
| `RefreshPainter()` |public void RefreshPainter(Serie serie)</br> |
| `RefreshTopPainter()` |public void RefreshTopPainter()</br> |
| `RefreshUpperPainter()` |public void RefreshUpperPainter()</br> |
| `RemoveAllChartComponent()` |public void RemoveAllChartComponent()</br> |
| `RemoveChartComponent()` |public bool RemoveChartComponent(MainComponent component)</br> |
| `RemoveChartComponent()` |public bool RemoveChartComponent(Type type, int index = 0)</br> |
| `RemoveChartComponent<T>()` |public bool RemoveChartComponent<T>(int index = 0)</br> |
| `RemoveChartComponents()` |public int RemoveChartComponents(Type type)</br> |
| `RemoveChartComponents<T>()` |public int RemoveChartComponents<T>()</br> |
| `RemoveData()` |public virtual void RemoveData()</br>The series list is also cleared. |
| `RemoveData()` |public virtual void RemoveData(string serieName)</br>清除指定系列名称的数据。 |
| `RemoveSerie()` |public void RemoveSerie(int serieIndex)</br> |
| `RemoveSerie()` |public void RemoveSerie(Serie serie)</br> |
| `RemoveSerie()` |public void RemoveSerie(string serieName)</br> |
| `ReplaceSerie()` |public bool ReplaceSerie(Serie oldSerie, Serie newSerie)</br> |
| `SetBasePainterMaterial()` |public void SetBasePainterMaterial(Material material)</br>设置Base Painter的材质球 |
| `SetMaxCache()` |public void SetMaxCache(int maxCache)</br>设置可缓存的最大数据量。当数据量超过该值时，会自动删除第一个值再加入最新值。 |
| `SetPainterActive()` |public void SetPainterActive(int index, bool flag)</br> |
| `SetSerieActive()` |public void SetSerieActive(int serieIndex, bool active)</br>设置指定系列是否显示。 |
| `SetSerieActive()` |public void SetSerieActive(Serie serie, bool active)</br> |
| `SetSerieActive()` |public void SetSerieActive(string serieName, bool active)</br>设置指定系列是否显示。 |
| `SetSeriePainterMaterial()` |public void SetSeriePainterMaterial(Material material)</br>设置Serie Painter的材质球 |
| `SetTopPainterMaterial()` |public void SetTopPainterMaterial(Material material)</br>设置Top Painter的材质球 |
| `SetUpperPainterMaterial()` |public void SetUpperPainterMaterial(Material material)</br>设置Upper Painter的材质球 |
| `TryAddChartComponent()` |public bool TryAddChartComponent(Type type)</br> |
| `TryGetChartComponent<T>()` |public bool TryGetChartComponent<T>(out T component, int index = 0)</br> |
| `UdpateXAxisIcon()` |public void UdpateXAxisIcon(int index, Sprite icon, int xAxisIndex = 0)</br>更新X轴图标。 |
| `UpdateData()` |public bool UpdateData(int serieIndex, int dataIndex, double value)</br>更新指定系列中的指定索引数据。 |
| `UpdateData()` |public bool UpdateData(int serieIndex, int dataIndex, int dimension, double value)</br>更新指定系列指定索引指定维数的数据。维数从0开始。 |
| `UpdateData()` |public bool UpdateData(int serieIndex, int dataIndex, List<double> multidimensionalData)</br>更新指定系列指定索引的数据项的多维数据。 |
| `UpdateData()` |public bool UpdateData(string serieName, int dataIndex, double value)</br>更新指定系列中的指定索引数据。 |
| `UpdateData()` |public bool UpdateData(string serieName, int dataIndex, int dimension, double value)</br>更新指定系列指定索引指定维数的数据。维数从0开始。 |
| `UpdateData()` |public bool UpdateData(string serieName, int dataIndex, List<double> multidimensionalData)</br>更新指定系列指定索引的数据项的多维数据。 |
| `UpdateDataName()` |public bool UpdateDataName(int serieIndex, int dataIndex, string dataName)</br>更新指定系列中的指定索引数据名称。 |
| `UpdateDataName()` |public bool UpdateDataName(string serieName, int dataIndex, string dataName)</br>更新指定系列中的指定索引数据名称。 |
| `UpdateLegendColor()` |public virtual void UpdateLegendColor(string legendName, bool active)</br> |
| `UpdateTheme()` |public bool UpdateTheme(ThemeType theme)</br>切换内置主题。 |
| `UpdateTheme()` |public void UpdateTheme(Theme theme)</br>切换图表主题。 |
| `UpdateXAxisData()` |public void UpdateXAxisData(int index, string category, int xAxisIndex = 0)</br>更新X轴类目数据。 |
| `UpdateYAxisData()` |public void UpdateYAxisData(int index, string category, int yAxisIndex = 0)</br>更新Y轴类目数据。 |
| `UpdateYAxisIcon()` |public void UpdateYAxisIcon(int index, Sprite icon, int yAxisIndex = 0)</br>更新Y轴图标。 |

## `BaseGraph`

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic),[IPointerDownHandler](#IPointerDownHandler),[IPointerUpHandler](#IPointerUpHandler),[](#)

|public method|description|
|--|--|
| `CheckWarning()` |public string CheckWarning()</br>检测警告信息。 |
| `OnBeginDrag()` |public virtual void OnBeginDrag(PointerEventData eventData)</br> |
| `OnDrag()` |public virtual void OnDrag(PointerEventData eventData)</br> |
| `OnEndDrag()` |public virtual void OnEndDrag(PointerEventData eventData)</br> |
| `OnPointerClick()` |public virtual void OnPointerClick(PointerEventData eventData)</br> |
| `OnPointerDown()` |public virtual void OnPointerDown(PointerEventData eventData)</br> |
| `OnPointerEnter()` |public virtual void OnPointerEnter(PointerEventData eventData)</br> |
| `OnPointerExit()` |public virtual void OnPointerExit(PointerEventData eventData)</br> |
| `OnPointerUp()` |public virtual void OnPointerUp(PointerEventData eventData)</br> |
| `OnScroll()` |public virtual void OnScroll(PointerEventData eventData)</br> |
| `RebuildChartObject()` |public void RebuildChartObject()</br>移除并重新创建所有图表的Object。 |
| `RefreshAllComponent()` |public void RefreshAllComponent()</br> |
| `RefreshGraph()` |public void RefreshGraph()</br>在下一帧刷新图形。 |
| `ScreenPointToChartPoint()` |public bool ScreenPointToChartPoint(Vector2 screenPoint, out Vector2 chartPoint)</br> |
| `SetPainterDirty()` |public void SetPainterDirty()</br>重新初始化Painter |
| `SetSize()` |public virtual void SetSize(float width, float height)</br>设置图形的宽高（在非stretch pivot下才有效，其他情况需要自己调整RectTransform） |

## `CandlestickChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `ChartCached`

|public method|description|
|--|--|
| `ColorToDotStr()` |public static string ColorToDotStr(Color color)</br> |
| `ColorToStr()` |public static string ColorToStr(Color color)</br> |
| `FloatToStr()` |public static string FloatToStr(double value, string numericFormatter = "F", int precision = 0)</br> |
| `GetSerieLabelName()` |public static string GetSerieLabelName(string prefix, int i, int j)</br> |
| `IntToStr()` |public static string IntToStr(int value, string numericFormatter = "")</br> |
| `NumberToStr()` |public static string NumberToStr(double value, string formatter)</br> |

## `ChartConst`

## `ChartDrawer`

## `ChartHelper`

|public method|description|
|--|--|
| `ActiveAllObject()` |public static void ActiveAllObject(Transform parent, bool active, string match = null)</br> |
| `AddIcon()` |public static Image AddIcon(string name, Transform parent, IconStyle iconStyle)</br> |
| `Cancat()` |public static string Cancat(string str1, int i)</br> |
| `Cancat()` |public static string Cancat(string str1, string str2)</br> |
| `ClearEventListener()` |public static void ClearEventListener(GameObject obj)</br> |
| `CopyArray<T>()` |public static bool CopyArray<T>(T[] toList, T[] fromList)</br> |
| `CopyList<T>()` |public static bool CopyList<T>(List<T> toList, List<T> fromList)</br> |
| `DestoryGameObject()` |public static void DestoryGameObject(GameObject go)</br> |
| `DestoryGameObject()` |public static void DestoryGameObject(Transform parent, string childName)</br> |
| `DestoryGameObjectByMatch()` |public static void DestoryGameObjectByMatch(Transform parent, string match)</br> |
| `DestroyAllChildren()` |public static void DestroyAllChildren(Transform parent)</br> |
| `GetActualValue()` |public static float GetActualValue(float valueOrRate, float total, float maxRate = 1.5f)</br> |
| `GetAngle360()` |public static float GetAngle360(Vector2 from, Vector2 to)</br>获得0-360的角度（12点钟方向为0度） |
| `GetColor()` |public static Color32 GetColor(string hexColorStr)</br> |
| `GetDire()` |public static Vector3 GetDire(float angle, bool isDegree = false)</br> |
| `GetFloatAccuracy()` |public static int GetFloatAccuracy(double value)</br> |
| `GetFullName()` |public static string GetFullName(Transform transform)</br> |
| `GetHighlightColor()` |public static Color32 GetHighlightColor(Color32 color, float rate = 0.8f)</br> |
| `GetLastValue()` |public static Vector3 GetLastValue(List<Vector3> list)</br> |
| `GetMaxDivisibleValue()` |public static double GetMaxDivisibleValue(double max, double ceilRate)</br> |
| `GetMaxLogValue()` |public static double GetMaxLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)</br> |
| `GetMinDivisibleValue()` |public static double GetMinDivisibleValue(double min, double ceilRate)</br> |
| `GetMinLogValue()` |public static double GetMinLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)</br> |
| `GetPointList()` |public static void GetPointList(ref List<Vector3> posList, Vector3 sp, Vector3 ep, float k = 30f)</br> |
| `GetPos()` |public static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)</br> |
| `GetPosition()` |public static Vector3 GetPosition(Vector3 center, float angle, float radius)</br> |
| `GetVertialDire()` |public static Vector3 GetVertialDire(Vector3 dire)</br> |
| `HideAllObject()` |public static void HideAllObject(GameObject obj, string match = null)</br> |
| `HideAllObject()` |public static void HideAllObject(Transform parent, string match = null)</br> |
| `IsClearColor()` |public static bool IsClearColor(Color color)</br> |
| `IsClearColor()` |public static bool IsClearColor(Color32 color)</br> |
| `IsColorAlphaZero()` |public static bool IsColorAlphaZero(Color color)</br> |
| `IsEquals()` |public static bool IsEquals(double d1, double d2)</br> |
| `IsEquals()` |public static bool IsEquals(float d1, float d2)</br> |
| `IsIngore()` |public static bool IsIngore(Vector3 pos)</br> |
| `IsInRect()` |public static bool IsInRect(Vector3 pos, float xMin, float xMax, float yMin, float yMax)</br> |
| `IsPointInQuadrilateral()` |public static bool IsPointInQuadrilateral(Vector3 P, Vector3 A, Vector3 B, Vector3 C, Vector3 D)</br> |
| `IsValueEqualsColor()` |public static bool IsValueEqualsColor(Color color1, Color color2)</br> |
| `IsValueEqualsColor()` |public static bool IsValueEqualsColor(Color32 color1, Color32 color2)</br> |
| `IsValueEqualsList<T>()` |public static bool IsValueEqualsList<T>(List<T> list1, List<T> list2)</br> |
| `IsValueEqualsString()` |public static bool IsValueEqualsString(string str1, string str2)</br> |
| `IsValueEqualsVector2()` |public static bool IsValueEqualsVector2(Vector2 v1, Vector2 v2)</br> |
| `IsValueEqualsVector3()` |public static bool IsValueEqualsVector3(Vector3 v1, Vector3 v2)</br> |
| `IsZeroVector()` |public static bool IsZeroVector(Vector3 pos)</br> |
| `ParseFloatFromString()` |public static List<float> ParseFloatFromString(string jsonData)</br> |
| `ParseStringFromString()` |public static List<string> ParseStringFromString(string jsonData)</br> |
| `RemoveComponent<T>()` |public static void RemoveComponent<T>(GameObject gameObject)</br> |
| `RotateRound()` |public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)</br> |
| `SetActive()` |public static void SetActive(GameObject gameObject, bool active)</br> |
| `SetActive()` |public static void SetActive(Image image, bool active)</br> |
| `SetActive()` |public static void SetActive(Text text, bool active)</br> |
| `SetActive()` |public static void SetActive(Transform transform, bool active)</br>通过设置scale实现是否显示，优化性能，减少GC |
| `SetBackground()` |public static void SetBackground(Image background, ImageStyle imageStyle)</br> |
| `SetColorOpacity()` |public static void SetColorOpacity(ref Color32 color, float opacity)</br> |

## `ChartLabel`

Inherits or Implemented: [Image](#Image)

|public method|description|
|--|--|
| `GetHeight()` |public float GetHeight()</br> |
| `GetPosition()` |public Vector3 GetPosition()</br> |
| `GetTextHeight()` |public float GetTextHeight()</br> |
| `GetTextWidth()` |public float GetTextWidth()</br> |
| `GetWidth()` |public float GetWidth()</br> |
| `SetActive()` |public void SetActive(bool flag)</br> |
| `SetIcon()` |public void SetIcon(Image image)</br> |
| `SetIconActive()` |public void SetIconActive(bool flag)</br> |
| `SetIconSize()` |public void SetIconSize(float width, float height)</br> |
| `SetIconSprite()` |public void SetIconSprite(Sprite sprite)</br> |
| `SetPadding()` |public void SetPadding(float[] padding)</br> |
| `SetPosition()` |public void SetPosition(Vector3 position)</br> |
| `SetRectPosition()` |public void SetRectPosition(Vector3 position)</br> |
| `SetSize()` |public void SetSize(float width, float height)</br> |
| `SetText()` |public bool SetText(string text)</br> |
| `SetTextActive()` |public void SetTextActive(bool flag)</br> |
| `SetTextColor()` |public void SetTextColor(Color color)</br> |
| `SetTextPadding()` |public void SetTextPadding(TextPadding padding)</br> |
| `SetTextRotate()` |public void SetTextRotate(float rotate)</br> |
| `UpdateIcon()` |public void UpdateIcon(IconStyle iconStyle, Sprite sprite = null)</br> |

## `ChartObject`

|public method|description|
|--|--|
| `Destroy()` |public virtual void Destroy()</br> |

## `CheckHelper`

|public method|description|
|--|--|
| `CheckChart()` |public static string CheckChart(BaseChart chart)</br> |
| `CheckChart()` |public static string CheckChart(BaseGraph chart)</br> |

## `ColorUtil`

|public method|description|
|--|--|
| `GetColor()` |public static Color32 GetColor(string hexColorStr)</br>将字符串颜色值转成Color。 |

## `ComponentHandlerAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `ComponentHandlerAttribute()` |public ComponentHandlerAttribute(Type handler)</br> |
| `ComponentHandlerAttribute()` |public ComponentHandlerAttribute(Type handler, bool allowMultiple)</br> |

## `ComponentHelper`

|public method|description|
|--|--|
| `GetAngleAxis()` |public static AngleAxis GetAngleAxis(List<MainComponent> components, int polarIndex)</br> |
| `GetRadiusAxis()` |public static RadiusAxis GetRadiusAxis(List<MainComponent> components, int polarIndex)</br> |
| `GetXAxisOnZeroOffset()` |public static float GetXAxisOnZeroOffset(List<MainComponent> components, XAxis axis)</br> |
| `GetYAxisOnZeroOffset()` |public static float GetYAxisOnZeroOffset(List<MainComponent> components, YAxis axis)</br> |
| `IsAnyCategoryOfYAxis()` |public static bool IsAnyCategoryOfYAxis(List<MainComponent> components)</br> |

## `CoordOptionsAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `CoordOptionsAttribute()` |public CoordOptionsAttribute(Type coord)</br> |
| `CoordOptionsAttribute()` |public CoordOptionsAttribute(Type coord, Type coord2)</br> |
| `CoordOptionsAttribute()` |public CoordOptionsAttribute(Type coord, Type coord2, Type coord3)</br> |
| `CoordOptionsAttribute()` |public CoordOptionsAttribute(Type coord, Type coord2, Type coord3, Type coord4)</br> |

## `DataZoomContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `DataZoomHelper`

|public method|description|
|--|--|
| `UpdateDataZoomRuntimeStartEndValue()` |public static void UpdateDataZoomRuntimeStartEndValue(DataZoom dataZoom, Serie serie)</br> |

## `DateTimeUtil`

|public method|description|
|--|--|
| `GetDateTime()` |public static DateTime GetDateTime(int timestamp)</br> |
| `GetTimestamp()` |public static int GetTimestamp()</br> |
| `GetTimestamp()` |public static int GetTimestamp(DateTime time)</br> |

## `DefaultAnimationAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `DefaultAnimationAttribute()` |public DefaultAnimationAttribute(AnimationType handler)</br> |

## `DefineSymbolsUtil`

|public method|description|
|--|--|
| `AddGlobalDefine()` |public static void AddGlobalDefine(string symbol)</br> |
| `RemoveGlobalDefine()` |public static void RemoveGlobalDefine(string symbol)</br> |

## `FormatterHelper`

|public method|description|
|--|--|
| `NeedFormat()` |public static bool NeedFormat(string content)</br> |
| `ReplaceAxisLabelContent()` |public static void ReplaceAxisLabelContent(ref string content, string numericFormatter, double value)</br> |
| `ReplaceAxisLabelContent()` |public static void ReplaceAxisLabelContent(ref string content, string value)</br> |
| `TrimAndReplaceLine()` |public static string TrimAndReplaceLine(string content)</br> |
| `TrimAndReplaceLine()` |public static string TrimAndReplaceLine(StringBuilder sb)</br> |

## `GridCoordContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `HeatmapChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `IgnoreDoc`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `IgnoreDoc()` |public IgnoreDoc()</br> |

## `InteractData`

|public method|description|
|--|--|
| `Reset()` |public void Reset()</br> |
| `SetColor()` |public void SetColor(ref bool needInteract, Color32 color)</br> |
| `SetColor()` |public void SetColor(ref bool needInteract, Color32 color, Color32 toColor)</br> |
| `SetValue()` |public void SetValue(ref bool needInteract, float size)</br> |
| `SetValue()` |public void SetValue(ref bool needInteract, float size, bool highlight, float rate = 1.3f)</br> |
| `SetValueAndColor()` |public void SetValueAndColor(ref bool needInteract, float value, Color32 color)</br> |
| `SetValueAndColor()` |public void SetValueAndColor(ref bool needInteract, float value, Color32 color, Color32 toColor)</br> |
| `TryGetColor()` |public bool TryGetColor(ref Color32 color, ref bool interacting, float animationDuration = 250)</br> |
| `TryGetColor()` |public bool TryGetColor(ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)</br> |
| `TryGetValue()` |public bool TryGetValue(ref float value, ref bool interacting, float animationDuration = 250)</br> |
| `TryGetValueAndColor()` |public bool TryGetValueAndColor(ref float value, ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)</br> |

## `LayerHelper`

|public method|description|
|--|--|
| `IsFixedWidthHeight()` |public static bool IsFixedWidthHeight(RectTransform rt)</br> |
| `IsStretchPivot()` |public static bool IsStretchPivot(RectTransform rt)</br> |

## `LegendContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `LegendHelper`

|public method|description|
|--|--|
| `CheckDataHighlighted()` |public static bool CheckDataHighlighted(Serie serie, string legendName, bool heighlight)</br> |
| `CheckDataShow()` |public static bool CheckDataShow(Serie serie, string legendName, bool show)</br> |
| `GetContentColor()` |public static Color GetContentColor(BaseChart chart, int legendIndex, string legendName, Legend legend, ThemeStyle theme, bool active)</br> |
| `GetIconColor()` |public static Color GetIconColor(BaseChart chart, Legend legend, int readIndex, string legendName, bool active)</br> |
| `ResetItemPosition()` |public static void ResetItemPosition(Legend legend, Vector3 chartPos, float chartWidth, float chartHeight)</br> |
| `SetLegendBackground()` |public static void SetLegendBackground(Legend legend, ImageStyle style)</br> |

## `LegendItem`

|public method|description|
|--|--|
| `GetIconColor()` |public Color GetIconColor()</br> |
| `GetIconRect()` |public Rect GetIconRect()</br> |
| `SetActive()` |public void SetActive(bool active)</br> |
| `SetBackground()` |public void SetBackground(ImageStyle imageStyle)</br> |
| `SetButton()` |public void SetButton(Button button)</br> |
| `SetContent()` |public bool SetContent(string content)</br> |
| `SetContentBackgroundColor()` |public void SetContentBackgroundColor(Color color)</br> |
| `SetContentColor()` |public void SetContentColor(Color color)</br> |
| `SetContentPosition()` |public void SetContentPosition(Vector3 offset)</br> |
| `SetIcon()` |public void SetIcon(Image icon)</br> |
| `SetIconActive()` |public void SetIconActive(bool active)</br> |
| `SetIconColor()` |public void SetIconColor(Color color)</br> |
| `SetIconImage()` |public void SetIconImage(Sprite image)</br> |
| `SetIconSize()` |public void SetIconSize(float width, float height)</br> |
| `SetObject()` |public void SetObject(GameObject obj)</br> |
| `SetPosition()` |public void SetPosition(Vector3 position)</br> |
| `SetText()` |public void SetText(ChartText text)</br> |
| `SetTextBackground()` |public void SetTextBackground(Image image)</br> |

## `LineChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `ListFor`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `ListFor()` |public ListFor(Type type)</br> |

## `ListForComponent`

Inherits or Implemented: [ListFor](#ListFor)

|public method|description|
|--|--|
| `ListForComponent()` |public ListForComponent(Type type) : base(type)</br> |

## `ListForSerie`

Inherits or Implemented: [ListFor](#ListFor)

|public method|description|
|--|--|
| `ListForSerie()` |public ListForSerie(Type type) : base(type)</br> |

## `MainComponentContext`

## `MainComponentHandler`

## `MainComponentHandler<T>`

Inherits or Implemented: [MainComponentHandler](#MainComponentHandler)

## `MathUtil`

|public method|description|
|--|--|
| `Abs()` |public static double Abs(double d)</br> |
| `Approximately()` |public static bool Approximately(double a, double b)</br> |
| `Clamp()` |public static double Clamp(double d, double min, double max)</br> |
| `Clamp01()` |public static double Clamp01(double value)</br> |
| `Lerp()` |public static double Lerp(double a, double b, double t)</br> |

## `ObjectPool<T> where T`

Inherits or Implemented: [new()](#new())

|public method|description|
|--|--|
| `ClearAll()` |public void ClearAll()</br> |
| `Get()` |public T Get()</br> |
| `new()` |public class ObjectPool<T> where T : new()</br> |
| `ObjectPool()` |public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease, bool newIfEmpty = true)</br> |
| `Release()` |public void Release(T element)</br> |

## `Painter`

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic)

|public method|description|
|--|--|
| `Init()` |public void Init()</br> |
| `Refresh()` |public void Refresh()</br> |
| `SetActive()` |public void SetActive(bool flag, bool isDebugMode = false)</br> |

## `ParallelChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `ParallelCoordContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `PieChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `PolarChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `PolarCoordContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `ProgressBar`

Inherits or Implemented: [BaseChart](#BaseChart)

## `PropertyUtil`

|public method|description|
|--|--|
| `SetColor()` |public static bool SetColor(ref Color currentValue, Color newValue)</br> |
| `SetColor()` |public static bool SetColor(ref Color32 currentValue, Color32 newValue)</br> |

## `RadarChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `RadarCoordContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `ReflectionUtil`

|public method|description|
|--|--|
| `DeepCloneSerializeField()` |public static object DeepCloneSerializeField(object obj)</br> |
| `InvokeListAdd()` |public static void InvokeListAdd(object obj, FieldInfo field, object item)</br> |
| `InvokeListAddTo<T>()` |public static void InvokeListAddTo<T>(object obj, FieldInfo field, Action<T> callback)</br> |
| `InvokeListClear()` |public static void InvokeListClear(object obj, FieldInfo field)</br> |
| `InvokeListCount()` |public static int InvokeListCount(object obj, FieldInfo field)</br> |
| `InvokeListGet<T>()` |public static T InvokeListGet<T>(object obj, FieldInfo field, int i)</br> |

## `RequireChartComponentAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `RequireChartComponentAttribute()` |public RequireChartComponentAttribute(Type requiredComponent)</br> |
| `RequireChartComponentAttribute()` |public RequireChartComponentAttribute(Type requiredComponent, Type requiredComponent2)</br> |
| `RequireChartComponentAttribute()` |public RequireChartComponentAttribute(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)</br> |

## `RingChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `RuntimeUtil`

|public method|description|
|--|--|
| `GetAllAssemblyTypes()` |public static IEnumerable<Type> GetAllAssemblyTypes()</br> |
| `GetAllTypesDerivedFrom()` |public static IEnumerable<Type> GetAllTypesDerivedFrom(Type type)</br> |
| `GetAllTypesDerivedFrom<T>()` |public static IEnumerable<Type> GetAllTypesDerivedFrom<T>()</br> |
| `HasSubclass()` |public static bool HasSubclass(Type type)</br> |

## `ScatterChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `SerieContext`

## `SerieConvertAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `Contains()` |public bool Contains(Type type)</br> |
| `SerieConvertAttribute()` |public SerieConvertAttribute(Type serie)</br> |
| `SerieConvertAttribute()` |public SerieConvertAttribute(Type serie, Type serie2)</br> |
| `SerieConvertAttribute()` |public SerieConvertAttribute(Type serie, Type serie2, Type serie3)</br> |
| `SerieConvertAttribute()` |public SerieConvertAttribute(Type serie, Type serie2, Type serie3, Type serie4)</br> |

## `SerieDataContext`

|public method|description|
|--|--|
| `Reset()` |public void Reset()</br> |

## `SerieDataExtraComponentAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `Contains()` |public bool Contains(Type type)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute()</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1, Type type2)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)</br> |
| `SerieDataExtraComponentAttribute()` |public SerieDataExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)</br> |

## `SerieDataExtraFieldAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `Contains()` |public bool Contains(string field)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute()</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1, string field2)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1, string field2, string field3)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5, string field6)</br> |
| `SerieDataExtraFieldAttribute()` |public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5, string field6, string field7)</br> |

## `SerieExtraComponentAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `Contains()` |public bool Contains(Type type)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute()</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1, Type type2)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)</br> |
| `SerieExtraComponentAttribute()` |public SerieExtraComponentAttribute(Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)</br> |

## `SerieHandler`

## `SerieHandler<T>`

Inherits or Implemented: [SerieHandler where T](#SerieHandler where T),[Serie](#Serie)

|public method|description|
|--|--|
| `GetSerieDataAutoColor()` |public virtual Color GetSerieDataAutoColor(SerieData serieData)</br> |
| `GetSerieDataLabelOffset()` |public virtual Vector3 GetSerieDataLabelOffset(SerieData serieData, LabelStyle label)</br> |
| `GetSerieDataLabelPosition()` |public virtual Vector3 GetSerieDataLabelPosition(SerieData serieData, LabelStyle label)</br> |
| `GetSerieDataTitlePosition()` |public virtual Vector3 GetSerieDataTitlePosition(SerieData serieData, TitleStyle titleStyle)</br> |
| `InitComponent()` |public override void InitComponent()</br> |
| `OnLegendButtonClick()` |public override void OnLegendButtonClick(int index, string legendName, bool show)</br> |
| `OnLegendButtonEnter()` |public override void OnLegendButtonEnter(int index, string legendName)</br> |
| `OnLegendButtonExit()` |public override void OnLegendButtonExit(int index, string legendName)</br> |
| `RefreshEndLabelInternal()` |public virtual void RefreshEndLabelInternal()</br> |
| `RefreshLabelInternal()` |public override void RefreshLabelInternal()</br> |
| `RefreshLabelNextFrame()` |public override void RefreshLabelNextFrame()</br> |
| `RemoveComponent()` |public override void RemoveComponent()</br> |
| `Update()` |public override void Update()</br> |

## `SerieHandlerAttribute`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `SerieHandlerAttribute()` |public SerieHandlerAttribute(Type handler)</br> |
| `SerieHandlerAttribute()` |public SerieHandlerAttribute(Type handler, bool allowMultiple)</br> |

## `SerieHelper`

|public method|description|
|--|--|
| `CopySerie()` |public static void CopySerie(Serie oldSerie, Serie newSerie)</br> |
| `GetAllMinMaxData()` |public static void GetAllMinMaxData(Serie serie, double ceilRate = 0, DataZoom dataZoom = null)</br> |
| `GetAreaColor()` |public static Color32 GetAreaColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight)</br> |
| `GetAreaStyle()` |public static AreaStyle GetAreaStyle(Serie serie, SerieData serieData)</br> |
| `GetAreaToColor()` |public static Color32 GetAreaToColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight)</br> |
| `GetAverageData()` |public static double GetAverageData(Serie serie, int dimension = 1, DataZoom dataZoom = null)</br> |
| `GetItemColor()` |public static Color32 GetItemColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight, bool opacity = true)</br> |
| `GetItemColor0()` |public static Color32 GetItemColor0(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight, Color32 defaultColor)</br> |
| `GetItemFormatter()` |public static string GetItemFormatter(Serie serie, SerieData serieData, string defaultFormatter = null)</br> |
| `GetItemMarker()` |public static string GetItemMarker(Serie serie, SerieData serieData, string defaultMarker = null)</br> |
| `GetItemStyle()` |public static ItemStyle GetItemStyle(Serie serie, SerieData serieData, bool highlight = false)</br> |
| `GetItemStyleEmphasis()` |public static ItemStyle GetItemStyleEmphasis(Serie serie, SerieData serieData)</br> |
| `GetItemToColor()` |public static Color32 GetItemToColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight, bool opacity = true)</br> |
| `GetLineColor()` |public static Color32 GetLineColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight)</br> |
| `GetLineStyle()` |public static LineStyle GetLineStyle(Serie serie, SerieData serieData)</br> |
| `GetMaxData()` |public static double GetMaxData(Serie serie, int dimension = 1, DataZoom dataZoom = null)</br> |
| `GetMaxSerieData()` |public static SerieData GetMaxSerieData(Serie serie, int dimension = 1, DataZoom dataZoom = null)</br> |
| `GetMedianData()` |public static double GetMedianData(Serie serie, int dimension = 1, DataZoom dataZoom = null)</br> |
| `GetMinData()` |public static double GetMinData(Serie serie, int dimension = 1, DataZoom dataZoom = null)</br> |
| `GetMinMaxData()` |public static void GetMinMaxData(Serie serie, out double min, out double max, DataZoom dataZoom = null, int dimension = 0)</br>获得系列所有数据的最大最小值。 |
| `GetMinSerieData()` |public static SerieData GetMinSerieData(Serie serie, int dimension = 1, DataZoom dataZoom = null)</br> |
| `GetNumericFormatter()` |public static string GetNumericFormatter(Serie serie, SerieData serieData, string defaultFormatter = null)</br> |
| `GetSerieEmphasisLabel()` |public static LabelStyle GetSerieEmphasisLabel(Serie serie, SerieData serieData)</br> |
| `GetSerieLabel()` |public static LabelStyle GetSerieLabel(Serie serie, SerieData serieData, bool highlight = false)</br> |
| `GetSerieLabelLine()` |public static LabelLine GetSerieLabelLine(Serie serie, SerieData serieData, bool highlight = false)</br> |
| `GetSerieSymbol()` |public static SerieSymbol GetSerieSymbol(Serie serie, SerieData serieData)</br> |
| `GetSymbolBorder()` |public static float GetSymbolBorder(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight)</br> |
| `GetSymbolBorder()` |public static float GetSymbolBorder(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight, float defaultWidth)</br> |
| `GetSymbolBorderColor()` |public static Color32 GetSymbolBorderColor(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight)</br> |
| `GetSymbolCornerRadius()` |public static float[] GetSymbolCornerRadius(Serie serie, SerieData serieData, bool highlight)</br> |
| `GetTitleStyle()` |public static TitleStyle GetTitleStyle(Serie serie, SerieData serieData)</br> |
| `IsAllZeroValue()` |public static bool IsAllZeroValue(Serie serie, int dimension = 1)</br>系列指定维数的数据是否全部为0。 |
| `IsDownPoint()` |public static bool IsDownPoint(Serie serie, int index)</br> |
| `UpdateCenter()` |public static void UpdateCenter(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)</br>更新运行时中心点和半径 |
| `UpdateFilterData()` |public static void UpdateFilterData(Serie serie, DataZoom dataZoom)</br>根据dataZoom更新数据列表缓存 |
| `UpdateMinMaxData()` |public static void UpdateMinMaxData(Serie serie, int dimension, double ceilRate = 0, DataZoom dataZoom = null)</br>获得指定维数的最大最小值 |
| `UpdateRect()` |public static void UpdateRect(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)</br> |
| `UpdateSerieRuntimeFilterData()` |public static void UpdateSerieRuntimeFilterData(Serie serie, bool filterInvisible = true)</br> |

## `SerieLabelHelper`

|public method|description|
|--|--|
| `AvoidLabelOverlap()` |public static void AvoidLabelOverlap(Serie serie, ComponentTheme theme)</br> |
| `CanShowLabel()` |public static bool CanShowLabel(Serie serie, SerieData serieData, LabelStyle label, int dimesion)</br> |
| `GetLabelColor()` |public static Color GetLabelColor(Serie serie, ThemeStyle theme, int index)</br> |
| `GetRealLabelPosition()` |public static Vector3 GetRealLabelPosition(Serie serie, SerieData serieData, LabelStyle label, LabelLine labelLine)</br> |
| `SetGaugeLabelText()` |public static void SetGaugeLabelText(Serie serie)</br> |
| `UpdatePieLabelPosition()` |public static void UpdatePieLabelPosition(Serie serie, SerieData serieData)</br> |

## `SerieLabelPool`

|public method|description|
|--|--|
| `ClearAll()` |public static void ClearAll()</br> |
| `Release()` |public static void Release(GameObject element)</br> |
| `ReleaseAll()` |public static void ReleaseAll(Transform parent)</br> |

## `SerieParams`

## `SeriesHelper`

|public method|description|
|--|--|
| `GetLastStackSerie()` |public static Serie GetLastStackSerie(List<Serie> series, Serie serie)</br>获得上一个同堆叠且显示的serie。 |
| `GetLegalSerieNameList()` |public static List<string> GetLegalSerieNameList(List<Serie> series)</br> |
| `GetMaxSerieDataCount()` |public static int GetMaxSerieDataCount(List<Serie> series)</br> |
| `GetNameColor()` |public static Color GetNameColor(BaseChart chart, int index, string name)</br> |
| `GetStackSeries()` |public static void GetStackSeries(List<Serie> series, ref Dictionary<int, List<Serie>> stackSeries)</br>获得堆叠系列列表 |
| `IsAnyClipSerie()` |public static bool IsAnyClipSerie(List<Serie> series)</br>是否有需裁剪的serie。 |
| `IsLegalLegendName()` |public static bool IsLegalLegendName(string name)</br> |
| `IsStack()` |public static bool IsStack(List<Serie> series)</br>是否由数据堆叠 |
| `UpdateSerieNameList()` |public static void UpdateSerieNameList(BaseChart chart, ref List<string> serieNameList)</br>获得所有系列名，不包含空名字。 |
| `UpdateStackDataList()` |public static void UpdateStackDataList(List<Serie> series, Serie currSerie, DataZoom dataZoom, List<List<SerieData>> dataList)</br> |

## `SimplifiedBarChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `SimplifiedCandlestickChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `SimplifiedLineChart`

Inherits or Implemented: [BaseChart](#BaseChart)

## `Since`

Inherits or Implemented: [Attribute](#Attribute)

|public method|description|
|--|--|
| `Since()` |public Since(string version)</br> |

## `SVG`

|public method|description|
|--|--|
| `DrawPath()` |public static void DrawPath(VertexHelper vh, string path)</br> |
| `DrawPath()` |public static void DrawPath(VertexHelper vh, SVGPath path)</br> |
| `Test()` |public static void Test(VertexHelper vh)</br> |

## `SVGImage`

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic)

## `SVGPath`

|public method|description|
|--|--|
| `AddSegment()` |public void AddSegment(SVGPathSeg seg)</br> |
| `Draw()` |public void Draw(VertexHelper vh)</br> |
| `Parse()` |public static SVGPath Parse(string path)</br> |

## `SVGPathSeg`

|public method|description|
|--|--|
| `SVGPathSeg()` |public SVGPathSeg(SVGPathSegType type)</br> |

## `TooltipContext`

## `TooltipData`

## `TooltipHelper`

|public method|description|
|--|--|
| `GetItemNumericFormatter()` |public static string GetItemNumericFormatter(Tooltip tooltip, Serie serie, SerieData serieData)</br> |
| `GetLineColor()` |public static Color32 GetLineColor(Tooltip tooltip, ThemeStyle theme)</br> |
| `IsIgnoreFormatter()` |public static bool IsIgnoreFormatter(string itemFormatter)</br> |
| `LimitInRect()` |public static void LimitInRect(Tooltip tooltip, Rect chartRect)</br> |

## `TooltipView`

|public method|description|
|--|--|
| `CreateView()` |public static TooltipView CreateView(Tooltip tooltip, ThemeStyle theme, Transform parent)</br> |
| `GetCurrentPos()` |public Vector3 GetCurrentPos()</br> |
| `GetTargetPos()` |public Vector3 GetTargetPos()</br> |
| `Refresh()` |public void Refresh()</br> |
| `SetActive()` |public void SetActive(bool flag)</br> |
| `Update()` |public void Update()</br> |
| `UpdatePosition()` |public void UpdatePosition(Vector3 pos)</br> |

## `TooltipViewItem`

## `UGL`

|public method|description|
|--|--|
| `DrawDiamond()` |public static void DrawDiamond(VertexHelper vh, Vector3 center, float size, Color32 color)</br>Draw a diamond. 画菱形（钻石形状） |
| `DrawDiamond()` |public static void DrawDiamond(VertexHelper vh, Vector3 center, float size, Color32 color, Color32 toColor)</br>Draw a diamond. 画菱形（钻石形状） |
| `DrawEllipse()` |public static void DrawEllipse(VertexHelper vh, Vector3 center, float w, float h, Color32 color, float smoothness = 1)</br> |
| `DrawLine()` |public static void DrawLine(VertexHelper vh, List<Vector3> points, float width, Color32 color, bool smooth, bool closepath = false)</br> |
| `DrawLine()` |public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width, Color32 color)</br>Draw a line. 画直线 |
| `DrawLine()` |public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width, Color32 color, Color32 toColor)</br>Draw a line. 画直线 |
| `DrawRectangle()` |public static void DrawRectangle(VertexHelper vh, Rect rect, Color32 color)</br> |
| `DrawRectangle()` |public static void DrawRectangle(VertexHelper vh, Rect rect, Color32 color, Color32 toColor)</br> |
| `DrawRectangle()` |public static void DrawRectangle(VertexHelper vh, Rect rect, float border, Color32 color)</br> |
| `DrawRectangle()` |public static void DrawRectangle(VertexHelper vh, Rect rect, float border, Color32 color, Color32 toColor)</br> |
| `DrawRectangle()` |public static void DrawRectangle(VertexHelper vh, Vector3 p1, Vector3 p2, float radius, Color32 color)</br>Draw a rectangle. 画带长方形 |
| `DrawSquare()` |public static void DrawSquare(VertexHelper vh, Vector3 center, float radius, Color32 color)</br>Draw a square. 画正方形 |
| `DrawSvgPath()` |public static void DrawSvgPath(VertexHelper vh, string path)</br> |
| `DrawTriangle()` |public static void DrawTriangle(VertexHelper vh, Vector3 pos, float size, Color32 color)</br> |
| `DrawTriangle()` |public static void DrawTriangle(VertexHelper vh, Vector3 pos, float size, Color32 color, Color32 toColor)</br> |

## `UGLExample`

Inherits or Implemented: [MaskableGraphic](#MaskableGraphic)

## `UGLHelper`

|public method|description|
|--|--|
| `GetAngle360()` |public static float GetAngle360(Vector2 from, Vector2 to)</br>获得0-360的角度（12点钟方向为0度） |
| `GetBezier()` |public static Vector3 GetBezier(float t, Vector3 sp, Vector3 cp, Vector3 ep)</br> |
| `GetBezier2()` |public static Vector3 GetBezier2(float t, Vector3 sp, Vector3 p1, Vector3 p2, Vector3 ep)</br> |
| `GetBezierList()` |public static List<Vector3> GetBezierList(Vector3 sp, Vector3 ep, int segment, Vector3 cp)</br> |
| `GetDire()` |public static Vector3 GetDire(float angle, bool isDegree = false)</br> |
| `GetIntersection()` |public static bool GetIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, ref Vector3 intersection)</br>获得两直线的交点 |
| `GetPos()` |public static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)</br> |
| `GetVertialDire()` |public static Vector3 GetVertialDire(Vector3 dire)</br> |
| `IsClearColor()` |public static bool IsClearColor(Color color)</br> |
| `IsClearColor()` |public static bool IsClearColor(Color32 color)</br> |
| `IsPointInPolygon()` |public static bool IsPointInPolygon(Vector3 p, List<Vector2> polyons)</br> |
| `IsPointInPolygon()` |public static bool IsPointInPolygon(Vector3 p, List<Vector3> polyons)</br> |
| `IsPointInTriangle()` |public static bool IsPointInTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 check)</br> |
| `IsValueEqualsColor()` |public static bool IsValueEqualsColor(Color color1, Color color2)</br> |
| `IsValueEqualsColor()` |public static bool IsValueEqualsColor(Color32 color1, Color32 color2)</br> |
| `IsValueEqualsList<T>()` |public static bool IsValueEqualsList<T>(List<T> list1, List<T> list2)</br> |
| `IsValueEqualsString()` |public static bool IsValueEqualsString(string str1, string str2)</br> |
| `IsValueEqualsVector2()` |public static bool IsValueEqualsVector2(Vector2 v1, Vector2 v2)</br> |
| `IsValueEqualsVector3()` |public static bool IsValueEqualsVector3(Vector3 v1, Vector2 v2)</br> |
| `IsValueEqualsVector3()` |public static bool IsValueEqualsVector3(Vector3 v1, Vector3 v2)</br> |
| `IsZeroVector()` |public static bool IsZeroVector(Vector3 pos)</br> |
| `RotateRound()` |public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)</br> |

## `VisualMapContext`

Inherits or Implemented: [MainComponentContext](#MainComponentContext)

## `VisualMapHelper`

|public method|description|
|--|--|
| `AutoSetLineMinMax()` |public static void AutoSetLineMinMax(VisualMap visualMap, Serie serie, bool isY, Axis axis, Axis relativedAxis)</br> |
| `GetDimension()` |public static int GetDimension(VisualMap visualMap, int serieDataCount)</br> |
| `IsNeedAreaGradient()` |public static bool IsNeedAreaGradient(VisualMap visualMap)</br> |
| `IsNeedGradient()` |public static bool IsNeedGradient(VisualMap visualMap)</br> |
| `IsNeedLineGradient()` |public static bool IsNeedLineGradient(VisualMap visualMap)</br> |
| `SetMinMax()` |public static void SetMinMax(VisualMap visualMap, double min, double max)</br> |

## `XChartsMgr`

|public method|description|
|--|--|
| `AddChart()` |public static void AddChart(BaseChart chart)</br> |
| `ContainsChart()` |public static bool ContainsChart(BaseChart chart)</br> |
| `ContainsChart()` |public static bool ContainsChart(string chartName)</br> |
| `DisableTextMeshPro()` |public static void DisableTextMeshPro()</br> |
| `EnableTextMeshPro()` |public static void EnableTextMeshPro()</br> |
| `GetChart()` |public static BaseChart GetChart(string chartName)</br> |
| `GetCharts()` |public static List<BaseChart> GetCharts(string chartName)</br> |
| `GetPackageFullPath()` |public static string GetPackageFullPath()</br> |
| `GetRepeatChartNameInfo()` |public static string GetRepeatChartNameInfo(BaseChart chart, string chartName)</br> |
| `IsExistTMPAssembly()` |public static bool IsExistTMPAssembly()</br> |
| `IsRepeatChartName()` |public static bool IsRepeatChartName(BaseChart chart, string chartName = null)</br> |
| `ModifyTMPRefence()` |public static bool ModifyTMPRefence(bool removeTMP = false)</br> |
| `RemoveAllChartObject()` |public static void RemoveAllChartObject()</br> |
| `RemoveChart()` |public static void RemoveChart(string chartName)</br> |

## `XCResourceImporterWindow`

Inherits or Implemented: [UnityEditor.EditorWindow](#UnityEditor.EditorWindow)

|public method|description|
|--|--|
| `ShowPackageImporterWindow()` |public static void ShowPackageImporterWindow()</br> |

## `XCThemeMgr`

|public method|description|
|--|--|
| `AddTheme()` |public static void AddTheme(Theme theme)</br> |
| `CheckReloadTheme()` |public static void CheckReloadTheme()</br> |
| `ContainsTheme()` |public static bool ContainsTheme(string themeName)</br> |
| `ExportTheme()` |public static bool ExportTheme(Theme theme)</br> |
| `ExportTheme()` |public static bool ExportTheme(Theme theme, string themeNewName)</br> |
| `GetAllThemeNames()` |public static List<string> GetAllThemeNames()</br> |
| `GetTheme()` |public static Theme GetTheme(string themeName)</br> |
| `GetTheme()` |public static Theme GetTheme(ThemeType type)</br> |
| `GetThemeAssetPath()` |public static string GetThemeAssetPath(string themeName)</br> |
| `GetThemeList()` |public static List<Theme> GetThemeList()</br> |
| `LoadTheme()` |public static Theme LoadTheme(string themeName)</br> |
| `LoadTheme()` |public static Theme LoadTheme(ThemeType type)</br> |
| `ReloadThemeList()` |public static void ReloadThemeList()</br>重新加载主题列表 |
| `SwitchTheme()` |public static void SwitchTheme(BaseChart chart, string themeName)</br> |

[XCharts主页](https://github.com/XCharts-Team/XCharts)</br>
[XCharts配置项手册](XChartsConfiguration-ZH.md)</br>
[XCharts问答](XChartsFAQ-ZH.md)
