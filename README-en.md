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

## Overview

A powerful and easy-to-use data visualization library for Unity.  It supports more than ten built-in charts, including line, bar, pie, radar, scatter, heatmap, ring, candlestick, polar, parallel coordinates, as well as extended charts such as 3d pie, 3d bar, 3d pyramid, funnel, gauge, liquid, pictorialbar, gantt, treemap, sankey, line3d and graph chart.

## Key Features

- __Pure Code Rendering__: Charts are rendered with pure code, eliminating the need for extra texture or shader resources.
- __Visual Configuration__: Configure parameters visually with real-time preview and support for dynamic configuration and data adjustments at runtime.
- __High Customizability__: Themes and configuration parameters can be adjusted as needed, with support for custom drawing and callbacks.
- __Built-in and Extended Charts__: Supports a variety of chart types, including 3D charts and special chart types like gauges and treemaps.
- __Multiple Chart Combinations__: Combine multiple charts of the same or different types within a single instance.
- __Various Coordinate Systems__: Supports different coordinate systems such as Cartesian, polar, and single axes.
- __Rich Components__: Includes titles, legends, tooltips, and more.
- __Custom Drawing__: Utilize a powerful API for custom drawing of points, lines, and other graphics.
- __Large Data Rendering__: Capable of rendering tens of thousands of data points with support for sampling rendering.
- __Custom Themes__: Customize themes and use the included light and dark default themes.
- __Animations and Interactions__: Supports various animations and interactions for a dynamic user experience.
- __Third-Party Extensions__: Integrates with TextMeshPro and the New Input System.
- __Version and Compatibility__: Compatible with all Unity versions above 5.6 and runs on all platforms.

## Documentation

- [XCharts3.0 Homepage](https://xcharts-team.github.io)
- [XCharts3.0 Tutorial](Documentation~/en/tutorial01.md)  
- [XCharts3.0 API](Documentation~/en/api.md)  
- [XCharts3.0 FAQ](Documentation~/en/faq.md)  
- [XCharts3.0 Configurate](Documentation~/en/configuration.md)  
- [XCharts3.0 Changelog](Documentation~/en/changelog.md)  
- [XCharts3.0 Support](Documentation~/en/support.md)  

## Screenshots

![buildinchart](Documentation~/en/img/readme_buildinchart.png)

![extendchart](Documentation~/en/img/readme_extendchart.png)

## Important Notes

- `XCharts3.0` is not fully compatible with `XCharts2.0`. Upgrading to 3.0 may require code adjustments and reconfiguration of some charts.
- `XCharts2.0` is in the maintenance phase with only critical bug fixes applied.
- While XCharts supports Unity 5.6 and above, compatibility issues may arise due to limited testing.
- This repository contains only the `XCharts` source code. For demos, visit the [XCharts-Demo](https://github.com/XCharts-Team/XCharts-Demo) repo or the [Online Demo](https://xcharts-team.github.io/examples/).

## Getting Started

1. Import the `XCharts` unitypackage or source code into your Unity project.
2. Create a chart by right-clicking in the `Hierarchy` view and selecting `UI->XCharts->LineChart`.
3. Adjust component parameters in the `Inspector` to see real-time effects in the `Game` view.
4. For more details, refer to the [5-minute tutorial](Documentation~/en/tutorial01.md).

## Branch Information

- __master__ indicates the development branch. The latest changes and new features are first committed to the `master` branch, and after some time from the `master` branch `merge` to the `3.0` branch, and the `release` version.
- __3.0__ Stable branch of XCharts 3.0. It is generally updated once a month, with the latest changes from the `master` branch `merge`, and the `release` version is released.
- __2.0__ A stable branch of XCharts 2.0. With Demo, currently no longer maintenance, only to modify serious bugs.
- __2.0-upm__ Stable UMP branch of XCharts 2.0. Only the Package part is included without Demo. It is dedicated to the UMP and is not maintained.
- __1.0__ Stable branch of XCharts 1.0. With Demo, no maintenance.
- __1.0-upm__ stable UMP branch of XCharts 1.0. No Demo, no maintenance.

## FAQ

- __Is XCharts free to use?__ Yes, XCharts is free under the MIT license and includes value-added VIP services.
- __Does XCharts support dynamic data addition and modification?__ Yes, but data must be parsed or retrieved by the user.
- __Does this plugin work on platforms other than Unity?__ No, it is designed for Unity only.

## Changelog

- [Changelog](Documentation~/en/changelog.md)  

## Licenses

- XCharts is released under the [MIT License](https://github.com/XCharts-Team/XCharts/blob/master/LICENSE.md).

## Contact

- For more information or support, contact us at `monitor1394@gmail.com`.
