
# 更新日志

[master](#master)  
[v3.2.0](#v3.2.0)  
[v3.1.0](#v3.1.0)  
[v3.0.1](#v3.0.1)  
[v3.0.0](#v3.0.0)  
[v3.0.0-preivew9](#v3.0.0-preivew9)  
[v3.0.0-preivew8](#v3.0.0-preivew8)  
[v3.0.0-preivew7](#v3.0.0-preivew7)  
[v3.0.0-preivew6](#v3.0.0-preivew6)  
[v3.0.0-preivew5](#v3.0.0-preivew5)  
[v3.0.0-preivew4](#v3.0.0-preivew4)  
[v3.0.0-preivew3](#v3.0.0-preivew3)  
[v3.0.0-preivew2](#v3.0.0-preivew2)  
[v3.0.0-preivew1](#v3.0.0-preivew1)  
[v2.8.1](#v2.8.1)  
[v2.8.0](#v2.8.0)  
[v2.7.0](#v2.7.0)  
[v2.6.0](#v2.6.0)  
[v2.5.0](#v2.5.0)  
[v2.4.0](#v2.4.0)  
[v2.3.0](#v2.3.0)  
[v2.2.3](#v2.2.3)  
[v2.2.2](#v2.2.2)  
[v2.2.1](#v2.2.1)  
[v2.2.0](#v2.2.0)  
[v2.1.1](#v2.1.1)  
[v2.1.0](#v2.1.0)  
[v2.0.1](#v2.0.1)  
[v2.0.0](#v2.0.0)  
[v2.0.0-preview.2](#v2.0.0-preview.2)  
[v2.0.0-preview.1](#v2.0.0-preview.1)  
[v1.6.3](#v1.6.3)  
[v1.6.1](#v1.6.1)  
[v1.6.0](#v1.6.0)  
[v1.5.2](#v1.5.2)  
[v1.5.1](#v1.5.1)  
[v1.5.0](#v1.5.0)  
[v1.4.0](#v1.4.0)  
[v1.3.1](#v1.3.1)  
[v1.3.0](#v1.3.0)  
[v1.2.0](#v1.2.0)  
[v1.1.0](#v1.1.0)  
[v1.0.5](#v1.0.5)  
[v1.0.4](#v1.0.4)  
[v1.0.3](#v1.0.3)  
[v1.0.2](#v1.0.2)  
[v1.0.1](#v1.0.1)  
[v1.0.0](#v1.0.0)  
[v0.8.3](#v0.8.3)  
[v0.8.2](#v0.8.2)  
[v0.8.1](#v0.8.1)  
[v0.8.0](#v0.8.0)  
[v0.5.0](#v0.5.0)  
[v0.1.0](#v0.1.0)  

## master

## v3.2.0

### Main points

* `Serie` supports highlighting, EmphasisStyle, EmphasisStyle, BlurStyle, and SelectStyle
* `Axis` supports sub-scale and sub-partition of coordinate axes:`MinorTick` and `MinorSplitLine`
* `Serie` supports different color selection strategies: `colorBy`
* `Radar` supports smooth curves: `smooth`
* `Line` supports filling as a convex polygon: `AreaStyle` `innerFill`
* `DataZoom` supports timeline
* Other optimizations and issue fixes

### Log details

* (2022.08.16) Release `v3.2.0` version
* (2022.08.15) optimized `Smooth` Bezier curve algorithm
* (2022.08.13) Fixed an issue where the `DataZoom` component might not display correctly when opened
* (2022.08.11) Optimized Tooltip supports `ignoreDataDefaultContent`
* (2022.08.10) fixed abnormal display of some components of `Chart` under 3D camera
* (2022.08.10) Fix `RemoveSerie()` interface not working (#219)
* (2022.08.10) Optimized font synchronization for Theme
* (2022.08.10) optimizes the default `layer` of Chart to `UI`
* (2022.08.09) optimizes the `Time` timeline of `Axis`
* (2022.08.09) added AreaStyle `innerFill` parameter to support filling convex polygons
* (2022.08.08) Optimized the maintenance of data item indexes in `Serie`, added detection and repair functions, and fixed related problems
* (2022.07.29) Fixed `Unity` version compatibility: Chart creation exception after some versions import
* (2022.07.29) Add `Axis` to` Time `timeline, support sub-scale and sub-divider
* (2022.07.28) optimizes the `Radar` image
* (2022.07.28) increase `Serie` `colorBy` parameter configuration color taking strategy
* (2022.07.27) Adds StateStyle `Symbol` to configure the Symbol style in the state
* (2022.07.27) remove selectedSize from SerieSymbol
* (2022.07.24) adds default state Settings for `Serie` and `SerieData`
* (2022.07.22) add three states` EmphasisStyle `, `EmphasisStyle`, `SelectStyle` of `Serie`
* (2022.07.22) remove `highlightColor` and `highlightToColor` arguments from `AreaStyle`
* (2022.07.22) Omit the `Emphasis`,` EmphasisItemStyle `, `EmphasisLabelStyle`, `EmphasisLabelLine` component
* (2022.07.20) Added `Since` feature support for classes
* (2022.07.20) fixed the `showStartLabel` and `showEndLabel` parameter Settings for `AxisLabel` not taking effect when `Axis` is on the` Value `Axis
* (2022.07.19) added `Axis` to` MinorSplitLine `to set the Axis degree divider
* (2022.07.19) added `Axis` `MinorTick` to set the Axis sub-scale
* (2022.07.17) Add the `smooth` parameter for Radar to set the smooth curve
* (2022.07.15) added DataZoom support for the `Time` timeline

## v3.1.0

* (2022.07.12) Release `v3.1.0` version
* (2022.07.12) Fixed `Serie` `ignoreLineBreak` not working
* (2022.07.07) Optimized `Axis` `minMaxType` to support precision to decimals when specified as `MinMax`
* (2022.07.05) Fixed drawing exception when there are multiple coordinate systems in `Chart` (#210)
* (2022.07.04) Added the axisMaxSplitNumber parameter of `Settings` to set the maximum number of partitions for `Axis`
* (2022.07.04) Fixed Axis` Tick `drawing position after setting `offset`(#209)
* (2022.07.03) Optimize the `AxisLabel` formatterFunction custom delegate
* (2022.07.03) added the `onZero` parameter of `AxisName` to support setting the coordinate AxisName and position to match the Y-axis 0 scale (#207)
* (2022.07.02) Fixed bug where `Legend` was not working when `PieChart` was being created dynamically with code (#206)
* (2022.07.02) Fixed `YAxis` AxisLabel setting `onZero` not working
* (2022.07.02) Fixed `AxisLabel` code refreshing after setting `distance` property
* (2022.06.30) Fixed an issue where components could not be initialized when creating diagrams under `Runtime` code
* (2022.06.29) Added `itemFormatter` support for `{c0}` in `Tooltip` to display dimension data (#205)
* (2022.06.28) Optimize text performance when `Pie` sets up `avoidLabelOverlap` (#56)
* (2022.06.25) Optimize smooth curve representation of `Line` (#169)
* (2022.06.25) Fixed inconsistent display of `Tooltip` when `DataZoom` is enabled (#203)
* (2022.06.25) Fixed `Toolip` drawing exception when there is no data in the category axis (#204)
* (2022.06.25) Optimize `Serie` setting `PlaceHolder` for `Tooltip` performance
* (2022.06.25) Added `Since` to identify the version from which the configuration parameter is supported
* (2022.06.24) Optimize `Painter` drawing layer, `Top` layer is subdivided into `Upper` and `Top` layers
* (2022.06.24) Added `Legend` support for `Background` and `Padding`
* (2022.06.21) Added `TextStyle` support for `Sprite Asset` of `TextMeshPro` (#201)
* (2022.06.20) Optimize boundary limits for `Tooltip` (#202)
* (2022.06.20) Fixes compilation error when `TextMeshPro` is turned on
* (2022.06.20) Fixed issue where the fade Animation of `Animation` would not work

## v3.0.1

* (2022.06.16) Release `v3.0.1` version
* (2022.06.16) Fixed an issue where the `foldout` arrow on `Inspector` could not be expanded
* (2022.06.15) Optimized `Doc` auto-generation, improved code comments and configuration item manual documentation
* (2022.06.14) Optimized `SerieLabelStyle` to support dynamic adjustment of `Icon`
* (2022.06.13) Optimized `Background` setting
* (2022.06.10) Added `Legend` AxisLabel support for `autoColor`
* (2022.06.08) Fixed issue where `Axis` `AxisLabel` still shows the first and last two labels when not displayed

## v3.0.0

* More robust underlying framework.
* More powerful performance.
* Smaller serialized files.
* Better interactive experience.
* More component support.
* More powerful ability to self-report text.
* More reasonable component adjustments.
* More flexible component insertion and removal.
* More efficient secondary development.
* Richer Demo examples.
* Added `Time` axis.
* Added `SingleAxis`.
* Added multiple coordinate systems: `Grid`, `Polar`, `Radar`, `SingleAxis`.
* Added multiple animation methods.
* Added multiple chart interactions.
* Added internationalization support.
* Added `Widgets`.
* Added multiple extension charts.

## v3.0.0_preview9

## v3.0.0_preview8

## v3.0.0_preview7

## v3.0.0_preview6

## v3.0.0_preview5

## v3.0.0_preview4

## v3.0.0_preview3

## v3.0.0_preview2

## v3.0.0_preview1

## v2.8.1

* (2022.05.03) Added `onLegendClick`, `onLegendEnter` and `onLegendExit` delegate callbacks for `Legend`
* (2022.04.21) Fixed bug #192 with `RingChart` `Tooltip` exception
* (2022.04.21) Fixed error when setting `minShowNum` in `DataZoom`

## v2.8.0

* (2022.04.10) Added the debug information panel
* (2022.04.09) Fixed `VisualMap` not working in some cases
* (2022.04.08) Optimized `XCharts` initialization #190
* (2022.04.08) Fixed color error #187 in `Radar`
* (2022.03.24) Fixed `Axis` precision issue #184

## v2.7.0

* (2022.03.20) Release `v2.7.0` version
* (2022.02.21) Fixed chart name repeat check error #183
* (2022.02.17) Fixed bug where axis split line might be displayed outside the coordinate system #181
* (2022.02.08) Fixed {d} formatter error when value is 0
* (2022.02.08) Fixed `YAxis` `AxisLabel`'s `onZero` does not work
* (2022.01.06) Improved `Zebra` bar chart

## v2.6.0

* (2021.12.30) Release `v2.6.0` version
* (2021.12.21) Fixed `Emphasis` dont work
* (2021.12.17) Fixed `MarkLine` does not auto refresh label active when serie hide #178
* (2021.12.10) Improved `Radar`'s `AxisLine` and `SplitLine` to be controlled separately
* (2021.12.08) Fixed y axis does not refresh when serie hidden
* (2021.12.04) Added `Symbol` new types: `EmptyRect`, `EmptyTriangle`, `EmptyDiamond`
* (2021.12.04) Added setting symbol empty area color by itemStyle's backgroundColor
* (2021.12.03) Fixed formatter `{c}` not work #175
* (2021.12.03) Fixed axis `boundaryGap` display error in some cases #174
* (2021.11.30) Fixed serie `ignore` display error in some cases #173

## v2.5.0

* (2021.11.27) Release `v2.5.0` version
* (2021.11.27) Added `Tooltip` delegate function `positionFunction`
* (2021.10.29) Removed settings for `TextMeshPro` when package first imported
* (2021.10.29) Added support for `{e}` in `Tooltip` #170
* (2021.09.08) Improved `RadarChart`
* (2021.09.07) Fixed bug where `label` does not disappear at the end of `PieChart` fade animation #168
* (2021.09.06) Fixed bug where `GaugeChart` changing `splitNumber` with code does not refresh `label` #167

## v2.4.0

### Main points

* LineChart support the line of ignore data is disconnected or connected
* LineChart support animation at a constant speed
* Other optimizations and bug fixes

### Details

* (2021.08.31) Release `v2.4.0` version
* (2021.08.31) Optimized the gradient effect of `RingChart`
* (2021.08.31) Fixed bug where `SerieLabel` does not refresh when `DataZoom` is dragged (#165)
* (2021.08.25) Fixed an issue where the theme switch could not be save to the scene (#166)
* (2021.08.24) Added `Animation`'s `alongWithLinePath`
* (2021.08.22) Added `Serie`'s `ignoreLineBreak` (#164)
* (2021.08.22) Fixed `Axis` label may not be updated when `DataZoom` is turn on (#164)
* (2021.08.15) Improved `Axis`'s `AxisLabel` text rotate setting to avoid inconsistency offset in `DataZoom` (#163)
* (2021.08.14) Added `Legend`'s `textAutoColor` to set the text color match with `Serie` color (#163)
* (2021.08.12) Optimize `BarChart` setting `Corner` when the positive and negative columns are fillet symmetric
* (2021.08.03) Fixed y axis not displaying when all data is 0
* (2021.07.29) Fixed ignored data will also participate in calculations when `ignore` is enabled (#161)
* (2021.07.29) Improved `BarChart`'s `Zebra` gradient support
* (2021.07.26) Fixed issue where `XCharts` path could not be found when `TextMeshPro Enable` (#160)

## v2.3.0

### Main points

* Data store upgraded from `float` to `double`
* Added `MarkLine`
* `Serie` can use `IconStyle` to configure ICONS uniformly
* `Label` supports custom display styles with code
* `DataZoom` is perfect
* `PieChart` optimization
* Problem fixes

### Upgrade Note

Since the data type is upgraded to `double`, the implicit conversion of `float` to `double` may have precision problems, so it is recommended that all previous data types of `float` be manually changed to `double`.

### Details

* (2021.07.24) Release `v2.3.0` version
* (2021.07.22) Improved `SerieSymbol` to support `PictorialBarchart` extension
* (2021.07.19) Fixed issue where `Tooltip` was not displayed on `WdbGL` platform
* (2021.07.18) Added `iconStyle` for serie
* (2021.07.15) Added `MarkLine` (#142)
* (2021.07.09) Optimize `BarChart` to set whether to show bars via `seriedata.show`
* (2021.07.08) Optimize data storage type from `float` to `double`
* (2021.07.05) Fixed `Piechart` `avoidLabelOverlap` parameter not working
* (2021.07.04) Fixed incorrect mouse area indication after `PieChart` selected sector
* (2021.07.04) Optimize when the `Label` of `PieChart` is `Inside`, the offset can be adjusted by the parameter `Margin`
* (2021.07.01) Added `DataZoom` arguments to `supportInsideScroll` and `supportInsideDrag` to set whether scrolling and dragging are supported in the coordinate system
* (2021.06.27) Add `showStartLabel` and `showEndLabel` arguments to `AxisLabel` to set whether the `Label` should be displayed at the beginning and end of the `AxisLabel`
* (2021.06.27) Added `formatter` delegate method to `AxisLabel` and `SerieLabel` (#145)
* (2021.06.27) Added `DataZoom`'s `orient` parameter to set horizontal or vertical styles
* (2021.06.21) Added `iconStyle`'s `AutoHideWhenLabelEmpty` to set whether the icon is automatically hidden when `label` is empty

# # v2.2.3

* (2021.06.20) Release `v2.2.3` version
* (2021.06.20) Fixed the default display of `Icon` in `Axis`

## v2.2.2

* (2021.06.18) Release `v2.2.2` version
* (2021.06.18) Optimize `Axis` to automatically hide `Icon` when `Label` is empty
* (2021.06.17) Fixed an issue where `maxCache` was set to one more number of actual data
* (2021.06.17) Fixed an issue where `TextMeshPro` could not be opened and closed in time to refresh
* (2021.06.17) Fixed an issue where `XCharts` always pops up when importing `XCharts`

## v2.2.1

* (2021.06.13) Release `v2.2.1` version
* (2021.06.13) Improved support for multiple screens
* (2021.06.12) Added `iconStyle` `align` parameter to set the horizontal alignment of the icon
* (2021.06.12) Improve `Theme` import (#148)
* (2021.06.10) Fixed compatibility issues with `Unity` version (#154)
* (2021.06.05) Improved Candlestickchart support for inverse (#152)
* (2021.06.04) Fixed `Gauge` having an abnormal pointer position when the minimum value is negative (#153)

## v2.2.0

* (2021.05.30) Release `v2.2.0` version
* (2021.05.25) Improved `TextStyle` support for `alignment`
* (2021.05.24) Fixed the problem that `Label` could not display properly when `PieChart` data were all `0`
* (2021.05.24) Fixed an issue where `Serie Name` was not working on the `Add Serie` panel (#149)
* (2021.05.23) Added `TextStyle` `autoWrap` to set whether to wrap lines
* (2021.05.23) Added `TextStyle` `autoAlign` whether to set alignment automatically
* (2021.05.23) Added `width` and `height` of `axisLabel` to support custom text length and width
* (2021.05.23) Added `Axis` `iconStyle` and `icons` to support setting coordinate Axis labels to display icons
* (2021.05.20) Added the `insertDataHead` parameter to `Serie` and `Axis` to control whether data is inserted into the head or tail
* (2021.05.18) Optimize chart creation under `Editor` #147
* (2021.05.16) Pull out the `Ganttchart` chart and provide it as an extension module
* (2021.05.11) Added support for `VisualMap` to set color by `Piecewise`
* (2021.05.09) Fixed an issue where `RingChart` could not set the background color of the ring  #141
* (2021.05.08) Added `Liquidchart` support for `Rect` shape
* (2021.05.07) Improved the `Axis` scale performance #135
* (2021.05.01) Added `Settings` parameters for painter's material #140
* (2021.05.01) Fixed an issue where some super large or super small values could not be properly represented
* (2021.04.29) Fixed an issue with `Radar` switching to `Circle` anomaly #139
* (2021.04.29) Added `Settings`'s `reversePainter` to set whether or not `Serie` is drawn in reverse order
* (2021.04.28) Fixed bug where `AxisLabel` displayed incorrectly with `DataRoom` (#138)
* (2021.04.26) Fixed dynamically creating chart at runtime would be abnormal #137
* (2021.04.26) Added support for `Barchart` to draw gradient borders
* (2021.04.23) Added support for custom charts
* (2021.04.22) Fixed bug where `Gauge` `axisLabel`'s text color could not be adjusted
* (2021.04.13) Add the `ShowStarttick` and '`ShowEndTick` parameters of 'AxisTick' to control whether the first and last ticks are displayed
* (2021.04.13) Improved multi-axis support #132

## v2.1.1

* (2021.04.13) Define the code and clear `Warning`
* (2021.04.13) Fixed compatibility issues with `Unity` version
* (2021.04.12) Fixed problem `missing class attribute 'ExtensionOfNativeClass'` after Theme refactoring #131

## v2.1.0

* (2021.04.07) Release `v2.1.0` version
* (2021.03.31) Optimized and refactor `Theme` to solve problems with the same or missing references #118
* (2021.03.30) Optimized `Tooltip` to support setting different category axis data #129
* (2021.03.29) Optimized the custom draw callback API
* (2021.03.25) Added `Ganttchart`
* (2021.03.22) Added `Theme` `Unbind` button to unbind theme when copying chart #118
* (2021.03.18) Fixed an issue where the check box after `Foldout` in `Inspector` could not be checked
* (2021.03.18) Fixed an issue with `BarChart` displaying an exception in the `0` value
* (2021.03.14) Fixed `Tooltip` indicator was not indicating the correct location in some cases
* (2021.03.13) Optimized the editing experience and component refresh after `MulticomponentMode` is enabled #128
* (2021.03.10) Added `CandlestickChart` #124
* (2021.03.06) Added `PieChart`'s `minAngle` parameter to support setting minimum sector angle #117
* (2021.03.05) Added support for `Legend` for several built-in ICONS #90
* (2021.03.02) Added `DataRoom` support for value axes #71
* (2021.03.02) Optimized `TextMeshPro` compatibility issue #125
* (2021.03.01) Fixed display exception of hidden gameObjects when enabling and disabling a chart #125

## v2.0.1

* (2021.02.26) Fixed incorrect position of `Tooltip` in `HeatmapChart` #123
* (2021.02.22) Fixed compatibility issues with `Unity` version
* (2021.02.21) Added `Tooltip` parameter `ignoreDataShow`
* (2021.02.19) Fixed an issue where charts could appear abnormal when under `LayoutGroup` control #121
* (2021.02.18) Fixed an issue where the `Radar` could not refresh itself after parameter changing #122

## v2.0.0

* (2021.02.05) Release `v2.0.0` version
* (2021.02.03) Fixed an issue where `Axisline` `OnZero` did not work on `YAxis` #116
* (2021.01.29) Fixed incorrect display of `Tick` on `Category` axis when `BoundaryGap` and `alignWithLabel` are `True` #115
* (2021.01.25) Optimized some details
* (2021.01.22) Fixed a `Inpsector` displayed error

## v2.0.0-preview.2

* (2021.01.21) Release `v2.0.0-preview.2` version
* (2021.01.21) Fixed an error about `AxisTick` in `Inpsector`
* (2021.01.21) Fixed a build compatibility error
* (2021.01.19) Added `XChartsSettings` `editorShowAllListData` parameter to configure whether to display all the list's data in Inspector

## v2.0.0-preview.1

* (2021.01.19) Release `v2.0.0-preview.1` version

## v1.6.3

* (2021.01.02) Release `v1.6.3` version
* (2020.12.18) fixed an issue where updating data when `Animation` was not enabled caused the chart to keep refreshing
* (2020.12.01) fixed an issue where a newly created chart on `Unity2020` could not be drawn properly

## v1.6.2

* (2020.11.22) Release `v1.6.2` version
* (2020.11.22) Fixed an issue where `LineChart` draws an exception when the data is too dense #99
* (2020.11.21) Fixed an issue where the scale position of `LineChart` could be abnormal if `alignWithLabel` was `true`
* (2020.11.21) Fixed `Unity5` compatibility error reporting problem
* (2020.11.13) Improved `RadarChart` `Indicator` support for `\n` line feed
* (2020.11.12) Fixed `LineChart` reporting errors when the type was `Smooth` when the data was too secure #100
* (2020.10.22) Optimized the support of `VisualMap` for `Piecewise` in `HeatmapChart`
* (2020.09.22) Fixed `PieChart` inconsistent border size
* (2020.09.18) Added `Remove All Chart Object` to Remove All child nodes under the Chart (automatically reinitialized)
* (2020.09.18) Fixed `SerieLabel` also displayed after hided `Serie` by clicked the legend #94
* (2020.09.18) Optimized coordinate axis calibration and text display #93
* (2020.09.17) Fixed `Package` import missing `meta` file causing failure #92
* (2020.09.08) Optimized the color of `Legend` to automatically match the custom color of `ItemStyle`
* (2020.09.05) Optimized `LineChart` to display `XAxis1` without using `XAxis1`.
* (2020.08.29) Added `toColor` and `toColor2` of `LineStyle` to set the horizontal gradient of `LineChart`. Cancel `ItemStyle` to set the horizontal gradient of `LineChart`.
* (2020.08.29) Added the `onPointerClickPie` of `PieChart`, a callback function of click pie area.
* (2020.08.29) Added the `onPointerClickBar` of `BarChart`, a callback function of click bar.

## v1.6.0

* (2020.08.24) Release `v1.6.0` version
* (2020.08.23) Refactor code, replace `Color` with `Color32` for reduce implicit conversion (Can cause custom colors to lose, reference [FAQ 29](https://github.com/XCharts-Team/XCharts/blob/master/Assets/XCharts/Documentation/XChartsFAQ-ZH.md) to upgrade)
* (2020.08.15) Optimized `PieChart` drawing performance effect #85
* (2020.08.11) Added `LiquidChart` data change animation#83
* (2020.08.11) Optimized `PieChart` text stack and lead line effects#85
* (2020.08.08) Optimized `LineChart` the rendering performance of dense data
* (2020.07.30) Added `LineChart` to configure gradient through `VisualMap` or `ItemStyle`#78
* (2020.07.25) Fixed a problem with `LineChart` emerging abnormal in animation drawing#79
* (2020.07.25) Fixed a problem with gradual discoloration on `LiquidChart` at `100%`#80
* (2020.07.25) Added `RadarChart` support for `formatter` of `Tooltip`#77
* (2020.07.23) Added `RingChart` ring gradient support#75
* (2020.07.21) Added `formatter` of `AxisLabel` and `SerieLabel` to configure numeric formatting separately.
* (2020.07.17) Added animation completion callback interface for `SerieAnimation`.
* (2020.07.17) Optimized `Chart` under `ScrollView` without affecting the scrolling and dragging of `ScrollView`.
* (2020.07.16) Fixed a problem with `Tooltip` that would also show up if it was blocked on top. #74
* (2020.07.07) Fixed issue where  `SerieLabel` position was out of order
* (2020.07.07) Added `Tooltip` to the `offset` parameter
* (2020.07.06) Added `Liquidchart`
* (2020.07.01) Added `PolarChart`

## v1.5.2

* (2020.06.25) Fixed an issue where `BarChart` would draw a small number of bars when the value was  `0`
* (2020.06.24) Fixed an issue where `PieChart` was drawing abnormally after setting `Clockwise` #65
* (2020.06.23) Optimized the drawing effect of `LineChart` when the difference between peak and valley is too large #64
* (2020.06.18) Fixed an issue where `SerieLabel` might not be displayed when adding data again
* (2020.06.17) Added `SerieData` to `serieSymbol` #66
* (2020.06.17) Fixed `Check For Update` bug in `Unity 2018` version #63
* (2020.06.16) Added `Serie` `avoidLabelOverlap` parameter to avoid pie chart TAB stacking #56
* (2020.06.15) Fixed an issue where the `SerieLabel` control display could be deranged
* (2020.06.11) Fixed `Check warning` not working
* (2020.06.11) Fixed issue where `Piechart` and `Ringchart` were not displayed when data fraction was very small
* (2020.06.11) Added `Tooltip` to `titleFormatter` to support configuration placeholder `{i}` to ignore not showing titles
* (2020.06.07) Added `customFadeInDelay` and other custom data item delay and duration callback function #58
* (2020.06.07) Optimized `Piechart` to display equal parts when all the data are `0` #59
* (2020.06.04) Added `autoOffset` parameter setting for `SerieLabel` to determine whether the up and down offset is automatically determined
* (2020.06.04) Added `Tooltip` to `AlwayShow` parameter setting to always show after triggering
* (2020.06.04) Tooltip's `formatter` supports `{.1}` wildcards
* (2020.06.04) Optimizes the number of `Legend` to automatically wrap to display #53

## v1.5.1

* (2020.06.03) 发布`v1.5.1`版本
* (2020.06.02) 增加`Radar`的`ceilRate`，设置最大最小值的取整倍率
* (2020.06.02) 优化`Tooltip`的`formatter`，支持`{c1:1-1:f1}`格式配置
* (2020.05.31) 优化`Background`组件的生效条件，需要有单独的父节点（升级前需要自己处理旧的背景节点）
* (2020.05.30) 优化`PieChart`支持设置`ignoreValue`不显示指定数据
* (2020.05.30) 修复`RadarChart`为`Circle`时不绘制`SplitArea`的问题
* (2020.05.30) 优化`RadarChart`在设置`max`为`0`时可自动刷新最大值
* (2020.05.29) 修复`PieChart`设置`gap`时只有一个数据时绘制异常的问题
* (2020.05.27) 修复调用`UpdateDataName()`接口时不会自动刷新的问题
* (2020.05.27) 优化`柱状图`的渐变色效果
* (2020.05.24) 修复`Axis`同时设置`boundaryGap`和`alignWithLabel`时`Tick`绘制异常的问题
* (2020.05.24) 优化版本更新检测
* (2020.06.25) release `v1.5.2`


## v1.5.0

* (2020.05.22) 发布`v1.5.0`版本
* (2020.05.21) 增加`圆角柱图`支持渐变
* (2020.05.21) 增加`Background`背景组件
* (2020.05.19) 隐藏`Hierarchy`试图下自动生成的子节点
* (2020.05.18) 增加`chartName`属性可指定图表的别称，可通过`XChartMgr.Instance.GetChart(chartName)`获取图表
* (2020.05.16) 增加部分鼠标事件回调
* (2020.05.15) 优化自带例子，`Demo`改名为`Example`
* (2020.05.13) 增加`Serie`的`large`和`largeThreshold`参数配置折线图和柱状图的性能模式
* (2020.05.13) 完善Demo，增加性能演示Demo
* (2020.05.13) 优化性能，优化大数据绘制，重构代码
* (2020.05.04) 增加`numericFormatter`参数可配置数值格式化显示，去掉`forceENotation`参数
* (2020.04.28) 增加`自由锚点`支持，任意对齐方式
* (2020.04.23) 优化`ScatterChart`的`Tooltip`显示效果
* (2020.04.23) 增加`Tooltip`的`formatter`对`{.}`、`{c:0}`、`{c1:1}`的支持
* (2020.04.19) 优化`LineChart`折线图的区域填充渐变效果
* (2020.04.19) 增加`AxisLabel`的`onZero`参数可将`Label`显示在`0`刻度上
* (2020.04.19) 增加`Serie`和`AxisLabel`的`showAsPositiveNumber`参数将负数数值显示为正数
* (2020.04.18) 增加`Covert XY Axis`互换XY轴配置
* (2020.04.17) 增加`Axis`可通过`inverse`参数设置坐标轴反转
* (2020.04.16) 修复`Check warning`在`Unity2019.3`上的显示问题
* (2020.04.16) 修复`PieChart`在设置`Space`参数后动画绘制异常的问题

## v1.4.0

* (2020.04.11) 发布`v1.4.0`版本
* (2020.04.11) 增加`Check warning`检测功能
* (2020.04.09) 修复`Legend`初始化异常的问题
* (2020.04.08) 增加`PieChart`通过`ItemStyle`设置边框的支持
* (2020.03.29) 增加`Axis`的`ceilRate`设置最大最小值的取整倍率
* (2020.03.29) 增加`BarChart`可通过`itemStyle`的`cornerRadius`设置`圆角柱图`
* (2020.03.29) 增加`itemStyle`的`cornerRadius`支持圆角矩形
* (2020.03.24) 优化`Editor`参数编辑，兼容`Unity2019.3`及以上版本
* (2020.03.24) 增加`Serie`在`inspector`上可进行调整顺序、添加和删除操作
* (2020.03.23) 修复`Title`的`textStyle`和`subTextStyle`无效的问题
* (2020.03.22) 增加`BarChart`通过`barType`参数设置`胶囊柱状图`
* (2020.03.21) 增加`BarChart`和`HeatmapChart`可通过`ignore`参数设置忽略数据的支持
* (2020.03.21) 增加`ItemStyle`的`tooltipFormatter`参数可单独配置`Serie`的`Tooltip`显示
* (2020.03.20) 修复`X Axis 1`和`Y Axis 1`配置变更时不会自动刷新的问题
* (2020.03.20) 增加`AxisTick`的`width`参数可单独设置坐标轴刻度的宽度
* (2020.03.20) 增加`Serie`的`radarType`参数设置`多圈`和`单圈`雷达图
* (2020.03.17) 增加`BarChart`可用`ItemStyle`的`backgroundColor`设置数据项背景颜色
* (2020.03.17) 增加`SerieData`的`ItemStyle`和`Emphasis`可单独配置数据项样式的支持
* (2020.03.15) 重构`EmptyCricle`类型的`Symbol`边宽取自`ItemStyle`的`borderWidth`参数
* (2020.03.15) 重构`SerieSymbol`，去掉`color`和`opacity`参数，取自`ItemStyle`

## v1.3.1

* (2020.03.14) 发布`v1.3.1`版本
* (2020.03.14) 修复`LineChart`开启`ingore`时部分数据可能绘制异常的问题
* (2020.03.13) 修复`LineChart`的`label`偏移显示异常的问题

## v1.3.0

* (2020.03.11) 发布`v1.3.0`版本
* (2020.03.11) 优化`LineChart`的`label`偏移显示
* (2020.03.11) 优化清空并重新添加数据后的自动刷新问题
* (2020.03.10) 增加`LineChart`的普通折线图可通过`ignore`参数设置忽略数据的支持
* (2020.03.09) 增加`BarChart`可通过`ItemStyle`配置边框的支持
* (2020.03.08) 增加`RingChart`环形图
* (2020.03.05) 调整`Serie`的`arcShaped`参数重命名为`roundCap`
* (2020.03.05) 增加运行时和非运行时参数变更自动刷新图表
* (2020.02.26) 重构`Legend`图例，改变样式，增加自定义图标等设置
* (2020.02.23) 增加`BaseChart.AnimationFadeOut()`渐出动画，重构动画系统
* (2020.02.13) 增加`BaseChart.RefreshTooltip()`接口立即重新初始化`Tooltip`组件
* (2020.02.13) 增加`Tooltip`的`textStyle`参数配置内容文本样式，去掉`fontSize`和`fontStyle`参数
* (2020.02.13) 增加`TextStyle`的`lineSpacing`参数配置行间距
* (2020.02.11) 增加`Radar`的`splitLine`参数配置分割线，去掉`lineStyle`参数
* (2020.02.11) 增加`Tooltip`的`backgroundImage`参数配置背景图
* (2020.02.11) 增加`Tooltip`的`paddingLeftRight`和`paddingTopBottom`参数配置文字和边框的间距
* (2020.02.11) 增加`Tooltip`的`lineStyle`参数配置指示线样式
* (2020.02.11) 增加`Axis`的`splitLine`参数控制分割线，去掉`showSplitLine`和`splitLineType`参数（更新时需要重新设置分割线相关设置）
* (2020.02.10) 增加`Serie`的`clip`参数控制是否超出坐标系外裁剪（只适用于折线图、柱状图、散点图）
* (2020.02.08) 增加`SerieSymbol`的`gap`参数控制图形标记的外留白距离
* (2020.01.26) 增加`TextLimit`组件可以设置`AxisLabel`的文本自适应
* (2020.01.20) 优化`Tooltip`设置`itemFormatter`时显示系列颜色
* (2020.01.20) 增加`Radar`雷达图在`inspector`配置`areaStyle`的支持

## v1.2.0

* (2020.01.15) 发布`v1.2.0`版本
* (2020.01.15) 增加`AxisLabel`格式化为整数的支持（`{value:f0}`）
* (2020.01.15) 增加折线图对数轴`Log`的支持
* (2020.01.09) 修复当设置`DataZoom`的`minShowNum`时可能异常的问题
* (2020.01.08) 修复当设置`AxisLine`的`onZero`时刻度显示异常的问题
* (2020.01.08) 增加`Mask`遮罩遮挡支持
* (2019.12.21) 增加`Tooltip`的单个数据项和标题的字符串模版格式器
* (2019.12.21) 增加`DataZoom`的最小显示数据个数`minShowNum`
* (2019.12.20) 增加`Demo40_Radar.cs`雷达图代码操作`Demo`
* (2019.12.20) 添加`RadarChart`相关API接口

## v1.1.0

* (2019.12.17) 发布`v1.1.0`版本
* (2019.12.16) 修复`Overlay`模式下不显示`Tooltip`的问题
* (2019.12.15) 增加`Title`的`TextStyle`支持
* (2019.12.11) 修复`Legend`都隐藏时`Value轴`还显示数值的问题
* (2019.12.11) 修复`Series->Data->Size`重置为0后设置无效的问题
* (2019.12.06) 修复数据过小时`AxisLabel`直接科学计数法显示的问题
* (2019.12.04) 优化和完善数据更新`UpdateData`接口
* (2019.12.03) 增加圆环饼图的圆角支持，参数：`serie.arcShaped`
* (2019.12.03) 增加数据更新动画,参数：`serie.animation.dataChangeEnable`
* (2019.11.30) 增加`GaugeChart`仪表盘
* (2019.11.22) 修复`BarChart`清空数据重新赋值后`SerieLabel`显示异常的问题
* (2019.11.16) 修复`SerieLabel`设置`color`等参数不生效的问题

## v1.0.5

* (2019.11.12) 发布`v1.0.5`版本
* (2019.11.12) 修复`2018.3`以下版本打开项目报错的问题
* (2019.11.12) 增加`IconStyle`子组件，优化`SerieData`的图标配置
* (2019.11.11) 修复`Serie`的图标显示在上层遮挡`Label`的问题
* (2019.11.11) 修复饼图当数据过小时视觉引导线会穿透的的问题
* (2019.11.09) 修复饼图添加数据时`Label`异常的问题
* (2019.11.09) 优化结构，分离为`XCharts`和`XChartsDemo`两部分

## v1.0.4

* (2019.11.05) 发布`v1.0.4`版本
* (2019.11.05) 增加`Radar`雷达组件文本样式参数配置支持
* (2019.11.04) 修复`Unity2018.3`以下版本代码不兼容的问题
* (2019.11.04) 优化`SerieLabel`过多时引起的性能问题

## v1.0.3

* (2019.11.03) 发布`v1.0.3`版本
* (2019.11.03) 增加`Editor`快捷添加图表：`Hierarchy`试图下右键`XCharts->LineChart`
* (2019.11.02) 优化非配置参数变量命名和访问权限，简化`API`

## v1.0.2

* (2019.10.31) 发布`v1.0.2`版本
* (2019.10.31) 修复`prefab`预设制作报错的问题
* (2019.10.31) 增加访问主题组件API：`BaseChart.theme`

## v1.0.1

* (2019.10.26) 发布`v1.0.1`版本
* (2019.10.26) 修复版本检查功能在非运行时异常的问题
* (2019.10.26) 增加科学计数法显示数值的支持（查阅`forceENotation`参数）
* (2019.10.26) 增加`Axis`类目轴数据为空时的默认显示支持
* (2019.10.26) 增加`Axis`数值轴的最大最小值可设置为小数的支持，优化极小数图表的表现效果

## v1.0.0

* (2019.10.25) 发布`v1.0.0`版本
* (2019.10.23) 增加版本检测功能：`Component -> XCharts -> Check For Update`
* (2019.10.22) 增加`Package Manager`安装的支持
* (2019.10.20) 增加`Demo`首页`BarChart`的代码动态控制效果
* (2019.10.18) 增加`Serie`的`barType`参数，可配置`斑马柱状图`
* (2019.10.18) 增加`Serie`的`barPercentStack`参数，可配置`百分比堆叠柱状图`
* (2019.10.16) 增加`Demo`首页`LineChart`的代码动态控制效果
* (2019.10.15) 移除`Pie`组件，相关参数放到`Settings`中配置
* (2019.10.15) 增加`Demo`首页，展示代码动态控制效果
* (2019.10.14) 增加`RadarChart`、`ScatterChart`和`HeatmapChart`的起始动画效果
* (2019.10.14) 增加`SerieData`的`radius`自定义数据项的半径
* (2019.10.14) 增加`HeatmapChart`热力图
* (2019.10.14) 增加`VisualMap`视觉映射组件
* (2019.10.14) 增加`ItemStyle`数据项样式组件
* (2019.10.14) 增加`Emphasis`高亮样式组件
* (2019.10.10) 增加`Settings`全局参数配置组件，开放更多参数可配置
* (2019.10.09) 增加`AreaStyle`的高亮相关参数配置鼠标悬浮时高亮之前区域
* (2019.10.09) 优化`DataZoom`组件，增加双指缩放
* (2019.10.05) 增加`SerieLabel`的`LineType`给饼图配置不同类型的视觉引导线
* (2019.10.02) 增加`ScatterChart`同时对`Scatter`和`Line`的支持，实现折线图和散点图的组合图
* (2019.10.01) 重构代码，废弃`Series.series`接口，用`Series.list`代替
* (2019.10.01) 增加`customDrawCallback`自定义绘制回调
* (2019.10.01) 增加`SmoothDash`平滑虚线的支持
* (2019.09.30) 增加`Serie`采样类型`sampleType`的相关配置
* (2019.09.29) 增加`SerieSymbol`关于显示间隔的相关配置
* (2019.09.29) 重构代码：
  * `BaseChart`的`sampleDist`删除，`Serie`增加`lineSampleDist`
  * `BaseChart`的`minShowDataNumber`删除，`Serie`增加`minShow`
  * `BaseChart`的`maxShowDataNumber`删除，`Serie`增加`maxShow`
  * `BaseChart`的`maxCacheDataNumber`删除，`Serie`增加`maxCache`
  * `BaseChart`的`AddSerie()`接口参数调整
  * `BaseChart`的`UpdateData()`接口参数调整
  * `Axis`增加`maxCache`
* (2019.09.28) 增加`LineChart`和`BarChart`同时对`Line`、`Bar`类型`Serie`的支持，实现折线图和柱状图的组合图
* (2019.09.27) 增加`Axis`的`splitNumber`设置为`0`时表示绘制所有类目数据
* (2019.09.27) 增加`SampleDist`采样距离的配置，对过密的曲线开启采样，优化绘制效率
* (2019.09.27) 增加`XCharts问答`、`XChartsAPI接口`、`XCharts配置项手册`等文档
* (2019.09.26) 增加`AnimationReset()`重置初始化动画接口
* (2019.09.26) 优化`LineChart`的密集数据的曲线效果
* (2019.09.25) 优化`SerieData`的自定义图标不与`SerieLabel`关联，可单独控制是否显示
* (2019.09.24) 增加`SerieData`的自定义图标相关配置支持
* (2019.09.23) 增加`Formatter`配置`Axis`的`AxisLabel`的格式化输出
* (2019.09.23) 增加`Tooltip`的`FontSize`、`FontStyle`配置字体大小和样式
* (2019.09.23) 增加`Formatter`配置`SerieLabel`、`Legend`、`Tooltip`的格式化输出
* (2019.09.19) 增加`LineArrow`配置带箭头曲线
* (2019.09.19) 增加`Tooltip`的`FixedWidth`、`FixedHeight`、`MinWidth`、`MinHeight`设置支持
* (2019.09.18) 增加单条堆叠柱状图
* (2019.09.18) 增加虚线`Dash`、点线`Dot`、点划线`DashDot`、双点划线`DashDotDot`等类型的折线图支持
* (2019.09.17) 增加`AnimationEnabel()`启用或取消起始动画接口
* (2019.09.17) 增加`Axis`的`Interval`强制设置坐标轴分割间隔
* (2019.09.16) 去掉`Serie`中的旧版本数据兼容，不再支持`xData`和`yData`
* (2019.09.06) 增加`Animation`在重新初始化数据时自启动功能
* (2019.09.06) 增加`SerieLabel`的`Border`边框相关配置支持
* (2019.09.05) 增加`PieChart`的`Animation`初始化动画配置支持
* (2019.09.03) 增加`BarChart`的`Animation`初始化动画配置支持
* (2019.09.02) 增加`LineChart`的`Animation`初始化动画配置支持
* (2019.08.22) 增加`AxisName`的`Offset`偏移配置支持
* (2019.08.22) 增加`AxisLine`的`Width`配置支持
* (2019.08.20) 增加`SerieLabel`的背景宽高、文字边距、文字旋转的配置
* (2019.08.20) 增加`BarChart`的`Label`配置支持
* (2019.08.15) 增加`LineChart`的`Label`配置
* (2019.08.15) 重构`BarChart`，移除`Bar`组件，相关参数统一放到`Serie`中配置
* (2019.08.15) 重构`LineChart`，移除`Line`组件，相关参数统一放到`Serie`中配置

## v0.8.3

* (2019.08.15) 发布`v0.8.3`版本
* (2019.08.14) 修复`PieChart`的`Label`无法自动更新的问题
* (2019.08.13) 修复`UpdateData`接口无法更新数据的问题
* (2019.08.07) 增加`SerieSymbol`的`Color`、`Opacity`配置

## v0.8.2

* (2019.08.07) 发布`v0.8.2`版本
* (2019.08.07) 修复区域平滑折线图显示异常的问题
* (2019.08.06) 修复`serie`系列数超过调色盘颜色数时获取的颜色异常的问题
* (2019.08.06) 修复当`Axis`的`minMaxType`为`Custom`时`max`设置为`100`不生效的问题

## v0.8.1

* (2019.08.04) 发布`v0.8.1`版本
* (2019.08.04) 修复`Inspector`中修改数据不生效的问题

## v0.8.0

* (2019.08.04) 发布`v0.8.0`版本
* (2019.08.04) 优化`RadarChart`雷达图，增加多雷达图支持
* (2019.08.01) 增加代码API注释文档，整理代码
* (2019.07.29) 增加`Radius`、`Area`两种南丁格尔玫瑰图展示类型
* (2019.07.29) 增加`SerieLabel`配置饼图标签，支持`Center`、`Inside`、`Outside`等显示位置
* (2019.07.28) 增加`PieChart`多饼图支持
* (2019.07.23) 优化`Theme`主题的自定义，切换主题时自定义配置不受影响
* (2019.07.22) 增加`EffectScatter`类型的散点图
* (2019.07.21) 增加`ScatterChart`散点图
* (2019.07.21) 增加`SerieData`支持多维数据配置
* (2019.07.20) 增加`Symbol`配置`Serie`标志图形的显示
* (2019.07.19) 增加用代码添加动态正弦曲线的示例`Demo11_AddSinCurve`
* (2019.07.19) 优化`Legend`的显示和控制
* (2019.07.18) 优化抗锯齿，曲线更平滑
* (2019.07.18) 增加`Tooltip`指示器类型，优化显示控制
* (2019.07.15) 增加`Size`设置图表尺寸
* (2019.07.14) 增加`二维数据`支持，XY轴都可以设置为数值轴
* (2019.07.13) 增加`双坐标轴`支持，代码改动较大

## v0.5.0

* (2019.07.10) 发布`v0.5.0`版本
* (2019.07.09) 增加`AxisLine`配置坐标轴轴线和箭头
* (2019.07.03) 增加`AxisLabel`配置坐标轴`刻度标签`
* (2019.07.02) 增加`selected`等相关参数配置`PieChart`的选中效果
* (2019.06.30) 增加`SplitArea`配置坐标轴`分割区域`
* (2019.06.29) 增加`AxisName`配置坐标轴`名称`
* (2019.06.20) 增加`AreaAlpha`控制`RadarChart`的`Area`透明度
* (2019.06.13) 增加`DataZoom`实现`区域缩放`
* (2019.06.01) 增加`stepType`实现`LineChart`的`阶梯线图`
* (2019.05.29) 增加`InSameBar`实现`BarChart`的`非堆叠同柱`
* (2019.05.29) 增加`crossLabel`控制`Tooltip`的`十字准星指示器`
* (2019.05.24) 增加`堆叠区域图`
* (2019.05.16) 增加`AxisMinMaxType`控制坐标轴最大最小刻度
* (2019.05.15) 完善数据接口
* (2019.05.14) 增加X轴`AxisType.Value`模式支持
* (2019.05.13) 增加负数数值轴支持
* (2019.05.11) 增加自定义`Editor`编辑
* (2019.03.21) 增加`Tooltip`
* (2018.11.01) 增加`Default`、`Light`、`Dark`三种默认主题

## v0.1.0

* (2018.09.05) 发布`v0.1.0`版本
