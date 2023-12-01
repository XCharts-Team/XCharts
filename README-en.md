<p align="center">
  <a href="">
    <img src="" alt="" width="" height="">
  </a>
</p>
<h2 align="center">XCharts</h3>
<p align="center">
  A powerful, easy-to-use, configurable charting and data visualization library for Unity.
  <br>
  Unity数据可视化图表插件。
  <br>
  <a href="https://github.com/XCharts-Team/XCharts">中文文档</a>
</p>
<p align="center">
  <a href="https://github.com/XCharts-Team/XCharts/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/XCharts-Team/XCharts">
  </a>
  <a href="https://github.com/XCharts-Team/XCharts/releases">
    <img src="https://img.shields.io/github/v/release/XCharts-Team/XCharts?include_prereleases">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/repo-size/monitor1394/unity-ugui-xcharts">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/languages/code-size/monitor1394/unity-ugui-xcharts">
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
    <img src="https://img.shields.io/github/stars/XCharts-Team/XCharts?style=social">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/forks/XCharts-Team/XCharts?style=social">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/issues-closed/XCharts-Team/XCharts?color=green&label=%20%20%20%20issues&logoColor=green&style=social">
  </a>
</p>

![XCharts](Documentation~/zh/img/xcharts.png)

A powerful and easy-to-use data visualization library for Unity.  It supports more than ten built-in charts, including line, bar, pie, radar, scatter, heatmap, ring, candlestick, polar, parallel coordinates, as well as extended charts such as 3d pie, 3d bar, 3d pyramid, funnel, gauge, liquid, pictorialbar, gantt, and treemap.

