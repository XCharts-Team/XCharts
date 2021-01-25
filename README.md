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
  <a href="Assets/XCharts/README.md">English Doc</a>
</p>
<p align="center">
  <a href="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/monitor1394/unity-ugui-XCharts">
  </a>
  <a href="https://github.com/monitor1394/unity-ugui-XCharts/releases">
    <img src="https://img.shields.io/github/v/release/monitor1394/unity-ugui-XCharts?include_prereleases">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/repo-size/monitor1394/unity-ugui-xcharts">
  </a>
  <a href="">
    <img src="https://img.shields.io/github/languages/code-size/monitor1394/unity-ugui-xcharts">
  </a>
  <a href="https://github.com/monitor1394/unity-ugui-XCharts/releases">
    <img src="https://img.shields.io/github/downloads/monitor1394/unity-ugui-XCharts/total?label=github%20downloads">
  </a>
  <a href="https://www.npmjs.org/package/unity-ugui-xcharts">
    <img src="https://img.shields.io/npm/dt/unity-ugui-xcharts?label=npm%20downloads%20">
  </a>
  <a href="https://www.npmjs.org/package/unity-ugui-xcharts">
    <img src="https://img.shields.io/npm/dm/unity-ugui-xcharts?label=%20">
  </a>
  <a href="">
    <img src="https://img.shields.io/badge/Unity-5.6%20%7C%202017%20%7C%202018%20%7C%202019%20%7C%202020%20%7C%202021-green">
  </a>
  <a href="">
    <img src="https://img.shields.io/badge/TextMeshPro-YES-green">
  </a>
</p>

一款基于`UGUI`的功能强大、易用、参数可配置的数据可视化图表插件。支持折线图、柱状图、饼图、雷达图、散点图、热力图、仪表盘、环形图、极坐标、水位图等常见图表。

