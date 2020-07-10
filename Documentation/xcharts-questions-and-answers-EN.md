# XCharts Q&A

[XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts API](xcharts-api-EN.md)  
[XCharts Configuration](xcharts-configuration-EN.md)

[QA 1: How to adjust the margin between the axis and the background？](#Q：How-to-adjust-the-margin-between-the-axis-and-the-background)  
[QA 2: How to play agian the fadeIn animation](#Q:How-to-play-agian-the-fadeIn-animation)  
[QA 3: How to customize the color of data item in line chart and pie chart?](#Q:How-to-customize-the-color-of-data-item-in-line-chart-and-pie-chart)  
[QA 4: How to formatter the text of axis label, such as add a units text](#Q:How-to-formatter-the-text-of-axis-label-such-as-add-a-units-text)  
[QA 5: How to stack the bar of bar chart](#Q:How-to-stack-the-bar-of-bar-chart)  
[QA 6: How to make the bar serie in the same bar but not stack](#Q:How-to-make-the-bar-serie-in-the-same-bar-but-not-stack)  
[QA 7: How to adjust the bar width and gap of barchart](#Q:How-to-adjust-the-bar-width-and-gap-of-barchart)  
[QA 8: How to adjust the color of bar](#Q:How-to-adjust-the-color-of-bar)  
[QA 9: Can I adjust the anchor of chart](#Q:Can-I-adjust-the-anchor-of-chart)  
[QA 10: Can display more than 1000 data](#Q:Can-display-more-than-1000-data)  
[QA 11: Can line chart drawing be dash, dot and dash-dot](#Q:Can-line-chart-drawing-be-dash-dot-and-dash-dot)  
[QA 12: How to limit the value range of the Y-axis](#Q:How-to-limit-the-value-range-of-the-Y-axis)  
[QA 13: How to customize the tick value range of value axis](#Q:How-to-customize-the-tick-value-range-of-value-axis)  
[QA 14: How to display text at the top of data items](#Q:How-to-display-text-at-the-top-of-data-items)  
[QA 15: How do I customize icons for data items](#Q:How-do-I-customize-icons-for-data-items)  
[QA 16: How to anti-aliasing and make the chart smoother](#Q:How-to-anti-aliasing-and-make-the-chart-smoother)  
[QA 17: Why does mouse over chart Tooltip not show](#Q:Why-does-mouse-over-chart-Tooltip-not-show)  
[QA 18: How not to display the bar line of Tooltip](#Q:How-not-to-display-the-bar-line-of-Tooltip)  
[QA 19: How do I customize the display of Tooltip](#Q:How-do-I-customize-the-display-of-Tooltip)  
[QA 20: How do I get the Y-axis to display multiple decimal places?](#Q:How-do-I-get-the-Y-axis-to-display-multiple-decimal-places)  
[QA 21: How do I dynamically update data with code](#Q:How-do-I-dynamically-update-data-with-code)  
[QA 22: How to display legend? Why are legends sometimes not displayed?](#Q:How-to-display-legend?Why-are-legends-sometimes-not-displayed)  
[QA 23: How to make chart as prefab](#Q:How-to-make-chart-as-prefab)  
[QA 24: How do I draw custom graphic in chart,such as line or dot](#Q:How-do-I-draw-custom-content-in-chart-such-as-line-or-dot)  
[QA 25: How to achieve similar data movement effect of ELECTRO cardiogram](#Q:How-to-achieve-similar-data-movement-effect-of-ELECTRO-cardiogram)  
[QA 26: How do I use the background component? What are the conditions](#Q:How-do-I-use-the-background-component-What-are-the-conditions)  
[QA 27: Mesh can not have more than 65000 vertices?](#Q:Mesh-cannot-have-more-than-65000-vertices)  
[QA 28: Why are the parameters set in Serie reset after they run?](#Q:Why-are-the-parameters-set-in-Serie-reset-after-they-run)  

## Q:How-to-adjust-the-margin-between-the-axis-and-the-background

A: `Grid` conponent，which can adjust the left, right, up, down margins of chart.

## Q:How-to-play-agian-the-fadeIn-animation

A: call the `chart.AnimationReset()` API.

## Q:How-to-customize-the-color-of-data-item-in-line chart-and-pie-chart

A: `Theme`->`colorPalette`, or the sub component `LineStyle` and `ItemStyle` of `Serie`.

## Q:How-to-formatter-the-text-of-axis-label-such-as-add-a-units-text

A: Adjust `formatter` and `numericFormatter` parameter of `Legend`, `AxisLabel`, `Tooltop`, `SerieLabel`.

## Q:How-to-stack-the-bar-of-bar-chart

A: Set the `stack` parameter of `Serie`, the series will stack in a bar with the same `stack`.

## Q:How-to-make-the-bar-serie-in-the-same-bar-but-not-stack

A: Set the `barGap` of `Serie` to `-1`，`stack` to null.

## Q:How-to-adjust-the-bar-width-and-gap-of-barchart

A: Adjust the `barWidth` and `barGap` parameter of `Serie`, the last `serie`'s `barWidth` and `barGap` are valid when multiple `serie`.

## Q:How-to-adjust-the-color-of-bar

A: Adjust the `ItemStyle` of `Data` in `inspector`.

## Q:Can-I-adjust-the-anchor-of-chart

A: Yes, you can set any one of 16 anchors but the value use default.

## Can-display-more-than-1000-data

A: Yes. But `UGUI` limits `65000` vertices to a single `Graphic`, so too much data may not be displayed completely. The sampling simplification curve can be turned on by setting the sampling distance `sampleDist`. You can also set some parameters to reduce the number of vertices in the chart to help show more data. Such as reducing the size of the chart, close or reduce the axis of the client drawing, close `Symbol` and `Label` display. A `Normal` line chart occupies fewer vertices than a `Smooth` line chart. The `1.5.0` and above versions can set `large` and `largeThreshold` parameters to enable performance mode.

## Q:Can-line-chart-drawing-be-dash-dot-and-dash-dot

A: Yes. Adjust the `lineType` of `Serie`.

## Q:How-to-limit-the-value-range-of-the-Y-axis

A: Select the `minMaxType` of `Axis` as `Custom`, then set `min` and `max` to the values you want.

## Q:How-to-customize-the-tick-value-range-of-value-axis

A: By default, it is automatically split by the `splitNumber` of `Axis`. Also, you can customize the `interval` to the range you want.

## Q:How-to-display-text-at-the-top-of-data-items

A: Adjust the `Label` of `Serie`.

## Q:How-do-I-customize-icons-for-data-items

A: Set the `Icon` of `Data` in `Serie`.

## Q:How-to-anti-aliasing-and-make-the-chart-smoother

A: Open the `Anti-Aliasing` setting in `Unity`. Selected the UI Canvas `Render Mode` as `Screen Space-Camera`, selected `MSAA`, set `4` times or higher anti-aliasing. The sawtooth can only be reduced and unavoidable. The higher the pixel, the less obvious the sawtooth is.

## Q:Why-does-mouse-over-chart-Tooltip-not-show

A: Verify `Toolip` is opened. Verify that the parent node of chart has turned off mouse events.

## Q:How-not-to-display-the-bar-line-of-Tooltip

A: Set the `type` of `Tooltup` as `None`. Or adjust the parameters of `lineStyle`.

## Q:How-do-I-customize-the-display-of-Tooltip

A: See the `formatter`, `itemFormatter`, `titleFormatter` parameters of `Tooltip`.

## Q:How-do-I-get-the-Y-axis-to-display-multiple-decimal-places

A: Set the `numericFormatter` parameter of `AxisLabel`.

## Q:How-do-I-dynamically-update-data-with-code

A: See example: `Example01_UpdateData.cs`

## Q:How-to-display-legend?Why-are-legends-sometimes-not-displayed

A: First, the `name` in `Serie` must have a value that is not null. Then set `Legend` is `show`, where `data` can be empty by default, indicating that all legends are displayed. If you only want to display part of the `Serie` legend, fill in `data` with the `name` of the legend you want to display. If none of the values in `data` are `name` of the series, the legend will not be displayed.

## Q:How-to-make-chart-as-prefab

A: Before make prefab, please delete all sub gameObject under chart which auto-created by `XCharts`.

## Q:How-do-I-draw-custom-content-in-chart-such-as-line-or-dot

A: Implement `onCustomDraw` of chart, see `Example12_CustomDrawing.cs`.

## Q:How-to-achieve-similar-data-movement-effect-of-ELECTRO-cardiogram

A: See `Example_Dynamic.cs`.

## Q:How-do-I-use-the-background-component-What-are-the-conditions

A: Setting `show` to `true` for the `background` component does not necessarily activate the background component. Due to the limitations of the XCharts's framework, there are two prerequisites for the background component: first, the parent node of the chart cannot be controlled by layout, because the node relationship between the background component and the chart is parallel, and the location of the background component cannot be controlled by layout. Second, the parent node of the chart can only have one child node of the chart itself, which is convenient to manage the background component node needs, otherwise the reason of parallel relationship, easy to confuse. In addition, it is best to hide the `background` component when adjusting the chart hierarchy, which automatically removes the associated background component nodes.

## Q:Mesh-cannot-have-more-than-65000-vertices

A: This is the limit of `UGUI` on the number of vertices for a single `Graphic`. `XCharts` is draw chart on a single `Graphic`, so there is also this limitation. The solution can be referred to: [QA 10: Can display more than 1000 data](#Q:Can-display-more-than-1000-data)  

## Q:Why-are-the-parameters-set-in-Serie-reset-after-they-run

A: Check whether `RemoveData()` and add new `Serie` in the code. If you want to keep the configuration of `Serie`, you can only `ClearData()` which just clear data and then readd the data to the old serie.

[XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts API](xcharts-api-EN.md)  
[XCharts Configuration](xcharts-configuration-EN.md)