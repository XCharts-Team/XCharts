
# 更新日志

[branch-2.0](#branch-2.0)  
[v2.8.2](#v2.8.2)  
[v2.8.1](#v2.8.1)  
[v2.8.0](#v2.8.0)  
[v2.7.0](#v2.7.0)  
[v2.6.0](#v2.6.0)  
[v2.5.0](#v2.5.0)  
[v2.4.0](#v2.4.0)  
[v2.3.0](#v2.3.0)  
[v2.2.3](#v2.2.3)  
[v2.2.2](#v2.2.2)  
[v2.2.1](#v2.2.1)  
[v2.2.0](#v2.2.0)  
[v2.1.1](#v2.1.1)  
[v2.1.0](#v2.1.0)  
[v2.0.1](#v2.0.1)  
[v2.0.0](#v2.0.0)  
[v2.0.0-preview.2](#v2.0.0-preview.2)  
[v2.0.0-preview.1](#v2.0.0-preview.1)  
[v1.6.3](#v1.6.3)  
[v1.6.1](#v1.6.1)  
[v1.6.0](#v1.6.0)  
[v1.5.2](#v1.5.2)  
[v1.5.1](#v1.5.1)  
[v1.5.0](#v1.5.0)  
[v1.4.0](#v1.4.0)  
[v1.3.1](#v1.3.1)  
[v1.3.0](#v1.3.0)  
[v1.2.0](#v1.2.0)  
[v1.1.0](#v1.1.0)  
[v1.0.5](#v1.0.5)  
[v1.0.4](#v1.0.4)  
[v1.0.3](#v1.0.3)  
[v1.0.2](#v1.0.2)  
[v1.0.1](#v1.0.1)  
[v1.0.0](#v1.0.0)  
[v0.8.3](#v0.8.3)  
[v0.8.2](#v0.8.2)  
[v0.8.1](#v0.8.1)  
[v0.8.0](#v0.8.0)  
[v0.5.0](#v0.5.0)  
[v0.1.0](#v0.1.0)  

## branch-2.0

## v2.8.2

* (2022.08.15) 发布`v2.8.2`版本
* (2022.08.15) 增加`HeatmapChart`对自定义`Tooltip`的`formatter`的支持
* (2022.07.13) 修复`SerieLabel`刷新异常的问题 #215
* (2022.06.30) 优化`Radar`让`Tooltip`的层在`Indicator`之上

## v2.8.1

* (2022.05.06) 发布`v2.8.1`版本
* (2022.05.03) 增加`Legend`的`onLegendClick`,`onLegendEnter`和`onLegendExit`委托回调
* (2022.04.21) 修复`RingChart`的`Tooltip`异常的问题 #192
* (2022.04.21) 修复`DataZoom`设置`minShowNum`时可能会报错的问题

## v2.8.0

* (2022.04.10) 发布`v2.8.0`版本
* (2022.04.10) 增加`Debug`调试信息面板
* (2022.04.09) 修复`VisualMap`某些情况下不生效的问题
* (2022.04.08) 优化`XCharts`初始化 #190
* (2022.04.08) 修复`Radar`的颜色异常问题 #187
* (2022.03.24) 修复`Axis`的精度问题 #184

## v2.7.0

* (2022.03.20) 发布`v2.7.0`版本
* (2022.02.21) 修复`Chart`的`chartName`重复检测问题 #183
* (2022.02.17) 修复`Axis`的`SplitLine`可能会显示在坐标系外的问题 #181
* (2022.02.08) 修复数据全0时`{d}`显示不正确的问题
* (2022.02.08) 修复`YAxis`的`AxisLabel`的`onZero`参数不生效的问题
* (2022.01.06) 优化`Zebra`斑马柱图

## v2.6.0

* (2021.12.30) 发布`v2.6.0`版本
* (2021.12.21) 修复`Emphasis`不生效的问题
* (2021.12.17) 修复`MarkLine`在运行时`Label`不自动刷新显示隐藏的问题 #178
* (2021.12.10) 完善`Radar`的`AxisLine`和`SplitLine`可单独控制
* (2021.12.08) 修复`Serie`隐藏后`Y`轴最大值不刷新的问题
* (2021.12.04) 增加`Symbol`新类型：`EmptyRect`,`EmptyTriangle`,`EmptyDiamond`
* (2021.12.04) 增加`Symbol`的`Empty`区域颜色可通过`ItemStyle`的`backgroundColor`设置的支持
* (2021.12.03) 修复`Formatter`的`{c}`通配符不生效的问题 #175
* (2021.12.03) 修复`Axis`的`boundaryGap`某些情况下显示的问题 #174
* (2021.11.30) 修复`Serie`的`ignore`某些情况下绘制异常的问题 #173

## v2.5.0

* (2021.11.27) 发布`v2.5.0`版本
* (2021.11.27) 增加`Tooltip`的`positionFunction`的坐标设置委托函数
* (2021.10.29) 移除`XCharts`首次导入时`TextMeshPro`的相关设置
* (2021.10.29) 增加`Tooltip`对通配符`{e}`的支持 #170
* (2021.09.08) 完善`RadarChart`
* (2021.09.07) 修复`PieChart`渐出动画结束时`label`没有消失的问题 #168
* (2021.09.06) 修复`GaugeChart`用代码改变`splitNumber`不会刷新`label`的问题 #167

## v2.4.0

### 版本要点

* 折线图支持忽略数据的连线是断开还是连接
* 折线图支持轨迹匀速动画
* 其他优化和问题修复

### 日志详情

* (2021.08.31) 发布`v2.4.0`版本
* (2021.08.31) 优化`RingChart`的渐变效果
* (2021.08.31) 修复`DataZoom`拖动时`SerieLabel`不刷新的问题 (#165)
* (2021.08.25) 修复`Theme`主题切换无法保持到场景上的问题 (#166)
* (2021.08.24) 增加`Animation`的`alongWithLinePath`参数设置折线轨迹匀速动画
* (2021.08.22) 增加`Serie`的`ignoreLineBreak`参数设置忽略数据连线是否断开 (#164)
* (2021.08.22) 修复`Axis`在`DataZoom`开启时`Label`可能不更新的问题 (#164)
* (2021.08.15) 优化`Axis`的`AxisLabel`文本旋转设置，避免在DataZoom开启时偏移不一致 (#163)
* (2021.08.14) 增加`Legend`的`textAutoColor`设置文本颜色和`Serie`一致 (#163)
* (2021.08.12) 优化`BarChart`设置`Corner`时正负柱条圆角对称
* (2021.08.03) 优化`Serie`的数据全为0时Y轴不显示的问题
* (2021.07.29) 修复`Serie`开启`ignore`时被忽略的数据还会参与计算的问题 (#161)
* (2021.07.29) 完善`BarChart`的`Zebra`斑马柱图渐变支持
* (2021.07.26) 修复`TextMeshPro Enable`时找不到`XCharts`路径的问题 (#160)

## v2.3.0

### 版本要点

* 数据存储由`float`升级为`double`
* 新增`MarkLine`标线
* `Serie`下可用`IconStyle`统一配置图标
* `Label`支持用代码自定义显示样式
* `DataZoom`完善
* `PieChart`优化
* 问题修复

### 升级注意

由于数据类型升级为了`double`，`float`隐式转`double`可能有精度问题，所以建议之前为`float`的数据类型都手动改为`double`类型。

### 日志详情

* (2021.07.24) 发布`v2.3.0`版本
* (2021.07.22) 完善`SerieSymbol`以支持象形柱图`PictorialBarChart`扩展
* (2021.07.19) 修复`WdbGL`平台上`Tooltip`不显示的问题
* (2021.07.18) 增加`Serie`的`iconStyle`统一配置图标
* (2021.07.15) 增加`MarkLine`标线 (#142)
* (2021.07.09) 优化`BarChart`可通过`serieData.show`设置是否显示柱条
* (2021.07.08) 优化`data`数据存储类型由`float`全部转为`double`
* (2021.07.05) 修复`PieChart`的`avoidLabelOverlap`参数不生效的问题
* (2021.07.04) 修复`PieChart`选中扇区后鼠标区域指示不准确的问题
* (2021.07.04) 优化`PieChart`的`Label`为`Inside`时可通过参数`Margin`调节偏移
* (2021.07.01) 增加`DataZoom`的`supportInsideScroll`和`supportInsideDrag`参数设置坐标系内是否支持滚动和拖拽
* (2021.06.27) 增加`AxisLabel`的`showStartLabel`和`showEndLabel`参数设置首尾的`Label`是否显示
* (2021.06.27) 增加`AxisLabel`和`SerieLabel`的`formatter`委托方法 (#145)
* (2021.06.27) 增加`DataZoom`的`orient`参数设置水平或垂直样式
* (2021.06.21) 增加`IconStyle`的`autoHideWhenLabelEmpty`参数设置当`label`为空时是否自动隐藏图标

## v2.2.3

* (2021.06.20) 发布`v2.2.3`版本
* (2021.06.20) 修复`Axis`的`Icon`默认显示出来的问题

## v2.2.2

* (2021.06.18) 发布`v2.2.2`版本
* (2021.06.18) 优化`Axis`的`Label`为空时自动隐藏`Icon`
* (2021.06.17) 修复`maxCache`设置时实际数据个数多一个的问题
* (2021.06.17) 修复`TextMeshPro`的开启和关闭不及时刷新的问题
* (2021.06.17) 修复`XCharts`导入时总是弹出`XCharts Importer`的问题

## v2.2.1

* (2021.06.13) 发布`v2.2.1`版本
* (2021.06.13) 完善对多屏幕的支持
* (2021.06.12) 增加`IconStyle`的`align`参数设置图标的水平对齐
* (2021.06.12) 完善`Theme`主题导入 (#148)
* (2021.06.10) 修复`Unity`版本兼容问题 (#154)
* (2021.06.05) 完善`CandlestickChart`对`inverse`的支持 (#152)
* (2021.06.04) 修复`Gauge`在最小值为负数时指针指示位置异常的问题 (#153)

## v2.2.0

* (2021.05.30) 发布`v2.2.0`版本
* (2021.05.25) 完善`TextStyle`的`alignment`的支持 (#150)
* (2021.05.24) 修复`PieChart`数据全为`0`时`Label`无法正常显示的问题
* (2021.05.24) 修复`Add Serie`面板上`Serie Name`不生效的问题 (#149)
* (2021.05.23) 增加`TextStyle`的`autoWrap`设置是否自动换行
* (2021.05.23) 增加`TextStyle`的`autoAlign`设置是否让系统自动设置对齐方式
* (2021.05.23) 增加`AxisLabel`的`width`和`height`支持自定义文本的长宽
* (2021.05.23) 增加`Axis`的`iconStyle`和`icons`支持设置坐标轴标签显示图标
* (2021.05.20) 增加`Serie`和`Axis`的`insertDataToHead`参数控制数据插入头部还是尾部
* (2021.05.18) 优化`Editor`下的图表创建 #147
* (2021.05.16) 抽离`GanttChart`甘特图，通过扩展模块的方式来提供
* (2021.05.11) 增加`VisualMap`对`Piecewise`分段设置颜色的支持
* (2021.05.09) 修复`RingChart`无法设置环形的背景色的问题 #141
* (2021.05.08) 增加`LiquidChart`的方形水位图支持
* (2021.05.07) 优化`Axis`的刻度表现 #135
* (2021.05.01) 增加`Settings`中关于关于材质球设置的参数 #140
* (2021.05.01) 修复无法正确表示部分超大或超小数值的问题
* (2021.04.29) 修复`Radar`切换到`Circle`异常的问题 #139
* (2021.04.29) 增加`Settings`的`reversePainter`可设置`Serie`的绘制是否逆序
* (2021.04.28) 增加`SerieData`的`ignore`可忽略当前数据项
* (2021.04.28) 修复`DataZoom`下`AxisLabel`显示不准确的问题 #138
* (2021.04.26) 修复运行时动态创建图表会异常的问题 #137
* (2021.04.26) 增加`BarChart`绘制渐变边框的支持
* (2021.04.23) 增加自定义图表支持
* (2021.04.22) 修复`Gauge`的`AxisLabel`和文字颜色无法调整的问题
* (2021.04.13) 增加`AxisTick`的`ShowStartTick`和`ShowEndTick`参数控制第一个和最后一个刻度是否显示
* (2021.04.13) 完善多坐标轴的支持 #132

## v2.1.1

* (2021.04.13) 整理代码，清除`Warning`
* (2021.04.13) 修复`Unity`版本兼容问题
* (2021.04.12) 修复`Theme`重构后引起的`missing class attribute 'ExtensionOfNativeClass'`的问题 #131

## v2.1.0

* (2021.04.07) 发布`v2.1.0`版本
* (2021.03.31) 优化和重构`Theme`，解决引用相同或丢失的问题 #118
* (2021.03.30) 优化`Tooltip`支持设置不同的类目轴数据 #129
* (2021.03.29) 优化自定义绘制回调接口，增加`onCustomDrawBeforeSerie`、`onCustomDrawAfterSerie`和`onCustomDrawTop`
* (2021.03.25) 增加`GanttChart`甘特图
* (2021.03.22) 增加`Theme`的`Unbind`按钮用于解绑复制图表时的主题 #118
* (2021.03.18) 修复`Inspector`下`Foldout`后的勾选框无法选中的问题
* (2021.03.18) 修复`BarChart`在`0`数值时显示异常的问题
* (2021.03.14) 修复`Tooltip`的指示器在某些情况下指示位置不准的问题
* (2021.03.13) 优化`MultiComponentMode`开启后的编辑体验和组件刷新 #128
* (2021.03.10) 增加`CandlestickChart`K线图 #124
* (2021.03.06) 增加`PieChart`的`minAngle`参数支持设置最小扇区角度 #117
* (2021.03.05) 增加`Legend`几种内置图标的支持 #90
* (2021.03.02) 增加`DataZoom`对数值轴的支持 #71
* (2021.03.02) 优化`TextMeshPro`兼容问题 #125
* (2021.03.01) 修复隐藏和显示图表时部分已隐藏的节点显示异常的问题 #125

## v2.0.1

* (2021.02.26) 修复`HeatmapChart`的`Tooltip`指示的位置不准的问题 #123
* (2021.02.22) 修复`Unity`版本兼容问题
* (2021.02.21) 增加`Tooltip`的`ignoreDataShow`参数
* (2021.02.19) 修复图表在`LayoutGroup`控制下时可能显示错乱的问题 #121
* (2021.02.18) 修复`Radar`参数变更后无法自刷新的问题 #122

## v2.0.0

* (2021.02.05) 发布`v2.0.0`版本
* (2021.02.03) 修复`AxisLine`的`OnZero`对`YAxis`不生效的问题 #116
* (2021.01.29) 修复`Category`轴在`BoundaryGap`和`AlignWithLabel`为`True`时`Tick`显示效果不对的问题 #115
* (2021.01.25) 优化一些细节
* (2021.01.22) 修复`Inpsector`上部分属性显示异常的问题

## v2.0.0-preview.2

* (2021.01.21) 发布`v2.0.0-preview.2`版本
* (2021.01.21) 修复`Inpsector`上展开`AxisTick`时报错问题
* (2021.01.21) 修复打包兼容报错问题
* (2021.01.19) 增加`XChartsSettings`的`editorShowAllListData`参数配置是否在`Inspector`中显示列表的所有数据

## v2.0.0-preview.1

* (2021.01.19) 发布`v2.0.0-preview.1`版本

## v1.6.3

* (2021.01.02) 发布`v1.6.3`版本
* (2020.12.18) 修复`Animation`不启用时更新数据会导致图表一直刷新的问题
* (2020.12.01) 修复`Unity2020`上新创建的图表无法正常绘制的问题
* (2020.11.22) 发布`v1.6.2`版本
* (2020.11.22) 修复`LineChart`在数据过于密集时折线绘制异常的问题 #99
* (2020.11.21) 修复`LineChart`的刻度位置在`alignWithLabel`为`true`时可能异常的问题
* (2020.11.21) 修复`Unity5`兼容报错的问题
* (2020.11.13) 完善`RadarChart`的`Indicator`对`\n`换行的支持
* (2020.11.12) 修复`LineChart`当类型为`Smooth`时数据过密情况下报错的问题 #100
* (2020.10.22) 完善`HeatmapChart`中`VisualMap`对`Piecewise`的支持
* (2020.09.22) 修复`PieChart`边框大小不一致的问题

## v1.6.1

* (2020.09.19) 发布`v1.6.1`版本
* (2020.09.19) 增加`Remove All Chart Object`移除图表下的所有子节点（会自动重新初始化）
* (2020.09.18) 修复`SerieLabel`在点击图例隐藏`Serie`后还显示的问题#94
* (2020.09.18) 优化`Axis`的类目轴刻度和文本显示#93
* (2020.09.17) 修复`Package`导入时缺失`meta`文件导致失败的问题#92
* (2020.09.08) 优化`Legend`的颜色可自动匹配`ItemStyle`的自定义颜色#89
* (2020.09.05) 优化`LineChart`在不使用`XAxis1`时也能显示`XAxis1`
* (2020.08.29) 增加`LineStyle`的`toColor`和`toColor2`设置`LineChart`的水平渐变，取消通过`ItemStyle`设置`LineChart`的水平渐变
* (2020.08.29) 增加`PieChart`的`onPointerClickPie`点击扇形图扇区回调
* (2020.08.29) 增加`BarChart`的`onPointerClickBar`点击柱形图柱条回调

## v1.6.0

* (2020.08.24) 发布`v1.6.0`版本
* (2020.08.23) 重构代码，将与绘制相关的`Color`改为`Color32`，减少隐式转换（更新后会导致自定义的颜色丢失，可参考[问答29](https://github.com/monitor1394/unity-ugui-XCharts/blob/2.0/Assets/XCharts/Documentation/XCharts问答.md)进行升级）
* (2020.08.15) 优化`PieChart`绘制表现效果#85
* (2020.08.11) 增加`LiquidChart`数据变更动画#83
* (2020.08.11) 优化`PieChart`文本堆叠和引线效果#85
* (2020.08.08) 优化`LineChart`密集数据的绘制表现效果
* (2020.07.30) 增加`LineChart`可通过`VisualMap`或`ItemStyle`配置渐变#78
* (2020.07.25) 修复`LineChart`渐出动画绘制异常的问题#79
* (2020.07.25) 修复`LiquidChart`在`100%`时渐变色会失效的问题#80
* (2020.07.25) 增加`RadarChart`对`Tooltip`的`formatter`支持#77
* (2020.07.23) 增加`RingChart`环形渐变支持#75
* (2020.07.21) 增加`AxisLabel`和`SerieLabel`的`formatter`可单独配置数值格式化#68
* (2020.07.17) 增加`SerieAnimation`动画完成回调接口
* (2020.07.17) 优化`Chart`放在`ScrollView`下时不影响`ScrollView`的滚动和拖动
* (2020.07.16) 修复`Tooltip`在上层有遮挡还会显示的问题#74
* (2020.07.08) 优化`Scatter`类型`Serie`支持`Log`轴#70
* (2020.07.07) 修复`SerieLabel`位置错乱的问题
* (2020.07.07) 增加`Tooltip`的`offset`参数配置偏移
* (2020.07.06) 增加`LiquidChart`水位图
* (2020.07.01) 增加`PolarChart`极坐标图表

## v1.5.2

* (2020.06.25) 发布`v1.5.2`版本
* (2020.06.25) 修复`BarChart`在数值为`0`时还会绘制一小部分柱条的问题
* (2020.06.24) 修复`PieChart`在设置`clockwise`后绘制异常的问题#65
* (2020.06.23) 优化`LineChart`在峰谷差异过大时的绘制效果#64
* (2020.06.18) 修复`SerieLabel`在重新添加数据时可能不显示的问题
* (2020.06.17) 增加`SerieData`可单独设置`SerieSymbol`#66
* (2020.06.17) 修复`Check For Update`在`Unity 2018`部分版本报错的问题#63
* (2020.06.16) 增加`Serie`的`avoidLabelOverlap`参数避免饼图标签堆叠的情况#56
* (2020.06.15) 修复`SerieLabel`单独控制显示时可能错乱的问题
* (2020.06.11) 修复`Check warning`不生效的问题
* (2020.06.11) 修复`PieChart`和`RingChart`在数据占比很小时不显示的问题
* (2020.06.11) 增加`Tooltip`的`titleFormatter`支持配置占位符`{i}`表示忽略不显示标题
* (2020.06.07) 增加`Animation`的`customFadeInDelay`等自定义数据项延时和时长回调函数#58
* (2020.06.07) 优化`PieChart`在数据全为`0`时的显示为等份的效果#59
* (2020.06.04) 增加`SerieLabel`的`autoOffset`参数设置是否自动判断上下偏移
* (2020.06.04) 增加`Tooltip`的`alwayShow`参数设置触发后一直显示
* (2020.06.04) 优化`Tooltip`的`formatter`支持`{.1}`通配符
* (2020.06.04) 优化`Legend`数量过多时自动换行显示#53

## v1.5.1

* (2020.06.03) 发布`v1.5.1`版本
* (2020.06.02) 增加`Radar`的`ceilRate`，设置最大最小值的取整倍率
* (2020.06.02) 优化`Tooltip`的`formatter`，支持`{c1:1-1:f1}`格式配置
* (2020.05.31) 优化`Background`组件的生效条件，需要有单独的父节点（升级前需要自己处理旧的背景节点）
* (2020.05.30) 优化`PieChart`支持设置`ignoreValue`不显示指定数据
* (2020.05.30) 修复`RadarChart`为`Circle`时不绘制`SplitArea`的问题
* (2020.05.30) 优化`RadarChart`在设置`max`为`0`时可自动刷新最大值
* (2020.05.29) 修复`PieChart`设置`space`时只有一个数据时绘制异常的问题
* (2020.05.27) 修复调用`UpdateDataName()`接口时不会自动刷新的问题
* (2020.05.27) 优化`柱状图`的渐变色效果
* (2020.05.24) 修复`Axis`同时设置`boundaryGap`和`alignWithLabel`时`Tick`绘制异常的问题
* (2020.05.24) 优化版本更新检测

## v1.5.0

* (2020.05.22) 发布`v1.5.0`版本
* (2020.05.21) 增加`圆角柱图`支持渐变
* (2020.05.21) 增加`Background`背景组件
* (2020.05.19) 隐藏`Hierarchy`试图下自动生成的子节点
* (2020.05.18) 增加`chartName`属性可指定图表的别称，可通过`XChartMgr.Instance.GetChart(chartName)`获取图表
* (2020.05.16) 增加部分鼠标事件回调
* (2020.05.15) 优化自带例子，`Demo`改名为`Example`
* (2020.05.13) 增加`Serie`的`large`和`largeThreshold`参数配置折线图和柱状图的性能模式
* (2020.05.13) 完善Demo，增加性能演示Demo
* (2020.05.13) 优化性能，优化大数据绘制，重构代码
* (2020.05.04) 增加`numericFormatter`参数可配置数值格式化显示，去掉`forceENotation`参数
* (2020.04.28) 增加`自由锚点`支持，任意对齐方式
* (2020.04.23) 优化`ScatterChart`的`Tooltip`显示效果
* (2020.04.23) 增加`Tooltip`的`formatter`对`{.}`、`{c:0}`、`{c1:1}`的支持
* (2020.04.19) 优化`LineChart`折线图的区域填充渐变效果
* (2020.04.19) 增加`AxisLabel`的`onZero`参数可将`Label`显示在`0`刻度上
* (2020.04.19) 增加`Serie`和`AxisLabel`的`showAsPositiveNumber`参数将负数数值显示为正数
* (2020.04.18) 增加`Covert XY Axis`互换XY轴配置
* (2020.04.17) 增加`Axis`可通过`inverse`参数设置坐标轴反转
* (2020.04.16) 修复`Check warning`在`Unity2019.3`上的显示问题
* (2020.04.16) 修复`PieChart`在设置`Space`参数后动画绘制异常的问题

## v1.4.0

* (2020.04.11) 发布`v1.4.0`版本
* (2020.04.11) 增加`Check warning`检测功能
* (2020.04.09) 修复`Legend`初始化异常的问题
* (2020.04.08) 增加`PieChart`通过`ItemStyle`设置边框的支持
* (2020.03.29) 增加`Axis`的`ceilRate`设置最大最小值的取整倍率
* (2020.03.29) 增加`BarChart`可通过`itemStyle`的`cornerRadius`设置`圆角柱图`
* (2020.03.29) 增加`itemStyle`的`cornerRadius`支持圆角矩形
* (2020.03.24) 优化`Editor`参数编辑，兼容`Unity2019.3`及以上版本
* (2020.03.24) 增加`Serie`在`inspector`上可进行调整顺序、添加和删除操作
* (2020.03.23) 修复`Title`的`textStyle`和`subTextStyle`无效的问题
* (2020.03.22) 增加`BarChart`通过`barType`参数设置`胶囊柱状图`
* (2020.03.21) 增加`BarChart`和`HeatmapChart`可通过`ignore`参数设置忽略数据的支持
* (2020.03.21) 增加`ItemStyle`的`tooltipFormatter`参数可单独配置`Serie`的`Tooltip`显示
* (2020.03.20) 修复`X Axis 1`和`Y Axis 1`配置变更时不会自动刷新的问题
* (2020.03.20) 增加`AxisTick`的`width`参数可单独设置坐标轴刻度的宽度
* (2020.03.20) 增加`Serie`的`radarType`参数设置`多圈`和`单圈`雷达图
* (2020.03.17) 增加`BarChart`可用`ItemStyle`的`backgroundColor`设置数据项背景颜色
* (2020.03.17) 增加`SerieData`的`ItemStyle`和`Emphasis`可单独配置数据项样式的支持
* (2020.03.15) 重构`EmptyCricle`类型的`Symbol`边宽取自`ItemStyle`的`borderWidth`参数
* (2020.03.15) 重构`SerieSymbol`，去掉`color`和`opacity`参数，取自`ItemStyle`

## v1.3.1

* (2020.03.14) 发布`v1.3.1`版本
* (2020.03.14) 修复`LineChart`开启`ingore`时部分数据可能绘制异常的问题
* (2020.03.13) 修复`LineChart`的`label`偏移显示异常的问题

## v1.3.0

* (2020.03.11) 发布`v1.3.0`版本
* (2020.03.11) 优化`LineChart`的`label`偏移显示
* (2020.03.11) 优化清空并重新添加数据后的自动刷新问题
* (2020.03.10) 增加`LineChart`的普通折线图可通过`ignore`参数设置忽略数据的支持
* (2020.03.09) 增加`BarChart`可通过`ItemStyle`配置边框的支持
* (2020.03.08) 增加`RingChart`环形图
* (2020.03.05) 调整`Serie`的`arcShaped`参数重命名为`roundCap`
* (2020.03.05) 增加运行时和非运行时参数变更自动刷新图表
* (2020.02.26) 重构`Legend`图例，改变样式，增加自定义图标等设置
* (2020.02.23) 增加`BaseChart.AnimationFadeOut()`渐出动画，重构动画系统
* (2020.02.13) 增加`BaseChart.RefreshTooltip()`接口立即重新初始化`Tooltip`组件
* (2020.02.13) 增加`Tooltip`的`textStyle`参数配置内容文本样式，去掉`fontSize`和`fontStyle`参数
* (2020.02.13) 增加`TextStyle`的`lineSpacing`参数配置行间距
* (2020.02.11) 增加`Radar`的`splitLine`参数配置分割线，去掉`lineStyle`参数
* (2020.02.11) 增加`Tooltip`的`backgroundImage`参数配置背景图
* (2020.02.11) 增加`Tooltip`的`paddingLeftRight`和`paddingTopBottom`参数配置文字和边框的间距
* (2020.02.11) 增加`Tooltip`的`lineStyle`参数配置指示线样式
* (2020.02.11) 增加`Axis`的`splitLine`参数控制分割线，去掉`showSplitLine`和`splitLineType`参数（更新时需要重新设置分割线相关设置）
* (2020.02.10) 增加`Serie`的`clip`参数控制是否超出坐标系外裁剪（只适用于折线图、柱状图、散点图）
* (2020.02.08) 增加`SerieSymbol`的`gap`参数控制图形标记的外留白距离
* (2020.01.26) 增加`TextLimit`组件可以设置`AxisLabel`的文本自适应
* (2020.01.20) 优化`Tooltip`设置`itemFormatter`时显示系列颜色
* (2020.01.20) 增加`Radar`雷达图在`inspector`配置`areaStyle`的支持

## v1.2.0

* (2020.01.15) 发布`v1.2.0`版本
* (2020.01.15) 增加`AxisLabel`格式化为整数的支持（`{value:f0}`）
* (2020.01.15) 增加折线图对数轴`Log`的支持
* (2020.01.09) 修复当设置`DataZoom`的`minShowNum`时可能异常的问题
* (2020.01.08) 修复当设置`AxisLine`的`onZero`时刻度显示异常的问题
* (2020.01.08) 增加`Mask`遮罩遮挡支持
* (2019.12.21) 增加`Tooltip`的单个数据项和标题的字符串模版格式器
* (2019.12.21) 增加`DataZoom`的最小显示数据个数`minShowNum`
* (2019.12.20) 增加`Demo40_Radar.cs`雷达图代码操作`Demo`
* (2019.12.20) 添加`RadarChart`相关API接口

## v1.1.0

* (2019.12.17) 发布`v1.1.0`版本
* (2019.12.16) 修复`Overlay`模式下不显示`Tooltip`的问题
* (2019.12.15) 增加`Title`的`TextStyle`支持
* (2019.12.11) 修复`Legend`都隐藏时`Value轴`还显示数值的问题
* (2019.12.11) 修复`Series->Data->Size`重置为0后设置无效的问题
* (2019.12.06) 修复数据过小时`AxisLabel`直接科学计数法显示的问题
* (2019.12.04) 优化和完善数据更新`UpdateData`接口
* (2019.12.03) 增加圆环饼图的圆角支持，参数：`serie.arcShaped`
* (2019.12.03) 增加数据更新动画,参数：`serie.animation.dataChangeEnable`
* (2019.11.30) 增加`GaugeChart`仪表盘
* (2019.11.22) 修复`BarChart`清空数据重新赋值后`SerieLabel`显示异常的问题
* (2019.11.16) 修复`SerieLabel`设置`color`等参数不生效的问题

## v1.0.5

* (2019.11.12) 发布`v1.0.5`版本
* (2019.11.12) 修复`2018.3`以下版本打开项目报错的问题
* (2019.11.12) 增加`IconStyle`子组件，优化`SerieData`的图标配置
* (2019.11.11) 修复`Serie`的图标显示在上层遮挡`Label`的问题
* (2019.11.11) 修复饼图当数据过小时视觉引导线会穿透的的问题
* (2019.11.09) 修复饼图添加数据时`Label`异常的问题
* (2019.11.09) 优化结构，分离为`XCharts`和`XChartsDemo`两部分

## v1.0.4

* (2019.11.05) 发布`v1.0.4`版本
* (2019.11.05) 增加`Radar`雷达组件文本样式参数配置支持
* (2019.11.04) 修复`Unity2018.3`以下版本代码不兼容的问题
* (2019.11.04) 优化`SerieLabel`过多时引起的性能问题

## v1.0.3

* (2019.11.03) 发布`v1.0.3`版本
* (2019.11.03) 增加`Editor`快捷添加图表：`Hierarchy`试图下右键`XCharts->LineChart`
* (2019.11.02) 优化非配置参数变量命名和访问权限，简化`API`

## v1.0.2

* (2019.10.31) 发布`v1.0.2`版本
* (2019.10.31) 修复`prefab`预设制作报错的问题
* (2019.10.31) 增加访问主题组件API：`BaseChart.theme`

## v1.0.1

* (2019.10.26) 发布`v1.0.1`版本
* (2019.10.26) 修复版本检查功能在非运行时异常的问题
* (2019.10.26) 增加科学计数法显示数值的支持（查阅`forceENotation`参数）
* (2019.10.26) 增加`Axis`类目轴数据为空时的默认显示支持
* (2019.10.26) 增加`Axis`数值轴的最大最小值可设置为小数的支持，优化极小数图表的表现效果

## v1.0.0

* (2019.10.25) 发布`v1.0.0`版本
* (2019.10.23) 增加版本检测功能：`Component -> XCharts -> Check For Update`
* (2019.10.22) 增加`Package Manager`安装的支持
* (2019.10.20) 增加`Demo`首页`BarChart`的代码动态控制效果
* (2019.10.18) 增加`Serie`的`barType`参数，可配置`斑马柱状图`
* (2019.10.18) 增加`Serie`的`barPercentStack`参数，可配置`百分比堆叠柱状图`
* (2019.10.16) 增加`Demo`首页`LineChart`的代码动态控制效果
* (2019.10.15) 移除`Pie`组件，相关参数放到`Settings`中配置
* (2019.10.15) 增加`Demo`首页，展示代码动态控制效果
* (2019.10.14) 增加`RadarChart`、`ScatterChart`和`HeatmapChart`的起始动画效果
* (2019.10.14) 增加`SerieData`的`radius`自定义数据项的半径
* (2019.10.14) 增加`HeatmapChart`热力图
* (2019.10.14) 增加`VisualMap`视觉映射组件
* (2019.10.14) 增加`ItemStyle`数据项样式组件
* (2019.10.14) 增加`Emphasis`高亮样式组件
* (2019.10.10) 增加`Settings`全局参数配置组件，开放更多参数可配置
* (2019.10.09) 增加`AreaStyle`的高亮相关参数配置鼠标悬浮时高亮之前区域
* (2019.10.09) 优化`DataZoom`组件，增加双指缩放
* (2019.10.05) 增加`SerieLabel`的`LineType`给饼图配置不同类型的视觉引导线
* (2019.10.02) 增加`ScatterChart`同时对`Scatter`和`Line`的支持，实现折线图和散点图的组合图
* (2019.10.01) 重构代码，废弃`Series.series`接口，用`Series.list`代替
* (2019.10.01) 增加`customDrawCallback`自定义绘制回调
* (2019.10.01) 增加`SmoothDash`平滑虚线的支持
* (2019.09.30) 增加`Serie`采样类型`sampleType`的相关配置
* (2019.09.29) 增加`SerieSymbol`关于显示间隔的相关配置
* (2019.09.29) 重构代码：
  * `BaseChart`的`sampleDist`删除，`Serie`增加`lineSampleDist`
  * `BaseChart`的`minShowDataNumber`删除，`Serie`增加`minShow`
  * `BaseChart`的`maxShowDataNumber`删除，`Serie`增加`maxShow`
  * `BaseChart`的`maxCacheDataNumber`删除，`Serie`增加`maxCache`
  * `BaseChart`的`AddSerie()`接口参数调整
  * `BaseChart`的`UpdateData()`接口参数调整
  * `Axis`增加`maxCache`
* (2019.09.28) 增加`LineChart`和`BarChart`同时对`Line`、`Bar`类型`Serie`的支持，实现折线图和柱状图的组合图
* (2019.09.27) 增加`Axis`的`splitNumber`设置为`0`时表示绘制所有类目数据
* (2019.09.27) 增加`SampleDist`采样距离的配置，对过密的曲线开启采样，优化绘制效率
* (2019.09.27) 增加`XCharts问答`、`XChartsAPI接口`、`XCharts配置项手册`等文档
* (2019.09.26) 增加`AnimationReset()`重置初始化动画接口
* (2019.09.26) 优化`LineChart`的密集数据的曲线效果
* (2019.09.25) 优化`SerieData`的自定义图标不与`SerieLabel`关联，可单独控制是否显示
* (2019.09.24) 增加`SerieData`的自定义图标相关配置支持
* (2019.09.23) 增加`Formatter`配置`Axis`的`AxisLabel`的格式化输出
* (2019.09.23) 增加`Tooltip`的`FontSize`、`FontStyle`配置字体大小和样式
* (2019.09.23) 增加`Formatter`配置`SerieLabel`、`Legend`、`Tooltip`的格式化输出
* (2019.09.19) 增加`LineArrow`配置带箭头曲线
* (2019.09.19) 增加`Tooltip`的`FixedWidth`、`FixedHeight`、`MinWidth`、`MinHeight`设置支持
* (2019.09.18) 增加单条堆叠柱状图
* (2019.09.18) 增加虚线`Dash`、点线`Dot`、点划线`DashDot`、双点划线`DashDotDot`等类型的折线图支持
* (2019.09.17) 增加`AnimationEnabel()`启用或取消起始动画接口
* (2019.09.17) 增加`Axis`的`Interval`强制设置坐标轴分割间隔
* (2019.09.16) 去掉`Serie`中的旧版本数据兼容，不再支持`xData`和`yData`
* (2019.09.06) 增加`Animation`在重新初始化数据时自启动功能
* (2019.09.06) 增加`SerieLabel`的`Border`边框相关配置支持
* (2019.09.05) 增加`PieChart`的`Animation`初始化动画配置支持
* (2019.09.03) 增加`BarChart`的`Animation`初始化动画配置支持
* (2019.09.02) 增加`LineChart`的`Animation`初始化动画配置支持
* (2019.08.22) 增加`AxisName`的`Offset`偏移配置支持
* (2019.08.22) 增加`AxisLine`的`Width`配置支持
* (2019.08.20) 增加`SerieLabel`的背景宽高、文字边距、文字旋转的配置
* (2019.08.20) 增加`BarChart`的`Label`配置支持
* (2019.08.15) 增加`LineChart`的`Label`配置
* (2019.08.15) 重构`BarChart`，移除`Bar`组件，相关参数统一放到`Serie`中配置
* (2019.08.15) 重构`LineChart`，移除`Line`组件，相关参数统一放到`Serie`中配置

## v0.8.3

* (2019.08.15) 发布`v0.8.3`版本
* (2019.08.14) 修复`PieChart`的`Label`无法自动更新的问题
* (2019.08.13) 修复`UpdateData`接口无法更新数据的问题
* (2019.08.07) 增加`SerieSymbol`的`Color`、`Opacity`配置

## v0.8.2

* (2019.08.07) 发布`v0.8.2`版本
* (2019.08.07) 修复区域平滑折线图显示异常的问题
* (2019.08.06) 修复`serie`系列数超过调色盘颜色数时获取的颜色异常的问题
* (2019.08.06) 修复当`Axis`的`minMaxType`为`Custom`时`max`设置为`100`不生效的问题

## v0.8.1

* (2019.08.04) 发布`v0.8.1`版本
* (2019.08.04) 修复`Inspector`中修改数据不生效的问题

## v0.8.0

* (2019.08.04) 发布`v0.8.0`版本
* (2019.08.04) 优化`RadarChart`雷达图，增加多雷达图支持
* (2019.08.01) 增加代码API注释文档，整理代码
* (2019.07.29) 增加`Radius`、`Area`两种南丁格尔玫瑰图展示类型
* (2019.07.29) 增加`SerieLabel`配置饼图标签，支持`Center`、`Inside`、`Outside`等显示位置
* (2019.07.28) 增加`PieChart`多饼图支持
* (2019.07.23) 优化`Theme`主题的自定义，切换主题时自定义配置不受影响
* (2019.07.22) 增加`EffectScatter`类型的散点图
* (2019.07.21) 增加`ScatterChart`散点图
* (2019.07.21) 增加`SerieData`支持多维数据配置
* (2019.07.20) 增加`Symbol`配置`Serie`标志图形的显示
* (2019.07.19) 增加用代码添加动态正弦曲线的示例`Demo11_AddSinCurve`
* (2019.07.19) 优化`Legend`的显示和控制
* (2019.07.18) 优化抗锯齿，曲线更平滑
* (2019.07.18) 增加`Tooltip`指示器类型，优化显示控制
* (2019.07.15) 增加`Size`设置图表尺寸
* (2019.07.14) 增加`二维数据`支持，XY轴都可以设置为数值轴
* (2019.07.13) 增加`双坐标轴`支持，代码改动较大

## v0.5.0

* (2019.07.10) 发布`v0.5.0`版本
* (2019.07.09) 增加`AxisLine`配置坐标轴轴线和箭头
* (2019.07.03) 增加`AxisLabel`配置坐标轴`刻度标签`
* (2019.07.02) 增加`selected`等相关参数配置`PieChart`的选中效果
* (2019.06.30) 增加`SplitArea`配置坐标轴`分割区域`
* (2019.06.29) 增加`AxisName`配置坐标轴`名称`
* (2019.06.20) 增加`AreaAlpha`控制`RadarChart`的`Area`透明度
* (2019.06.13) 增加`DataZoom`实现`区域缩放`
* (2019.06.01) 增加`stepType`实现`LineChart`的`阶梯线图`
* (2019.05.29) 增加`InSameBar`实现`BarChart`的`非堆叠同柱`
* (2019.05.29) 增加`crossLabel`控制`Tooltip`的`十字准星指示器`
* (2019.05.24) 增加`堆叠区域图`
* (2019.05.16) 增加`AxisMinMaxType`控制坐标轴最大最小刻度
* (2019.05.15) 完善数据接口
* (2019.05.14) 增加X轴`AxisType.Value`模式支持
* (2019.05.13) 增加负数数值轴支持
* (2019.05.11) 增加自定义`Editor`编辑
* (2019.03.21) 增加`Tooltip`
* (2018.11.01) 增加`Default`、`Light`、`Dark`三种默认主题

## v0.1.0

* (2018.09.05) 发布`v0.1.0`版本
