<p align="center">
  <a href="">
    <img src="" alt="" width="" height="">
  </a>
</p>
<h2 align="center">XCharts</h3>
<p align="center">
  A powerful, easy-to-use, configurable charting and data visualization library for Unity.
  <br>
  一款基于UGUI的数据可视化图表插件。
  <br>
</p>
<p align="center">
  <a href="https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/LICENSE">
    <img src="https://img.shields.io/github/license/monitor1394/unity-ugui-XCharts">
  </a>
  <a href="https://github.com/monitor1394/unity-ugui-XCharts/releases">
    <img src="https://img.shields.io/github/v/release/monitor1394/unity-ugui-XCharts?include_prereleases">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/languages/code-size/monitor1394/unity-ugui-xcharts">
  </a>
  <a href="https://www.npmjs.org/package/unity-ugui-xcharts">
    <img src="https://img.shields.io/npm/dm/unity-ugui-xcharts?label=npm%20downloads%20">
  </a>
  <a href="">
    <img src="https://img.shields.io/badge/Unity-5.6+-green">
  </a>
  <a href="">
    <img src="https://img.shields.io/badge/TextMeshPro-YES-green">
  </a>
</p>
<p align="center">
  <a href="">
    <img src="https://img.shields.io/github/stars/monitor1394/unity-ugui-XCharts?style=social">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/forks/monitor1394/unity-ugui-XCharts?style=social">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/issues-closed/monitor1394/unity-ugui-XCharts?color=green&label=%20%20%20%20issues&logoColor=green&style=social">
  </a>
</p>

A powerful, easy-to-use, configurable charting and data visualization library for Unity.  Supporting line, bar, pie, radar, scatter, heatmap, gauge, ring, polar, liquid and other common chart.

[XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
[XCharts Q&A](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-questions-and-answers-EN.md)  
[XCharts API](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-api-EN.md)  
[XCharts Configuration](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-configuration-EN.md)  
[XCharts Changelog](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/CHANGELOG-EN.md)  
[Tutorial - Get start with XCharts in 5 minute](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/tutorial--get-start-with-xcharts-in-5-minute-EN.md)

## XCharts 2.0

* Underlevel reconstruction, layered rendering, optimized scalability, support more data.
* Support for multi-component patterns.
* Support for any combination of most charts.
* Support theme customization, import and export, and more theme configuration parameters.
* Support global adjustment of configuration parameters.
* Added support for `TextMeshPro`.
* Added `CandlestickChart`.
* __ Note:__ `2.0` has undergone a lot of refactoring and adjustments and is not fully compatible with version `1.0`, so `1.0` should not be directly upgraded to `2.0`. Some parameter Settings may be reset to default values after upgrading.

## Features

* Rich built-in examples and templates, parameter visualization configuration, effect real-time preview, pure code drawing.
* Support line, bar, pie, radar, scatter, heatmaps, gauge, ring, polar, liquid and other common chart.
* Support line graph, curve graph, area graph, step graph and other LineChart.
* Support parallel bar, stack bar, stack percentage bar, zebra bar and other BarChart.
* Support ring, rose and other PieChart.
* Support line-bar chart, scatter-line chart and other combination chart.
* Support solid line, curve, ladder line, dotted line, dash line, dot line, double dot line and other lines.
* Support custom theme, built-in theme switching.
* Support custom chart content drawing, drawing points, line, curve, triangle, quadrilateral, circle, ring, sector, border, arrow and other drawing API.
* Support interactive operations such as data filtering, view zooming and detail display on PC and mobile terminals.
* Support 10,000-level big data rendering.

## Screenshot

![linechart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-line.png)
![barchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-bar.png)
![piechart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-pie.png)
![radarchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-radar.png)
![scatterchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-scatter.png)
![heatmapchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-heatmap.png)
![gaugechart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-gauge.png)
![ringchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-ring.png)
![polarchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-polar.png)
![liquidchart](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-liquid.png)
![combinations](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-combinations.png)

## Cheat Sheet

![cheatsheet](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/screenshot/xcharts-cheatsheet.gif)

`XCharts` consist of components and data. Different components and data can be combined into different types of charts. The component is divided into main component and sub component. The main component contains the sub components.

`XCharts` main components:

* `Theme` theme component, which can configure the default colors, fonts and so on.
* `Title` title component, which contains the main title and subtitle.
* `Legend` legend component, which represent different sets of symbols, colors, and names. You can control which series are not displayed by clicking on the legend.
* `Grid` grid component, drawing grid in rectangular coordinate system. Up to two X axes and two Y axes can be placed within a grid component. You can draw line, bar and scatter chart on the grid.
* `Axis` axis component, the axis of a rectangular coordinate system. Supports the upper and lower X axes and the left and right Y axes.
* `Series` series component, a list of serie. A chart can contain many different series, and each series determines its own chart type by type.
* `Tooltip` tooltip component, feedback more details of the data indicated by the mouse at the time.
* `DataZoom` data zoom component, used for area zooming so you can focus on detailed data information, or overview the data as a whole, or remove the impact of outliers.
* `VisualMap` visual mapping component, you can map data in different colors.
* `Radar` radar component, suitable for radar chart only.
* `Settings` global Settings component, Some global parameters can be adjusted. Use the default values in general and adjust them as needed.

`XCharts` support chart:

* `LineChart`
* `BarChart`
* `PieChart`
* `RadarChart`
* `ScatterChart`
* `HeatmapChart`
* `GuageChart`
* `RingChart`
* `PolarChart`
* `LiquidChart`

The following is the relationship structure of LineChart:

``` js
.
├── LineChart
.   ├── ThemeInfo
    ├── Title
    │   └── Location
    ├── Legend
    │   └── Location
    ├── Tooltip
    ├── DataZoom
    ├── VisualMap
    ├── Grid
    ├── Axis
    │   ├── AxisLine
    │   ├── AxisName
    │   ├── AxisLabel
    │   ├── AxisTick
    │   └── AxisSplitArea
    ├── Series
    │   ├── ItemStyle
    │   ├── AreaStyle
    │   ├── SerieSymbol
    │   ├── LineStyle
    │   ├── LineArrow
    │   ├── SerieLabel
    │   ├── Emphasis
    │   ├── Animation
    │   └── SerieData
    └── Settings
```

## Environment

* Unity2017.4.27f1
* .Net 3.5
* macOS 10.15.4

## Usage

* This project was developed under `Unity 2017.4.27f1` and `.net 3.5`, tested normally on `Unity 5`, `Unity 2018` and `Unity 2019`. It can theoretically run on any version that supports `UGUI`.
* Download the source code or `unitypackage` to import into your project. If `Unity` version are `2018.3` or above, it is recommended to import packages through `Package Manager`:
  1. Open the `manifest.json` file under `Packages` directory and add under `dependencies`:
  ``` json
     "com.monitor1394.xcharts": "https://github.com/monitor1394/unity-ugui-XCharts.git#2.0-upm",
  ```
  2. Going back to `Unity`, it may take 3 to 5 minutes to download.
  3. If you want to delete `XCharts`, just delete the content added in step 1.
  4. If you want to update `XCharts`, open `manifest.json` file , delete the content about `com.monitor1394.xcharts` under `lock`, it will download anagain. Also can check For update in `components-> XCharts -> Check For Update`.

* Add a chart in Editor quickly:
  1. In `Hierarchy`, right-click menu `XChart->LineChart`.
  2. In unity menu bar, `Component->XCharts->LineChart`.
  3. In `Inspector`,`Add Component->LineChart`.
  4. Then a simple line chart is done.
  5. In `Inspector` you can adjust the parameters of components, and in `Game` will feedback the adjustment effect in realtime 。the detail of parameters  go to see: [XCharts Configuration](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-configuration-EN.md).

* See more examples of code dynamic control: [Tutorial - Get start with XCharts in 5 minute](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/tutorial--get-start-with-xcharts-in-5-minute-EN.md).

* Enable TextMeshPro:
  1. `XCharts -> TextMeshPro Enable` or `Project Setting -> XCharts -> Enable TextMeshPro`
  2. `Project Setting -> XCharts -> Settings -> TMP Font` set the TextMeshPro font.
  3. If the chart does not initialize properly, you can use the `Remove All Chart Object` button to clean up the chart and reinitialize it.

## Documents

* [XCharts Homepage](https://github.com/monitor1394/unity-ugui-XCharts)  
* [XCharts Q&A](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-questions-and-answers-EN.md)  
* [XCharts API](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-api-EN.md)  
* [XCharts Configuration](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/xcharts-configuration-EN.md)
* [XCharts Changelog](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/CHANGELOG-EN.md)  
* [XCharts Tutorial: Get start with XCharts in 5 minute](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Doc/tutorial--get-start-with-xcharts-in-5-minute-EN.md)

## Changelog

[XCharts Changelog](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/CHANGELOG.md)  

## Licenses

[MIT License](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/LICENSE.md)

## Contact

gmail: monitor1394@gmail.com
