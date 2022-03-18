# XCharts问答

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts配置项手册](XCharts配置项手册.md)

[QA 1：如何调整坐标轴与背景的边距？](#如何调整坐标轴与背景的边距)  
[QA 2：如何让初始动画重新播放？](#如何让初始动画重新播放)  
[QA 3：如何自定义折线图、饼图等数据项的颜色？](#如何自定义折线图_饼图等数据项的颜色)  
[QA 4：如何格式化文字，如我想给坐标轴标签加上单位？](#如何格式化文字_如我想给坐标轴标签加上单位)  
[QA 5：如何让柱形图的柱子堆叠显示？](#如何让柱形图的柱子堆叠显示)  
[QA 6：如何让柱形图的柱子同柱但不重叠？](#如何让柱形图的柱子同柱但不重叠)  
[QA 7：如何调整柱形图的柱子宽度和间距？](#如何调整柱形图的柱子宽度和间距)  
[QA 8：如何调整柱形图单个柱子的颜色？](#如何调整柱形图单个柱子的颜色)  
[QA 9：如何调整图表的对齐方式？](#如何调整图表的对齐方式)  
[QA 10：可以显示超过1000以上的大数据吗？](#可以显示超过1000以上的大数据吗)  
[QA 11：折线图可以画虚线、点线、点划线吗？](#折线图可以画虚线_点线_点划线吗)  
[QA 12：如何限定Y轴（Value轴）的值范围？](#如何限定Y轴的值范围)  
[QA 13：如何自定义数值轴刻度大小？](#如何自定义数值轴刻度大小)  
[QA 14：如何在数据项顶上显示文本？](#如何在数据项顶上显示文本)  
[QA 15：如何给数据项自定义图标？](#如何给数据项自定义图标)  
[QA 16：锯齿太严重，如何让图表更顺滑？](#锯齿太严重_如何让图表更顺滑)  
[QA 17：为什么鼠标移上图表 Tooltip 不显示？](#为什么鼠标移上图表Tooltip不显示)  
[QA 18：如何取消 Tooltip 的竖线？](#如何取消Tooltip的竖线)  
[QA 19：如何自定义 Tooltip 的显示内容？](#如何自定义Tooltip的显示内容)  
[QA 20：如何让Y轴（数值轴）显示多位小数？](#如何让Y轴显示多位小数)  
[QA 21：如何用代码动态更新数据？](#如何用代码动态更新数据)  
[QA 22：如何显示图例？为什么有时候图例无法显示？](#如何显示图例_为什么有时候图例无法显示)  
[QA 23：如何做成预设？](#如何做成预设)  
[QA 24：如何在图表上画点画线等自定义内容？](#如何在图表上画点画线等自定义内容)  
[QA 25：如何实现心电图类似的数据移动效果？](#如何实现心电图类似的数据移动效果)  
[QA 26：如何使用背景组件？有什么条件限制？](#如何使用背景组件_有什么条件限制)  
[QA 27：Mesh can not have more than 65000 vertices?](#Mesh_cannot_have_more_than_65000_vertices)  
[QA 28：为什么serie里设置的参数运行后又被重置了?](#为什么serie里设置的参数运行后又被重置了)  
[QA 29：为什么升级到1.6.0版本后很多自定义颜色丢失了?应该如何升级？](#为什么升级到1_6_0版本后很多自定义颜色丢失了_应该如何升级)  

## 如何调整坐标轴与背景的边距

答：`Grid`组件，可调整上下左右边距。

## 如何让初始动画重新播放

答：调用`AnimationReset()`接口。

## 如何自定义折线图_饼图等数据项的颜色

答：通过`Theme`的`colorPalette`调整，或者部分`Serie`下的`LineStyle`和`ItemStyle`。

## 如何格式化文字_如我想给坐标轴标签加上单位

答：通过`formatter`和`numericFormatter`参数，在`Legend`、`Axis`的`AxisLabel`、`Tooltop`、`Serie`的`Label`都提供该参数的配置。

## 如何让柱形图的柱子堆叠显示

答：设置`Serie`下的`stack`，`stack`相同的`serie`会堆叠显示在一个柱子上。

## 如何让柱形图的柱子同柱但不重叠

答：设置`Serie`下的`barGap`为`-1`，`stack`为空。

## 如何调整柱形图的柱子宽度和间距

答：调整`Serie`下的`barWidth`和`barGap`，多个`serie`时最后一个`serie`的`barWidth`和`barGap`有效。

## 如何调整柱形图单个柱子的颜色

答：可通过调整单个`Data`下的`ItemStyle`调整，也可以通过两个`serie`同柱不堆叠来实现，通过设置数据项为`0`来达到类似效果。

## 如何调整图表的对齐方式

答：默认为左下角对齐，暂不支持调整。可以通过包一层parent来辅助控制。（最新版本`1.5.0`及以上已支持任意锚点，可和做UI一样任意调整对其方式）。

## 可以显示超过1000以上的大数据吗

答：可以。但`UGUI`对单个`Graphic`限制`65000`个顶点，所以太多的数据不一定能显示完全。可通过设置采样距离`sampleDist`开启采样简化过密曲线。也可以通过设置一些参数来减少图表的顶点数有助于显示更多数据。如缩小图表的尺寸，关闭或减少坐标轴的客户端绘制，关闭`Serie`的`symbol`和`label`显示等。折线图的普通线图`Normal`比平滑线图`Smooth`占用顶点数更少。`1.5.0`以上版本可以设置`large`和`largeThreshold`参数来开启性能模式。

## 折线图可以画虚线_点线_点划线吗

答：可以。通过`Serie`下的`lineType`选择线条样式。当要显示的数据过多（成千以上）数据间过密时建议使用`Normal`或者`Step`样式。

## 如何限定Y轴的值范围

答：设置`Axis`下的`minMaxType`为`Custom`，自定义`min`和`max`。

## 如何自定义数值轴刻度大小

答：默认时通过`Axis`下的`splitNumer`进行自动划分。也可以设置`interval`自定义刻度大小。

## 如何在数据项顶上显示文本

答：通过设置`Serie`下的`Label`。

## 如何给数据项自定义图标

答：通过设置`Serie`的`data`下的数据项可单独设置`icon`相关参数。

## 锯齿太严重_如何让图表更顺滑

答：开启抗锯齿设置（在`Unity`里设置）。调整UI渲染模式为`Camera`模式，开启`MSAA`，设置`4`倍或更高抗锯齿。

## 为什么鼠标移上图表Tooltip不显示

答：确认`Tooltip`是否开启；确认父节点是否关闭了鼠标事件。

## 如何取消Tooltip的竖线

答：设置`Tooltip`的`type`为`None`。或者调整`lineStyle`的参数。

## 如何自定义Tooltip的显示内容

答：自定义总的内容可以通过`Tooltip`的`formatter`。如果只是想调整所有的`serie`的显示格式可以用`itemFormatter`和`titleFormatter`结合。如果想每个`serie`的显示格式不一样，可以定制`serie`的`itemStyle`里的`tooltipFormatter`。具体的用法请查阅[XCharts配置项手册](XCharts配置项手册.md)。

## 如何让Y轴显示多位小数

答：设置`Axis`下的`AxisLabel`中的`formatter`为`{value:f1}`或`{value:f2}`。`1.5.0`及以上版本通过`numericFormatter`设置。

## 如何用代码动态更新数据

答：请查阅`Example`下的代码，`Example13_LineSimple.cs`就是一个简单添加数据构建折线图的例子，其他`Demo`也都是通过代码控制不同的组件实现不同的功能，相关API请查看文档：[XChartsAPI接口](XChartsAPI.md)  。

## 如何显示图例_为什么有时候图例无法显示

答：首先，你的`serie`里的`name`需有值不为空。然后开启`Legend`显示，里面的`data`可以默认为空，表示显示所有的图例。如果你只想显示部分`serie`的图例，在`data`中填入要显示的图例的`name`即可。如果`data`中的值都不是系列的`name`，那图例就不会显示。

## 如何做成预设

答：请删除chart下所有的子组件再拖成预设。

## 如何在图表上画点画线等自定义内容

答：`XCharts`有自定义绘制回调`onCustomDraw`，具体可参考`Example12_CustomDrawing.cs`

## 如何实现心电图类似的数据移动效果

答：参考`Example`目录下的`Example_Dynamic.cs`。主要通过设置`maxCache`参数实现。`axis`和`serie`都设置相同的`maxCache`。`maxCache`可固定数据个数，当数据超过设定时会先删除第一个在添加新数据，实现数据移动效果。

## 如何使用背景组件_有什么条件限制

答：设置`background`组件的`show`为`true`。

## 区域折线图在用半透明颜色时有时候会一条叠加的线

答：这是区域折线图绘制的bug。可以用浅的不透的颜色替代半透明颜色。

## Mesh_cannot_have_more_than_65000_vertices

答：这是`UGUI`对单个`Graphic`的顶点数限制。`XCharts`是将图形绘制在单个`Graphic`上，所以也会有这个限制。解决的办法可以参考：[QA 10：可以显示超过1000以上的大数据吗？](#可以显示超过1000以上的大数据吗)  

## 为什么serie里设置的参数运行后又被重置了

答：检测下代码里是否调用了`RemoveData()`并重新添加`Serie`了。如果想保留`Serie`的配置可以只`ClearData()`，然后重新添加数据。

## 为什么升级到1_6_0版本后很多自定义颜色丢失了_应该如何升级

答：1.6.0版本为了减少隐式转换，将所有的绘制相关的`Color`都改为了`Color32`，所以会导致一些自定义的颜色的丢失。影响到的主要组件有：`ItemStyle`，`LineStyle`，`AreaStyle`，`Vessel`，`VisualMap`，`AxisSplitArea`，`AxisSplitLine`，`GaugeAxis`，`SerieLabel`等。可以用脚本[UpgradeChartColor.cs](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Editor/Tools/UpgradeChartColor.cs)进行升级。
升级步骤如下：
1. 备份好你的项目。
2. 先不升级`XCharts`，只下载或拷贝脚本[UpgradeChartColor.cs](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Editor/Tools/UpgradeChartColor.cs)放到旧项目的`Editor`下，由于旧版本可能不存在某些新版本才有的图表或者属性配置，可能会编译错误，需要处理按3，4步骤处理一下。
3. 若是由`itemStyle.toColor2`引起的编译报错，可将导出地方的`itemStyle.toColor2`改为`Color.clear`；导入的地方注释掉即可。
4. 若是由`LiquidChart`引起的编译报错，将所有涉及`LiquidChart`的地方都注释掉即可。
5. 编译通过后，通过`菜单栏->XCharts->ExportColorConfig`导出旧版本的颜色配置文件（配置文件默认保存到`Assets`下的`color.config`）。
6. 升级`XCharts`到最新版本。
7. 通过`菜单栏->XCharts->ImportColorConfig`将`color.config`导入即可恢复自定义的颜色（如果`color.config`不在升级后的项目的`Assets`下的话需要拷贝到此目录下）。

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts配置项手册](XCharts配置项手册.md)
