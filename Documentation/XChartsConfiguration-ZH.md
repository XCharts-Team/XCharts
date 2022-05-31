# 配置项手册

[XCharts主页](https://github.com/XCharts-Team/XCharts)</br>
[XChartsAPI接口](XChartsAPI-ZH.md)</br>
[XCharts问答](XChartsFAQ-ZH.md)

## Serie 系列

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

## Theme 主题

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

## MainComponent 主组件

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

## ChildComponent 子组件

- [AngleAxisTheme](#AngleAxisTheme)
- [AnimationStyle](#AnimationStyle)
- [AreaStyle](#AreaStyle)
- [ArrowStyle](#ArrowStyle)
- [AxisLabel](#AxisLabel)
- [AxisLine](#AxisLine)
- [AxisName](#AxisName)
- [AxisSplitArea](#AxisSplitArea)
- [AxisSplitLine](#AxisSplitLine)
- [AxisTheme](#AxisTheme)
- [AxisTick](#AxisTick)
- [BaseAxisTheme](#BaseAxisTheme)
- [BaseLine](#BaseLine)
- [CommentItem](#CommentItem)
- [CommentMarkStyle](#CommentMarkStyle)
- [ComponentTheme](#ComponentTheme)
- [DataZoomTheme](#DataZoomTheme)
- [Emphasis](#Emphasis)
- [EmphasisItemStyle](#EmphasisItemStyle)
- [EmphasisLabelLine](#EmphasisLabelLine)
- [EmphasisLabelStyle](#EmphasisLabelStyle)
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
- [PolarAxisTheme](#PolarAxisTheme)
- [RadarAxisTheme](#RadarAxisTheme)
- [RadiusAxisTheme](#RadiusAxisTheme)
- [SerieData](#SerieData)
- [SerieSymbol](#SerieSymbol)
- [SerieTheme](#SerieTheme)
- [StageColor](#StageColor)
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

## ISerieExtraComponent Serie额外组件

- [AreaStyle](#AreaStyle)
- [Emphasis](#Emphasis)
- [EmphasisItemStyle](#EmphasisItemStyle)
- [EmphasisLabelLine](#EmphasisLabelLine)
- [EmphasisLabelStyle](#EmphasisLabelStyle)
- [ImageStyle](#ImageStyle)
- [LabelLine](#LabelLine)
- [LabelStyle](#LabelStyle)
- [LineArrow](#LineArrow)
- [TitleStyle](#TitleStyle)

## ISerieDataComponent SerieData额外组件

- [AreaStyle](#AreaStyle)
- [Emphasis](#Emphasis)
- [EmphasisItemStyle](#EmphasisItemStyle)
- [EmphasisLabelLine](#EmphasisLabelLine)
- [EmphasisLabelStyle](#EmphasisLabelStyle)
- [ImageStyle](#ImageStyle)
- [ItemStyle](#ItemStyle)
- [LabelLine](#LabelLine)
- [LabelStyle](#LabelStyle)
- [LineStyle](#LineStyle)
- [SerieSymbol](#SerieSymbol)
- [TitleStyle](#TitleStyle)

## Other 其他

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

## `AngleAxis`

Inherits or Implemented: [Axis](#Axis)

极坐标系的角度轴。

|field|default|comment|
|--|--|--|
| `startAngle` |0 | 起始刻度的角度，默认为 0 度，即圆心的正右方。 |

## `AngleAxisTheme`

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## `AnimationStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

动画表现。

|field|default|comment|
|--|--|--|
| `enable` |true | 是否开启动画效果。 |
| `type` | | 动画类型。</br>`AnimationType`:</br>- `Default`: 默认。内部会根据实际情况选择一种动画播放方式。</br>- `LeftToRight`: 从左往右播放动画。</br>- `BottomToTop`: 从下往上播放动画。</br>- `InsideOut`: 由内到外播放动画。</br>- `AlongPath`: 沿着路径播放动画。</br>- `Clockwise`: 顺时针播放动画。</br>|
| `easting` | | 动画的缓动效果。</br>`AnimationEasing`:</br>- `Linear`: </br>|
| `threshold` |2000 | 是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。 |
| `fadeInDuration` |1000 | 设定的渐入动画时长（毫秒）。如果要设置单个数据项的渐入时长，可以用代码定制：customFadeInDuration。 |
| `fadeInDelay` |0 | 渐入动画延时（毫秒）。如果要设置单个数据项的延时，可以用代码定制：customFadeInDelay。 |
| `fadeOutDuration` |1000f | 设定的渐出动画时长（毫秒）。如果要设置单个数据项的渐出时长，可以用代码定制：customFadeOutDuration。 |
| `fadeOutDelay` |0 | 渐出动画延时（毫秒）。如果要设置单个数据项的延时，可以用代码定制：customFadeOutDelay。 |
| `dataChangeEnable` |true | 是否开启数据变更动画。 |
| `dataChangeDuration` |500 | 数据变更的动画时长（毫秒）。 |
| `actualDuration` | | 实际的动画时长（毫秒）。 |
| `alongWithLinePath` | |  |

## `AreaStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

区域填充样式。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示区域填充。 |
| `origin` | | 区域填充的起始位置。</br>`AreaStyle.AreaOrigin`:</br>- `Auto`: 填充坐标轴轴线到数据间的区域。</br>- `Start`: 填充坐标轴底部到数据间的区域。</br>- `End`: 填充坐标轴顶部到数据间的区域。</br>|
| `color` | | 区域填充的颜色，如果toColor不是默认值，则表示渐变色的起点颜色。 |
| `toColor` | | 渐变色的终点颜色。 |
| `opacity` |0.6f | 图形透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。 |
| `highlightColor` | | 高亮时区域填充的颜色，如果highlightToColor不是默认值，则表示渐变色的起点颜色。 |
| `highlightToColor` | | 高亮时渐变色的终点颜色。 |

## `ArrowStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `width` |10 | 箭头宽。 |
| `height` |15 | 箭头高。 |
| `offset` |0 | 箭头偏移。 |
| `dent` |3 | 箭头的凹度。 |
| `color` |Color.clear | 箭头颜色。 |

## `Axis`

Inherits or Implemented: [MainComponent](#MainComponent)

直角坐标系的坐标轴组件。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示坐标轴。 |
| `type` | | 坐标轴类型。</br>`Axis.AxisType`:</br>- `Value`: </br>- `Category`: </br>- `Log`: 对数轴。适用于对数数据。</br>- `Time`: 时间轴。适用于连续的时序数据。</br>|
| `minMaxType` | | 坐标轴刻度最大最小值显示类型。</br>`Axis.AxisMinMaxType`:</br>- `Default`: 0-最大值。</br>- `MinMax`: 最小值-最大值。</br>- `Custom`: 自定义最小值最大值。</br>|
| `gridIndex` | | 坐标轴所在的 grid 的索引，默认位于第一个 grid。 |
| `polarIndex` | | 坐标轴所在的 ploar 的索引，默认位于第一个 polar。 |
| `parallelIndex` | | 坐标轴所在的 parallel 的索引，默认位于第一个 parallel。 |
| `position` | | 坐标轴在Grid中的位置。</br>`Axis.AxisPosition`:</br>- `Left`: 坐标轴在Grid中的位置</br>- `Right`: 坐标轴在Grid中的位置</br>- `Bottom`: 坐标轴在Grid中的位置</br>- `Top`: 坐标轴在Grid中的位置</br>|
| `offset` | | 坐标轴相对默认位置的偏移。在相同position有多个坐标轴时有用。 |
| `min` | | 设定的坐标轴刻度最小值，当minMaxType为Custom时有效。 |
| `max` | | 设定的坐标轴刻度最大值，当minMaxType为Custom时有效。 |
| `splitNumber` |0 | 坐标轴的期望的分割段数。默认为0表示自动分割。 |
| `interval` |0 | 强制设置坐标轴分割间隔。无法在类目轴中使用。 |
| `boundaryGap` |true |  |
| `maxCache` |0 | The first data will be remove when the size of axis data is larger then maxCache. |
| `logBase` |10 | 对数轴的底数，只在对数轴（type:'Log'）中有效。 |
| `logBaseE` |false | 对数轴是否以自然数 e 为底数，为 true 时 logBase 失效。 |
| `ceilRate` |0 | 最大最小值向上取整的倍率。默认为0时自动计算。 |
| `inverse` |false | 是否反向坐标轴。在类目轴中无效。 |
| `clockwise` |true | 刻度增长是否按顺时针，默认顺时针。 |
| `insertDataToHead` | | 添加新数据时是在列表的头部还是尾部加入。 |
| `icons` | | 类目数据对应的图标。 |
| `data` | | 类目数据，在类目轴（type: 'category'）中有效。 |
| `axisLine` | |  [AxisLine](AxisLine)|
| `axisName` | | 坐标轴名称。 [AxisName](AxisName)|
| `axisTick` | | 坐标轴刻度。 [AxisTick](AxisTick)|
| `axisLabel` | | 坐标轴刻度标签。 [AxisLabel](AxisLabel)|
| `splitLine` | | 坐标轴分割线。 [AxisSplitLine](AxisSplitLine)|
| `splitArea` | | 坐标轴分割区域。 [AxisSplitArea](AxisSplitArea)|

## `AxisLabel`

Inherits or Implemented: [LabelStyle](#LabelStyle)

坐标轴刻度标签的相关设置。

|field|default|comment|
|--|--|--|
| `interval` |0 | 坐标轴刻度标签的显示间隔，在类目轴中有效。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。 |
| `inside` |false | 刻度标签是否朝内，默认朝外。 |
| `showAsPositiveNumber` |false | 将负数数值显示为正数。一般和`Serie`的`showAsPositiveNumber`配合使用。 |
| `onZero` |false | 刻度标签显示在0刻度上。 |
| `showStartLabel` |true | 是否显示第一个文本。 |
| `showEndLabel` |true | 是否显示最后一个文本。 |
| `textLimit` | | 文本限制。 [TextLimit](TextLimit)|

## `AxisLine`

Inherits or Implemented: [BaseLine](#BaseLine)

坐标轴轴线。

|field|default|comment|
|--|--|--|
| `onZero` | | X 轴或者 Y 轴的轴线是否在另一个轴的 0 刻度上，只有在另一个轴为数值轴且包含 0 刻度时有效。 |
| `showArrow` | | 是否显示箭头。 |
| `arrow` | | 轴线箭头。 [ArrowStyle](ArrowStyle)|

## `AxisName`

Inherits or Implemented: [ChildComponent](#ChildComponent)

坐标轴名称。

|field|default|comment|
|--|--|--|
| `show` | | 是否显示坐标名称。 |
| `name` | | 坐标轴名称。 |
| `labelStyle` | | 文本样式。 [LabelStyle](LabelStyle)|

## `AxisSplitArea`

Inherits or Implemented: [ChildComponent](#ChildComponent)

坐标轴在 grid 区域中的分隔区域，默认不显示。

|field|default|comment|
|--|--|--|
| `show` | | 是否显示分隔区域。 |
| `color` | | Dark and light colors in turns are used by default. |

## `AxisSplitLine`

Inherits or Implemented: [BaseLine](#BaseLine)

坐标轴在 grid 区域中的分隔线。

|field|default|comment|
|--|--|--|
| `interval` | |  |
| `distance` | | 刻度线与轴线的距离。 |
| `autoColor` | |  |

## `AxisTheme`

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## `AxisTick`

Inherits or Implemented: [BaseLine](#BaseLine)

坐标轴刻度相关设置。

|field|default|comment|
|--|--|--|
| `alignWithLabel` | | 类目轴中在 boundaryGap 为 true 的时候有效，可以保证刻度线和标签对齐。 |
| `inside` | | 坐标轴刻度是否朝内，默认朝外。 |
| `showStartTick` | | 是否显示第一个刻度。 |
| `showEndTick` | | 是否显示最后一个刻度。 |
| `distance` | | 刻度线与轴线的距离。 |
| `splitNumber` |0 | 分隔线之间分割的刻度数。 |
| `autoColor` | |  |

## `Background`

Inherits or Implemented: [MainComponent](#MainComponent)

背景组件。

|field|default|comment|
|--|--|--|
| `show` |true | 是否启用背景组件。 |
| `image` | | 背景图。 |
| `imageType` | | 背景图填充类型。 |
| `imageColor` | | 背景图颜色。 |
| `hideThemeBackgroundColor` |true | 当background组件开启时，是否隐藏主题中设置的背景色。 |

## `Bar`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `BaseAxisTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|comment|
|--|--|--|
| `lineType` | | 坐标轴线类型。 |
| `lineWidth` |1f | 坐标轴线宽。 |
| `lineLength` |0f | 坐标轴线长。 |
| `lineColor` | | 坐标轴线颜色。 |
| `splitLineType` | | 分割线线类型。 |
| `splitLineWidth` |1f | 分割线线宽。 |
| `splitLineLength` |0f | 分割线线长。 |
| `splitLineColor` | | 分割线线颜色。 |
| `tickWidth` |1f | 刻度线线宽。 |
| `tickLength` |5f | 刻度线线长。 |
| `tickColor` | | 坐标轴线颜色。 |
| `splitAreaColors` | |  |

## `BaseLine`

Inherits or Implemented: [ChildComponent](#ChildComponent)

线条基础配置。

|field|default|comment|
|--|--|--|
| `show` | | 是否显示坐标轴轴线。 |
| `lineStyle` | | 线条样式 [LineStyle](LineStyle)|

## `BaseScatter`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `BaseSerie`


## `CalendarCoord`

Inherits or Implemented: [CoordSystem](#CoordSystem),[IUpdateRuntimeData](#IUpdateRuntimeData),[ISerieContainer](#ISerieContainer)


## `Candlestick`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `ChartText`


## `ChildComponent`


## `Comment`

Inherits or Implemented: [MainComponent](#MainComponent)

图表注解组件。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示注解组件。 |
| `labelStyle` | | 所有组件的文本样式。 [LabelStyle](LabelStyle)|
| `markStyle` | | 所有组件的文本样式。 [CommentMarkStyle](CommentMarkStyle)|
| `items` | |  |

## `CommentItem`

Inherits or Implemented: [ChildComponent](#ChildComponent)

注解项。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示当前注解项。 |
| `content` | | 注解的文本内容。 |
| `position` | | 注解项的位置坐标。 |
| `markRect` | |  |
| `markStyle` | |  [CommentMarkStyle](CommentMarkStyle)|
| `labelStyle` | | 注解项的文本样式。 [LabelStyle](LabelStyle)|

## `CommentMarkStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

注解项。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示当前注解项。 |
| `lineStyle` | |  [LineStyle](LineStyle)|

## `ComponentTheme`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `font` | | 字体。 |
| `textColor` | | 文本颜色。 |
| `textBackgroundColor` | | 文本颜色。 |
| `fontSize` |18 | 文本字体大小。 |
| `tMPFont` | | the font of chart text。 字体。 |

## `CoordSystem`

Inherits or Implemented: [MainComponent](#MainComponent)

坐标系系统。


## `DataZoom`

Inherits or Implemented: [MainComponent](#MainComponent),[IUpdateRuntimeData](#IUpdateRuntimeData)

DataZoom 组件 用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。

|field|default|comment|
|--|--|--|
| `enable` |true | 是否显示缩放区域。 |
| `filterMode` | | 数据过滤类型。</br>`DataZoom.FilterMode`:</br>- `Filter`: 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只要有一个维度在数据窗口外，整个数据项就会被过滤掉。</br>- `WeakFilter`: 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只有当全部维度都在数据窗口同侧外部，整个数据项才会被过滤掉。</br>- `Empty`: 当前数据窗口外的数据，被 设置为空。即 不会 影响其他轴的数据范围。</br>- `None`: 不过滤数据，只改变数轴范围。</br>|
| `xAxisIndexs` | | 控制的 x 轴索引列表。 |
| `yAxisIndexs` | | 控制的 y 轴索引列表。 |
| `supportInside` | | 是否支持内置。内置于坐标系中，使用户可以在坐标系上通过鼠标拖拽、鼠标滚轮、手指滑动（触屏上）来缩放或漫游坐标系。 |
| `supportInsideScroll` |true | 是否支持坐标系内滚动 |
| `supportInsideDrag` |true | 是否支持坐标系内拖拽 |
| `supportSlider` | | 是否支持滑动条。有单独的滑动条，用户在滑动条上进行缩放或漫游。 |
| `supportSelect` | | 是否支持框选。提供一个选框进行数据区域缩放。 |
| `showDataShadow` | | 是否显示数据阴影。数据阴影可以简单地反应数据走势。 |
| `showDetail` | | 是否显示detail，即拖拽时候显示详细数值信息。 |
| `zoomLock` | | 是否锁定选择区域（或叫做数据窗口）的大小。 如果设置为 true 则锁定选择区域的大小，也就是说，只能平移，不能缩放。 |
| `realtime` | |  |
| `fillerColor` | | 数据区域颜色。 |
| `borderColor` | | 边框颜色。 |
| `borderWidth` | | 边框宽。 |
| `backgroundColor` | | 组件的背景颜色。 |
| `left` | | 组件离容器左侧的距离。 |
| `right` | | 组件离容器右侧的距离。 |
| `top` | | 组件离容器上侧的距离。 |
| `bottom` | | 组件离容器下侧的距离。 |
| `rangeMode` | | 取绝对值还是百分比。</br>`DataZoom.RangeMode`:</br>- `//Value`: The value type of start and end.取值类型</br>- `Percent`: 百分比。</br>|
| `start` | | 数据窗口范围的起始百分比。范围是：0 ~ 100。 |
| `end` | | 数据窗口范围的结束百分比。范围是：0 ~ 100。 |
| `startValue` | |  |
| `endValue` | |  |
| `minShowNum` |1 | 最小显示数据个数。当DataZoom放大到最大时，最小显示的数据个数。 |
| `scrollSensitivity` |1.1f | 缩放区域组件的敏感度。值越高每次缩放所代表的数据越多。 |
| `orient` | | 布局方式是横还是竖。不仅是布局方式，对于直角坐标系而言，也决定了，缺省情况控制横向数轴还是纵向数轴。</br>`Orient`:</br>- `Horizonal`: 水平</br>- `Vertical`: 垂直</br>|
| `labelStyle` | | 文本标签格式。 [LabelStyle](LabelStyle)|
| `lineStyle` | | 阴影线条样式。 [LineStyle](LineStyle)|
| `areaStyle` | | 阴影填充样式。 [AreaStyle](AreaStyle)|

## `DataZoomTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|comment|
|--|--|--|
| `borderWidth` | | 边框线宽。 |
| `dataLineWidth` | | 数据阴影线宽。 |
| `fillerColor` | | 数据区域颜色。 |
| `borderColor` | | 边框颜色。 |
| `dataLineColor` | | 数据阴影的线条颜色。 |
| `dataAreaColor` | | 数据阴影的填充颜色。 |
| `backgroundColor` | | 背景颜色。 |

## `DebugInfo`

|field|default|comment|
|--|--|--|
| `show` |true |  |
| `showDebugInfo` |false |  |
| `showAllChartObject` |false |  |
| `foldSeries` |false |  |
| `labelStyle` | |  [LabelStyle](LabelStyle)|

## `EffectScatter`

Inherits or Implemented: [BaseScatter](#BaseScatter)


## `Emphasis`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

高亮的图形样式和文本标签样式。

|field|default|comment|
|--|--|--|
| `show` | | 是否启用高亮样式。 |
| `label` | | 图形文本标签。 [LabelStyle](LabelStyle)|
| `labelLine` | |  [LabelLine](LabelLine)|
| `itemStyle` | | 图形样式。 [ItemStyle](ItemStyle)|

## `EmphasisItemStyle`

Inherits or Implemented: [ItemStyle](#ItemStyle),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

高亮的图形样式


## `EmphasisLabelLine`

Inherits or Implemented: [LabelLine](#LabelLine),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

高亮的标签引导线样式


## `EmphasisLabelStyle`

Inherits or Implemented: [LabelStyle](#LabelStyle),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

高亮的标签样式


## `EndLabelStyle`

Inherits or Implemented: [LabelStyle](#LabelStyle)


## `GridCoord`

Inherits or Implemented: [CoordSystem](#CoordSystem),[IUpdateRuntimeData](#IUpdateRuntimeData),[ISerieContainer](#ISerieContainer)

Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示直角坐标系网格。 |
| `left` |0.1f | grid 组件离容器左侧的距离。 |
| `right` |0.08f | grid 组件离容器右侧的距离。 |
| `top` |0.22f | grid 组件离容器上侧的距离。 |
| `bottom` |0.12f | grid 组件离容器下侧的距离。 |
| `backgroundColor` | | 网格背景色，默认透明。 |
| `showBorder` |false | 是否显示网格边框。 |
| `borderWidth` |0f | 网格边框宽。 |
| `borderColor` | | 网格边框颜色。 |

## `Heatmap`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `IconStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `show` |false | 是否显示图标。 |
| `layer` | | 显示在上层还是在下层。</br>`IconStyle.Layer`:</br>- `UnderText`: The icon is display under the label text. 图标在标签文字下</br>- `AboveText`: The icon is display above the label text. 图标在标签文字上</br>|
| `align` | | 水平方向对齐方式。</br>`Align`:</br>- `Center`: 对齐方式</br>- `Left`: 对齐方式</br>- `Right`: 对齐方式</br>|
| `sprite` | | 图标的图片。 |
| `type` | | 图片的显示类型。 |
| `color` | | 图标颜色。 |
| `width` |20 | 图标宽。 |
| `height` |20 | 图标高。 |
| `offset` | | 图标偏移。 |
| `autoHideWhenLabelEmpty` |false | 当label内容为空时是否自动隐藏图标 |

## `ImageStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示图标。 |
| `sprite` | | 图标的图片。 |
| `type` | | 图片的显示类型。 |
| `autoColor` | | 是否自动颜色。 |
| `color` | | 图标颜色。 |
| `width` |0 | 图标宽。 |
| `height` |0 | 图标高。 |

## `Indicator`

雷达图的指示器，用来指定雷达图中的多个变量（维度）。

|field|default|comment|
|--|--|--|
| `name` | |  |
| `max` | | 指示器的最大值，默认为 0 无限制。 |
| `min` | | 指示器的最小值，默认为 0 无限制。 |
| `range` | | 正常值范围。当数值不在这个范围时，会自动变更显示颜色。 |
| `show` | | 是否显示雷达坐标系组件。 |
| `shape` | | 雷达图绘制类型，支持 'Polygon' 和 'Circle'。</br>`RadarCoord.Shape`:</br>- `Polygon`: 雷达图绘制类型，支持 'Polygon' 和 'Circle'。</br>- `Circle`: 雷达图绘制类型，支持 'Polygon' 和 'Circle'。</br>|
| `radius` |100 | 雷达图的半径。 |
| `splitNumber` |5 | 指示器轴的分割段数。 |
| `center` | | 雷达图的中心点。数组的第一项是横坐标，第二项是纵坐标。 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。 |
| `axisLine` | | 轴线。 [AxisLine](AxisLine)|
| `axisName` | | 雷达图每个指示器名称的配置项。 [AxisName](AxisName)|
| `splitLine` | | 分割线。 [AxisSplitLine](AxisSplitLine)|
| `splitArea` | | 分割区域。 [AxisSplitArea](AxisSplitArea)|
| `indicator` |true | 是否显示指示器。 |
| `positionType` | | 显示位置类型。</br>`RadarCoord.PositionType`:</br>- `Vertice`: 显示在顶点处。</br>- `Between`: 显示在两者之间。</br>|
| `indicatorGap` |10 | 指示器和雷达的间距。 |
| `ceilRate` |0 | 最大最小值向上取整的倍率。默认为0时自动计算。 |
| `isAxisTooltip` | | 是否Tooltip显示轴线上的所有数据。 |
| `outRangeColor` |Color.red | 数值超出范围时显示的颜色。 |
| `connectCenter` |false | 数值是否连线到中心点。 |
| `lineGradient` |true | 数值线段是否需要渐变。 |
| `indicatorList` | | 指示器列表。 |

## `ItemStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieDataComponent](#ISerieDataComponent)

图形样式。

|field|default|comment|
|--|--|--|
| `show` |true | 是否启用。 |
| `color` | | 数据项颜色。 |
| `color0` | | 数据项颜色。 |
| `toColor` | | 渐变色的颜色1。 |
| `toColor2` | | 渐变色的颜色2。只在折线图中有效。 |
| `backgroundColor` | | 数据项背景颜色。 |
| `backgroundWidth` | | 数据项背景宽度。 |
| `centerColor` | | 中心区域颜色。 |
| `centerGap` | | 中心区域间隙。 |
| `borderWidth` |0 | 边框宽。 |
| `borderGap` |0 | 边框间隙。 |
| `borderColor` | | 边框的颜色。 |
| `borderColor0` | | 边框的颜色。 |
| `borderToColor` | | 边框的渐变色。 |
| `opacity` |1 | 透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。 |
| `itemMarker` | | 提示框单项的字符标志。用在Tooltip中。 |
| `itemFormatter` | | 提示框单项的字符串模版格式器。具体配置参考`Tooltip`的`formatter` |
| `numericFormatter` | | 标准数字格式字符串。用于将数值格式化显示为字符串。 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings |
| `cornerRadius` | | 圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。 |

## `LabelLine`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

标签的引导线

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示视觉引导线。 |
| `lineType` | | 视觉引导线类型。</br>`LineType`:</br>- `Normal`: 普通折线图。</br>- `Smooth`: 平滑曲线。</br>- `StepStart`: 阶梯线图：当前点。</br>- `StepMiddle`: 阶梯线图：当前点和下一个点的中间。</br>- `StepEnd`: 阶梯线图：下一个拐点。</br>|
| `lineColor` |ChartConst.clearColor32 | 视觉引导线颜色。默认和serie一致取自调色板。 |
| `lineAngle` |0 | 视觉引导线的固定角度。对折线和曲线有效。 |
| `lineWidth` |1.0f | 视觉引导线的宽度。 |
| `lineGap` |1.0f | 视觉引导线和容器的间距。 |
| `lineLength1` |25f | 视觉引导线第一段的长度。 |
| `lineLength2` |15f | 视觉引导线第二段的长度。 |
| `startSymbol` | | 起始点的图形标记。 [SymbolStyle](SymbolStyle)|
| `endSymbol` | | 结束点的图形标记。 [SymbolStyle](SymbolStyle)|

## `LabelStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent),[ISerieDataComponent](#ISerieDataComponent)

图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示文本标签。 |
| `Position` | |  |
| `autoOffset` |false | 是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。 |
| `offset` | | 距离图形元素的偏移 |
| `rotate` | | 文本的旋转。 |
| `distance` | | 距离轴线的距离。 |
| `formatter` | |  |
| `numericFormatter` | | 标准数字格式字符串。用于将数值格式化显示为字符串。 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings |
| `width` |0 | 标签的宽度。一般不用指定，不指定时则自动是文字的宽度。 |
| `height` |0 | 标签的高度。一般不用指定，不指定时则自动是文字的高度。 |
| `icon` | | 图标样式。 [IconStyle](IconStyle)|
| `background` | | 背景图样式。 [ImageStyle](ImageStyle)|
| `textPadding` | | 文本的边距。 [TextPadding](TextPadding)|
| `textStyle` | | 文本样式。 [TextStyle](TextStyle)|

## `Lang`

Inherits or Implemented: [ScriptableObject](#ScriptableObject)

国际化语言表。


## `LangCandlestick`


## `LangTime`


## `Legend`

Inherits or Implemented: [MainComponent](#MainComponent),[IPropertyChanged](#IPropertyChanged)

图例组件。 图例组件展现了不同系列的标记，颜色和名字。可以通过点击图例控制哪些系列不显示。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示图例组件。 |
| `iconType` | | 图例类型。 [default:Type.Auto]</br>`Painter.Type`:</br>- `Base`: </br>- `Serie`: </br>- `Top`: </br>|
| `selectedMode` | | 选择模式。控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。 [default:SelectedMode.Multiple]</br>`VisualMap.SelectedMode`:</br>- `Multiple`: 多选。</br>- `Single`: 单选。</br>|
| `orient` | | 布局方式是横还是竖。 [default:Orient.Horizonal]</br>`Orient`:</br>- `Horizonal`: 水平</br>- `Vertical`: 垂直</br>|
| `location` | | 图例显示的位置。 [default:Location.defaultTop] [Location](Location)|
| `itemWidth` |25.0f | 图例标记的图形宽度。 [default:24f] |
| `itemHeight` |12.0f | 图例标记的图形高度。 [default:12f] |
| `itemGap` |10f | 图例每项之间的间隔。横向布局时为水平间隔，纵向布局时为纵向间隔。 [default:10f] |
| `itemAutoColor` |true | 图例标记的图形是否自动匹配颜色。 [default:true] |
| `itemOpacity` |1 | 图例标记的图形的颜色透明度。 |
| `formatter` | |  |
| `numericFormatter` | | 标准数字格式字符串。用于将数值格式化显示为字符串。 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings |
| `labelStyle` | | 文本样式。 [LabelStyle](LabelStyle)|
| `data` | | If data is not specified, it will be auto collected from series. |
| `icons` | | 自定义的图例标记图形。 |
| `colors` | |  |

## `LegendTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|comment|
|--|--|--|
| `unableColor` | | 文本颜色。 |

## `Level`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `label` | |  [LabelStyle](LabelStyle)|
| `upperLabel` | |  [LabelStyle](LabelStyle)|
| `itemStyle` | |  [ItemStyle](ItemStyle)|

## `LevelStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `show` |false | 是否启用LevelStyle |
| `levels` | | 各层节点对应的配置。当enableLevels为true时生效，levels[0]对应的第一层的配置，levels[1]对应第二层，依次类推。当levels中没有对应层时用默认的设置。 |

## `Line`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `LineArrow`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieExtraComponent](#ISerieExtraComponent)

|field|default|comment|
|--|--|--|
| `show` | | 是否显示箭头。 |
| `position` | | 箭头位置。</br>`LabelStyle.Position`:</br>- `Default`: 标签的位置。</br>- `Outside`: 饼图扇区外侧，通过视觉引导线连到相应的扇区。</br>- `Inside`: 饼图扇区内部。</br>- `Center`: 在饼图中心位置。</br>- `Top`: 图形标志的顶部。</br>- `Bottom`: 图形标志的底部。</br>- `Left`: 图形标志的左边。</br>- `Right`: 图形标志的右边。</br>- `Start`: 线的起始点。</br>- `Middle`: 线的中点。</br>- `End`: 线的结束点。</br>|
| `arrow` | | 箭头。 [ArrowStyle](ArrowStyle)|

## `LineStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent),[ISerieDataComponent](#ISerieDataComponent)

线条样式。 注： 修改 lineStyle 中的颜色不会影响图例颜色，如果需要图例颜色和折线图颜色一致，需修改 itemStyle.color，线条颜色默认也会取该颜色。 toColor，toColor2可设置水平方向的渐变，如需要设置垂直方向的渐变，可使用VisualMap。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示线条。当作为子组件，它的父组件有参数控制是否显示时，改参数无效。 |
| `type` | | 线的类型。</br>`Painter.Type`:</br>- `Base`: </br>- `Serie`: </br>- `Top`: </br>|
| `color` | | 线的颜色。 |
| `toColor` | | 线的渐变颜色（需要水平方向渐变时）。 |
| `toColor2` | | 线的渐变颜色2（需要水平方向三个渐变色的渐变时）。 |
| `width` |0 |  |
| `length` |0 |  |
| `opacity` |1 | 线的透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。 |

## `Location`

Inherits or Implemented: [ChildComponent](#ChildComponent),[IPropertyChanged](#IPropertyChanged)

位置类型。通过Align快速设置大体位置，再通过left，right，top，bottom微调具体位置。

|field|default|comment|
|--|--|--|
| `align` | | 对齐方式。</br>`Align`:</br>- `Center`: 对齐方式</br>- `Left`: 对齐方式</br>- `Right`: 对齐方式</br>|
| `left` | | 离容器左侧的距离。 |
| `right` | | 离容器右侧的距离。 |
| `top` | | 离容器上侧的距离。 |
| `bottom` | | 离容器下侧的距离。 |

## `MainComponent`

Inherits or Implemented: [IComparable](#IComparable)


## `MarkArea`

Inherits or Implemented: [MainComponent](#MainComponent)

|field|default|comment|
|--|--|--|
| `show` |true |  |
| `text` | |  |
| `serieIndex` |0 |  |
| `start` | |  [MarkAreaData](MarkAreaData)|
| `end` | |  [MarkAreaData](MarkAreaData)|
| `itemStyle` | |  [ItemStyle](ItemStyle)|
| `label` | |  [LabelStyle](LabelStyle)|

## `MarkAreaData`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `type` | | 特殊的标域类型，用于标注最大值最小值等。</br>`MarkAreaType`:</br>- `None`: 标域类型</br>- `Min`: 最小值。</br>- `Max`: 最大值。</br>- `Average`: 平均值。</br>- `Median`: 中位数。</br>|
| `name` | |  |
| `dimension` |1 | 从哪个维度的数据计算最大最小值等。 |
| `xPosition` | | 相对原点的 x 坐标，单位像素。当type为None时有效。 |
| `yPosition` | | 相对原点的 y 坐标，单位像素。当type为None时有效。 |
| `xValue` | | X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。 |
| `yValue` | | Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。 |

## `MarkLine`

Inherits or Implemented: [MainComponent](#MainComponent)

图表标线。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示标线。 |
| `serieIndex` |0 |  |
| `animation` | | 标线的动画样式。 [AnimationStyle](AnimationStyle)|
| `data` | | 标线的数据列表。当数据项的group为0时，每个数据项表示一条标线；当group不为0时，相同group的两个数据项分别表 示标线的起始点和终止点来组成一条标线，此时标线的相关样式参数取起始点的参数。 |

## `MarkLineData`

Inherits or Implemented: [ChildComponent](#ChildComponent)

Data of marking line. 图表标线的数据。

|field|default|comment|
|--|--|--|
| `type` | | 特殊的标线类型，用于标注最大值最小值等。</br>`MarkLineType`:</br>- `None`: 标线类型</br>- `Min`: 最小值。</br>- `Max`: 最大值。</br>- `Average`: 平均值。</br>- `Median`: 中位数。</br>|
| `name` | |  |
| `dimension` |1 | 从哪个维度的数据计算最大最小值等。 |
| `xPosition` | | 相对原点的 x 坐标，单位像素。当type为None时有效。 |
| `yPosition` | | 相对原点的 y 坐标，单位像素。当type为None时有效。 |
| `xValue` | | X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。 |
| `yValue` | | Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。 |
| `group` |0 | 分组。当group不为0时，表示这个data是标线的起点或终点，group一致的data组成一条标线。 |
| `zeroPosition` |false | 是否为坐标系原点。 |
| `startSymbol` | | 起始点的图形标记。 [SymbolStyle](SymbolStyle)|
| `endSymbol` | | 结束点的图形标记。 [SymbolStyle](SymbolStyle)|
| `lineStyle` | | 标线样式。 [LineStyle](LineStyle)|
| `label` | | 文本样式。可设置position为Start、Middle和End在不同的位置显示文本。 [LabelStyle](LabelStyle)|
| `emphasis` | |  [Emphasis](Emphasis)|

## `Parallel`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `ParallelAxis`

Inherits or Implemented: [Axis](#Axis)


## `ParallelCoord`

Inherits or Implemented: [CoordSystem](#CoordSystem),[IUpdateRuntimeData](#IUpdateRuntimeData),[ISerieContainer](#ISerieContainer)

Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示直角坐标系网格。 |
| `orient` | | 坐标轴朝向。默认为垂直朝向。</br>`Orient`:</br>- `Horizonal`: 水平</br>- `Vertical`: 垂直</br>|
| `left` |0.1f | grid 组件离容器左侧的距离。 |
| `right` |0.08f | grid 组件离容器右侧的距离。 |
| `top` |0.22f | grid 组件离容器上侧的距离。 |
| `bottom` |0.12f | grid 组件离容器下侧的距离。 |
| `backgroundColor` | | 网格背景色，默认透明。 |

## `Pie`

Inherits or Implemented: [Serie](#Serie)


## `PolarAxisTheme`

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## `PolarCoord`

Inherits or Implemented: [CoordSystem](#CoordSystem),[ISerieContainer](#ISerieContainer)

极坐标系组件。 极坐标系，可以用于散点图和折线图。每个极坐标系拥有一个角度轴和一个半径轴。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示极坐标。 |
| `center` | | 极坐标的中心点。数组的第一项是横坐标，第二项是纵坐标。 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。 |
| `radius` |0.35f | 极坐标的半径。 |
| `backgroundColor` | | 极坐标的背景色，默认透明。 |

## `Radar`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer)


## `RadarAxisTheme`

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## `RadarCoord`

Inherits or Implemented: [CoordSystem](#CoordSystem),[ISerieContainer](#ISerieContainer)

Radar coordinate conponnet for radar charts. 雷达图坐标系组件，只适用于雷达图。


## `RadiusAxis`

Inherits or Implemented: [Axis](#Axis)

极坐标系的径向轴。


## `RadiusAxisTheme`

Inherits or Implemented: [BaseAxisTheme](#BaseAxisTheme)


## `Ring`

Inherits or Implemented: [Serie](#Serie)


## `Scatter`

Inherits or Implemented: [BaseScatter](#BaseScatter)


## `Serie`

Inherits or Implemented: [BaseSerie](#BaseSerie),[IComparable](#IComparable)

系列。

|field|default|comment|
|--|--|--|
| `labels` | |  |
| `labelLines` | |  |
| `endLabels` | |  |
| `lineArrows` | |  |
| `areaStyles` | |  |
| `titleStyles` | |  |
| `emphasisItemStyles` | |  |
| `emphasisLabels` | |  |
| `emphasisLabelLines` | |  |
| `index` | | 系列索引。 |
| `show` |true | 系列是否显示在图表上。 |
| `coordSystem` | | 使用的坐标系。 |
| `serieType` | | 系列类型。 |
| `serieName` | | 系列名称，用于 tooltip 的显示，legend 的图例筛选。 |
| `stack` | | 数据堆叠，同个类目轴上系列配置相同的stack值后，后一个系列的值会在前一个系列的值上相加。 |
| `xAxisIndex` |0 | 使用X轴的index。 |
| `yAxisIndex` |0 | 使用Y轴的index。 |
| `radarIndex` |0 | 雷达图所使用的 radar 组件的 index。 |
| `vesselIndex` |0 | 水位图所使用的 vessel 组件的 index。 |
| `polarIndex` |0 | 所使用的 polar 组件的 index。 |
| `singleAxisIndex` |0 | 所使用的 singleAxis 组件的 index。 |
| `parallelIndex` |0 | 所使用的 parallel coord 组件的 index。 |
| `minShow` | | 系列所显示数据的最小索引 |
| `maxShow` | | 系列所显示数据的最大索引 |
| `maxCache` | | The first data will be remove when the size of serie data is larger then maxCache. |
| `sampleDist` |0 | 采样的最小像素距离，默认为0时不采样。当两个数据点间的水平距离小于改值时，开启采样，保证两点间的水平距离不小于改值。 |
| `sampleType` | | 采样类型。当sampleDist大于0时有效。</br>`SampleType`:</br>- `Peak`: 取峰值。</br>- `Average`: 取过滤点的平均值。</br>- `Max`: 取过滤点的最大值。</br>- `Min`: 取过滤点的最小值。</br>- `Sum`: 取过滤点的和。</br>|
| `sampleAverage` |0 | 设定的采样平均值。当sampleType 为 Peak 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为0时会实时计算所有数据的平均值。 |
| `lineType` | | 折线图样式类型。</br>`LineType`:</br>- `Normal`: 普通折线图。</br>- `Smooth`: 平滑曲线。</br>- `StepStart`: 阶梯线图：当前点。</br>- `StepMiddle`: 阶梯线图：当前点和下一个点的中间。</br>- `StepEnd`: 阶梯线图：下一个拐点。</br>|
| `barType` | | 柱形图类型。</br>`BarType`:</br>- `Normal`: 普通柱形图。</br>- `Zebra`: 斑马柱形图。</br>- `Capsule`: 胶囊柱形图。</br>|
| `barPercentStack` |false | 柱形图是否为百分比堆积。相同stack的serie只要有一个barPercentStack为true，则就显示成百分比堆叠柱状图。 |
| `barWidth` |0 | 柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。 |
| `barGap` |0.1f | <para>Set barGap as '-1' can overlap bars that belong to different series, which is useful when making a series of bar be background. |
| `barZebraWidth` |4f | 斑马线的粗细。 |
| `barZebraGap` |2f | 斑马线的间距。 |
| `min` | | 最小值。 |
| `max` | | 最大值。 |
| `minSize` |0f | 数据最小值 min 映射的宽度。 |
| `maxSize` |1f | 数据最大值 max 映射的宽度。 |
| `startAngle` | | 起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。 |
| `endAngle` | | 结束角度。和时钟一样，12点钟位置是0度，顺时针到360度。 |
| `minAngle` | | 最小的扇区角度（0-360）。用于防止某个值过小导致扇区太小影响交互。 |
| `clockwise` |true | 是否顺时针。 |
| `roundCap` | | 是否开启圆弧效果。 |
| `splitNumber` | | 刻度分割段数。最大可设置36。 |
| `clickOffset` |true | 鼠标点击时是否开启偏移，一般用在PieChart图表中。 |
| `roseType` | | 是否展示成南丁格尔图，通过半径区分数据大小。</br>`RoseType`:</br>- `None`: 不展示成南丁格尔玫瑰图。</br>- `Radius`: 扇区圆心角展现数据的百分比，半径展现数据的大小。</br>- `Area`: 所有扇区圆心角相同，仅通过半径展现数据大小。</br>|
| `gap` | | 间距。 |
| `center` | | 中心点。 |
| `radius` | | 半径。radius[0]表示内径，radius[1]表示外径。 |
| `showDataDimension` | | 数据项里的数据维数。 |
| `showDataName` | | 在Editor的inpsector上是否显示name参数 |
| `showDataIcon` | |  |
| `clip` |false | 是否裁剪超出坐标系部分的图形。 |
| `ignore` |false | 是否开启忽略数据。当为 true 时，数据值为 ignoreValue 时不进行绘制。 |
| `ignoreValue` |0 | 忽略数据的默认值。当ignore为true才有效。 |
| `ignoreLineBreak` |false | 忽略数据时折线是断开还是连接。默认false为连接。 |
| `showAsPositiveNumber` |false | 将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。 |
| `large` |true | 是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。 开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。 缺点：优化后不能自定义设置单个数据项的样式，不能显示Label。 |
| `largeThreshold` |200 | 开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。 |
| `avoidLabelOverlap` |false | 在饼图且标签外部显示的情况下，是否启用防止标签重叠策略，默认关闭，在标签拥挤重叠的情况下会挪动各个标签的位置，防止标签间的重叠。 |
| `radarType` | | 雷达图类型。</br>`RadarType`:</br>- `Multiple`: 多圈雷达图。此时可一个雷达里绘制多个圈，一个serieData就可组成一个圈（多维数据）。</br>- `Single`: 单圈雷达图。此时一个雷达只能绘制一个圈，多个serieData组成一个圈，数据取自`data[1]`。</br>|
| `placeHolder` |false | 占位模式。占位模式时，数据有效但不参与渲染和显示。 |
| `dataSortType` | | 组件的数据排序。</br>`SerieDataSortType`:</br>- `None`: 按 data 的顺序</br>- `Ascending`: 升序</br>- `Descending`: 降序</br>|
| `orient` | | 组件的朝向。</br>`Orient`:</br>- `Horizonal`: 水平</br>- `Vertical`: 垂直</br>|
| `align` | | 组件水平方向对齐方式。</br>`Align`:</br>- `Center`: 对齐方式</br>- `Left`: 对齐方式</br>- `Right`: 对齐方式</br>|
| `left` | | 组件离容器左侧的距离。 |
| `right` | | 组件离容器右侧的距离。 |
| `top` | | 组件离容器上侧的距离。 |
| `bottom` | | 组件离容器下侧的距离。 |
| `insertDataToHead` | | 添加新数据时是在列表的头部还是尾部加入。 |
| `lineStyle` | | 线条样式。 [LineStyle](LineStyle)|
| `symbol` | | 标记的图形。 [SerieSymbol](SerieSymbol)|
| `animation` | | 起始动画。 [AnimationStyle](AnimationStyle)|
| `itemStyle` | | 图形样式。 [ItemStyle](ItemStyle)|
| `data` | | 系列中的数据内容数组。SerieData可以设置1到n维数据。 |

## `SerieData`

Inherits or Implemented: [ChildComponent](#ChildComponent)

系列中的一个数据项。可存储数据名和1-n维个数据。

|field|default|comment|
|--|--|--|
| `index` | |  |
| `name` | | 数据项名称。 |
| `id` | | 数据项的唯一id。唯一id不是必须设置的。 |
| `parentId` | |  |
| `ignore` | | 是否忽略数据。当为 true 时，数据不进行绘制。 |
| `selected` | | 该数据项是否被选中。 |
| `radius` | | 自定义半径。可用在饼图中自定义某个数据项的半径。 |
| `itemStyles` | |  |
| `labels` | |  |
| `labelLines` | |  |
| `symbols` | |  |
| `lineStyles` | |  |
| `areaStyles` | |  |
| `titleStyles` | |  |
| `emphasisItemStyles` | |  |
| `emphasisLabels` | |  |
| `emphasisLabelLines` | |  |
| `data` | | 可指定任意维数的数值列表。 |

## `SerieSymbol`

Inherits or Implemented: [SymbolStyle](#SymbolStyle),[ISerieDataComponent](#ISerieDataComponent)

系列数据项的标记的图形

|field|default|comment|
|--|--|--|
| `sizeType` | | 标记图形的大小获取方式。</br>`SymbolSizeType`:</br>- `Custom`: 自定义大小。</br>- `FromData`: 通过 dataIndex 从数据中获取，再乘以一个比例系数 dataScale 。</br>- `Function`: 通过委托函数获取。</br>|
| `selectedSize` |0f | 被选中的标记的大小。 |
| `dataIndex` |1 | 当sizeType指定为FromData时，指定的数据源索引。 |
| `dataScale` |1 | 当sizeType指定为FromData时，指定的倍数系数。 |
| `selectedDataScale` |1.5f | 当sizeType指定为FromData时，指定的高亮倍数系数。 |
| `sizeFunction` | | 当sizeType指定为Function时，指定的委托函数。 |
| `selectedSizeFunction` | | 当sizeType指定为Function时，指定的高亮委托函数。 |
| `startIndex` | | 开始显示图形标记的索引。 |
| `interval` | | 显示图形标记的间隔。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。 |
| `forceShowLast` |false | 是否强制显示最后一个图形标记。 |
| `repeat` |false | 图形是否重复。 |

## `SerieTheme`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `lineWidth` | | 文本颜色。 |
| `lineSymbolSize` | |  |
| `scatterSymbolSize` | |  |
| `pieTooltipExtraRadius` | | 饼图鼠标移到高亮时的额外半径 |
| `selectedRate` |1.3f |  |
| `pieSelectedOffset` | | 饼图选中时的中心点偏移 |
| `candlestickColor` |Color32(235, 84, 84, 255) | K线图阳线（涨）填充色 |
| `candlestickColor0` |Color32(71, 178, 98, 255) | K线图阴线（跌）填充色 |
| `candlestickBorderWidth` |1 | K线图边框宽度 |
| `candlestickBorderColor` |Color32(235, 84, 84, 255) | K线图阳线（跌）边框色 |
| `candlestickBorderColor0` |Color32(71, 178, 98, 255) | K线图阴线（跌）边框色 |

## `Settings`

Inherits or Implemented: [MainComponent](#MainComponent)

全局参数设置组件。一般情况下可使用默认值，当有需要时可进行调整。

|field|default|comment|
|--|--|--|
| `show` |true |  |
| `maxPainter` |10 | 设定的painter数量。 |
| `reversePainter` |false | Painter是否逆序。逆序时index大的serie最先绘制。 |
| `basePainterMaterial` | | Base Pointer 材质球，设置后会影响Axis等。 |
| `seriePainterMaterial` | | Serie Pointer 材质球，设置后会影响所有Serie。 |
| `topPainterMaterial` | | Top Pointer 材质球，设置后会影响Tooltip等。 |
| `lineSmoothStyle` |3f | 曲线平滑系数。通过调整平滑系数可以改变曲线的曲率，得到外观稍微有变化的不同曲线。 |
| `lineSmoothness` |2f | When the area with gradient is filled, the larger the value, the worse the transition effect. |
| `lineSegmentDistance` |3f | 线段的分割距离。普通折线图的线是由很多线段组成，段数由该数值决定。值越小段数越多，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。 |
| `cicleSmoothness` |2f | 圆形的平滑度。数越小圆越平滑，但顶点数也会随之增加。 |
| `legendIconLineWidth` |2 | Line类型图例图标的线条宽度。 |
| `legendIconCornerRadius` | | 图例圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。 |

## `SimplifiedBar`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer),[ISimplifiedSerie](#ISimplifiedSerie)


## `SimplifiedCandlestick`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer),[ISimplifiedSerie](#ISimplifiedSerie)


## `SimplifiedLine`

Inherits or Implemented: [Serie](#Serie),[INeedSerieContainer](#INeedSerieContainer),[ISimplifiedSerie](#ISimplifiedSerie)


## `SingleAxis`

Inherits or Implemented: [Axis](#Axis),[IUpdateRuntimeData](#IUpdateRuntimeData)

单轴。

|field|default|comment|
|--|--|--|
| `orient` | | 坐标轴朝向。默认为水平朝向。</br>`Orient`:</br>- `Horizonal`: 水平</br>- `Vertical`: 垂直</br>|
| `left` |0.1f | 组件离容器左侧的距离。 |
| `right` |0.1f | 组件离容器右侧的距离。 |
| `top` |0f | 组件离容器上侧的距离。 |
| `bottom` |0.2f | 组件离容器下侧的距离。 |
| `width` |0 |  |
| `height` |50 |  |

## `SingleAxisCoord`

Inherits or Implemented: [CoordSystem](#CoordSystem)


## `StageColor`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `percent` | | 结束位置百分比。 |
| `color` | | 颜色。 |

## `SubTitleTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)


## `SymbolStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

系列数据项的标记的图形

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示标记。 |
| `type` | | 标记类型。</br>`SymbolType`:</br>- `None`: 不显示标记。</br>- `Custom`: 自定义标记。</br>- `Circle`: 圆形。</br>- `EmptyCircle`: 空心圆。</br>- `Rect`: 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。</br>- `EmptyRect`: 空心正方形。</br>- `Triangle`: 三角形。</br>- `EmptyTriangle`: 空心三角形。</br>- `Diamond`: 菱形。</br>- `EmptyDiamond`: 空心菱形。</br>- `Arrow`: 箭头。</br>- `EmptyArrow`: 空心箭头。</br>|
| `size` |0f | 标记的大小。 |
| `gap` |0 | 图形标记和线条的间隙距离。 |
| `width` |0f | 图形的宽。 |
| `height` |0f | 图形的高。 |
| `offset` |Vector2.zero | 图形的偏移。 |
| `image` | | 自定义的标记图形。 |
| `imageType` | |  |
| `color` | | 图形的颜色。 |

## `TextLimit`

Inherits or Implemented: [ChildComponent](#ChildComponent)

文本字符限制和自适应。当文本长度超过设定的长度时进行裁剪，并将后缀附加在最后。 只在类目轴中有效。

|field|default|comment|
|--|--|--|
| `enable` |false | 是否启用文本自适应。 [default:true] |
| `maxWidth` |0 | Clipping occurs when the width of the text is greater than this value. |
| `gap` |1 | 两边留白像素距离。 [default:10f] |
| `suffix` | | 长度超出时的后缀。 [default: "..."] |

## `TextPadding`

Inherits or Implemented: [ChildComponent](#ChildComponent)

文本的内边距设置。

|field|default|comment|
|--|--|--|
| `show` |true |  |
| `top` |2 |  |
| `right` |4 |  |
| `left` |4 |  |
| `bottom` |2 |  |

## `TextStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

文本的相关设置。

|field|default|comment|
|--|--|--|
| `show` |true | 文本的相关设置。 |
| `font` | | 文本字体。 [default: null] |
| `autoWrap` |false | 是否自动换行。 |
| `autoAlign` |true | 文本是否让系统自动选对齐方式。为false时才会用alignment。 |
| `rotate` |0 | 文本的旋转。 [default: `0f`] |
| `autoColor` |false | 是否开启自动颜色。当开启时，会自动设置颜色。 |
| `color` | | 文本的颜色。 [default: `Color.clear`] |
| `fontSize` |0 | 文本字体大小。 [default: 18] |
| `fontStyle` | | 文本字体的风格。 [default: FontStyle.Normal] |
| `lineSpacing` |1f | 行间距。 [default: 1f] |
| `alignment` | | 对齐方式。 |
| `tMPFont` | |  |
| `tMPFontStyle` | |  |
| `tMPAlignment` | |  |

## `Theme`

Inherits or Implemented: [ScriptableObject](#ScriptableObject)

主题相关配置。

|field|default|comment|
|--|--|--|
| `themeType` | | 主题类型。</br>`ThemeType`:</br>- `Default`: 默认主题。</br>- `Light`: 亮主题。</br>- `Dark`: 暗主题。</br>- `Custom`: 自定义主题。</br>|
| `themeName` | |  |
| `font` | | the font of chart text。 字体。 |
| `tMPFont` | | the font of chart text。 字体。 |
| `contrastColor` | | 对比色。 |
| `backgroundColor` | | 背景颜色。 |
| `colorPalette` | | 调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。 |
| `common` | |  [ComponentTheme](ComponentTheme)|
| `title` | |  [TitleTheme](TitleTheme)|
| `subTitle` | |  [SubTitleTheme](SubTitleTheme)|
| `legend` | |  [LegendTheme](LegendTheme)|
| `axis` | |  [AxisTheme](AxisTheme)|
| `tooltip` | |  [TooltipTheme](TooltipTheme)|
| `dataZoom` | |  [DataZoomTheme](DataZoomTheme)|
| `visualMap` | |  [VisualMapTheme](VisualMapTheme)|
| `serie` | |  [SerieTheme](SerieTheme)|

## `ThemeStyle`

Inherits or Implemented: [ChildComponent](#ChildComponent)

主题相关配置。

|field|default|comment|
|--|--|--|
| `show` |true |  |
| `sharedTheme` | |  [Theme](Theme)|
| `transparentBackground` |false | Whether the background color is transparent. When true, the background color is not drawn. ｜是否透明背景颜色。当设置为true时，不绘制背景颜色。 |
| `enableCustomTheme` |false | 是否自定义主题颜色。当设置为true时，可以用‘sync color to custom’同步主题的颜色到自定义颜色。也可以手动设置。 |
| `customFont` | |  |
| `customBackgroundColor` | | 自定义的背景颜色。 |
| `customColorPalette` | |  |

## `Title`

Inherits or Implemented: [MainComponent](#MainComponent),[IPropertyChanged](#IPropertyChanged)

标题组件，包含主标题和副标题。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示标题组件。 |
| `text` | | 主标题文本，支持使用 \n 换行。 |
| `subText` | | 副标题文本，支持使用 \n 换行。 |
| `labelStyle` | | 主标题文本样式。 [LabelStyle](LabelStyle)|
| `subLabelStyle` | | 副标题文本样式。 [LabelStyle](LabelStyle)|
| `itemGap` |0 | 主副标题之间的间距。 |
| `location` | | 标题显示位置。 [Location](Location)|

## `TitleStyle`

Inherits or Implemented: [LabelStyle](#LabelStyle),[ISerieDataComponent](#ISerieDataComponent),[ISerieExtraComponent](#ISerieExtraComponent)

标题相关设置。


## `TitleTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)


## `Tooltip`

Inherits or Implemented: [MainComponent](#MainComponent)

提示框组件。

|field|default|comment|
|--|--|--|
| `show` |true | 是否显示提示框组件。 |
| `type` | | 提示框指示器类型。</br>`Painter.Type`:</br>- `Base`: </br>- `Serie`: </br>- `Top`: </br>|
| `trigger` | | 触发类型。</br>`Tooltip.Trigger`:</br>- `Item`: 数据项图形触发，主要在散点图，饼图等无类目轴的图表中使用。</br>- `Axis`: 坐标轴触发，主要在柱状图，折线图等会使用类目轴的图表中使用。</br>- `None`: 什么都不触发。</br>|
| `itemFormatter` | | 提示框单个serie或数据项内容的字符串模版格式器。支持用 \n 换行。当formatter不为空时，优先使用formatter，否则使用itemFormatter。 |
| `titleFormatter` | |  |
| `marker` | | serie的符号标志。 |
| `fixedWidth` |0 | 固定宽度。比 minWidth 优先。 |
| `fixedHeight` |0 | 固定高度。比 minHeight 优先。 |
| `minWidth` |0 | 最小宽度。如若 fixedWidth 设有值，优先取 fixedWidth。 |
| `minHeight` |0 | 最小高度。如若 fixedHeight 设有值，优先取 fixedHeight。 |
| `numericFormatter` | | 标准数字格式字符串。用于将数值格式化显示为字符串。 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings |
| `paddingLeftRight` |10 | 左右边距。 |
| `paddingTopBottom` |10 | 上下边距。 |
| `ignoreDataShow` |false | 是否显示忽略数据在tooltip上。 |
| `ignoreDataDefaultContent` | | 被忽略数据的默认显示字符信息。 |
| `showContent` |true | 是否显示提示框浮层，默认显示。只需tooltip触发事件或显示axisPointer而不需要显示内容时可配置该项为false。 |
| `alwayShowContent` |false | 是否触发后一直显示提示框浮层。 |
| `offset` |Vector2(18f, -25f) | 提示框相对于鼠标位置的偏移。 |
| `backgroundImage` | | 提示框的背景图片。 |
| `backgroundType` | | 提示框的背景图片显示类型。 |
| `backgroundColor` | | 提示框的背景颜色。 |
| `borderWidth` |2f | 边框线宽。 |
| `fixedXEnable` |false |  |
| `fixedX` |0f |  |
| `fixedYEnable` |false |  |
| `fixedY` |0f |  |
| `titleHeight` |25f |  |
| `itemHeight` |25f |  |
| `borderColor` |Color32(230, 230, 230, 255) | 边框颜色。 |
| `lineStyle` | | 指示线样式。 [LineStyle](LineStyle)|
| `indicatorLabelStyle` | | 提示框的坐标轴指示器文本的样式。 [LabelStyle](LabelStyle)|
| `titleLabelStyle` | | 标题的文本样式。 [LabelStyle](LabelStyle)|
| `contentLabelStyles` | |  |

## `TooltipTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|comment|
|--|--|--|
| `lineType` | | 坐标轴线类型。 |
| `lineWidth` |1f | 指示线线宽。 |
| `lineColor` | | 指示线颜色。 |
| `areaColor` | | 区域指示的颜色。 |
| `labelTextColor` | | 十字指示器坐标轴标签的文本颜色。 |
| `labelBackgroundColor` | | 十字指示器坐标轴标签的背景颜色。 |

## `VisualMap`

Inherits or Implemented: [MainComponent](#MainComponent)

视觉映射组件。用于进行『视觉编码』，也就是将数据映射到视觉元素（视觉通道）。

|field|default|comment|
|--|--|--|
| `show` |true | 组件是否生效。 |
| `showUI` |false | 是否显示组件。如果设置为 false，不会显示，但是数据映射的功能还存在。 |
| `type` | | 组件类型。</br>`Painter.Type`:</br>- `Base`: </br>- `Serie`: </br>- `Top`: </br>|
| `selectedMode` | | 选择模式。</br>`VisualMap.SelectedMode`:</br>- `Multiple`: 多选。</br>- `Single`: 单选。</br>|
| `serieIndex` |0 | 影响的serie索引。 |
| `min` |0 | 范围最小值 |
| `max` |100 | 范围最大值 |
| `range` | | 指定手柄对应数值的位置。range 应在[min,max]范围内。 |
| `text` | | 两端的文本，如 ['High', 'Low']。 |
| `textGap` | | 两端文字主体之间的距离，单位为px。 |
| `splitNumber` |5 | 对于连续型数据，自动平均切分成几段，默认为0时自动匹配inRange颜色列表大小。 |
| `calculable` |false | 是否显示拖拽用的手柄（手柄能拖拽调整选中范围）。 |
| `realtime` |true | 拖拽时，是否实时更新。 |
| `itemWidth` |20f | 图形的宽度，即颜色条的宽度。 |
| `itemHeight` |140f | 图形的高度，即颜色条的高度。 |
| `itemGap` |10f | 每个图元之间的间隔距离。 |
| `borderWidth` |0 | 边框线宽，单位px。 |
| `dimension` |-1 | Starting at 1, the default is 0 to take the last dimension in data. |
| `hoverLink` |true | Conversely, when the mouse hovers over a graphic element in a diagram, the corresponding value of the visualMap component is triangulated in the corresponding position. |
| `autoMinMax` |true | Automatically set min, Max value 自动设置min，max的值 |
| `orient` | | 布局方式是横还是竖。</br>`Orient`:</br>- `Horizonal`: 水平</br>- `Vertical`: 垂直</br>|
| `location` | | 组件显示的位置。 [Location](Location)|
| `workOnLine` |true | 组件是否对LineChart的LineStyle有效。 |
| `workOnArea` |false | 组件是否对LineChart的AreaStyle有效。 |
| `outOfRange` | | 定义 在选中范围外 的视觉颜色。 |
| `inRange` | | 分段式每一段的相关配置。 |

## `VisualMapRange`

Inherits or Implemented: [ChildComponent](#ChildComponent)

|field|default|comment|
|--|--|--|
| `min` | | 范围最小值 |
| `max` | | 范围最大值 |
| `label` | | 文字描述 |
| `color` | | 颜色 |

## `VisualMapTheme`

Inherits or Implemented: [ComponentTheme](#ComponentTheme)

|field|default|comment|
|--|--|--|
| `borderWidth` | | 边框线宽。 |
| `borderColor` | | 边框颜色。 |
| `backgroundColor` | | 背景颜色。 |
| `triangeLen` |20f | 可视化组件的调节三角形边长。 |

## `XAxis`

Inherits or Implemented: [Axis](#Axis)

直角坐标系 grid 中的 x 轴。


## `XCResourcesImporter`


## `XCSettings`

Inherits or Implemented: [ScriptableObject](#ScriptableObject)

|field|default|comment|
|--|--|--|
| `lang` | |  [Lang](Lang)|
| `font` | |  |
| `tMPFont` | |  |
| `fontSizeLv1` |28 |  |
| `fontSizeLv2` |24 |  |
| `fontSizeLv3` |20 |  |
| `fontSizeLv4` |18 |  |
| `axisLineType` | |  |
| `axisLineWidth` |0.8f |  |
| `axisSplitLineType` | |  |
| `axisSplitLineWidth` |0.8f |  |
| `axisTickWidth` |0.8f |  |
| `axisTickLength` |5f |  |
| `gaugeAxisLineWidth` |15f |  |
| `gaugeAxisSplitLineWidth` |0.8f |  |
| `gaugeAxisSplitLineLength` |15f |  |
| `gaugeAxisTickWidth` |0.8f |  |
| `gaugeAxisTickLength` |5f |  |
| `tootipLineWidth` |0.8f |  |
| `dataZoomBorderWidth` |0.5f |  |
| `dataZoomDataLineWidth` |0.5f |  |
| `visualMapBorderWidth` |0f |  |
| `serieLineWidth` |1.8f |  |
| `serieLineSymbolSize` |5f |  |
| `serieScatterSymbolSize` |20f |  |
| `serieSelectedRate` |1.3f |  |
| `serieCandlestickBorderWidth` |1f |  |
| `editorShowAllListData` |false |  |
| `maxPainter` |10 |  |
| `lineSmoothStyle` |3f |  |
| `lineSmoothness` |2f |  |
| `lineSegmentDistance` |3f |  |
| `cicleSmoothness` |2f |  |
| `visualMapTriangeLen` |20f |  |
| `pieTooltipExtraRadius` |8f |  |
| `pieSelectedOffset` |8f |  |
| `customThemes` | |  |

## `YAxis`

Inherits or Implemented: [Axis](#Axis)

直角坐标系 grid 中的 y 轴。


[XCharts主页](https://github.com/XCharts-Team/XCharts)</br>
[XChartsAPI接口](XChartsAPI-ZH.md)</br>
[XCharts问答](XChartsFAQ-ZH.md)
