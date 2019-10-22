# XCharts API

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts配置项手册](XCharts配置项手册.md)  
[XCharts问答](XCharts问答.md)

## `BaseChart`

---

* `BaseChart.title`：标题组件`Title`。
* `BaseChart.legend`：图例组件`Legend`。
* `BaseChart.tooltip`：提示框组件`Tooltip`。
* `BaseChart.series`：系列列表`Series`。
* `BaseChart.chartWidth`：图表的宽。
* `BaseChart.chartHeight`：图表的高。
* `BaseChart.customDrawCallback`：自定义绘制回调函数。
* `BaseChart.SetSize(float width, float height)`：设置图表的大小。
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
* `BaseChart.UpdateDataName(string serieName,int dataIndex, string dataName)`：更新指定系列中的指定索引数据名称。
* `BaseChart.UpdateDataName(int serieIndex, int dataIndex, string dataName)`：更新指定系列中的指定索引数据名称。
* `BaseChart.SetActive(string serieName, bool active)`：设置指定系列是否显示。
* `BaseChart.SetActive(int serieIndex, bool active)`：设置指定系列是否显示。
* `BaseChart.IsActive(string serieName)`：获取指定系列是否显示。
* `BaseChart.IsActive(int serieIndex)`：获取指定系列是否显示。
* `BaseChart.IsActiveByLegend(string legendName)`：获得指定图例名字的系列是否显示。
* `BaseChart.RefreshChart()`：在下一帧刷新图表。
* `BaseChart.RefreshLabel()`：在下一帧刷新文本标签。
* `BaseChart.UpdateTheme(Theme theme)`：切换图表主题。
* `BaseChart.AnimationEnable(bool flag)`：启用或关闭起始动画。
* `BaseChart.AnimationStart()`：开始初始动画。
* `BaseChart.AnimationStop()`：停止初始化动画。
* `BaseChart.AnimationReset()`：重置初始动画，重新播放。
* `BaseChart.ClickLegendButton(int legendIndex, string legendName, bool show)`：点击图例按钮。

## `CoordinateChart`

---

* `CoordinateChart.grid`：网格组件 `Grid`。
* `CoordinateChart.xAxises`：左右两个 `X` 轴组件 `XAxis`。
* `CoordinateChart.yAxises`：左右两个 `Y` 轴组件 `YAxis`。
* `CoordianteChart.xAxis0`：X轴（下）。
* `CoordianteChart.xAxis1`：X轴（上）。
* `CoordianteChart.xAxis0`：Y轴（左）。
* `CoordianteChart.yAxis1`：Y轴（右）。
* `CoordinateChart.dataZoom`：区域缩放组件 `DataZoom`。
* `CoordinateChart.coordinateX`：坐标系的左下角坐标 `X`。
* `CoordinateChart.coordinateY`：坐标系的左下角坐标 `Y`。
* `CoordinateChart.coordinateWidth`：坐标系的宽。
* `CoordinateChart.coordinateHeight`：坐标系的高。
* `CoordinateChart.ClearAxisData()`：清除所有x轴和y轴的类目数据。
* `CoordinateChart.AddXAxisData(string category, int xAxisIndex = 0)`：添加一个类目数据到指定的 `X` 轴。
* `CoordinateChart.AddYAxisData(string category, int yAxisIndex = 0)`：添加一个类目数据到指定的 `Y` 轴。
* `CoordinateChart.IsValue()`：是否是纯数值坐标。
* `CoordinateChart.RefreshDataZoom()`：在下一帧刷新DataZoom组件。
* `CoordinateChart.RefreshAxisMinMaxValue()`：立即刷新数值坐标轴的最大最小值（更新坐标轴标签并触发重绘）。

## `LineChart`

---

* 继承 `BaseChart`。
* 继承自 `CoordinateChart`。

## `BarChart`

---

* 继承自 `BaseChart`。
* 继承自 `CoordinateChart`。

## `PieChart`

---

* 继承自 `BaseChart`。

## `RadarChart`

---

* 继承自 `BaseChart`。
* `radars`：雷达组件列表 `Radar`。

## `ScatterChart`

---

* 继承自 `BaseChart`。
* 继承自 `CoordinateChart`。

## `HeatmapChart`

---

* 继承自 `BaseChart`。
* 继承自 `CoordinateChart`。

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts配置项手册](XCharts配置项手册.md)  
[XCharts问答](XCharts问答.md)
