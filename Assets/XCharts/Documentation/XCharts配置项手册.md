# 配置项手册

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts问答](XCharts问答.md)

主组件：

* [Axis 坐标轴](#XAxis)  
* [Background 背景图](#Background)  
* [DataZoom 区域缩放](#DataZoom)  
* [Grid 网格](#Grid)  
* [Legend 图例](#Legend)  
* [Polar 极坐标](#Polar)  
* [Radar 雷达](#Radar)  
* [Series 系列](#Series)  
* [Serie-Line 折线图](#Serie-Line)  
* [Serie-Bar 柱形图](#Serie-Bar)  
* [Serie-Pie 饼图](#Serie-Pie)  
* [Serie-Radar 雷达图](#Serie-Radar)  
* [Serie-Scatter 散点图](#Serie-Scatter)  
* [Serie-Heatmap 热力图](#Serie-Heatmap)  
* [Serie-Gauge 仪表盘](#Serie-Gauge)  
* [Serie-Ring 环形图](#Serie-Ring)  
* [Serie-Liquid 水位图](#Serie-Liquid)  
* [Serie-Candlestick K线图](#Serie-Candlestick)  
* [Settings 设置](#Settings)
* [Theme 主题](#Theme)  
* [Tooltip 提示框](#Tooltip)  
* [Vessel 容器](#Vessel)  
* [Title 标题](#Title)  
* [VisualMap 视觉映射](#VisualMap)  

子组件：

* [AreaStyle 区域填充样式](#AreaStyle)  
* [AxisLabel 坐标轴刻度标签](#AxisLabel)  
* [AxisLine 坐标轴轴线](#AxisLine)  
* [AxisName 坐标轴名称](#AxisName)  
* [AxisSplitLine 坐标轴分割线条](#AxisSplitLine)  
* [AxisSplitArea 坐标轴分割区域](#AxisSplitArea)  
* [AxisTick 坐标轴刻度](#AxisTick)  
* [Emphasis 高亮样式](#Emphasis)  
* [ItemStyle 数据项样式](#ItemStyle)  
* [LineArrow 折线图箭头](#LineArrow)  
* [LineStyle 折线图样式](#LineStyle)  
* [Location 位置](#Location)  
* [MarkLine 标线](#MarkLine)  
* [SerieAnimation 动画](#SerieAnimation)  
* [SerieData 数据项](#SerieData)  
* [SerieLabel 图形上的文本标签](#SerieLabel)  
* [SerieSymbol 图形标记](#SerieSymbol)  
* [TextLimit 文本自适应](#TextLimit)  
* [TextStyle 文本样式](#TextStyle)  
* [IconStyle 图标样式](#IconStyle)  

## `Theme`

主题组件。主题用来配置图表的全局配色等其他参数。

相关参数：

* `theme`：内置主题类型。有`Default`、`Light`、`Dark`三种可选内置主题。
* `font`：所有文字的通用字体。
* `backgroundColor`：图表背景颜色。
* `titleTextColor`：主题的主标题文字颜色。
* `titleSubTextColor`：主题的副标题文字颜色。
* `legendTextColor`：图例的激活时文字颜色。
* `legendUnableColor`：图例的非激活时文字颜色。
* `axisTextColor`：坐标轴的文字颜色。
* `axisLineColor`：坐标轴的轴线颜色。
* `axisSplitLineColor`：坐标轴的分割线颜色，默认和轴线颜色一致。
* `tooltipBackgroundColor`：提示框的背景颜色。
* `tooltipFlagAreaColor`：提示框的阴影指示器的颜色。
* `tooltipTextColor`：提示框的文字颜色。
* `tooltipLabelColor`：提示框的十字指示器坐标轴标签的背景颜色。
* `tooltipLineColor`：提示框的指示线的颜色。
* `dataZoomTextColor`：区域缩放的文字颜色。
* `dataZoomLineColor`：区域缩放的线条颜色。
* `dataZoomSelectedColor`：区域缩放的选中区域颜色。
* `colorPalette`：调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。

相关接口：

* `GetColor(int index)`：获得调色盘对应系列索引的颜色值。
* `GetColorStr(int index)`：获得指定系列索引的十六进制颜色值字符串。
* `GetColor(string hexColorStr)`：将字符串颜色值转成Color。

## `Title`

标题组件，包含主标题和副标题。

相关参数：

* `show`：是否显示标题组件。
* `text`：主标题文本，支持使用 `\n` 换行。
* `textStyle`：主标题文本样式 [TextStyle](#TextStyle)。
* `subText`：副标题文本，支持使用 `\n` 换行。
* `subTextStyle`：副标题文本样式 [TextStyle](#TextStyle)。
* `itemGap`：主副标题之间的间距。
* `location`：标题显示位置 [Location](#Location)。

## `TitleStyle`

标题样式。

* `show`：是否显示标题组件。
* `rotate`：旋转。
* `offset`：偏移。
* `color`：颜色。
* `fontSize`：字体大小。
* `fontStyle`：字体风格。

## `Legend`

图例组件。图例组件展现了不同系列的标记，颜色和名字。可以通过点击图例控制哪些系列不显示。

相关参数：

* `show`：是否显示图例组件。
* `iconType`：图例图标类型。有以下几种类型：
  * `Auto`：自动匹配。
  * `Custom`：自定义图标。
  * `EmptyCircle`：空心圆。
  * `Circle`：实心圆。
  * `Rect`：正方形。可通过`Setting`的`legendIconCornerRadius`参数调整圆角。
  * `Triangle`：三角形。
  * `Diamond`：菱形。
* `selectedMode`：选择模式。控制是否可以通过点击图例改变系列的显示状态。默认开启图例选择，可以设成 `None` 关闭。有以下三种选择方式：
  * `Multiple`：多选。
  * `Single`：单选。
  * `None`：无法选择。
* `orient`：布局方式是横还是竖。
  * `Horizonal`：水平。
  * `Vertical`：垂直。
* `location`：图例的显示位置 [Location](#Location)。
* `itemWidth`：图例标记的图形宽度。
* `itemHeight`：图例标记的图形高度。
* `itemGap`：图例每项之间的间隔。横向布局时为水平间隔，纵向布局时为纵向间隔。
* `itemAutoColor`：图例标记的图形是否自动匹配颜色。
* `formatter`：图例内容字符串模版格式器。支持用 `\n` 换行。模板变量为图例名称 `{name}`
* `data`：图例的数据数组。数组项通常为一个字符串，每一项代表一个系列的 `name`（如果是饼图，也可以是饼图单个数据的 `name`）。如果 `data` 没有被指定，会自动从当前系列中获取。指定 `data` 时里面的数据项和 `serie` 匹配时才会生效。
* `icons`：自定义的图例标记图形。
* `textStyle`：图例的内容文本样式 [TextStyle](#TextStyle)。

相关接口：

* `ClearData()`：清空数据。
* `ContainsData(string name)`：是否包括指定名字的图例。
* `RemoveData(string name)`：移除指定名字的图例。
* `AddData(string name)`：添加图例项。
* `GetData(int index)`：获得指定索引的图例。
* `GetIndex(string legendName)`：获得指定图例的索引。

## `Polar`

极坐标系组件。极坐标系，可以用于散点图和折线图。每个极坐标系拥有一个角度轴和一个半径轴。

相关参数：

* `show`：是否显示直角坐标系网格。
* `center`：极坐标的中心点。数组的第一项是横坐标，第二项是纵坐标。当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
* `radius`：极坐标的半径。
* `backgroundColor`：极坐标的背景色，默认透明。

## `Radar`

* `shape`：雷达图绘制类型。
  * `Polygon`：多边形。
  * `Circle`：圆形。
* `positionType`：显示位置类型。
  * `Vertice`：显示在顶点处。
  * `Between`：显示在顶点之间。
* `radius`：雷达图的半径。
* `center`：雷达图的中心点。数组的第一项是横坐标，第二项是纵坐标。当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
* `ceilRate`：最大最小值向上取整的倍率。默认为0时自动计算。
* `splitNumber`：分割段数。默认为 `5`。
* `isAxisTooltip`：是否Tooltip显示轴线上的所有数据。只对Mutiple类型的Radar有效。
* `outRangeColor`：数值超出范围时显示的颜色。
* `connectCenter`：数值是否连线到中心点。
* `lineGradient`：数值线段是否渐变。
* `splitLine`：分割线条 [AxisSplitLine](#AxisSplitLine)。
* `splitArea`：分割区域 [AxisSplitArea](#AxisSplitArea)。
* `indicator`：是否显示指示器。
* `indicatorGap`：指示器和雷达的间距。
* `indicatorList`指示器列表 [Radar.Indicator](#Radar.Indicator)。

## `Radar.Indicator`

* `name`：指示器名称。
* `max`：指示器的最大值，默认为 0 无限制。
* `min`：指示器的最小值，默认为 0 无限制。
* `range`：指示器的正常值范围，当数值超出这个范围时显示`Radar`的`outRangeColor`颜色。
* `textStyle`：文本样式 [TextStyle](#TextStyle)。

## `TextLimit`

* `enable`：是否启用文本自适应。默认为`true`。
* `maxWidth`：设定最大宽度。默认为`0`表示自动获取，否则表示自定义。当文本的宽度大于该值进行裁剪。
* `gap`：两边留白像素距离。默认为`10`。
* `suffix`： 长度超出被裁剪后附加的后缀。默认为`...`。

## `TextStyle`

* `rotate`：旋转。
* `offset`：偏移。
* `color`：颜色。
* `backgroundColor`：背景颜色。
* `font`：字体。
* `fontSize`：字体大小。
* `fontStyle`：字体风格。
* `lineSpacing`：行间距。
* `alignment`：对齐方式。
* `autoWrap`：是否自动换行。
* `autoAlign`：是否让系统自动设置对齐方式。为true时系统自动选择对齐方式，为false时用alignment。

## `Tooltip`

提示框组件。

相关参数：

* `show`：是否显示提示框组件。
* `type`：提示框指示器类型。指示器类型有：
  * `Line`：线性指示器。
  * `Shadow`：阴影指示器。
  * `None`：无指示器。
  * `Corss`：十字准星指示器。坐标轴显示`Label`和交叉线。
* `formatter`：提示框内容字符串模版格式器。支持用 `\n` 换行。当`formatter`不为空时，优先使用`formatter`，否则使用`itemFormatter`。
  * 模板变量有`{.}`、`{a}`、`{b}`、`{c}`、`{d}`。
  * `{.}`为当前所指示或`index`为`0`的`serie`的对应颜色的圆点。
  * `{a}`为当前所指示或`index`为`0`的`serie`的系列名`name`。
  * `{b}`为当前所指示或`index`为`0`的`serie`的数据项`serieData`的`name`，或者类目值（如折线图的`X`轴）。
  * `{c}`为当前所指示或`index`为`0`的`serie`的`y`维（`dimesion`为`1`）的数值。
  * `{d}`为当前所指示或`index`为`0`的`serie`的`y`维（`dimesion`为`1`）百分比值，注意不带`%`号。
  * `{e}`为当前所指示或`index`为`0`的`serie`的数据项`serieData`的`name`。
  * `{.1}`表示指定`index`为`1`的`serie`对应颜色的圆点。
  * `{a1}`、`{b1}`、`{c1}`中的`1`表示指定`index`为`1`的`serie`。
  * `{c1:2}`表示索引为`1`的`serie`的当前指示数据项的第`3`个数据（一个数据项有多个数据，index为`2`表示第`3`个数据）。
  * `{c1:2-2}`表示索引为`1`的`serie`的第`3`个数据项的第`3`个数据（也就是要指定第几个数据项时必须要指定第几个数据）。
  * `{d1:2:f2}`表示单独指定了数值的格式化字符串为`f2`（不指定时用`numericFormatter`）。
  * 示例：`"{a}:{c}"`、`"{a1}:{c1:f1}"`、`"{a1}:{c1:1f1}"`
* `titleFormatter`：提示框标题内容的字符串模版格式器。支持用 `\n` 换行。仅当`itemFormatter`生效时才有效。可以单独设置占位符`{i}`表示忽略不显示标题内容。
* `itemFormatter`：提示框单个`serie`或数据项内容的字符串模版格式器。支持用 `\n`  换行。当`formatter`不为空时，优先使用`formatter`，否则使用`itemFormatter`。
* `numericFormatter`：标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式：`A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。
* `fixedWidth`：固定宽度。当同时设置 `fixedWidth` 和 `minWidth` 时，`fixedWidth` 比 `minWidth` 优先级高。
* `fixedHeight`：固定高度。当同时设置 `fixedHeight` 和 `minHeight` 时，`fixedHeight` 比 `minHeight` 优先级高。
* `minWidth`：最小宽度。当同时设置 `fixedWidth` 和 `minWidth` 时，`fixedWidth` 比 `minWidth` 优先级高。
* `minHeight`：最小高度。当同时设置 f`ixedHeight` 和 `minHeight` 时，`fixedHeight` 比 `minHeight` 优先级高。
* `paddingLeftRight`：文字和边框的左右边距。
* `paddingTopBottom`：文字和边框的上下边距。
* `backgroundImage`：提示框的背景图。
* `ignoreDataShow`：`(since v2.0.1)`是否显示忽略数据在`tooltip`上。 [defaut: `false`]。
* `ignoreDataDefaultContent`：忽略数据的默认显示字符信息。
* `alwayShow`：是否触发后一直显示。
* `offset`：`(since v1.5.3)`提示框相对于鼠标位置的偏移。

* `lineStyle`：指示器线条样式 [LineStyle](#LineStyle)。
* `textStyle`：显示内容文本样式 [TextStyle](#TextStyle)。

## `Vessel`

容器组件。一般用于LiquidChart。

相关参数：

* `show`：是否显示容器组件。
* `shape`：容器形状。
* `shapeWidth`：容器的厚度。
* `gap`：间隙。容器和液体的间隙。
* `center`：中心点。数组的第一项是横坐标，第二项是纵坐标。当值为0-1之间时表示百分比，设置成百分比时表示图表宽高最小值的百分比。
* `backgroundColor`：背景色，默认透明。
* `color`：容器颜色。当`autoColor`为`false`时生效。
* `autoColor`：是否自动颜色。默认`true`。为`true`时颜色会和`serie`一致。
* `radius`：半径。
* `smoothness`：开启或关闭缩放区域功能。
* `width`：容器的宽。shape为Rect时有效。
* `height`：容器的高。shape为Rect时有效。
* `cornerRadius`： 容器的圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。shape为Rect时有效。

## `DataZoom`

区域缩放组件。用于区域缩放，从而能自由关注细节的数据信息，或者概览数据整体，或者去除离群点的影响。  
目前只支持控制 `X` 轴。

相关参数：

* `enable`：开启或关闭缩放区域功能。
* `supportInside`：是否支持内置缩放。内置于坐标系中，可在坐标系上通过鼠标拖拽、鼠标滚轮、手指滑动（触屏上）来缩放或漫游坐标系。
* `supportSlider`：是否支持滑动条缩放。有单独的滑动条，可在滑动条上进行缩放或漫游。
* ~~`filterMode`：数据过滤，暂未启用。支持以下几种类型：~~
  * ~~`Filter`：当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只要有一个维度在数据窗口外，整个数据项就会被过滤掉。~~
  * ~~`WeakFilter`：当前数据窗口外的数据，被 过滤掉。即 会 影响其他轴的数据范围。每个数据项，只有当全部维度都在数据窗口同侧外部，整个数据项才会被过滤掉。~~
  * ~~`Empty`：当前数据窗口外的数据，被 设置为空。即 不会 影响其他轴的数据范围。~~
  * ~~`None`：不过滤数据，只改变数轴范围。~~
* ~~`xAxisIndex`：控制哪一个 `x` 轴。~~
* ~~`yAxisIndex`：控制哪一个 `y` 轴。~~
* `showDataShadow`：是否显示数据阴影。数据阴影可以简单地反应数据走势。
* `showDetail`：是否显示 `detail`，即拖拽时候显示详细数值信息。
* `zoomLock`：是否锁定选择区域（或叫做数据窗口）的大小。如果设置为 `true` 则锁定选择区域的大小，也就是说，只能平移，不能缩放。
* ~~`realtime`：拖动时，是否实时更新系列的视图。如果设置为 `false`，则只在拖拽结束的时候更新。~~
* `backgroundColor`：组件的背景颜色。
* `selectedAreaColor`：框选区域颜色。
* `bottom`：组件离容器下侧的距离。
* `top`：组件离容器上侧的距离。
* `left`：组件离容器左侧的距离。
* `right`：组件离容器右侧的距离。
* `height`：组件高度。
* `rangeMode`：取值类型是取绝对值还是百分比。
  * `Percent`：百分比。
* `start`：数据窗口范围的起始百分比。范围是：0 ~ 100。
* `end`：数据窗口范围的结束百分比。范围是：0 ~ 100。
* `scrollSensitivity`：缩放区域组件的敏感度。值越高每次缩放所代表的数据越多。
* `textStyle`：字体风格。
* `minShowNum`：最小显示数据个数。当DataZoom放大到最大时，最小显示的数据个数。

## `VisualMap`

视觉映射组件。用于进行『视觉编码』，也就是将数据映射到视觉元素（视觉通道）。

* `enable`：开启或关闭视觉映射功能。
* `show`：是否显示组件。如果设置为 `false`，不会显示，但是数据映射的功能还存在。
* `type`：组件类型。支持以下类型：
  * `Continuous`：连续型。
  * ~~`Piecewise`：分段型。~~
* ~~`selectedMode`：分段型的选择模式，支持以下模式：~~
  * ~~`Multiple`：多选。~~
  * ~~`Single`：单选。~~
* `autoMinMax`：自动设置`min`，`max`的值。
* `min`：允许的最小值。`autoMinMax`为`false`时必须指定。`[min, max]` 形成了视觉映射的『定义域』。
* `max`：允许的最大值。`autoMinMax`为`false`时必须指定。`[min, max]` 形成了视觉映射的『定义域』。
* `range`：指定手柄对应数值的位置。range 应在 min max 范围内。
* ~~`text`：两端的文本，如 ['High', 'Low']。~~
* ~~`textGap`：两端文字主体之间的距离，单位为px。~~
* `splitNumber`：对于连续型数据，自动平均切分成几段，默认为0时自动匹配inRange颜色列表大小。
* `calculable`：是否显示拖拽用的手柄（手柄能拖拽调整选中范围）。
* ~~`realtime`：拖拽时，是否实时更新。~~
* `itemWidth`：图形的宽度，即颜色条的宽度。
* `itemHeight`：图形的高度，即颜色条的高度。
* `borderWidth`：边框线宽，单位px。
* `dimension`：指定用数据的『哪个维度』，映射到视觉元素上。『数据』即 series.data。从1开始，默认为0取 data 中最后一个维度。
* `hoverLink`：打开 hoverLink 功能时，鼠标悬浮到 visualMap 组件上时，鼠标位置对应的数值 在 图表中对应的图形元素，会高亮。
* `orient`：布局方式是横还是竖。
* `location`：组件显示在图表中的位置。
* `inRange`：定义 在选中范围中 的视觉颜色。
* ~~`outOfRange`：定义 在选中范围外 的视觉颜色。~~

## `Grid`

网格组件。直角坐标系内绘图网格，单个 `grid` 内最多可以放置上下两个 `X` 轴，左右两个 `Y` 轴。可以在网格上绘制折线图，柱状图，散点图。目前最多只能存在一个 `grid` 组件。

相关参数：

* `show`：是否显示直角坐标系网格组件。
* `left`：组件离容器左侧的距离。
* `right`：组件离容器右侧的距离。
* `top`：组件离容器顶部的距离。
* `bottom`：组件离容器底部的距离。
* `backgroundColor`：背景颜色。

## `GaugeAxis`

仪表盘坐标轴。

* `axisLine`：坐标轴轴线样式。
* `splitLine`：坐标轴分割线样式。
* `axisTick`：坐标轴刻度样式。
* `axisLabel`：坐标轴刻度标签样式。
* `axisLabelText`：坐标轴刻度标签自定义内容。当内容为空时，`axisLabel`根据刻度自动显示内容，否则取自该列表定义的内容。

## `GaugePointer`

仪表盘指针。

* `width`：指针宽度。
* `length`：指针长度。当为`0-1`的浮点数时表示相对仪表盘半径的百分比。

## `XAxis`

直角坐标系 `grid` 中的 `X` 轴。单个 `grid` 组件最多只能放上下两个 `X` 轴。两个 `X` 轴存储在 `xAxes` 中。

相关参数：

* `show`：是否显示 `X` 轴。默认 `xAxes[0]` 为 `true`，`xAxes[1]` 为 `false`。
* `gridIndex`：坐标轴所在的 grid 的索引，默认位于第一个 grid。
* `type`：坐标轴类型。默认为 `Category`。支持以下类型：
  * `Value`：数值轴，用于连续数据。
  * `Category`：类目轴，适用于离散的类目数据，为该类型时必须通过 `data` 设置类目数据。
  * `Log`：对数轴，适用于对数数据。
* `position`：坐标轴在Grid中的位置`AxisPosition`。支持以下类型：
  * `Left`：左边。
  * `Right`：右边。
  * `Bottom`：底部。
  * `Top`：顶部。
* `offset`：坐标轴相对默认位置的偏移。在相同position有多个坐标轴时有用。
* `logBaseE`：对数轴是否以自然数 `e` 为底数，为 `true` 时 `logBase` 失效，只在对数轴（`type:'Log'`）中有效。
* `logBase`：对数轴的底数，只在对数轴（`type:'Log'`）中有效。
* `minMaxType`：坐标轴刻度最大最小值显示类型。默认为 `Default`。有以下三种类型：
  * `Default`：0-最大值。
  * `MinMax`：最小值-最大值。
  * `Custom`：自定义的最小值-最大值。
* `min`：设定的坐标轴刻度最小值，当 `minMaxType` 为 `Custom` 时有效。
* `max`：设定的坐标轴刻度最大值，当 `minMaxType` 为 `Custom` 时有效。
* `ceilRate`：最大最小值向上取整的倍率。默认为0时自动计算。
* `splitNumber`：坐标轴的分割段数。默认为 `5`。当 `splitNumber` 设为 `0` 时，表示绘制所有的类目数据。
* `interval`：强制设置坐标轴分割间隔。无法在类目轴中使用。设置该值时 `splitNumber` 无效。
* `boundaryGap`：坐标轴两边是否留白。默认为 `true`。
* `maxCache`：类目数据中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `inverse`：是否反向坐标轴。只在数值轴`Value`中有效。
* `insertDataToHead`：添加新数据时是在列表的头部还是尾部加入。
* `data`：类目数据，在类目轴（`type: 'Category'`）中有效。
* `icons`：刻度的图标，在类目轴（`type: 'Category'`）中有效。
* `axisLine`：坐标轴轴线相关配置 [AxisLine](#AxisLine)。
* `axisName`：坐标轴名称相关配置 [AxisName](#AxisName)。
* `axisTick`：坐标轴刻度相关配置 [AxisTick](#AxisTick)。
* `axisLabel`：坐标轴刻度标签 [AxisLabel](#AxisLabel)。
* `splitLine`：坐标轴轴线坐标轴分割线 [AxisSplitLine](#SplitLine)。
* `splitArea`：坐标轴轴线坐标轴分割区域 [AxisSplitArea](#AxisSplitArea)。
* `iconStyle`：坐标轴刻度图标的样式 [IconStyle](#IconStyle)。

相关接口：

* `ClearData()`：清空类目数据。
* `IsCategory()`：是否为类目轴。
* `IsValue()`：是否为数值轴。
* `AddData(string category, int maxDataNumber)`：添加一个类目到类目数据列表。

## `Background`

背景组件。
由于框架的局限性，背景组件使用有以下两个限制：
1：`chart`的父节点不能有布局控制类组件。
2：`chart`的父节点只能有当前`chart`一个子节点。
背景组件的开启需要通过接口来开启：`BaseChart.EnableBackground(bool flag)`。

相关参数：

* `show`：是否显示启用背景组件。但能否激活背景组件还要受其他条件限制。
* `image`：背景图。
* `imageType`：背景图填充类型。
* `imageColor`背景图颜色。默认`white`。
* `hideThemeBackgroundColor`：当背景组件启用时，是否隐藏主题中设置的背景色。

## `YAxis`

直角坐标系 `grid` 中的 `Y` 轴。单个 `grid` 组件最多只能放左右两个 `Y` 轴。两个 `Y` 轴存储在 `yAxes` 中。

相关参数：

* `show`：是否显示 `Y` 轴。默认 `yAxes[0]` 为 `true`，`yAxes[1]` 为 `false`。
* `type`：坐标轴类型。默认为 `Value`。有以下两种类型：
  * `Value`：数值轴，用于连续数据。
  * `Category`：类目轴，适用于离散的类目数据，为该类型时必须通过 `data` 设置类目数据。
* `minMaxType`：坐标轴刻度最大最小值显示类型。默认为 `Default`。有以下三种类型：
  * `Default`：0-最大值。
  * `MinMax`：最小值-最大值。
  * `Custom`：自定义的最小值-最大值。
* `min`：设定的坐标轴刻度最小值，当 `minMaxType` 为 `Custom` 时有效。
* `max`：设定的坐标轴刻度最大值，当 `minMaxType` 为 `Custom` 时有效。
* `splitNumber`：坐标轴的分割段数。默认为 `5`。
* `interval`：强制设置坐标轴分割间隔。无法在类目轴中使用。设置改值时 `splitNumber` 无效。
* `splitLineType`：分割线类型。默认为 `Dashed`。支持以下五种类型：
  * `None`：不显示分割线。
  * `Solid`：实线。
  * `Dashed`：虚线。
  * `Dotted`：点线。
  * `DashDot`：点划线。
  * `DashDotDot`：双点划线。
* `boundaryGap`：坐标轴两边是否留白。默认为 `false`。
* `data`：类目数据，在类目轴（`type: 'Category'`）中有效。
* `axisLine`：坐标轴轴线相关配置 [AxisLine](#AxisLine)。
* `axisName`：坐标轴名称相关配置 [AxisName](#AxisName)。
* `axisTick`：坐标轴刻度相关配置 [AxisTick](#AxisTick)。
* `axisLabel`：坐标轴刻度标签 [AxisLabel](#AxisLabel)。
* `splitArea`：坐标轴轴线坐标轴分割区域 [SplitArea](#SplitArea)。

相关接口：

* `ClearData()`：清空类目数据。
* `IsCategory()`：是否为类目轴。
* `IsValue()`：是否为数值轴。
* `AddData(string category, int maxDataNumber)`：添加一个类目到类目数据列表。

## `Series`

系列列表。每个系列通过 type 决定自己的图表类型。

相关参数：

* `show`：系列是否显示在图表上。
* `type`：系列的图表类型。有以下几种类型：
  * `Line`：折线图。
  * `Bar`：柱状图。
  * `Pie`：饼图。
  * `Radar`：雷达图。
  * `Scatter`：散点图。
  * `EffectScatter`：带有涟漪特效动画的散点图。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `stack`：数据堆叠。同个类目轴上系列配置相同的 `stack` 值后，后一个系列的值会在前一个系列的值上相加。
* `axisIndex`：使用的坐标轴轴的 `index`，在单个图表实例中存在多个坐标轴轴的时候有用。
* `radarIndex`：雷达图所使用的 `radar` 组件的 `index`。
* `minShow`：系列显示数据的最小索引。
* `maxShow`：系列显示数据的最大索引。
* `maxCache`：系列中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `sampleDist`采样的最小水平像素距离，默认为`0`时不采样。当两个数据点间的水平像素距离小于该值时，开启采样，保证两点间的水平像素距离不小于该值。
* `sampleType`：采样类型。当`sampleDist`大于`0`时有效。支持以下五种采样类型：
  * `Peak`：取峰值。当过滤点的平均值大于等于`sampleAverage`时，取最大值；反之取最小值。
  * `Average`：取过滤点的平均值。
  * `Max`：取过滤点的最大值。
  * `Min`：取过滤点的最小值。
  * `Sum`：取过滤点之和。
* `sampleAverage`：设定的采样平均值。当 `sampleType` 为 `Peak` 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为`0`时会实时计算所有数据的平均值。
* `areaStyle`：区域填充样式 [AreaStyle](#AreaStyle)。
* `symbol`：标记的图形 [SerieSymbol](#SerieSymbol)。
* `lineType`：折线图样式类型。支持以下十种类型：
  * `Normal`：普通折线图。
  * `Smooth`：平滑曲线。
  * `SmoothDash`：平滑虚线。
  * `StepStart`：阶梯线图：当前点。
  * `StepMiddle`：阶梯线图：当前点和下一个点的中间。
  * `StepEnd`：阶梯线图：下一个拐点。
  * `Dash`：虚线。
  * `Dot`：点线。
  * `DashDot`：点划线。
  * `DashDotDot`：双点划线。
* `lineStyle`：线条样式 [LineStyle](#LineStyle)。
* `barType`：柱状图类型。以下几种类型：
  * `Normal`：普通柱状图。
  * `Zebra`：斑马柱状图。
  * `Capsule`：胶囊柱状图。
* `barPercentStack`：是否百分比堆叠柱状图，相同 `stack` 的 `serie` 只要有一个 `barPercentStack` 为 `true`，则就显示成百分比堆叠柱状图。
* `barWidth`：柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。
* `barGap`：不同系列的柱间距离。为百分比（如 `'0.3f'`，表示柱子宽度的 `30%`）。如果想要两个系列的柱子重叠，可以设置 `barGap` 为 `'-1f'`。这在用柱子做背景的时候有用。在同一坐标系上，此属性会被多个 `'bar'` 系列共享。此属性应设置于此坐标系中最后一个 `'bar'` 系列上才会生效，并且是对此坐标系中所有 `'bar'` 系列生效。
* `barCategoryGap`：同一系列的柱间距离，默认为类目间距的20%，可设固定值。在同一坐标系上，此属性会被多个 `'bar'` 系列共享。此属性应设置于此坐标系中最后一个 `'bar'` 系列上才会生效，并且是对此坐标系中所有 `'bar'` 系列生效。
* `barZebraWidth`：斑马线的粗细。`barType` 为 `Zebra` 时有效。
* `barZebraGap`：斑马线的间距。`barType` 为 `Zebra` 时有效。
* `pieClickOffset`：鼠标点击时是否开启偏移，一般用在PieChart图表中。
* `pieRoseType`：是否展示成南丁格尔图，通过半径区分数据大小。
* `pieSpace`：饼图项间的空隙留白。
* `pieCenter`：饼图的中心点。
* `pieRadius`：饼图的半径。`radius[0]` 表示内径，`radius[1]` 表示外径。
* `roundCap`：启用圆弧效果。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `lineArrow`：折线图的箭头 [LineArrow](#LineArrow)。
* `insertDataToHead`：添加新数据时是在列表的头部还是尾部加入。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Line`

折线图系列。

* `show`：系列是否显示在图表上。
* `type`：`Line`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `stack`：数据堆叠。同个类目轴上系列配置相同的 `stack` 值后，后一个系列的值会在前一个系列的值上相加。
* `xAxisIndex`：使用的坐标轴X轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `yAxisIndex`：使用的坐标轴Y轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `minShow`：系列显示数据的最小索引。
* `maxShow`：系列显示数据的最大索引。
* `maxCache`：系列中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `sampleDist`采样的最小水平像素距离，默认为`0`时不采样。当两个数据点间的水平像素距离小于该值时，开启采样，保证两点间的水平像素距离不小于该值。
* `sampleType`：采样类型。当`sampleDist`大于`0`时有效。支持以下五种采样类型：
  * `Peak`：取峰值。当过滤点的平均值大于等于`sampleAverage`时，取最大值；反之取最小值。
  * `Average`：取过滤点的平均值。
  * `Max`：取过滤点的最大值。
  * `Min`：取过滤点的最小值。
  * `Sum`：取过滤点之和。
* `sampleAverage`：设定的采样平均值。当 `sampleType` 为 `Peak` 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为`0`时会实时计算所有数据的平均值。
* `clip`：是否裁剪超出坐标系部分的图形。
* `ignore`：是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`：忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `showAsPositiveNumber`：将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
* `large`：是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。缺点：优化后不能自定义设置单个数据项的样式，不能显示Label，折线图不绘制Symbol。
* `largeThreshold`：开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
* `areaStyle`：区域填充样式 [AreaStyle](#AreaStyle)。
* `symbol`：标记的图形 [SerieSymbol](#SerieSymbol)。
* `lineType`：折线图样式类型。支持以下十种类型：
  * `Normal`：普通折线图。
  * `Smooth`：平滑曲线。
  * `SmoothDash`：平滑虚线。
  * `StepStart`：阶梯线图：当前点。
  * `StepMiddle`：阶梯线图：当前点和下一个点的中间。
  * `StepEnd`：阶梯线图：下一个拐点。
  * `Dash`：虚线。
  * `Dot`：点线。
  * `DashDot`：点划线。
  * `DashDotDot`：双点划线。
* `lineStyle`：线条样式 [LineStyle](#LineStyle)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `lineArrow`：折线图的箭头 [LineArrow](#LineArrow)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Bar`

折线图系列。

* `show`：系列是否显示在图表上。
* `type`：`Bar`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `stack`：数据堆叠。同个类目轴上系列配置相同的 `stack` 值后，后一个系列的值会在前一个系列的值上相加。
* `xAxisIndex`：使用的坐标轴X轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `yAxisIndex`：使用的坐标轴Y轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `minShow`：系列显示数据的最小索引。
* `maxShow`：系列显示数据的最大索引。
* `maxCache`：系列中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `barType`：柱状图类型。以下几种类型：
  * `Normal`：普通柱状图。
  * `Zebra`：斑马柱状图。
  * `Capsule`：胶囊柱状图。
* `barPercentStack`：是否百分比堆叠柱状图，相同 `stack` 的 `serie` 只要有一个 `barPercentStack` 为 `true`，则就显示成百分比堆叠柱状图。
* `barWidth`：柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。
* `barGap`：不同系列的柱间距离。为百分比（如 `'0.3f'`，表示柱子宽度的 `30%`）。如果想要两个系列的柱子重叠，可以设置 `barGap` 为 `'-1f'`。这在用柱子做背景的时候有用。在同一坐标系上，此属性会被多个 `'bar'` 系列共享。此属性应设置于此坐标系中最后一个 `'bar'` 系列上才会生效，并且是对此坐标系中所有 `'bar'` 系列生效。
* `barCategoryGap`：同一系列的柱间距离，默认为类目间距的20%，可设固定值。在同一坐标系上，此属性会被多个 `'bar'` 系列共享。此属性应设置于此坐标系中最后一个 `'bar'` 系列上才会生效，并且是对此坐标系中所有 `'bar'` 系列生效。
* `barZebraWidth`：斑马线的粗细。`barType` 为 `Zebra` 时有效。
* `barZebraGap`：斑马线的间距。`barType` 为 `Zebra` 时有效。
* `clip`：是否裁剪超出坐标系部分的图形。
* `ignore`：是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`：忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `showAsPositiveNumber`：将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
* `large`：是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。缺点：优化后不能自定义设置单个数据项的样式，不能显示Label，折线图不绘制Symbol。
* `largeThreshold`：开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
* `symbol`：标记的图形 [SerieSymbol](#SerieSymbol)。
* `itemStyle`：柱条样式 [ItemStyle](#ItemStyle)。
* `areaStyle`：区域填充样式 [AreaStyle](#AreaStyle)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Pie`

饼图系列。

* `show`：系列是否显示在图表上。
* `type`：`Pie`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `pieRoseType`：南丁格尔玫瑰图类型，支持以下类型：
  * `None`：不展示成南丁格尔玫瑰图。
  * `Radius`：扇区圆心角展现数据的百分比，半径展现数据的大小。
  * `Area`：所有扇区圆心角相同，仅通过半径展现数据大小。
* `space`：扇区间隙。
* `center`：中心点坐标。当值为`0-1`的浮点数时表示百分比。
* `radius`：半径。`radius[0]`为内径，`radius[1]`为外径。当内径大于0时即为圆环图。
* `minAngle`：最小的扇区角度（0-360）。用于防止某个值过小导致扇区太小影响交互。
* `roundCap`：是否启用圆弧效果。
* `ignore`：是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制，对应的`Label`和`Legend`也不会显示。
* `ignoreValue`：忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `avoidLabelOverlap`：在饼图且标签外部显示的情况下，是否启用防止标签重叠策略，默认关闭，在标签拥挤重叠的情况下会挪动各个标签的位置，防止标签间的重叠。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Radar`

雷达图系列。

* `show`：系列是否显示在图表上。
* `type`：`Radar`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `radarType`：雷达图类型`RadarType`，支持以下类型：
  * `Multiple`：多圈雷达图。此时可一个雷达里绘制多个圈，一个`serieData`就可组成一个圈（多维数据）。
  * `Single`：单圈雷达图。此时一个雷达只能绘制一个圈，多个`serieData`组成一个圈，数据取自`data[1]`。
* `radarIndex`：雷达图所使用的 `radar` 组件的 `index`。
* `symbol`：标记的图形 [SerieSymbol](#SerieSymbol)。
* `lineStyle`：线条样式 [LineStyle](#LineStyle)。
* `itemStyle`：标记样式 [ItemStyle](#ItemStyle)。
* `areaStyle`：区域填充样式 [AreaStyle](#AreaStyle)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Scatter`

散点图系列。

* `show`：系列是否显示在图表上。
* `type`：`Scatter`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `clip`：是否裁剪超出坐标系部分的图形。
* `symbol`：标记的图形 [SerieSymbol](#SerieSymbol)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Heatmap`

热力图系列。

* `show`：系列是否显示在图表上。
* `type`：`Scatter`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `ignore`：是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`：忽略数据的默认值。默认值默认为`0`，当 `ignore` 为 `true` 才有效。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。

## `Serie-Gauge`

仪表盘系列。

* `show`：系列是否显示在图表上。
* `type`：`Gauge`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `gaugeType`：仪表盘类型，支持以下类型：
  * `Pointer`：指针类型。
  * `ProgressBar`：进度条类型。
* `center`：中心点坐标。当值为0-1的浮点数时表示百分比。
* `radius`：仪表盘半径。
* `min`：最小的数据值。映射到`startAngle`。
* `max`：最大的数据值。映射到`endAngle`。
* `startAngle`：仪表盘起始角度。和时钟一样，`12`点钟位置是`0`度，顺时针到`360`度。
* `endAngle`：仪表盘结束角度。和时钟一样，`12`点钟位置是`0`度，顺时针到`360`度。
* `splitNumber`：仪表盘刻度分割段数。
* `roundCap`：是否启用圆弧效果。
* `titleStyle`：仪表盘标题 [TitleStyle](#TitleStyle)。
* `gaugeAxis`： 仪表盘坐标轴 [GaugeAxis](#GaugeAxis)。
* `gaugePointer`：仪表盘指针 [GaugePointer](#GaugePointer)。
* `itemStyle`：仪表盘指针样式 [ItemStyle](#ItemStyle)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。仪表盘的数据一般只有一个，值通过`label`样式显示，`name`通过`titleStyle`样式显示。

## `Serie-Ring`

环形图系列。

* `show`：系列是否显示在图表上。
* `type`：`Ring`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `center`：中心点坐标。当值为`0-1`的浮点数时表示百分比。
* `radius`：仪表盘半径。
* `startAngle`：仪表盘起始角度。和时钟一样，`12`点钟位置是`0`度，顺时针到`360`度。
* `ringGap`：环形图的环间隙。
* `roundCap`：是否启用圆弧效果。
* `clockwise`：是否顺时针，默认为`true`。
* `titleStyle`：环形图中心标题 [TitleStyle](#TitleStyle)。
* `itemStyle`：环形图的圆环样式，包括设置背景颜色和边框等 [ItemStyle](#ItemStyle)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。环形图的数据只有二维，`data[0]`表示当前值，`data[1]`表示最大值。

## `Serie-Liquid`

水位图系列。

* `show`：系列是否显示在图表上。
* `type`：`Liquid`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `vesselIndex`：水位图所使用的`vessel`组件的`index`。
* `min`：最小值。
* `max`：最大值。
* `waveLength`：水波长。
* `waveHeight`：水波高。
* `waveSpeed`：水波移动速度。正数时左移，负数时右移。
* `waveOffset`：水波偏移。
* `itemStyle`：环形图的圆环样式，包括设置背景颜色和边框等 [ItemStyle](#ItemStyle)。
* `label`：图形上的文本标签 [SerieLabel](#SerieLabel)，可用于说明图形的一些数据信息，比如值，名称等。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，可以设置`1`到`n`维数据。水位图的数据一般只有一个，表示当前水位值，用`max`设置最大水位值。

## `Serie-Candlestick`

K线图系列。

* `show`：系列是否显示在图表上。
* `type`：`Candlestick`。
* `name`：系列名称。用于 `tooltip` 的显示，`legend` 的图例筛选。
* `xAxisIndex`：使用的坐标轴X轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `yAxisIndex`：使用的坐标轴Y轴的 `index`，在单个图表实例中存在多个坐标轴的时候有用。
* `minShow`：系列显示数据的最小索引。
* `maxShow`：系列显示数据的最大索引。
* `maxCache`：系列中可缓存的最大数据量。默认为`0`没有限制，大于0时超过指定值会移除旧数据再插入新数据。
* `clip`：是否裁剪超出坐标系部分的图形。
* `ignore`：是否开启忽略数据。当为 `true` 时，数据值为 `ignoreValue` 时不进行绘制。
* `ignoreValue`：忽略数据的默认值。默认值默认为0，当 `ignore` 为 `true` 才有效。
* `showAsPositiveNumber`：将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
* `large`：是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。缺点：优化后不能自定义设置单个数据项的样式，不能显示Label，折线图不绘制Symbol。
* `largeThreshold`：开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
* `itemStyle`：环形图的圆环样式，包括设置背景颜色和边框等 [ItemStyle](#ItemStyle)。
* `emphasis`：高亮样式 [Emphasis](#Emphasis)。
* `animation`：起始动画 [SerieAnimation](#SerieAnimation)。
* `data`：系列中的数据项 [SerieData](#SerieData) 数组，K线图至少需要4个维度的数组`[open, close, lowest, highest]`。

## `Settings`

全局参数设置组件。一般情况下可使用默认值，当有需要时可进行调整。

* `reversePainter`：Painter是否逆序。逆序时index大的serie最先绘制。
* `maxPainter`：默认最大Painter数据，当Serie数量大于maxPainter时会平均分配Painter。
* `basePainterMaterial`：Base Pointer 材质球，设置后会影响Axis等。
* `seriePainterMaterial`：Serie Pointer 材质球，设置后会影响所有Serie。
* `topPainterMaterial`：Top Pointer 材质球，设置后会影响Tooltip等。
* `lineSmoothStyle`：曲线平滑系数。通过调整平滑系数可以改变曲线的曲率，得到外观稍微有变化的不同曲线。
* `lineSmoothness`：曲线平滑度。值越小曲线越平滑，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
* `lineSegmentDistance`： 线段的分割距离。普通折线图的线是由很多线段组成，段数由该数值决定。值越小段数越多，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
* `cicleSmoothness`：圆形（包括扇形、环形等）的平滑度。数越小圆越平滑，但顶点数也会随之增加。

## `SerieAnimation`

* `enable`：是否开启动画系统。
* ~~`threshold`：是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。~~
* `fadeInDelay`：设定的渐入动画延时，单位毫秒。如果要设置单个数据项的延时，可以用代码定制：`customFadeInDelay`。
* `fadeInDuration`：设定的渐入动画时长，单位毫秒。如果要设置单个数据项的渐入时长，可以用代码定制：`customFadeInDuration`。
* `fadeOutDelay`：设定的渐出动画延时，单位毫秒。如果要设置单个数据项的延时，可以用代码定制：`customFadeOutDelay`。
* `fadeOutDuration`：设定的渐出动画时长，单位毫秒。如果要设置单个数据项的渐出时长，可以用代码定制：`customFadeOutDuration`。
* `dataChangeEnable`：是否开启数据变更动画。
* `dataChangeDuration`：数据变更动画时长，单位毫秒。
* `alongWithLinePath`：是否沿着线的轨迹进行匀速动画。

## `AreaStyle`

* `show`：是否显示区域填充。
* `origin`：区域填充的起始位置 `AreaOrigin`。有以下三种填充方式：
  * `Auto`：填充坐标轴轴线到数据间的区域。
  * `Start`：填充坐标轴底部到数据间的区域。
  * `End`：填充坐标轴顶部到数据间的区域。
* `color`：区域填充的颜色，默认取 `serie` 对应的颜色。如果 `toColor` 不是默认值，则表示渐变色的起点颜色。
* `toColor`：区域填充的渐变色的终点颜色。
* `highlightColor`：高亮时区域填充的颜色，默认取 `serie` 对应的颜色。如果 `highlightToColor` 不是默认值，则表示渐变色的起点颜色。
* `highlightToColor`：高亮时区域填充的渐变色的终点颜色。
* `opacity`：图形透明度。支持从 `0` 到 `1` 的数字，为 `0` 时不绘制该图形。
* `tooltipHighlight`：鼠标悬浮时是否高亮之前的区域。

## `AxisLabel`

* `show`：是否显示刻度标签。
* `interval`：坐标轴刻度标签的显示间隔，在类目轴中有效。`0`表示显示所有标签，`1`表示隔一个隔显示一个标签，以此类推。
* `inside`：刻度标签是否朝内，默认朝外。
* `margin`：刻度标签与轴线之间的距离。
* `formatter`：图例内容字符串模版格式器。支持用 `\n` 换行。模板变量为图例名称 `{value}`，数值格式化通过`numericFormatter`。
* `numericFormatter`：标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式：`A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。
* `showAsPositiveNumber`：将负数数值显示为正数。一般和`Serie`的`showAsPositiveNumber`配合使用。
* `onZero`：刻度标签显示在`0`刻度上。
* `width`：刻度标签的宽。当为0时系统自动设置。
* `height`：刻度标签的高。当为0时系统自动设置。
* `textLimit`：文本自适应 [TextLimit](#TextLimit)。只在类目轴中有效。
* `textStyle`：文本样式 [TextStyle](#TextStyle)。

## `AxisLine`

* `show`：是否显示坐标轴轴线。
* `onZero`： `X` 轴或者 `Y` 轴的轴线是否在另一个轴的 `0` 刻度上，只有在另一个轴为数值轴且包含 `0` 刻度时有效。
* `width`：坐标轴线线宽。
* `symbol`：是否显示箭头。
* `symbolWidth`：箭头宽。
* `symbolHeight`：箭头高。
* `symbolOffset`：箭头偏移。
* `symbolDent`：箭头的凹陷程度。

## `AxisName`

* `show`：是否显示坐标名称。
* `name`：坐标轴名称。
* `location`：坐标轴名称的位置。支持以下类型：
  * `Start`：坐标轴起始处。
  * `Middle`：坐标轴中间。
  * `End`：坐标轴末端。
* `textStyle`：显示内容文本样式 [TextStyle](#TextStyle)。

## `AxisSplitLine`

* `show`：是否显示坐标分割线。
* `interval`：分割线的显示间隔。`0` 表示显示所有分割线，`1` 表示隔一个隔显示一个分割线，以此类推。
* `lineStyle`：线条样式 [LineStyle](#LineStyle)。

## `AxisSplitArea`

* `show`：是否显示坐标分割区域。
* `color`：分隔区域颜色。分隔区域会按数组中颜色的顺序依次循环设置颜色。默认是一个深浅的间隔色。

## `AxisTick`

* `show`：是否显示坐标轴刻度。
* `alignWithLabel`：类目轴中在 `boundaryGap` 为 `true` 的时候有效，可以保证刻度线和标签对齐。
* `inside`：坐标轴刻度是否朝内，默认朝外。
* `length`：坐标轴刻度的长度。
* `width`：坐标轴刻度的宽度。默认为0时宽度和坐标轴一致。
* `showStartTick`：是否显示第一个刻度。
* `showEndTick`：是否显示最后一个刻度。

## `Emphasis`

* `show`：是否启用高亮样式。
* `label`：图形文本标签样式 [SerieLabel](#SerieLabel)。
* `itemStyle`：图形样式 [ItemStyle](#ItemStyle)。

## `ItemStyle`

* `show`：是否启用。
* `color`：颜色。对于K线图，对应阳线的颜色。
* `color0`：颜色。对于K线图，对应阴线的颜色。
* `toColor`：渐变颜色1。
* `toColor2`：渐变颜色2。只在折线图中有效。
* `backgroundColor`：背景颜色。
* `backgroundWidth`：背景的宽。
* `centerColor`：中心区域的颜色。如环形图的中心区域。
* `centerGap`：中心区域的间隙。如环形图的中心区域于最内环的间隙。
* `borderType`：边框的类型。
* `borderColor`：边框的颜色。对于K线图，对应阳线的边框颜色。
* `borderColor0`：边框的颜色。对于K线图，对应阴线的边框颜色。
* `borderWidth`：边框宽。
* `opacity`：透明度。
* `tooltipFormatter`：提示框单项的字符串模版格式器。具体配置参考`Tooltip`的`formatter`。
* `numericFormatter`：标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式：`A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。此字段优先于`SerieLabel`和`Tooltip`的`numericFormatter`。
* `cornerRadius`：圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。支持用0-1的浮点数设置百分比。

## `LineArrow`

* `show`：是否显示箭头。
* `position`：箭头显示位置。支持以下两种位置：
  * `End`：末端显示。最后一个数据上显示箭头。
  * `Start`：起始端显示。第一个数据上显示箭头。
* `width`：箭头宽。
* `height`：箭头长。
* `offset`：箭头偏移。默认箭头的中心点和数据坐标点一致，可通过 `offset` 调整偏移。
* `dent`：箭头的凹度。

## `LineStyle`

* `show`：是否显示线条。当作为子组件，它的父组件有参数控制是否显示时，改参数无效。
* `type`：线条类型。支持以下五种类型：
  * `None`：不显示分割线。
  * `Solid`：实线。
  * `Dashed`：虚线。
  * `Dotted`：点线。
  * `DashDot`：点划线。
  * `DashDotDot`：双点划线。
* `color`：线条颜色。默认和 `serie` 一致。
* `toColor`：线的渐变颜色（需要水平方向渐变时）。
* `toColor2`：线的渐变颜色2（需要水平方向三个渐变色的渐变时）。
* `width`：线条宽。
* `opacity`：线条的透明度。支持从 `0` 到 `1` 的数字，为 `0` 时不绘制该图形。

## `Location`

* `align`：对齐方式。有以下对齐方式。
  * `TopLeft`：左上角对齐。
  * `TopRight`：右上角对齐。
  * `TopCenter`：置顶居中对齐。
  * `BottomLeft`：左下对齐。
  * `BottomRight`：右下对齐。
  * `BottomCenter`：底部居中对齐。
  * `Center`：居中对齐。
  * `CenterLeft`：中部靠左对齐。
  * `CenterRight`：中部靠右对齐。
* `left`：离容器左侧的距离。
* `right`：离容器右侧的距离。
* `top`：离容器上侧的距离。
* `bottom`：离容器下侧的距离。

## `MarkLine`

* `show`：是否显示标线。
* `animation`：标线的动画样式。
* `data`：标线的数据项[MarkLineData](#MarkLineData)列表。当数据项的group为0时，每个数据项表示一条标线；当group不为0时，相同group的两个数据项分别表示标线的起始点和终止点来组成一条标线，此时标线的相关样式参数取起始点的参数。

## `MarkLineData`

* `name`：标注名称，将会作为文字显示。label的formatter可通过{b}显示名称，通过{c}显示数值。
* `type`：特殊的标注类型，用于标注最大值最小值等。。有以下标注类型：
  * `None`：无类型。此时通过
  * `Min`：最小值。`dimension`维度上数据的最小值。
  * `Max`：最大值。`dimension`维度上数据的最大值。
  * `Average`：平均值。`dimension`维度上数据的平均值。
  * `Median`：中位数。`dimension`维度上数据的中位数。
* `dimension`：当type为特殊类型时，指示从哪个维度的数据上计算特殊值。
* `xPosition`：相对原点的 x 坐标，单位像素。当type为None时有效。
* `yPosition`：相对原点的 y 坐标，单位像素。当type为None时有效。
* `xValue`：X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
* `yValue`：Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。

## `SerieData`

* `name`：数据项名称。
* `selected`：该数据项是否被选中。
* `radius`：自定义半径。可用在饼图中自定义某个数据项的半径。
* `enableIconStyle`: 是否启用单个数据项的图标设置。
* `iconStyle`：数据项图标样式。
* `enableLabel`：是否启用单个数据项的标签设置。
* `label`：单个数据项的标签设置。
* `enableItemStyle`：是否启用单个数据项的样式。
* `itemStyle`：单个数据项的样式设置。
* `enableEmphasis`：是否启用单个数据项的高亮样式。
* `emphasis`：单个数据项的高亮样式设置。
* `enableSymbol`：是否启用单个数据项的标记设置。
* `symbol`：单个数据项的标记设置。
* `data`：可指定任意维数的数值列表。对于折线图和柱状图，`data`其实是`size`为`2`的数组，`data[0]`是x的编号，`data[1]`是`y`的数值，默认显示`data[1]`。其他图表看需求而定是长度大于`2`的数组。可通过`Serie`的`showDataDimension`指定数据长度。

## `SerieLabel`

* `show`：是否显示文本标签。
* `position`：标签的位置。折线图时强制默认为 `Center`，支持以下 `5` 种位置：
  * `Outside`：饼图扇区外侧，通过视觉引导线连到相应的扇区。只在饼图种可用。
  * `Inside`：饼图扇区内部。只在饼图可用。
  * `Center`：在中心位置（折线图，柱状图，饼图）。
  * `Top`：顶部（柱状图）。
  * `Bottom`：底部（柱状图）。
* `formatter`：标签内容字符串模版格式器。支持用 `\n` 换行。模板变量有：`{a}`：系列名；`{b}`：数据名；`{c}`：数据值；`{d}`：百分比。示例：`{b}:{c}`。
* `numericFormatter`：标准数字格式字符串。用于将数值格式化显示为字符串。使用`Axx`的形式：`A`是格式说明符的单字符，支持`C`货币、`D`十进制、`E`指数、`F`顶点数、`G`常规、`N`数字、`P`百分比、`R`往返过程、`X`十六进制等九种。`xx`是精度说明，从`0`-`99`。
* `offset`：距离图形元素的偏移。
* `autoOffset`：是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。
* `backgroundWidth`：标签的背景宽度。一般不用指定，不指定时则自动是文字的宽度。
* `backgroundHeight`：标签的背景高度。一般不用指定，不指定时则自动是文字的高度。
* `paddingLeftRight`：标签文字和边框的左右边距。
* `paddingTopBottom`：标签文字和边框的上下边距。
* `line`：是否显示视觉引导线。在 `label` 位置 设置为 `'Outside'` 的时候会显示视觉引导线。
* `lineType`：视觉引导线类型。支持以下几种类型：
  * `BrokenLine`：折线。
  * `Curves`：曲线。
  * `HorizontalLine`：水平线。
* `lineColor`：视觉引导线自定义颜色。
* `lineWidth`：视觉引导线的宽度。
* `lineLength1`：视觉引导线第一段的长度。
* `lineLength2`：视觉引导线第二段的长度。
* `border`：是否显示边框。
* `borderWidth`：边框宽度。
* `borderColor`：边框颜色。
* `textStyle`：显示内容文本样式 [TextStyle](#TextStyle)。

## `SerieSymbol`

* `show`：是否显示标记。
* `type`：标记类型。支持以下六种类型：
  * `EmptyCircle`：空心圆。
  * `Circle`：实心圆。
  * `Rect`：正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。
  * `Triangle`：三角形。
  * `Diamond`：菱形。
  * `None`：不显示标记。
* `gap`：图形标记的外留白距离。
* `sizeType`：标记图形的大小获取方式。支持以下三种类型：
  * `Custom`：自定义大小。
  * `FromData`：通过 `dataIndex` 从数据中获取，再乘以一个比例系数 `dataScale` 。
  * `Callback`：通过回调函数 `sizeCallback` 获取。
* `size`：标记的大小。
* `selectedSize`：被选中的标记的大小。
* `dataIndex`：当 `sizeType` 指定为 `FromData` 时，指定的数据源索引。
* `dataScale`：当 `sizeType` 指定为 `FromData` 时，指定的倍数系数。
* `selectedDataScale`：当 `sizeType` 指定为 `FromData` 时，指定的高亮倍数系数。
* `sizeCallback`：当 `sizeType` 指定为 `Callback` 时，指定的回调函数。
* `selectedSizeCallback`：当 `sizeType` 指定为 `Callback` 时，指定的高亮回调函数。
* `color`：标记图形的颜色，默认和系列一致。
* `opacity`：图形标记的透明度。
* `startIndex`：开始显示图形标记的索引。
* `interval`：显示图形标记的间隔。`0`表示显示所有标签，`1`表示隔一个隔显示一个标签，以此类推。
* `forceShowLast`：是否强制显示最后一个图形标记。默认为 `false`。

## `IconStyle`

* `show`：是否显示图标。
* `layer`：显示在上层还是在下层。
* `sprite`：图标。
* `color`：颜色。
* `width`：图标的宽。
* `height`：图标的高。
* `offset`：位置偏移。

[返回首页](https://github.com/monitor1394/unity-ugui-XCharts)  
[XChartsAPI接口](XChartsAPI.md)  
[XCharts问答](XCharts问答.md)
