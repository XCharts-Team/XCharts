# XCharts

A powerful, easy-to-use, configurable charting and data visualization library for Unity.  

一款基于`UGUI`的功能强大、易用、参数可配置的数据可视化图表插件。支持折线图、柱状图、饼图、雷达图、散点图、热力图等常见图表。

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

* [XCharts主页](https://github.com/monitor1394/unity-ugui-XCharts)  
* 常见问题看这里☞ [XCharts问答](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts问答.md)  
* 接口文档看这里☞ [XChartsAPI手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XChartsAPI.md)  
* 参数配置看这里☞ [XCharts配置项手册](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/Documentation/XCharts配置项手册.md)  
* 更新日志看这里☞ [XCharts更新日志](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Assets/XCharts/CHANGELOG.md)  
* 新手教程看这里☞ [教程：5分钟上手XCharts](https://github.com/monitor1394/unity-ugui-XCharts/blob/master/Doc/教程：5分钟上手XCharts.md)  
