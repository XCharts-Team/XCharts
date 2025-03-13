
<h2 align="center">XCharts</h2>
<p align="center">
A powerful, easy-to-use, configurable charting and data visualization library for Unity.<br/>Unity数据可视化图表插件。<br/>
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

![XCharts](Documentation~/zh/img/xcharts.png)

XCharts 是一款基于 UGUI 的功能强大、简单易用的 Unity 数据可视化图表插件。它提供了丰富的图表类型和灵活的配置选项，帮助开发者快速实现专业级的数据可视化效果。支持折线图、柱状图、饼图、雷达图、散点图、热力图、环形图、K线图、极坐标、平行坐标等十多种常用的内置图表。提供3D饼图、3D柱图、3D金字塔、漏斗图、仪表盘、水位图、象形柱图、甘特图、矩形树图、桑基图、3D折线图、关系图等十多种高级扩展图表。

[XCharts 官方主页](https://xcharts-team.github.io)  
[XCharts 在线示例](https://xcharts-team.github.io/examples)  

[XCharts 教程：5分钟上手 XCharts](Documentation~/zh/tutorial01.md)  
[XCharts API文档](Documentation~/zh/api.md)  
[XCharts 常见问题](Documentation~/zh/faq.md)  
[XCharts 配置项手册](Documentation~/zh/configuration.md)  
[XCharts 更新日志](Documentation~/zh/changelog.md)  
[XCharts 订阅服务](Documentation~/zh/support.md)  

## 特性

- __纯代码绘制__：图表完全通过代码生成，无需额外贴图或 Shader 资源，轻量高效。
- __可视化配置__：提供直观的参数配置界面，支持实时预览效果，并可在运行时动态修改配置和数据。
- __高度定制化__：支持从主题、组件到数据项的全面参数设置，同时允许通过代码自定义绘制逻辑、回调函数及图表实现。
- __多内置图表__：支持线图、柱状图、饼图、雷达图、散点图、热力图、环形图、K线图、极坐标、平行坐标等多种常用的内置图表。
- __多扩展图表__：支持3D柱图、3D饼图、漏斗图、金字塔、仪表盘、水位图、象形柱图、甘特图、矩形树图、桑基图、3D折线图、关系图等多种高级扩展图表，满足复杂数据可视化需求。
- __多扩展组件__：支持多种实用 UI 组件，如表格、统计数值、滑动条、进度条等，增强图表交互性。
- __多图表组合__：支持在同一图表中组合显示多个相同或不同类型的图表，满足复杂场景需求。
- __多种坐标系__：支持直角坐标系、极坐标系、单轴等多种坐标系，适应不同数据展示需求。
- __丰富的组件__：提供标题、图例、提示框、标线、标域、数据区域缩放、视觉映射等常用组件，提升图表可读性。
- __多样式线图__：支持直线、曲线、虚线、面积图、阶梯线图等多种线图样式，满足不同数据趋势展示需求。
- __多样式柱图__：支持并列柱图、堆叠柱图、堆积百分比柱图、斑马柱图、胶囊柱图等多种柱状图样式。
- __多样式饼图__：支持环形图、玫瑰图、环形玫瑰图等多种饼图样式，直观展示数据占比。
- __自定义绘制__：提供强大的绘图 API，支持自定义绘制点、线、面等图形，满足个性化需求。
- __大数据绘制__：支持万级数据量绘制，优化性能表现；支持采样绘制，进一步提升大数据场景下的性能。
- __自定义主题__：支持主题定制、导入和导出，内置明暗两种默认主题，轻松适配不同应用场景。
- __动画和交互__：支持渐入、渐出、变更、新增等多种动画效果，以及数据筛选、视图缩放、细节展示等交互操作，提升用户体验。
- __第三方扩展__：无缝集成TexMeshPro和New Input System，扩展功能兼容性。
- __版本和兼容__：支持 Unity 5.6 及以上版本，兼容全平台运行。

## 截图

![内置图表](Documentation~/zh/img/readme_buildinchart.png)

![扩展图表](Documentation~/zh/img/readme_extendchart.png)

## 使用

- 导入`XCharts`的`unitypackage`或者源码到项目。建议也导入`XCharts`守护程序 [XCharts-Daemon](https://github.com/XCharts-Team/XCharts-Daemon)。
- 在`Hierarchy`视图下右键选择`XCharts->LineChart`，即可创建一个默认的折线图。
- 用`Inspector`视图下的`Add Serie`和`Add Main Component`按钮可以添加`Serie`和`组件`。
- 在`Inspector`视图下可以调整各个组件的参数，`Game`视图可看到实时效果。
- 更多细节，请看[【XCharts教程：5分钟上手教程】](Documentation~/zh/tutorial01.md)。
- 首次使用，建议先认真看一遍教程。

## 常见问题 (FAQ)

- __XCharts 可以免费使用吗？__  
  XCharts 基于 MIT 协议，核心功能完全免费。您也可以订阅 VIP 服务，享受更多高级功能和专属技术支持。

- __XCharts 支持代码动态添加和修改数据吗？__  
  是的，XCharts 提供了丰富的数据操作接口，支持代码动态修改配置和数据。但数据来源（如 Excel 或数据库）需要您自行解析后调用 XCharts 接口添加到图表中。

- __XCharts 支持哪些平台？__  
  XCharts 专为 Unity 平台设计，支持 Unity 5.6 及以上版本。理论上，任何支持 UGUI 的 Unity 版本均可运行 XCharts。目前不支持 Winform 或 WPF 等其他平台。

- __如何解决锯齿问题？XCharts 支持多大的数据量？__  
  XCharts 基于 UGUI 实现，因此 UGUI 的常见问题（如锯齿、Mesh 顶点数限制）在 XCharts 中也会存在。  
  - __锯齿问题__：可通过调整抗锯齿设置或使用更高分辨率解决。  
  - __数据量限制__：单条折线图（Line）支持约 2 万数据点，开启采样后可支持更多数据，但会消耗更多 CPU 资源。  
  更多解决方案请参考 [问答 16](Documentation~/zh/faq.md) 和 [问答 27](Documentation~/zh/faq.md)。

- __哪里可以查看 Demo？__  
  本仓库仅包含 XCharts 源码，Demo 示例请访问 [XCharts-Demo](https://github.com/XCharts-Team/XCharts-Demo) 仓库。您也可以在浏览器中查看 [在线 Demo](https://xcharts-team.github.io/examples/)。

## 日志

- 各版本的详细更新日志请查看 [更新日志](Documentation~/zh/changelog.md)  

## 扩展

- __[XCharts](https://github.com/XCharts-Team/XCharts)__ 核心功能，完全开源免费
- __[XCharts-Daemon](https://github.com/XCharts-Team/XCharts-Daemon)__ 守护程序，确保XCharts更新时的编译正常
- __[XCharts-Demo](https://github.com/XCharts-Team/XCharts-Demo)__ 官方示例（不包含扩展图表的示例）
- __[XCharts-Pro](https://github.com/XCharts-Team/XCharts-Pro)__ 专业版，包含所有扩展图表和扩展组件（需订阅 SVIP）
- __[XCharts-Pro-Demo](https://github.com/XCharts-Team/XCharts-Pro-Demo)__ 专业版官方示例（需订阅 SVIP）
- __[XCharts-UI](https://github.com/XCharts-Team/XCharts-UI)__ 扩展UI组件（需订阅 VIP）
- __[XCharts-Bar3DChart](https://github.com/XCharts-Team/XCharts-Bar3DChart)__ 3D柱图（需订阅 VIP）
- __[XCharts-FunnelChart](https://github.com/XCharts-Team/XCharts-FunnelChart)__ 漏斗图（需订阅 VIP）
- __[XCharts-GanttChart](https://github.com/XCharts-Team/XCharts-GanttChart)__ 甘特图（需订阅 VIP）
- __[XCharts-GaugeChart](https://github.com/XCharts-Team/XCharts-GaugeChart)__ 仪表盘（需订阅 VIP）
- __[XCharts-LiquidChart](https://github.com/XCharts-Team/XCharts-LiquidChart)__ 水位图（需订阅 VIP）
- __[XCharts-PictorialBarChart](https://github.com/XCharts-Team/XCharts-PictorialBarChart)__ 象形住图（需订阅 VIP）
- __[XCharts-Pie3DChart](https://github.com/XCharts-Team/XCharts-Pie3DChart)__ 3D饼图（需订阅 VIP）
- __[XCharts-PyramidChart](https://github.com/XCharts-Team/XCharts-PyramidChart)__ 3D金字塔（需订阅 VIP）
- __[XCharts-TreemapChart](https://github.com/XCharts-Team/XCharts-TreemapChart)__ 矩形树图（需订阅 VIP）
- __[XCharts-SankeyChart](https://github.com/XCharts-Team/XCharts-SankeyChart)__ 桑基图（需订阅 VIP）
- __[XCharts-Line3DChart](https://github.com/XCharts-Team/XCharts-Line3DChart)__ 3D折线图（需订阅 VIP）
- __[XCharts-GraphChart](https://github.com/XCharts-Team/XCharts-GraphChart)__ 关系图（需订阅 VIP）

## 许可

- __[MIT License](https://github.com/XCharts-Team/XCharts/blob/master/LICENSE.md)__：XCharts 核心库基于 MIT 协议，允许免费商用和二次开发。

- __扩展功能授权__：扩展图表和高级功能需订阅 VIP 或 SVIP 服务获得使用许可。

## 订阅

- __核心功能免费__：XCharts 核心库基于 MIT 协议完全开源，可免费使用。
- __增值服务__：为满足多样化需求，我们提供多种订阅服务，详情请查看 [订阅详情](Documentation~/zh/support.md)。
- __灵活选择__：订阅非强制，不影响核心功能使用。
- __按年付费__：订阅服务按年计费，到期后可选择续订。中断订阅后，将无法享受更新和技术支持服务。

## 其他

- 邮箱：`monitor1394@gmail.com`  
- QQ群：XCharts交流群（`202030963`）  
- VIP群：XCharts VIP群（`867291970`）  
- 支持与合作：[订阅与支持](Documentation~/zh/support.md)
