<p align="center">
  <a href="">
    <img src="" alt="" width="" height=""></img>
  </a>
</p>
<h2 align="center">XCharts</h2>
<p align="center">
  A powerful, easy-to-use, configurable charting and data visualization library for Unity.
  <br/>
  Unity数据可视化图表插件。
  <br/>
  <a href="README-en.md">English README</a>
</p>
<p align="center">
  <a href="https://github.com/XCharts-Team/XCharts/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/XCharts-Team/XCharts"></img>
  </a>
  <a href="https://github.com/XCharts-Team/XCharts/releases">
    <img src="https://img.shields.io/github/v/release/XCharts-Team/XCharts?include_prereleases"></img>
  </a>
  <a href="https://github.com/XCharts-Team/XCharts">
    <img src="https://img.shields.io/github/repo-size/monitor1394/unity-ugui-xcharts"></img>
  </a>
  <a href="https://github.com/XCharts-Team/XCharts">
    <img src="https://img.shields.io/github/languages/code-size/monitor1394/unity-ugui-xcharts"></img>
  </a>
  <a href="https://xcharts-team.github.io/docs/tutorial01">
    <img src="https://img.shields.io/badge/Unity-5.6+-green"></img>
  </a>
  <a href="https://xcharts-team.github.io/docs/tutorial01">
    <img src="https://img.shields.io/badge/TextMeshPro-YES-green"></img>
  </a>
</p>
<p align="center">
  <a href="https://github.com/XCharts-Team/XCharts/stargazers">
    <img src="https://img.shields.io/github/stars/XCharts-Team/XCharts?style=social"></img>
  </a>
  <a href="https://github.com/XCharts-Team/XCharts/forks">
    <img src="https://img.shields.io/github/forks/XCharts-Team/XCharts?style=social"></img>
  </a>
  <a href="https://github.com/XCharts-Team/XCharts/issues">
    <img src="https://img.shields.io/github/issues-closed/XCharts-Team/XCharts?color=green&label=%20%20%20%20issues&logoColor=green&style=social"></img>
  </a>
</p>

一款基于`UGUI`的功能强大、简单易用的数据可视化图表插件。支持`折线图`、`柱状图`、`饼图`、`雷达图`、`散点图`、`热力图`、`环形图`、`K线图`、`极坐标`、`平行坐标`等十多种内置图表，以及`3D饼图`、`3D柱图`、`3D金字塔`、`漏斗图`、`仪表盘`、`水位图`、`象形柱图`、`甘特图`、`矩形树图`等扩展图表。

