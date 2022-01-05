# XCharts API

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts配置项手册](XCharts配置项手册.md)  
[XCharts问答](XCharts问答.md)

## `BaseChart`

* `BaseChart.theme`：主题组件`ThemeInfo`。
* `BaseChart.title`：标题组件`Title`。
* `BaseChart.legend`：图例组件`Legend`。
* `BaseChart.tooltip`：提示框组件`Tooltip`。
* `BaseChart.series`：系列列表`Series`。
* `BaseChart.chartName`：图表的别称。
* `BaseChart.chartWidth`：图表的宽。
* `BaseChart.chartHeight`：图表的高。
* `BaseChart.forceOpenRaycastTarget`：强制开启鼠标事件检测。一般不用手动设置，内部会自动判断是否需要检测。
* `BaseChart.onCustomDraw`：自定义底部绘制回调。在绘制Serie前调用。
* `BaseChart.onCustomDrawBeforeSerie`：自定义Serie绘制回调。在每个Serie绘制完前调用。
* `BaseChart.onCustomDrawAfterSerie`：自定义Serie绘制回调。在每个Serie绘制完后调用。
* `BaseChart.onCustomDrawTop`：自定义顶部绘制回调。在绘制Tooltip前调用。
* `BaseChart.onPointerClick`：鼠标点击回调。
* `BaseChart.onPointerDown`：鼠标按下回调。
* `BaseChart.onPointerUp`：鼠标弹起回调。
* `BaseChart.onPointerEnter`：鼠标进入图表回调。
* `BaseChart.onPointerExit`：鼠标退出图表回调。
* `BaseChart.onBeginDrag`：鼠标开始拖拽回调。
* `BaseChart.onDrag`：鼠标拖拽回调。
* `BaseChart.onEndDrag`：鼠标结束拖拽回调。
* `BaseChart.onScroll`：鼠标滚动回调。
* `BaseChart.onPointerClickPie`：点击柱条回调。参数：`eventData`, `serieIndex`, `dataIndex`
* `BaseChart.SetSize(float width, float height)`： 设置图表的宽高（在非stretch pivot下才有效，其他情况需要自己调整RectTransform）。
* `BaseChart.ClearData()`：清除所有数据，系列列表会保留，只是移除列表中系列的数据。
* `BaseChart.RemoveData()`：清除所有系列和图例数据，系列列表也会被清除。
* `BaseChart.RemoveData(string serieName)`：清除指定系列名称的数据。
* `BaseChart.AddSerie(SerieType type, string serieName = null, bool show = true)`：添加一个系列到系列列表中。
* `BaseChart.AddData(string serieName, float data, string dataName = null)`：添加一个数据到指定的系列中。
* `BaseChart.AddData(int serieIndex, float data, string dataName = null)`：添加一个数据到指定的系列中。
* `BaseChart.AddData(string serieName, List<float> multidimensionalData, string dataName = null)`：添加多维数据`（x,y,z...）`到指定的系列中。
* `BaseChart.AddData(int serieIndex, List<float> multidimensionalData, string dataName = null)`：添加多维数据`（x,y,z...）`到指定的系列中。
* `BaseChart.AddData(string serieName, float xValue, float yValue, string dataName)`：添加`（x,y）`数据到指定系列中。
* `BaseChart.AddData(int serieIndex, float xValue, float yValue, string dataName = null)`：添加`（x,y）`数据到指定系列中。
* `BaseChart.UpdateData(string serieName,int dataIndex, float value)`：更新指定系列中的指定索引数据。
* `BaseChart.UpdateData(int serieIndex,int dataIndex, float value)`：更新指定系列中的指定索引数据。
* `BaseChart.UpdateData(string serieName, int dataIndex, List<float> multidimensionalData)`：更新指定系列指定索引的数据项的多维数据。
* `BaseChart.UpdateData(int serieIndex, int dataIndex, List<float> multidimensionalData)`：更新指定系列指定索引的数据项的多维数据。
* `BaseChart.UpdateData(string serieName, int dataIndex, int dimension, float value)`：更新指定系列指定索引指定维数的数据。维数从0开始。
* `BaseChart.UpdateData(int serieIndex, int dataIndex, int dimension, float value)`：更新指定系列指定索引指定维数的数据。维数从0开始。
* `BaseChart.UpdateDataName(string serieName,int dataIndex, string dataName)`：更新指定系列中的指定索引数据名称。
* `BaseChart.UpdateDataName(int serieIndex, int dataIndex, string dataName)`：更新指定系列中的指定索引数据名称。
* `BaseChart.SetActive(string serieName, bool active)`：设置指定系列是否显示。
* `BaseChart.SetActive(int serieIndex, bool active)`：设置指定系列是否显示。
* `BaseChart.IsActive(string serieName)`：获取指定系列是否显示。
* `BaseChart.IsActive(int serieIndex)`：获取指定系列是否显示。
* `BaseChart.IsActiveByLegend(string legendName)`：获得指定图例名字的系列是否显示。
* `BaseChart.RefreshChart()`：在下一帧刷新图表。
* `BaseChart.RefreshLabel()`：在下一帧刷新文本标签。
* `BaseChart.RefreshTooltip()`：立即刷新`Tooltip`组件。
* `BaseChart.UpdateTheme(Theme theme)`：切换图表主题。
* `BaseChart.AnimationEnable(bool flag)`：启用或关闭动画。
* `BaseChart.AnimationFadeIn()`：渐入动画。
* `BaseChart.AnimationFadeOut()`：渐出动画。
* `BaseChart.AnimationPause()`：暂停动画。
* `BaseChart.AnimationResume()`：继续动画。
* `BaseChart.AnimationReset()`：重置动画。
* `BaseChart.ClickLegendButton(int legendIndex, string legendName, bool show)`：点击图例按钮。
* `BaseChart.IsInChart(Vector2 local)`：坐标是否在图表范围内。
* `BaseChart.IsInChart(float x, float y)`：坐标是否在图表范围内。
* `BaseChart.EnableBackground(bool flag)`：开启背景组件。背景组件在`chart`受上层布局控制时无法开启。
* `BaseChart.SetBasePainterMaterial(Material material)`：设置Base Painter的材质球。
* `BaseChart.SetSeriePainterMaterial(Material material)`：设置Serie Painter的材质球。
* `BaseChart.SetTopPainterMaterial(Material material)`：设置Top Painter的材质球。

