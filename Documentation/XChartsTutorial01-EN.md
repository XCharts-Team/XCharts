# 教程：5分钟上手 XCharts 3.0

[XCharts主页](https://github.com/XCharts-Team/XCharts)</br>
[XCharts问答](XChartsFAQ-ZH.md)</br>
[XChartsAPI接口](XChartsAPI-ZH.md)</br>
[XCharts配置项手册](XChartsConfiguration-ZH.md)

## 获取和导入 XCharts

1. 直接放入XCharts源码到项目

   下载好XCharts源码后，直接将XCharts目录拷贝到Unity项目工程的Assets目录下。

2. 通过`Assets/Import Package`导入XCharts

   下载好XCharts的.unitypackage文件后，打开Unity，菜单栏 Assets-->Import Package-->选中.unitypackage导入即可开始使用XCharts。

3. 通过`Package Manager`导入XCharts

   对于Unity 2018.3以上版本，可通过 Package Manager来导入XCharts，打开Package Manager后，通过 `Add package form git URL...`，输入XCharts的github URL: https://github.com/XCharts-Team/XCharts.git#master 稍等片刻后即可使用XCharts。

   也可以直接将package加入到`manifest.json`文件：打开`Packages`目录下的`manifest.json`文件，在`dependencies`下加入：

    ``` json
        "com.monitor1394.xcharts": "https://github.com/XCharts-Team/XCharts.git#master",
    ```

    如需更新`XCharts`，删除`manifest.json`文件（部分Unity版本可能是packages-lock.json文件）的`lock`下的`com.monitor1394.xcharts`相关内容即会重新下载编译。

## 添加一个简单图表

在`Hierarchy`试图下右键或菜单栏`GameObject`下拉：`XCharts->LineChart`：

![op_addchart](res/op_addchart.png)

即可快速创建一个简单的折线图出来：

![linechart1](res/linechart1.png)

## 添加多个Seire

如何在上图的基础上添加两条折线呢，只需`Add Serie`按钮，选择对应的`Serie`类型即可：
![op_addserie](res/op_addserie.png)
![linechart2](res/linechart2.png)

## 添加其他组件

默认图表没有`Legend`，需要`Legend`组件可通过`Add Component`按钮添加：
![op_addcomponent](res/op_addcomponent.png)

## 添加Serie组件

如果需要给折线图区域填充颜色，可单独给`Serie`添加`Component`：
![op_addseriecomponent](res/op_addseriecomponent.png)
![linechart3](res/linechart3.png)

## 添加SerieData组件

如果需要个性化定制每个数据项的配置，可以单独给每个`SerieData`添加`Component`。比如我们给第一个折线图的第二个数据单独显示`Label`：
![op_addseriedatacomponent](res/op_addseriedatacomponent.png)
![linechart4](res/linechart4.png)

## 更多组件和配置参数

功能越丰富就越需要更多的组件和参数支持。XCharts有多达几十种的主组件和子组件，每个组件有几个至几十个不等的可配置参数，以支持各种灵活而复杂的功能。

首次接触XCharts者可在 `Inspector` 视图下可以添加和调整各个组件，`Game` 视图会实时反馈调整的效果，以熟悉各种组件实现的效果。各个组件的详细参数说明可查阅[XCharts配置项手册](XChartsConfiguration-ZH.md)。

## 如何快速调整参数

`XCharts`是配置参数驱动。想要什么效果，你只需要去调整对应组件下的配置参数就可以，不需要去改`Hierarchy`试图下的节点，因为那些节点是由`XCharts`内部根据配置参数生成的。你改了也会变还原。

快速定位你想要改的效果对应的组件。这就需要对组件有一定的了解。比如我们想要让X轴的轴线末端显示箭头，如何定位？第一步，X轴定位到`XAxis0`；第二步，轴线定位到`AxisLine`；最后，再去看`AxisLine`组件下有没有这样的参数可以实现这个效果。

`XCharts`提供从全局`Theme`、系列`Serie`、单个数据项`SerieData`全方位的参数配置。优先级从大到小为：`SerieData`->`Serie`->`Theme`。以`ItemStyle`的颜色例，如果`SerieData`的`ItemStyle`配置有颜色值，则优先用这个颜色值。

## 用代码添加折线图

给`gameObject`挂上`LineChart`脚本：

```C#
var chart = gameObject.GetComponent<LineChart>();
if (chart == null)
{
    chart = gameObject.AddComponent<LineChart>();
    chart.Init();
}
```

调整大小：

```C#
chart.SetSize(580, 300);//代码动态设置尺寸，或直接操作chart.rectTransform，或直接在Inspector上改
```

设置标题：

```C#
var title = chart.GetOrAddChartComponent<Title>();
title.text = "Simple Line";
```

设置提示框和图例是否显示：

```C#
var tooltip = chart.GetOrAddChartComponent<Tooltip>();
tooltip.show = true;

var legend = chart.GetOrAddChartComponent<Legend>();
legend.show = false;
```

设置坐标轴：

```C#
var xAxis = chart.GetOrAddChartComponent<XAxis>();
xAxis.splitNumber = 10;
xAxis.boundaryGap = true;
xAxis.type =  Axis.AxisType.Category;

var yAxis = chart.GetOrAddChartComponent<YAxis>();
yAxis.type =  Axis.AxisType.Value;
```

清空默认数据，添加`Line`类型的`Serie`用于接收数据：

```C#
chart.RemoveData();
chart.AddSerie<Line>("line");
```

添加10个数据：

```C#
for (int i = 0; i < 10; i++)
{
    chart.AddXAxisData("x" + i);
    chart.AddData(0, Random.Range(10, 20));
}
```

这样一个简单的折线图就出来了：
![linechart-simple](res/linechart-simple.png)

如果一个Chart里面有多个系列时，则Axis的data只需要加一次，不要多个循环加重复了。记住：Axis的数据个数要和Serie的数据个数一致。

完整代码请查阅`Examples`：`Example13_LineSimple.cs`  

你还可以用代码控制更多的参数，`Examples`下还有更多的其他例子，凡是`Inspector`上看到的可配置的参数，都可以通过代码来设置。[XCharts配置项手册](XChartsConfiguration-ZH.md)里面的所有参数都是可以通过代码控制的。

另外，除非定制，建议调用[XChartsAPI接口](https://github.com/XCharts-Team/XCharts/master/Documentation/XChartsAPI-ZH.md)  里面的接口，这些接口内部会做一些关联处理，比如刷新图表等。如果自己调用了内部组件的接口，需要自己处理刷新等其他问题。

## 使用TextMeshPro

XCharts支持TextMeshPro，但默认是不开启的，需要自己手动切换。可通过一下两种方式开启和关闭：
![textmeshpro1](res/op_textmeshpro.png)
![textmeshpro2](res/op_textmeshpro2.png)
开启后需要设置好TextMeshPro要用的全局字体，也可以在主题Theme里单独设置：
![textmeshpro-font](res/op_textmeshpro3.png)
建议在项目初就规划好是否使用TextMeshPro，在有很多图表的情况下再切换，可能导致某些图表无法正常初始化，这时可能需要每个图表单独的使用`Remove All Chart Object`来清理让图表重新初始化。

## 用代码改图表参数

`Inspector`上看到的所有参数都可以用代码来修改，关键是要定位好你要改的参数是在组件上、还是serie上、还是在具体的数据项上改。

### 改主组件上的参数

需要先获取组件，再修改里面的参数：

```C#
var title = chart.GetOrAddChartComponent<Title>();
title.text = "Simple LineChart";
title.subText = "normal line";

var xAxis = chart.GetOrAddChartComponent<XAxis>();
xAxis.splitNumber = 10;
xAxis.boundaryGap = true;
xAxis.type = Axis.AxisType.Category;
```

### 改Serie的参数

新添加Serie：

```C#
var serie = chart.AddSerie<Pie>();
serie.center[0] = 0.5f;
serie.center[1] = 0.5f;
serie.radius[0] = 80;
serie.radius[1] = 90;
serie.animation.dataChangeEnable = true;
serie.roundCap = true;
```

已存在的Serie：

```C#
var serie = chart.GetSerie<Pie>();
serie.center[0] = 0.5f;
serie.center[1] = 0.5f;
serie.radius[0] = 80;
serie.radius[1] = 90;
serie.animation.dataChangeEnable = true;
serie.roundCap = true;
```

给Serie添加额外组件：

```C#
serie.AddExtraComponent<AreaStyle>();

var label = serie1.AddExtraComponent<LabelStyle>();
label.offset = new Vector3(0,20,0);
```

### 改数据项SerieData上的参数

```C#
var serieData = chart.AddData(0, 20);
//var serieData = serie.GetSerieData(0); //从已有数据中获取
serieData.radius = 10;

var itemStyle = serieData.GetOrAddComponent<ItemStyle>(); //启用SerieData的Component
itemStyle.color = Color.blue;

```

[XCharts主页](https://github.com/XCharts-Team/XCharts)</br>
[XCharts问答](XChartsFAQ-ZH.md)</br>
[XChartsAPI接口](XChartsAPI-ZH.md)</br>
[XCharts配置项手册](XChartsConfiguration-ZH.md)
