
# 更新日志

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
* (2019.08.15) 发布`v0.8.3`版本
* (2019.08.14) 修复`PieChart`的`Label`无法自动更新的问题
* (2019.08.13) 修复`UpdateData`接口无法更新数据的问题
* (2019.08.07) 增加`SerieSymbol`的`Color`、`Opacity`配置
* (2019.08.07) 发布`v0.8.2`版本
* (2019.08.07) 修复区域平滑折线图显示异常的问题
* (2019.08.06) 修复`serie`系列数超过调色盘颜色数时获取的颜色异常的问题
* (2019.08.06) 修复当`Axis`的`minMaxType`为`Custom`时`max`设置为`100`不生效的问题
* (2019.08.04) 发布`v0.8.1`版本
* (2019.08.04) 修复从Inspector中修改数据不生效的问题
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
* (2018.09.05) 发布`v0.1.0`版本