## `CoordinateChart`

* `CoordinateChart.grid`：网格组件 `Grid`。
* `CoordinateChart.xAxes`：左右两个 `X` 轴组件 `XAxis`。
* `CoordinateChart.yAxes`：左右两个 `Y` 轴组件 `YAxis`。
* `CoordianteChart.xAxis0`：X轴（下）。
* `CoordianteChart.xAxis1`：X轴（上）。
* `CoordianteChart.xAxis0`：Y轴（左）。
* `CoordianteChart.yAxis1`：Y轴（右）。
* `CoordinateChart.dataZoom`：区域缩放组件 `DataZoom`。
* `CoordinateChart.ClearAxisData()`：清除所有x轴和y轴的类目数据。
* `CoordinateChart.AddXAxisData(string category, int xAxisIndex = 0)`：添加一个类目数据到指定的 `X` 轴。
* `CoordinateChart.AddYAxisData(string category, int yAxisIndex = 0)`：添加一个类目数据到指定的 `Y` 轴。
* `CoordinateChart.AddXAxisIcon(Sprite icon, int xAxisIndex = 0)`：添加一个图标到指定的 `X` 轴。
* `CoordinateChart.AddYAxisIcon(Sprite icon, int yAxisIndex = 0)`：添加一个图标到指定的 `Y` 轴。
* `CoordinateChart.UpdateXAxisData(int index, string category, int xAxisIndex = 0)`：更新 `X` 轴的类目数据。
* `CoordinateChart.UpdateYAxisData(int index, string category, int yAxisIndex = 0)`：更新 `Y` 轴的类目数据。
* `CoordinateChart.UpdateXAxisIcon(int index, Sprite icon, int xAxisIndex = 0)`：更新 `X` 轴的图标。
* `CoordinateChart.UpdateYAxisIcon(int index, Sprite icon, int yAxisIndex = 0)`：更新 `Y` 轴的图标。

