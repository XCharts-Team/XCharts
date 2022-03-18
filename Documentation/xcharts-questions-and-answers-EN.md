# XCharts Q&A

[XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts API](xcharts-api-EN.md)  
[XCharts Configuration](xcharts-configuration-EN.md)

[QA 1: How to adjust the margin between the axis and the background?](#How-to-adjust-the-margin-between-the-axis-and-the=-background)  
[QA 2: How to play agian the fadeIn animation?](#How-to-play-agian-the-fadeIn-animation)  
[QA 3: How to customize the color of data item in line chart and pie chart?](#How-to-customize-the-color-of-data-item-in-line-chart-and-pie-chart)  
[QA 4: How to formatter the text of axis label, such as add a units text?](#How-to-formatter-the-text-of-axis-label-such-as-add-a-units-text)  
[QA 5: How to stack the bar of bar chart](#How-to-stack-the-bar-of-bar-chart)  
[QA 6: How to make the bar serie in the same bar but not stack?](#How-to-make-the-bar-serie-in-the-same-bar-but-not-stack)  
[QA 7: How to adjust the bar width and gap of barchart?](#How-to-adjust-the-bar-width-and-gap-of-barchart)  
[QA 8: How to adjust the color of bar?](#How-to-adjust-the-color-of-bar)  
[QA 9: Can I adjust the anchor of chart?](#Can-I-adjust-the-anchor-of-chart)  
[QA 10: Can display more than 1000 data?](#Can-display-more-than-1000-data)  
[QA 11: Can line chart drawing be dash, dot and dash-dot?](#Can-line-chart-drawing-be-dash-dot-and-dash-dot)  
[QA 12: How to limit the value range of the Y-axis?](#How-to-limit-the-value-range-of-the-Y-axis)  
[QA 13: How to customize the tick value range of value axis?](#How-to-customize-the-tick-value-range-of-value-axis)  
[QA 14: How to display text at the top of data items?](#How-to-display-text-at-the-top-of-data-items)  
[QA 15: How do I customize icons for data items?](#How-do-I-customize-icons-for-data-items)  
[QA 16: How to anti-aliasing and make the chart smoother?](#How-to-anti-aliasing-and-make-the-chart-smoother)  
[QA 17: Why does mouse over chart Tooltip not show?](#Why-does-mouse-over-chart-Tooltip-not-show)  
[QA 18: How not to display the bar line of Tooltip?](#How-not-to-display-the-bar-line-of-Tooltip)  
[QA 19: How do I customize the display of Tooltip?](#How-do-I-customize-the-display-of-Tooltip)  
[QA 20: How do I get the Y-axis to display multiple decimal places?](#How-do-I-get-the-Y-axis-to-display-multiple-decimal-places)  
[QA 21: How do I dynamically update data with code?](#How-do-I-dynamically-update-data-with-code)  
[QA 22: How to display legend? Why are legends sometimes not displayed?](#How-to-display-legend?Why-are-legends-sometimes-not-displayed)  
[QA 23: How to make chart as prefab?](#How-to-make-chart-as-prefab)  
[QA 24: How do I draw custom graphic in chart,such as line or dot?](#How-do-I-draw-custom-content-in-chart-such-as-line-or-dot)  
[QA 25: How to achieve similar data movement effect of ELECTRO cardiogram?](#How-to-achieve-similar-data-movement-effect-of-ELECTRO-cardiogram)  
[QA 26: How do I use the background component? What are the conditions?](#How-do-I-use-the-background-component-What-are-the-conditions)  
[QA 27: Mesh can not have more than 65000 vertices?](#Mesh-cannot-have-more-than-65000-vertices)  
[QA 28: Why are the parameters set in Serie reset after they run?](#Why-are-the-parameters-set-in-Serie-reset-after-they-run)  
[QA 29: Why are many custom colors lost after upgrading to 1.6.0? How should I upgrade?](#Why_are_many_custom_colors_lost_after_upgrading_to_1_6_0_How_should_I_upgrade)  

## How-to-adjust-the-margin-between-the-axis-and-the=-background

A: `Grid` conponent，which can adjust the left, right, up, down margins of chart.

## How-to-play-agian-the-fadeIn-animation

A: call the `chart.AnimationReset()` API.

## How-to-customize-the-color-of-data-item-in-line chart-and-pie-chart

A: `Theme`->`colorPalette`, or the sub component `LineStyle` and `ItemStyle` of `Serie`.

## How-to-formatter-the-text-of-axis-label-such-as-add-a-units-text

A: Adjust `formatter` and `numericFormatter` parameter of `Legend`, `AxisLabel`, `Tooltop`, `SerieLabel`.

## How-to-stack-the-bar-of-bar-chart

A: Set the `stack` parameter of `Serie`, the series will stack in a bar with the same `stack`.

## How-to-make-the-bar-serie-in-the-same-bar-but-not-stack

A: Set the `barGap` of `Serie` to `-1`，`stack` to null.

## How-to-adjust-the-bar-width-and-gap-of-barchart

A: Adjust the `barWidth` and `barGap` parameter of `Serie`, the last `serie`'s `barWidth` and `barGap` are valid when multiple `serie`.

## How-to-adjust-the-color-of-bar

A: Adjust the `ItemStyle` of `Data` in `inspector`.

## Can-I-adjust-the-anchor-of-chart

A: Yes, you can set any one of 16 anchors but the value use default.

## Can-display-more-than-1000-data

A: Yes. But `UGUI` limits `65000` vertices to a single `Graphic`, so too much data may not be displayed completely. The sampling simplification curve can be turned on by setting the sampling distance `sampleDist`. You can also set some parameters to reduce the number of vertices in the chart to help show more data. Such as reducing the size of the chart, close or reduce the axis of the client drawing, close `Symbol` and `Label` display. A `Normal` line chart occupies fewer vertices than a `Smooth` line chart. The `1.5.0` and above versions can set `large` and `largeThreshold` parameters to enable performance mode.

## Can-line-chart-drawing-be-dash-dot-and-dash-dot

A: Yes. Adjust the `lineType` of `Serie`.

## How-to-limit-the-value-range-of-the-Y-axis

A: Select the `minMaxType` of `Axis` as `Custom`, then set `min` and `max` to the values you want.

## How-to-customize-the-tick-value-range-of-value-axis

A: By default, it is automatically split by the `splitNumber` of `Axis`. Also, you can customize the `interval` to the range you want.

## How-to-display-text-at-the-top-of-data-items

A: Adjust the `Label` of `Serie`.

## How-do-I-customize-icons-for-data-items

A: Set the `Icon` of `Data` in `Serie`.

## How-to-anti-aliasing-and-make-the-chart-smoother

A: Open the `Anti-Aliasing` setting in `Unity`. Selected the UI Canvas `Render Mode` as `Screen Space-Camera`, selected `MSAA`, set `4` times or higher anti-aliasing. The sawtooth can only be reduced and unavoidable. The higher the pixel, the less obvious the sawtooth is.

## Why-does-mouse-over-chart-Tooltip-not-show

A: Verify `Toolip` is opened. Verify that the parent node of chart has turned off mouse events.

## How-not-to-display-the-bar-line-of-Tooltip

A: Set the `type` of `Tooltup` as `None`. Or adjust the parameters of `lineStyle`.

## How-do-I-customize-the-display-of-Tooltip

A: See the `formatter`, `itemFormatter`, `titleFormatter` parameters of `Tooltip`.

## How-do-I-get-the-Y-axis-to-display-multiple-decimal-places

A: Set the `numericFormatter` parameter of `AxisLabel`.

## How-do-I-dynamically-update-data-with-code

A: See example: `Example01_UpdateData.cs`

## How-to-display-legend?Why-are-legends-sometimes-not-displayed

A: First, the `name` in `Serie` must have a value that is not null. Then set `Legend` is `show`, where `data` can be empty by default, indicating that all legends are displayed. If you only want to display part of the `Serie` legend, fill in `data` with the `name` of the legend you want to display. If none of the values in `data` are `name` of the series, the legend will not be displayed.

## How-to-make-chart-as-prefab

A: Before make prefab, please delete all sub gameObject under chart which auto-created by `XCharts`.

## How-do-I-draw-custom-content-in-chart-such-as-line-or-dot

A: Implement `onCustomDraw` of chart, see `Example12_CustomDrawing.cs`.

## How-to-achieve-similar-data-movement-effect-of-ELECTRO-cardiogram

A: See `Example_Dynamic.cs`.

## How-do-I-use-the-background-component-What-are-the-conditions

A: Setting `show` to `true` for the `background` component.

## Mesh-cannot-have-more-than-65000-vertices

A: This is the limit of `UGUI` on the number of vertices for a single `Graphic`. `XCharts` is draw chart on a single `Graphic`, so there is also this limitation. The solution can be referred to: [QA 10: Can display more than 1000 data](#Can-display-more-than-1000-data)  

## Why-are-the-parameters-set-in-Serie-reset-after-they-run

A: Check whether `RemoveData()` and add new `Serie` in the code. If you want to keep the configuration of `Serie`, you can only `ClearData()` which just clear data and then readd the data to the old serie.

## Why_are_many_custom_colors_lost_after_upgrading_to_1_6_0_How_should_I_upgrade

A: In version `1.6.0`, in order to reduce implicit conversion, all drawing related `Color` was changed to `Color32`, so some custom colors were lost. The main components affected are: `ItemStyle`, `LineStyle`, `AreaStyle`, `Vessel`, `VisualMap`, `AxisSplitArea`, `AxisSplitLine`, `GaugeAxis`,`SerieLabel`, etc. Can use the script [UpgradeChartColor.cs](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Editor/Tools/UpgradeChartColor.cs) to upgrade.
The upgrade steps are as follows:
1. Back up the project.
2. Download or copy the script [UpgradeChartColor.cs](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Editor/Tools/UpgradeChartColor.cs) in the old project `Editor`, Change the `color` field inside to `color.clear` (because some fields may not exist in the old version).
3. After compilation, the old version of color configuration file is exported through `menu bar -> XCharts-> ExportColorConfig` (the configuration file is saved by default to `color.config` under `Assets`).
4. Upgrade `XCharts` to the latest version.
5. The custom color can be restored by importing `color.config` through `menu bar -> XCharts-> ImportColorConfig` (if `color.config` is not under `Assets` of the upgraded project, copy it to this directory).

[XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts API](xcharts-api-EN.md)  
[XCharts Configuration](xcharts-configuration-EN.md)