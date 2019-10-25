# XCharts

![license](https://img.shields.io/github/license/monitor1394/unity-ugui-XCharts)
![issues](https://img.shields.io/github/issues/monitor1394/unity-ugui-XCharts)
![issues](https://img.shields.io/github/stars/monitor1394/unity-ugui-XCharts)
![issues](https://img.shields.io/github/forks/monitor1394/unity-ugui-XCharts)

A powerful, easy-to-use, configurable charting and data visualization library for Unity.  

一款基于`UGUI`的功能强大、易用、参数可配置的数据可视化图表插件。支持折线图、柱状图、饼图、雷达图、散点图、热力图等常见图表。

[XCharts问答](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)  
[XChartsAPI手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XChartsAPI.md)  
[XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)  
[XCharts更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  
[教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  

## 特性

* 内置丰富示例和模板，参数可视化配置，效果实时预览，纯代码绘制。
* 支持折线图、柱状图、饼图、雷达图、散点图、热力图等常见图表。
* 支持直线图、曲线图、面积图、阶梯线图等折线图。
* 支持并列柱图、堆叠柱图、堆积百分比柱图、斑马柱图等柱状图。
* 支持环形图、玫瑰图等饼图。
* 支持折线图—柱状图、散点图-折线图等组合图。
* 支持实线、曲线、阶梯线、虚线、点线、点划线、双点划线等线条。
* 支持自定义主题，内置主题切换。
* 支持自定义图表内容绘制，提供绘制点、直线、曲线、三角形、四边形、圆形、环形、扇形、边框、箭头等绘图API。
* 支持PC端和手机端上的数据筛选、视图缩放、细节展示等交互操作。
* 支持万级大数据绘制。

## 截图

<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-line.png" width="550" height="auto"/>
<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-bar.png" width="550" height="auto"/>
<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-pie.png" width="550" height="auto"/>
<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-radar.png" width="550" height="auto"/>
<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-scatter.png" width="550" height="auto"/>
<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-heatmap.png" width="550" height="auto"/>

## 术语

<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/screenshot/xcharts-cheatsheet.gif" />

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

## 使用

* 本项目在`Unity 2018.3.14f1`和`.Net 3.5`下开发，在 `Unity 5`、`Unity 2017`、`Unity 2019`上测试正常。理论上可运行于任何支持`UGUI`的`Unity`版本。
* 通过下载源码或`unitypackage`包导入到你的项目中。如果你是`2018.3`及以上版本，强烈建议通过`Package Manager`的`Git`来导入包：
  1. 打开`Packages`目录下的`manifest.json`文件，在`dependencies`下加入：  
  ``` json
  "com.monitor1394.xcharts": "https://github.com/monitor1394/unity-ugui-XCharts.git#package",
  ```
  2. 回到`Unity`，可能会花3到5分钟进行下载和编译，成功后就可以开始使用`XCharts`了。
  3. 如果要删除`XCharts`，删除掉1步骤所加的内容即可。
  4. 如果要更新`XCharts`，删除`manifest.json`文件的`lock`下的`com.monitor1394.xcharts`相关内容即会从新下载编译。在 `Component -> XCharts -> Check For Update`可以检测是否有新版本可更新。

* 在Editor上快速创建一个图表：

  1. 在`Canvas`下通过`Create Empty`创建一个空`gameObject`，命名为 `line_chart`。
  2. 通过菜单栏 `Component->XCharts->LineChart` 或者  `Inspector` 视图的 `Add Component` 添加 `LineChart` 脚本。一个简单的折线图就出来了。
  3. `Inspector` 视图下可以调整各个组件的参数，`Game` 视图会实时反馈调整的效果。各个组件的详细参数说明可查阅[XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)。

* 更多的代码动态控制的例子请参考[教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  。

## 文档

* 常见问题看这里☞ [XCharts问答](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)  
* 接口文档看这里☞ [XChartsAPI手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XChartsAPI.md)  
* 参数配置看这里☞ [XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)  
* 更新日志看这里☞ [XCharts更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  
* 新手教程看这里☞ [教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  

## 结构

``` js
.
├── Demo                                        // Demo
│   ├── Editor
│   │   ├── ChartModuleDrawer.cs
│   │   └── DemoEditor.cs
│   ├── Runtime
│   │   ├── Demo_Dynamic.cs
│   │   ├── Demo_LargeData.cs
│   │   ├── Demo_PieChart.cs
│   │   ├── Demo_Test.cs
│   │   ├── Demo.cs
│   │   ├── Demo00_CheatSheet.cs
│   │   ├── Demo10_LineChart.cs
│   │   ├── Demo11_AddSinCurve.cs
│   │   ├── Demo12_CustomDrawing.cs
│   │   ├── Demo13_LineSimple.cs
│   │   ├── Demo20_BarChart.cs
│   │   ├── Demo30_PieChart.cs
│   │   ├── Demo50_Scatter.cs
│   │   ├── Demo60_Heatmap.cs
│   └── demo-xchart.unity
├── Scripts                                     // 源码
.   ├── Editor                                  // Editor相关代码
    │   ├── PropertyDrawers                     // 组件Drawer
    │   │   ├── AnimationDrawer.cs
    │   │   ├── AreaStyleDrawer.cs
    │   │   ├── AxisDrawer.cs
    │   │   ├── AxisLabelDrawer.cs
    │   │   ├── AxisLineDrawer.cs
    │   │   ├── AxisNameDrawer.cs
    │   │   ├── AxisSplitAreaDrawer.cs
    │   │   ├── AxisTickDrawer.cs
    │   │   ├── DataZoomDrawer.cs
    │   │   ├── EmphasisDrawer.cs
    │   │   ├── GridDrawer.cs
    │   │   ├── ItemStyleDrawer.cs
    │   │   ├── LegendDrawer.cs
    │   │   ├── LineArrowDrawer.cs
    │   │   ├── LineStyleDrawer.cs
    │   │   ├── LocationDrawer.cs
    │   │   ├── RadarDrawer.cs
    │   │   ├── RadarIndicatorDrawer.cs
    │   │   ├── SerieDrawer.cs
    │   │   ├── SerieLabelDrawer.cs
    │   │   ├── SeriesDrawer.cs
    │   │   ├── SerieSymbolDrawer.cs
    │   │   ├── SettingsDrawer.cs
    │   │   ├── ThemeInfoDrawer.cs
    │   │   ├── TitleDrawer.cs
    │   │   ├── TooltipDrawer.cs
    │   │   ├── VisualMapDrawer.cs
    │   │   ├── XAxisDrawer.cs
    │   │   └── YAxisDrawer.cs
    │   ├── Ultility                            // Editor相关工具类
    │   │   └── ChartEditorHelper.cs
    │   ├── BarChartEditor.cs
    │   ├── BaseChartEditor.cs
    │   ├── CoordinateChartEditor.cs
    │   ├── HeatmapChartEditor.cs
    │   ├── LineChartEditor.cs
    │   ├── PieChartEditor.cs
    │   ├── RadarChartEditor.cs
    │   └── ScatterChartEditor.cs
    └── Runtime                                 // 核心代码
        ├── API                                 // Chart API
        │   ├── BaseChart_API.cs
        │   └── CoordinateChart_API.cs
        ├── Component                           // Chart的主组件和子组件
        │   ├── Main
        │   │   ├── Axis.cs
        │   │   ├── DataZoom.cs
        │   │   ├── Grid.cs
        │   │   ├── Legend.cs
        │   │   ├── Radar.cs
        │   │   ├── Serie.cs
        │   │   ├── Series.cs
        │   │   ├── Settings.cs
        │   │   ├── Theme.cs
        │   │   ├── Title.cs
        │   │   ├── Tooltip.cs
        │   │   └── VisualMap.cs
        │   ├── Sub
        │   │   ├── Animation.cs
        │   │   ├── AreaStyle.cs
        │   │   ├── AxisLabel.cs
        │   │   ├── AxisLine.cs
        │   │   ├── AxisName.cs
        │   │   ├── AxisSplitName.cs
        │   │   ├── AxisTick.cs
        │   │   ├── Emphasis.cs
        │   │   ├── ItemStyle.cs
        │   │   ├── LineArrow.cs
        │   │   ├── LineStyle.cs
        │   │   ├── Location.cs
        │   │   ├── SerieData.cs
        │   │   ├── SerieLabel.cs
        │   │   └── SerieSymbol.cs
        │   ├── ChartComponent.cs
        │   ├── MainComponent.cs
        │   └── SubComponent.cs
        ├── Helper
        ├── Interface
        │   ├── IJsonData.cs
        │   └── IPropertyChanged.cs
        ├── Internal
        │   ├── AxisPool.cs
        │   ├── BaseChart.cs
        │   ├── CoordinateChart_DrawBar.cs
        │   ├── CoordinateChart_DrawHeatmap.cs
        │   ├── CoordinateChart_DrawLine.cs
        │   ├── CoordinateChart_DrawScatter.cs
        │   ├── CoordinateChart.cs
        │   ├── JsonDataSupport.cs
        │   ├── ListPool.cs
        │   └── ObjectPool.cs
        ├── Template
        ├── Utility
        │   ├── ChartCached.cs
        │   ├── ChartDrawer.cs
        │   └── ChartHelper.cs
        ├── BarChart.cs
        ├── HeatmapChart.cs
        ├── LineChart.cs
        ├── PieChart.cs
        ├── RadarChart.cs
        └── ScatterChart.cs


? directories, ? files
```

## 更新日志

[更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  

## Licenses

[MIT License](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/LICENSE.md)

## 开发交流

邮箱：monitor1394@gmail.com  
QQ群：XCharts交流群（`202030963`）  
VIP群：XCharts技术支持VIP群（`867291970`）  

## 捐助

如果这个项目对您有帮助，请右上方点 `Star` 予以支持！也欢迎各方任何形式的捐助，任何金额的赞助都将非常感谢。

企业赞助请备注公司名称。

<img src="https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/alipay.png?raw=true" width="200"  height="auto"/>  

如需商业技术支持，捐助280¥可加VIP群（`867291970`，验证信息请输入捐助的支付宝账号）。