* `CoordinateChart.IsValue()`：是否是纯数值坐标。
* `CoordinateChart.RefreshDataZoom()`：在下一帧刷新DataZoom组件。
* `CoordinateChart.RefreshAxisMinMaxValue()`：立即刷新数值坐标轴的最大最小值（更新坐标轴标签并触发重绘）。
* `CoordinateChart.IsInCooridate(Vector2 local)`：坐标是否在坐标轴内。
* `CoordinateChart.IsInCooridate(Vector3 local)`：坐标是否在坐标轴内。
* `CoordinateChart.IsInCooridate(float x, float y)`：坐标是否在坐标轴内。
* `CoordinateChart.IsInCooridate(Vector2 local)`：坐标是否在坐标轴内。
* `CoordinateChart.ClampInGrid(grid, Vector3 pos)`：将坐标限制在坐标系内。
* `CoordinateChart.CovertXYAxis(int index)`：转换X轴和Y轴的配置。
* `CoordinateChart.UpdateCoordinate()`：更新坐标系原点和宽高。一般内部会自动更新，也可强制更新。
* `CoordinateChart.SetMaxCache(int maxCache)`：设置可缓存的最大数据量。当数据量超过该值时，会自动删除第一个值再加入最新值。

## `LineChart`

* 继承 `BaseChart`。
* 继承自 `CoordinateChart`。

## `BarChart`

* 继承自 `BaseChart`。
* 继承自 `CoordinateChart`。

* `BarChart.onPointerClickBar`：点击柱条回调。参数：`eventData`, `dataIndex`


## `RadarChart`

* 继承自 `BaseChart`。
* `RadarChart.radars`：雷达坐标系组件列表 `Radar`。
* `RadarChart.RemoveRadar()`：移除所有雷达坐标系组件。
* `RadarChart.AddRadar(Radar radar)`：添加雷达坐标系组件。
* `RadarChart.AddRadar(Radar.Shape shape, Vector2 center, float radius, int splitNumber = 5,float lineWidth = 0.6f, bool showIndicator = true, bool showSplitArea = true)`：添加雷达坐标系组件。
* `RadarChart.AddIndicator(int radarIndex, string name, float min, float max)`：添加指示器。
* `RadarChart.UpdateIndicator(int radarIndex, int indicatorIndex, string name, float min, float max)`：更新指示器。
* `RadarChart.GetRadar(int radarIndex)`：获得指定索引的雷达坐标系组件。
* `RadarChart.GetIndicator(int radarIndex, int indicatorIndex)`：获得指定雷达坐标系组件指定索引的指示器。

## `ScatterChart`

* 继承自 `BaseChart`。
* 继承自 `CoordinateChart`。

## `HeatmapChart`

* 继承自 `BaseChart`。
* 继承自 `CoordinateChart`。

## `RingChart`

* 继承自 `BaseChart`。
* `RingChart.UpdateMax(int serieIndex, int dataIndex, float value)`：更新指定系列执行数据项的最大值。
* `RingChart.UpdateMax(int serieIndex, float value)`：更新指定系列的所有数据项的最大值。
* `RingChart.UpdateMax(float value)`：更新第一个系列第一个数据项的最大值。

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts配置项手册](XCharts配置项手册.md)  
[XCharts问答](XCharts问答.md)

