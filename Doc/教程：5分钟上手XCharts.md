# 教程：5分钟上手XCharts

[返回首页](../Readme.md)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts配置项手册](XCharts配置项手册.md)

## 获取和引入 XCharts

---

你可以通过以下两种方式获取 `XCharts` 。

1. 如果你只是想运行 `Demo` 查看效果，可以在 [Github](https://github.com/monitor1394/unity-ugui-XCharts)上的 [Clone or download](https://github.com/monitor1394/unity-ugui-XCharts/archive/master.zip)下载最新版本或去 [release](https://github.com/monitor1394/unity-ugui-XCharts/releases)下载稳定版本，将源码工程解压后用`unity`打开即可。
2. 如果你要将 `XCharts` 加入你的项目中，可以在[Github](https://github.com/monitor1394/unity-ugui-XCharts)上下载最新的 [release](https://github.com/monitor1394/unity-ugui-XCharts/releases)稳定版本，将 `XCharts-vx.x.x.unitypackage` 通过 Unity 导入到你的项目中，或下载 Source code 解压后将内部的 `XCharts` 文件夹拷贝到你项目的 `Assets` 目录下。

## 绘制一个简单的图表

---

1. 新建场景或在已有场景的 `Canvas` 下添加一个名为 `line_chart` 的 `GameObject`。
2. 选中 `line_chart`，通过菜单栏 `Component->XCharts->LineChart` 或者  `Inspector` 视图的 `Add Component` 添加 `LineChart` 脚本。设置 `line_chart` 的尺寸，一个简单的折线图就出来了。
3. 在 `Inspector` 视图下可以调整各个组件的参数，`Game` 视图会实时反馈调整的效果。各个组件的详细参数说明可查阅[XCharts配置项手册](XCharts配置项手册.md)。

[返回首页](../Readme.md)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts配置项手册](XCharts配置项手册.md)
