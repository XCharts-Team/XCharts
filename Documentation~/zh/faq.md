---
sidebar_position: 41
slug: /faq
---

# 常见问题

## FAQ 1：如何调整坐标轴与背景的边距？

调整`Grid`组件，可调整上下左右边距。

## FAQ 2：如何让初始动画重新播放？

调用`chart.AnimationReset()`接口。

## FAQ 3：如何自定义折线图饼图的颜色？

通过`Theme`的`colorPalette`调整，或者部分`Serie`下的`LineStyle`和`ItemStyle`。

## FAQ 4：如何给坐标轴上的文本加上单位？

通过`formatter`和`numericFormatter`参数，在`Legend`、`Axis`的`AxisLabel`、`Tooltop`、`Serie`的`Label`都提供该参数的配置。

## FAQ 5：如何让柱形图的柱子堆叠显示？

设置`Serie`下的`stack`，`stack`相同的`serie`会堆叠显示在一个柱子上。

## FAQ 6：如何让柱形图的柱子同柱但不重叠？

设置`Serie`下的`barGap`为`-1`，`stack`为空。

## FAQ 7：如何调整柱形图的柱子宽度和间距？

调整`Serie`下的`barWidth`和`barGap`，多个`serie`时最后一个`serie`的`barWidth`和`barGap`有效。

## FAQ 8：如何调整柱形图单个柱子的颜色？

可通过调整单个`Data`下的`ItemStyle`调整，也可以通过两个`serie`同柱不堆叠来实现，通过设置数据项为`0`来达到类似效果。

## FAQ 9：如何调整图表的对齐方式？

调整RectTransform的锚点，和UGUI的其他组件的用法一致。

## FAQ 10：可以显示超过1000以上的大数据吗？

可以。但`UGUI`对单个`Graphic`限制`65000`个顶点，所以太多的数据不一定能显示完全。可通过设置采样距离`sampleDist`开启采样简化过密曲线。也可以通过设置一些参数来减少图表的顶点数有助于显示更多数据。如缩小图表的尺寸，关闭或减少坐标轴的客户端绘制，关闭`Serie`的`symbol`和`label`显示等。折线图的普通线图`Normal`比平滑线图`Smooth`占用顶点数更少。`1.5.0`以上版本可以设置`large`和`largeThreshold`参数来开启性能模式。

## FAQ 11：折线图可以画虚线、点线、点划线吗？

可以。通过`Serie`下的`lineType`选择线条样式。当要显示的数据过多（成千以上）数据间过密时建议使用`Normal`或者`Step`样式。

## FAQ 12：如何限定Y轴的值范围？

设置`Axis`下的`minMaxType`为`Custom`，自定义`min`和`max`。

## FAQ 13：如何自定义数值轴刻度大小？

默认时通过`Axis`下的`splitNumer`进行自动划分。也可以设置`interval`自定义刻度大小。

## FAQ 14：如何在数据项顶上显示文本？

通过设置`Serie`下的`Label`。3.0版本需要先添加`LabelStyle`组件。

## FAQ 15：如何给数据项自定义图标？

通过设置`Serie`的`data`下的数据项可单独设置`icon`相关参数。

## FAQ 16：锯齿太严重，如何让图表更顺滑？

开启抗锯齿设置（在`Unity`里设置）。调整UI渲染模式为`Camera`模式，开启`MSAA`，设置`4`倍或更高抗锯齿。

## FAQ 17：为什么鼠标移上图表Tooltip不显示？

确认`Tooltip`是否开启；确认父节点是否关闭了鼠标事件。

## FAQ 18：如何取消Tooltip的竖线？

设置`Tooltip`的`type`为`None`。或者调整`lineStyle`的参数。

## FAQ 19：如何自定义Tooltip的显示内容？

