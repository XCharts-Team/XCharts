# XCharts FAQ

[FAQ 1: How to adjust the margin between the axis and the background?](#how-to-adjust-the-margin-between-the-axis-and-the-background)  
[FAQ 2: How to play agian the fadeIn animation?](#how-to-play-agian-the-fadein-animation)  
[FAQ 3: How to customize the color of data item in line chart and pie chart?](#how-to-customize-the-color-of-data-item-in-line-chart-and-pie-chart)  
[FAQ 4: How to formatter the text of axis label, such as add a units text?](#how-to-formatter-the-text-of-axis-label-such-as-add-a-units-text)  
[FAQ 5: How to stack the bar of bar chart](#how-to-stack-the-bar-of-bar-chart)  
[FAQ 6: How to make the bar serie in the same bar but not stack?](#how-to-make-the-bar-serie-in-the-same-bar-but-not-stack)  
[FAQ 7: How to adjust the bar width and gap of barchart?](#how-to-adjust-the-bar-width-and-gap-of-barchart)  
[FAQ 8: How to adjust the color of bar?](#how-to-adjust-the-color-of-bar)  
[FAQ 9: Can I adjust the anchor of chart?](#can-i-adjust-the-anchor-of-chart)  
[FAQ 10: Can display more than 1000 data?](#can-display-more-than-1000-data)  
[FAQ 11: Can line chart drawing be dash, dot and dash-dot?](#can-line-chart-drawing-be-dash-dot-and-dash-dot)  
[FAQ 12: How to limit the value range of the Y-axis?](#how-to-limit-the-value-range-of-the-y-axis)  
[FAQ 13: How to customize the tick value range of value axis?](#how-to-customize-the-tick-value-range-of-value-axis)  
[FAQ 14: How to display text at the top of data items?](#how-to-display-text-at-the-top-of-data-items)  
[FAQ 15: How do I customize icons for data items?](#how-do-i-customize-icons-for-data-items)  
[FAQ 16: How to anti-aliasing and make the chart smoother?](#how-to-anti-aliasing-and-make-the-chart-smoother)  
[FAQ 17: Why does mouse over chart Tooltip not show?](#why-does-mouse-over-chart-tooltip-not-show)  
[FAQ 18: How not to display the bar line of Tooltip?](#how-not-to-display-the-bar-line-of-tooltip)  
[FAQ 19: How do I customize the display of Tooltip?](#how-do-i-customize-the-display-of-tooltip)  
[FAQ 20: How do I get the Y-axis to display multiple decimal places?](#how-do-i-get-the-y-axis-to-display-multiple-decimal-places)  
[FAQ 21: How do I dynamically update data with code?](#how-do-i-dynamically-update-data-with-code)  
[FAQ 22: How to display legend? Why are legends sometimes not displayed?](#how-to-display-legend-why-are-legends-sometimes-not-displayed)  
[FAQ 23: How to make chart as prefab?](#how-to-make-chart-as-prefab)  
[FAQ 24: How do I draw custom graphic in chart,such as line or dot?](#how-do-i-draw-custom-content-in-chart-such-as-line-or-dot)  
[FAQ 25: How to achieve similar data movement effect of ELECTRO cardiogram?](#how-to-achieve-similar-data-movement-effect-of-electro-cardiogram)  
[FAQ 26: How do I use the background component? What are the conditions?](#how-do-i-use-the-background-component-what-are-the-conditions)  
[FAQ 27: Mesh can not have more than 65000 vertices?](#mesh-cannot-have-more-than-65000-vertices)  
[FAQ 28: Why are the parameters set in Serie reset after they run?](#why-are-the-parameters-set-in-serie-reset-after-they-run)  
[FAQ 29: How to change the color of serie symbol?](#how-to-change-the-color-of-serie-symbol)  
[FAQ 30: How to deal with TMP errors when importing or updating XCharts?](#what-if-tmp-errors-occur-when-importing-or-updating-xcharts)  
[FAQ 31: Support empty data? How to achieve the effect of line chart disconnection?](#support-empty-data-how-to-achieve-the-effect-of-line-chart-disconnection)  
[FAQ 32: 2.x What are the common problems when upgrading version 3.x?](#what-are-the-common-problems-when-upgrading-xcharts2-to-xcharts3)  

## how-to-adjust-the-margin-between-the-axis-and-the-background

`Grid` conponent，which can adjust the left, right, up, down margins of chart.

## how-to-play-agian-the-fadein-animation

call the `chart.AnimationReset()` API.

## how-to-customize-the-color-of-data-item-in-line chart-and-pie-chart

`Theme`->`colorPalette`, or the sub component `LineStyle` and `ItemStyle` of `Serie`.

## how-to-formatter-the-text-of-axis-label-such-as-add-a-units-text

Adjust `formatter` and `numericFormatter` parameter of `Legend`, `AxisLabel`, `Tooltop`, `SerieLabel`.

## how-to-stack-the-bar-of-bar-chart

Set the `stack` parameter of `Serie`, the series will stack in a bar with the same `stack`.

## how-to-make-the-bar-serie-in-the-same-bar-but-not-stack

Set the `barGap` of `Serie` to `-1`，`stack` to null.

## how-to-adjust-the-bar-width-and-gap-of-barchart

Adjust the `barWidth` and `barGap` parameter of `Serie`, the last `serie`'s `barWidth` and `barGap` are valid when multiple `serie`.

## how-to-adjust-the-color-of-bar

Adjust the `ItemStyle` of `Data` in `inspector`.

## can-i-adjust-the-anchor-of-chart

Yes, you can set any one of 16 anchors but the value use default.

## can-display-more-than-1000-data

Yes. But `UGUI` limits `65000` vertices to a single `Graphic`, so too much data may not be displayed completely. The sampling simplification curve can be turned on by setting the sampling distance `sampleDist`. You can also set some parameters to reduce the number of vertices in the chart to help show more data. Such as reducing the size of the chart, close or reduce the axis of the client drawing, close `Symbol` and `Label` display. A `Normal` line chart occupies fewer vertices than a `Smooth` line chart. The `1.5.0` and above versions can set `large` and `largeThreshold` parameters to enable performance mode.

## can-line-chart-drawing-be-dash-dot-and-dash-dot

Yes. Adjust the `lineType` of `Serie`.

## how-to-limit-the-value-range-of-the-y-axis

Select the `minMaxType` of `Axis` as `Custom`, then set `min` and `max` to the values you want.

## how-to-customize-the-tick-value-range-of-value-axis

By default, it is automatically split by the `splitNumber` of `Axis`. Also, you can customize the `interval` to the range you want.

## how-to-display-text-at-the-top-of-data-items

Adjust the `Label` of `Serie`.

## how-do-i-customize-icons-for-data-items

Set the `Icon` of `Data` in `Serie`.

## how-to-anti-aliasing-and-make-the-chart-smoother

Open the `Anti-Aliasing` setting in `Unity`. Selected the UI Canvas `Render Mode` as `Screen Space-Camera`, selected `MSAA`, set `4` times or higher anti-aliasing. The sawtooth can only be reduced and unavoidable. The higher the pixel, the less obvious the sawtooth is.

## why-does-mouse-over-chart-tooltip-not-show

Verify `Toolip` is opened. Verify that the parent node of chart has turned off mouse events.

## how-not-to-display-the-bar-line-of-tooltip

Set the `type` of `Tooltup` as `None`. Or adjust the parameters of `lineStyle`.

## how-do-i-customize-the-display-of-tooltip

See the `formatter`, `itemFormatter`, `titleFormatter` parameters of `Tooltip`.

## how-do-i-get-the-y-axis-to-display-multiple-decimal-places

Set the `numericFormatter` parameter of `AxisLabel`.

## how-do-i-dynamically-update-data-with-code

See example: `Example01_UpdateData.cs`

## how-to-display-legend-why-are-legends-sometimes-not-displayed

First, the `name` in `Serie` must have a value that is not null. Then set `Legend` is `show`, where `data` can be empty by default, indicating that all legends are displayed. If you only want to display part of the `Serie` legend, fill in `data` with the `name` of the legend you want to display. If none of the values in `data` are `name` of the series, the legend will not be displayed.

## how-to-make-chart-as-prefab

Before make prefab, please delete all sub gameObject under chart which auto-created by `XCharts`.

## how-do-i-draw-custom-content-in-chart-such-as-line-or-dot

Implement `onCustomDraw` of chart, see `Example12_CustomDrawing.cs`.

## how-to-achieve-similar-data-movement-effect-of-electro-cardiogram

See `Example_Dynamic.cs`.

## how-do-i-use-the-background-component-what-are-the-conditions

Setting `show` to `true` for the `background` component.

## mesh-cannot-have-more-than-65000-vertices

This is the limit of `UGUI` on the number of vertices for a single `Graphic`. `XCharts` is draw chart on a single `Graphic`, so there is also this limitation. The solution can be referred to: [FAQ 10: Can display more than 1000 data](#can-display-more-than-1000-data)  

## why-are-the-parameters-set-in-serie-reset-after-they-run

Check whether `RemoveData()` and add new `Serie` in the code. If you want to keep the configuration of `Serie`, you can only `ClearData()` which just clear data and then readd the data to the old serie.

## how-to-change-the-color-of-serie-symbol

The color of 'Symbol' is the color of 'ItemStyle' used.

## what-if-tmp-errors-occur-when-importing-or-updating-xcharts

XCharts does not enable TMP by default, so there are no references to TMP on asmdef. This issue may occur when updating XCharts after TMP is enabled locally. It can be solved in the following two ways:

1. Find `XCharts.Runtime.asmdef` and `XCharts.Editor.asmdef` and manually add references to `TextMeshPro`
2. Remove the `dUI_TextMeshPro` macro for Scripting Define Symbols in PlayerSetting

Version ` 3.8.0 ` after adding Daemon [XCharts - Daemon](https://github.com/XCharts-Team/XCharts-Daemon), will be XCharts - Daemon import project, When updating XCharts, the daemon automatically refreshes the asmdef based on the locally enabled TMP to ensure proper compilation.

## support-empty-data-how-to-achieve-the-effect-of-line-chart-disconnection

`data` of `Serie` is of type `double`, so it cannot represent empty data. Empty data can be achieved by turning on Serie's ignore and specifying ignoreValue. You can also set the ignore parameter for each SerieData. The ignoreLineBreak parameter can be set to disconnect or connect after ignoring data.

## what-are-the-common-problems-when-upgrading-xcharts2-to-xcharts3

1. `XCharts.Runtime.XChartsMgr` is missing the class attribute `ExtensionOfNativeClass`!
3.x version does not need to mount XChartsMgr, directly delete the `_xcharts_` node on the scene.