[XCharts3.0 Homepage](https://xcharts-team.github.io)

[XCharts3.0 Tutorial](Documentation~/en/tutorial01.md)  
[XCharts3.0 API](Documentation~/en/api.md)  
[XCharts3.0 FAQ](Documentation~/en/faq.md)  
[XCharts3.0 Configurate](Documentation~/en/configuration.md)  
[XCharts3.0 Changelog](Documentation~/en/changelog.md)  
[XCharts3.0 Support](Documentation~/en/support.md)  

## Features

* __Pure code rendering__: The chart is completely rendered with pure code, without the need for additional texture or shader resources.
* __Visual configuration__: Visual configuration of parameters with real-time preview of the effect, and support for dynamic modification of configuration and data during runtime.
* __High customizability__: Supports arbitrary adjustments from theme and configuration parameters; supports custom drawing, callback functions, and custom implementations of charts.
* __Multiple built-in charts__: Supports various built-in charts such as line charts, bar charts, pie charts, radar charts, scatter plots, heat maps, polar charts, K-line charts, parallel coordinates, etc.
* __Multiple extended charts__: Supports extended charts such as 3D column charts, 3D pie charts, funnel charts, pyramids, dashboards, water level charts, iconic bar charts, Gantt charts, and tree maps.
* __Multiple extended features__: Supports extended UI components such as tables and statistical values.
* __Multiple chart combinations__: Supports arbitrary combinations of built-in charts, with multiple same or different types of charts displayed simultaneously in the same chart.
* __Various coordinate systems__: Supports coordinate systems such as Cartesian coordinates, polar coordinates, and single axes.
* __Rich components__: Supports common components such as titles, legends, tooltips, markings, marking areas, data area zooming, and visual mapping.
* __Rich line charts__: Supports various line charts such as straight line charts, curved line charts, dashed line charts, area charts, step line charts, etc.
* __Rich bar charts__: Supports various bar charts such as stacked bar charts, stacked percentage bar charts, zebra bar charts, and capsule bar charts.
* __Rich pie charts__: Supports various pie charts such as ring charts, rose charts, ring rose charts, etc.
* __Rich lines__: Supports various lines such as solid lines, curves, step lines, dashed lines, dot lines, dotted lines, and double dot-dashed lines.
* __Custom drawing__: Supports custom chart content drawing with powerful drawing APIs for drawing points, lines, and other graphics.
* __Large data rendering__: Supports rendering of tens of thousands of data points; supports sampling rendering; special simplified charts support better performance.
* __Custom themes__: Supports theme customization and import/export; includes both light and dark default themes.
* __Animations and interactions__: Supports various animations such as fade-in animation, fade-out animation, change animation, addition animation, and * interactive animation; supports interactive operations such as data filtering, view zooming, and detailed display on multiple platforms.
* __Third-party extensions__: Supports integration with TexMeshPro and New Input System.
* __Version and compatibility__: Supports all Unity versions above 5.6 and runs on all platforms.

## Screenshots

![buildinchart](Documentation~/en/img/readme_buildinchart.png)

![extendchart](Documentation~/en/img/readme_extendchart.png)

## Attention

* `XCharts3.0` is not fully compatible with `XCharts2.0` version, upgrading `3.0` may require some code adjustments, and some chart configurations need to be readjusting. It is recommended that old projects can continue to use `XCharts2.0`, and new projects are recommended to use `XCharts3.0`.
* `XCharts2.0` enters the maintenance phase, and only serious `bugs` will be fixed later, in principle, no more new features will be added.
* `XCharts` theoretically supports `Unity 5.6` and above, but due to limited version testing, it is inevitable to slip up, and version compatibility issues can be raised.
* This repository only contains `XCharts` source code, does not contain `Demo` sample section. Need to look at ` Demo ` please go to the sample source code [XCharts - Demo](https://github.com/XCharts-Team/XCharts-Demo) repo. You can also view the running effect of `WebGL` in your browser [Online Demo](https://xcharts-team.github.io/examples/).

## Use

* Import `XCharts` unitypackage or source code into the project.
* Right-click `Hierarchy` view and choose `XCharts->LineChart` to create a default LineChart.
* You can adjust the parameters of each component in `Inspector` and see the real-time effects in `Game` view.
* For more details, see [[XCharts Tutorial: 5-minute tutorial]](Documentation~/en/tutorial01.md)
* For the first time, it is recommended to read the tutorial carefully.

## Branch

* `master` : indicates the development branch. The latest changes and new features are first committed to the `master` branch, and after some time from the `master` branch `merge` to the `3.0` branch, and the `release` version.
* `3.0` : Stable branch of XCharts 3.0. It is generally updated once a month, with the latest changes from the `master` branch `merge`, and the `release` version is released.
* `2.0` : A stable branch of XCharts 2.0. With Demo, currently no longer maintenance, only to modify serious bugs.
* `2.0-upm` : Stable UMP branch of XCharts 2.0. Only the Package part is included without Demo. It is dedicated to the UMP and is not maintained.
* `1.0` : Stable branch of XCharts 1.0. With Demo, no maintenance.
* `1.0-upm` : stable UMP branch of XCharts 1.0. No Demo, no maintenance.

## FAQ

* Is `XCharts` free to use?  
A: `XCharts` uses the `MIT` licence and is free to use. You can also subscribe to `VIP` to enjoy more value-added services.

* Does `XCharts` support code to dynamically add and modify data? Does it support getting data from `Excel` or a database?  
A: Support code to dynamically add and modify data, but data needs to be parsed or retrieved by itself, and then added to `XCharts` by calling the public interface of `XCharts`.

* Does this plugin work on other platforms (e.g. Winform or WPF) besides Unity?  
A: It is currently only supported on Unity. Theoretically any version of Unity that supports `UGUI` can run `XCharts`.

* What about the jags? What magnitude of data is supported?
A: XCharts is based on UGUI implementation, so the problems encountered in UGUI will also exist in XCharts. For example, the sawtooth problem, such as the number of vertices in `Mesh` exceeds `65535`. Solutions to these two problems can be found in [Q&A 16](Documentation~/en/faq.md) and [Q&A 27](Documentation~/en/faq.md).
Due to the `Mesh` of the `65535` vertex limit, the current `XCharts` single `Line` supports about `20,000` of data, of course, open sampling can support more data to draw, but at the same time it will consume more CPU.

## Changelog

* [Changelog](Documentation~/en/changelog.md)  

## Licenses

* [MIT License](https://github.com/XCharts-Team/XCharts/blob/master/LICENSE.md)
* Free commercial, secondary development
* The extended charts and advanced features sections require a separate purchase license

## Other

email: `monitor1394@gmail.com`