自定义总的内容可以通过`Tooltip`的`formatter`。如果只是想调整所有的`serie`的显示格式可以用`itemFormatter`和`titleFormatter`结合。如果想每个`serie`的显示格式不一样，可以定制`serie`的`itemStyle`里的`tooltipFormatter`。具体的用法请查阅[XCharts配置项手册](https://xcharts-team.github.io/docs/configuration#Tooltip-itemFormatter)。

## FAQ 20：如何让y轴显示多位小数？

设置`Axis`下的`AxisLabel`中的`formatter`为`{value:f1}`或`{value:f2}`。`1.5.0`及以上版本通过`numericFormatter`设置。

## FAQ 21：如何用代码动态更新数据？

请查阅`Example`下的代码，`Example13_LineSimple.cs`就是一个简单添加数据构建折线图的例子，其他`Demo`也都是通过代码控制不同的组件实现不同的功能，相关API请查看文档：[XChartsAPI接口](api.md)  。

## FAQ 22：如何显示图例？为什么有时候图例无法显示？

首先，你的`serie`里的`name`需有值不为空。然后开启`Legend`显示，里面的`data`可以默认为空，表示显示所有的图例。如果你只想显示部分`serie`的图例，在`data`中填入要显示的图例的`name`即可。如果`data`中的值都不是系列的`name`，那图例就不会显示。

## FAQ 23：如何做成预设？

做成prefab前，执行一下`Rebuild Chart Object`重新刷新节点，避免有冗余的节点存在。

## FAQ 24：如何在图表上画点画线等自定义内容？

`XCharts`有自定义绘制回调`onCustomDraw`，具体可参考`Example12_CustomDrawing.cs`

## FAQ 25：如何实现心电图类似的数据移动效果？

参考`Example`目录下的`Example_Dynamic.cs`。主要通过设置`maxCache`参数实现。`axis`和`serie`都设置相同的`maxCache`。`maxCache`可固定数据个数，当数据超过设定时会先删除第一个在添加新数据，实现数据移动效果。

## FAQ 26：如何使用背景组件？有什么条件限制？

设置`background`组件的`show`为`true`。

## FAQ 27：Mesh can not have more than 65000 vertices？

这是`UGUI`对单个`Graphic`的顶点数限制。`XCharts`是将图形绘制在单个`Graphic`上，所以也会有这个限制。解决的办法可以参考：[FAQ 10：可以显示超过1000以上的大数据吗？](#可以显示超过1000以上的大数据吗)  

## FAQ 28：为什么serie里设置的参数运行后又被重置了？

检测下代码里是否调用了`RemoveData()`并重新添加`Serie`了。如果想保留`Serie`的配置可以只`ClearData()`，然后重新添加数据。

## FAQ 29：如何修改serie的symbol的颜色？

`Symbol` 的颜色是使用的 `ItemStyle` 的 `color`。

## FAQ 30：导入或更新XCharts时TMP报错如何处理？

XCharts默认时不开启TMP，所以asmdef上没有TMP的引用。当本地开启TMP后再更新XCharts可能会出现这个问题。可通过以下两种方式的任意一种解决：

1. 找到`XCharts.Runtime.asmdef`和`XCharts.Editor.asmdef`，手动加上 `TextMeshPro`的引用
2. 移除`PlayerSetting`中`Scripting Define Symbols`的`dUI_TextMeshPro`宏

`3.8.0`版本后增加[XCharts-Daemon](https://github.com/XCharts-Team/XCharts-Daemon)守护程序，将XCharts-Daemon导入项目后，在更新XCharts时守护程序会自动根据本地开启的TMP情况刷新asmdef，确保编译正常。

## FAQ 31：支持空数据吗？如何实现折线图断开的效果？

`Serie`的`data`是`double`类型，所以无法表示空数据。可通过开启`Serie`的`ignore`和指定`ignoreValue`来达到空数据的效果。也可以每个`SerieData`设置`ignore`参数。忽略数据后断开还是连接可设置`ignoreLineBreak`参数。

## FAQ 32：2.x版本升级3.x版本时常见的问题有哪些？

1. 出现：`'XCharts.Runtime.XChartsMgr' is missing the class attribute 'ExtensionOfNativeClass'!`的问题时，是因为3.x版本时不需要挂载XChartsMgr，直接删掉场景上的`_xcharts_`节点即可。

## FAQ 33：折线图如何两边不留空白而从0坐标开始显示？

设置X轴的`boundaryGap`参数。

## FAQ 34：如何设置部分Serie不显示Tooltip？

`serie`的`itemStyle`的`itemFormatter`设置为`-`可以忽略该`serie`的`tooltip`。

## FAQ 35：区域折线图在用半透明颜色时有时候会一条叠加的线

这是区域折线图绘制的bug。可以用浅的不透的颜色替代半透明颜色。
