# XCharts Configuration

[XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts API](xcharts-api-EN.md)  
[XCharts Q&A](xcharts-questions-and-answers-EN.md)

The translation work is still in progress.

__Main component:__

* [Axis](#XAxis)  
* [Background](#Background)  
* [DataZoom](#DataZoom)  
* [Grid](#Grid)  
* [Legend](#Legend)  
* [Polar](#Polar)  
* [Radar](#Radar)  
* [Series](#Series)  
* [Serie-Line](#Serie-Line)  
* [Serie-Bar](#Serie-Bar)  
* [Serie-Pie](#Serie-Pie)  
* [Serie-Radar](#Serie-Radar)  
* [Serie-Scatter](#Serie-Scatter)  
* [Serie-Heatmap](#Serie-Heatmap)  
* [Serie-Gauge](#Serie-Gauge)  
* [Serie-Ring](#Serie-Ring)  
* [Serie-Liquid](#Serie-Liquid)  
* [Serie-Candlestick](#Serie-Candlestick)  
* [Serie-Gantt](#Serie-Gantt)  
* [Settings](#Settings)
* [Theme](#Theme)  
* [Title](#Title)  
* [Tooltip](#Tooltip)  
* [Vessel](#Vessel)  
* [VisualMap](#VisualMap)  

__Sub component:__

* [AreaStyle](#AreaStyle)  
* [AxisLabel](#AxisLabel)  
* [AxisLine](#AxisLine)  
* [AxisName](#AxisName)  
* [AxisSplitLine](#AxisSplitLine)  
* [AxisSplitArea](#AxisSplitArea)  
* [AxisTick](#AxisTick)  
* [Emphasis](#Emphasis)  
* [ItemStyle](#ItemStyle)  
* [LineArrow](#LineArrow)  
* [LineStyle](#LineStyle)  
* [Location](#Location)  
* [MarkLine](#MarkLine) 
* [SerieAnimation](#SerieAnimation)  
* [SerieData](#SerieData)  
* [SerieLabel](#SerieLabel)  
* [SerieSymbol](#SerieSymbol)  
* [TextLimit](#TextLimit)  
* [TextStyle](#TextStyle)  
* [IconStyle](#IconStyle)  

## `Theme`

Theme components. Topics are used to configure other parameters such as the global color scheme for the chart.

Parameters:

* `theme`: Built-in theme types. There are `Default`, `Light`, `Dark` three optional built-in theme.
* `font`: A common font for all text.
* `backgroundColor`: Chart background color.
* `titleTextColor`: The text color of the main title.
* `titleSubTextColor`: The text color of the sub title.
* `legendTextColor`: Legend text color when actived.
* `legendUnableColor`: Legend text color when unactived.
* `axisTextColor`: The text color of axis label.
* `axisLineColor`: The color of axis line.
* `axisSplitLineColor`: The color of the dividing line of the coordinate axis is the same as the default color of the axis.
* `tooltipBackgroundColor`: The background color of the tooltip.
* `tooltipFlagAreaColor`: The color of the shadow indicator for the tooltip.
* `tooltipTextColor`: The text color of Tooltip.
* `tooltipLabelColor`: The cross indicator in the tooltip coordinates the background color of the label.
* `tooltipLineColor`: The color of the indicator line in the tooltip.
* `dataZoomTextColor`: The text color of dataZoom.
* `dataZoomLineColor`: The line color of dataZoom.
* `dataZoomSelectedColor`: The selected area color of dataZoom.
* `colorPalette`: Palette color list. The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.

API:

* `GetColor(int index)`: Gets the color of the specified index from the palette.
* `GetColorStr(int index)`: Gets the hexadecimal color string of the specified index from the palette.
* `GetColor(string hexColorStr)`: Convert the html string to color.

## `Title`

Title component, including main title and subtitle.

Parameters:

* `show`: Whether to show title component.Set this to false to prevent the title component from showing.
* `text`: The content of main title, supporting `\n` for newlines.
* `textStyle`: The text style of main title [TextStyle](#TextStyle).
* `subText`: The content of sub title, supporting `\n` for newlines.
* `subTextStyle`: The text style of sub title [TextStyle](#TextStyle).
* `itemGap`: The gap between the main title and sub title.
* `location`: The location of title component [Location](#Location).

## `TitleStyle`

Sub component for serie title.

* `show`: Whether to show serie title.
* `textStyle`: The text style of title [TextStyle](#TextStyle).

## `Legend`

Legend component.The legend component shows different sets of symbol, colors, and names. You can control which series are not displayed by clicking on the legend.

Parameters:

* `show`: Whether show legend component.
* `iconType`: the legend icon symbol type:
  * `Auto` : Auto match.
  * `Custom` : Custom icon.
  * `EmptyCircle` : hollow circle.
  * `Circle` : solid Circle.
  * `Rect` : square.
  * `Triangle` :
  * `Diamond` :
* `selectedMode`: Selected mode of legend, which controls whether series can be toggled displaying by clicking legends:
  * `Multiple`: multi-select.
  * `Single`: single select.
  * `None`: can’t select.
* `orient`: horizontal or vertical layout:
  * `Horizonal`: horizontal layout.
  * `Vertical`: vertical layout.
* `location`: the localtion of legend in chart [Location](#Location).
* `itemWidth`: the width of legend icon.
* `itemHeight`: the height of legend icon.
* `itemGap`: The distance between each legend, horizontal distance in horizontal layout, and vertical distance in vertical layout.
* `itemAutoColor`: Whether the legend symbol matches the color automatically.
* `formatter`: Legend content string template formatter. Support for wrapping lines with `\n`. Template:`{name}`.
* `data`: Data array of legend. An array item is usually a name representing string. (If it is a pie chart, it could also be the name of a single data in the pie chart) of a series. If data is not specified, it will be auto collected from series.
* `icons`: The list of cunstomize icons.
* `textStyle`: The style of text [TextStyle](#TextStyle).

API:

* `ClearData()`: Clear legend data.
* `ContainsData(string name)`: Whether include in legend data by the specified name.
* `RemoveData(string name)`: Remove legend from data.
* `AddData(string name)`: Add legend.
* `GetData(int index)`: Get legend.
* `GetIndex(string legendName)`: Get the index of legend.

## `Polar`

Polar coordinate can be used in scatter and line chart. Every polar coordinate has an angleAxis and a radiusAxis.

Parameters:

* `show`: Whether to show the polor component.
* `center`: The center of ploar. The `center[0]` is the x-coordinate, and the `center[1]` is the y-coordinate. When value between 0 and 1 represents a percentage  relative to the chart.
* `radius`: the radius of polar.
* `backgroundColor`: Background color of polar, which is transparent by default.

## `Radar`

Radar coordinate conponnet for radar charts.

* `shape`: Radar render type, in which `Polygon` and `Circle` are supported.
  * `Polygon`: Polygon.
  * `Circle`: Circle.
* `positionType`: The position type of radar indicator label display.
  * `Vertice`: Display at the vertex.
  * `Between`: Display at the middle of line.
* `radius`: The radius of radar.[default:0.3f].
* `center`: the center of radar chart. The `center[0]` is the x-coordinate, and the `center[1]` is the y-coordinate. When value between 0 and 1 represents a percentage  relative to the chart.[default:[0.5f,0.4f]].
* `ceilRate`: The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.[default:0].
* `splitNumber`: Segments of indicator axis.[default:5].
* `isAxisTooltip`: Tooltip displays all the data on the axis.[default:false].
* `outRangeColor`: The color displayed when data out of range.[default:red]
* `connectCenter`: Whether serie data connect to radar center with line.[default:false]
* `lineGradient`: Whether need gradient for data line..[default:true]
* `splitLine`: The split line style of radar [AxisSplitLine](#AxisSplitLine).
* `splitArea`: The split area style of radar [AxisSplitArea](#AxisSplitArea).
* `indicator`: Whether to show indicator.
* `indicatorGap`: The gap of indicator and radar.
* `indicatorList`: The indicator list [Radar.Indicator](#Radar.Indicator).

## `Radar.Indicator`

Indicator of radar chart, which is used to assign multiple variables(dimensions) in radar chart.

* `name`: The name of indicator.
* `max`: The maximum value of indicator, with default value of 0, but we recommend to set it manually.
* `min`: The minimum value of indicator, with default value of 0.
* `min`: Normal range. When the value is outside this range, the display color is automatically changed.
* `textStyle`: The text style of indicator [TextStyle](#TextStyle).

## `TextLimit`

Text character limitation and adaptation component. When the length of the text exceeds the set length, it is cropped and suffixes are appended to the end. Only valid in the category axis.

* `enable`: Whether to enable text limit. [default: `true`].
* `maxWidth`: Set the maximum width. A default of 0 indicates automatic fetch; otherwise, custom. Clipping occurs when the width of the text is greater than this value. [default: `0f`].
* `gap`: White pixel distance at both ends. [default: `10f`].
* `suffix`: Suffixes when the length exceeds. [default: `"..."`].

## `TextStyle`

The component of settings related to text.

* `rotate`: rotate of text. [default: `0f`].
* `offset`: offset of text position. [default: `Vector2.zero`].
* `color`: color of text. [default: `Color.clear`].
* `backgroundColor`: color of text background. [default: `Color.clear`].
* `font`: the font of text. When `null`, the theme's font is used by default. [default: `null`].
* `fontSize`: the size of text. [default: `18`].
* `fontStyle`: the font style of text. [default: `FontStyle.Normal`].
* `lineSpacing`: the space of text line.  [default: `1f`].
* `autoWrap`: Whether to wrap lines.
* `autoAlign`: Whether to let the system automatically set alignment. If true, the system automatically selects alignment, and if false, use alignment.

## `Tooltip`

Tooltip component.

* `show`: Whether to show the tooltip component.
* `type`: Indicator type. Indicator types are:
  * `Line`: line indicator.
  * `Shadow`: shadow crosshair indicator.
  * `None`: no indicator displayed.
  * `Corss`: crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.
* `formatter`: A string template formatter for the total content of the prompt box. Support for wrapping lines with `\n`. When formatter is not null, use formatter first, otherwise use itemFormatter.
  * Template variables are `{.}`, `{a}`, `{b}`, `{c}`, `{d}`.
  * `{.}` is the dot of the corresponding color of `serie` that is currently indicated or whose `index` is `0`.`
  * `{a}` is the `name` of the `serie` that is currently indicated or whose `index` is `0`.
  * `{b}` is the `name` of the `serieData` that is currently indicated or whose `index` is `0`, or a `category` value (such as the X-axis of a line chart).
  * `{c}` is the value of a Y-dimension (`dimesion` is 1) from the `serie` that is currently indicated or whose `index` is `0`.
  * `{d}` is the percentage value of Y-dimensions (`dimesion` is 1) from the `serie` that is currently indicated or whose `index` is `0`, with no `%` sign.
  * `{e}` is the `name` of the `serieData` that is currently indicated or whose `index` is `0`.
  * `{.1}` represents a dot from serie corresponding color that specifies `index` as `1`.
  * `1` in `{a1}`, `{b1}`, `{c1}` represents a `serie` that specifies an `index` of `1`.
  * `{c1:2}` represents the third data from `serie`'s current indication data item indexed to `1` (a data item has multiple data, `index` 2 represents the third data).
  * `{c1:2-2}` represents the third data item from `serie`'s third data item indexed to `1` (i.e., which data item must be specified to specify).
  * `{d1:2: F2}` indicates that a formatted string with a value specified separately is `F2` (`numericFormatter` is used when not specified).
  * Example: `"{a}, {c}"`, `"{a1}, {c1: f1}"`, `"{a1}, {c1:0: f1}"`, `"{a1} : {c1:1-1: f1}"`
* `titleFormatter`: The string template formatter for the tooltip title content. Support for wrapping lines with `\n`. This is only valid if the `itemFormatter` is in effect. The placeholder `{I}` can be set separately to indicate that the title is ignored and not displayed.
* `itemFormatter`: a string template formatter for a single Serie or data item content. Support for wrapping lines with `\n`. When `formatter` is not null, use `formatter` first, otherwise use `itemFormatter`.
* `numericFormatter`: Standard numeric format string. Used to format numeric values to display as strings. Using `Axx` form: `A` is the single character of the format specifier, supporting `C` currency, `D` decimal, `E` exponent, `F` number of vertices, `G` regular, `N` digits, `P` percentage, `R` round tripping, `X` hex etc. `XX` is the precision specification, from `0` - `99`. see: <https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings>
* `fixedWidth`: Fixed width. Higher priority than `minWidth`.
* `fixedHeight`: Fixed height. Higher priority than `minHeight`.
* `minWidth`: Minimum width. If `fixedWidth` has a value, get `fixedWidth` first.
* `minHeight`: Minimum height. If `fixedHeight` has a value, get `fixedHeight` first.
* `paddingLeftRight`: the text padding of left and right. [defaut: `5f`].
* `paddingTopBottom`: the text padding of top and bottom. [defaut: `5f`].
* `backgroundImage`: The image of icon.
* `ignoreDataShow`: Whether to show ignored data on tooltip. [defaut: `false`].
* `ignoreDataDefaultContent`: The default display character information for ignored data.
* `alwayShow`: Whether to trigger after always display.
* `offset`: `(since v1.5.3)`The position offset of tooltip relative to the mouse position.
* `lineStyle`: the line style of indicator line [LineStyle](#LineStyle).
* `textStyle`: the text style of content [TextStyle](#TextStyle).

## `Vessel`

Vessel component for liquid chart. There can be multiple vessels in a Chart, which can be matched by vesselIndex in Serie.

* `show`: Whether to show the vessel. [defaut: `true`]
* `shape`: The shape of vessel. [default: `Shape.Circle`]
* `shapeWidth`: Thickness of vessel. [defaut: `5f`]
* `gap`: The gap between the vessel and the liquid. [defaut: `10f`]
* `center`: The center of vessel. The `center[0]` is the x-coordinate, and the `center[1]` is the y-coordinate. When value between `0` and `1` represents a percentage relative to the chart. [default: `[0.5f,0.45f]`]
* `radius`: The radius of vessel. When value between 0 and 1 represents a percentage relative to the chart. [default: `0.35f`]
* `smoothness`: The smoothness of wave. [default: `1f`]
* `backgroundColor`: Background color of polar, which is transparent by default. [default: `Color.clear`]
* `color`: Vessel color. The default is consistent with Serie. [default: `Color32(70, 70, 240, 255)`]
* `autoColor`: Whether automatic color. If true, the color matches serie. [default: `true`]
* `width`：The width of vessel. This value is valid when `shape` is `Rect`.
* `height`：The height of vessel. This value is valid when `shape` is `Rect`.
* `cornerRadius`： The radius of rounded corner. This value is valid when `shape` is `Rect`.

## `DataZoom`

DataZoom component is used for zooming a specific area, which enables user to investigate data in detail, or get an overview of the data, or get rid of outlier points.  
Currently only the control `X` axis is supported.

* `enable`: Whether to show dataZoom.
* `supportInside`: Whether built-in support is supported. Built into the coordinate system to allow the user to zoom in and out of the coordinate system by mouse dragging, mouse wheel, finger swiping (on the touch screen).
* `supportSlider`: Whether a slider is supported. There are separate sliders on which the user zooms or roams.
* ~~`filterMode`: The mode of data filter, not support yet.~~
  * ~~`Filter`: data that outside the window will be filtered, which may lead to some changes of windows of other axes. For each data item, it will be filtered if one of the relevant dimensions is out of the window.~~
  * ~~`WeakFilter`: data that outside the window will be filtered, which may lead to some changes of windows of other axes. For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.~~
  * ~~`Empty`: data that outside the window will be set to NaN, which will not lead to changes of windows of other axes.~~
  * ~~`None`: Do not filter data.~~
* ~~`xAxisIndex`: Specify which xAxis is controlled by the dataZoom.~~
* ~~`yAxisIndex`: Specify which yAxis is controlled by the dataZoom.~~
* `showDataShadow`: Whether to show data shadow, to indicate the data tendency in brief. [default: `true`]
* `showDetail`: Whether to show detail, that is, show the detailed data information when dragging. [default: `false`]
* `zoomLock`: Specify whether to lock the size of window (selected area). [default: `false`]
* ~~`realtime`: Whether to show data shadow in dataZoom-silder component, to indicate the data tendency in brief. [default: `true`]~~
* `backgroundColor`: The background color of the component.
* `selectedAreaColor`: The color of the selected area.
* `bottom`: Distance between dataZoom component and the bottom side of the container. [default: `10f`]
* `top`: Distance between dataZoom component and the top side of the container.  [default: `0`]
* `left`: Distance between dataZoom component and the left side of the container. [default: `0`]
* `right`: Distance between dataZoom component and the right side of the container. [default: `0`]
* `height`: The height of dataZoom component. height value is a instant pixel value like 10. [default: `50f`]
* `rangeMode`: Use absolute value or percent value in `DataZoom.start` and `DataZoom.end`. [default: `RangeMode.Percent`].
  * `Percent`: percent.
* `start`: The start percentage of the window out of the data extent, in the range of `0 ~ 100`. [default: `30f`]
* `end`: The end percentage of the window out of the data extent, in the range of 0 ~ 100. [default: `70f`]
* `scrollSensitivity`: The sensitivity of dataZoom scroll. The larger the number, the more sensitive it is. [default: `10f`]
* `textStyle`: style of datazoom label.
* `minShowNum`: Minimum number of display data. Minimum number of data displayed when DataZoom is enlarged to maximum. [default: `1`]

## `VisualMap`

VisualMap component. mapping data to visual elements such as colors.

* `enable`: Whether enable visualMap component. [default: false]
* `show`: Whether to display components. If set to false, it will not show up, but the data mapping function still exists. [default: true]
* `type`: the type of visualmap component.
  * `Continuous`: Continuous.
  * ~~`Piecewise`: Piecewise.~~
* ~~`selectedMode`: the selected mode for Piecewise visualMap.~~
  * ~~`Multiple`: Multiple.~~
  * ~~`Single`: Single.~~

* `autoMinMax`: Automatically set min, Max value.
* `min`: The minimum allowed. `min` must be user specified. `[min, max]` forms the domain of the visualMap.
* `max`: The maximum allowed. `max` must be user specified. `[min, max]` forms the domain of the visualMap.
* `range`: Specifies the position of the numeric value corresponding to the handle. Range should be within the range of [min,max].
* ~~`text`: Text on both ends. such as [`High`, `Low`].~~
* ~~`textGap`: The distance between the two text bodies.~~
* `splitNumber`: For continuous data, it is automatically evenly divided into several segments and automatically matches the size of inRange color list when the default is 0.
* `calculable`: Whether the handle used for dragging is displayed (the handle can be dragged to adjust the selected range).
* ~~`realtime`: Whether to update in real time while dragging.~~
* `itemWidth`: The width of the figure, that is, the width of the color bar.
* `itemHeight`: The height of the figure, that is, the height of the color bar.
* `borderWidth`: 边框线宽，单位px。
* `dimension`: Specifies which `dimension` of the `Data` to map to the visual element. `Data` is series.data. Starting at 1, the default is 0 to take the last dimension in data.
* `hoverLink`: When the hoverLink function is turned on, when the mouse hovers over the visualMap component, the corresponding value of the mouse position is highlighted in the corresponding graphic element in the diagram. Conversely, when the mouse hovers over a graphic element in a diagram, the corresponding value of the visualMap component is triangulated in the corresponding position.
* `orient`: Is the layout horizontal or vertical.
* `location`: The location of component.
* `inRange`: Defines the visual color in the selected range.
* ~~`outOfRange`: Defines a visual color outside of the selected range.~~

## `Grid`

Grid component. Drawing grid in rectangular coordinate. In a single grid, at most two X and Y axes each is allowed. Line chart, bar chart, and scatter chart can be drawn in grid. There is only one single grid component at most in a single echarts instance.

* `show`: Whether to show the grid in rectangular coordinate.
* `left`: Distance between grid component and the left side of the container.
* `right`: Distance between grid component and the right side of the container.
* `top`: Distance between grid component and the top side of the container.
* `bottom`: Distance between grid component and the bottom side of the container.
* `backgroundColor`: Background color of grid, which is transparent by default.

## `GaugeAxis`

GaugeAxis sub component. Settings related to gauge axis line.

* `axisLine`: axis line style.
* `splitLine`: slit line style.
* `axisTick`: axis tick style.
* `axisLabel`: axis label style.
* `axisLabelText`: Coordinate axis scale label custom content. When the content is empty, `axisLabel` automatically displays the content according to the scale; otherwise, the content is taken from the list definition.

## `GaugePointer`

GaugePointer sub component. Settings related to gauge pointer.

* `show`: Whether to display a pointer.
* `width`: Pointer width.
* `length`: Pointer length. It can be an absolute value, or it can be a percentage relative to the radius (0-1).

## `XAxis`

The x axis in cartesian(rectangular) coordinate. a grid component can place at most 2 x axis, one on the bottom and another on the top.

* `show`: Whether to show axis. By default `xAxes[0]` is `true` and `xAxes[1]` is `false`.
* `gridIndex`: The index of the grid on which the axis are located, by default, is in the first grid.
* `type`: the type of axis. The default is `Category`.
  * `Value`: Numerical axis for continuous data.
  * `Category`: Category axis, applicable to discrete category data, category data must be set through `data` for this type.
  * `Log`: Log axis, it applies to logarithmic data.
* `position`: the position of axis in grid.
  * `Left`: left of grid.
  * `Right`: right of grid.
  * `Bottom`: bottom of grid.
  * `Top`: top of grid.
* `offset`: the offset of axis from the default position. Useful when the same position has multiple axes.
* `logBaseE`: On the log axis, if base e is the natural number, and is true, logBase fails.
* `logBase`: Base of logarithm, which is valid only for numeric axes with type: `Log`.
* `minMaxType`: the type of axis minmax.The default is `Default`.
  * `Default`: 0 - max.
  * `MinMax`: min - max.
  * `Custom`: Custom min - max.
* `min`: The minimun value of axis. Valid when `minMaxType` is `Custom`.
* `max`: The maximum value of axis. Valid when `minMaxType` is `Custom`.
* `ceilRate`: The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.
* `splitNumber`: Number of segments that the axis is split into. The default is `5`, When `splitNumber` is set to `0`, it draws all the category data.
* `interval`: Compulsively set segmentation interval for axis.This is unavailable for category axis. The `splitNumber` is invalid when set.
* `boundaryGap`: The boundary gap on both sides of a coordinate axis.
* `maxCache`: The max number of axis data cache. The first data will be remove when the size of axis data is larger then `maxCache`.
* `inverse`: Whether the axis are reversed or not. Invalid in `Category` axis.
* `insertDataToHead`: Whether to add new data at the head or at the end of the list.
* `data`: Category data, valid in the `Category` axis.
* `icons`: icon list.
* `axisLine`: the style of axis line [AxisLine](#AxisLine).
* `axisName`: the style of axis name [AxisName](#AxisName).
* `axisTick`: the style of axis tick [AxisTick](#AxisTick).
* `axisLabel`: the style of axis label [AxisLabel](#AxisLabel).
* `splitLine`: the style of axis split line [AxisSplitLine](#SplitLine).
* `splitArea`: the style of axis split area [AxisSplitArea](#AxisSplitArea).
* `iconStyle`: the style of the axis scale icon [IconStyle](#IconStyle).

## `Background`

Background component.Due to the limitations of the framework, there are two limitations to the use of background component:
1: The parent node of chart cannot have a layout control class component.
2: The parent node of Chart can only have one child node of the current chart.

* `show`: Whether to enable the background component. However, the ability to activate the background component is subject to other conditions.
* `image`: the image of background.
* `imageType`: the fill type of background image.
* `imageColor`: the color of background image, The default is `white`.
* `hideThemeBackgroundColor`: Whether to hide the background color set in the `theme` when the background component is on.

## `YAxis`

The y axis in cartesian(rectangular) coordinate. a grid component can place at most 2 y axis, one on the left and another on the right.

The parameters are the same as XAxis.

## `Series`

Serie list. Each serie determines its own chart type by `type`.

Check each serie for parameters.

## `Serie-Line`

Line chart serie.

* `show`: Whether to show serie in chart.
* `type`: `Line`.
* `name`: Series name used for displaying in tooltip and filtering with legend.
* `stack`: If stack the value. On the same category axis, the series with the same stack name would be put on top of each other.
* `xAxisIndex`: Index of x axis to combine with, which is useful for multiple axes in one chart.
* `yAxisIndex`: Index of y axis to combine with, which is useful for multiple axes in one chart.
* `minShow`: The min number of data to show in chart.
* `maxShow`: The max number of data to show in chart.
* `maxCache`: The max number of serie data cache. The first data will be remove when the size of serie data is larger then maxCache.
* `sampleDist`: The minimum horizontal pixel distance of sampling, which defaults to `0` without sampling. When the horizontal pixel distance between two data points is less than this value, start sampling to ensure that the horizontal pixel distance between two points is not less than this value.
* `sampleType`: Sample type. This is valid when `sampleDist` is greater than `0`. The following five sampling types are supported:
  * `Peak`: Take a peak. When the average value of the filter point is greater than or equal to `sampleAverage`, take the maximum value; If you do it the other way around, you get the minimum.
  * `Average`: Take the average of the filter points.
  * `Max`: Take the maximum value of the filter point.
  * `Min`: Take the minimum value of the filter point.
  * `Sum`: Take the sum of the filter points.
* `sampleAverage`: Set the sampling average. When `sampleType` is `Peak`, is the maximum or minimum value used to compare the average value of the filtered data. The default of `0` is to calculate the average of all data in real time.
* `clip`: 是否裁剪超出坐标系部分的图形。
* `ignore`: 是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`: 忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `showAsPositiveNumber`: 将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
* `large`: 是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。缺点: 优化后不能自定义设置单个数据项的样式，不能显示Label，折线图不绘制Symbol。
* `largeThreshold`: 开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
* `areaStyle`: 区域填充样式 [AreaStyle](#AreaStyle)。
* `symbol`: 标记的图形 [SerieSymbol](#SerieSymbol)。
* `lineType`: 折线图样式类型。支持以下十种类型:
  * `Normal`: 普通折线图。
  * `Smooth`: 平滑曲线。
  * `SmoothDash`: 平滑虚线。
  * `StepStart`: 阶梯线图: 当前点。
  * `StepMiddle`: 阶梯线图: 当前点和下一个点的中间。
  * `StepEnd`: 阶梯线图: 下一个拐点。
  * `Dash`: 虚线。
  * `Dot`: 点线。
  * `DashDot`: 点划线。
  * `DashDotDot`: 双点划线。
* `lineStyle`: 线条样式 [LineStyle](#LineStyle)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `lineArrow`: 折线图的箭头 [LineArrow](#LineArrow)。
* `insertDataToHead`: Whether to add new data at the head or at the end of the list.
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Bar`

折线图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Bar`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `stack`: 数据堆叠。同个类目轴上系列配置相同的 `stack` 值后，后一个系列的值会在前一个系列的值上相加。
* `xAxisIndex`: Index of x axis to combine with, which is useful for multiple axes in one chart.
* `yAxisIndex`: Index of y axis to combine with, which is useful for multiple axes in one chart.
* `minShow`: 系列显示数据的最小索引。
* `maxShow`: 系列显示数据的最大索引。
* `maxCache`: 系列中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `barType`: 柱状图类型。以下几种类型: 
  * `Normal`: 普通柱状图。
  * `Zebra`: 斑马柱状图。
  * `Capsule`: 胶囊柱状图。
* `barPercentStack`: 是否百分比堆叠柱状图，相同 `stack` 的 `serie` 只要有一个 `barPercentStack` 为 `true`，则就显示成百分比堆叠柱状图。
* `barWidth`: 柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。
* `barGap`: 不同系列的柱间距离。为百分比（如 `'0.3f'`，表示柱子宽度的 `30%`）。如果想要两个系列的柱子重叠，可以设置 `barGap` 为 `'-1f'`。这在用柱子做背景的时候有用。在同一坐标系上，此属性会被多个 `'bar'` 系列共享。此属性应设置于此坐标系中最后一个 `'bar'` 系列上才会生效，并且是对此坐标系中所有 `'bar'` 系列生效。
* `barCategoryGap`: 同一系列的柱间距离，默认为类目间距的20%，可设固定值。在同一坐标系上，此属性会被多个 `'bar'` 系列共享。此属性应设置于此坐标系中最后一个 `'bar'` 系列上才会生效，并且是对此坐标系中所有 `'bar'` 系列生效。
* `barZebraWidth`: 斑马线的粗细。`barType` 为 `Zebra` 时有效。
* `barZebraGap`: 斑马线的间距。`barType` 为 `Zebra` 时有效。
* `clip`: 是否裁剪超出坐标系部分的图形。
* `ignore`: 是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`: 忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `showAsPositiveNumber`: 将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
* `large`: 是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。缺点: 优化后不能自定义设置单个数据项的样式，不能显示Label，折线图不绘制Symbol。
* `largeThreshold`: 开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
* `symbol`: 标记的图形 [SerieSymbol](#SerieSymbol)。
* `itemStyle`: 柱条样式 [ItemStyle](#ItemStyle)。
* `areaStyle`: 区域填充样式 [AreaStyle](#AreaStyle)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Pie`

饼图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Pie`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `pieRoseType`: 南丁格尔玫瑰图类型，支持以下类型:
  * `None`: 不展示成南丁格尔玫瑰图。
  * `Radius`: 扇区圆心角展现数据的百分比，半径展现数据的大小。
  * `Area`: 所有扇区圆心角相同，仅通过半径展现数据大小。
* `space`: 扇区间隙。
* `center`: 中心点坐标。当值为`0-1`的浮点数时表示百分比。
* `radius`: 半径。`radius[0]`为内径，`radius[1]`为外径。当内径大于0时即为圆环图。
* `minAngle`: The minimum angle of sector(0-360). It prevents some sector from being too small when value is small.
* `roundCap`: 是否启用圆弧效果。
* `ignore`: 是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制，对应的`Label`和`Legend`也不会显示。
* `ignoreValue`: 忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `avoidLabelOverlap`: 在饼图且标签外部显示的情况下，是否启用防止标签重叠策略，默认关闭，在标签拥挤重叠的情况下会挪动各个标签的位置，防止标签间的重叠。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Radar`

雷达图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Radar`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `radarType`: 雷达图类型`RadarType`，支持以下类型: 
  * `Multiple`: 多圈雷达图。此时可一个雷达里绘制多个圈，一个`serieData`就可组成一个圈（多维数据）。
  * `Single`: 单圈雷达图。此时一个雷达只能绘制一个圈，多个`serieData`组成一个圈，数据取自`data[1]`。
* `radarIndex`: 雷达图所使用的 `radar` 组件的 `index`。
* `symbol`: 标记的图形 [SerieSymbol](#SerieSymbol)。
* `lineStyle`: 线条样式 [LineStyle](#LineStyle)。
* `itemStyle`: 标记样式 [ItemStyle](#ItemStyle)。
* `areaStyle`: 区域填充样式 [AreaStyle](#AreaStyle)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Scatter`

散点图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Scatter`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `clip`: 是否裁剪超出坐标系部分的图形。
* `symbol`: 标记的图形 [SerieSymbol](#SerieSymbol)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Heatmap`

热力图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Scatter`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `ignore`: 是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`: 忽略数据的默认值。默认值默认为`0`，当 `ignore` 为 `true` 才有效。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Gauge`

仪表盘系列。

* `show`: 系列是否显示在图表上。
* `type`: `Gauge`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `gaugeType`: 仪表盘类型，支持以下类型:
  * `Pointer`: 指针类型。
  * `ProgressBar`: 进度条类型。
* `center`: 中心点坐标。当值为0-1的浮点数时表示百分比。
* `radius`: 仪表盘半径。
* `min`: 最小的数据值。映射到`startAngle`。
* `max`: 最大的数据值。映射到`endAngle`。
* `startAngle`: 仪表盘起始角度。和时钟一样，`12`点钟位置是`0`度，顺时针到`360`度。
* `endAngle`: 仪表盘结束角度。和时钟一样，`12`点钟位置是`0`度，顺时针到`360`度。
* `splitNumber`: 仪表盘刻度分割段数。
* `roundCap`: 是否启用圆弧效果。
* `titleStyle`: 仪表盘标题 [TitleStyle](#TitleStyle)。
* `gaugeAxis`:  仪表盘坐标轴 [GaugeAxis](#GaugeAxis)。
* `gaugePointer`: 仪表盘指针 [GaugePointer](#GaugePointer)。
* `itemStyle`: 仪表盘指针样式 [ItemStyle](#ItemStyle)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。仪表盘的数据一般只有一个，值通过`label`样式显示，`name`通过`titleStyle`样式显示。

## `Serie-Ring`

环形图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Ring`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `center`: 中心点坐标。当值为`0-1`的浮点数时表示百分比。
* `radius`: 仪表盘半径。
* `startAngle`: 仪表盘起始角度。和时钟一样，`12`点钟位置是`0`度，顺时针到`360`度。
* `ringGap`: 环形图的环间隙。
* `roundCap`: 是否启用圆弧效果。
* `clockwise`: 是否顺时针，默认为`true`。
* `titleStyle`: 环形图中心标题 [TitleStyle](#TitleStyle)。
* `itemStyle`: 环形图的圆环样式，包括设置背景颜色和边框等 [ItemStyle](#ItemStyle)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`: 高亮样式 [Emphasis](#Emphasis)。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。环形图的数据只有二维，`data[0]`表示当前值，`data[1]`表示最大值。

## `Serie-Liquid`

水位图系列。

* `show`: 系列是否显示在图表上。
* `type`: `Liquid`。
* `name`: 系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `vesselIndex`: 水位图所使用的`vessel`组件的`index`。
* `min`: 最小值。
* `max`: 最大值。
* `waveLength`: 水波长。
* `waveHeight`: 水波高。
* `waveSpeed`: 水波移动速度。正数时左移，负数时右移。
* `waveOffset`: 水波偏移。
* `itemStyle`: 环形图的圆环样式，包括设置背景颜色和边框等 [ItemStyle](#ItemStyle)。
* `label`: 图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `animation`: 起始动画 [SerieAnimation](#SerieAnimation)。
* `data`: 系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。水位图的数据一般只有一个，表示当前水位值，用`max`设置最大水位值。

## `Serie-Candlestick`

K线图系列。

* `show`：系列是否显示在图表上。
* `type`：`Candlestick`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `xAxisIndex`：使用的坐标轴X轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `yAxisIndex`：使用的坐标轴Y轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `minShow`：系列显示数据的最小索引。
* `maxShow`：系列显示数据的最大索引。
* `maxCache`：系列中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `clip`：是否裁剪超出坐标系部分的图形。
* `ignore`：是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`：忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `showAsPositiveNumber`：将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
* `large`：是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。缺点：优化后不能自定义设置单个数据项的样式，不能显示Label，折线图不绘制Symbol。
* `largeThreshold`：开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
* `itemStyle`：环形图的圆环样式，包括设置背景颜色和边框等 [ItemStyle](#ItemStyle)。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，K线图至少需要4个维度的数组`[open, close, lowest, highest]`。

## `Settings`

全局参数设置组件。一般情况下可使用默认值，当有需要时可进行调整。

* `reversePainter`：Painter是否逆序。逆序时index大的serie最先绘制。
* `maxPainter`：默认最大Painter数据，当Serie数量大于maxPainter时会平均分配Painter。
* `basePainterMaterial`：Base Pointer 材质球，设置后会影响Axis等。
* `seriePainterMaterial`：Serie Pointer 材质球，设置后会影响所有Serie。
* `topPainterMaterial`：Top Pointer 材质球，设置后会影响Tooltip等。
* `lineSmoothStyle`: 曲线平滑系数。通过调整平滑系数可以改变曲线的曲率，得到外观稍微有变化的不同曲线。
* `lineSmoothness`: 曲线平滑度。值越小曲线越平滑，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
* `lineSegmentDistance`:  线段的分割距离。普通折线图的线是由很多线段组成，段数由该数值决定。值越小段数越多，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
* `cicleSmoothness`: 圆形（包括扇形、环形等）的平滑度。数越小圆越平滑，但顶点数也会随之增加。

## `SerieAnimation`

* `enable`: 是否开启动画系统。
* ~~`threshold`: 是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。~~
* `fadeInDelay`: 设定的渐入动画延时，单位毫秒。如果要设置单个数据项的延时，可以用代码定制: `customFadeInDelay`。
* `fadeInDuration`: 设定的渐入动画时长，单位毫秒。如果要设置单个数据项的渐入时长，可以用代码定制: `customFadeInDuration`。
* `fadeOutDelay`: 设定的渐出动画延时，单位毫秒。如果要设置单个数据项的延时，可以用代码定制: `customFadeOutDelay`。
* `fadeOutDuration`: 设定的渐出动画时长，单位毫秒。如果要设置单个数据项的渐出时长，可以用代码定制: `customFadeOutDuration`。
* `dataChangeEnable`: 是否开启数据变更动画。
* `dataChangeDuration`: 数据变更动画时长，单位毫秒。

## `AreaStyle`

* `show`: 是否显示区域填充。
* `origin`: 区域填充的起始位置 `AreaOrigin`。有以下三种填充方式:
  * `Auto`: 填充坐标轴轴线到数据间的区域。
  * `Start`: 填充坐标轴底部到数据间的区域。
  * `End`: 填充坐标轴顶部到数据间的区域。
* `color`: 区域填充的颜色，默认取 `serie` 对应的颜色。如果 `toColor` 不是默认值，则表示渐变色的起点颜色。
* `toColor`: 区域填充的渐变色的终点颜色。
* `highlightColor`: 高亮时区域填充的颜色，默认取 `serie` 对应的颜色。如果 `highlightToColor` 不是默认值，则表示渐变色的起点颜色。
* `highlightToColor`: 高亮时区域填充的渐变色的终点颜色。
* `opacity`: 图形透明度。支持从 `0` 到 `1` 的数字，为 `0` 时不绘制该图形。
* `tooltipHighlight`: 鼠标悬浮时是否高亮之前的区域。

## `AxisLabel`

* `show`: 是否显示刻度标签。
* `interval`: 坐标轴刻度标签的显示间隔，在类目轴中有效。`0`表示显示所有标签，`1`表示隔一个隔显示一个标签，以此类推。
* `inside`: 刻度标签是否朝内，默认朝外。
* `margin`: 刻度标签与轴线之间的距离。
* `formatter`: 图例内容字符串模版格式器。支持用 `\n` 换行。模板变量为图例名称 `{value}`，数值格式化通过`numericFormatter`。
* `numericFormatter`: 标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式: `A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。
* `showAsPositiveNumber`: 将负数数值显示为正数。一般和`Serie`的`showAsPositiveNumber`配合使用。
* `onZero`: 刻度标签显示在`0`刻度上。
* `width`：刻度标签的宽。当为0时系统自动设置。
* `height`：刻度标签的高。当为0时系统自动设置。

* `textLimit`: 文本自适应 [TextLimit](#TextLimit)。只在类目轴中有效。
* `textStyle`: The style of text [TextStyle](#TextStyle).

## `AxisLine`

* `show`: 是否显示坐标轴轴线。
* `onZero`:  `X` 轴或者 `Y` 轴的轴线是否在另一个轴的 `0` 刻度上，只有在另一个轴为数值轴且包含 `0` 刻度时有效。
* `width`: 坐标轴线线宽。
* `symbol`: 是否显示箭头。
* `symbolWidth`: 箭头宽。
* `symbolHeight`: 箭头高。
* `symbolOffset`: 箭头偏移。
* `symbolDent`: 箭头的凹陷程度。

## `AxisName`

* `show`: 是否显示坐标名称。
* `name`: 坐标轴名称。
* `location`: 坐标轴名称的位置。支持以下类型:
  * `Start`: 坐标轴起始处。
  * `Middle`: 坐标轴中间。
  * `End`: 坐标轴末端。
* `textStyle`: The style of text [TextStyle](#TextStyle).

## `AxisSplitLine`

* `show`: 是否显示坐标分割线。
* `interval`: 分割线的显示间隔。`0` 表示显示所有分割线，`1` 表示隔一个隔显示一个分割线，以此类推。
* `lineStyle`: 线条样式 [LineStyle](#LineStyle)。

## `AxisSplitArea`

* `show`: 是否显示坐标分割区域。
* `color`: 分隔区域颜色。分隔区域会按数组中颜色的顺序依次循环设置颜色。默认是一个深浅的间隔色。

## `AxisTick`

* `show`: 是否显示坐标轴刻度。
* `alignWithLabel`: 类目轴中在 `boundaryGap` 为 `true` 的时候有效，可以保证刻度线和标签对齐。
* `inside`: 坐标轴刻度是否朝内，默认朝外。
* `length`: 坐标轴刻度的长度。
* `width`: 坐标轴刻度的宽度。默认为0时宽度和坐标轴一致。
* `showStartTick`：是否显示第一个刻度。
* `showEndTick`：是否显示最后一个刻度。

## `Emphasis`

* `show`: 是否启用高亮样式。
* `label`: 图形文本标签样式 [SerieLabel](#SerieLabel)。
* `itemStyle`: 图形样式 [ItemStyle](#ItemStyle)。

## `ItemStyle`

* `show`: 是否启用。
* `color`: 颜色。对于K线图，对应阳线的颜色。
* `color0`: 颜色。对于K线图，对应阴线的颜色。
* `toColor`: 渐变颜色1。
* `toColor2`: 渐变颜色2。只在折线图中有效。
* `backgroundColor`: 背景颜色。
* `backgroundWidth`: 背景的宽。
* `centerColor`: 中心区域的颜色。如环形图的中心区域。
* `centerGap`: 中心区域的间隙。如环形图的中心区域于最内环的间隙。
* `borderType`: 边框的类型。
* `borderColor`: 边框的颜色。对于K线图，对应阳线的边框颜色。
* `borderColor0`: 边框的颜色。对于K线图，对应阴线的边框颜色。
* `borderWidth`: 边框宽。
* `opacity`: 透明度。
* `tooltipFormatter`: 提示框单项的字符串模版格式器。具体配置参考`Tooltip`的`formatter`。
* `numericFormatter`: 标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式: `A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。此字段优先于`SerieLabel`和`Tooltip`的`numericFormatter`。
* `cornerRadius`: 圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。支持用0-1的浮点数设置百分比。

## `LineArrow`

* `show`: 是否显示箭头。
* `position`: 箭头显示位置。支持以下两种位置:
  * `End`: 末端显示。最后一个数据上显示箭头。
  * `Start`: 起始端显示。第一个数据上显示箭头。
* `width`: 箭头宽。
* `height`: 箭头长。
* `offset`: 箭头偏移。默认箭头的中心点和数据坐标点一致，可通过 `offset` 调整偏移。
* `dent`: 箭头的凹度。

## `LineStyle`

* `show`: 是否显示线条。当作为子组件，它的父组件有参数控制是否显示时，改参数无效。
* `type`: 线条类型。支持以下五种类型: 
  * `None`: 不显示分割线。
  * `Solid`: 实线。
  * `Dashed`: 虚线。
  * `Dotted`: 点线。
  * `DashDot`: 点划线。
  * `DashDotDot`: 双点划线。
* `color`: 线条颜色。默认和 `serie` 一致。
* `toColor`: 线的渐变颜色（需要水平方向渐变时）。
* `toColor2`: 线的渐变颜色2（需要水平方向三个渐变色的渐变时）。
* `width`: 线条宽。
* `opacity`: 线条的透明度。支持从 `0` 到 `1` 的数字，为 `0` 时不绘制该图形。

## `Location`

* `align`: 对齐方式。有以下对齐方式。
  * `TopLeft`: 左上角对齐。
  * `TopRight`: 右上角对齐。
  * `TopCenter`: 置顶居中对齐。
  * `BottomLeft`: 左下对齐。
  * `BottomRight`: 右下对齐。
  * `BottomCenter`: 底部居中对齐。
  * `Center`: 居中对齐。
  * `CenterLeft`: 中部靠左对齐。
  * `CenterRight`: 中部靠右对齐。
* `left`: 离容器左侧的距离。
* `right`: 离容器右侧的距离。
* `top`: 离容器上侧的距离。
* `bottom`: 离容器下侧的距离。

## `MarkLine`

* `show`：是否显示标线。
* `animation`：标线的动画样式。
* `data`：标线的数据项[MarkLineData](#MarkLineData)列表。当数据项的group为0时，每个数据项表示一条标线；当group不为0时，相同group的两个数据项分别表示标线的起始点和终止点来组成一条标线，此时标线的相关样式参数取起始点的参数。

## `MarkLineData`

* `name`：标注名称，将会作为文字显示。label的formatter可通过{b}显示名称，通过{c}显示数值。
* `type`：特殊的标注类型，用于标注最大值最小值等。。有以下标注类型：
  * `None`：无类型。此时通过
  * `Min`：最小值。`dimension`维度上数据的最小值。
  * `Max`：最大值。`dimension`维度上数据的最大值。
  * `Average`：平均值。`dimension`维度上数据的平均值。
  * `Median`：中位数。`dimension`维度上数据的中位数。
* `dimension`：当type为特殊类型时，指示从哪个维度的数据上计算特殊值。
* `xPosition`：相对原点的 x 坐标，单位像素。当type为None时有效。
* `yPosition`：相对原点的 y 坐标，单位像素。当type为None时有效。
* `xValue`：X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
* `yValue`：Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。

## `SerieData`

* `name`: 数据项名称。
* `selected`: 该数据项是否被选中。
* `radius`: 自定义半径。可用在饼图中自定义某个数据项的半径。
* `enableIconStyle`: 是否启用单个数据项的图标设置。
* `iconStyle`: 数据项图标样式。
* `enableLabel`: 是否启用单个数据项的标签设置。
* `label`: 单个数据项的标签设置。
* `enableItemStyle`: 是否启用单个数据项的样式。
* `itemStyle`: 单个数据项的样式设置。
* `enableEmphasis`: 是否启用单个数据项的高亮样式。
* `emphasis`: 单个数据项的高亮样式设置。
* `enableSymbol`: 是否启用单个数据项的标记设置。
* `symbol`: 单个数据项的标记设置。
* `data`: 可指定任意维数的数值列表。对于折线图和柱状图，`data`其实是`size`为`2`的数组，`data[0]`是x的编号，`data[1]`是`y`的数值，默认显示`data[1]`。其他图表看需求而定是长度大于`2`的数组。可通过`Serie`的`showDataDimension`指定数据长度。

## `SerieLabel`

* `show`: 是否显示文本标签。
* `position`: 标签的位置。折线图时强制默认为 `Center`，支持以下 `5` 种位置:
  * `Outside`: 饼图扇区外侧，通过视觉引导线连到相应的扇区。只在饼图种可用。
  * `Inside`: 饼图扇区内部。只在饼图可用。
  * `Center`: 在中心位置（折线图，柱状图，饼图）。
  * `Top`: 顶部（柱状图）。
  * `Bottom`: 底部（柱状图）。
* `formatter`: 标签内容字符串模版格式器。支持用 `\n` 换行。模板变量有: `{a}`: 系列名；`{b}`: 数据名；`{c}`: 数据值；`{d}`: 百分比。示例: `{b}:{c}`。
* `numericFormatter`: 标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式: `A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。
* `offset`: 距离图形元素的偏移。
* `autoOffset`: 是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。
* `backgroundWidth`: 标签的背景宽度。一般不用指定，不指定时则自动是文字的宽度。
* `backgroundHeight`: 标签的背景高度。一般不用指定，不指定时则自动是文字的高度。
* `paddingLeftRight`: 标签文字和边框的左右边距。
* `paddingTopBottom`: 标签文字和边框的上下边距。
* `line`: 是否显示视觉引导线。在 `label` 位置 设置为 `'Outside'` 的时候会显示视觉引导线。
* `lineType`: 视觉引导线类型。支持以下几种类型:
  * `BrokenLine`: 折线。
  * `Curves`: 曲线。
  * `HorizontalLine`: 水平线。
* `lineColor`: 视觉引导线自定义颜色。
* `lineWidth`: 视觉引导线的宽度。
* `lineLength1`: 视觉引导线第一段的长度。
* `lineLength2`: 视觉引导线第二段的长度。
* `border`: 是否显示边框。
* `borderWidth`: 边框宽度。
* `borderColor`: 边框颜色。
* `textStyle`: The style of text [TextStyle](#TextStyle).

## `SerieSymbol`

* `show`: 是否显示标记。
* `type`: 标记类型。支持以下六种类型: 
  * `EmptyCircle`: 空心圆。
  * `Circle`: 实心圆。
  * `Rect`: 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。
  * `Triangle`: 三角形。
  * `Diamond`: 菱形。
  * `None`: 不显示标记。
* `gap`: 图形标记的外留白距离。
* `sizeType`: 标记图形的大小获取方式。支持以下三种类型: 
  * `Custom`: 自定义大小。
  * `FromData`: 通过 `dataIndex` 从数据中获取，再乘以一个比例系数 `dataScale` 。
  * `Callback`: 通过回调函数 `sizeCallback` 获取。
* `size`: 标记的大小。
* `selectedSize`: 被选中的标记的大小。
* `dataIndex`: 当 `sizeType` 指定为 `FromData` 时，指定的数据源索引。
* `dataScale`: 当 `sizeType` 指定为 `FromData` 时，指定的倍数系数。
* `selectedDataScale`: 当 `sizeType` 指定为 `FromData` 时，指定的高亮倍数系数。
* `sizeCallback`: 当 `sizeType` 指定为 `Callback` 时，指定的回调函数。
* `selectedSizeCallback`: 当 `sizeType` 指定为 `Callback` 时，指定的高亮回调函数。
* `color`: 标记图形的颜色，默认和系列一致。
* `opacity`: 图形标记的透明度。
* `startIndex`: 开始显示图形标记的索引。
* `interval`: 显示图形标记的间隔。`0`表示显示所有标签，`1`表示隔一个隔显示一个标签，以此类推。
* `forceShowLast`: 是否强制显示最后一个图形标记。默认为 `false`。

## `IconStyle`

* `show` : whether to show the icon.
* `Layer` : Shows on top or bottom.
* `Sprite` : Icon.
* `color` : color.
* `width` : The width of the icon.
* `height` : the height of the icon.
* `Offset` : Offset.

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts问答](XCharts问答.md)
