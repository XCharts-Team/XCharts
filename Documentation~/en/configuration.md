---
sidebar_position: 31
slug: /configuration
---
import APITable from '@site/src/components/APITable';

# Chart Configuration

## Serie

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


## Theme

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


## MainComponent

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


## ChildComponent

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


## ISerieComponent

- [AreaStyle](#areastyle)
- [BlurStyle](#blurstyle)
- [EmphasisStyle](#emphasisstyle)
- [ImageStyle](#imagestyle)
- [LabelLine](#labelline)
- [LabelStyle](#labelstyle)
- [LineArrow](#linearrow)
- [SelectStyle](#selectstyle)
- [TitleStyle](#titlestyle)


## ISerieDataComponent

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


## Other

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

> class in XCharts.Runtime / Inherits from: [Axis](#axis)

Angle axis of Polar Coordinate.

```mdx-code-block
<APITable name="AngleAxis">
```


|field|default|since|comment|
|--|--|--|--|
|startAngle|0||Starting angle of axis. 0 degrees by default, standing for right position of center.

```mdx-code-block
</APITable>
```

## AngleAxisTheme

> class in XCharts.Runtime / Inherits from: [BaseAxisTheme](#baseaxistheme)

## AnimationAddition

> class in XCharts.Runtime / Inherits from: [AnimationInfo](#animationinfo)

> Since `v3.8.0`

Data addition animation.

## AnimationChange

> class in XCharts.Runtime / Inherits from: [AnimationInfo](#animationinfo)

> Since `v3.8.0`

Data change animation.

## AnimationFadeIn

> class in XCharts.Runtime / Inherits from: [AnimationInfo](#animationinfo)

> Since `v3.8.0`

Fade in animation.

## AnimationFadeOut

> class in XCharts.Runtime / Inherits from: [AnimationInfo](#animationinfo)

> Since `v3.8.0`

Fade out animation.

## AnimationHiding

> class in XCharts.Runtime / Inherits from: [AnimationInfo](#animationinfo)

> Since `v3.8.0`

Data hiding animation.

## AnimationInfo

> class in XCharts.Runtime / Subclasses: [AnimationFadeIn](#animationfadein), [AnimationFadeOut](#animationfadeout), [AnimationChange](#animationchange), [AnimationAddition](#animationaddition), [AnimationHiding](#animationhiding), [AnimationInteraction](#animationinteraction)

> Since `v3.8.0`

the animation info.

```mdx-code-block
<APITable name="AnimationInfo">
```


|field|default|since|comment|
|--|--|--|--|
|enable|true|v3.8.0|whether enable animation.
|reverse|false|v3.8.0|whether enable reverse animation.
|delay|0|v3.8.0|the delay time before animation start.
|duration|1000|v3.8.0|the duration of animation.

```mdx-code-block
</APITable>
```

## AnimationInteraction

> class in XCharts.Runtime / Inherits from: [AnimationInfo](#animationinfo)

> Since `v3.8.0`

Interactive animation of charts.

```mdx-code-block
<APITable name="AnimationInteraction">
```


|field|default|since|comment|
|--|--|--|--|
|width||v3.8.0|the mlvalue of width. [MLValue](#mlvalue)|
|radius||v3.8.0|the mlvalue of radius. [MLValue](#mlvalue)|
|offset||v3.8.0|the mlvalue of offset. Such as the offset of the pie chart when the sector is selected. [MLValue](#mlvalue)|

```mdx-code-block
</APITable>
```

## AnimationStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

the animation of serie. support animation type: fadeIn, fadeOut, change, addition.

```mdx-code-block
<APITable name="AnimationStyle">
```


|field|default|since|comment|
|--|--|--|--|
|enable|true||Whether to enable animation.
|type|||The type of animation.<br/>`AnimationType`:<br/>- `Default`: he default. An animation playback mode will be selected according to the actual situation.<br/>- `LeftToRight`: Play the animation from left to right.<br/>- `BottomToTop`: Play the animation from bottom to top.<br/>- `InsideOut`: Play animations from the inside out.<br/>- `AlongPath`: Play the animation along the path.<br/>- `Clockwise`: Play the animation clockwise.<br/>|
|easting|||<br/>`AnimationEasing`:<br/>- `Linear`: <br/>|
|threshold|2000||Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
|unscaledTime||v3.4.0|Animation updates independently of Time.timeScale.
|fadeIn||v3.8.0|Fade in animation configuration. [AnimationFadeIn](#animationfadein)|
|fadeOut||v3.8.0|Fade out animation configuration. [AnimationFadeOut](#animationfadeout)|
|change||v3.8.0|Update data animation configuration. [AnimationChange](#animationchange)|
|addition||v3.8.0|Add data animation configuration. [AnimationAddition](#animationaddition)|
|hiding||v3.8.0|Data hiding animation configuration. [AnimationHiding](#animationhiding)|
|interaction||v3.8.0|Interaction animation configuration. [AnimationInteraction](#animationinteraction)|

```mdx-code-block
</APITable>
```

## AreaStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

The style of area.

```mdx-code-block
<APITable name="AreaStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent the areafrom showing.
|origin|||the origin of area.<br/>`AreaStyle.AreaOrigin`:<br/>- `Auto`: to fill between axis line to data.<br/>- `Start`: to fill between min axis value (when not inverse) to data.<br/>- `End`: to fill between max axis value (when not inverse) to data.<br/>|
|color|||the color of area,default use serie color.
|toColor|||Gradient color, start color to toColor.
|opacity|0.6f||Opacity of the component. Supports value from 0 to 1, and the component will not be drawn when set to 0.
|innerFill||v3.2.0|Whether to fill only polygonal areas. Currently, only convex polygons are supported.
|toTop|true|v3.6.0|Whether to fill the gradient color to the top. The default is true, which means that the gradient color is filled to the top. If it is false, the gradient color is filled to the actual position.

```mdx-code-block
</APITable>
```

## ArrowStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="ArrowStyle">
```


|field|default|since|comment|
|--|--|--|--|
|width|10||The widht of arrow.
|height|15||The height of arrow.
|offset|0||The offset of arrow.
|dent|3||The dent of arrow.
|color|Color.clear||the color of arrow.

```mdx-code-block
</APITable>
```

## Axis

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent) / Subclasses: [AngleAxis](#angleaxis), [ParallelAxis](#parallelaxis), [RadiusAxis](#radiusaxis), [SingleAxis](#singleaxis), [XAxis](#xaxis), [YAxis](#yaxis)

The axis in rectangular coordinate.

```mdx-code-block
<APITable name="Axis">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show axis.
|type|||the type of axis.<br/>`Axis.AxisType`:<br/>- `Value`: Numerical axis, suitable for continuous data.<br/>- `Category`: Category axis, suitable for discrete category data. Data should only be set via data for this type.<br/>- `Log`: Log axis, suitable for log data.<br/>- `Time`: Time axis, suitable for continuous time series data.<br/>|
|minMaxType|||the type of axis minmax.<br/>`Axis.AxisMinMaxType`:<br/>- `Default`: 0 - maximum.<br/>- `MinMax`: minimum - maximum.<br/>- `Custom`: Customize the minimum and maximum.<br/>- `MinMaxAuto`: [since("v3.7.0")]minimum - maximum, automatically calculate the appropriate values.<br/>|
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
|axisLine|||axis Line. [AxisLine](#axisline)|
|axisName|||axis name. [AxisName](#axisname)|
|axisTick|||axis tick. [AxisTick](#axistick)|
|axisLabel|||axis label. [AxisLabel](#axislabel)|
|splitLine|||axis split line. [AxisSplitLine](#axissplitline)|
|splitArea|||axis split area. [AxisSplitArea](#axissplitarea)|
|animation|||animation of axis. [AxisAnimation](#axisanimation)|
|minorTick||v3.2.0|axis minor tick. [AxisMinorTick](#axisminortick)|
|minorSplitLine||v3.2.0|axis minor split line. [AxisMinorSplitLine](#axisminorsplitline)|
|indicatorLabel||v3.4.0|Style of axis tooltip indicator label. [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## AxisAnimation

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

> Since `v3.9.0`

animation style of axis.

```mdx-code-block
<APITable name="AxisAnimation">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||whether to enable animation.
|duration|||the duration of animation (ms). When it is set to 0, the animation duration will be automatically calculated according to the serie.
|unscaledTime|||Animation updates independently of Time.timeScale.

```mdx-code-block
</APITable>
```

## AxisLabel

> class in XCharts.Runtime / Inherits from: [LabelStyle](#labelstyle)

Settings related to axis label.

```mdx-code-block
<APITable name="AxisLabel">
```


|field|default|since|comment|
|--|--|--|--|
|interval|0||The display interval of the axis label.
|inside|false||Set this to true so the axis labels face the inside direction.
|showAsPositiveNumber|false||Show negative number as positive number.
|onZero|false||刻度标签显示在0刻度上。
|showStartLabel|true||Whether to display the first label.
|showEndLabel|true||Whether to display the last label.
|textLimit|||文本限制。 [TextLimit](#textlimit)|

```mdx-code-block
</APITable>
```

## AxisLine

> class in XCharts.Runtime / Inherits from: [BaseLine](#baseline)

Settings related to axis line.

```mdx-code-block
<APITable name="AxisLine">
```


|field|default|since|comment|
|--|--|--|--|
|onZero|||When mutiple axes exists, this option can be used to specify which axis can be "onZero" to.
|showArrow|||Whether to show the arrow symbol of axis.
|arrow|||the arrow of line. [ArrowStyle](#arrowstyle)|

```mdx-code-block
</APITable>
```

## AxisMinorSplitLine

> class in XCharts.Runtime / Inherits from: [BaseLine](#baseline)

> Since `v3.2.0`

Minor split line of axis in grid area.

```mdx-code-block
<APITable name="AxisMinorSplitLine">
```


|field|default|since|comment|
|--|--|--|--|
|distance|||The distance between the split line and axis line.
|autoColor|||auto color.

```mdx-code-block
</APITable>
```

## AxisMinorTick

> class in XCharts.Runtime / Inherits from: [BaseLine](#baseline)

> Since `v3.2.0`

Settings related to axis minor tick.

```mdx-code-block
<APITable name="AxisMinorTick">
```


|field|default|since|comment|
|--|--|--|--|
|splitNumber|5||Number of segments that the axis is split into.
|autoColor|||

```mdx-code-block
</APITable>
```

## AxisName

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

the name of axis.

```mdx-code-block
<APITable name="AxisName">
```


|field|default|since|comment|
|--|--|--|--|
|show|||Whether to show axis name.
|name|||the name of axis.
|onZero||v3.1.0|Whether the axis name position are the same with 0 position of YAxis.
|labelStyle|||The text style of axis name. [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## AxisSplitArea

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

Split area of axis in grid area, not shown by default.

```mdx-code-block
<APITable name="AxisSplitArea">
```


|field|default|since|comment|
|--|--|--|--|
|show|||Set this to true to show the splitArea.
|color|||Color of split area. SplitArea color could also be set in color array, which the split lines would take as their colors in turns. Dark and light colors in turns are used by default.

```mdx-code-block
</APITable>
```

## AxisSplitLine

> class in XCharts.Runtime / Inherits from: [BaseLine](#baseline)

Split line of axis in grid area.

```mdx-code-block
<APITable name="AxisSplitLine">
```


|field|default|since|comment|
|--|--|--|--|
|interval|||Interval of Axis splitLine.
|distance|||The distance between the split line and axis line.
|autoColor|||auto color.
|showStartLine|true|v3.3.0|Whether to show the first split line.
|showEndLine|true|v3.3.0|Whether to show the last split line.

```mdx-code-block
</APITable>
```

## AxisTheme

> class in XCharts.Runtime / Inherits from: [BaseAxisTheme](#baseaxistheme)

## AxisTick

> class in XCharts.Runtime / Inherits from: [BaseLine](#baseline)

Settings related to axis tick.

```mdx-code-block
<APITable name="AxisTick">
```


|field|default|since|comment|
|--|--|--|--|
|alignWithLabel|||Align axis tick with label, which is available only when boundaryGap is set to be true in category axis.
|inside|||Set this to true so the axis labels face the inside direction.
|showStartTick|||Whether to display the first tick.
|showEndTick|||Whether to display the last tick.
|distance|||The distance between the tick line and axis line.
|splitNumber|0||Number of segments that the axis is split into.
|autoColor|||

```mdx-code-block
</APITable>
```

## Background

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent)

Background component.

```mdx-code-block
<APITable name="Background">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to enable the background component.
|image|||the image of background.
|imageType|||the fill type of background image.
|imageColor|||背景图颜色。
|autoColor|true||Whether to use theme background color for component color when the background component is on.

```mdx-code-block
</APITable>
```

## Bar

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## BaseAxisTheme

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme) / Subclasses: [AxisTheme](#axistheme), [RadiusAxisTheme](#radiusaxistheme), [AngleAxisTheme](#angleaxistheme), [PolarAxisTheme](#polaraxistheme), [RadarAxisTheme](#radaraxistheme)

```mdx-code-block
<APITable name="BaseAxisTheme">
```


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

```mdx-code-block
</APITable>
```

## BaseLine

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent) / Subclasses: [AxisLine](#axisline), [AxisMinorSplitLine](#axisminorsplitline), [AxisMinorTick](#axisminortick), [AxisSplitLine](#axissplitline), [AxisTick](#axistick)

Settings related to base line.

```mdx-code-block
<APITable name="BaseLine">
```


|field|default|since|comment|
|--|--|--|--|
|show|||Set this to false to prevent the axis line from showing.
|lineStyle|||线条样式 [LineStyle](#linestyle)|

```mdx-code-block
</APITable>
```

## BaseScatter

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer) / Subclasses: [EffectScatter](#effectscatter), [Scatter](#scatter)

## BaseSerie

> class in XCharts.Runtime / Subclasses: [Serie](#serie)

## BlurStyle

> class in XCharts.Runtime / Inherits from: [StateStyle](#statestyle), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

> Since `v3.2.0`

Configurations of blur state.

## CalendarCoord

> class in XCharts.Runtime / Inherits from: [CoordSystem](#coordsystem), [IUpdateRuntimeData](#iupdateruntimedata), [ISerieContainer](#iseriecontainer)

## Candlestick

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## ChartText

> class in XCharts.Runtime

## ChildComponent

> class in XCharts.Runtime / Subclasses: [AnimationStyle](#animationstyle), [AxisAnimation](#axisanimation), [AxisName](#axisname), [AxisSplitArea](#axissplitarea), [AreaStyle](#areastyle), [ArrowStyle](#arrowstyle), [BaseLine](#baseline), [IconStyle](#iconstyle), [ImageStyle](#imagestyle), [ItemStyle](#itemstyle), [Level](#level), [LevelStyle](#levelstyle), [LineArrow](#linearrow), [LineStyle](#linestyle), [Location](#location), [MLValue](#mlvalue), [MarqueeStyle](#marqueestyle), [Padding](#padding), [StageColor](#stagecolor), [SymbolStyle](#symbolstyle), [TextLimit](#textlimit), [TextStyle](#textstyle), [CommentItem](#commentitem), [CommentMarkStyle](#commentmarkstyle), [LabelLine](#labelline), [LabelStyle](#labelstyle), [MarkAreaData](#markareadata), [MarkLineData](#marklinedata), [StateStyle](#statestyle), [VisualMapRange](#visualmaprange), [UIComponentTheme](#uicomponenttheme), [SerieData](#seriedata), [ComponentTheme](#componenttheme), [SerieTheme](#serietheme), [ThemeStyle](#themestyle)

## Comment

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent), [IPropertyChanged](#ipropertychanged)

comment of chart.

```mdx-code-block
<APITable name="Comment">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent the comment from showing.
|labelStyle|||The text style of all comments. [LabelStyle](#labelstyle)|
|markStyle|||The text style of all comments. [CommentMarkStyle](#commentmarkstyle)|
|items|||The items of comment.

```mdx-code-block
</APITable>
```

## CommentItem

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

comment of chart.

```mdx-code-block
<APITable name="CommentItem">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent this comment item from showing.
|content|||content of comment.
|markRect|||the mark rect of comment.
|markStyle|||the mark rect style. [CommentMarkStyle](#commentmarkstyle)|
|labelStyle|||The text style of all comments. [LabelStyle](#labelstyle)|
|location||v3.5.0|The location of comment. [Location](#location)|

```mdx-code-block
</APITable>
```

## CommentMarkStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

the comment mark style.

```mdx-code-block
<APITable name="CommentMarkStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Set this to false to prevent this comment item from showing.
|lineStyle|||line style of comment mark area. [LineStyle](#linestyle)|

```mdx-code-block
</APITable>
```

## ComponentTheme

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent) / Subclasses: [BaseAxisTheme](#baseaxistheme), [DataZoomTheme](#datazoomtheme), [LegendTheme](#legendtheme), [SubTitleTheme](#subtitletheme), [TitleTheme](#titletheme), [TooltipTheme](#tooltiptheme), [VisualMapTheme](#visualmaptheme)

```mdx-code-block
<APITable name="ComponentTheme">
```


|field|default|since|comment|
|--|--|--|--|
|font|||the font of text.
|textColor|||the color of text.
|textBackgroundColor|||the color of text.
|fontSize|18||the font size of text.
|tMPFont|||the font of chart text。

```mdx-code-block
</APITable>
```

## CoordSystem

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent) / Subclasses: [RadarCoord](#radarcoord), [CalendarCoord](#calendarcoord), [GridCoord](#gridcoord), [ParallelCoord](#parallelcoord), [PolarCoord](#polarcoord), [SingleAxisCoord](#singleaxiscoord)

Coordinate system component.

## DataZoom

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent), [IUpdateRuntimeData](#iupdateruntimedata)

DataZoom component is used for zooming a specific area, which enables user to investigate data in detail, or get an overview of the data, or get rid of outlier points.

```mdx-code-block
<APITable name="DataZoom">
```


|field|default|since|comment|
|--|--|--|--|
|enable|true||Whether to show dataZoom.
|filterMode|||The mode of data filter.<br/>`DataZoom.FilterMode`:<br/>- `Filter`: data that outside the window will be filtered, which may lead to some changes of windows of other axes. For each data item, it will be filtered if one of the relevant dimensions is out of the window.<br/>- `WeakFilter`: data that outside the window will be filtered, which may lead to some changes of windows of other axes. For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.<br/>- `Empty`: data that outside the window will be set to NaN, which will not lead to changes of windows of other axes.<br/>- `None`: Do not filter data.<br/>|
|xAxisIndexs|||Specify which xAxis is controlled by the dataZoom.
|yAxisIndexs|||Specify which yAxis is controlled by the dataZoom.
|supportInside|||Whether built-in support is supported. Built into the coordinate system to allow the user to zoom in and out of the coordinate system by mouse dragging, mouse wheel, finger swiping (on the touch screen).
|supportInsideScroll|true||Whether inside scrolling is supported.
|supportInsideDrag|true||Whether insde drag is supported.
|supportSlider|||Whether a slider is supported. There are separate sliders on which the user zooms or roams.
|supportMarquee|||Supported Box Selected. Provides a marquee for scaling the data area.
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
|minShowNum|2||Minimum number of display data. Minimum number of data displayed when DataZoom is enlarged to maximum.
|scrollSensitivity|1.1f||The sensitivity of dataZoom scroll. The larger the number, the more sensitive it is.
|orient|||Specify whether the layout of dataZoom component is horizontal or vertical. What's more, it indicates whether the horizontal axis or vertical axis is controlled by default in catesian coordinate system.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|labelStyle|||label style. [LabelStyle](#labelstyle)|
|lineStyle|||阴影线条样式。 [LineStyle](#linestyle)|
|areaStyle|||阴影填充样式。 [AreaStyle](#areastyle)|
|marqueeStyle||v3.5.0|选取框样式。 [MarqueeStyle](#marqueestyle)|
|startLock||v3.6.0|Lock start value.
|endLock||v3.6.0|Lock end value.

```mdx-code-block
</APITable>
```

## DataZoomTheme

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="DataZoomTheme">
```


|field|default|since|comment|
|--|--|--|--|
|borderWidth|||the width of border line.
|dataLineWidth|||the width of data line.
|fillerColor|||the color of dataZoom data area.
|borderColor|||the color of dataZoom border.
|dataLineColor|||the color of data area line.
|dataAreaColor|||the color of data area line.
|backgroundColor|||the background color of datazoom.

```mdx-code-block
</APITable>
```

## DebugInfo

> class in XCharts.Runtime

```mdx-code-block
<APITable name="DebugInfo">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether show debug component.
|showDebugInfo|false||
|showAllChartObject|false||Whether show children components of chart in hierarchy view.
|foldSeries|false||Whether to fold series in inspector view.
|labelStyle||| [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## EffectScatter

> class in XCharts.Runtime / Inherits from: [BaseScatter](#basescatter)

## EmphasisStyle

> class in XCharts.Runtime / Inherits from: [StateStyle](#statestyle), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

> Since `v3.2.0`

Configurations of emphasis state.

```mdx-code-block
<APITable name="EmphasisStyle">
```


|field|default|since|comment|
|--|--|--|--|
|scale|1.1f||Whether to scale to highlight the data in emphasis state.
|focus|||When the data is highlighted, whether to fade out of other data to focus the highlighted.<br/>`EmphasisStyle.FocusType`:<br/>- `None`: Do not fade out other data, it's by default.<br/>- `Self`: Only focus (not fade out) the element of the currently highlighted data.<br/>- `Series`: Focus on all elements of the series which the currently highlighted data belongs to.<br/>|
|blurScope|||The range of fade out when focus is enabled.<br/>`EmphasisStyle.BlurScope`:<br/>- `GridCoord`: coordinate system.<br/>- `Series`: series.<br/>- `Global`: global.<br/>|

```mdx-code-block
</APITable>
```

## EndLabelStyle

> class in XCharts.Runtime / Inherits from: [LabelStyle](#labelstyle)

## GridCoord

> class in XCharts.Runtime / Inherits from: [CoordSystem](#coordsystem), [IUpdateRuntimeData](#iupdateruntimedata), [ISerieContainer](#iseriecontainer)

Grid component.

```mdx-code-block
<APITable name="GridCoord">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the grid in rectangular coordinate.
|layoutIndex|-1|v3.8.0|The index of the grid layout component to which the grid belongs. The default is -1, which means that it does not belong to any grid layout component. When this value is set, the left, right, top, and bottom properties will be invalid.
|left|0.1f||Distance between grid component and the left side of the container.
|right|0.08f||Distance between grid component and the right side of the container.
|top|0.22f||Distance between grid component and the top side of the container.
|bottom|0.12f||Distance between grid component and the bottom side of the container.
|backgroundColor|||Background color of grid, which is transparent by default.
|showBorder|false||Whether to show the grid border.
|borderWidth|0f||Border width of grid.
|borderColor|||The color of grid border.

```mdx-code-block
</APITable>
```

## GridLayout

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent), [IUpdateRuntimeData](#iupdateruntimedata)

> Since `v3.8.0`

Grid layout component. Used to manage the layout of multiple `GridCoord`, and the number of rows and columns of the grid can be controlled by `row` and `column`.

```mdx-code-block
<APITable name="GridLayout">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the grid in rectangular coordinate.
|left|0.1f||Distance between grid component and the left side of the container.
|right|0.08f||Distance between grid component and the right side of the container.
|top|0.22f||Distance between grid component and the top side of the container.
|bottom|0.12f||Distance between grid component and the bottom side of the container.
|row|2||the row count of grid layout.
|column|2||the column count of grid layout.
|spacing|Vector2.zero||the spacing of grid layout.
|inverse|false||Whether to inverse the grid layout.

```mdx-code-block
</APITable>
```

## Heatmap

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

```mdx-code-block
<APITable name="Heatmap">
```


|field|default|since|comment|
|--|--|--|--|
|heatmapType||v3.3.0|The mapping type of heatmap.<br/>`HeatmapType`:<br/>- `Data`: Data mapping type.By default, the second dimension data is used as the color map.<br/>- `Count`: Number mapping type.The number of occurrences of a statistic in a divided grid, as a color map.<br/>|

```mdx-code-block
</APITable>
```

## IconStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="IconStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|false||Whether the data icon is show.
|layer|||显示在上层还是在下层。<br/>`IconStyle.Layer`:<br/>- `UnderText`: The icon is display under the label text. 图标在标签文字下<br/>- `AboveText`: The icon is display above the label text. 图标在标签文字上<br/>|
|align|||水平方向对齐方式。<br/>`Align`:<br/>- `Center`: Alignment mode.<br/>- `Left`: Alignment mode.<br/>- `Right`: Alignment mode.<br/>|
|sprite|||The image of icon.
|type|||How to display the icon.
|color|||图标颜色。
|width|20||图标宽。
|height|20||图标高。
|offset|||图标偏移。
|autoHideWhenLabelEmpty|false||当label内容为空时是否自动隐藏图标

```mdx-code-block
</APITable>
```

## ImageStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

```mdx-code-block
<APITable name="ImageStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the data icon is show.
|sprite|||The image of icon.
|type|||How to display the image.
|autoColor|||是否自动颜色。
|color|||图标颜色。
|width|0||图标宽。
|height|0||图标高。

```mdx-code-block
</APITable>
```

## Indicator

> class in XCharts.Runtime

Indicator of radar chart, which is used to assign multiple variables(dimensions) in radar chart.

```mdx-code-block
<APITable name="Indicator">
```


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
|axisLine|||axis line. [AxisLine](#axisline)|
|axisName|||Name options for radar indicators. [AxisName](#axisname)|
|splitLine|||split line. [AxisSplitLine](#axissplitline)|
|splitArea|||Split area of axis in grid area. [AxisSplitArea](#axissplitarea)|
|indicator|true||Whether to show indicator.
|positionType|||The position type of indicator.
|indicatorGap|10||The gap of indicator and radar.
|ceilRate|0||The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.
|isAxisTooltip|||是否Tooltip显示轴线上的所有数据。
|outRangeColor|Color.red||The color displayed when data out of range.
|connectCenter|false||Whether serie data connect to radar center with line.
|lineGradient|true||Whether need gradient for data line.
|startAngle||v3.4.0|起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。
|gridIndex|-1|v3.8.0|Index of layout component that serie uses. Default is -1 means not use layout, otherwise use the first layout component.
|indicatorList|||the indicator list.

```mdx-code-block
</APITable>
```

## INeedSerieContainer

> class in XCharts.Runtime / Subclasses: [Bar](#bar), [SimplifiedBar](#simplifiedbar), [Candlestick](#candlestick), [SimplifiedCandlestick](#simplifiedcandlestick), [Heatmap](#heatmap), [Line](#line), [SimplifiedLine](#simplifiedline), [Parallel](#parallel), [Radar](#radar), [BaseScatter](#basescatter)

## IPropertyChanged

> class in XCharts.Runtime / Subclasses: [Location](#location), [Comment](#comment), [Legend](#legend), [Title](#title)

属性变更接口

## ISerieComponent

> class in XCharts.Runtime / Subclasses: [AreaStyle](#areastyle), [ImageStyle](#imagestyle), [LineArrow](#linearrow), [LabelLine](#labelline), [LabelStyle](#labelstyle), [BlurStyle](#blurstyle), [EmphasisStyle](#emphasisstyle), [SelectStyle](#selectstyle), [TitleStyle](#titlestyle)

The interface for serie component.

## ISerieContainer

> class in XCharts.Runtime / Subclasses: [RadarCoord](#radarcoord), [CalendarCoord](#calendarcoord), [GridCoord](#gridcoord), [ParallelCoord](#parallelcoord), [PolarCoord](#polarcoord)

## ISerieDataComponent

> class in XCharts.Runtime / Subclasses: [AreaStyle](#areastyle), [ImageStyle](#imagestyle), [ItemStyle](#itemstyle), [LineStyle](#linestyle), [SerieSymbol](#seriesymbol), [LabelLine](#labelline), [LabelStyle](#labelstyle), [BlurStyle](#blurstyle), [EmphasisStyle](#emphasisstyle), [SelectStyle](#selectstyle), [TitleStyle](#titlestyle)

The interface for serie data component.

## ISimplifiedSerie

> class in XCharts.Runtime / Subclasses: [SimplifiedBar](#simplifiedbar), [SimplifiedCandlestick](#simplifiedcandlestick), [SimplifiedLine](#simplifiedline)

## ItemStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieDataComponent](#iseriedatacomponent)

图形样式。

```mdx-code-block
<APITable name="ItemStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||是否启用。
|color|||数据项颜色。
|color0|||数据项颜色。
|toColor|||Gradient color1.
|toColor2|||Gradient color2.Only valid in line diagrams.
|markColor||v3.6.0|Serie's mark color. It is only used to display Legend and Tooltip, and does not affect the drawing color. The default value is clear.
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

```mdx-code-block
</APITable>
```

## IUpdateRuntimeData

> class in XCharts.Runtime / Subclasses: [SingleAxis](#singleaxis), [DataZoom](#datazoom), [CalendarCoord](#calendarcoord), [GridCoord](#gridcoord), [GridLayout](#gridlayout), [ParallelCoord](#parallelcoord)

## LabelLine

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

标签的引导线

```mdx-code-block
<APITable name="LabelLine">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the label line is showed.
|lineType|||the type of visual guide line.<br/>`LabelLine.LineType`:<br/>- `BrokenLine`: 折线<br/>- `Curves`: 曲线<br/>- `HorizontalLine`: 水平线<br/>|
|lineColor|Color32(0,0,0,0)||the color of visual guild line.
|lineAngle|60||the angle of visual guild line. Valid for broken line and curve line. Invalid in Pie.
|lineWidth|1.0f||the width of visual guild line.
|lineGap|1.0f||the gap of container and guild line.
|lineLength1|25f||The length of the first segment of visual guide line.
|lineLength2|15f||The length of the second segment of visual guide line.
|lineEndX|0f|v3.8.0|The fixed x position of the end point of visual guide line.
|startSymbol|||The symbol of the start point of labelline. [SymbolStyle](#symbolstyle)|
|endSymbol|||The symbol of the end point of labelline. [SymbolStyle](#symbolstyle)|

```mdx-code-block
</APITable>
```

## LabelStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent) / Subclasses: [AxisLabel](#axislabel), [EndLabelStyle](#endlabelstyle), [TitleStyle](#titlestyle)

Text label of chart, to explain some data information about graphic item like value, name and so on.

```mdx-code-block
<APITable name="LabelStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the label is showed.
|Position|||The position of label.
|autoOffset|false||Whether to automatically offset. When turned on, the Y offset will automatically determine the opening of the curve to determine whether to offset up or down.
|offset|||offset to the host graphic element.
|rotate|||Rotation of label.
|autoRotate|false|v3.6.0|auto rotate of label.
|distance|||the distance of label to axis line.
|formatter|||label content string template formatter. \n line wrapping is supported. Formatters for some components will not take effect. <br /> Template placeholder have the following, some of which apply only to fixed components: <br /> `{.}` : indicates the dot mark. <br /> `{a}` : indicates the series name. <br /> `{b}` : category value or data name. <br /> `{c}` : data value. <br /> `{d}` : percentage. <br /> `{e}` : indicates the data name. <br /> `{f}` : data sum. <br /> `{g}` : indicates the total number of data. <br /> `{h}` : hexadecimal color value. <br /> `{value}` : The value of the axis or legend. <br /> The following placeholder apply to `UITable` components: <br /> `{name}` : indicates the row name of the table. <br /> `{index}` : indicates the row number of the table. <br /> The following placeholder apply to `UIStatistc` components: <br /> `{title}` : title text. <br /> `{dd}` : day. <br /> `{hh}` : hours. <br /> `{mm}` : minutes. <br /> `{ss}` : second. <br /> `{fff}` : milliseconds. <br /> `{d}` : day. <br /> `{h}` : hours. <br /> `{m}` : minutes. <br /> `{s}` : second. <br /> `{f}` : milliseconds. <br /> Example :{b}:{c}<br />
|numericFormatter|||Standard number and date format string. Used to format a Double value or a DateTime date as a string. numericFormatter is used as an argument to either `Double.ToString ()` or `DateTime.ToString()`. <br /> The number format uses the Axx format: A is a single-character format specifier that supports C currency, D decimal, E exponent, F fixed-point number, G regular, N digit, P percentage, R round trip, and X hexadecimal. xx is precision specification, from 0-99. E.g. F1, E2<br /> Date format Common date formats are: yyyy year, MM month, dd day, HH hour, mm minute, ss second, fff millisecond. For example: yyyy-MM-dd HH:mm:ss<br /> number format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/> date format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/>
|width|0||the width of label. If set as default value 0, it means than the label width auto set as the text width.
|height|0||the height of label. If set as default value 0, it means than the label height auto set as the text height.
|icon|||the sytle of icon. [IconStyle](#iconstyle)|
|background|||the sytle of background. [ImageStyle](#imagestyle)|
|textPadding|||the text padding of label. [TextPadding](#textpadding)|
|textStyle|||the sytle of text. [TextStyle](#textstyle)|

```mdx-code-block
</APITable>
```

## Lang

> class in XCharts.Runtime / Inherits from: [ScriptableObject](https://docs.unity3d.com/ScriptReference/30_search.html?q=ScriptableObject)

Language.

## LangCandlestick

> class in XCharts.Runtime

## LangTime

> class in XCharts.Runtime

## Legend

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent), [IPropertyChanged](#ipropertychanged)

Legend component.The legend component shows different sets of tags, colors, and names. You can control which series are not displayed by clicking on the legend.

```mdx-code-block
<APITable name="Legend">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show legend component.
|iconType|||Type of legend.<br/>`Legend.Type`:<br/>- `Auto`: 自动匹配。<br/>- `Custom`: 自定义图标。<br/>- `EmptyCircle`: 空心圆。<br/>- `Circle`: 圆形。<br/>- `Rect`: 正方形。可通过Setting的legendIconCornerRadius参数调整圆角。<br/>- `Triangle`: 三角形。<br/>- `Diamond`: 菱形。<br/>- `Candlestick`: 烛台（可用于K线图）。<br/>|
|selectedMode|||Selected mode of legend, which controls whether series can be toggled displaying by clicking legends.<br/>`Legend.SelectedMode`:<br/>- `Multiple`: 多选。<br/>- `Single`: 单选。<br/>- `None`: 无法选择。<br/>|
|orient|||Specify whether the layout of legend component is horizontal or vertical.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|location|||The location of legend. [Location](#location)|
|itemWidth|25.0f||Image width of legend symbol.
|itemHeight|12.0f||Image height of legend symbol.
|itemGap|10f||The distance between each legend, horizontal distance in horizontal layout, and vertical distance in vertical layout.
|itemAutoColor|true||Whether the legend symbol matches the color automatically.
|itemOpacity|1||the opacity of item color.
|formatter|||No longer used, the use of LabelStyle.formatter instead.
|labelStyle|||the style of text. [LabelStyle](#labelstyle)|
|data|||Data array of legend. An array item is usually a name representing string. (If it is a pie chart, it could also be the name of a single data in the pie chart) of a series. If data is not specified, it will be auto collected from series.
|icons|||自定义的图例标记图形。
|colors|||the colors of legend item.
|background||v3.1.0|the sytle of background. [ImageStyle](#imagestyle)|
|padding||v3.1.0|the paddinng of item and background. [Padding](#padding)|
|positions||v3.6.0|the custom positions of legend item.

```mdx-code-block
</APITable>
```

## LegendTheme

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="LegendTheme">
```


|field|default|since|comment|
|--|--|--|--|
|unableColor|||the color of text.

```mdx-code-block
</APITable>
```

## Level

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="Level">
```


|field|default|since|comment|
|--|--|--|--|
|label|||文本标签样式。 [LabelStyle](#labelstyle)|
|upperLabel|||上方的文本标签样式。 [LabelStyle](#labelstyle)|
|itemStyle|||数据项样式。 [ItemStyle](#itemstyle)|

```mdx-code-block
</APITable>
```

## LevelStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="LevelStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|false||是否启用LevelStyle
|levels|||各层节点对应的配置。当enableLevels为true时生效，levels[0]对应的第一层的配置，levels[1]对应第二层，依次类推。当levels中没有对应层时用默认的设置。

```mdx-code-block
</APITable>
```

## Line

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## LineArrow

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieComponent](#iseriecomponent)

```mdx-code-block
<APITable name="LineArrow">
```


|field|default|since|comment|
|--|--|--|--|
|show|||Whether to show the arrow.
|position|||The position of arrow.<br/>`LineArrow.Position`:<br/>- `End`: 末端箭头<br/>- `Start`: 头端箭头<br/>|
|arrow|||the arrow of line. [ArrowStyle](#arrowstyle)|

```mdx-code-block
</APITable>
```

## LineStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [ISerieDataComponent](#iseriedatacomponent)

The style of line.

```mdx-code-block
<APITable name="LineStyle">
```


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
|dashLength|4|v3.8.1|the length of dash line. default value is 0, which means the length of dash line is 12 times of line width. Represents a multiple of the number of segments in a line chart.
|dotLength|2|v3.8.1|the length of dot line. default value is 0, which means the length of dot line is 2 times of line width. Represents a multiple of the number of segments in a line chart.
|gapLength|2|v3.8.1|the length of gap line. default value is 0, which means the length of gap line is 3 times of line width. Represents a multiple of the number of segments in a line chart.

```mdx-code-block
</APITable>
```

## Location

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent), [IPropertyChanged](#ipropertychanged)

Location type. Quick to set the general location.

```mdx-code-block
<APITable name="Location">
```


|field|default|since|comment|
|--|--|--|--|
|align|||对齐方式。<br/>`Location.Align`:<br/>- `TopLeft`: 对齐方式<br/>- `TopRight`: 对齐方式<br/>- `TopCenter`: 对齐方式<br/>- `BottomLeft`: 对齐方式<br/>- `BottomRight`: 对齐方式<br/>- `BottomCenter`: 对齐方式<br/>- `Center`: 对齐方式<br/>- `CenterLeft`: 对齐方式<br/>- `CenterRight`: 对齐方式<br/>|
|left|||Distance between component and the left side of the container.
|right|||Distance between component and the left side of the container.
|top|||Distance between component and the left side of the container.
|bottom|||Distance between component and the left side of the container.

```mdx-code-block
</APITable>
```

## MainComponent

> class in XCharts.Runtime / Inherits from: [IComparable](https://docs.unity3d.com/ScriptReference/30_search.html?q=IComparable) / Subclasses: [Axis](#axis), [Background](#background), [Comment](#comment), [DataZoom](#datazoom), [Legend](#legend), [MarkArea](#markarea), [MarkLine](#markline), [Settings](#settings), [Title](#title), [Tooltip](#tooltip), [VisualMap](#visualmap), [GridLayout](#gridlayout), [CoordSystem](#coordsystem)

## MarkArea

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent)

Used to mark an area in chart. For example, mark a time interval.

```mdx-code-block
<APITable name="MarkArea">
```


|field|default|since|comment|
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

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

标域的数据。

```mdx-code-block
<APITable name="MarkAreaData">
```


|field|default|since|comment|
|--|--|--|--|
|type|||Special markArea types, are used to label maximum value, minimum value and so on.<br/>`MarkAreaType`:<br/>- `None`: 标域类型<br/>- `Min`: 最小值。<br/>- `Max`: 最大值。<br/>- `Average`: 平均值。<br/>- `Median`: 中位数。<br/>|
|name|||Name of the marker, which will display as a label.
|dimension|1||From which dimension of data to calculate the maximum and minimum value and so on.
|xPosition|||The x coordinate relative to the origin, in pixels.
|yPosition|||The y coordinate relative to the origin, in pixels.
|xValue|||The value specified on the X-axis. A value specified when the X-axis is the category axis represents the index of the category axis data, otherwise a specific value.
|yValue|||That's the value on the Y-axis. The value specified when the Y axis is the category axis represents the index of the category axis data, otherwise the specific value.

```mdx-code-block
</APITable>
```

## MarkLine

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent)

Use a line in the chart to illustrate.

```mdx-code-block
<APITable name="MarkLine">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to display the marking line.
|serieIndex|0||The serie index of markLine.
|onTop|true|v3.9.0|whether the markline is on top.
|animation|||The animation of markline. [AnimationStyle](#animationstyle)|
|data|||A list of marked data. When the group of data item is 0, each data item represents a line; When the group is not 0, two data items of the same group represent the starting point and the ending point of the line respectively to form a line. In this case, the relevant style parameters of the line are the parameters of the starting point.

```mdx-code-block
</APITable>
```

## MarkLineData

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

> Since `v3.9.0`

Data of marking line.

```mdx-code-block
<APITable name="MarkLineData">
```


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
|startSymbol|||The symbol of the start point of markline. [SymbolStyle](#symbolstyle)|
|endSymbol|||The symbol of the end point of markline. [SymbolStyle](#symbolstyle)|
|lineStyle|||The line style of markline. [LineStyle](#linestyle)|
|label|||Text styles of label. You can set position to Start, Middle, and End to display text in different locations. [LabelStyle](#labelstyle)|

```mdx-code-block
</APITable>
```

## MarqueeStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

> Since `v3.5.0`

Marquee style. It can be used for the DataZoom component. 选取框样式。可用于DataZoom组件。

```mdx-code-block
<APITable name="MarqueeStyle">
```


|field|default|since|comment|
|--|--|--|--|
|apply|false|v3.5.0|Check whether the scope is applied to the DataZoom. If this parameter is set to true, the range after the selection is complete is the DataZoom selection range.
|realRect|false|v3.5.0|Whether to select the actual box selection area. When true, the actual range between the mouse's actual point and the end point is used as the box selection area.
|areaStyle||v3.5.0|The area style of marquee. [AreaStyle](#areastyle)|
|lineStyle||v3.5.0|The line style of marquee border. [LineStyle](#linestyle)|

```mdx-code-block
</APITable>
```

## MLValue

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

> Since `v3.8.0`

多样式数值。

```mdx-code-block
<APITable name="MLValue">
```


|field|default|since|comment|
|--|--|--|--|
|type|||<br/>`MLValue.Type`:<br/>- `Percent`: Percent value form.<br/>- `Absolute`: Absolute value form.<br/>- `Extra`: Extra value form.<br/>|
|value|||

```mdx-code-block
</APITable>
```

## Padding

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent) / Subclasses: [TextPadding](#textpadding)

padding setting of item or text.

```mdx-code-block
<APITable name="Padding">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||show padding. 是否显示。
|top|0||padding of top.
|right|2f||padding of right.
|left|2f||padding of left.
|bottom|0||padding of bottom.

```mdx-code-block
</APITable>
```

## Parallel

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

## ParallelAxis

> class in XCharts.Runtime / Inherits from: [Axis](#axis)

## ParallelCoord

> class in XCharts.Runtime / Inherits from: [CoordSystem](#coordsystem), [IUpdateRuntimeData](#iupdateruntimedata), [ISerieContainer](#iseriecontainer)

Grid component.

```mdx-code-block
<APITable name="ParallelCoord">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the grid in rectangular coordinate.
|orient|||Orientation of the axis. By default, it's 'Vertical'. You can set it to be 'Horizonal' to make a vertical axis.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|left|0.1f||Distance between grid component and the left side of the container.
|right|0.08f||Distance between grid component and the right side of the container.
|top|0.22f||Distance between grid component and the top side of the container.
|bottom|0.12f||Distance between grid component and the bottom side of the container.
|backgroundColor|||Background color of grid, which is transparent by default.

```mdx-code-block
</APITable>
```

## Pie

> class in XCharts.Runtime / Inherits from: [Serie](#serie)

```mdx-code-block
<APITable name="Pie">
```


|field|default|since|comment|
|--|--|--|--|
|radiusGradient|false|v3.8.1|Whether to use gradient color in pie chart.

```mdx-code-block
</APITable>
```

## PolarAxisTheme

> class in XCharts.Runtime / Inherits from: [BaseAxisTheme](#baseaxistheme)

## PolarCoord

> class in XCharts.Runtime / Inherits from: [CoordSystem](#coordsystem), [ISerieContainer](#iseriecontainer)

Polar coordinate can be used in scatter and line chart. Every polar coordinate has an angleAxis and a radiusAxis.

```mdx-code-block
<APITable name="PolarCoord">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the polor component.
|center|||The center of ploar. The center[0] is the x-coordinate, and the center[1] is the y-coordinate. When value between 0 and 1 represents a percentage  relative to the chart.
|radius|||the radius of polar.
|backgroundColor|||Background color of polar, which is transparent by default.
|indicatorLabelOffset|30f|v3.8.0|The offset of indicator label.

```mdx-code-block
</APITable>
```

## Radar

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer)

```mdx-code-block
<APITable name="Radar">
```


|field|default|since|comment|
|--|--|--|--|
|smooth|false|v3.2.0|Whether use smooth curve.

```mdx-code-block
</APITable>
```

## RadarAxisTheme

> class in XCharts.Runtime / Inherits from: [BaseAxisTheme](#baseaxistheme)

## RadarCoord

> class in XCharts.Runtime / Inherits from: [CoordSystem](#coordsystem), [ISerieContainer](#iseriecontainer)

Radar coordinate conponnet for radar charts. 雷达图坐标系组件，只适用于雷达图。

## RadiusAxis

> class in XCharts.Runtime / Inherits from: [Axis](#axis)

Radial axis of polar coordinate.

## RadiusAxisTheme

> class in XCharts.Runtime / Inherits from: [BaseAxisTheme](#baseaxistheme)

## Ring

> class in XCharts.Runtime / Inherits from: [Serie](#serie)

## Scatter

> class in XCharts.Runtime / Inherits from: [BaseScatter](#basescatter)

## SelectStyle

> class in XCharts.Runtime / Inherits from: [StateStyle](#statestyle), [ISerieComponent](#iseriecomponent), [ISerieDataComponent](#iseriedatacomponent)

> Since `v3.2.0`

Configurations of select state.

## Serie

> class in XCharts.Runtime / Inherits from: [BaseSerie](#baseserie), [IComparable](https://docs.unity3d.com/ScriptReference/30_search.html?q=IComparable) / Subclasses: [SerieHandler&lt;T&gt;](#seriehandlert), [Bar](#bar), [SimplifiedBar](#simplifiedbar), [Candlestick](#candlestick), [SimplifiedCandlestick](#simplifiedcandlestick), [Heatmap](#heatmap), [Line](#line), [SimplifiedLine](#simplifiedline), [Parallel](#parallel), [Pie](#pie), [Radar](#radar), [Ring](#ring), [BaseScatter](#basescatter)

系列。系列一般由数据和配置组成，用来表示具体的图表图形，如折线图的一条折线，柱图的一组柱子等。一个图表中可以包含多个不同类型的系列。

```mdx-code-block
<APITable name="Serie">
```


|field|default|since|comment|
|--|--|--|--|
|index|||The index of serie.
|show|true||Whether to show serie in chart.
|coordSystem|||the chart coord system of serie.
|serieType|||the type of serie.
|serieName|||Series name used for displaying in tooltip and filtering with legend.
|state||v3.2.0|The default state of a serie.<br/>`SerieState`:<br/>- `Normal`: Normal state.<br/>- `Emphasis`: Emphasis state.<br/>- `Blur`: Blur state.<br/>- `Select`: Select state.<br/>- `Auto`: Auto state.<br/>|
|colorBy||v3.2.0|The policy to take color from theme.<br/>`SerieColorBy`:<br/>- `Default`: Select state.<br/>- `Serie`: assigns the colors in the palette by serie, so that all data in the same series are in the same color.<br/>- `Data`: assigns colors in the palette according to data items, with each data item using a different color.<br/>|
|stack|||If stack the value. On the same category axis, the series with the same stack name would be put on top of each other.
|xAxisIndex|0||the index of XAxis.
|yAxisIndex|0||the index of YAxis.
|radarIndex|0||Index of radar component that radar chart uses.
|vesselIndex|0||Index of vesel component that liquid chart uses.
|polarIndex|0||Index of polar component that serie uses.
|singleAxisIndex|0||Index of single axis component that serie uses.
|parallelIndex|0||Index of parallel coord component that serie uses.
|gridIndex|-1|v3.8.0|Index of layout component that serie uses. Default is -1 means not use layout, otherwise use the first layout component.
|minShow|||The min number of data to show in chart.
|maxShow|||The max number of data to show in chart.
|maxCache|||The max number of serie data cache. The first data will be remove when the size of serie data is larger then maxCache.
|sampleDist|0||the min pixel dist of sample.
|sampleType|||the type of sample.<br/>`SampleType`:<br/>- `Peak`: Take a peak. When the average value of the filter point is greater than or equal to 'sampleAverage', take the maximum value; If you do it the other way around, you get the minimum.<br/>- `Average`: Take the average of the filter points.<br/>- `Max`: Take the maximum value of the filter point.<br/>- `Min`: Take the minimum value of the filter point.<br/>- `Sum`: Take the sum of the filter points.<br/>|
|sampleAverage|0||设定的采样平均值。当sampleType 为 Peak 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为0时会实时计算所有数据的平均值。
|lineType|||The type of line chart.<br/>`LineType`:<br/>- `Normal`: the normal line chart，<br/>- `Smooth`: the smooth line chart，<br/>- `StepStart`: step line.<br/>- `StepMiddle`: step line.<br/>- `StepEnd`: step line.<br/>|
|smoothLimit|false|v3.4.0|Whether to restrict the curve. When true, the curve between two continuous data of the same value is restricted to not exceed the data point, and is flat to the data point.
|barType|||柱形图类型。<br/>`BarType`:<br/>- `Normal`: normal bar.<br/>- `Zebra`: zebra bar.<br/>- `Capsule`: capsule bar.<br/>|
|barPercentStack|false||柱形图是否为百分比堆积。相同stack的serie只要有一个barPercentStack为true，则就显示成百分比堆叠柱状图。
|barWidth|0||The width of the bar. Adaptive when default 0.
|barMaxWidth|0|v3.5.0|The max width of the bar. Adaptive when default 0.
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
|minRadius|0f|v3.8.0|the min radius of chart. It can be used to limit the minimum radius of the rose chart.
|showDataDimension|||数据项里的数据维数。
|showDataName|||在Editor的inpsector上是否显示name参数
|clip|false||If clip the overflow on the coordinate system.
|ignore|false||是否开启忽略数据。当为 true 时，数据值为 ignoreValue 时不进行绘制。
|ignoreValue|0||忽略数据的默认值。当ignore为true才有效。
|ignoreLineBreak|false||忽略数据时折线是断开还是连接。默认false为连接。
|showAsPositiveNumber|false||Show negative number as positive number.
|large|true||是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。 开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。 缺点：优化后不能自定义设置单个数据项的样式，不能显示Label。
|largeThreshold|200||Turn on the threshold for mass optimization. Enter performance mode only when large is enabled and the amount of data is greater than the threshold.
|avoidLabelOverlap|false||If the pie chart and labels are displayed externally, whether to enable the label overlap prevention policy is disabled by default. If labels are crowded and overlapped, the positions of labels are moved to prevent label overlap.
|radarType|||雷达图类型。<br/>`RadarType`:<br/>- `Multiple`: multiple radar.<br/>- `Single`: single radar.<br/>|
|placeHolder|false||占位模式。占位模式时，数据有效但不参与渲染和显示。
|dataSortType|||组件的数据排序。<br/>`SerieDataSortType`:<br/>- `None`: In the order of data.<br/>- `Ascending`: Sort data in ascending order.<br/>- `Descending`: Sort data in descending order.<br/>|
|orient|||组件的朝向。<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|align|||组件水平方向对齐方式。<br/>`Align`:<br/>- `Center`: Alignment mode.<br/>- `Left`: Alignment mode.<br/>- `Right`: Alignment mode.<br/>|
|left|||Distance between component and the left side of the container.
|right|||Distance between component and the right side of the container.
|top|||Distance between component and the top side of the container.
|bottom|||Distance between component and the bottom side of the container.
|insertDataToHead|||Whether to add new data at the head or at the end of the list.
|lineStyle|||The style of line. [LineStyle](#linestyle)|
|symbol|||the symbol of serie data item. [SerieSymbol](#seriesymbol)|
|animation|||The start animation. [AnimationStyle](#animationstyle)|
|itemStyle|||The style of data item. [ItemStyle](#itemstyle)|
|data|||系列中的数据内容数组。SerieData可以设置1到n维数据。

```mdx-code-block
</APITable>
```

## SerieData

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

A data item of serie.

```mdx-code-block
<APITable name="SerieData">
```


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

```mdx-code-block
</APITable>
```

## SerieSymbol

> class in XCharts.Runtime / Inherits from: [SymbolStyle](#symbolstyle), [ISerieDataComponent](#iseriedatacomponent)

系列数据项的标记的图形

```mdx-code-block
<APITable name="SerieSymbol">
```


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

```mdx-code-block
</APITable>
```

## SerieTheme

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="SerieTheme">
```


|field|default|since|comment|
|--|--|--|--|
|lineWidth|||the color of text.
|lineSymbolSize|||the symbol size of line serie.
|scatterSymbolSize|||the symbol size of scatter serie.
|candlestickColor|Color32(235, 84, 84, 255)||K线图阳线（涨）填充色
|candlestickColor0|Color32(71, 178, 98, 255)||K线图阴线（跌）填充色
|candlestickBorderWidth|1||K线图边框宽度
|candlestickBorderColor|Color32(235, 84, 84, 255)||K线图阳线（跌）边框色
|candlestickBorderColor0|Color32(71, 178, 98, 255)||K线图阴线（跌）边框色

```mdx-code-block
</APITable>
```

## Settings

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent)

Global parameter setting component. The default value can be used in general, and can be adjusted when necessary.

```mdx-code-block
<APITable name="Settings">
```


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

```mdx-code-block
</APITable>
```

## SimplifiedBar

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer), [ISimplifiedSerie](#isimplifiedserie)

## SimplifiedCandlestick

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer), [ISimplifiedSerie](#isimplifiedserie)

## SimplifiedLine

> class in XCharts.Runtime / Inherits from: [Serie](#serie), [INeedSerieContainer](#ineedseriecontainer), [ISimplifiedSerie](#isimplifiedserie)

## SingleAxis

> class in XCharts.Runtime / Inherits from: [Axis](#axis), [IUpdateRuntimeData](#iupdateruntimedata)

Single axis.

```mdx-code-block
<APITable name="SingleAxis">
```


|field|default|since|comment|
|--|--|--|--|
|orient|||Orientation of the axis. By default, it's 'Horizontal'. You can set it to be 'Vertical' to make a vertical axis.<br/>`Orient`:<br/>- `Horizonal`: 水平<br/>- `Vertical`: 垂直<br/>|
|left|0.1f||Distance between component and the left side of the container.
|right|0.1f||Distance between component and the right side of the container.
|top|0f||Distance between component and the top side of the container.
|bottom|0.2f||Distance between component and the bottom side of the container.
|width|0||width of axis.
|height|50||height of axis.

```mdx-code-block
</APITable>
```

## SingleAxisCoord

> class in XCharts.Runtime / Inherits from: [CoordSystem](#coordsystem)

## StageColor

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="StageColor">
```


|field|default|since|comment|
|--|--|--|--|
|percent|||结束位置百分比。
|color|||颜色。

```mdx-code-block
</APITable>
```

## StateStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent) / Subclasses: [BlurStyle](#blurstyle), [EmphasisStyle](#emphasisstyle), [SelectStyle](#selectstyle)

> Since `v3.2.0`

the state style of serie.

```mdx-code-block
<APITable name="StateStyle">
```


|field|default|since|comment|
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

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme)

## SymbolStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent) / Subclasses: [SerieSymbol](#seriesymbol)

系列数据项的标记的图形

```mdx-code-block
<APITable name="SymbolStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether the symbol is showed.
|type|||the type of symbol.<br/>`SymbolType`:<br/>- `None`: 不显示标记。<br/>- `Custom`: 自定义标记。<br/>- `Circle`: 圆形。<br/>- `EmptyCircle`: 空心圆。<br/>- `Rect`: 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。<br/>- `EmptyRect`: 空心正方形。<br/>- `Triangle`: 三角形。<br/>- `EmptyTriangle`: 空心三角形。<br/>- `Diamond`: 菱形。<br/>- `EmptyDiamond`: 空心菱形。<br/>- `Arrow`: 箭头。<br/>- `EmptyArrow`: 空心箭头。<br/>- `Plus`: 加号。<br/>- `Minus`: 减号。<br/>|
|size|0f||the size of symbol.
|gap|0||the gap of symbol and line segment.
|width|0f||图形的宽。
|height|0f||图形的高。
|offset|Vector2.zero||图形的偏移。
|image|||自定义的标记图形。
|imageType|||the fill type of image.
|color|||图形的颜色。

```mdx-code-block
</APITable>
```

## TextLimit

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

Text character limitation and adaptation component. When the length of the text exceeds the set length, it is cropped and suffixes are appended to the end.Only valid in the category axis.

```mdx-code-block
<APITable name="TextLimit">
```


|field|default|since|comment|
|--|--|--|--|
|enable|false||Whether to enable text limit.
|maxWidth|0||Set the maximum width. A default of 0 indicates automatic fetch; otherwise, custom.
|gap|1||White pixel distance at both ends.
|suffix|||Suffixes when the length exceeds.

```mdx-code-block
</APITable>
```

## TextPadding

> class in XCharts.Runtime / Inherits from: [Padding](#padding)

Settings related to text.

## TextStyle

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

Settings related to text.

```mdx-code-block
<APITable name="TextStyle">
```


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

```mdx-code-block
</APITable>
```

## Theme

> class in XCharts.Runtime / Inherits from: [ScriptableObject](https://docs.unity3d.com/ScriptReference/30_search.html?q=ScriptableObject)

Theme.

```mdx-code-block
<APITable name="Theme">
```


|field|default|since|comment|
|--|--|--|--|
|themeType|||the theme of chart.<br/>`ThemeType`:<br/>- `Default`: 默认主题。<br/>- `Light`: 亮主题。<br/>- `Dark`: 暗主题。<br/>- `Custom`: 自定义主题。<br/>|
|themeName|||the name of theme.
|font|||the font of chart text。
|tMPFont|||the font of chart text。
|contrastColor|||the contrast color of chart.
|backgroundColor|||the background color of chart.
|colorPalette|||The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
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

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

Theme.

```mdx-code-block
<APITable name="ThemeStyle">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||
|sharedTheme|||the asset of theme. [Theme](#theme)|
|transparentBackground|false||Whether the background color is transparent. When true, the background color is not drawn.
|enableCustomTheme|false||Whether to customize theme colors. When set to true, you can use 'sync color to custom' to synchronize the theme color to the custom color. It can also be set manually.
|customFont|||
|customBackgroundColor|||the custom background color of chart.
|customColorPalette|||

```mdx-code-block
</APITable>
```

## Title

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent), [IPropertyChanged](#ipropertychanged)

Title component, including main title and subtitle.

```mdx-code-block
<APITable name="Title">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||[default:true] Set this to false to prevent the title from showing.
|text|||The main title text, supporting \n for newlines.
|subText|||Subtitle text, supporting for \n for newlines.
|labelStyle|||The text style of main title. [LabelStyle](#labelstyle)|
|subLabelStyle|||The text style of sub title. [LabelStyle](#labelstyle)|
|itemGap|0||[default:8] The gap between the main title and subtitle.
|location|||The location of title component. [Location](#location)|

```mdx-code-block
</APITable>
```

## TitleStyle

> class in XCharts.Runtime / Inherits from: [LabelStyle](#labelstyle), [ISerieDataComponent](#iseriedatacomponent), [ISerieComponent](#iseriecomponent)

the title of serie.

## TitleTheme

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme)

## Tooltip

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent)

Tooltip component.

```mdx-code-block
<APITable name="Tooltip">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||Whether to show the tooltip component.
|type|||Indicator type.<br/>`Tooltip.Type`:<br/>- `Line`: line indicator.<br/>- `Shadow`: shadow crosshair indicator.<br/>- `None`: no indicator displayed.<br/>- `Corss`: crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.<br/>- `Auto`: Auto select indicator according to serie type.<br/>|
|trigger|||Type of triggering.<br/>`Tooltip.Trigger`:<br/>- `Item`: Triggered by data item, which is mainly used for charts that don't have a category axis like scatter charts or pie charts.<br/>- `Axis`: Triggered by axes, which is mainly used for charts that have category axes, like bar charts or line charts.<br/>- `None`: Trigger nothing.<br/>- `Auto`: Auto select trigger according to serie type.<br/>|
|position||v3.3.0|Type of position.<br/>`Tooltip.Position`:<br/>- `Auto`: Auto. The mobile platform is displayed at the top, and the non-mobile platform follows the mouse position.<br/>- `Custom`: Custom. Fully customize display position (x,y).<br/>- `FixedX`: Just fix the coordinate X. Y follows the mouse position.<br/>- `FixedY`: <br/>|
|itemFormatter|||a string template formatter for a single Serie or data item content. Support for wrapping lines with \n. Template variables are {.}, {a}, {b}, {c}, {d}.<br/> {.} is the dot of the corresponding color of a Serie that is currently indicated or whose index is 0.<br/> {a} is the series name of the serie that is currently indicated or whose index is 0.<br/> {b} is the name of the data item serieData that is currently indicated or whose index is 0, or a category value (such as the X-axis of a line chart).<br/> {c} is the value of a Y-dimension (dimesion is 1) from a Serie that is currently indicated or whose index is 0.<br/> {d} is the percentage value of Y-dimensions (dimesion is 1) from serie that is currently indicated or whose index is 0, with no % sign.<br/> {e} is the name of the data item serieData that is currently indicated or whose index is 0.<br/> {f} is sum of data.<br/> {.1} represents a dot from serie corresponding color that specifies index as 1.<br/> 1 in {a1}, {b1}, {c1} represents a serie that specifies an index of 1.<br/> {c1:2} represents the third data from serie's current indication data item indexed to 1 (a data item has multiple data, index 2 represents the third data).<br/> {c1:2-2} represents the third data item from serie's third data item indexed to 1 (i.e., which data item must be specified to specify).<br/> {d1:2: F2} indicates that a formatted string with a value specified separately is F2 (numericFormatter is used when numericFormatter is not specified).<br/> {d:0.##} indicates that a formatted string with a value specified separately is 0.##   (used for percentage, reserved 2 valid digits while avoiding the situation similar to "100.00%" when using f2 ).<br/> Example: "{a}, {c}", "{a1}, {c1: f1}", "{a1}, {c1:0: f1}", "{a1} : {c1:1-1: f1}"<br/>
|titleFormatter|||String template formatter for tooltip title content. \n line wrapping is supported. The placeholder {i} can be set separately to indicate that title is ignored and not displayed. Template variables are {.}, {a}, {b}, {c}, {d}, {e}, {f}, and {g}. <br /> {.} is the dot of the corresponding color of serie currently indicated or index 0. <br /> {a} is the series name name of serie currently indicated or index 0. <br /> {b} is the name of the serie data item serieData currently indicated or index 0, or the category value (such as the X-axis of a line chart). <br /> {c} is the value of the serie y-dimension (dimesion is 1) currently indicated or index is 0. <br /> {d} is the serie y-dimensional (dimesion 1) percentage value of the currently indicated or index 0, note without the % sign. <br /> {e} is the name of the serie data item serieData currently indicated or whose index is 0. <br /> {h} is the hexadecimal color value of serieData for the serie data item currently indicated or index 0. <br /> {f} is the sum of data. <br /> {g} indicates the total number of data. <br /> {.1} represents a dot of the corresponding color with serie specified as index 1. <br /> The 1 in {a1}, {b1}, {c1} represents serie where index is specified as 1. <br /> {c1:2} represents the third data of the current indicator data item in serie with index 1 (one data item has multiple data, index 2 represents the third data). <br /> {c1:2-2} represents the third data of serie third data item with index 1 (that is, the number of data items must be specified when specifying the number of data items). <br /> {d1:2:f2} indicates that a format string with a single value is f2 (numericFormatter is used if no value is specified). <br /> {d:0.##} indicates that the format string with a value specified alone is 0.## # (for percentages, preserving a 2-digit significant number while avoiding the "100.00%" situation with f2). <br /> example: "{a}, {c}", "{a1}, {c1: f1}", "{a1}, {c1:0: f1}", "{a1}, {c1:1-1: f1}"
|marker|||the marker of serie.
|fixedWidth|0||Fixed width. Higher priority than minWidth.
|fixedHeight|0||Fixed height. Higher priority than minHeight.
|minWidth|0||Minimum width. If fixedWidth has a value, get fixedWidth first.
|minHeight|0||Minimum height. If fixedHeight has a value, take priority over fixedHeight.
|numericFormatter|||Standard number and date format string. Used to format a Double value or a DateTime date as a string. numericFormatter is used as an argument to either `Double.ToString ()` or `DateTime.ToString()`. <br /> The number format uses the Axx format: A is a single-character format specifier that supports C currency, D decimal, E exponent, F fixed-point number, G regular, N digit, P percentage, R round trip, and X hexadecimal. xx is precision specification, from 0-99. E.g. F1, E2<br /> Date format Common date formats are: yyyy year, MM month, dd day, HH hour, mm minute, ss second, fff millisecond. For example: yyyy-MM-dd HH:mm:ss<br /> number format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/> date format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/>
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
|lineStyle|||the line style of indicator line. [LineStyle](#linestyle)|
|titleLabelStyle|||the textstyle of title. [LabelStyle](#labelstyle)|
|contentLabelStyles|||the textstyle list of content.

```mdx-code-block
</APITable>
```

## TooltipTheme

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="TooltipTheme">
```


|field|default|since|comment|
|--|--|--|--|
|lineType|||the type of line.<br/>`LineStyle.Type`:<br/>- `Solid`: 实线<br/>- `Dashed`: 虚线<br/>- `Dotted`: 点线<br/>- `DashDot`: 点划线<br/>- `DashDotDot`: 双点划线<br/>- `None`: 双点划线<br/>|
|lineWidth|1f||the width of line.
|lineColor|||the color of line.
|areaColor|||the color of line.
|labelTextColor|||the text color of tooltip cross indicator's axis label.
|labelBackgroundColor|||the background color of tooltip cross indicator's axis label.

```mdx-code-block
</APITable>
```

## UIComponentTheme

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="UIComponentTheme">
```


|field|default|since|comment|
|--|--|--|--|
|show|true||
|sharedTheme|||the asset of theme. [Theme](#theme)|
|transparentBackground|false||

```mdx-code-block
</APITable>
```

## VisualMap

> class in XCharts.Runtime / Inherits from: [MainComponent](#maincomponent)

VisualMap component. Mapping data to visual elements such as colors.

```mdx-code-block
<APITable name="VisualMap">
```


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
|location|||The location of component. [Location](#location)|
|workOnLine|true||Whether the visualmap is work on linestyle of linechart.
|workOnArea|false||Whether the visualmap is work on areaStyle of linechart.
|outOfRange|||Defines a visual color outside of the selected range.
|inRange|||分段式每一段的相关配置。

```mdx-code-block
</APITable>
```

## VisualMapRange

> class in XCharts.Runtime / Inherits from: [ChildComponent](#childcomponent)

```mdx-code-block
<APITable name="VisualMapRange">
```


|field|default|since|comment|
|--|--|--|--|
|min|||范围最小值
|max|||范围最大值
|label|||文字描述
|color|||颜色

```mdx-code-block
</APITable>
```

## VisualMapTheme

> class in XCharts.Runtime / Inherits from: [ComponentTheme](#componenttheme)

```mdx-code-block
<APITable name="VisualMapTheme">
```


|field|default|since|comment|
|--|--|--|--|
|borderWidth|||the width of border.
|borderColor|||the color of dataZoom border.
|backgroundColor|||the background color of visualmap.
|triangeLen|20f||可视化组件的调节三角形边长。

```mdx-code-block
</APITable>
```

## XAxis

> class in XCharts.Runtime / Inherits from: [Axis](#axis)

The x axis in cartesian(rectangular) coordinate.

## XCResourcesImporter

> class in XCharts.Runtime

## XCSettings

> class in XCharts.Runtime / Inherits from: [ScriptableObject](https://docs.unity3d.com/ScriptReference/30_search.html?q=ScriptableObject)

```mdx-code-block
<APITable name="XCSettings">
```


|field|default|since|comment|
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

> class in XCharts.Runtime / Inherits from: [Axis](#axis)

The x axis in cartesian(rectangular) coordinate.

