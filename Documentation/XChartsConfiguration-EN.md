# Chart Configuration

[XCharts Homepage](https://github.com/XCharts-Team/XCharts)<br/>
[XCharts API](XChartsAPI-EN.md)<br/>
[XCharts FAQ](XChartsFAQ-EN.md)

## Serie

- [Bar](#Bar)
- [BaseScatter](#BaseScatter)
- [Candlestick](#Candlestick)
- [EffectScatter](#EffectScatter)
- [Heatmap](#Heatmap)
- [Line](#Line)
- [Parallel](#Parallel)
- [Pie](#Pie)
- [Radar](#Radar)
- [Ring](#Ring)
- [Scatter](#Scatter)
- [Serie](#Serie)
- [SimplifiedBar](#SimplifiedBar)
- [SimplifiedCandlestick](#SimplifiedCandlestick)
- [SimplifiedLine](#SimplifiedLine)

## Theme

- [AngleAxisTheme](#AngleAxisTheme)
- [AxisTheme](#AxisTheme)
- [BaseAxisTheme](#BaseAxisTheme)
- [ComponentTheme](#ComponentTheme)
- [DataZoomTheme](#DataZoomTheme)
- [LegendTheme](#LegendTheme)
- [PolarAxisTheme](#PolarAxisTheme)
- [RadarAxisTheme](#RadarAxisTheme)
- [RadiusAxisTheme](#RadiusAxisTheme)
- [SerieTheme](#SerieTheme)
- [SubTitleTheme](#SubTitleTheme)
- [Theme](#Theme)
- [ThemeStyle](#ThemeStyle)
- [TitleTheme](#TitleTheme)
- [TooltipTheme](#TooltipTheme)
- [VisualMapTheme](#VisualMapTheme)

## MainComponent

- [AngleAxis](#AngleAxis)
- [Axis](#Axis)
- [Background](#Background)
- [CalendarCoord](#CalendarCoord)
- [Comment](#Comment)
- [CoordSystem](#CoordSystem)
- [DataZoom](#DataZoom)
- [GridCoord](#GridCoord)
- [Legend](#Legend)
- [MarkArea](#MarkArea)
- [MarkLine](#MarkLine)
- [ParallelAxis](#ParallelAxis)
- [ParallelCoord](#ParallelCoord)
- [PolarCoord](#PolarCoord)
- [RadarCoord](#RadarCoord)
- [RadiusAxis](#RadiusAxis)
- [Settings](#Settings)
- [SingleAxis](#SingleAxis)
- [SingleAxisCoord](#SingleAxisCoord)
- [Title](#Title)
- [Tooltip](#Tooltip)
- [VisualMap](#VisualMap)
- [XAxis](#XAxis)
- [YAxis](#YAxis)

## ChildComponent

- [AngleAxisTheme](#AngleAxisTheme)
- [AnimationStyle](#AnimationStyle)
- [AreaStyle](#AreaStyle)
- [ArrowStyle](#ArrowStyle)
- [AxisLabel](#AxisLabel)
- [AxisLine](#AxisLine)
- [AxisMinorSplitLine](#AxisMinorSplitLine)
- [AxisMinorTick](#AxisMinorTick)
- [AxisName](#AxisName)
- [AxisSplitArea](#AxisSplitArea)
- [AxisSplitLine](#AxisSplitLine)
- [AxisTheme](#AxisTheme)
- [AxisTick](#AxisTick)
- [BaseAxisTheme](#BaseAxisTheme)
- [BaseLine](#BaseLine)
- [BlurStyle](#BlurStyle)
- [CommentItem](#CommentItem)
- [CommentMarkStyle](#CommentMarkStyle)
- [ComponentTheme](#ComponentTheme)
- [DataZoomTheme](#DataZoomTheme)
- [EmphasisStyle](#EmphasisStyle)
- [EndLabelStyle](#EndLabelStyle)
- [IconStyle](#IconStyle)
- [ImageStyle](#ImageStyle)
- [ItemStyle](#ItemStyle)
- [LabelLine](#LabelLine)
- [LabelStyle](#LabelStyle)
- [LegendTheme](#LegendTheme)
- [Level](#Level)
- [LevelStyle](#LevelStyle)
- [LineArrow](#LineArrow)
- [LineStyle](#LineStyle)
- [Location](#Location)
- [MarkAreaData](#MarkAreaData)
- [MarkLineData](#MarkLineData)
- [Padding](#Padding)
- [PolarAxisTheme](#PolarAxisTheme)
- [RadarAxisTheme](#RadarAxisTheme)
- [RadiusAxisTheme](#RadiusAxisTheme)
- [SelectStyle](#SelectStyle)
- [SerieData](#SerieData)
- [SerieSymbol](#SerieSymbol)
- [SerieTheme](#SerieTheme)
- [StageColor](#StageColor)
- [StateStyle](#StateStyle)
- [SubTitleTheme](#SubTitleTheme)
- [SymbolStyle](#SymbolStyle)
- [TextLimit](#TextLimit)
- [TextPadding](#TextPadding)
- [TextStyle](#TextStyle)
- [ThemeStyle](#ThemeStyle)
- [TitleStyle](#TitleStyle)
- [TitleTheme](#TitleTheme)
- [TooltipTheme](#TooltipTheme)
- [VisualMapRange](#VisualMapRange)
- [VisualMapTheme](#VisualMapTheme)

## ISerieExtraComponent

- [AreaStyle](#AreaStyle)
- [BlurStyle](#BlurStyle)
- [EmphasisStyle](#EmphasisStyle)
- [ImageStyle](#ImageStyle)
- [LabelLine](#LabelLine)
- [LabelStyle](#LabelStyle)
- [LineArrow](#LineArrow)
- [SelectStyle](#SelectStyle)
- [TitleStyle](#TitleStyle)

## ISerieDataComponent

- [AreaStyle](#AreaStyle)
- [BlurStyle](#BlurStyle)
- [EmphasisStyle](#EmphasisStyle)
- [ImageStyle](#ImageStyle)
- [ItemStyle](#ItemStyle)
- [LabelLine](#LabelLine)
- [LabelStyle](#LabelStyle)
- [LineStyle](#LineStyle)
- [SelectStyle](#SelectStyle)
- [SerieSymbol](#SerieSymbol)
- [TitleStyle](#TitleStyle)

## Other

- [BaseSerie](#BaseSerie)
- [ChartText](#ChartText)
- [ChildComponent](#ChildComponent)
- [DebugInfo](#DebugInfo)
- [Indicator](#Indicator)
- [Lang](#Lang)
- [LangCandlestick](#LangCandlestick)
- [LangTime](#LangTime)
- [MainComponent](#MainComponent)
- [XCResourcesImporter](#XCResourcesImporter)
- [XCSettings](#XCSettings)

## AngleAxis

Inherits or Implemented: [Axis](#Axis)

Angle axis of Polar Coordinate.

|field|default|since|comment|
|--|--|--|--|
|startAngle|0||Starting angle of axis. 0 degrees by default, standing for right position of center.

## AngleAxisTheme

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## AnimationStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

the animation of serie.

|field|default|since|comment|
|--|--|--|--|
|enable|true||Whether to enable animation.
|type|||The type of animation.<br/>`AnimationType`:<br/>- `Default`: he default. An animation playback mode will be selected according to the actual situation.<br/>- `LeftToRight`: Play the animation from left to right.<br/>- `BottomToTop`: Play the animation from bottom to top.<br/>- `InsideOut`: Play animations from the inside out.<br/>- `AlongPath`: Play the animation along the path.<br/>- `Clockwise`: Play the animation clockwise.<br/>|
|easting|||Easing method used for the first animation.<br/>`AnimationEasing`:<br/>- `Linear`: <br/>|
|threshold|2000||Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
|fadeInDuration|1000||The milliseconds duration of the fadeIn animation.
|fadeInDelay|0||The milliseconds delay before updating the first animation.
|fadeOutDuration|1000f||The milliseconds duration of the fadeOut animation.
|fadeOutDelay|0||渐出动画延时（毫秒）。如果要设置单个数据项的延时，可以用代码定制：customFadeOutDelay。
|dataChangeEnable|true||是否开启数据变更动画。
|dataChangeDuration|500||The milliseconds duration of the data change animation.
|actualDuration|||The milliseconds actual duration of the first animation.
|unscaledTime||v3.4.0|Animation updates independently of Time.timeScale.

## AreaStyle

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

The style of area.

|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent the areafrom showing.
|origin|||the origin of area.<br/>`AreaStyle.AreaOrigin`:<br/>- `Auto`: to fill between axis line to data.<br/>- `Start`: to fill between min axis value (when not inverse) to data.<br/>- `End`: to fill between max axis value (when not inverse) to data.<br/>|
|color|||the color of area,default use serie color.
|toColor|||Gradient color, start color to toColor.
|opacity|0.6f||Opacity of the component. Supports value from 0 to 1, and the component will not be drawn when set to 0.
|innerFill||v3.2.0|Whether to fill only polygonal areas. Currently, only convex polygons are supported.

## ArrowStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|width|10||The widht of arrow.
|height|15||The height of arrow.
|offset|0||The offset of arrow.
|dent|3||The dent of arrow.
|color|Color.clear||the color of arrow.

## Axis

Inherits or Implemented: [MainComponent](#MainComponent)

The axis in rectangular coordinate.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show axis.
|type|||the type of axis.<br/>`Axis.AxisType`:<br/>- `Value`: Numerical axis, suitable for continuous data.<br/>- `Category`: Category axis, suitable for discrete category data. Data should only be set via data for this type.<br/>- `Log`: Log axis, suitable for log data.<br/>- `Time`: Time axis, suitable for continuous time series data.<br/>|
|minMaxType|||the type of axis minmax.<br/>`Axis.AxisMinMaxType`:<br/>- `Default`: 0 - maximum.<br/>- `MinMax`: minimum - maximum.<br/>- `Custom`: Customize the minimum and maximum.<br/>|
|gridIndex|||The index of the grid on which the axis are located, by default, is in the first grid.
|polarIndex|||The index of the polar on which the axis are located, by default, is in the first polar.
|parallelIndex|||The index of the parallel on which the axis are located, by default, is in the first parallel.
|position|||the position of axis in grid.<br/>`Axis.AxisPosition`:<br/>- `Left`: the position of axis in grid.<br/>- `Right`: the position of axis in grid.<br/>- `Bottom`: the position of axis in grid.<br/>- `Top`: the position of axis in grid.<br/>|
|offset|||the offset of axis from the default position. Useful when the same position has multiple axes.
|min|||The minimun value of axis.Valid when `minMaxType` is `Custom`
|max|||The maximum value of axis.Valid when `minMaxType` is `Custom`
|splitNumber|0||Number of segments that the axis is split into.
|interval|0||Compulsively set segmentation interval for axis.This is unavailable for category axis.
|boundaryGap|true||The boundary gap on both sides of a coordinate axis, which is valid only for category axis with type: 'Category'.
|maxCache|0||The max number of axis data cache.
|logBase|10||Base of logarithm, which is valid only for numeric axes with type: 'Log'.
|logBaseE|false||On the log axis, if base e is the natural number, and is true, logBase fails.
|ceilRate|0||The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.
|inverse|false||Whether the axis are reversed or not. Invalid in `Category` axis.
|clockwise|true||Whether the positive position of axis is in clockwise. True for clockwise by default.
|insertDataToHead|||Whether to add new data at the head or at the end of the list.
|icons|||类目数据对应的图标。
|data|||Category data, available in type: 'Category' axis.
|axisLine|||axis Line. [AxisLine](#AxisLine)|
|axisName|||axis name. [AxisName](#AxisName)|
|axisTick|||axis tick. [AxisTick](#AxisTick)|
|axisLabel|||axis label. [AxisLabel](#AxisLabel)|
|splitLine|||axis split line. [AxisSplitLine](#AxisSplitLine)|
|splitArea|||axis split area. [AxisSplitArea](#AxisSplitArea)|
|minorTick||v3.2.0|axis minor tick. [AxisMinorTick](#AxisMinorTick)|
|minorSplitLine||v3.2.0|axis minor split line. [AxisMinorSplitLine](#AxisMinorSplitLine)|
|indicatorLabel||v3.4.0|Style of axis tooltip indicator label. [LabelStyle](#LabelStyle)|

## AxisLabel

Inherits or Implemented: [LabelStyle](#LabelStyle)

Settings related to axis label.

|field|default|since|comment|
|--|--|--|--|
|interval|0||The display interval of the axis label.
|inside|false||Set this to true so the axis labels face the inside direction.
|showAsPositiveNumber|false||Show negative number as positive number.
|onZero|false||刻度标签显示在0刻度上。
|showStartLabel|true||Whether to display the first label.
|showEndLabel|true||Whether to display the last label.
|textLimit|||文本限制。 [TextLimit](#TextLimit)|

## AxisLine

Inherits or Implemented: [BaseLine](#BaseLine)

Settings related to axis line.

|field|default|since|comment|
|--|--|--|--|
|onZero|||When mutiple axes exists, this option can be used to specify which axis can be "onZero" to.
|showArrow|||Whether to show the arrow symbol of axis.
|arrow|||the arrow of line. [ArrowStyle](#ArrowStyle)|

## AxisMinorSplitLine

Inherits or Implemented: [BaseLine](#BaseLine)

> Since `v3.2.0`

Minor split line of axis in grid area.

|field|default|since|comment|
|--|--|--|--|
|distance|||The distance between the split line and axis line.
|autoColor|||auto color.

## AxisMinorTick

Inherits or Implemented: [BaseLine](#BaseLine)

> Since `v3.2.0`

Settings related to axis minor tick.

|field|default|since|comment|
|--|--|--|--|
|splitNumber|5||Number of segments that the axis is split into.
|autoColor|||

## AxisName

Inherits or Implemented: [ChildComponent](#ChildComponent)

the name of axis.

|field|default|since|comment|
|--|--|--|--|
|show|||Whether to show axis name.
|name|||the name of axis.
|onZero||v3.1.0|Whether the axis name position are the same with 0 position of YAxis.
|labelStyle|||The text style of axis name. [LabelStyle](#LabelStyle)|

## AxisSplitArea

Inherits or Implemented: [ChildComponent](#ChildComponent)

Split area of axis in grid area, not shown by default.

|field|default|since|comment|
|--|--|--|--|
|show|||Set this to true to show the splitArea.
|color|||Color of split area. SplitArea color could also be set in color array, which the split lines would take as their colors in turns. Dark and light colors in turns are used by default.

## AxisSplitLine

Inherits or Implemented: [BaseLine](#BaseLine)

Split line of axis in grid area.

|field|default|since|comment|
|--|--|--|--|
|interval|||Interval of Axis splitLine.
|distance|||The distance between the split line and axis line.
|autoColor|||auto color.
|showStartLine|true|v3.3.0|Whether to show the first split line.
|showEndLine|true|v3.3.0|Whether to show the last split line.

## AxisTheme

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## AxisTick

Inherits or Implemented: [BaseLine](#BaseLine)

Settings related to axis tick.

|field|default|since|comment|
|--|--|--|--|
|alignWithLabel|||Align axis tick with label, which is available only when boundaryGap is set to be true in category axis.
|inside|||Set this to true so the axis labels face the inside direction.
|showStartTick|||Whether to display the first tick.
|showEndTick|||Whether to display the last tick.
|distance|||The distance between the tick line and axis line.
|splitNumber|0||Number of segments that the axis is split into.
|autoColor|||

## Background

Inherits or Implemented: [MainComponent](#MainComponent)

Background component.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to enable the background component.
|image|||the image of background.
|imageType|||the fill type of background image.
|imageColor|||背景图颜色。
|autoColor|true||Whether to use theme background color for component color when the background component is on.

## Bar

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## BaseAxisTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|since|comment|
|--|--|--|--|
|lineType|||the type of line.<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|lineWidth|1f||the width of line.
|lineLength|0f||the length of line.
|lineColor|||the color of line.
|splitLineType|||the type of split line.<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|splitLineWidth|1f||the width of split line.
|splitLineLength|0f||the length of split line.
|splitLineColor|||the color of split line.
|minorSplitLineColor|||the color of minor split line.
|tickWidth|1f||the width of tick.
|tickLength|5f||the length of tick.
|tickColor|||the color of tick.
|splitAreaColors|||the colors of split area.

## BaseLine

Inherits or Implemented: [ChildComponent](#ChildComponent)

Settings related to base line.

|field|default|since|comment|
|--|--|--|--|
|show|||Set this to false to prevent the axis line from showing.
|lineStyle|||线条样式 [LineStyle](#LineStyle)|

## BaseScatter

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## BaseSerie


## BlurStyle

Inherits or Implemented: [StateStyle](#StateStyle),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

> Since `v3.2.0`

Configurations of blur state.


## CalendarCoord

Inherits or Implemented: [CoordSystem](#CoordSystem),[IUpdateRuntimeData](#IUpdateRuntimeData),[ISerieContainer](#ISerieContainer)


## Candlestick

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## ChartText


## ChildComponent


## Comment

Inherits or Implemented: [MainComponent](#MainComponent)

comment of chart.

|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent the comment from showing.
|labelStyle|||The text style of all comments. [LabelStyle](#LabelStyle)|
|markStyle|||The text style of all comments. [CommentMarkStyle](#CommentMarkStyle)|
|items|||The items of comment.

## CommentItem

Inherits or Implemented: [ChildComponent](#ChildComponent)

comment of chart.

|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent this comment item from showing.
|content|||content of comment.
|position|||position of comment.
|markRect|||the mark rect of comment.
|markStyle|||the mark rect style. [CommentMarkStyle](#CommentMarkStyle)|
|labelStyle|||The text style of all comments. [LabelStyle](#LabelStyle)|

## CommentMarkStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

the comment mark style.

|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent this comment item from showing.
|lineStyle|||line style of comment mark area. [LineStyle](#LineStyle)|

## ComponentTheme

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|font|||the font of text.
|textColor|||the color of text.
|textBackgroundColor|||the color of text.
|fontSize|18||the font size of text.
|tMPFont|||the font of chart text。

## CoordSystem

Inherits or Implemented: [MainComponent](#MainComponent)

Coordinate system component.


## DataZoom

Inherits or Implemented: [MainComponent](#MainComponent),[IUpdateRuntimeData](#IUpdateRuntimeData)

DataZoom component is used for zooming a specific area, which enables user to investigate data in detail, or get an overview of the data, or get rid of outlier points.

|field|default|since|comment|
|--|--|--|--|
|enable|true||Whether to show dataZoom.
|filterMode|||The mode of data filter.<br/>`DataZoom.FilterMode`:<br/>- `Filter`: data that outside the window will be filtered, which may lead to some changes of windows of other axes. For each data item, it will be filtered if one of the relevant dimensions is out of the window.<br/>- `WeakFilter`: data that outside the window will be filtered, which may lead to some changes of windows of other axes. For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.<br/>- `Empty`: data that outside the window will be set to NaN, which will not lead to changes of windows of other axes.<br/>- `None`: Do not filter data.<br/>|
|xAxisIndexs|||Specify which xAxis is controlled by the dataZoom.
|yAxisIndexs|||Specify which yAxis is controlled by the dataZoom.
|supportInside|||Whether built-in support is supported. Built into the coordinate system to allow the user to zoom in and out of the coordinate system by mouse dragging, mouse wheel, finger swiping (on the touch screen).
|supportInsideScroll|true||是否支持坐标系内滚动
|supportInsideDrag|true||是否支持坐标系内拖拽
|supportSlider|||Whether a slider is supported. There are separate sliders on which the user zooms or roams.
|supportSelect|||是否支持框选。提供一个选框进行数据区域缩放。
|showDataShadow|||Whether to show data shadow, to indicate the data tendency in brief.
|showDetail|||Whether to show detail, that is, show the detailed data information when dragging.
|zoomLock|||Specify whether to lock the size of window (selected area).
|fillerColor|||the color of dataZoom data area.
|borderColor|||the color of dataZoom border.
|borderWidth|||边框宽。
|backgroundColor|||The background color of the component.
|left|||Distance between dataZoom component and the left side of the container. left value is a instant pixel value like 10 or float value [0-1].
|right|||Distance between dataZoom component and the right side of the container. right value is a instant pixel value like 10 or float value [0-1].
|top|||Distance between dataZoom component and the top side of the container. top value is a instant pixel value like 10 or float value [0-1].
|bottom|||Distance between dataZoom component and the bottom side of the container. bottom value is a instant pixel value like 10 or float value [0-1].
|rangeMode|||Use absolute value or percent value in DataZoom.start and DataZoom.end.<br/>`DataZoom.RangeMode`:<br/>- `//Value`: The value type of start and end.取值类型<br/>- `Percent`: percent value.<br/>|
|start|||The start percentage of the window out of the data extent, in the range of 0 ~ 100.
|end|||The end percentage of the window out of the data extent, in the range of 0 ~ 100.
|minShowNum|1||Minimum number of display data. Minimum number of data displayed when DataZoom is enlarged to maximum.
|scrollSensitivity|1.1f||The sensitivity of dataZoom scroll. The larger the number, the more sensitive it is.
|orient|||Specify whether the layout of dataZoom component is horizontal or vertical. What's more, it indicates whether the horizontal axis or vertical axis is controlled by default in catesian coordinate system.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|labelStyle|||label style. [LabelStyle](#LabelStyle)|
|lineStyle|||阴影线条样式。 [LineStyle](#LineStyle)|
|areaStyle|||阴影填充样式。 [AreaStyle](#AreaStyle)|

## DataZoomTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|since|comment|
|--|--|--|--|
|borderWidth|||the width of border line.
|dataLineWidth|||the width of data line.
|fillerColor|||the color of dataZoom data area.
|borderColor|||the color of dataZoom border.
|dataLineColor|||the color of data area line.
|dataAreaColor|||the color of data area line.
|backgroundColor|||the background color of datazoom.

## DebugInfo

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether show debug component.
|showDebugInfo|false||
|showAllChartObject|false||Whether show children components of chart in hierarchy view.
|foldSeries|false||Whether to fold series in inspector view.
|labelStyle||| [LabelStyle](#LabelStyle)|

## EffectScatter

Inherits or Implemented: [BaseScatter](#BaseScatter)


## EmphasisStyle

Inherits or Implemented: [StateStyle](#StateStyle),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

> Since `v3.2.0`

Configurations of emphasis state.

|field|default|since|comment|
|--|--|--|--|
|scale|1.1f||Whether to scale to highlight the data in emphasis state.
|focus|||When the data is highlighted, whether to fade out of other data to focus the highlighted.<br/>`EmphasisStyle.FocusType`:<br/>- `None`: Do not fade out other data, it's by default.<br/>- `Self`: Only focus (not fade out) the element of the currently highlighted data.<br/>- `Series`: Focus on all elements of the series which the currently highlighted data belongs to.<br/>|
|blurScope|||The range of fade out when focus is enabled.<br/>`EmphasisStyle.BlurScope`:<br/>- `GridCoord`: coordinate system.<br/>- `Series`: series.<br/>- `Global`: global.<br/>|

## EndLabelStyle

Inherits or Implemented: [LabelStyle](#LabelStyle)


## GridCoord

Inherits or Implemented: [CoordSystem](#CoordSystem),[IUpdateRuntimeData](#IUpdateRuntimeData),[ISerieContainer](#ISerieContainer)

Grid component.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the grid in rectangular coordinate.
|left|0.1f||Distance between grid component and the left side of the container.
|right|0.08f||Distance between grid component and the right side of the container.
|top|0.22f||Distance between grid component and the top side of the container.
|bottom|0.12f||Distance between grid component and the bottom side of the container.
|backgroundColor|||Background color of grid, which is transparent by default.
|showBorder|false||Whether to show the grid border.
|borderWidth|0f||Border width of grid.
|borderColor|||The color of grid border.

## Heatmap

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)

|field|default|since|comment|
|--|--|--|--|
|heatmapType||v3.3.0|The mapping type of heatmap.<br/>`HeatmapType`:<br/>- `Data`: Data mapping type.By default, the second dimension data is used as the color map.<br/>- `Count`: Number mapping type.The number of occurrences of a statistic in a divided grid, as a color map.<br/>|

## IconStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|show|false||Whether the data icon is show.
|layer|||显示在上层还是在下层。<br/>`IconStyle.Layer`:<br/>- `UnderText`: The icon is display under the label text. 图标在标签文字下<br/>- `AboveText`: The icon is display above the label text. 图标在标签文字上<br/>|
|align|||水平方向对齐方式。<br/>`Align`:<br/>- `Center`: 对齐方式<br/>- `Left`: 对齐方式<br/>- `Right`: 对齐方式<br/>|
|sprite|||The image of icon.
|type|||How to display the icon.
|color|||图标颜色。
|width|20||图标宽。
|height|20||图标高。
|offset|||图标偏移。
|autoHideWhenLabelEmpty|false||当label内容为空时是否自动隐藏图标

## ImageStyle

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the data icon is show.
|sprite|||The image of icon.
|type|||How to display the image.
|autoColor|||是否自动颜色。
|color|||图标颜色。
|width|0||图标宽。
|height|0||图标高。

## Indicator

Indicator of radar chart, which is used to assign multiple variables(dimensions) in radar chart.

|field|default|since|comment|
|--|--|--|--|
|name|||The name of indicator.
|max|||The maximum value of indicator, with default value of 0, but we recommend to set it manually.
|min|||The minimum value of indicator, with default value of 0.
|range|||Normal range. When the value is outside this range, the display color is automatically changed.
|show|||[default:true] Set this to false to prevent the radar from showing.
|shape|||Radar render type, in which 'Polygon' and 'Circle' are supported.
|radius|100||the radius of radar.
|splitNumber|5||Segments of indicator axis.
|center|||the center of radar chart.
|axisLine|||axis line. [AxisLine](#AxisLine)|
|axisName|||Name options for radar indicators. [AxisName](#AxisName)|
|splitLine|||split line. [AxisSplitLine](#AxisSplitLine)|
|splitArea|||Split area of axis in grid area. [AxisSplitArea](#AxisSplitArea)|
|indicator|true||Whether to show indicator.
|positionType|||The position type of indicator.
|indicatorGap|10||The gap of indicator and radar.
|ceilRate|0||The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.
|isAxisTooltip|||是否Tooltip显示轴线上的所有数据。
|outRangeColor|Color.red||The color displayed when data out of range.
|connectCenter|false||Whether serie data connect to radar center with line.
|lineGradient|true||Whether need gradient for data line.
|startAngle||v3.4.0|起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|indicatorList|||the indicator list.

## ItemStyle

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieDataComponent](#ISerieDataComponent)

图形样式。

|field|default|since|comment|
|--|--|--|--|
|show|true||是否启用。
|color|||数据项颜色。
|color0|||数据项颜色。
|toColor|||Gradient color1.
|toColor2|||Gradient color2.Only valid in line diagrams.
|backgroundColor|||数据项背景颜色。
|backgroundWidth|||数据项背景宽度。
|centerColor|||中心区域颜色。
|centerGap|||中心区域间隙。
|borderWidth|0||边框宽。
|borderGap|0||边框间隙。
|borderColor|||边框的颜色。
|borderColor0|||边框的颜色。
|borderToColor|||边框的渐变色。
|opacity|1||透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
|itemMarker|||提示框单项的字符标志。用在Tooltip中。
|itemFormatter|||提示框单项的字符串模版格式器。具体配置参考`Tooltip`的`formatter`
|numericFormatter|||Standard numeric format strings.
|cornerRadius|||The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, upper right, bottom right and bottom left)).

## LabelLine

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

标签的引导线

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the label line is showed.
|lineType|||the type of visual guide line.<br/>`LabelLine.LineType`:<br/>- `BrokenLine`: 折线<br/>- `Curves`: 曲线<br/>- `HorizontalLine`: 水平线<br/>|
|lineColor|ChartConst.clearColor32||the color of visual guild line.
|lineAngle|0||the angle of visual guild line.
|lineWidth|1.0f||the width of visual guild line.
|lineGap|1.0f||the gap of container and guild line.
|lineLength1|25f||The length of the first segment of visual guide line.
|lineLength2|15f||The length of the second segment of visual guide line.
|startSymbol|||The symbol of the start point of labelline. [SymbolStyle](#SymbolStyle)|
|endSymbol|||The symbol of the end point of labelline. [SymbolStyle](#SymbolStyle)|

## LabelStyle

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

Text label of chart, to explain some data information about graphic item like value, name and so on.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the label is showed.
|Position|||The position of label.
|autoOffset|false||是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。
|offset|||offset to the host graphic element.
|rotate|||Rotation of label.
|distance|||距离轴线的距离。
|formatter|||formatter of label.
|numericFormatter|||Standard numeric format strings.
|width|0||the width of label. If set as default value 0, it means than the label width auto set as the text width.
|height|0||the height of label. If set as default value 0, it means than the label height auto set as the text height.
|icon|||the sytle of icon. [IconStyle](#IconStyle)|
|background|||the sytle of background. [ImageStyle](#ImageStyle)|
|textPadding|||the text padding of label. [TextPadding](#TextPadding)|
|textStyle|||the sytle of text. [TextStyle](#TextStyle)|

## Lang

Inherits or Implemented: [ScriptableObject](#ScriptableObject)

Language.


## LangCandlestick


## LangTime


## Legend

Inherits or Implemented: [MainComponent](#MainComponent),[IPropertyChanged](#IPropertyChanged)

Legend component.The legend component shows different sets of tags, colors, and names. You can control which series are not displayed by clicking on the legend.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show legend component.
|iconType|||Type of legend.<br/>`Legend.Type`:<br/>- `Auto`: 自动匹配。<br/>- `Custom`: 自定义图标。<br/>- `EmptyCircle`: 空心圆。<br/>- `Circle`: 圆形。<br/>- `Rect`: 正方形。可通过Setting的legendIconCornerRadius参数调整圆角。<br/>- `Triangle`: 三角形。<br/>- `Diamond`: 菱形。<br/>- `Candlestick`: 烛台（可用于K线图）。<br/>|
|selectedMode|||Selected mode of legend, which controls whether series can be toggled displaying by clicking legends.<br/>`Legend.SelectedMode`:<br/>- `Multiple`: 多选。<br/>- `Single`: 单选。<br/>- `None`: 无法选择。<br/>|
|orient|||Specify whether the layout of legend component is horizontal or vertical.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|location|||The location of legend. [Location](#Location)|
|itemWidth|25.0f||Image width of legend symbol.
|itemHeight|12.0f||Image height of legend symbol.
|itemGap|10f||The distance between each legend, horizontal distance in horizontal layout, and vertical distance in vertical layout.
|itemAutoColor|true||Whether the legend symbol matches the color automatically.
|itemOpacity|1||the opacity of item color.
|formatter|||Legend content string template formatter. Support for wrapping lines with \n. Template:{value}.
|numericFormatter|||Standard numeric format strings.
|labelStyle|||the style of text. [LabelStyle](#LabelStyle)|
|data|||Data array of legend. An array item is usually a name representing string. (If it is a pie chart, it could also be the name of a single data in the pie chart) of a series. If data is not specified, it will be auto collected from series.
|icons|||自定义的图例标记图形。
|colors|||the colors of legend item.
|background||v3.1.0|the sytle of background. [ImageStyle](#ImageStyle)|
|padding||v3.1.0|the paddinng of item and background. [Padding](#Padding)|

## LegendTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|since|comment|
|--|--|--|--|
|unableColor|||the color of text.

## Level

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|label|||文本标签样式。 [LabelStyle](#LabelStyle)|
|upperLabel|||上方的文本标签样式。 [LabelStyle](#LabelStyle)|
|itemStyle|||数据项样式。 [ItemStyle](#ItemStyle)|

## LevelStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|show|false||是否启用LevelStyle
|levels|||各层节点对应的配置。当enableLevels为true时生效，levels[0]对应的第一层的配置，levels[1]对应第二层，依次类推。当levels中没有对应层时用默认的设置。

## Line

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## LineArrow

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent)

|field|default|since|comment|
|--|--|--|--|
|show|||Whether to show the arrow.
|position|||The position of arrow.<br/>`LineArrow.Position`:<br/>- `End`: 末端箭头<br/>- `Start`: 头端箭头<br/>|
|arrow|||the arrow of line. [ArrowStyle](#ArrowStyle)|

## LineStyle

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieDataComponent](#ISerieDataComponent)

The style of line.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether show line.
|type|||the type of line.<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|color|||the color of line, default use serie color.
|toColor|||the middle color of line, default use serie color.
|toColor2|||the end color of line, default use serie color.
|width|0||the width of line.
|length|0||the length of line.
|opacity|1||Opacity of the line. Supports value from 0 to 1, and the line will not be drawn when set to 0.

## Location

Inherits or Implemented: [ChildComponent](#ChildComponent),[IPropertyChanged](#IPropertyChanged)

Location type. Quick to set the general location.

|field|default|since|comment|
|--|--|--|--|
|align|||对齐方式。<br/>`Location.Align`:<br/>- `TopLeft`: 对齐方式<br/>- `TopRight`: 对齐方式<br/>- `TopCenter`: 对齐方式<br/>- `BottomLeft`: 对齐方式<br/>- `BottomRight`: 对齐方式<br/>- `BottomCenter`: 对齐方式<br/>- `Center`: 对齐方式<br/>- `CenterLeft`: 对齐方式<br/>- `CenterRight`: 对齐方式<br/>|
|left|||Distance between component and the left side of the container.
|right|||Distance between component and the left side of the container.
|top|||Distance between component and the left side of the container.
|bottom|||Distance between component and the left side of the container.

## MainComponent

Inherits or Implemented: [IComparable](#IComparable)


## MarkArea

Inherits or Implemented: [MainComponent](#MainComponent)

Used to mark an area in chart. For example, mark a time interval.

|field|default|since|comment|
|--|--|--|--|
|show|true||是否显示标域。
|text|||The text of markArea. 标域显示的文本。
|serieIndex|0||Serie index of markArea. 标域影响的Serie索引。
|start|||标域范围的起始数据。 [MarkAreaData](#MarkAreaData)|
|end|||标域范围的结束数据。 [MarkAreaData](#MarkAreaData)|
|itemStyle|||标域样式。 [ItemStyle](#ItemStyle)|
|label|||标域文本样式。 [LabelStyle](#LabelStyle)|

## MarkAreaData

Inherits or Implemented: [ChildComponent](#ChildComponent)

标域的数据。

|field|default|since|comment|
|--|--|--|--|
|type|||Special markArea types, are used to label maximum value, minimum value and so on.<br/>`MarkAreaType`:<br/>- `None`: 标域类型<br/>- `Min`: 最小值。<br/>- `Max`: 最大值。<br/>- `Average`: 平均值。<br/>- `Median`: 中位数。<br/>|
|name|||Name of the marker, which will display as a label.
|dimension|1||From which dimension of data to calculate the maximum and minimum value and so on.
|xPosition|||The x coordinate relative to the origin, in pixels.
|yPosition|||The y coordinate relative to the origin, in pixels.
|xValue|||The value specified on the X-axis. A value specified when the X-axis is the category axis represents the index of the category axis data, otherwise a specific value.
|yValue|||That's the value on the Y-axis. The value specified when the Y axis is the category axis represents the index of the category axis data, otherwise the specific value.

## MarkLine

Inherits or Implemented: [MainComponent](#MainComponent)

Use a line in the chart to illustrate.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to display the marking line.
|serieIndex|0||The serie index of markLine.
|animation|||The animation of markline. [AnimationStyle](#AnimationStyle)|
|data|||A list of marked data. When the group of data item is 0, each data item represents a line; When the group is not 0, two data items of the same group represent the starting point and the ending point of the line respectively to form a line. In this case, the relevant style parameters of the line are the parameters of the starting point.

## MarkLineData

Inherits or Implemented: [ChildComponent](#ChildComponent)

Data of marking line.

|field|default|since|comment|
|--|--|--|--|
|type|||Special label types, are used to label maximum value, minimum value and so on.<br/>`MarkLineType`:<br/>- `None`: 标线类型<br/>- `Min`: 最小值。<br/>- `Max`: 最大值。<br/>- `Average`: 平均值。<br/>- `Median`: 中位数。<br/>|
|name|||Name of the marker, which will display as a label.
|dimension|1||From which dimension of data to calculate the maximum and minimum value and so on.
|xPosition|||The x coordinate relative to the origin, in pixels.
|yPosition|||The y coordinate relative to the origin, in pixels.
|xValue|||The value specified on the X-axis. A value specified when the X-axis is the category axis represents the index of the category axis data, otherwise a specific value.
|yValue|||That's the value on the Y-axis. The value specified when the Y axis is the category axis represents the index of the category axis data, otherwise the specific value.
|group|0||Grouping. When the group is not 0, it means that this data is the starting point or end point of the marking line. Data consistent with the group form a marking line.
|zeroPosition|false||Is the origin of the coordinate system.
|startSymbol|||The symbol of the start point of markline. [SymbolStyle](#SymbolStyle)|
|endSymbol|||The symbol of the end point of markline. [SymbolStyle](#SymbolStyle)|
|lineStyle|||The line style of markline. [LineStyle](#LineStyle)|
|label|||Text styles of label. You can set position to Start, Middle, and End to display text in different locations. [LabelStyle](#LabelStyle)|

## Padding

Inherits or Implemented: [ChildComponent](#ChildComponent)

padding setting of item or text.

|field|default|since|comment|
|--|--|--|--|
|show|true||show padding. 是否显示。
|top|0||padding of top.
|right|2f||padding of right.
|left|2f||padding of left.
|bottom|0||padding of bottom.

## Parallel

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## ParallelAxis

Inherits or Implemented: [Axis](#Axis)


## ParallelCoord

Inherits or Implemented: [CoordSystem](#CoordSystem),[IUpdateRuntimeData](#IUpdateRuntimeData),[ISerieContainer](#ISerieContainer)

Grid component.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the grid in rectangular coordinate.
|orient|||Orientation of the axis. By default, it's 'Vertical'. You can set it to be 'Horizonal' to make a vertical axis.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|left|0.1f||Distance between grid component and the left side of the container.
|right|0.08f||Distance between grid component and the right side of the container.
|top|0.22f||Distance between grid component and the top side of the container.
|bottom|0.12f||Distance between grid component and the bottom side of the container.
|backgroundColor|||Background color of grid, which is transparent by default.

## Pie

Inherits or Implemented: [Serie](#Serie)


## PolarAxisTheme

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## PolarCoord

Inherits or Implemented: [CoordSystem](#CoordSystem),[ISerieContainer](#ISerieContainer)

Polar coordinate can be used in scatter and line chart. Every polar coordinate has an angleAxis and a radiusAxis.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the polor component.
|center|||The center of ploar. The center[0] is the x-coordinate, and the center[1] is the y-coordinate. When value between 0 and 1 represents a percentage  relative to the chart.
|radius|||the radius of polar.
|backgroundColor|||Background color of polar, which is transparent by default.

## Radar

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)

|field|default|since|comment|
|--|--|--|--|
|smooth|false|v3.2.0|Whether use smooth curve.

## RadarAxisTheme

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## RadarCoord

Inherits or Implemented: [CoordSystem](#CoordSystem),[ISerieContainer](#ISerieContainer)

Radar coordinate conponnet for radar charts. 雷达图坐标系组件，只适用于雷达图。


## RadiusAxis

Inherits or Implemented: [Axis](#Axis)

Radial axis of polar coordinate.


## RadiusAxisTheme

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## Ring

Inherits or Implemented: [Serie](#Serie)


## Scatter

Inherits or Implemented: [BaseScatter](#BaseScatter)


## SelectStyle

Inherits or Implemented: [StateStyle](#StateStyle),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

> Since `v3.2.0`

Configurations of select state.


## Serie

Inherits or Implemented: [BaseSerie](#BaseSerie),[IComparable](#IComparable)

系列。

|field|default|since|comment|
|--|--|--|--|
|index|||The index of serie.
|show|true||Whether to show serie in chart.
|coordSystem|||the chart coord system of serie.
|serieType|||the type of serie.
|serieName|||Series name used for displaying in tooltip and filtering with legend.
|state||v3.2.0|The default state of a serie.<br/>`SerieState`:<br/>- `Normal`: Normal state.<br/>- `Emphasis`: Emphasis state.<br/>- `Blur`: Blur state.<br/>- `Select`: Select state.<br/>- `Auto`: Auto state.<br/>|
|colorBy||v3.2.0|The policy to take color from theme.<br/>`SerieColorBy`:<br/>- `Default`: Select state.<br/>- `Serie`: assigns the colors in the palette by serie, so that all data in the same series are in the same color;.<br/>- `Data`: assigns colors in the palette according to data items, with each data item using a different color..<br/>|
|markColor||v3.4.0|Serie's mark color. It is only used to display Legend and Tooltip, and does not affect the drawing color. The default value is clear.
|stack|||If stack the value. On the same category axis, the series with the same stack name would be put on top of each other.
|xAxisIndex|0||the index of XAxis.
|yAxisIndex|0||the index of YAxis.
|radarIndex|0||Index of radar component that radar chart uses.
|vesselIndex|0||Index of vesel component that liquid chart uses.
|polarIndex|0||Index of polar component that serie uses.
|singleAxisIndex|0||Index of single axis component that serie uses.
|parallelIndex|0||Index of parallel coord component that serie uses.
|minShow|||The min number of data to show in chart.
|maxShow|||The max number of data to show in chart.
|maxCache|||The max number of serie data cache. The first data will be remove when the size of serie data is larger then maxCache.
|sampleDist|0||the min pixel dist of sample.
|sampleType|||the type of sample.<br/>`SampleType`:<br/>- `Peak`: Take a peak. When the average value of the filter point is greater than or equal to 'sampleAverage', take the maximum value; If you do it the other way around, you get the minimum.<br/>- `Average`: Take the average of the filter points.<br/>- `Max`: Take the maximum value of the filter point.<br/>- `Min`: Take the minimum value of the filter point.<br/>- `Sum`: Take the sum of the filter points.<br/>|
|sampleAverage|0||设定的采样平均值。当sampleType 为 Peak 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为0时会实时计算所有数据的平均值。
|lineType|||The type of line chart.<br/>`LineType`:<br/>- `Normal`: the normal line chart，<br/>- `Smooth`: the smooth line chart，<br/>- `StepStart`: step line.<br/>- `StepMiddle`: step line.<br/>- `StepEnd`: step line.<br/>|
|smoothLimit|true|v3.4.0|Whether to restrict the curve. When true, the curve between two continuous data of the same value is restricted to not exceed the data point, and is flat to the data point.
|barType|||柱形图类型。<br/>`BarType`:<br/>- `Normal`: normal bar.<br/>- `Zebra`: zebra bar.<br/>- `Capsule`: capsule bar.<br/>|
|barPercentStack|false||柱形图是否为百分比堆积。相同stack的serie只要有一个barPercentStack为true，则就显示成百分比堆叠柱状图。
|barWidth|0||The width of the bar. Adaptive when default 0.
|barGap|0.1f||The gap between bars between different series, is a percent value like '0.3f' , which means 30% of the bar width, can be set as a fixed value. Set barGap as '-1' can overlap bars that belong to different series, which is useful when making a series of bar be background. In a single coodinate system, this attribute is shared by multiple 'bar' series. This attribute should be set on the last 'bar' series in the coodinate system, then it will be adopted by all 'bar' series in the coordinate system.
|barZebraWidth|4f||斑马线的粗细。
|barZebraGap|2f||斑马线的间距。
|min|||最小值。
|max|||最大值。
|minSize|0f||数据最小值 min 映射的宽度。
|maxSize|1f||数据最大值 max 映射的宽度。
|startAngle|||起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|endAngle|||结束角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|minAngle|||The minimum angle of sector(0-360). It prevents some sector from being too small when value is small.
|clockwise|true||是否顺时针。
|roundCap|||是否开启圆弧效果。
|splitNumber|||刻度分割段数。最大可设置36。
|clickOffset|true||Whether offset when mouse click pie chart item.
|roseType|||Whether to show as Nightingale chart.<br/>`RoseType`:<br/>- `None`: Don't show as Nightingale chart.<br/>- `Radius`: Use central angle to show the percentage of data, radius to show data size.<br/>- `Area`: All the sectors will share the same central angle, the data size is shown only through radiuses.<br/>|
|gap|||gap of item.
|center|||the center of chart.
|radius|||the radius of chart.
|showDataDimension|||数据项里的数据维数。
|showDataName|||在Editor的inpsector上是否显示name参数
|clip|false||If clip the overflow on the coordinate system.
|ignore|false||是否开启忽略数据。当为 true 时，数据值为 ignoreValue 时不进行绘制。
|ignoreValue|0||忽略数据的默认值。当ignore为true才有效。
|ignoreLineBreak|false||忽略数据时折线是断开还是连接。默认false为连接。
|showAsPositiveNumber|false||Show negative number as positive number.
|large|true||是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。 开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。 缺点：优化后不能自定义设置单个数据项的样式，不能显示Label。
|largeThreshold|200||开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
|avoidLabelOverlap|false||在饼图且标签外部显示的情况下，是否启用防止标签重叠策略，默认关闭，在标签拥挤重叠的情况下会挪动各个标签的位置，防止标签间的重叠。
|radarType|||雷达图类型。<br/>`RadarType`:<br/>- `Multiple`: multiple radar.<br/>- `Single`: single radar.<br/>|
|placeHolder|false||占位模式。占位模式时，数据有效但不参与渲染和显示。
|dataSortType|||组件的数据排序。<br/>`SerieDataSortType`:<br/>- `None`: 按 data 的顺序<br/>- `Ascending`: 升序<br/>- `Descending`: 降序<br/>|
|orient|||组件的朝向。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|align|||组件水平方向对齐方式。<br/>`Align`:<br/>- `Center`: 对齐方式<br/>- `Left`: 对齐方式<br/>- `Right`: 对齐方式<br/>|
|left|||Distance between component and the left side of the container.
|right|||Distance between component and the right side of the container.
|top|||Distance between component and the top side of the container.
|bottom|||Distance between component and the bottom side of the container.
|insertDataToHead|||Whether to add new data at the head or at the end of the list.
|lineStyle|||The style of line. [LineStyle](#LineStyle)|
|symbol|||the symbol of serie data item. [SerieSymbol](#SerieSymbol)|
|animation|||The start animation. [AnimationStyle](#AnimationStyle)|
|itemStyle|||The style of data item. [ItemStyle](#ItemStyle)|
|data|||系列中的数据内容数组。SerieData可以设置1到n维数据。

## SerieData

Inherits or Implemented: [ChildComponent](#ChildComponent)

A data item of serie.

|field|default|since|comment|
|--|--|--|--|
|index|||the index of SerieData.
|name|||the name of data item.
|id|||the id of data.
|parentId|||the id of parent SerieData.
|ignore|||是否忽略数据。当为 true 时，数据不进行绘制。
|selected|||Whether the data item is selected.
|radius|||自定义半径。可用在饼图中自定义某个数据项的半径。
|state||v3.2.0|the state of serie data.<br/>`SerieState`:<br/>- `Normal`: Normal state.<br/>- `Emphasis`: Emphasis state.<br/>- `Blur`: Blur state.<br/>- `Select`: Select state.<br/>- `Auto`: Auto state.<br/>|
|data|||An arbitrary dimension data list of data item.

## SerieSymbol

Inherits or Implemented: [SymbolStyle](#SymbolStyle),[ISerieDataComponent](#ISerieDataComponent)

系列数据项的标记的图形

|field|default|since|comment|
|--|--|--|--|
|sizeType|||the type of symbol size.<br/>`SymbolSizeType`:<br/>- `Custom`: Specify constant for symbol size.<br/>- `FromData`: Specify the dataIndex and dataScale to calculate symbol size.<br/>- `Function`: Specify function for symbol size.<br/>|
|dataIndex|1||whitch data index is when the sizeType assined as FromData.
|dataScale|1||the scale of data when sizeType assined as FromData.
|sizeFunction|||the function of size when sizeType assined as Function.
|startIndex|||the index start to show symbol.
|interval|||the interval of show symbol.
|forceShowLast|false||whether to show the last symbol.
|repeat|false||图形是否重复。
|minSize|0f|v3.3.0|Minimum symbol size.
|maxSize|0f|v3.3.0|Maximum symbol size.

## SerieTheme

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|lineWidth|||the color of text.
|lineSymbolSize|||the symbol size of line serie.
|scatterSymbolSize|||the symbol size of scatter serie.
|pieTooltipExtraRadius|||the extra radius of pie when actived by tooltip.
|selectedRate|1.3f||the rate of symbol size of line or scatter serie.
|pieSelectedOffset|||the center offset of pie if selected.
|candlestickColor|Color32(235, 84, 84, 255)||K线图阳线（涨）填充色
|candlestickColor0|Color32(71, 178, 98, 255)||K线图阴线（跌）填充色
|candlestickBorderWidth|1||K线图边框宽度
|candlestickBorderColor|Color32(235, 84, 84, 255)||K线图阳线（跌）边框色
|candlestickBorderColor0|Color32(71, 178, 98, 255)||K线图阴线（跌）边框色

## Settings

Inherits or Implemented: [MainComponent](#MainComponent)

Global parameter setting component. The default value can be used in general, and can be adjusted when necessary.

|field|default|since|comment|
|--|--|--|--|
|show|true||
|maxPainter|10||max painter.
|reversePainter|false||Painter是否逆序。逆序时index大的serie最先绘制。
|basePainterMaterial|||Base Pointer 材质球，设置后会影响Axis等。
|seriePainterMaterial|||Serie Pointer 材质球，设置后会影响所有Serie。
|upperPainterMaterial|||Upper Pointer 材质球。
|topPainterMaterial|||Top Pointer 材质球。
|lineSmoothStyle|2.5f||Curve smoothing factor. By adjusting the smoothing coefficient, the curvature of the curve can be changed, and different curves with slightly different appearance can be obtained.
|lineSmoothness|2f||Smoothness of curve. The smaller the value, the smoother the curve, but the number of vertices will increase.
|lineSegmentDistance|3f||The partition distance of a line segment. A line in a normal line chart is made up of many segments, the number of which is determined by the change in value. The smaller the number of segments, the higher the number of vertices. When the area with gradient is filled, the larger the value, the worse the transition effect.
|cicleSmoothness|2f||the smoothess of cricle.
|legendIconLineWidth|2||the width of line serie legend.
|legendIconCornerRadius|||The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, upper right, bottom right and bottom left)).
|axisMaxSplitNumber|50|v3.1.0|the max splitnumber of axis.

## SimplifiedBar

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer),[ISimplifiedSerie](#ISimplifiedSerie)


## SimplifiedCandlestick

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer),[ISimplifiedSerie](#ISimplifiedSerie)


## SimplifiedLine

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer),[ISimplifiedSerie](#ISimplifiedSerie)


## SingleAxis

Inherits or Implemented: [Axis](#Axis),[IUpdateRuntimeData](#IUpdateRuntimeData)

Single axis.

|field|default|since|comment|
|--|--|--|--|
|orient|||Orientation of the axis. By default, it's 'Horizontal'. You can set it to be 'Vertical' to make a vertical axis.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|left|0.1f||Distance between component and the left side of the container.
|right|0.1f||Distance between component and the right side of the container.
|top|0f||Distance between component and the top side of the container.
|bottom|0.2f||Distance between component and the bottom side of the container.
|width|0||width of axis.
|height|50||height of axis.

## SingleAxisCoord

Inherits or Implemented: [CoordSystem](#CoordSystem)


## StageColor

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|percent|||结束位置百分比。
|color|||颜色。

## StateStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

> Since `v3.2.0`

the state style of serie.

|field|default|since|comment|
|--|--|--|--|
|show|true||是否启用高亮样式。
|label|||图形文本标签。 [LabelStyle](#LabelStyle)|
|labelLine|||图形文本引导线样式。 [LabelLine](#LabelLine)|
|itemStyle|||图形样式。 [ItemStyle](#ItemStyle)|
|lineStyle|||折线样式。 [LineStyle](#LineStyle)|
|areaStyle|||区域样式。 [AreaStyle](#AreaStyle)|
|symbol|||标记样式。 [SerieSymbol](#SerieSymbol)|

## SubTitleTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)


## SymbolStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

系列数据项的标记的图形

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the symbol is showed.
|type|||the type of symbol.<br/>`SymbolType`:<br/>- `None`: 不显示标记。<br/>- `Custom`: 自定义标记。<br/>- `Circle`: 圆形。<br/>- `EmptyCircle`: 空心圆。<br/>- `Rect`: 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。<br/>- `EmptyRect`: 空心正方形。<br/>- `Triangle`: 三角形。<br/>- `EmptyTriangle`: 空心三角形。<br/>- `Diamond`: 菱形。<br/>- `EmptyDiamond`: 空心菱形。<br/>- `Arrow`: 箭头。<br/>- `EmptyArrow`: 空心箭头。<br/>|
|size|0f||the size of symbol.
|gap|0||the gap of symbol and line segment.
|width|0f||图形的宽。
|height|0f||图形的高。
|offset|Vector2.zero||图形的偏移。
|image|||自定义的标记图形。
|imageType|||the fill type of image.
|color|||图形的颜色。

## TextLimit

Inherits or Implemented: [ChildComponent](#ChildComponent)

Text character limitation and adaptation component. When the length of the text exceeds the set length, it is cropped and suffixes are appended to the end.Only valid in the category axis.

|field|default|since|comment|
|--|--|--|--|
|enable|false||Whether to enable text limit.
|maxWidth|0||Set the maximum width. A default of 0 indicates automatic fetch; otherwise, custom.
|gap|1||White pixel distance at both ends.
|suffix|||Suffixes when the length exceeds.

## TextPadding

Inherits or Implemented: [Padding](#Padding)

Settings related to text.


## TextStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

Settings related to text.

|field|default|since|comment|
|--|--|--|--|
|show|true||Settings related to text.
|font|||the font of text. When `null`, the theme's font is used by default.
|autoWrap|false||是否自动换行。
|autoAlign|true||文本是否让系统自动选对齐方式。为false时才会用alignment。
|rotate|0||Rotation of text.
|autoColor|false||是否开启自动颜色。当开启时，会自动设置颜色。
|color|||the color of text.
|fontSize|0||font size.
|fontStyle|||font style.
|lineSpacing|1f||text line spacing.
|alignment|||对齐方式。
|tMPFont|||the font of textmeshpro.
|tMPFontStyle|||
|tMPAlignment|||
|tMPSpriteAsset||v3.1.0|

## Theme

Inherits or Implemented: [ScriptableObject](#ScriptableObject)

Theme.

|field|default|since|comment|
|--|--|--|--|
|themeType|||the theme of chart.<br/>`ThemeType`:<br/>- `Default`: 默认主题。<br/>- `Light`: 亮主题。<br/>- `Dark`: 暗主题。<br/>- `Custom`: 自定义主题。<br/>|
|themeName|||the name of theme.
|font|||the font of chart text。
|tMPFont|||the font of chart text。
|contrastColor|||the contrast color of chart.
|backgroundColor|||the background color of chart.
|colorPalette|||The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
|common||| [ComponentTheme](#ComponentTheme)|
|title||| [TitleTheme](#TitleTheme)|
|subTitle||| [SubTitleTheme](#SubTitleTheme)|
|legend||| [LegendTheme](#LegendTheme)|
|axis||| [AxisTheme](#AxisTheme)|
|tooltip||| [TooltipTheme](#TooltipTheme)|
|dataZoom||| [DataZoomTheme](#DataZoomTheme)|
|visualMap||| [VisualMapTheme](#VisualMapTheme)|
|serie||| [SerieTheme](#SerieTheme)|

## ThemeStyle

Inherits or Implemented: [ChildComponent](#ChildComponent)

Theme.

|field|default|since|comment|
|--|--|--|--|
|show|true||
|sharedTheme|||the asset of theme. [Theme](#Theme)|
|transparentBackground|false||Whether the background color is transparent. When true, the background color is not drawn.
|enableCustomTheme|false||Whether to customize theme colors. When set to true, you can use 'sync color to custom' to synchronize the theme color to the custom color. It can also be set manually.
|customFont|||
|customBackgroundColor|||the custom background color of chart.
|customColorPalette|||

## Title

Inherits or Implemented: [MainComponent](#MainComponent),[IPropertyChanged](#IPropertyChanged)

Title component, including main title and subtitle.

|field|default|since|comment|
|--|--|--|--|
|show|true||[default:true] Set this to false to prevent the title from showing.
|text|||The main title text, supporting \n for newlines.
|subText|||Subtitle text, supporting for \n for newlines.
|labelStyle|||The text style of main title. [LabelStyle](#LabelStyle)|
|subLabelStyle|||The text style of sub title. [LabelStyle](#LabelStyle)|
|itemGap|0||[default:8] The gap between the main title and subtitle.
|location|||The location of title component. [Location](#Location)|

## TitleStyle

Inherits or Implemented: [LabelStyle](#LabelStyle),[ISerieDataComponent](#ISerieDataComponent),[ISerieExtraComponent](#ISerieExtraComponent)

the title of serie.


## TitleTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)


## Tooltip

Inherits or Implemented: [MainComponent](#MainComponent)

Tooltip component.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the tooltip component.
|type|||Indicator type.<br/>`Tooltip.Type`:<br/>- `Line`: line indicator.<br/>- `Shadow`: shadow crosshair indicator.<br/>- `None`: no indicator displayed.<br/>- `Corss`: crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.<br/>|
|trigger|||Type of triggering.<br/>`Tooltip.Trigger`:<br/>- `Item`: Triggered by data item, which is mainly used for charts that don't have a category axis like scatter charts or pie charts.<br/>- `Axis`: Triggered by axes, which is mainly used for charts that have category axes, like bar charts or line charts.<br/>- `None`: Trigger nothing.<br/>|
|position||v3.3.0|Type of position.<br/>`Tooltip.Position`:<br/>- `Auto`: Auto. The mobile platform is displayed at the top, and the non-mobile platform follows the mouse position.<br/>- `Custom`: Custom. Fully customize display position (x,y).<br/>- `FixedX`: Just fix the coordinate X. Y follows the mouse position.<br/>- `FixedY`: <br/>|
|itemFormatter|||a string template formatter for a single Serie or data item content. Support for wrapping lines with \n. Template variables are {.}, {a}, {b}, {c}, {d}.<br/> {.} is the dot of the corresponding color of a Serie that is currently indicated or whose index is 0.<br/> {a} is the series name of the serie that is currently indicated or whose index is 0.<br/> {b} is the name of the data item serieData that is currently indicated or whose index is 0, or a category value (such as the X-axis of a line chart).<br/> {c} is the value of a Y-dimension (dimesion is 1) from a Serie that is currently indicated or whose index is 0.<br/> {d} is the percentage value of Y-dimensions (dimesion is 1) from serie that is currently indicated or whose index is 0, with no % sign.<br/> {e} is the name of the data item serieData that is currently indicated or whose index is 0.<br/> {f} is sum of data.<br/> {.1} represents a dot from serie corresponding color that specifies index as 1.<br/> 1 in {a1}, {b1}, {c1} represents a serie that specifies an index of 1.<br/> {c1:2} represents the third data from serie's current indication data item indexed to 1 (a data item has multiple data, index 2 represents the third data).<br/> {c1:2-2} represents the third data item from serie's third data item indexed to 1 (i.e., which data item must be specified to specify).<br/> {d1:2: F2} indicates that a formatted string with a value specified separately is F2 (numericFormatter is used when numericFormatter is not specified).<br/> {d:0.##} indicates that a formatted string with a value specified separately is 0.##   (used for percentage, reserved 2 valid digits while avoiding the situation similar to "100.00%" when using f2 ).<br/> Example: "{a}, {c}", "{a1}, {c1: f1}", "{a1}, {c1:0: f1}", "{a1} : {c1:1-1: f1}"<br/>
|titleFormatter|||The string template formatter for the tooltip title content. Support for wrapping lines with \n. The placeholder {I} can be set separately to indicate that the title is ignored and not displayed. Template see itemFormatter.
|marker|||the marker of serie.
|fixedWidth|0||Fixed width. Higher priority than minWidth.
|fixedHeight|0||Fixed height. Higher priority than minHeight.
|minWidth|0||Minimum width. If fixedWidth has a value, get fixedWidth first.
|minHeight|0||Minimum height. If fixedHeight has a value, take priority over fixedHeight.
|numericFormatter|||Standard numeric format string. Used to format numeric values to display as strings. Using 'Axx' form: 'A' is the single character of the format specifier, supporting 'C' currency, 'D' decimal, 'E' exponent, 'F' number of vertices, 'G' regular, 'N' digits, 'P' percentage, 'R' round tripping, 'X' hex etc. 'XX' is the precision specification, from '0' - '99'.
|paddingLeftRight|10||the text padding of left and right. defaut:5.
|paddingTopBottom|10||the text padding of top and bottom. defaut:5.
|ignoreDataShow|false||Whether to show ignored data on tooltip.
|ignoreDataDefaultContent|||The default display character information for ignored data.
|showContent|true||Whether to show the tooltip floating layer, whose default value is true. It should be configurated to be false, if you only need tooltip to trigger the event or show the axisPointer without content.
|alwayShowContent|false||Whether to trigger after always display.
|offset|Vector2(18f, -25f)||The position offset of tooltip relative to the mouse position.
|backgroundImage|||The background image of tooltip.
|backgroundType|||The background type of tooltip.
|backgroundColor|||The background color of tooltip.
|borderWidth|2f||the width of tooltip border.
|fixedX|0f||the x positionn of fixedX.
|fixedY|0.7f||the y position of fixedY.
|titleHeight|25f||height of title text.
|itemHeight|25f||height of content text.
|borderColor|Color32(230, 230, 230, 255)||the color of tooltip border.
|lineStyle|||the line style of indicator line. [LineStyle](#LineStyle)|
|titleLabelStyle|||the textstyle of title. [LabelStyle](#LabelStyle)|
|contentLabelStyles|||the textstyle list of content.

## TooltipTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|since|comment|
|--|--|--|--|
|lineType|||the type of line.<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|lineWidth|1f||the width of line.
|lineColor|||the color of line.
|areaColor|||the color of line.
|labelTextColor|||the text color of tooltip cross indicator's axis label.
|labelBackgroundColor|||the background color of tooltip cross indicator's axis label.

## VisualMap

Inherits or Implemented: [MainComponent](#MainComponent)

VisualMap component. Mapping data to visual elements such as colors.

|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to enable components.
|showUI|false||Whether to display components. If set to false, it will not show up, but the data mapping function still exists.
|type|||the type of visualmap component.<br/>`VisualMap.Type`:<br/>- `Continuous`: 连续型。<br/>- `Piecewise`: 分段型。<br/>|
|selectedMode|||the selected mode for Piecewise visualMap.<br/>`VisualMap.SelectedMode`:<br/>- `Multiple`: 多选。<br/>- `Single`: 单选。<br/>|
|serieIndex|0||the serie index of visualMap.
|min|0||范围最小值
|max|0||范围最大值
|range|||Specifies the position of the numeric value corresponding to the handle. Range should be within the range of [min,max].
|text|||Text on both ends.
|textGap|||The distance between the two text bodies.
|splitNumber|5||For continuous data, it is automatically evenly divided into several segments and automatically matches the size of inRange color list when the default is 0.
|calculable|false||Whether the handle used for dragging is displayed (the handle can be dragged to adjust the selected range).
|realtime|true||Whether to update in real time while dragging.
|itemWidth|20f||The width of the figure, that is, the width of the color bar.
|itemHeight|140f||The height of the figure, that is, the height of the color bar.
|itemGap|10f||每个图元之间的间隔距离。
|borderWidth|0||Border line width.
|dimension|-1||Specifies "which dimension" of the data to map to the visual element. "Data" is series.data.
|hoverLink|true||When the hoverLink function is turned on, when the mouse hovers over the visualMap component, the corresponding value of the mouse position is highlighted in the corresponding graphic element in the diagram.
|autoMinMax|true||Automatically set min, Max value 自动设置min，max的值
|orient|||Specify whether the layout of component is horizontal or vertical.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|location|||The location of component. [Location](#Location)|
|workOnLine|true||Whether the visualmap is work on linestyle of linechart.
|workOnArea|false||Whether the visualmap is work on areaStyle of linechart.
|outOfRange|||Defines a visual color outside of the selected range.
|inRange|||分段式每一段的相关配置。

## VisualMapRange

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|since|comment|
|--|--|--|--|
|min|||范围最小值
|max|||范围最大值
|label|||文字描述
|color|||颜色

## VisualMapTheme

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|since|comment|
|--|--|--|--|
|borderWidth|||the width of border.
|borderColor|||the color of dataZoom border.
|backgroundColor|||the background color of visualmap.
|triangeLen|20f||可视化组件的调节三角形边长。

## XAxis

Inherits or Implemented: [Axis](#Axis)

The x axis in cartesian(rectangular) coordinate.


## XCResourcesImporter


## XCSettings

Inherits or Implemented: [ScriptableObject](#ScriptableObject)

|field|default|since|comment|
|--|--|--|--|
|lang||| [Lang](#Lang)|
|font|||
|tMPFont|||
|fontSizeLv1|28||一级字体大小。
|fontSizeLv2|24||
|fontSizeLv3|20||
|fontSizeLv4|18||
|axisLineType|||<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|axisLineWidth|0.8f||
|axisSplitLineType|||<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|axisSplitLineWidth|0.8f||
|axisTickWidth|0.8f||
|axisTickLength|5f||
|gaugeAxisLineWidth|15f||
|gaugeAxisSplitLineWidth|0.8f||
|gaugeAxisSplitLineLength|15f||
|gaugeAxisTickWidth|0.8f||
|gaugeAxisTickLength|5f||
|tootipLineWidth|0.8f||
|dataZoomBorderWidth|0.5f||
|dataZoomDataLineWidth|0.5f||
|visualMapBorderWidth|0f||
|serieLineWidth|1.8f||
|serieLineSymbolSize|5f||
|serieScatterSymbolSize|20f||
|serieSelectedRate|1.3f||
|serieCandlestickBorderWidth|1f||
|editorShowAllListData|false||
|maxPainter|10||
|lineSmoothStyle|3f||
|lineSmoothness|2f||
|lineSegmentDistance|3f||
|cicleSmoothness|2f||
|visualMapTriangeLen|20f||
|pieTooltipExtraRadius|8f||
|pieSelectedOffset|8f||
|customThemes|||

## YAxis

Inherits or Implemented: [Axis](#Axis)

The x axis in cartesian(rectangular) coordinate.


[XCharts Homepage](https://github.com/XCharts-Team/XCharts)<br/>
[XCharts API](XChartsAPI-EN.md)<br/>
[XCharts FAQ](XChartsFAQ-EN.md)
