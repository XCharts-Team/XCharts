# XCharts

A powerful, easy-to-use, configurable charting and data visualization library for Unity. Support line charts, bar charts, pie charts, radar charts, scatter charts, heatmaps, gauge, ring charts and other common charts.

## Features

* Rich built-in examples and templates, parameter visualization configuration, effect real-time preview, pure code drawing.
* Support line charts, bar charts, pie charts, radar charts, scatter charts, heatmaps, gauge charts, ring charts and other common charts.
* Support line graph, curve graph, area graph, step graph, etc.
* Support parallel bar chart, stack bar chart, stack percentage bar chart, zebra bar chart, etc.
* Support for ring chart, rose chart and other pie chart.
* Support broken line graph - bar graph, scatter graph - broken line graph, etc.
* Support solid line, curve, ladder line, dotted line, dot line, dot line, double point line and other lines.
* Support custom theme, built-in theme switching.
* support custom chart content drawing, drawing points, line, curve, triangle, quadrilateral, circle, ring, sector, border, arrow and other drawing API.
* support interactive operations such as data filtering, view zooming and detail display on PC and mobile terminals.
* support 10,000-level big data rendering.

## Environment

* Unity2017.4.27f1
* .Net 3.5
* macOS 10.15.4

## Usage

* This project was developed under `Unity 2017.4.27f1` and `.net 3.5`, tested normally on `Unity 5`, `Unity 2018` and `Unity 2019`. It can theoretically run on any version that supports `UGUI`.
* Download the source code or `unitypackage` to import into your project. If `Unity` version are `2018.3` or above, it is recommended to import packages through `Package Manager`:
  1. Open the `manifest.json` file under `Packages` directory and add under `dependencies`:
  ``` json
     "com.monitor1394.xcharts": "https://github.com/monitor1394/unity-ugui-XCharts.git#package",
  ```
  2. Going back to `Unity`, it may take 3 to 5 minutes to download.
  3. If you want to delete `XCharts`, just delete the content added in step 1.
  4. If you want to update `XCharts`, open `manifest.json` file , delete the content about `com.monitor1394.xcharts` under `lock`, it will download anagain. Also can check For update in `components-> XCharts -> Check For Update`.

* Add a chart in Editor quickly:
  1. In `Hierarchy`, right-click menu `XChart->LineChart`.
  2. In unity menu bar, `Component->XCharts->LineChart`.
  3. In `Inspector`,`Add Component->LineChart`.
  4. Then a simple line chart is done.
  5. In `Inspector` you can adjust the parameters of components, and in `Game` will feedback the adjustment effect in realtime 。the detail of parameters  go to see: [XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md).

* See more examples of code dynamic control: [教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md) .

## Documents

* [XCharts主页](https://github.com/monitor1394/unity-ugui-XCharts)  
* 常见问题看这里☞ [XCharts问答](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)  
* 接口文档看这里☞ [XChartsAPI手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XChartsAPI.md)  
* 参数配置看这里☞ [XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)  
* 更新日志看这里☞ [XCharts更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  
* 新手教程看这里☞ [教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  