[XCharts问答](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)  
[XChartsAPI手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XChartsAPI.md)  
[XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)  
[XCharts更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  
[教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  

## XCharts 2.0

* 底层重构，分层绘制，优化可扩展性，支持更多数据。
* 支持TextMeshPro。
* 支持多组件模式。
* 支持大部分图表的任意组合。
* 支持主题定制、导入和导出，更多的主题配置参数。
* 支持全局配置参数调整。
* 更友好的编辑界面。
* 其他细节优化。

## 特性

* 内置丰富示例和模板，参数可视化配置，效果实时预览，纯代码绘制。
* 支持折线图、柱状图、饼图、雷达图、散点图、热力图、热力图、仪表盘、环形图、极坐标、水位图等十种常见图表。
* 支持直线图、曲线图、面积图、阶梯线图等折线图。
* 支持并列柱图、堆叠柱图、堆积百分比柱图、斑马柱图等柱状图。
* 支持环形图、玫瑰图等饼图。
* 支持大部分图表的任意组合，同一图表中可同时显示多个相同或不同类型的图表。
* 支持实线、曲线、阶梯线、虚线、点线、点划线、双点划线等线条。
* 支持主题定制、导入和导出，内置三种默认主题。
* 支持自定义图表内容绘制，提供绘制点、直线、曲线、三角形、四边形、圆形、环形、扇形、边框、箭头等绘图API。
* 支持PC端和手机端上的数据筛选、视图缩放、细节展示等交互操作。
* 支持万级大数据绘制。
* 支持`TexMeshPro`。

## 截图

![linechart](Doc/screenshot/xcharts-line.png)
![barchart](Doc/screenshot/xcharts-bar.png)
![piechart](Doc/screenshot/xcharts-pie.png)
![radarchart](Doc/screenshot/xcharts-radar.png)
![scatterchart](Doc/screenshot/xcharts-scatter.png)
![heatmapchart](Doc/screenshot/xcharts-heatmap.png)

## 术语

![cheatsheet](Doc/screenshot/xcharts-cheatsheet.gif)

XCharts的图表由组件和数据组成。不同的组件和数据可以组合成不同类型的图表。组件分为主组件和子组件，主组件包含子组件。  

`XCharts` 支持的主组件：

* `Theme` 主题组件：可以配置图表各组件默认的颜色、字体等。
* `Title` 标题组件：包含主标题和副标题。
* `Legend` 图例组件：图例组件展现了不同系列的标记(symbol)，颜色和名字。可以通过点击图例控制哪些系列不显示。
* `Grid` 网格组件：直角坐标系内绘图网格。一个网格组件内最多可以放置上下两个 X 轴，左右两个 Y 轴。可以在网格上绘制折线图，柱状图，散点图。
* `Axis` 坐标轴组件：直角坐标系的坐标轴。支持上下两个 X 轴，左右两个 Y 轴。
* `Series` 系列组件：系列列表。一个图表可以包含多个不同的系列，每个系列通过 type 决定自己的图表类型。
* `Tooltip` 提示框组件：反馈当时鼠标所指示数据的更多细节。
* `DataZoom` 区域缩放组件：用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。
* `VisualMap` 视觉映射组件：可以对数据进行不同颜色的映射。
* `Radar` 雷达组件：雷达图坐标系组件，只适用于雷达图。
* `Settings` 全局设置组件：可以对一些全局的参数进行调整。一般情况下使用默认值即可，当有需要时可进行调整。

`XCharts` 支持的图表：

* `LineChart` 折线图：折线图是用折线将各个数据点标志连接起来的图表，用于展现数据的变化趋势。
* `BarChart` 柱状图：柱状图 通过 柱形的高度/条形的宽度 来表现数据的大小，用于有至少一个类目轴或时间轴的直角坐标系上。
* `PieChart` 饼图：饼图主要用于表现不同类目的数据在总和中的占比。每个的弧度表示数据数量的比例。饼图更适合表现数据相对于总数的百分比等关系。如果只是表示不同类目数据间的大小，建议使用 柱状图。
* `RadarChart` 雷达图：雷达图主要用于表现多变量的数据，例如球员的各个属性分析。依赖 radar 组件。
* `ScatterChart` 散点图：直角坐标系上的散点图可以用来展现数据的 x，y 之间的关系，如果数据项有多个维度，其它维度的值可以通过不同大小的 symbol 展现成气泡图，也可以用颜色来表现。
* `HeatmapChart` 热力图：热力图主要通过颜色去表现数值的大小，必须要配合 visualMap 组件使用。
* `GuageChart` 仪表盘。
* `RingChart` 环形图。区别于`PieChart`中的环形图，`RingChart`只支持一个数据，一般用于表示百分比。

以下是LineChart折线图和主组件、子组件的关系结构：

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

## 开发环境

* Unity2017.4.27f1, .Net 3.5
* macOS 10.15.4

## 使用

* 本项目在`Unity 2017.4.27f1`和`.Net 3.5`下开发，在 `Unity 5`、`Unity 2018`、`Unity 2019`上测试正常。理论上可运行于任何支持`UGUI`的`Unity`版本。
* 通过下载源码或`unitypackage`包导入到你的项目中。如果你是`2018.3`及以上版本，可通过`Package Manager`的`Git`来导入包：
  1. 打开`Packages`目录下的`manifest.json`文件，在`dependencies`下加入：  
  ``` json
  "com.monitor1394.xcharts": "https://github.com/monitor1394/unity-ugui-XCharts.git#upm",
  ```
  2. 回到`Unity`，可能会花3到5分钟进行下载和编译，成功后就可以开始使用`XCharts`了。
  3. 如果要删除`XCharts`，删除掉1步骤所加的内容即可。
  4. 如果要更新`XCharts`，删除`manifest.json`文件的`lock`下的`com.monitor1394.xcharts`相关内容即会从新下载编译。在 `Component -> XCharts -> Check For Update`可以检测是否有新版本可更新。

* 在Editor上快速创建一个图表：

  1. 在`Hierarchy`试图下右键或菜单栏`GameObject`下拉：`XCharts->LineChart`，即可快速创建一个简单的折线图出来。
  2. `Inspector` 视图下可以调整各个组件的参数，`Game` 视图会实时反馈调整的效果。各个组件的详细参数说明可查阅[XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)。

* 更多的代码动态控制的例子请参考[教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  。

## 文档

* 常见问题看这里☞ [XCharts问答](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)  
* 接口文档看这里☞ [XChartsAPI手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XChartsAPI.md)  
* 参数配置看这里☞ [XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)  
* 更新日志看这里☞ [XCharts更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  
* 新手教程看这里☞ [教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  

## 更新日志

[更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  

## Licenses

[MIT License](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/LICENSE.md)

## 其他

邮箱：monitor1394@gmail.com  
QQ群：XCharts交流群（`202030963`）  
VIP群：XCharts技术支持VIP群（`867291970`）  
捐助和技术支持：[☞ 看这里](SUPPORT.md)
