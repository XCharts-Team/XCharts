---
sidebar_position: 61
slug: /changelog
---

# Changelog

[master](#master)  
[v3.13.0](#v3130)  
[v3.12.1](#v3121)  
[v3.12.0](#v3120)  
[v3.11.2](#v3112)  
[v3.11.1](#v3111)  
[v3.11.0](#v3110)  
[v3.10.2](#v3102)  
[v3.10.1](#v3101)  
[v3.10.0](#v3100)  
[v3.9.0](#v390)  
[v3.8.1](#v381)  
[v3.8.0](#v380)  
[v3.7.0](#v370)  
[v3.6.0](#v360)  
[v3.5.0](#v350)  
[v3.4.0](#v340)  
[v3.3.0](#v330)  
[v3.2.0](#v320)  
[v3.1.0](#v310)  
[v3.0.1](#v301)  
[v3.0.0](#v300)  
[v3.0.0-preview9](#v300-preview9)  
[v3.0.0-preview8](#v300-preview8)  
[v3.0.0-preview7](#v300-preview7)  
[v3.0.0-preview6](#v300-preview6)  
[v3.0.0-preview5](#v300-preview5)  
[v3.0.0-preview4](#v300-preview4)  
[v3.0.0-preview3](#v300-preview3)  
[v3.0.0-preview2](#v300-preview2)  
[v3.0.0-preview1](#v300-preview1)  
[v2.8.1](#v281)  
[v2.8.0](#v280)  
[v2.7.0](#v270)  
[v2.6.0](#v260)  
[v2.5.0](#v250)  
[v2.4.0](#v240)  
[v2.3.0](#v230)  
[v2.2.3](#v223)  
[v2.2.2](#v222)  
[v2.2.1](#v221)  
[v2.2.0](#v220)  
[v2.1.1](#v211)  
[v2.1.0](#v210)  
[v2.0.1](#v201)  
[v2.0.0](#v200)  
[v2.0.0-preview.2](#v200-preview2)  
[v2.0.0-preview.1](#v200-preview1)  
[v1.6.3](#v163)  
[v1.6.0](#v160)  
[v1.5.2](#v152)  
[v1.5.1](#v151)  
[v1.5.0](#v150)  
[v1.4.0](#v140)  
[v1.3.1](#v131)  
[v1.3.0](#v130)  
[v1.2.0](#v120)  
[v1.1.0](#v110)  
[v1.0.5](#v105)  
[v1.0.4](#v104)  
[v1.0.3](#v103)  
[v1.0.2](#v102)  
[v1.0.1](#v101)  
[v1.0.0](#v100)  
[v0.8.3](#v083)  
[v0.8.2](#v082)  
[v0.8.1](#v081)  
[v0.8.0](#v080)  
[v0.5.0](#v050)  
[v0.1.0](#v010)  

## master

## v3.13.0

Key Features:

* Added the `UIText` extension component  
* Added the `UIToggle` extension component  
* Added the `UISlider` extension component  
* Refactored the `UIProgress` extension component  
* Added `borderWidth` and `emptyColor` configurations to `SymbolStyle`  
* Added the `size2` parameter to `SymbolStyle` to support rectangular markers  
* Other optimizations and bug fixes  

Detailed Changelog:

* (2025.01.01) Released `v3.13.0`  
* (2024.12.27) Added the `size2` parameter to `SymbolStyle` to support rectangular markers  
* (2024.12.26) Optimized `Text` alignment in `TextMeshPro` for proper centering  
* (2024.12.25) Added support for `{f0}` in the `Tooltip`'s `itemFormatter` setting  
* (2024.12.25) Fixed an issue where some labels on the `YAxis` might not display during range refresh  
* (2024.12.23) Added `borderWidth` and `emptyColor` configurations to `SymbolStyle`  
* (2024.12.17) Added the `UISlider` extension component  
* (2024.12.10) Added the `UIToggle` extension component  
* (2024.12.09) Fixed an issue where the `UITable`'s `scrollbar` could not be dragged  
* (2024.12.07) Fixed an issue where custom nodes could not be placed under the `Chart` node  
* (2024.12.05) Added the `UIText` extension component  
* (2024.12.04) Removed the unused `tmpAlignment` option from `TextStyle`  

## v3.12.1

Version Highlights:

* Enhanced Chinese and English support for the official website documentation.  
* Optimized the rendering performance of line charts when data points are densely packed.  
* Other issue fixes.  

Log Details:

* (2024.12.01) Released `v3.12.1` version.
* (2024.11.30) Fixed an issue where the `Tooltip` displayed incorrectly on mobile devices when setting other anchors in charts.
* (2024.11.27) Resolved some code warning issues in `Unity6`.
* (2024.11.26) Fixed a problem where the `Tooltip` might exceed the screen and appear incomplete under specific circumstances.
* (2024.11.24) Fixed an issue where `UITable` would also select items during dragging.
* (2024.11.22) Fixed an abnormal effect issue when dynamically changing the `Time` timeline with `Animation` enabled.
* (2024.11.18) Optimized `Line` rendering for better performance when data points are densely packed.
* (2024.11.16) Fixed an issue where `Animation` could not be enabled through code (#334).
* (2024.11.13) Fixed a problem where dynamically modifying the `start` and `end` of `DataZoom` through code did not refresh the chart.
* (2024.11.05) Fixed an issue where the `Title` remained visible after being set to hidden.
* (2024.11.01) Improved `website` documentation in both English and Chinese.

## v3.12.0

Version Highlights:

* Added `radiusGradient` parameter for `Ring` to set the gradient direction
* Added support for `date` and `time` in `numericFormatter`
* Improved `origin` parameter setting for `AreaStyle` to define the starting position of area filling
* Adjusted and perfected the documentation
* Other optimizations and fixes

Log Details:

* (2024.09.30) Released version `v3.12.0`
* (2024.09.27) Improved the `5-minute tutorial`
* (2024.09.24) Improved support for multiple Series in `Legend`'s `formatter` (#332)
* (2024.09.22) Adjusted the display style of the `Documentation`
* (2024.09.09) Added support for `date` and `time` in `numericFormatter`
* (2024.09.03) Improved the setting of the `origin` parameter for `AreaStyle` to define the starting position of area filling
* (2024.09.01) Added `radiusGradient` parameter for `Ring` to set the gradient direction
* (2024.09.01) Optimized the position of the first Label when `Axis` is used as a time axis


## v3.11.2

* (2024.08.01) Release `v3.11.2`
* (2024.07.29) Fixed compatibility issue with `Tooltip` reporting error on wechat mini game platform (#326)
* (2024.07.27) Adjust the default position of `AxisName` for `Axis`
* (2024.07.22) Improved the behavior of `Pie`'s `Label` when `Tooltip` is triggered
* (2024.07.21) Fix to `Tooltip` indicating inaccurate content when opening `DataZoom`
* (2024.07.17) Fixed an issue where `Label` of `MarkLine` could flash during initialization
* (2024.07.16) Optimized the default effect of `Tooltip` when `Axis` is `Time` timeline
* (2024.07.15) Optimized segmentation when Axis is Time
* (2024.07.14) Improved movement performance when `Axis` is `Time`
* (2024.07.12) Optimized the initial display effect of `Label`
* (2024.07.06) Fix to `Chart` background not adaptive when dynamically created (#323)

## v3.11.1

* (2024.07.01) Release `v3.11.1`
* (2024.07.01) Fixed an issue where `Serie` has multiple abnormal colors
* (2024.06.23) Fixed an issue where `labels` would pile up during initialization

## v3.11.0

Release Highlights:

* Added `Line3DChart` for 3D line charts
* Added `GraphChart` for relationship graphs
* Added support for 3D coordinate systems
* Added `triggerOn` setting for `Tooltip` to define trigger conditions
* Various bug fixes and optimizations

Changelog Details:

* (2024.06.16) Released version `v3.11.0`
* (2024.06.15) Added buttons for adding, deleting, and moving data up and down under `Editor`
* (2024.06.11) Fixed issue where `Axis`'s `IndicatorLabel` might overlap with `Tooltip`
* (2024.06.11) Fixed issue where `Tooltip`'s `Axis` `IndicatorLabel` might not display when in `Cross` mode (#315)
* (2024.06.10) Renamed `Tooltip`'s `Corss` to `Cross`
* (2024.06.09) Added `minCategorySpacing` setting for `Axis` to define the default minimum category spacing
* (2024.06.09) Fixed inaccurate indicator position of `Tooltip`'s `Cross` when `Axis` is a category axis and `DataZoom` is enabled
* (2024.06.06) Fixed animation issue when `Serie` is cloned (#320)
* (2024.06.04) Fixed issue where `Serie`'s `state` does not refresh when set dynamically via code
* (2024.05.29) Adjusted the right-click menu of `XCharts` in the `Hierarchy` view to `UI/XCharts`
* (2024.05.29) Added support for 3D coordinate systems to category axes
* (2024.05.19) Optimized editing performance in `Editor`
* (2024.05.09) Added utility class `JsonUtil`
* (2024.05.01) Fixed the issue where `Tooltip` caused garbage collection (GC) on every frame (#311) (by @stefanbursuc)
* (2024.04.23) Fixed chart exception issue after multiple calls to `ConvertXYAxis()`
* (2024.04.22) Fixed potential incorrect retrieval of `GridCoord` when `DataZoom` controls multiple axes (#317)
* (2024.04.22) Added 3D coordinate system
* (2024.04.15) Optimized `DateTimeUtil` for timezone issues when converting timestamps to `DateTime`
* (2024.04.15) Optimized `GridCoord` to display `Left` `Right` `Top` `Bottom` parameters even when `GridLayout` is enabled (#316)
* (2024.04.14) Fixed incorrect label position of `Tooltip`'s `Cross` when `DataZoom` is enabled (#315)
* (2024.04.12) Fixed incorrect effect of `Candlesticks` (#313)
* (2024.03.20) Added `triggerOn` setting for `Tooltip` to define trigger conditions
* (2024.03.19) Fixed color issue when setting `opacity` in `Pie`'s `ItemStyle` (#309)

## v3.10.2

* (2024.03.11) Release `v3.10.2`
* (2024.03.11) Fix to `Legend`'s `formatter` showing possible mismatches when setting `{d}` (#304)
* (2024.03.11) Fix to `Tooltip` still showing after moving out of coordinate system
* (2024.03.08) Fixed an issue where `Tooltip`'s title might not appear after upgrading from an older version

## v3.10.1

* (2024.02.21) Release `v3.10.1`
* (2024.02.19) Fix to `Tooltip` dot markers not adapting to color

## v3.10.0

Highlights:

* Added bi-category axis support
* Added more segmentation shortcut menu to create charts, which can create dozens of charts with one click
* Added chart border Settings to support rounded corner charts
* Fixed several issues

Extension features:

* Added `SankeyChart` Sankey chart
* Added `border` Settings for `UITable`

Log details:

* (2024.02.01) Release `v3.10.0`
* (2024.01.31) Fix to Tooltip not displaying after itemFormatter is set to -
* (2024.01.27) Fix to TextLimit not working after TextMeshPro is enabled (#301)
* (2024.01.24) Added `Bar` to support both X and Y axes as` Category `axes
* (2024.01.23) Added `{y}` wildcard to get the class name of the Y-axis
* (2024.01.23) Added `Line` to support both X and Y axes as` Category `axes
* (2024.01.18) Fixed dynamic modification of `Animation`s `type` code
* (2024.01.13) Added more quick Chart creation menus for Chart
* (2024.01.09) added `borderStyle` to `Background` to give the chart rounded corners by default
* (2024.01.07) Fix to invalid first ContentLabelStyle setting color for Tooltop
* (2024.01.01) Added `BorderStyle` border style
* (2023.12.26) Added support for Heatmap's maxCache parameter
* (2023.12.25) Optimizes the number of vertices drawn when `Line`` opens clip
* (2023.12.22) Fixed an issue where some border data of `Scatter` map was not displayed
* (2023.12.21) Fix to `TriggerTooltip()` interface may not fire when 0 or maximum index is specified
* (2023.12.19) Fixed an issue where `Legend`'s `LabelStyle` does not take effect after setting its formatter
* (2023.12.12) Added TextLimit for Legend to limit the length of the text displayed in the legend
* (2023.12.11) Fix to coordinate drawing failure when `Serie` was added with `double-maxvalue`
* (2023.12.10) Add `Serie` to `minShowLabel` to hide `labels` that are less than the specified value
* (2023.12.09) Add depth to `LevelStyle` to specify the level to which it belongs
* (2023.12.09) Added `LevelStyle` `LineStyle` to set line styles
* (2023.12.09) Adding `Serie` to `Link` can be used to add node-edge relationships to Sankey diagrams
* (2023.12.05) added `ResetChartStatus()` to actively reset chart status

## v3.9.0

Highlights:

* Added `Animation` in `Axis` to improve the animation effect of data changes
* Added `minorTick` for `Log` type of `Axis`
* Added MarkLine's `onTop` setting to displayed at the top level
* Perfect code comments and documentation
* Fixed several issues

Extension features:

* `UITable` adds the carousel function
* `UITable` adds data api and callback functions
* `Pie3DChart` optimizes rendering performance

Log details:

* (2023.12.01) Release `v3.9.0`
* (2023.12.01) Fixed inaccurate display of Tooltip's `titleFormatter` set to `{b}`
* (2023.11.30) Added support for `SerieData` to add `Label` separately
* (2023.11.28) Fix to `Tooltip` incorrectly indicating the number line
* (2023.11.24) fixed inaccurate return values in Chart's `UpdateData()` interface
* (2023.11.24) Fix to `Axis` not working smoothly when updating data
* (2023.11.23) Added Animation support for Axis
* (2023.11.16) Cancel `Legend`'s `formatter` and replace it with `LabelStyle`
* (2023.11.14) Improved annotation and documentation for LabelStyle's formatter (#291)
* (2023.11.11) Fix to comments Documentation for some comments in `Documentation` (#290)
* (2023.11.11) fixed an issue where Legend's formatter didn't refresh automatically when data was changed
* (2023.11.05) Fix to SerieEventData's value always being 0 (#287)
* (2023.11.03) Fix to abnormal mouse movement when setting `Bar` gradient (#285)
* (2023.11.02) Optimizes ignoring of formatter when SerieData is set to ignore
* (2023.11.01) Added whether MarkLine's `onTop` setting is displayed at the top level
* (2023.10.21) Fix to `Label` location exception when `Pie` has 0 data
* (2023.10.21) Added subscale support for `Axis`
* (2023.10.19) Fixed abnormal lead line when `Pie` set rose chart
* (2023.10.15) Fixed Animation exception when `Line` was set to `AlongPath` (#281)
* (2023.10.12) Fixed invalid value axis when `MarkLine` specified `yValue`
* (2023.10.11) Fixed invalid setting of `Serie` `showDataDimension`

## v3.8.1

* (2023.10.02) Release `v3.8.1` version
* (2023.09.29) Fixed issue where `Bar` is set to `Bottom` when horizontal does not take effect
* (2023.09.22) Added support for dashed lines in `Line`'s smooth curves
* (2023.09.16) Fix to `Tooltip` reporting an exception when there is no data in the category axis (#279)
* (2023.09.16) Fix to `Pie` drawing exception with no data (#278)
* (2023.09.12) Added `Pie` `radiusGradient` to set the gradient effect in the radius direction
* (2023.09.05) Improved the performance of LabelLine`s lineEndX in Pie
* (2023.09.05) Fixed `TriggerTooltip()` interface not working for `Ring`
* (2023.09.05) Fixed drawing error when `Radar` data is all zeros

## v3.8.0

Highlights:

* Refactoring `Animation` animation system, adding support for `New Animation` and `Interactive animation`
* Improved `PieChart` animation interactive representation
* Added four new markers for `Symbol` : `EmptyTriangle`, `EmptyDiamond`, `Plus` and `Minus`
* Improved `Chart` mouse interaction callback
* Added the function of `LabelLine` to fix the horizontal coordinate
* Added `GridLayout` grid layout component
* Added `Auto` type for `Tooltip`
* Optimizes and fixes several other issues

Log details:

* (2023.09.03) Release `v3.8.0`
* (2023.09.01) Added `Tooltip` Auto to automatically set display type and trigger type
* (2023.08.29) Added gridIndex support for `Ring` to set the specified grid
* (2023.08.29) Added gridIndex support for Radar to set the specified grid
* (2023.08.29) Added gridIndex support for `Pie` to set specified grids
* (2023.08.29) Added the GridLayout component for managing multiple GridCoord layouts
* (2023.08.25) Fixed display only one Label when there are multiple Marklines
* (2023.08.25) Fixed MarkLine drawing outside the coordinate system after opening Clip
* (2023.08.24) Optimizes `YAxis` to default 0-1 range when all data is 0
* (2023.08.23) Fixed an issue where `Label` of `YAxis` could duplicate
* (2023.08.22) Fixed `Bar` display hidden drawing performance exception
* (2023.08.22) Improved Zebra histogram rendering performance (#276)
* (2023.08.16) Added Daemon daemon to resolve an error after TMP is enabled locally
* (2023.08.15) Fixed `Data` displaying axes incorrectly when data is between -1 and 1 (#273) (b y@Ambitroc)
* (2023.08.14) Fixed `XCharts` updating error after` TextMeshPro `and` NewInputSystem `are enabled locally (#272)
* (2023.08.12) Fixed `Chart` error when deleted at runtime (#269)
* (2023.08.11) Fixed an issue where data could not be added when DataZoom was enabled
* (2023.08.11) Fixed `itemFormatter` not working when `SerieData` sets ItemStyle separately
* (2023.08.10) Improved BarChart`s performance when Tooltip`s Trigger is an Item
* (2023.08.09) Added `Axis` to support dynamic icon colors by setting `color` of `IconStyle` to `clear`
* (2023.08.08) Added support for `Pie` for `LabelLine`s` lineEndX `
* (2023.08.05) Clean up the code for `Examples` and remove unnecessary use cases
* (2023.08.04) Added support for `LabelLine`'s `lineEndX` to set the boot line to fixed X position
* (2023.08.04) Added support for Ring`s avoidLabelOverlap to avoid text stacking (#247)
* (2023.08.03) Improved Chart`s onSerieEnter, onSerieExit, and onSerieClick callbacks
* (2023.08.02) Fixed invalid `onSerieEnter` and `onSerieExit` callbacks for `BarChart`
* (2023.08.02) Added support for Symbol's `Plus` and `Minus` signs
* (2023.07.31) Added support for Symbol's `EmptyTriangle` and `EmptyDiamond`, improved `Symbol` performance
* (2023.07.31) Improved the default configuration effect of `Line`
* (2023.07.27) Add `Serie` to `minRadius` to set minimum radius
* (2023.07.26) Added `MLValue` multiple values
* (2023.07.25) Added `XLog` log system
* (2023.07.18) Improved the interactive animation effect of `Pie` pie chart
* (2023.07.14) Added support for `Animation` `Interaction` interactive animation configuration
* (2023.07.11) Added `Animation` `Addition` new animation configuration support
* (2023.07.11) Reconstructs `Animation` animation system to improve animation experience
* (2023.06.30) Added support for PolarCood`s indicatorLabelOffset setting indicating text offsets
* (2023.06.30) Fixed an issue where the background color of Axis `IndicatorLabel` could be abnormal
* (2023.06.30) Added support for Axis `IndicatorLabel` customizable `color`
* (2023.06.12) Fixed an issue where AxisLabel's formatterFunction had the wrong value on the value axis

## v3.7.0

Highlights:

* Added `HelpDoc` official website help document redirection
* Added support for `Clip` for `Line`
* Optimize the range Settings of `Axis`
* Other optimizations and fixes

Log details:

* (2022.06.08) Release v3.7.0
* (2023.06.04) Added `HelpDoc` help document skip
* (2023.05.30) Fixed Serie name with `_` line causing `Legend` to not fire (#252) (by @svr2kos2)
* (2023.05.10) Added `MinMaxAuto` range type for `Axis`
* (2023.05.10) Added support for `Clip` for `Line`
* (2023.05.04) Fixed `Axis` setting` CeilRate `not taking effect in range -1 to 1
* (2023.05.04) Optimizes MinMax type range calculations for Axis
* (2023.05.04) Fixed AxisLabel displaying `Label` formatting incorrectly when the data is all floating point numbers less than 1
* (2023.05.04) Fixed `Theme` being reset after modifying default theme parameters
* (2023.05.04) Added `Warning` when `Symbol` selects `Custom` type
* (2023.04.15) Fixed `DataZoom` may be abnormal in multiple charts (#252)
* (2023.04.14) Fixed `Tooltip` may be abnormal when there is only one data
* (2023.04.14) Added `BaseChart`s `TriggerTooltip()` interface to try to trigger `ToolTip`
* (2023.04.12) Optimizes` RadarCood `setting` startAngle `with text following the adjustment position
* (2023.04.12) Added `Radar` support for wildcard `{b}`
* (2023.04.11) Fixed an issue where Inspector could be abnormal when dynamically adding components

## v3.6.0

* (2023.04.01) Release `v3.6.0` version
* (2023.03.14) Fix for Tooltip's `titleFormater` setting `{b}` may not take effect
* (2023.03.14) Fix for `BarChart` not drawing bar background when data is 0 (#250) (by @Ambitroc)
* (2023.03.12) Added `LabelStyle` `autoRotate` to set automatic rotation of angled vertical text
* (2023.03.10) Added `VR` and other non-mouse input for Point location acquisition (#248) (by @Ambitroc)
* (2023.03.09) Adds callbacks to Chart's `onSerieClick`, `onSerieDown`, `onSerieEnter` and `onSerieExit`
* (2023.03.09) Fixed click-check offset for `Pie` not taking effect
* (2023.03.04) Added Positions for Legend to customize legend positions
* (2023.03.03) Fixed `Animation` changing animation that might not work
* (2023.02.28) Fixed issue with Serie's Label not refreshing when `Legend` is clicked
* (2023.02.26) Adds DataZoom's `startEndFunction` delegate
* (2023.02.12) Refactor the Component code and adjust the API
* (2023.02.10) Fix `Axis` with incorrect minimum in `Log` axis in some cases
* (2023.02.10) Optimizes the default display format of Axis's value Label
* (2023.02.08) Added startLock and endLock for DataZoom
* (2023.02.02) Fixed bug where datazoom xaxis label could be displayed off-chart when datazoom is turned on
* (2023.02.02) Optimizes the `ignore` setting of `SerieData` to ignore data
* (2023.02.01) Fix `XChartsMgr.ContainsChart()` interface exception
* (2023.01.31) Added support for `InputSystem` (#242) (by @Bian-Sh)
* (2023.01.11) Fixed chart not refreshing after removing Component from Inspector (#241)
* (2023.01.06) Fixed bug with `Pie` displaying abnormal Label when the last few values are 0 (#240)
* (2023.01.03) deletes serie `MarkColor` and adds ItemStyle `MarkColor`
* (2022.12.29) Added `+` to list editor
* (2022.12.29) Fixed `UpdateXYData()` interface affecting data accuracy (#238)
* (2022.12.28) Fixed abnormal display when setting `border` when `Pie` has only one data (#237)
* (2022.12.22) Adjust `Covert` rename to `Convert`, involving interfaces such as: `ConvertXYAxis()`, `CovertSerie()`, etc
* (2022.12.22) Fixed abnormal display of `Label` after `Convert XY Axis`
* (2022.12.12) Fixed an issue where the `Value` Axis of `axis` calculated the value range incorrectly in some cases
* (2022.12.12) Optimized legend's formatter to support `{h}` wildcards
* (2022.12.12) Fixed abnormal display of Legend's formatter when set to a fixed value
* (2022.12.08) Added `AreaStyle` `toTop` parameter to set whether the line graph gradient goes to the top or to the actual position
* (2022.12.07) Adds text wildcard `{h}` for `Formatter` to support setting current color values

## v3.5.0

Highlights:

* Updated documentation structure, added [Official XCharts Homepage](https://xcharts-team.github.io/en)
* Added support for the DataZoom box selected.
* Added support for maximum width Settings for bars.
* Other optimizations.

Upgrade Note:

* Due to the adjustment of the document directory structure, it is recommended to back up the files before upgrading and delete the original XCharts before upgrading them.

Log details:

* (2022.12.01) Release v3.5.0
* (2022.11.30) Increase `Serie` `barMaxWidth` can set the maximum width of `Bar`
* (2022.11.30) Optimize `Tooltip`s` Shadow `drawing to stay within chart scope
* (2022.11.29) Fix an issue with `Serie` data item index exception indicated by `Tooltip`
* (2022.11.27) Optimizes the offset Settings for `Axis` `AxisName`
* (2022.11.27) Optimize the Position of `Comment` by replacing position with `Location`
* (2022.11.27) Optimizes` Tooltip ` `LineStyle` to support setting `Shadow` when using color
* (2022.11.27) Adjust the Documentation structure
* (2022.11.26) Optimizes LabelLine's `symbol` not to be displayed by default
* (2022.11.26) Fixed `LineChart` adding unordered data display exception when `XY` is numeric axis
* (2022.11.26) Fixed an exception when selecting `DataZoom` from right to left
* (2022.11.20) Rename `UdpateXAxisIcon()` interface to `UpdateXAxisIcon()` (#235)
* (2022.11.12) Added `Pie` `LabelLine` support `Symbol`
* (2022.11.12) Added `DataZoom` `MarqueeStyle` support box selection area
* (2022.11.10) Optimized area color fill effect for `Radar` when type is` Single `
* (2022.11.04) Fixed exception after itemFormatter `Tooltip` set wildcard `{d}`

## v3.4.0

Highlights:

* Added `indicatorLabel` of `Axis` to set different indicator text styles separately
* Add `markColor` of `Serie` to set the logo color
* Add `startAngle` of `RadarCoord` to set the starting Angle of `Radar`
* Optimize the numerical interval representation of `Axis`
* Added `DataZoom` support for numeric axes
* Add `SmoothLimit` of `Line` to control different effects of smoothing curves

Details:

* (2022.11.01) Release `v3.4.0` version
* (2022.10.30) adds API: `AddData()`, `ClearSerieData()`, `ClearComponentData()`
* (2022.10.30) Added `Axis`'s `indicatorLabel`, removed `Tooltip`'s `indicatorLabelStyle` (#226)
* (2022.10.29) Add `Serie` `markColor` to set logo colors for display of `Legend` and `Tooltip` (#229)
* (2022.10.26) increase the startAngle of RadarCoord to set the startAngle of Radar
* (2022.10.21) Fixed `Chart` not displaying `Label` properly when controlled by `Layout` (#231)
* (2022.10.21) fixed compatibility issues on Unity2019.2
* (2022.10.18) Optimize the numerical performance of `Axis`
* (2022.10.15) Fixed an issue where `Axis` `Label` might not appear properly when `DataZoom` is enabled (#227)
* (2022.10.14) Added `DataZoom` support for numeric axes
* (2022.10.13) Fixed the `Pie` circle with abnormal border Settings (#225)
* (2022.10.13) Fixed the `Download` interface causing the `iOS` platform packaging failure
* (2022.10.12) Added support for `Animation` `UnscaledTime` to set whether the animation is affected by TimeScale (#223)
* (2022.10.10) Optimizes the `Documentation~` format
* (2022.10.10) Add `Line` `SmoothLimit` to control different effects of smoothing curves
* (2022.10.05) Fixed an issue where `Serie` hid information when `Tooltip` was also displayed
* (2022.09.30) Fixed `DivideByZeroException` when Chart `is very small (#230)

## v3.3.0

Highlights:

* Optimized chart details to support more functions
* Add lots of Demo examples
* Improved documentation and fixed several issues
* Added PolarChart support for Bar and Heatmap
* Added a HeatmapChart type
* Improved Tooltip display

Details:

* (2022.09.26) Optimizes the default number of segments for `Axis` at the category Axis
* (2022.09.25) Fixed the problem that some interfaces in the `API` document were not exported
* (2022.09.24) optimize `FunnelChart`
* (2022.09.23) Optimizes `ParallelChart`
* (2022.09.22) Added `SaveAsImage()` interface to save charts to images
* (2022.09.21) Fixed an issue where the `InsertSerie()` interface did not refresh the graph
* (2022.09.21) Optimized `PolarChart` for `Line` thermal map support
* (2022.09.20) Added `PolarChart` support for `Heatmap`
* (2022.09.19) Added `PolarChart` support for multi-bar graphs and stacked bar graphs
* (2022.09.16) Added `PolarChart` support for `Bar` histogram
* (2022.09.14) Added support for `PolarCoord` to set ring polar coordinates via `Radius`
* (2022.09.09) Fixed an issue where some components of edit parameters in `Editor` might not refresh in real time
* (2022.09.08) Added support for `RingChart` settable `LabelLine` bootline
* (2022.09.06) Added support for `SerieSymbol` `minSize` and `maxSize` parameters to set maximum and minimum sizes
* (2022.09.06) Added support for `showStartLine` and `showEndLine` parameters for `AxisSplitLine` to set whether to display the first splitter
* (2022.09.06) Added `Heatmap` support for different patterns via `symbol`
* (2022.09.05) Added `Heatmap` `heatmapType` support for setting `Data` and `Count` two different mapping methods of Heatmap
* (2022.09.05) Optimizes `Tooltip` when indicating numerical axis in thermograph
* (2022.09.02) Added `onPointerEnterPie` callback support
* (2022.09.02) Optimize the HeatmapChart `
* (2022.08.30) optimizes` RadarChart `
* (2022.08.30) Fixed `DataZoom` calculation range inaccuracies in some cases (#221)
* (2022.08.29) optimizes the default behavior of `BarChart` when data is too dense
* (2022.08.29) optimizes `YAxis` Max/min calculations when `DataZoom` is enabled
* (2022.08.29) optimized `CandlestickChart` massive data rendering
* (2022.08.28) fixed an issue where `LineChart` does not appear properly in the case of stacking and custom Y-axis range
* (2022.08.26) Added `Legend` new icon type `Candlestick`
* (2022.08.26) optimizes` CandlestickChart `performance and adjusts related` AddData() `interface parameters
* (2022.08.26) Added support for setting different display positions in Tooltip's `position` parameter
* (2022.08.26) Delete the `fixedXEnable` and `fixedYEnable` arguments of Tooltip
* (2022.08.25) EmphasisStyle `EmphasisStyle` has emphasised the support for `label`
* (2022.08.25) Added support for `formatter` for `{d3}` specified percentage of dimension data
* (2022.08.24) fixed the `label` of the `ScatterChart` not refreshing
* (2022.08.24) fixed abnormal display of `label` of `MarkLine` in some cases

## v3.2.0

Highlights:

* `Serie` supports highlighting, EmphasisStyle, EmphasisStyle, BlurStyle, and SelectStyle
* `Axis` supports sub-scale and sub-partition of coordinate axes:`MinorTick` and `MinorSplitLine`
* `Serie` supports different color selection strategies: `colorBy`
* `Radar` supports smooth curves: `smooth`
* `Line` supports filling as a convex polygon: `AreaStyle` `innerFill`
* `DataZoom` supports timeline
* Other optimizations and issue fixes

Details:

* (2022.08.16) Release `v3.2.0` version
* (2022.08.15) optimized `Smooth` Bezier curve algorithm
* (2022.08.13) Fixed an issue where the `DataZoom` component might not display correctly when opened
* (2022.08.11) Optimized Tooltip supports `ignoreDataDefaultContent`
* (2022.08.10) fixed abnormal display of some components of `Chart` under 3D camera
* (2022.08.10) Fix `RemoveSerie()` interface not working (#219)
* (2022.08.10) Optimized font synchronization for Theme
* (2022.08.10) optimizes the default `layer` of Chart to `UI`
* (2022.08.09) optimizes the `Time` timeline of `Axis`
* (2022.08.09) Added AreaStyle `innerFill` parameter to support filling convex polygons
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
* (2022.07.19) Added `Axis` to` MinorSplitLine `to set the Axis degree divider
* (2022.07.19) Added `Axis` `MinorTick` to set the Axis sub-scale
* (2022.07.17) Add the `smooth` parameter for Radar to set the smooth curve
* (2022.07.15) Added DataZoom support for the `Time` timeline

## v3.1.0

* (2022.07.12) Release `v3.1.0` version
* (2022.07.12) Fixed `Serie` `ignoreLineBreak` not working
* (2022.07.07) Optimized `Axis` `minMaxType` to support precision to decimals when specified as `MinMax`
* (2022.07.05) Fixed drawing exception when there are multiple coordinate systems in `Chart` (#210)
* (2022.07.04) Added the axisMaxSplitNumber parameter of `Settings` to set the maximum number of partitions for `Axis`
* (2022.07.04) Fixed Axis` Tick `drawing position after setting `offset`(#209)
* (2022.07.03) Optimize the `AxisLabel` formatterFunction custom delegate
* (2022.07.03) Added the `onZero` parameter of `AxisName` to support setting the coordinate AxisName and position to match the Y-axis 0 scale (#207)
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

## v3.0.0-preview9

## v3.0.0-preview8

## v3.0.0-preview7

## v3.0.0-preview6

## v3.0.0-preview5

## v3.0.0-preview4

## v3.0.0-preview3

## v3.0.0-preview2

## v3.0.0-preview1

## v2.8.2

* (2022.08.15) Release `v2.8.2` version
* (2022.08.15) Added support for the `HeatmapChart` formatter for custom Tooltip
* (2022.07.13) Fixed `SerieLabel` refresh exception #215
* (2022.06.30) Optimize `Radar` so that the `Tooltip` layer is above `Indicator`

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
* (2022.02.08) Fixed `{d}` formatter error when value is 0
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

Highlights:

* LineChart support the line of ignore data is disconnected or connected
* LineChart support animation at a constant speed
* Other optimizations and bug fixes

Details:

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

Highlights:

* Data store upgraded from `float` to `double`
* Added `MarkLine`
* `Serie` can use `IconStyle` to configure ICONS uniformly
* `Label` supports custom display styles with code
* `DataZoom` is perfect
* `PieChart` optimization
* Problem fixes

Upgrade Note:

* Since the data type is upgraded to `double`, the implicit conversion of `float` to `double` may have precision problems, so it is recommended that all previous data types of `float` be manually changed to `double`.

Details:

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

## v2.2.3

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
* (2020.08.23) Refactor code, replace `Color` with `Color32` for reduce implicit conversion (Can cause custom colors to lose, reference [FAQ 29](https://github.com/XCharts-Team/XCharts/blob/master/Assets/XCharts/Documentation~/fqa.md) to upgrade)
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
* (2020.04.18) 增加`Convert XY Axis`互换XY轴配置
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
