# XCharts问答

[返回首页](../Readme.md)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts配置项手册](XCharts配置项手册.md)

1. 如何调整坐标轴与背景的边距？  
   答：`Grid`组件，可调整上下左右边距。

2. 如何让初始动画重新播放？  
   答：调用`AnimationReset()`接口。

3. 如何自定义折线图、饼图等数据项的颜色？  
   答：通过`Theme`的`colorPalette`调整，或者部分`Serie`下的`LineStyle`和`ItemStyle`。

4. 如何格式化文字，如我想给坐标轴标签加上单位？  
   答：通过`formatter`参数，在`Legend`、`Axis`的`AxisLabel`、`Tooltop`、`Serie`的`Label`都提供改参数的配置。

5. 如何让柱形图的柱子堆叠显示？  
   答：设置`Serie`下的`stack`，`stack`相同的`serie`会堆叠显示在一个柱子上。

6. 如何让柱形图的柱子同柱但不重叠？  
   答：设置`Serie`下的`barGap`为`-1`，`stack`为空。

7. 如何调整柱形图的柱子宽度和间距？  
   答：调整`Serie`下的`barWidth`和`barGap`，多个`serie`时最后一个`serie`的`barWidth`和`barGap`有效。

8. 如何调整柱形图单个柱子的颜色？  
   答：目前暂不支持调整单子柱子的颜色，但可以通过两个`serie`同柱不堆叠来实现，通过设置数据项为`0`来达到类似效果。

9. 如何调整图表的对齐方式？  
   答：默认为左下角对齐，暂不支持调整。可以通过包一层parent来辅助控制。

10. 可以显示超过1000以上的大数据吗？  
   答：可以。但`UGUI`对单个`Graphic`限制`65000`个顶点，所以太多的数据不一定能显示完全。通过设置一些参数来减少图表的顶点数有助于显示更多数据。如缩小图表的尺寸，关闭或减少坐标轴的客户端绘制，关闭`Serie`的`symbol`和`label`显示等。折线图的普通线图`Normal`比平滑线图`Smooth`占用顶点数更少。

11. 折线图可以画虚线、点线、点划线吗？  
   答：可以。通过`Serie`下的`lineType`选择线条样式。当要显示的数据过多（成千以上）数据间过密时建议使用`Normal`或者`Step`样式。

12. 如何限定Y轴（Value轴）的值范围？  
   答：设置`Axis`下的`minMaxType`为`Custom`，自定义`min`和`max`。

13. 如何自定义数值轴刻度大小？  
   答：默认时通过`Axis`下的`splitNumer`进行自动划分。也可以设置`interval`自定义刻度大小。

14. 如何在数据项顶上显示文本？  
   答：通过设置`Serie`下的`Label`。

15. 如何给数据项自定义图标？  
   答：通过设置`Serie`的`data`下的数据项可单独设置`icon`相关参数。

16. 锯齿太严重，如何让图表更顺滑？  
   答：开启抗锯齿设置。调整UI渲染模式为Camera模式，开启MSAA，设置4倍或更高抗锯齿。锯齿只能减少难以避免，像素越高锯齿越不明显。

17. 为什么鼠标移上图表`Tooltip`不显示？  
   答：确认`Tooltip`是否开启。确认父节点是否关闭了鼠标事件。

18. 如何取消`Tooltip`的竖线？  
   答：设置`Tooltip`的`type`为`None`。

19. 如何让Y轴（数值轴）显示多位小数？  
   答：设置`Axis`下的`AxisLabel`中的`formatter`为`{value:f1}`或`{value:f2}`

[返回首页](../Readme.md)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts配置项手册](XCharts配置项手册.md)
