---
sidebar_position: 31
slug: /configuration
---
import APITable from '@site/src/components/APITable';

# 配置项手册

## Serie 系列

- [Bar](#bar)
- [BaseScatter](#basescatter)
- [Candlestick](#candlestick)
- [EffectScatter](#effectscatter)
- [Heatmap](#heatmap)
- [Line](#line)
- [Parallel](#parallel)
- [Pie](#pie)
- [Radar](#radar)
- [Ring](#ring)
- [Scatter](#scatter)
- [Serie](#serie)
- [SimplifiedBar](#simplifiedbar)
- [SimplifiedCandlestick](#simplifiedcandlestick)
- [SimplifiedLine](#simplifiedline)


## Theme 主题

- [AngleAxisTheme](#angleaxistheme)
- [AxisTheme](#axistheme)
- [BaseAxisTheme](#baseaxistheme)
- [ComponentTheme](#componenttheme)
- [DataZoomTheme](#datazoomtheme)
- [LegendTheme](#legendtheme)
- [PolarAxisTheme](#polaraxistheme)
- [RadarAxisTheme](#radaraxistheme)
- [RadiusAxisTheme](#radiusaxistheme)
- [SerieTheme](#serietheme)
- [SubTitleTheme](#subtitletheme)
- [Theme](#theme)
- [ThemeStyle](#themestyle)
- [TitleTheme](#titletheme)
- [TooltipTheme](#tooltiptheme)
- [UIComponentTheme](#uicomponenttheme)
- [VisualMapTheme](#visualmaptheme)


## MainComponent 主组件

- [AngleAxis](#angleaxis)
- [Axis](#axis)
- [Background](#background)
- [CalendarCoord](#calendarcoord)
- [Comment](#comment)
- [CoordSystem](#coordsystem)
- [DataZoom](#datazoom)
- [GridCoord](#gridcoord)
- [GridLayout](#gridlayout)
- [Legend](#legend)
- [MarkArea](#markarea)
- [MarkLine](#markline)
- [ParallelAxis](#parallelaxis)
- [ParallelCoord](#parallelcoord)
- [PolarCoord](#polarcoord)
- [RadarCoord](#radarcoord)
- [RadiusAxis](#radiusaxis)
- [Settings](#settings)
- [SingleAxis](#singleaxis)
- [SingleAxisCoord](#singleaxiscoord)
- [Title](#title)
- [Tooltip](#tooltip)
- [VisualMap](#visualmap)
- [XAxis](#xaxis)
- [YAxis](#yaxis)


## ChildComponent 子组件

- [AngleAxisTheme](#angleaxistheme)
- [AnimationStyle](#animationstyle)
- [AreaStyle](#areastyle)
- [ArrowStyle](#arrowstyle)
- [AxisAnimation](#axisanimation)
- [AxisLabel](#axislabel)
- [AxisLine](#axisline)
- [AxisMinorSplitLine](#axisminorsplitline)
- [AxisMinorTick](#axisminortick)
- [AxisName](#axisname)
- [AxisSplitArea](#axissplitarea)
- [AxisSplitLine](#axissplitline)
- [AxisTheme](#axistheme)
- [AxisTick](#axistick)
- [BaseAxisTheme](#baseaxistheme)
- [BaseLine](#baseline)
- [BlurStyle](#blurstyle)
- [CommentItem](#commentitem)
- [CommentMarkStyle](#commentmarkstyle)
- [ComponentTheme](#componenttheme)
- [DataZoomTheme](#datazoomtheme)
- [EmphasisStyle](#emphasisstyle)
- [EndLabelStyle](#endlabelstyle)
- [IconStyle](#iconstyle)
- [ImageStyle](#imagestyle)
- [ItemStyle](#itemstyle)
- [LabelLine](#labelline)
- [LabelStyle](#labelstyle)
- [LegendTheme](#legendtheme)
- [Level](#level)
- [LevelStyle](#levelstyle)
- [LineArrow](#linearrow)
- [LineStyle](#linestyle)
- [Location](#location)
- [MarkAreaData](#markareadata)
- [MarkLineData](#marklinedata)
- [MarqueeStyle](#marqueestyle)
- [MLValue](#mlvalue)
- [Padding](#padding)
- [PolarAxisTheme](#polaraxistheme)
- [RadarAxisTheme](#radaraxistheme)
- [RadiusAxisTheme](#radiusaxistheme)
- [SelectStyle](#selectstyle)
- [SerieData](#seriedata)
- [SerieSymbol](#seriesymbol)
- [SerieTheme](#serietheme)
- [StageColor](#stagecolor)
- [StateStyle](#statestyle)
- [SubTitleTheme](#subtitletheme)
- [SymbolStyle](#symbolstyle)
- [TextLimit](#textlimit)
- [TextPadding](#textpadding)
- [TextStyle](#textstyle)
- [ThemeStyle](#themestyle)
- [TitleStyle](#titlestyle)
- [TitleTheme](#titletheme)
- [TooltipTheme](#tooltiptheme)
- [UIComponentTheme](#uicomponenttheme)
- [VisualMapRange](#visualmaprange)
- [VisualMapTheme](#visualmaptheme)


## ISerieComponent 可添加到Serie的组件

- [AreaStyle](#areastyle)
- [BlurStyle](#blurstyle)
- [EmphasisStyle](#emphasisstyle)
- [ImageStyle](#imagestyle)
- [LabelLine](#labelline)
- [LabelStyle](#labelstyle)
- [LineArrow](#linearrow)
- [SelectStyle](#selectstyle)
- [TitleStyle](#titlestyle)


## ISerieDataComponent 可添加到SerieData的组件

- [AreaStyle](#areastyle)
- [BlurStyle](#blurstyle)
- [EmphasisStyle](#emphasisstyle)
- [ImageStyle](#imagestyle)
- [ItemStyle](#itemstyle)
- [LabelLine](#labelline)
- [LabelStyle](#labelstyle)
- [LineStyle](#linestyle)
- [SelectStyle](#selectstyle)
- [SerieSymbol](#seriesymbol)
- [TitleStyle](#titlestyle)


## Other 其他

- [AnimationAddition](#animationaddition)
- [AnimationChange](#animationchange)
- [AnimationFadeIn](#animationfadein)
- [AnimationFadeOut](#animationfadeout)
- [AnimationHiding](#animationhiding)
- [AnimationInfo](#animationinfo)
- [AnimationInteraction](#animationinteraction)
- [BaseSerie](#baseserie)
- [ChartText](#charttext)
- [ChildComponent](#childcomponent)
- [DebugInfo](#debuginfo)
- [Indicator](#indicator)
- [INeedSerieContainer](#ineedseriecontainer)
- [IPropertyChanged](#ipropertychanged)
- [ISerieComponent](#iseriecomponent)
- [ISerieContainer](#iseriecontainer)
- [ISerieDataComponent](#iseriedatacomponent)
- [ISimplifiedSerie](#isimplifiedserie)
- [IUpdateRuntimeData](#iupdateruntimedata)
- [Lang](#lang)
- [LangCandlestick](#langcandlestick)
- [LangTime](#langtime)
- [MainComponent](#maincomponent)
- [XCResourcesImporter](#xcresourcesimporter)
- [XCSettings](#xcsettings)


## AngleAxis

> class in XCharts.Runtime / 继承自: [Axis](#axis)

极坐标系的角度轴。

```mdx-code-block
<APITable name="AngleAxis">
```

|参数|默认|版本|描述|
|--|--|--|--|
|startAngle|0||起始刻度的角度，默认为 0 度，即圆心的正右方。

```mdx-code-block
</APITable>
```

## AngleAxisTheme

> class in XCharts.Runtime / 继承自: [BaseAxisTheme](#baseaxistheme)

## AnimationAddition

> class in XCharts.Runtime / 继承自: [AnimationInfo](#animationinfo)

> 从 `v3.8.0` 开始支持

数据新增动画。

## AnimationChange

> class in XCharts.Runtime / 继承自: [AnimationInfo](#animationinfo)

> 从 `v3.8.0` 开始支持

数据变更动画。

## AnimationFadeIn

> class in XCharts.Runtime / 继承自: [AnimationInfo](#animationinfo)

> 从 `v3.8.0` 开始支持

淡入动画。

## AnimationFadeOut

> class in XCharts.Runtime / 继承自: [AnimationInfo](#animationinfo)

> 从 `v3.8.0` 开始支持

淡出动画。

## AnimationHiding

> class in XCharts.Runtime / 继承自: [AnimationInfo](#animationinfo)

> 从 `v3.8.0` 开始支持

数据隐藏动画。

## AnimationInfo

> class in XCharts.Runtime / 子类: [AnimationFadeIn](#animationfadein), [AnimationFadeOut](#animationfadeout), [AnimationChange](#animationchange), [AnimationAddition](#animationaddition), [AnimationHiding](#animationhiding), [AnimationInteraction](#animationinteraction)

> 从 `v3.8.0` 开始支持

动画配置参数。

```mdx-code-block
<APITable name="AnimationInfo">
```

|参数|默认|版本|描述|
|--|--|--|--|
|enable|true|v3.8.0|是否开启动画效果。
|reverse|false|v3.8.0|是否开启反向动画效果。
|delay|0|v3.8.0|动画开始前的延迟时间。
|duration|1000|v3.8.0|动画的时长。

```mdx-code-block
</APITable>
```

## AnimationInteraction

> class in XCharts.Runtime / 继承自: [AnimationInfo](#animationinfo)

> 从 `v3.8.0` 开始支持

交互动画。

```mdx-code-block
<APITable name="AnimationInteraction">
```

|参数|默认|版本|描述|
|--|--|--|--|
|width||v3.8.0|宽度的多样式数值。 [MLValue](#mlvalue)|
|radius||v3.8.0|半径的多样式数值。 [MLValue](#mlvalue)|
|offset||v3.8.0|交互的多样式数值。如饼图的扇形选中时的偏移。 [MLValue](#mlvalue)|

```mdx-code-block
</APITable>
```

## AnimationStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

动画组件，用于控制图表的动画播放。支持配置五种动画表现：FadeIn（渐入动画），FadeOut（渐出动画），Change（变更动画），Addition（新增动画），Interaction（交互动画）。 按作用的对象可以分为两类：SerieAnimation（系列动画）和DataAnimation（数据动画）。

```mdx-code-block
<APITable name="AnimationStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|enable|true||是否开启动画效果。
|type|||动画类型。<br/>`AnimationType`:<br/>- `Default`: 默认。内部会根据实际情况选择一种动画播放方式。<br/>- `LeftToRight`: 从左往右播放动画。<br/>- `BottomToTop`: 从下往上播放动画。<br/>- `InsideOut`: 由内到外播放动画。<br/>- `AlongPath`: 沿着路径播放动画。当折线图从左到右无序或有折返时，可以使用该模式。<br/>- `Clockwise`: 顺时针播放动画。<br/>|
|easting|||<br/>`AnimationEasing`:<br/>- `Linear`: <br/>|
|threshold|2000||是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。
|unscaledTime||v3.4.0|动画是否受TimeScaled的影响。默认为 false 受TimeScaled的影响。
|fadeIn||v3.8.0|渐入动画配置。 [AnimationFadeIn](#animationfadein)|
|fadeOut||v3.8.0|渐出动画配置。 [AnimationFadeOut](#animationfadeout)|
|change||v3.8.0|数据变更动画配置。 [AnimationChange](#animationchange)|
|addition||v3.8.0|数据新增动画配置。 [AnimationAddition](#animationaddition)|
|hiding||v3.8.0|数据隐藏动画配置。 [AnimationHiding](#animationhiding)|
|interaction||v3.8.0|交互动画配置。 [AnimationInteraction](#animationinteraction)|

```mdx-code-block
</APITable>
```

## AreaStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

区域填充样式。

```mdx-code-block
<APITable name="AreaStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示区域填充。
|origin|||区域填充的起始位置。<br/>`AreaStyle.AreaOrigin`:<br/>- `Auto`: 填充坐标轴轴线到数据间的区域。<br/>- `Start`: 填充坐标轴底部到数据间的区域。<br/>- `End`: 填充坐标轴顶部到数据间的区域。<br/>|
|color|||区域填充的颜色，如果toColor不是默认值，则表示渐变色的起点颜色。
|toColor|||渐变色的终点颜色。
|opacity|0.6f||图形透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
|innerFill||v3.2.0|是否只填充多边形区域。目前只支持凸多边形。
|toTop|true|v3.6.0|渐变色是到顶部还是到实际位置。默认为true到顶部。

```mdx-code-block
</APITable>
```

## ArrowStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="ArrowStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|width|10||箭头宽。
|height|15||箭头高。
|offset|0||箭头偏移。
|dent|3||箭头的凹度。
|color|Color.clear||箭头颜色。

```mdx-code-block
</APITable>
```

## Axis

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent) / 子类: [AngleAxis](#angleaxis), [ParallelAxis](#parallelaxis), [RadiusAxis](#radiusaxis), [SingleAxis](#singleaxis), [XAxis](#xaxis), [YAxis](#yaxis)

直角坐标系的坐标轴组件。

```mdx-code-block
<APITable name="Axis">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示坐标轴。
|type|||坐标轴类型。<br/>`Axis.AxisType`:<br/>- `Value`: 数值轴。适用于连续数据。<br/>- `Category`: 类目轴。适用于离散的类目数据，为该类型时必须通过 data 设置类目数据。serie的数据第0维数据对应坐标轴data的index。<br/>- `Log`: 对数轴。适用于对数数据。<br/>- `Time`: 时间轴。适用于连续的时序数据。<br/>|
|minMaxType|||坐标轴刻度最大最小值显示类型。<br/>`Axis.AxisMinMaxType`:<br/>- `Default`: 0-最大值。<br/>- `MinMax`: 最小值-最大值。<br/>- `Custom`: 自定义最小值最大值。<br/>- `MinMaxAuto`: [since("v3.7.0")]最小值-最大值。自动计算合适的值。<br/>|
|gridIndex|||坐标轴所在的 grid 的索引，默认位于第一个 grid。
|polarIndex|||坐标轴所在的 ploar 的索引，默认位于第一个 polar。
|parallelIndex|||坐标轴所在的 parallel 的索引，默认位于第一个 parallel。
|position|||坐标轴在Grid中的位置。<br/>`Axis.AxisPosition`:<br/>- `Left`: 坐标轴在Grid中的位置<br/>- `Right`: 坐标轴在Grid中的位置<br/>- `Bottom`: 坐标轴在Grid中的位置<br/>- `Top`: 坐标轴在Grid中的位置<br/>|
|offset|||坐标轴相对默认位置的偏移。在相同position有多个坐标轴时有用。
|min|||设定的坐标轴刻度最小值，当minMaxType为Custom时有效。
|max|||设定的坐标轴刻度最大值，当minMaxType为Custom时有效。
|splitNumber|0||坐标轴的期望的分割段数。默认为0表示自动分割。
|interval|0||强制设置坐标轴分割间隔。无法在类目轴中使用。
|boundaryGap|true||坐标轴两边是否留白。只对类目轴有效。
|maxCache|0||The first data will be remove when the size of axis data is larger then maxCache.
|logBase|10||对数轴的底数，只在对数轴（type:'Log'）中有效。
|logBaseE|false||对数轴是否以自然数 e 为底数，为 true 时 logBase 失效。
|ceilRate|0||最大最小值向上取整的倍率。默认为0时自动计算。
|inverse|false||是否反向坐标轴。在类目轴中无效。
|clockwise|true||刻度增长是否按顺时针，默认顺时针。
|insertDataToHead|||添加新数据时是在列表的头部还是尾部加入。
|icons|||类目数据对应的图标。
|data|||类目数据，在类目轴（type: 'category'）中有效。
|axisLine|||坐标轴轴线。 [AxisLine](#axisline)|
|axisName|||坐标轴名称。 [AxisName](#axisname)|
|axisTick|||坐标轴刻度。 [AxisTick](#axistick)|
|axisLabel|||坐标轴刻度标签。 [AxisLabel](#axislabel)|
|splitLine|||坐标轴分割线。 [AxisSplitLine](#axissplitline)|
|splitArea|||坐标轴分割区域。 [AxisSplitArea](#axissplitarea)|
|animation|||坐标轴动画。 [AxisAnimation](#axisanimation)|
|minorTick||v3.2.0|坐标轴次刻度。 [AxisMinorTick](#axisminortick)|
|minorSplitLine||v3.2.0|坐标轴次分割线。 [AxisMinorSplitLine](#axisminorsplitline)|
|indicatorLabel||v3.4.0|指示器文本的样式。Tooltip为Cross时使用。 [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## AxisAnimation

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

> 从 `v3.9.0` 开始支持

坐标轴动画配置。

```mdx-code-block
<APITable name="AxisAnimation">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否开启动画。
|duration|||动画时长(ms)。 默认设置为0时，会自动获取serie的动画时长。
|unscaledTime|||动画是否受TimeScaled的影响。默认为 false 受TimeScaled的影响。

```mdx-code-block
</APITable>
```

## AxisLabel

> class in XCharts.Runtime / 继承自: [LabelStyle](#labelstyle)

坐标轴刻度标签的相关设置。

```mdx-code-block
<APITable name="AxisLabel">
```

|参数|默认|版本|描述|
|--|--|--|--|
|interval|0||坐标轴刻度标签的显示间隔，在类目轴中有效。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。
|inside|false||刻度标签是否朝内，默认朝外。
|showAsPositiveNumber|false||将负数数值显示为正数。一般和`Serie`的`showAsPositiveNumber`配合使用。
|onZero|false||刻度标签显示在0刻度上。
|showStartLabel|true||是否显示第一个文本。
|showEndLabel|true||是否显示最后一个文本。
|textLimit|||文本限制。 [TextLimit](#textlimit)|

```mdx-code-block
</APITable>
```

## AxisLine

> class in XCharts.Runtime / 继承自: [BaseLine](#baseline)

坐标轴轴线。

```mdx-code-block
<APITable name="AxisLine">
```

|参数|默认|版本|描述|
|--|--|--|--|
|onZero|||X 轴或者 Y 轴的轴线是否在另一个轴的 0 刻度上，只有在另一个轴为数值轴且包含 0 刻度时有效。
|showArrow|||是否显示箭头。
|arrow|||轴线箭头。 [ArrowStyle](#arrowstyle)|

```mdx-code-block
</APITable>
```

## AxisMinorSplitLine

> class in XCharts.Runtime / 继承自: [BaseLine](#baseline)

> 从 `v3.2.0` 开始支持

坐标轴在 grid 区域中的次分隔线。次分割线会对齐次刻度线 minorTick。

```mdx-code-block
<APITable name="AxisMinorSplitLine">
```

|参数|默认|版本|描述|
|--|--|--|--|
|distance|||刻度线与轴线的距离。
|autoColor|||自动设置颜色。

```mdx-code-block
</APITable>
```

## AxisMinorTick

> class in XCharts.Runtime / 继承自: [BaseLine](#baseline)

> 从 `v3.2.0` 开始支持

坐标轴次刻度相关设置。注意：次刻度无法在类目轴中使用。

```mdx-code-block
<APITable name="AxisMinorTick">
```

|参数|默认|版本|描述|
|--|--|--|--|
|splitNumber|5||分隔线之间分割的刻度数。
|autoColor|||

```mdx-code-block
</APITable>
```

## AxisName

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

坐标轴名称。

```mdx-code-block
<APITable name="AxisName">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|||是否显示坐标轴名称。
|name|||坐标轴名称。
|onZero||v3.1.0|坐标轴名称的位置是否保持和Y轴0刻度一致。
|labelStyle|||文本样式。 [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## AxisSplitArea

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

坐标轴在 grid 区域中的分隔区域，默认不显示。

```mdx-code-block
<APITable name="AxisSplitArea">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|||是否显示分隔区域。
|color|||分隔区域颜色。分隔区域会按数组中颜色的顺序依次循环设置颜色。默认是一个深浅的间隔色。

```mdx-code-block
</APITable>
```

## AxisSplitLine

> class in XCharts.Runtime / 继承自: [BaseLine](#baseline)

坐标轴在 grid 区域中的分隔线。

```mdx-code-block
<APITable name="AxisSplitLine">
```

|参数|默认|版本|描述|
|--|--|--|--|
|interval|||坐标轴分隔线的显示间隔。
|distance|||刻度线与轴线的距离。
|autoColor|||自动设置颜色。
|showStartLine|true|v3.3.0|是否显示第一条分割线。
|showEndLine|true|v3.3.0|是否显示最后一条分割线。

```mdx-code-block
</APITable>
```

## AxisTheme

> class in XCharts.Runtime / 继承自: [BaseAxisTheme](#baseaxistheme)

## AxisTick

> class in XCharts.Runtime / 继承自: [BaseLine](#baseline)

坐标轴刻度相关设置。

```mdx-code-block
<APITable name="AxisTick">
```

|参数|默认|版本|描述|
|--|--|--|--|
|alignWithLabel|||类目轴中在 boundaryGap 为 true 的时候有效，可以保证刻度线和标签对齐。
|inside|||坐标轴刻度是否朝内，默认朝外。
|showStartTick|||是否显示第一个刻度。
|showEndTick|||是否显示最后一个刻度。
|distance|||刻度线与轴线的距离。
|splitNumber|0||分隔线之间分割的刻度数。
|autoColor|||

```mdx-code-block
</APITable>
```

## Background

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent)

背景组件。

```mdx-code-block
<APITable name="Background">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否启用背景组件。
|image|||背景图。
|imageType|||背景图填充类型。
|imageColor|||背景图颜色。
|autoColor|true||当background组件开启时，是否自动使用主题背景色作为backgrounnd组件的颜色。当设置为false时，用imageColor作为颜色。

```mdx-code-block
</APITable>
```

## Bar

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## BaseAxisTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme) / 子类: [AxisTheme](#axistheme), [RadiusAxisTheme](#radiusaxistheme), [AngleAxisTheme](#angleaxistheme), [PolarAxisTheme](#polaraxistheme), [RadarAxisTheme](#radaraxistheme)

```mdx-code-block
<APITable name="BaseAxisTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|lineType|||坐标轴线类型。<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|lineWidth|1f||坐标轴线宽。
|lineLength|0f||坐标轴线长。
|lineColor|||坐标轴线颜色。
|splitLineType|||分割线线类型。<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|splitLineWidth|1f||分割线线宽。
|splitLineLength|0f||分割线线长。
|splitLineColor|||分割线线颜色。
|minorSplitLineColor|||次分割线线颜色。
|tickWidth|1f||刻度线线宽。
|tickLength|5f||刻度线线长。
|tickColor|||坐标轴线颜色。
|splitAreaColors|||坐标轴分隔区域的颜色。

```mdx-code-block
</APITable>
```

## BaseLine

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent) / 子类: [AxisLine](#axisline), [AxisMinorSplitLine](#axisminorsplitline), [AxisMinorTick](#axisminortick), [AxisSplitLine](#axissplitline), [AxisTick](#axistick)

线条基础配置。

```mdx-code-block
<APITable name="BaseLine">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|||是否显示坐标轴轴线。
|lineStyle|||线条样式 [LineStyle](#linestyle)|

```mdx-code-block
</APITable>
```

## BaseScatter

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer) / 子类: [EffectScatter](#effectscatter), [Scatter](#scatter)

## BaseSerie

> class in XCharts.Runtime / 子类: [Serie](#serie)

## BlurStyle

> class in XCharts.Runtime / 继承自: [StateStyle](#statestyle), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

> 从 `v3.2.0` 开始支持

淡出状态样式。

## CalendarCoord

> class in XCharts.Runtime / 继承自: [CoordSystem](#coordsystem), [IUpdateRuntimeData](#iupdateruntimedata), [ISerieContainer](#iseriecontainer)

## Candlestick

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## ChartText

> class in XCharts.Runtime

## ChildComponent

> class in XCharts.Runtime / 子类: [AnimationStyle](#animationstyle), [AxisAnimation](#axisanimation), [AxisName](#axisname), [AxisSplitArea](#axissplitarea), [AreaStyle](#areastyle), [ArrowStyle](#arrowstyle), [BaseLine](#baseline), [IconStyle](#iconstyle), [ImageStyle](#imagestyle), [ItemStyle](#itemstyle), [Level](#level), [LevelStyle](#levelstyle), [LineArrow](#linearrow), [LineStyle](#linestyle), [Location](#location), [MLValue](#mlvalue), [MarqueeStyle](#marqueestyle), [Padding](#padding), [StageColor](#stagecolor), [SymbolStyle](#symbolstyle), [TextLimit](#textlimit), [TextStyle](#textstyle), [CommentItem](#commentitem), [CommentMarkStyle](#commentmarkstyle), [LabelLine](#labelline), [LabelStyle](#labelstyle), [MarkAreaData](#markareadata), [MarkLineData](#marklinedata), [StateStyle](#statestyle), [VisualMapRange](#visualmaprange), [UIComponentTheme](#uicomponenttheme), [SerieData](#seriedata), [ComponentTheme](#componenttheme), [SerieTheme](#serietheme), [ThemeStyle](#themestyle)

## Comment

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent), [IPropertyChanged](#ipropertychanged)

图表注解组件。

```mdx-code-block
<APITable name="Comment">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示注解组件。
|labelStyle|||所有组件的文本样式。 [LabelStyle](#labelstyle)|
|markStyle|||所有组件的文本样式。 [CommentMarkStyle](#commentmarkstyle)|
|items|||注解项。每个注解组件可以设置多个注解项。

```mdx-code-block
</APITable>
```

## CommentItem

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

注解项。

```mdx-code-block
<APITable name="CommentItem">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示当前注解项。
|content|||注解的文本内容。支持模板参数，可以参考Tooltip的itemFormatter。
|markRect|||注解区域。
|markStyle|||注解标记区域样式。 [CommentMarkStyle](#commentmarkstyle)|
|labelStyle|||注解项的文本样式。 [LabelStyle](#labelstyle)|
|location||v3.5.0|Comment显示的位置。 [Location](#location)|

```mdx-code-block
</APITable>
```

## CommentMarkStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

注解项区域样式。

```mdx-code-block
<APITable name="CommentMarkStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示当前注解项。
|lineStyle|||线条样式。 [LineStyle](#linestyle)|

```mdx-code-block
</APITable>
```

## ComponentTheme

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent) / 子类: [BaseAxisTheme](#baseaxistheme), [DataZoomTheme](#datazoomtheme), [LegendTheme](#legendtheme), [SubTitleTheme](#subtitletheme), [TitleTheme](#titletheme), [TooltipTheme](#tooltiptheme), [VisualMapTheme](#visualmaptheme)

```mdx-code-block
<APITable name="ComponentTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|font|||字体。
|textColor|||文本颜色。
|textBackgroundColor|||文本颜色。
|fontSize|18||文本字体大小。
|tMPFont|||字体。

```mdx-code-block
</APITable>
```

## CoordSystem

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent) / 子类: [RadarCoord](#radarcoord), [CalendarCoord](#calendarcoord), [GridCoord](#gridcoord), [ParallelCoord](#parallelcoord), [PolarCoord](#polarcoord), [SingleAxisCoord](#singleaxiscoord)

坐标系系统。

## DataZoom

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent), [IUpdateRuntimeData](#iupdateruntimedata)

DataZoom 组件 用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。

```mdx-code-block
<APITable name="DataZoom">
```

|参数|默认|版本|描述|
|--|--|--|--|
|enable|true||是否显示缩放区域。
|filterMode|||数据过滤类型。<br/>`DataZoom.FilterMode`:<br/>- `Filter`: 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只要有一个维度在数据窗口外，整个数据项就会被过滤掉。<br/>- `WeakFilter`: 当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只有当全部维度都在数据窗口同侧外部，整个数据项才会被过滤掉。<br/>- `Empty`: 当前数据窗口外的数据，被 设置为空。即 不会 影响其他轴的数据范围。<br/>- `None`: 不过滤数据，只改变数轴范围。<br/>|
|xAxisIndexs|||控制的 x 轴索引列表。
|yAxisIndexs|||控制的 y 轴索引列表。
|supportInside|||是否支持内置。内置于坐标系中，使用户可以在坐标系上通过鼠标拖拽、鼠标滚轮、手指滑动（触屏上）来缩放或漫游坐标系。
|supportInsideScroll|true||是否支持坐标系内滚动
|supportInsideDrag|true||是否支持坐标系内拖拽
|supportSlider|||是否支持滑动条。有单独的滑动条，用户在滑动条上进行缩放或漫游。
|supportMarquee|||是否支持框选。提供一个选框进行数据区域缩放。
|showDataShadow|||是否显示数据阴影。数据阴影可以简单地反应数据走势。
|showDetail|||是否显示detail，即拖拽时候显示详细数值信息。
|zoomLock|||是否锁定选择区域（或叫做数据窗口）的大小。 如果设置为 true 则锁定选择区域的大小，也就是说，只能平移，不能缩放。
|fillerColor|||数据区域颜色。
|borderColor|||边框颜色。
|borderWidth|||边框宽。
|backgroundColor|||组件的背景颜色。
|left|||组件离容器左侧的距离。
|right|||组件离容器右侧的距离。
|top|||组件离容器上侧的距离。
|bottom|||组件离容器下侧的距离。
|rangeMode|||取绝对值还是百分比。<br/>`DataZoom.RangeMode`:<br/>- `//Value`: The value type of start and end.取值类型<br/>- `Percent`: 百分比。<br/>|
|start|||数据窗口范围的起始百分比。范围是：0 ~ 100。
|end|||数据窗口范围的结束百分比。范围是：0 ~ 100。
|minShowNum|2||最小显示数据个数。当DataZoom放大到最大时，最小显示的数据个数。
|scrollSensitivity|1.1f||缩放区域组件的敏感度。值越高每次缩放所代表的数据越多。
|orient|||布局方式是横还是竖。不仅是布局方式，对于直角坐标系而言，也决定了，缺省情况控制横向数轴还是纵向数轴。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|labelStyle|||文本标签格式。 [LabelStyle](#labelstyle)|
|lineStyle|||阴影线条样式。 [LineStyle](#linestyle)|
|areaStyle|||阴影填充样式。 [AreaStyle](#areastyle)|
|marqueeStyle||v3.5.0|选取框样式。 [MarqueeStyle](#marqueestyle)|
|startLock||v3.6.0|固定起始值，不让改变。
|endLock||v3.6.0|固定结束值，不让改变。

```mdx-code-block
</APITable>
```

## DataZoomTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="DataZoomTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|borderWidth|||边框线宽。
|dataLineWidth|||数据阴影线宽。
|fillerColor|||数据区域颜色。
|borderColor|||边框颜色。
|dataLineColor|||数据阴影的线条颜色。
|dataAreaColor|||数据阴影的填充颜色。
|backgroundColor|||背景颜色。

```mdx-code-block
</APITable>
```

## DebugInfo

> class in XCharts.Runtime

```mdx-code-block
<APITable name="DebugInfo">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示Debug组件。
|showDebugInfo|false||
|showAllChartObject|false||是否在Hierarchy试图显示所有chart下的节点。
|foldSeries|false||是否在Inspector上折叠Serie。
|labelStyle||| [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## EffectScatter

> class in XCharts.Runtime / 继承自: [BaseScatter](#basescatter)

## EmphasisStyle

> class in XCharts.Runtime / 继承自: [StateStyle](#statestyle), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

> 从 `v3.2.0` 开始支持

高亮状态样式。

```mdx-code-block
<APITable name="EmphasisStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|scale|1.1f||高亮时的缩放倍数。
|focus|||在高亮图形时，是否淡出其它数据的图形已达到聚焦的效果。<br/>`EmphasisStyle.FocusType`:<br/>- `None`: 不淡出其它图形，默认使用该配置。<br/>- `Self`: 只聚焦（不淡出）当前高亮的数据的图形。<br/>- `Series`: 聚焦当前高亮的数据所在的系列的所有图形。<br/>|
|blurScope|||在开启focus的时候，可以通过blurScope配置淡出的范围。<br/>`EmphasisStyle.BlurScope`:<br/>- `GridCoord`: 淡出范围为坐标系，默认使用该配置。<br/>- `Series`: 淡出范围为系列。<br/>- `Global`: 淡出范围为全局。<br/>|

```mdx-code-block
</APITable>
```

## EndLabelStyle

> class in XCharts.Runtime / 继承自: [LabelStyle](#labelstyle)

## GridCoord

> class in XCharts.Runtime / 继承自: [CoordSystem](#coordsystem), [IUpdateRuntimeData](#iupdateruntimedata), [ISerieContainer](#iseriecontainer)

Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.

```mdx-code-block
<APITable name="GridCoord">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示直角坐标系网格。
|layoutIndex|-1|v3.8.0|网格所属的网格布局组件的索引。默认为-1，表示不属于任何网格布局组件。当设置了该值时，left、right、top、bottom属性将失效。
|left|0.1f||grid 组件离容器左侧的距离。
|right|0.08f||grid 组件离容器右侧的距离。
|top|0.22f||grid 组件离容器上侧的距离。
|bottom|0.12f||grid 组件离容器下侧的距离。
|backgroundColor|||网格背景色，默认透明。
|showBorder|false||是否显示网格边框。
|borderWidth|0f||网格边框宽。
|borderColor|||网格边框颜色。

```mdx-code-block
</APITable>
```

## GridLayout

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent), [IUpdateRuntimeData](#iupdateruntimedata)

> 从 `v3.8.0` 开始支持

网格布局组件。用于管理多个`GridCoord`的布局，可以通过`row`和`column`来控制网格的行列数。

```mdx-code-block
<APITable name="GridLayout">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示直角坐标系网格。
|left|0.1f||grid 组件离容器左侧的距离。
|right|0.08f||grid 组件离容器右侧的距离。
|top|0.22f||grid 组件离容器上侧的距离。
|bottom|0.12f||grid 组件离容器下侧的距离。
|row|2||网格布局的行数。
|column|2||网格布局的列数。
|spacing|Vector2.zero||网格布局的间距。
|inverse|false||是否反转网格布局。

```mdx-code-block
</APITable>
```

## Heatmap

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

```mdx-code-block
<APITable name="Heatmap">
```

|参数|默认|版本|描述|
|--|--|--|--|
|heatmapType||v3.3.0|热力图类型。通过颜色映射划分。<br/>`HeatmapType`:<br/>- `Data`: 数据映射型。默认用第2维数据作为颜色映射。要求数据至少有3个维度数据。<br/>- `Count`: 个数映射型。统计数据在划分的格子中出现的次数，作为颜色映射。要求数据至少有2个维度数据。<br/>|

```mdx-code-block
</APITable>
```

## IconStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="IconStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|false||是否显示图标。
|layer|||显示在上层还是在下层。<br/>`IconStyle.Layer`:<br/>- `UnderText`: The icon is display under the label text. 图标在标签文字下<br/>- `AboveText`: The icon is display above the label text. 图标在标签文字上<br/>|
|align|||水平方向对齐方式。<br/>`Align`:<br/>- `Center`: 对齐方式。文本，图标，图形等的对齐方式。<br/>- `Left`: 对齐方式。文本，图标，图形等的对齐方式。<br/>- `Right`: 对齐方式。文本，图标，图形等的对齐方式。<br/>|
|sprite|||图标的图片。
|type|||图片的显示类型。
|color|||图标颜色。
|width|20||图标宽。
|height|20||图标高。
|offset|||图标偏移。
|autoHideWhenLabelEmpty|false||当label内容为空时是否自动隐藏图标

```mdx-code-block
</APITable>
```

## ImageStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

```mdx-code-block
<APITable name="ImageStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示图标。
|sprite|||图标的图片。
|type|||图片的显示类型。
|autoColor|||是否自动颜色。
|color|||图标颜色。
|width|0||图标宽。
|height|0||图标高。

```mdx-code-block
</APITable>
```

## Indicator

> class in XCharts.Runtime

雷达图的指示器，用来指定雷达图中的多个变量（维度）。

```mdx-code-block
<APITable name="Indicator">
```

|参数|默认|版本|描述|
|--|--|--|--|
|name|||指示器名称。
|max|||指示器的最大值，默认为 0 无限制。
|min|||指示器的最小值，默认为 0 无限制。
|range|||正常值范围。当数值不在这个范围时，会自动变更显示颜色。
|show|||是否显示雷达坐标系组件。
|shape|||雷达图绘制类型，支持 'Polygon' 和 'Circle'。
|radius|100||雷达图的半径。
|splitNumber|5||指示器轴的分割段数。
|center|||雷达图的中心点。数组的第一项是横坐标，第二项是纵坐标。 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
|axisLine|||轴线。 [AxisLine](#axisline)|
|axisName|||雷达图每个指示器名称的配置项。 [AxisName](#axisname)|
|splitLine|||分割线。 [AxisSplitLine](#axissplitline)|
|splitArea|||分割区域。 [AxisSplitArea](#axissplitarea)|
|indicator|true||是否显示指示器。
|positionType|||显示位置类型。
|indicatorGap|10||指示器和雷达的间距。
|ceilRate|0||最大最小值向上取整的倍率。默认为0时自动计算。
|isAxisTooltip|||是否Tooltip显示轴线上的所有数据。
|outRangeColor|Color.red||数值超出范围时显示的颜色。
|connectCenter|false||数值是否连线到中心点。
|lineGradient|true||数值线段是否需要渐变。
|startAngle||v3.4.0|起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|gridIndex|-1|v3.8.0|所使用的 layout 组件的 index。 默认为-1不指定index, 当为大于或等于0时, 为第一个layout组件的第index个格子。
|indicatorList|||指示器列表。

```mdx-code-block
</APITable>
```

## INeedSerieContainer

> class in XCharts.Runtime / 子类: [Bar](#bar), [SimplifiedBar](#simplifiedbar), [Candlestick](#candlestick), [SimplifiedCandlestick](#simplifiedcandlestick), [Heatmap](#heatmap), [Line](#line), [SimplifiedLine](#simplifiedline), [Parallel](#parallel), [Radar](#radar), [BaseScatter](#basescatter)

## IPropertyChanged

> class in XCharts.Runtime / 子类: [Location](#location), [Comment](#comment), [Legend](#legend), [Title](#title)

属性变更接口

## ISerieComponent

> class in XCharts.Runtime / 子类: [AreaStyle](#areastyle), [ImageStyle](#imagestyle), [LineArrow](#linearrow), [LabelLine](#labelline), [LabelStyle](#labelstyle), [BlurStyle](#blurstyle), [EmphasisStyle](#emphasisstyle), [SelectStyle](#selectstyle), [TitleStyle](#titlestyle)

可用于Serie的组件。

## ISerieContainer

> class in XCharts.Runtime / 子类: [RadarCoord](#radarcoord), [CalendarCoord](#calendarcoord), [GridCoord](#gridcoord), [ParallelCoord](#parallelcoord), [PolarCoord](#polarcoord)

## ISerieDataComponent

> class in XCharts.Runtime / 子类: [AreaStyle](#areastyle), [ImageStyle](#imagestyle), [ItemStyle](#itemstyle), [LineStyle](#linestyle), [SerieSymbol](#seriesymbol), [LabelLine](#labelline), [LabelStyle](#labelstyle), [BlurStyle](#blurstyle), [EmphasisStyle](#emphasisstyle), [SelectStyle](#selectstyle), [TitleStyle](#titlestyle)

可用于SerieData的组件。

## ISimplifiedSerie

> class in XCharts.Runtime / 子类: [SimplifiedBar](#simplifiedbar), [SimplifiedCandlestick](#simplifiedcandlestick), [SimplifiedLine](#simplifiedline)

## ItemStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieDataComponent](#iseriedatacomponent)

图形样式。

```mdx-code-block
<APITable name="ItemStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否启用。
|color|||数据项颜色。
|color0|||数据项颜色。
|toColor|||渐变色的颜色1。
|toColor2|||渐变色的颜色2。只在折线图中有效。
|markColor||v3.6.0|Serie的标识颜色。仅用于Legend和Tooltip的展示，不影响绘制颜色，默认为clear。
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
|numericFormatter|||标准数字格式字符串。用于将数值格式化显示为字符串。 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings
|cornerRadius|||圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。

```mdx-code-block
</APITable>
```

## IUpdateRuntimeData

> class in XCharts.Runtime / 子类: [SingleAxis](#singleaxis), [DataZoom](#datazoom), [CalendarCoord](#calendarcoord), [GridCoord](#gridcoord), [GridLayout](#gridlayout), [ParallelCoord](#parallelcoord)

## LabelLine

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

标签的引导线

```mdx-code-block
<APITable name="LabelLine">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示视觉引导线。
|lineType|||视觉引导线类型。<br/>`LabelLine.LineType`:<br/>- `BrokenLine`: 折线<br/>- `Curves`: 曲线<br/>- `HorizontalLine`: 水平线<br/>|
|lineColor|Color32(0,0,0,0)||视觉引导线颜色。默认和serie一致取自调色板。
|lineAngle|60||视觉引导线的固定角度。对折线和曲线有效。在Pie中无效。
|lineWidth|1.0f||视觉引导线的宽度。
|lineGap|1.0f||视觉引导线和容器的间距。
|lineLength1|25f||视觉引导线第一段的长度。
|lineLength2|15f||视觉引导线第二段的长度。
|lineEndX|0f|v3.8.0|视觉引导线结束点的固定x位置。当不为0时，会代替lineLength2设定引导线的x位置。
|startSymbol|||起始点的图形标记。 [SymbolStyle](#symbolstyle)|
|endSymbol|||结束点的图形标记。 [SymbolStyle](#symbolstyle)|

```mdx-code-block
</APITable>
```

## LabelStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent) / 子类: [AxisLabel](#axislabel), [EndLabelStyle](#endlabelstyle), [TitleStyle](#titlestyle)

图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。

```mdx-code-block
<APITable name="LabelStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示文本标签。
|Position|||标签的位置。
|autoOffset|false||是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。
|offset|||距离图形元素的偏移
|rotate|||文本的旋转。
|autoRotate|false|v3.6.0|是否自动旋转。
|distance|||距离轴线的距离。
|formatter|||标签内容字符串模版格式器。支持用 \n 换行。部分组件的格式器会不生效。<br/> 模板通配符有以下这些，部分只适用于固定的组件：<br/> `{.}`：圆点标记。<br/> `{a}`：系列名。<br/> `{b}`：类目值或数据名。<br/> `{c}`：数据值。<br/> `{d}`：百分比。<br/> `{e}`：数据名。<br/> `{f}`：数据和。<br/> `{g}`：数据总个数。<br/> `{h}`：十六进制颜色值。<br/> `{value}`：坐标轴或图例的值。<br/> 以下通配符适用UITable组件：<br/> `{name}`： 表格的行名。<br/> `{index}`：表格的行号。<br/> 以下通配符适用UIStatistc组件：<br/> `{title}`：标题文本。<br/> `{dd}`：天。<br/> `{hh}`：小时。<br/> `{mm}`：分钟。<br/> `{ss}`：秒。<br/> `{fff}`：毫秒。<br/> `{d}`：天。<br/> `{h}`：小时。<br/> `{m}`：分钟。<br/> `{s}`：秒。<br/> `{f}`：毫秒。<br/> 示例：“{b}:{c}”
|numericFormatter|||标准数字和日期格式字符串。用于将Double数值或DateTime日期格式化显示为字符串。numericFormatter用来作为Double.ToString()或DateTime.ToString()的参数。<br/> 数字格式使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。如：F1, E2<br/> 日期格式常见的格式：yyyy年，MM月，dd日，HH时，mm分，ss秒，fff毫秒。如：yyyy-MM-dd HH:mm:ss<br/> 数值格式化参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings <br/> 日期格式化参考：https://learn.microsoft.com/zh-cn/dotnet/standard/base-types/standard-date-and-time-format-strings
|width|0||标签的宽度。一般不用指定，不指定时则自动是文字的宽度。
|height|0||标签的高度。一般不用指定，不指定时则自动是文字的高度。
|icon|||图标样式。 [IconStyle](#iconstyle)|
|background|||背景图样式。 [ImageStyle](#imagestyle)|
|textPadding|||文本的边距。 [TextPadding](#textpadding)|
|textStyle|||文本样式。 [TextStyle](#textstyle)|

```mdx-code-block
</APITable>
```

## Lang

> class in XCharts.Runtime / 继承自: [ScriptableObject](https://docs.unity3d.com/ScriptReference/30_search.html?q=ScriptableObject)

国际化语言表。

## LangCandlestick

> class in XCharts.Runtime

## LangTime

> class in XCharts.Runtime

## Legend

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent), [IPropertyChanged](#ipropertychanged)

图例组件。 图例组件展现了不同系列的标记，颜色和名字。可以通过点击图例控制哪些系列不显示。

```mdx-code-block
<APITable name="Legend">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示图例组件。
|iconType|||图例类型。<br/>`Legend.Type`:<br/>- `Auto`: 自动匹配。<br/>- `Custom`: 自定义图标。<br/>- `EmptyCircle`: 空心圆。<br/>- `Circle`: 圆形。<br/>- `Rect`: 正方形。可通过Setting的legendIconCornerRadius参数调整圆角。<br/>- `Triangle`: 三角形。<br/>- `Diamond`: 菱形。<br/>- `Candlestick`: 烛台（可用于K线图）。<br/>|
|selectedMode|||选择模式。控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 None 关闭。<br/>`Legend.SelectedMode`:<br/>- `Multiple`: 多选。<br/>- `Single`: 单选。<br/>- `None`: 无法选择。<br/>|
|orient|||布局方式是横还是竖。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|location|||图例显示的位置。 [Location](#location)|
|itemWidth|25.0f||图例标记的图形宽度。
|itemHeight|12.0f||图例标记的图形高度。
|itemGap|10f||图例每项之间的间隔。横向布局时为水平间隔，纵向布局时为纵向间隔。
|itemAutoColor|true||图例标记的图形是否自动匹配颜色。
|itemOpacity|1||图例标记的图形的颜色透明度。
|formatter|||不再使用，使用LabelStyle.formatter代替。
|labelStyle|||文本样式。 [LabelStyle](#labelstyle)|
|data|||图例的数据数组。数组项通常为一个字符串，每一项代表一个系列的 name（如果是饼图，也可以是饼图单个数据的 name）。 如果 data 没有被指定，会自动从当前系列中获取。指定data时里面的数据项和serie匹配时才会生效。
|icons|||自定义的图例标记图形。
|colors|||图例标记的颜色列表。
|background||v3.1.0|背景图样式。 [ImageStyle](#imagestyle)|
|padding||v3.1.0|图例标记和背景的间距。 [Padding](#padding)|
|positions||v3.6.0|图例标记的自定义位置列表。

```mdx-code-block
</APITable>
```

## LegendTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="LegendTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|unableColor|||文本颜色。

```mdx-code-block
</APITable>
```

## Level

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="Level">
```

|参数|默认|版本|描述|
|--|--|--|--|
|label|||文本标签样式。 [LabelStyle](#labelstyle)|
|upperLabel|||上方的文本标签样式。 [LabelStyle](#labelstyle)|
|itemStyle|||数据项样式。 [ItemStyle](#itemstyle)|

```mdx-code-block
</APITable>
```

## LevelStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="LevelStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|false||是否启用LevelStyle
|levels|||各层节点对应的配置。当enableLevels为true时生效，levels[0]对应的第一层的配置，levels[1]对应第二层，依次类推。当levels中没有对应层时用默认的设置。

```mdx-code-block
</APITable>
```

## Line

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## LineArrow

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent)

```mdx-code-block
<APITable name="LineArrow">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|||是否显示箭头。
|position|||箭头位置。<br/>`LineArrow.Position`:<br/>- `End`: 末端箭头<br/>- `Start`: 头端箭头<br/>|
|arrow|||箭头。 [ArrowStyle](#arrowstyle)|

```mdx-code-block
</APITable>
```

## LineStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [ISerieDataComponent](#iseriedatacomponent)

线条样式。 注： 修改 lineStyle 中的颜色不会影响图例颜色，如果需要图例颜色和折线图颜色一致，需修改 itemStyle.color，线条颜色默认也会取该颜色。 toColor，toColor2可设置水平方向的渐变，如需要设置垂直方向的渐变，可使用VisualMap。

```mdx-code-block
<APITable name="LineStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示线条。当作为子组件，它的父组件有参数控制是否显示时，改参数无效。
|type|||线的类型。<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|color|||线的颜色。
|toColor|||线的渐变颜色（需要水平方向渐变时）。
|toColor2|||线的渐变颜色2（需要水平方向三个渐变色的渐变时）。
|width|0||线宽。
|length|0||线长。
|opacity|1||线的透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
|dashLength|4|v3.8.1|虚线的长度。默认0时为线条宽度的12倍。在折线图中代表分割段数的倍数。
|dotLength|2|v3.8.1|点线的长度。默认0时为线条宽度的3倍。在折线图中代表分割段数的倍数。
|gapLength|2|v3.8.1|点线的长度。默认0时为线条宽度的3倍。在折线图中代表分割段数的倍数。

```mdx-code-block
</APITable>
```

## Location

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent), [IPropertyChanged](#ipropertychanged)

位置类型。通过Align快速设置大体位置，再通过left，right，top，bottom微调具体位置。

```mdx-code-block
<APITable name="Location">
```

|参数|默认|版本|描述|
|--|--|--|--|
|align|||对齐方式。<br/>`Location.Align`:<br/>- `TopLeft`: 对齐方式<br/>- `TopRight`: 对齐方式<br/>- `TopCenter`: 对齐方式<br/>- `BottomLeft`: 对齐方式<br/>- `BottomRight`: 对齐方式<br/>- `BottomCenter`: 对齐方式<br/>- `Center`: 对齐方式<br/>- `CenterLeft`: 对齐方式<br/>- `CenterRight`: 对齐方式<br/>|
|left|||离容器左侧的距离。
|right|||离容器右侧的距离。
|top|||离容器上侧的距离。
|bottom|||离容器下侧的距离。

```mdx-code-block
</APITable>
```

## MainComponent

> class in XCharts.Runtime / 继承自: [IComparable](https://docs.unity3d.com/ScriptReference/30_search.html?q=IComparable) / 子类: [Axis](#axis), [Background](#background), [Comment](#comment), [DataZoom](#datazoom), [Legend](#legend), [MarkArea](#markarea), [MarkLine](#markline), [Settings](#settings), [Title](#title), [Tooltip](#tooltip), [VisualMap](#visualmap), [GridLayout](#gridlayout), [CoordSystem](#coordsystem)

## MarkArea

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent)

图表标域，常用于标记图表中某个范围的数据。

```mdx-code-block
<APITable name="MarkArea">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示标域。
|text|||The text of markArea. 标域显示的文本。
|serieIndex|0||Serie index of markArea. 标域影响的Serie索引。
|start|||标域范围的起始数据。 [MarkAreaData](#markareadata)|
|end|||标域范围的结束数据。 [MarkAreaData](#markareadata)|
|itemStyle|||标域样式。 [ItemStyle](#itemstyle)|
|label|||标域文本样式。 [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## MarkAreaData

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

标域的数据。

```mdx-code-block
<APITable name="MarkAreaData">
```

|参数|默认|版本|描述|
|--|--|--|--|
|type|||特殊的标域类型，用于标注最大值最小值等。<br/>`MarkAreaType`:<br/>- `None`: 标域类型<br/>- `Min`: 最小值。<br/>- `Max`: 最大值。<br/>- `Average`: 平均值。<br/>- `Median`: 中位数。<br/>|
|name|||标注名称。会作为文字显示。
|dimension|1||从哪个维度的数据计算最大最小值等。
|xPosition|||相对原点的 x 坐标，单位像素。当type为None时有效。
|yPosition|||相对原点的 y 坐标，单位像素。当type为None时有效。
|xValue|||X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
|yValue|||Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。

```mdx-code-block
</APITable>
```

## MarkLine

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent)

图表标线。

```mdx-code-block
<APITable name="MarkLine">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示标线。
|serieIndex|0||标线影响的Serie索引。
|onTop|true|v3.9.0|是否在最上层。
|animation|||标线的动画样式。 [AnimationStyle](#animationstyle)|
|data|||标线的数据列表。当数据项的group为0时，每个数据项表示一条标线；当group不为0时，相同group的两个数据项分别表 示标线的起始点和终止点来组成一条标线，此时标线的相关样式参数取起始点的参数。

```mdx-code-block
</APITable>
```

## MarkLineData

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

> 从 `v3.9.0` 开始支持

图表标线的数据。

```mdx-code-block
<APITable name="MarkLineData">
```

|参数|默认|版本|描述|
|--|--|--|--|
|type|||特殊的标线类型，用于标注最大值最小值等。<br/>`MarkLineType`:<br/>- `None`: 标线类型<br/>- `Min`: 最小值。<br/>- `Max`: 最大值。<br/>- `Average`: 平均值。<br/>- `Median`: 中位数。<br/>|
|name|||标线名称，将会作为文字显示。label的formatter可通过{b}显示名称，通过{c}显示数值。
|dimension|1||从哪个维度的数据计算最大最小值等。
|xPosition|||相对原点的 x 坐标，单位像素。当type为None时有效。
|yPosition|||相对原点的 y 坐标，单位像素。当type为None时有效。
|xValue|||X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
|yValue|||Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
|group|0||分组。当group不为0时，表示这个data是标线的起点或终点，group一致的data组成一条标线。
|zeroPosition|false||是否为坐标系原点。
|startSymbol|||起始点的图形标记。 [SymbolStyle](#symbolstyle)|
|endSymbol|||结束点的图形标记。 [SymbolStyle](#symbolstyle)|
|lineStyle|||标线样式。 [LineStyle](#linestyle)|
|label|||文本样式。可设置position为Start、Middle和End在不同的位置显示文本。 [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## MarqueeStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

> 从 `v3.5.0` 开始支持

Marquee style. It can be used for the DataZoom component. 选取框样式。可用于DataZoom组件。

```mdx-code-block
<APITable name="MarqueeStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|apply|false|v3.5.0|选取框范围是否应用到DataZoom上。当为true时，框选结束后的范围即为DataZoom的选择范围。
|realRect|false|v3.5.0|是否选取实际框选区域。当为true时，以鼠标的其实点和结束点间的实际范围作为框选区域。
|areaStyle||v3.5.0|选取框区域填充样式。 [AreaStyle](#areastyle)|
|lineStyle||v3.5.0|选取框区域边框样式。 [LineStyle](#linestyle)|

```mdx-code-block
</APITable>
```

## MLValue

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

> 从 `v3.8.0` 开始支持

多样式数值。

```mdx-code-block
<APITable name="MLValue">
```

|参数|默认|版本|描述|
|--|--|--|--|
|type|||<br/>`MLValue.Type`:<br/>- `Percent`: 百分比形式。<br/>- `Absolute`: 绝对值形式。<br/>- `Extra`: 额外形式。<br/>|
|value|||

```mdx-code-block
</APITable>
```

## Padding

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent) / 子类: [TextPadding](#textpadding)

边距设置。

```mdx-code-block
<APITable name="Padding">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||show padding. 是否显示。
|top|0||顶部间距。
|right|2f||右部间距。
|left|2f||左边间距。
|bottom|0||底部间距。

```mdx-code-block
</APITable>
```

## Parallel

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## ParallelAxis

> class in XCharts.Runtime / 继承自: [Axis](#axis)

## ParallelCoord

> class in XCharts.Runtime / 继承自: [CoordSystem](#coordsystem), [IUpdateRuntimeData](#iupdateruntimedata), [ISerieContainer](#iseriecontainer)

Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.

```mdx-code-block
<APITable name="ParallelCoord">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示直角坐标系网格。
|orient|||坐标轴朝向。默认为垂直朝向。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|left|0.1f||grid 组件离容器左侧的距离。
|right|0.08f||grid 组件离容器右侧的距离。
|top|0.22f||grid 组件离容器上侧的距离。
|bottom|0.12f||grid 组件离容器下侧的距离。
|backgroundColor|||网格背景色，默认透明。

```mdx-code-block
</APITable>
```

## Pie

> class in XCharts.Runtime / 继承自: [Serie](#serie)

```mdx-code-block
<APITable name="Pie">
```

|参数|默认|版本|描述|
|--|--|--|--|
|radiusGradient|false|v3.8.1|是否开启半径方向的渐变效果。

```mdx-code-block
</APITable>
```

## PolarAxisTheme

> class in XCharts.Runtime / 继承自: [BaseAxisTheme](#baseaxistheme)

## PolarCoord

> class in XCharts.Runtime / 继承自: [CoordSystem](#coordsystem), [ISerieContainer](#iseriecontainer)

极坐标系组件。 极坐标系，可以用于散点图和折线图。每个极坐标系拥有一个角度轴和一个半径轴。

```mdx-code-block
<APITable name="PolarCoord">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示极坐标。
|center|||极坐标的中心点。数组的第一项是横坐标，第二项是纵坐标。 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
|radius|||半径。radius[0]表示内径，radius[1]表示外径。
|backgroundColor|||极坐标的背景色，默认透明。
|indicatorLabelOffset|30f|v3.8.0|指示器标签的偏移量。

```mdx-code-block
</APITable>
```

## Radar

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

```mdx-code-block
<APITable name="Radar">
```

|参数|默认|版本|描述|
|--|--|--|--|
|smooth|false|v3.2.0|是否平滑曲线。平滑曲线时不支持区域填充颜色。

```mdx-code-block
</APITable>
```

## RadarAxisTheme

> class in XCharts.Runtime / 继承自: [BaseAxisTheme](#baseaxistheme)

## RadarCoord

> class in XCharts.Runtime / 继承自: [CoordSystem](#coordsystem), [ISerieContainer](#iseriecontainer)

Radar coordinate conponnet for radar charts. 雷达图坐标系组件，只适用于雷达图。

## RadiusAxis

> class in XCharts.Runtime / 继承自: [Axis](#axis)

极坐标系的径向轴。

## RadiusAxisTheme

> class in XCharts.Runtime / 继承自: [BaseAxisTheme](#baseaxistheme)

## Ring

> class in XCharts.Runtime / 继承自: [Serie](#serie)

## Scatter

> class in XCharts.Runtime / 继承自: [BaseScatter](#basescatter)

## SelectStyle

> class in XCharts.Runtime / 继承自: [StateStyle](#statestyle), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

> 从 `v3.2.0` 开始支持

选中状态样式。

## Serie

> class in XCharts.Runtime / 继承自: [BaseSerie](#baseserie), [IComparable](https://docs.unity3d.com/ScriptReference/30_search.html?q=IComparable) / 子类: [SerieHandler&lt;T&gt;](#seriehandlert), [Bar](#bar), [SimplifiedBar](#simplifiedbar), [Candlestick](#candlestick), [SimplifiedCandlestick](#simplifiedcandlestick), [Heatmap](#heatmap), [Line](#line), [SimplifiedLine](#simplifiedline), [Parallel](#parallel), [Pie](#pie), [Radar](#radar), [Ring](#ring), [BaseScatter](#basescatter)

系列。系列一般由数据和配置组成，用来表示具体的图表图形，如折线图的一条折线，柱图的一组柱子等。一个图表中可以包含多个不同类型的系列。

```mdx-code-block
<APITable name="Serie">
```

|参数|默认|版本|描述|
|--|--|--|--|
|index|||系列索引。
|show|true||系列是否显示在图表上。
|coordSystem|||使用的坐标系。
|serieType|||系列类型。
|serieName|||系列名称，用于 tooltip 的显示，legend 的图例筛选。
|state||v3.2.0|系列的默认状态。<br/>`SerieState`:<br/>- `Normal`: 正常状态。<br/>- `Emphasis`: 高亮状态。<br/>- `Blur`: 淡出状态。<br/>- `Select`: 选中状态。<br/>- `Auto`: 自动保持和父节点一致。一般用在SerieData。<br/>|
|colorBy||v3.2.0|从主题中取色的策略。<br/>`SerieColorBy`:<br/>- `Default`: 默认策略。每种Serie都有自己的默认的取颜色策略。比如Line默认是Series策略，Pie默认是Data策略。<br/>- `Serie`: 按照系列分配调色盘中的颜色，同一系列中的所有数据都是用相同的颜色。<br/>- `Data`: 按照数据项分配调色盘中的颜色，每个数据项都使用不同的颜色。<br/>|
|stack|||数据堆叠，同个类目轴上系列配置相同的stack值后，后一个系列的值会在前一个系列的值上相加。
|xAxisIndex|0||使用X轴的index。
|yAxisIndex|0||使用Y轴的index。
|radarIndex|0||雷达图所使用的 radar 组件的 index。
|vesselIndex|0||水位图所使用的 vessel 组件的 index。
|polarIndex|0||所使用的 polar 组件的 index。
|singleAxisIndex|0||所使用的 singleAxis 组件的 index。
|parallelIndex|0||所使用的 parallel coord 组件的 index。
|gridIndex|-1|v3.8.0|所使用的 layout 组件的 index。 默认为-1不指定index, 当为大于或等于0时, 为第一个layout组件的第index个格子。
|minShow|||系列所显示数据的最小索引
|maxShow|||系列所显示数据的最大索引
|maxCache|||系列中可缓存的最大数据量。默认为0没有限制，大于0时超过指定值会移除旧数据再插入新数据。
|sampleDist|0||采样的最小像素距离，默认为0时不采样。当两个数据点间的水平距离小于改值时，开启采样，保证两点间的水平距离不小于改值。
|sampleType|||采样类型。当sampleDist大于0时有效。<br/>`SampleType`:<br/>- `Peak`: 取峰值。<br/>- `Average`: 取过滤点的平均值。<br/>- `Max`: 取过滤点的最大值。<br/>- `Min`: 取过滤点的最小值。<br/>- `Sum`: 取过滤点的和。<br/>|
|sampleAverage|0||设定的采样平均值。当sampleType 为 Peak 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为0时会实时计算所有数据的平均值。
|lineType|||折线图样式类型。<br/>`LineType`:<br/>- `Normal`: 普通折线图。<br/>- `Smooth`: 平滑曲线。<br/>- `StepStart`: 阶梯线图：当前点。<br/>- `StepMiddle`: 阶梯线图：当前点和下一个点的中间。<br/>- `StepEnd`: 阶梯线图：下一个拐点。<br/>|
|smoothLimit|false|v3.4.0|是否限制曲线。当为true时，两个连续相同数值的数据间的曲线会限制为不超出数据点，和数据点是平直的。
|barType|||柱形图类型。<br/>`BarType`:<br/>- `Normal`: 普通柱形图。<br/>- `Zebra`: 斑马柱形图。<br/>- `Capsule`: 胶囊柱形图。<br/>|
|barPercentStack|false||柱形图是否为百分比堆积。相同stack的serie只要有一个barPercentStack为true，则就显示成百分比堆叠柱状图。
|barWidth|0||柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。
|barMaxWidth|0|v3.5.0|柱条的最大宽度，默认为0为不限制最大宽度。支持设置成相对于类目宽度的百分比。
|barGap|0.1f||不同系列的柱间距离。为百分比（如 '0.3f'，表示柱子宽度的 30%） 如果想要两个系列的柱子重叠，可以设置 barGap 为 '-1f'。这在用柱子做背景的时候有用。 在同一坐标系上，此属性会被多个 'bar' 系列共享。此属性应设置于此坐标系中最后一个 'bar' 系列上才会生效，并且是对此坐标系中所有 'bar' 系列生效。
|barZebraWidth|4f||斑马线的粗细。
|barZebraGap|2f||斑马线的间距。
|min|||最小值。
|max|||最大值。
|minSize|0f||数据最小值 min 映射的宽度。
|maxSize|1f||数据最大值 max 映射的宽度。
|startAngle|||起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|endAngle|||结束角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|minAngle|||最小的扇区角度（0-360）。用于防止某个值过小导致扇区太小影响交互。
|clockwise|true||是否顺时针。
|roundCap|||是否开启圆弧效果。
|splitNumber|||刻度分割段数。最大可设置36。
|clickOffset|true||鼠标点击时是否开启偏移，一般用在PieChart图表中。
|roseType|||是否展示成南丁格尔图，通过半径区分数据大小。<br/>`RoseType`:<br/>- `None`: 不展示成南丁格尔玫瑰图。<br/>- `Radius`: 扇区圆心角展现数据的百分比，半径展现数据的大小。<br/>- `Area`: 所有扇区圆心角相同，仅通过半径展现数据大小。<br/>|
|gap|||间距。
|center|||中心点。
|radius|||半径。radius[0]表示内径，radius[1]表示外径。
|minRadius|0f|v3.8.0|最小半径。可用于限制玫瑰图的最小半径。
|showDataDimension|||数据项里的数据维数。
|showDataName|||在Editor的inpsector上是否显示name参数
|clip|false||是否裁剪超出坐标系部分的图形。
|ignore|false||是否开启忽略数据。当为 true 时，数据值为 ignoreValue 时不进行绘制。
|ignoreValue|0||忽略数据的默认值。当ignore为true才有效。
|ignoreLineBreak|false||忽略数据时折线是断开还是连接。默认false为连接。
|showAsPositiveNumber|false||将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
|large|true||是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。 开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。 缺点：优化后不能自定义设置单个数据项的样式，不能显示Label。
|largeThreshold|200||开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
|avoidLabelOverlap|false||在饼图且标签外部显示的情况下，是否启用防止标签重叠策略，默认关闭，在标签拥挤重叠的情况下会挪动各个标签的位置，防止标签间的重叠。
|radarType|||雷达图类型。<br/>`RadarType`:<br/>- `Multiple`: 多圈雷达图。此时可一个雷达里绘制多个圈，一个serieData就可组成一个圈（多维数据）。<br/>- `Single`: 单圈雷达图。此时一个雷达只能绘制一个圈，多个serieData组成一个圈，数据取自`data[1]`。<br/>|
|placeHolder|false||占位模式。占位模式时，数据有效但不参与渲染和显示。
|dataSortType|||组件的数据排序。<br/>`SerieDataSortType`:<br/>- `None`: 按数据的顺序。<br/>- `Ascending`: 升序。<br/>- `Descending`: 降序。<br/>|
|orient|||组件的朝向。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|align|||组件水平方向对齐方式。<br/>`Align`:<br/>- `Center`: 对齐方式。文本，图标，图形等的对齐方式。<br/>- `Left`: 对齐方式。文本，图标，图形等的对齐方式。<br/>- `Right`: 对齐方式。文本，图标，图形等的对齐方式。<br/>|
|left|||组件离容器左侧的距离。
|right|||组件离容器右侧的距离。
|top|||组件离容器上侧的距离。
|bottom|||组件离容器下侧的距离。
|insertDataToHead|||添加新数据时是在列表的头部还是尾部加入。
|lineStyle|||线条样式。 [LineStyle](#linestyle)|
|symbol|||标记的图形。 [SerieSymbol](#seriesymbol)|
|animation|||起始动画。 [AnimationStyle](#animationstyle)|
|itemStyle|||图形样式。 [ItemStyle](#itemstyle)|
|data|||系列中的数据内容数组。SerieData可以设置1到n维数据。

```mdx-code-block
</APITable>
```

## SerieData

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

系列中的一个数据项。可存储数据名和1-n维个数据。

```mdx-code-block
<APITable name="SerieData">
```

|参数|默认|版本|描述|
|--|--|--|--|
|index|||数据项索引。
|name|||数据项名称。
|id|||数据项的唯一id。唯一id不是必须设置的。
|parentId|||父节点id。父节点id不是必须设置的。
|ignore|||是否忽略数据。当为 true 时，数据不进行绘制。
|selected|||该数据项是否被选中。
|radius|||自定义半径。可用在饼图中自定义某个数据项的半径。
|state||v3.2.0|数据项的默认状态。<br/>`SerieState`:<br/>- `Normal`: 正常状态。<br/>- `Emphasis`: 高亮状态。<br/>- `Blur`: 淡出状态。<br/>- `Select`: 选中状态。<br/>- `Auto`: 自动保持和父节点一致。一般用在SerieData。<br/>|
|data|||可指定任意维数的数值列表。

```mdx-code-block
</APITable>
```

## SerieSymbol

> class in XCharts.Runtime / 继承自: [SymbolStyle](#symbolstyle), [ISerieDataComponent](#iseriedatacomponent)

系列数据项的标记的图形

```mdx-code-block
<APITable name="SerieSymbol">
```

|参数|默认|版本|描述|
|--|--|--|--|
|sizeType|||标记图形的大小获取方式。<br/>`SymbolSizeType`:<br/>- `Custom`: 自定义大小。<br/>- `FromData`: 通过 dataIndex 从数据中获取，再乘以一个比例系数 dataScale 。<br/>- `Function`: 通过委托函数获取。<br/>|
|dataIndex|1||当sizeType指定为FromData时，指定的数据源索引。
|dataScale|1||当sizeType指定为FromData时，指定的倍数系数。
|sizeFunction|||当sizeType指定为Function时，指定的委托函数。
|startIndex|||开始显示图形标记的索引。
|interval|||显示图形标记的间隔。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。
|forceShowLast|false||是否强制显示最后一个图形标记。
|repeat|false||图形是否重复。
|minSize|0f|v3.3.0|图形最小尺寸。只在sizeType为SymbolSizeType.FromData时有效。
|maxSize|0f|v3.3.0|图形最大尺寸。只在sizeType为SymbolSizeType.FromData时有效。

```mdx-code-block
</APITable>
```

## SerieTheme

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="SerieTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|lineWidth|||文本颜色。
|lineSymbolSize|||折线图的Symbol大小。
|scatterSymbolSize|||散点图的Symbol大小。
|candlestickColor|Color32(235, 84, 84, 255)||K线图阳线（涨）填充色
|candlestickColor0|Color32(71, 178, 98, 255)||K线图阴线（跌）填充色
|candlestickBorderWidth|1||K线图边框宽度
|candlestickBorderColor|Color32(235, 84, 84, 255)||K线图阳线（跌）边框色
|candlestickBorderColor0|Color32(71, 178, 98, 255)||K线图阴线（跌）边框色

```mdx-code-block
</APITable>
```

## Settings

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent)

全局参数设置组件。一般情况下可使用默认值，当有需要时可进行调整。

```mdx-code-block
<APITable name="Settings">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||
|maxPainter|10||设定的painter数量。
|reversePainter|false||Painter是否逆序。逆序时index大的serie最先绘制。
|basePainterMaterial|||Base Pointer 材质球，设置后会影响Axis等。
|seriePainterMaterial|||Serie Pointer 材质球，设置后会影响所有Serie。
|upperPainterMaterial|||Upper Pointer 材质球。
|topPainterMaterial|||Top Pointer 材质球。
|lineSmoothStyle|2.5f||曲线平滑系数。通过调整平滑系数可以改变曲线的曲率，得到外观稍微有变化的不同曲线。
|lineSmoothness|2f||When the area with gradient is filled, the larger the value, the worse the transition effect.
|lineSegmentDistance|3f||线段的分割距离。普通折线图的线是由很多线段组成，段数由该数值决定。值越小段数越多，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
|cicleSmoothness|2f||圆形的平滑度。数越小圆越平滑，但顶点数也会随之增加。
|legendIconLineWidth|2||Line类型图例图标的线条宽度。
|legendIconCornerRadius|||图例圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。
|axisMaxSplitNumber|50|v3.1.0|坐标轴最大分隔段数。段数过大时可能会生成较多的label节点。

```mdx-code-block
</APITable>
```

## SimplifiedBar

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer), [ISimplifiedSerie](#isimplifiedserie)

## SimplifiedCandlestick

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer), [ISimplifiedSerie](#isimplifiedserie)

## SimplifiedLine

> class in XCharts.Runtime / 继承自: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer), [ISimplifiedSerie](#isimplifiedserie)

## SingleAxis

> class in XCharts.Runtime / 继承自: [Axis](#axis), [IUpdateRuntimeData](#iupdateruntimedata)

单轴。

```mdx-code-block
<APITable name="SingleAxis">
```

|参数|默认|版本|描述|
|--|--|--|--|
|orient|||坐标轴朝向。默认为水平朝向。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|left|0.1f||组件离容器左侧的距离。
|right|0.1f||组件离容器右侧的距离。
|top|0f||组件离容器上侧的距离。
|bottom|0.2f||组件离容器下侧的距离。
|width|0||坐标轴宽。
|height|50||坐标轴高。

```mdx-code-block
</APITable>
```

## SingleAxisCoord

> class in XCharts.Runtime / 继承自: [CoordSystem](#coordsystem)

## StageColor

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="StageColor">
```

|参数|默认|版本|描述|
|--|--|--|--|
|percent|||结束位置百分比。
|color|||颜色。

```mdx-code-block
</APITable>
```

## StateStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent) / 子类: [BlurStyle](#blurstyle), [EmphasisStyle](#emphasisstyle), [SelectStyle](#selectstyle)

> 从 `v3.2.0` 开始支持

Serie的状态样式。Serie的状态有正常，高亮，淡出，选中四种状态。

```mdx-code-block
<APITable name="StateStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否启用高亮样式。
|label|||图形文本标签。 [LabelStyle](#labelstyle)|
|labelLine|||图形文本引导线样式。 [LabelLine](#labelline)|
|itemStyle|||图形样式。 [ItemStyle](#itemstyle)|
|lineStyle|||折线样式。 [LineStyle](#linestyle)|
|areaStyle|||区域样式。 [AreaStyle](#areastyle)|
|symbol|||标记样式。 [SerieSymbol](#seriesymbol)|

```mdx-code-block
</APITable>
```

## SubTitleTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme)

## SymbolStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent) / 子类: [SerieSymbol](#seriesymbol)

系列数据项的标记的图形

```mdx-code-block
<APITable name="SymbolStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示标记。
|type|||标记类型。<br/>`SymbolType`:<br/>- `None`: 不显示标记。<br/>- `Custom`: 自定义标记。<br/>- `Circle`: 圆形。<br/>- `EmptyCircle`: 空心圆。<br/>- `Rect`: 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。<br/>- `EmptyRect`: 空心正方形。<br/>- `Triangle`: 三角形。<br/>- `EmptyTriangle`: 空心三角形。<br/>- `Diamond`: 菱形。<br/>- `EmptyDiamond`: 空心菱形。<br/>- `Arrow`: 箭头。<br/>- `EmptyArrow`: 空心箭头。<br/>- `Plus`: 加号。<br/>- `Minus`: 减号。<br/>|
|size|0f||标记的大小。
|gap|0||图形标记和线条的间隙距离。
|width|0f||图形的宽。
|height|0f||图形的高。
|offset|Vector2.zero||图形的偏移。
|image|||自定义的标记图形。
|imageType|||图形填充类型。
|color|||图形的颜色。

```mdx-code-block
</APITable>
```

## TextLimit

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

文本字符限制和自适应。当文本长度超过设定的长度时进行裁剪，并将后缀附加在最后。 只在类目轴中有效。

```mdx-code-block
<APITable name="TextLimit">
```

|参数|默认|版本|描述|
|--|--|--|--|
|enable|false||是否启用文本自适应。 [default:true]
|maxWidth|0||Clipping occurs when the width of the text is greater than this value.
|gap|1||两边留白像素距离。 [default:10f]
|suffix|||长度超出时的后缀。 [default: "..."]

```mdx-code-block
</APITable>
```

## TextPadding

> class in XCharts.Runtime / 继承自: [Padding](#padding)

文本的内边距设置。

## TextStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

文本的相关设置。

```mdx-code-block
<APITable name="TextStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||文本的相关设置。
|font|||文本字体。 [default: null]
|autoWrap|false||是否自动换行。
|autoAlign|true||文本是否让系统自动选对齐方式。为false时才会用alignment。
|rotate|0||文本的旋转。 [default: `0f`]
|autoColor|false||是否开启自动颜色。当开启时，会自动设置颜色。
|color|||文本的颜色。 [default: `Color.clear`]
|fontSize|0||文本字体大小。 [default: 18]
|fontStyle|||文本字体的风格。 [default: FontStyle.Normal]
|lineSpacing|1f||行间距。 [default: 1f]
|alignment|||对齐方式。
|tMPFont|||TextMeshPro字体。
|tMPFontStyle|||
|tMPAlignment|||
|tMPSpriteAsset||v3.1.0|

```mdx-code-block
</APITable>
```

## Theme

> class in XCharts.Runtime / 继承自: [ScriptableObject](https://docs.unity3d.com/ScriptReference/30_search.html?q=ScriptableObject)

主题相关配置。

```mdx-code-block
<APITable name="Theme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|themeType|||主题类型。<br/>`ThemeType`:<br/>- `Default`: 默认主题。<br/>- `Light`: 亮主题。<br/>- `Dark`: 暗主题。<br/>- `Custom`: 自定义主题。<br/>|
|themeName|||主题名称。
|font|||主题字体。
|tMPFont|||主题字体。
|contrastColor|||对比色。
|backgroundColor|||背景颜色。
|colorPalette|||调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。
|common||| [ComponentTheme](#componenttheme)|
|title||| [TitleTheme](#titletheme)|
|subTitle||| [SubTitleTheme](#subtitletheme)|
|legend||| [LegendTheme](#legendtheme)|
|axis||| [AxisTheme](#axistheme)|
|tooltip||| [TooltipTheme](#tooltiptheme)|
|dataZoom||| [DataZoomTheme](#datazoomtheme)|
|visualMap||| [VisualMapTheme](#visualmaptheme)|
|serie||| [SerieTheme](#serietheme)|

```mdx-code-block
</APITable>
```

## ThemeStyle

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

主题相关配置。

```mdx-code-block
<APITable name="ThemeStyle">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||
|sharedTheme|||主题配置。 [Theme](#theme)|
|transparentBackground|false||是否透明背景颜色。当设置为true时，不绘制背景颜色。
|enableCustomTheme|false||是否自定义主题颜色。当设置为true时，可以用‘sync color to custom’同步主题的颜色到自定义颜色。也可以手动设置。
|customFont|||
|customBackgroundColor|||自定义的背景颜色。
|customColorPalette|||

```mdx-code-block
</APITable>
```

## Title

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent), [IPropertyChanged](#ipropertychanged)

标题组件，包含主标题和副标题。

```mdx-code-block
<APITable name="Title">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示标题组件。
|text|||主标题文本，支持使用 \n 换行。
|subText|||副标题文本，支持使用 \n 换行。
|labelStyle|||主标题文本样式。 [LabelStyle](#labelstyle)|
|subLabelStyle|||副标题文本样式。 [LabelStyle](#labelstyle)|
|itemGap|0||主副标题之间的间距。
|location|||标题显示位置。 [Location](#location)|

```mdx-code-block
</APITable>
```

## TitleStyle

> class in XCharts.Runtime / 继承自: [LabelStyle](#labelstyle), [ISerieDataComponent](#iseriedatacomponent), [ISerieComponent](#iseriecomponent)

标题相关设置。

## TitleTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme)

## Tooltip

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent)

提示框组件。

```mdx-code-block
<APITable name="Tooltip">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||是否显示提示框组件。
|type|||提示框指示器类型。<br/>`Tooltip.Type`:<br/>- `Line`: 直线指示器<br/>- `Shadow`: 阴影指示器<br/>- `None`: 无指示器<br/>- `Corss`: 十字准星指示器。坐标轴显示Label和交叉线。<br/>- `Auto`: 根据serie的类型自动选择显示指示器。<br/>|
|trigger|||触发类型。<br/>`Tooltip.Trigger`:<br/>- `Item`: 数据项图形触发，主要在散点图，饼图等无类目轴的图表中使用。<br/>- `Axis`: 坐标轴触发，主要在柱状图，折线图等会使用类目轴的图表中使用。<br/>- `None`: 什么都不触发。<br/>- `Auto`: 根据serie的类型自动选择触发类型。<br/>|
|position||v3.3.0|显示位置类型。<br/>`Tooltip.Position`:<br/>- `Auto`: 自适应。移动平台靠顶部显示，非移动平台跟随鼠标位置。<br/>- `Custom`: 自定义。完全自定义显示位置(x,y)。<br/>- `FixedX`: 只固定坐标X。Y跟随鼠标位置。<br/>- `FixedY`: <br/>|
|itemFormatter|||提示框单个serie或数据项内容的字符串模版格式器。支持用 \n 换行。用|来表示多个列的分隔。 模板变量有{.}、{a}、{b}、{c}、{d}、{e}、{f}、{g}。<br/> {i}或-表示忽略当前项。 {.}为当前所指示的serie或数据项的对应颜色的圆点。<br/> {a}为当前所指示的serie或数据项的系列名name。<br/> {b}为当前所指示的serie或数据项的数据项serieData的name，或者类目值（如折线图的X轴）。<br/> {c}为当前所指示的serie或数据项的y维（dimesion为1）的数值。<br/> {d}为当前所指示的serie或数据项的y维（dimesion为1）百分比值，注意不带%号。<br/> {e}为当前所指示的serie或数据项的数据项serieData的name。<br/> {f}为当前所指示的serie的默认维度的数据总和。<br/> {g}为当前所指示的serie的数据总个数。<br/> {h}为当前所指示的serie的十六进制颜色值。<br/> {c0}表示当前数据项维度为0的数据。<br/> {c1}表示当前数据项维度为1的数据。<br/> {d3}表示维度3的数据的百分比。它的分母是默认维度（一般是1维度）数据。<br/> |表示多个列的分隔。<br/> 示例："{i}", "{.}|{a}|{c}", "{.}|{b}|{c2:f2}"
|titleFormatter|||提示框标题内容的字符串模版格式器。支持用 \n 换行。可以单独设置占位符{i}表示忽略不显示title。 模板变量有{.}、{a}、{b}、{c}、{d}、{e}、{f}、{g}。<br/> {.}为当前所指示或index为0的serie的对应颜色的圆点。<br/> {a}为当前所指示或index为0的serie的系列名name。<br/> {b}为当前所指示或index为0的serie的数据项serieData的name，或者类目值（如折线图的X轴）。<br/> {c}为当前所指示或index为0的serie的y维（dimesion为1）的数值。<br/> {d}为当前所指示或index为0的serie的y维（dimesion为1）百分比值，注意不带%号。<br/> {e}为当前所指示或index为0的serie的数据项serieData的name。<br/> {h}为当前所指示或index为0的serie的数据项serieData的十六进制颜色值。<br/> {f}为数据总和。<br/> {g}为数据总个数。<br/> {.1}表示指定index为1的serie对应颜色的圆点。<br/> {a1}、{b1}、{c1}中的1表示指定index为1的serie。<br/> {c1:2}表示索引为1的serie的当前指示数据项的第3个数据（一个数据项有多个数据，index为2表示第3个数据）。<br/> {c1:2-2}表示索引为1的serie的第3个数据项的第3个数据（也就是要指定第几个数据项时必须要指定第几个数据）。<br/> {d1:2:f2}表示单独指定了数值的格式化字符串为f2（不指定时用numericFormatter）。<br/> {d:0.##} 表示单独指定了数值的格式化字符串为 0.## （用于百分比，保留2位有效数同时又能避免使用 f2 而出现的类似于"100.00%"的情况 ）。<br/> 示例："{a}:{c}"、"{a1}:{c1:f1}"、"{a1}:{c1:0:f1}"、"{a1}:{c1:1-1:f1}"
|marker|||serie的符号标志。
|fixedWidth|0||固定宽度。比 minWidth 优先。
|fixedHeight|0||固定高度。比 minHeight 优先。
|minWidth|0||最小宽度。如若 fixedWidth 设有值，优先取 fixedWidth。
|minHeight|0||最小高度。如若 fixedHeight 设有值，优先取 fixedHeight。
|numericFormatter|||标准数字和日期格式字符串。用于将Double数值或DateTime日期格式化显示为字符串。numericFormatter用来作为Double.ToString()或DateTime.ToString()的参数。<br/> 数字格式使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。如：F1, E2<br/> 日期格式常见的格式：yyyy年，MM月，dd日，HH时，mm分，ss秒，fff毫秒。如：yyyy-MM-dd HH:mm:ss<br/> 数值格式化参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings <br/> 日期格式化参考：https://learn.microsoft.com/zh-cn/dotnet/standard/base-types/standard-date-and-time-format-strings
|paddingLeftRight|10||左右边距。
|paddingTopBottom|10||上下边距。
|ignoreDataShow|false||是否显示忽略数据在tooltip上。
|ignoreDataDefaultContent|||被忽略数据的默认显示字符信息。如果设置为空，则表示完全不显示忽略数据。
|showContent|true||是否显示提示框浮层，默认显示。只需tooltip触发事件或显示axisPointer而不需要显示内容时可配置该项为false。
|alwayShowContent|false||是否触发后一直显示提示框浮层。
|offset|Vector2(18f, -25f)||提示框相对于鼠标位置的偏移。
|backgroundImage|||提示框的背景图片。
|backgroundType|||提示框的背景图片显示类型。
|backgroundColor|||提示框的背景颜色。
|borderWidth|2f||边框线宽。
|fixedX|0f||固定X位置的坐标。
|fixedY|0.7f||固定Y位置的坐标。
|titleHeight|25f||标题文本的高。
|itemHeight|25f||数据项文本的高。
|borderColor|Color32(230, 230, 230, 255)||边框颜色。
|lineStyle|||指示线样式。 [LineStyle](#linestyle)|
|titleLabelStyle|||标题的文本样式。 [LabelStyle](#labelstyle)|
|contentLabelStyles|||内容部分的文本样式列表。和列一一对应。

```mdx-code-block
</APITable>
```

## TooltipTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="TooltipTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|lineType|||坐标轴线类型。<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|lineWidth|1f||指示线线宽。
|lineColor|||指示线颜色。
|areaColor|||区域指示的颜色。
|labelTextColor|||十字指示器坐标轴标签的文本颜色。
|labelBackgroundColor|||十字指示器坐标轴标签的背景颜色。

```mdx-code-block
</APITable>
```

## UIComponentTheme

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="UIComponentTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||
|sharedTheme|||主题配置。 [Theme](#theme)|
|transparentBackground|false||

```mdx-code-block
</APITable>
```

## VisualMap

> class in XCharts.Runtime / 继承自: [MainComponent](#maincomponent)

视觉映射组件。用于进行『视觉编码』，也就是将数据映射到视觉元素（视觉通道）。

```mdx-code-block
<APITable name="VisualMap">
```

|参数|默认|版本|描述|
|--|--|--|--|
|show|true||组件是否生效。
|showUI|false||是否显示组件。如果设置为 false，不会显示，但是数据映射的功能还存在。
|type|||组件类型。<br/>`VisualMap.Type`:<br/>- `Continuous`: 连续型。<br/>- `Piecewise`: 分段型。<br/>|
|selectedMode|||选择模式。<br/>`VisualMap.SelectedMode`:<br/>- `Multiple`: 多选。<br/>- `Single`: 单选。<br/>|
|serieIndex|0||影响的serie索引。
|min|0||范围最小值
|max|0||范围最大值
|range|||指定手柄对应数值的位置。range 应在[min,max]范围内。
|text|||两端的文本，如 ['High', 'Low']。
|textGap|||两端文字主体之间的距离，单位为px。
|splitNumber|5||对于连续型数据，自动平均切分成几段，默认为0时自动匹配inRange颜色列表大小。
|calculable|false||是否显示拖拽用的手柄（手柄能拖拽调整选中范围）。
|realtime|true||拖拽时，是否实时更新。
|itemWidth|20f||图形的宽度，即颜色条的宽度。
|itemHeight|140f||图形的高度，即颜色条的高度。
|itemGap|10f||每个图元之间的间隔距离。
|borderWidth|0||边框线宽，单位px。
|dimension|-1||Starting at 1, the default is 0 to take the last dimension in data.
|hoverLink|true||Conversely, when the mouse hovers over a graphic element in a diagram, the corresponding value of the visualMap component is triangulated in the corresponding position.
|autoMinMax|true||Automatically set min, Max value 自动设置min，max的值
|orient|||布局方式是横还是竖。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|location|||组件显示的位置。 [Location](#location)|
|workOnLine|true||组件是否对LineChart的LineStyle有效。
|workOnArea|false||组件是否对LineChart的AreaStyle有效。
|outOfRange|||定义 在选中范围外 的视觉颜色。
|inRange|||分段式每一段的相关配置。

```mdx-code-block
</APITable>
```

## VisualMapRange

> class in XCharts.Runtime / 继承自: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="VisualMapRange">
```

|参数|默认|版本|描述|
|--|--|--|--|
|min|||范围最小值
|max|||范围最大值
|label|||文字描述
|color|||颜色

```mdx-code-block
</APITable>
```

## VisualMapTheme

> class in XCharts.Runtime / 继承自: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="VisualMapTheme">
```

|参数|默认|版本|描述|
|--|--|--|--|
|borderWidth|||边框线宽。
|borderColor|||边框颜色。
|backgroundColor|||背景颜色。
|triangeLen|20f||可视化组件的调节三角形边长。

```mdx-code-block
</APITable>
```

## XAxis

> class in XCharts.Runtime / 继承自: [Axis](#axis)

直角坐标系 grid 中的 x 轴。

## XCResourcesImporter

> class in XCharts.Runtime

## XCSettings

> class in XCharts.Runtime / 继承自: [ScriptableObject](https://docs.unity3d.com/ScriptReference/30_search.html?q=ScriptableObject)

```mdx-code-block
<APITable name="XCSettings">
```

|参数|默认|版本|描述|
|--|--|--|--|
|lang||| [Lang](#lang)|
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
|customThemes|||

```mdx-code-block
</APITable>
```

## YAxis

> class in XCharts.Runtime / 继承自: [Axis](#axis)

直角坐标系 grid 中的 y 轴。