[XCharts3.0 官方主页](https://xcharts-team.github.io)
[XCharts3.0 在线示例](https://xcharts-team.github.io/examples)

[XCharts3.0 教程](Documentation~/zh/tutorial01.md)
[XCharts3.0 API](Documentation~/zh/api.md)
[XCharts3.0 问答](Documentation~/zh/faq.md)
[XCharts3.0 配置项手册](Documentation~/zh/configuration.md)
[XCharts3.0 更新日志](Documentation~/zh/changelog.md)
[XCharts3.0 订阅服务](Documentation~/zh/support.md)

## 特性

- 参数可视化配置，效果实时预览，纯代码绘制，无需额外资源。
- 支持折线图、柱状图、饼图、雷达图、散点图、热力图、环形图、K线图、极坐标、平行坐标等十种内置图表。
- 支持3D柱图、漏斗图、金字塔、仪表盘、水位图、象形柱图、甘特图、矩形树图等多种扩展图表。
- 支持直线图、曲线图、面积图、阶梯线图等折线图。
- 支持并列柱图、堆叠柱图、堆积百分比柱图、斑马柱图等柱状图。
- 支持环形图、玫瑰图等饼图。
- 支持内置图表的任意组合，同一图中可同时显示多个相同或不同类型的图表。
- 支持实线、曲线、阶梯线、虚线、点线、点划线、双点划线等线条。
- 支持主题定制、导入和导出，内置明暗两种默认主题。
- 支持自定义图表内容绘制，提供绘制点、线、面等其他图形的强大的绘图API。
- 支持PC端和手机端上的数据筛选、视图缩放、细节展示等交互操作。
- 支持万级大数据量绘制，支持采样绘制。
- 支持`TexMeshPro`。
- 支持所有`5.6`以上的`Unity`版本。
- 支持 Input System （[如何从 Input Manager 转 Input System](https://xcharts-team.github.io/docs/inputsystem)）。

## 截图

![内置图表](Documentation~/zh/img/readme_buildinchart.png)

![扩展图表](Documentation~/zh/img/readme_extendchart.png)

![XCharts 3.0和2.0的对比](Documentation~/zh/img/readme_xcharts3.png)

## 注意

- `XCharts3.0` 不完全兼容 `XCharts2.0` 版本，升级`3.0`可能需要调整部分代码，图表部分配置需要重新调整。建议旧项目可以继续使用`XCharts2.0`，新项目推荐使用`XCharts3.0`。
- `XCharts2.0` 进入维护阶段，后续只修复严重`bug`，原则上不再加新功能。
- `XCharts` 理论上支持`Unity 5.6`及以上版本，但由于版本测试有限难免疏漏，发现版本兼容问题可提`Issue`。
- 本仓库只包含`XCharts`源码，不包含`Demo`示例部分。需要查看`Demo`示例源码请到[XCharts-Demo](https://github.com/XCharts-Team/XCharts-Demo)仓库。也可以在浏览器查看`WebGL`下的运行效果 [在线Demo](https://xcharts-team.github.io/examples/) 。

## 使用

1. 导入`XCharts`的`unitypackage`或者源码到项目。
2. 在`Hierarchy`视图下右键选择`XCharts->LineChart`，即可创建一个默认的折线图。
3. 在`Inspector`视图可以调整各个组件的参数，`Game`视图可看到实时效果。

- 更多细节，请看：[XCharts教程：5分钟上手教程](Documentation~/zh/tutorial01.md)
- 首次使用，建议先认真看一遍教程。

## 分支

- `master`：开发分支。最新的修改和新功能都先提交到`master`分支，一段时间后再从`master`分支`merge`到`3.0`分支，并发布`release`版本。
- `3.0`：XCharts 3.0的稳定分支。一般一个月一更新，从`master`分支`merge`最新改动过来，并发布`release`版本。
- `2.0`：XCharts 2.0的稳定分支。带Demo，目前基本不再维护，仅修改严重bug。
- `2.0-upm`：XCharts 2.0的稳定UMP分支。不带Demo，只包含Package部分，专门给UMP用，不再维护。
- `1.0`：XCharts 1.0的稳定分支。带Demo，不再维护。
- `1.0-upm`：XCharts 1.0的稳定UMP分支。不带Demo，不再维护。

## 扩展

- [扩展图表](https://xcharts-team.github.io/docs/extra) 扩展图表订阅`VIP`后再按需购买，订阅详情请查看[☞ 看这里](Documentation~/zh/support.md)
- [扩展组件](https://xcharts-team.github.io/docs/ui) 扩展组件订阅`VIP`后即可免费使用。

## FAQ

1. `XCharts`可以免费使用吗？  
   答：`XCharts`使用`MIT`协议，可以免费使用。也可以订阅`VIP`享受更多增值服务。

2. `XCharts`支持代码动态添加和修改数据吗？支持从`Excel`或数据库中获取数据吗？  
   答：`XCharts`提供了各种数据操作的接口，支持代码动态修改配置，添加和修改数据，但数据来源需要自己解析和获取，再调用`XCharts`的接口添加到图表。

3. 这个插件除了用在`Unity`，还能用在其他平台（如`Winform`或`WPF`）吗？  
   答：目前只支持在`Unity`平台使用。理论上任何支持`UGUI`的`Unity`版本都能运行`XCharts`。

4. 锯齿怎么解决？支持多大量级的数据？  
   答：`XCharts`是基于`UGUI`实现的，所以`UGUI`中碰到的问题，在`XCharts`中也会存在。比如锯齿问题，比如`Mesh`顶点数超`65535`的问题。这两个问题的解决可参考[问答16](Documentation~/zh/faq.md)和[问答27](Documentation~/zh/faq.md)。  
   由于`Mesh`的`65535`顶点数的限制，目前`XCharts`的单条`Line`支持约`2万`的数据量，当然开启采样可以支持更多数据的绘制，但同时也会更消耗CPU。

## Licenses

- [MIT License](https://github.com/XCharts-Team/XCharts/blob/master/LICENSE.md)
- 可免费商用，可二次开发
- 扩展图表需单独购买授权

## 日志

- [更新日志](Documentation~/zh/changelog.md)  

## 其他

- 邮箱：`monitor1394@gmail.com`  
- QQ群：XCharts交流群（`202030963`）  
- VIP群：XCharts技术支持VIP群（`867291970`）  
- 捐助、合作、订阅和技术支持：[☞ 看这里](Documentation~/zh/support.md)
