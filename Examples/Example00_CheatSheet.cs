using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LineChart))]
    public class Example00_CheatSheet : MonoBehaviour
    {
        private LineChart chart;
        private float speed = 100f;

        private void OnEnable()
        {
            StartCoroutine(CheatSheet());
        }

        IEnumerator CheatSheet()
        {
            StartCoroutine(InitChart());
            while (true)
            {
                StartCoroutine(ComponentTitle());
                yield return new WaitForSeconds(2);
                StartCoroutine(ComponentAxis());
                yield return new WaitForSeconds(2);
                StartCoroutine(ComponentGrid());
                yield return new WaitForSeconds(2);
                StartCoroutine(ComponentSerie());
                yield return new WaitForSeconds(4);
                StartCoroutine(ComponentLegend());
                yield return new WaitForSeconds(4);
                StartCoroutine(ComponentTheme());
                yield return new WaitForSeconds(4);
                StartCoroutine(ComponentDataZoom());
                yield return new WaitForSeconds(5);
                StartCoroutine(ComponentVisualMap());
                yield return new WaitForSeconds(3);
            }
        }

        IEnumerator InitChart()
        {
            chart = gameObject.GetComponent<LineChart>();

            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = "术语解析-组件";

            var grid = chart.EnsureChartComponent<GridCoord>();
            grid.bottom = 30;
            grid.right = 30;
            grid.left = 50;
            grid.top = 80;

            chart.RemoveChartComponent<VisualMap>();

            chart.RemoveData();

            chart.AddSerie<Bar>("Bar");
            chart.AddSerie<Line>("Line");

            for (int i = 0; i < 8; i++)
            {
                chart.AddXAxisData("x" + (i + 1));
                chart.AddData(0, Random.Range(10, 100));
                chart.AddData(1, Random.Range(30, 100));
            }
            yield return null;
        }

        IEnumerator ComponentTitle()
        {
            chart.EnsureChartComponent<Title>().text = "术语解析 - 组件";
            chart.EnsureChartComponent<Title>().subText = "Title 标题：可指定主标题和子标题";
            chart.EnsureChartComponent<XAxis>().show = true;
            chart.EnsureChartComponent<YAxis>().show = true;
            chart.EnsureChartComponent<Legend>().show = false;
            chart.series[0].show = false;
            chart.series[1].show = false;

            for (int i = 0; i < 4; i++)
            {
                chart.EnsureChartComponent<Title>().show = !chart.EnsureChartComponent<Title>().show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.EnsureChartComponent<Title>().show = true;
            chart.RefreshChart();
        }

        IEnumerator ComponentAxis()
        {
            chart.EnsureChartComponent<Title>().subText = "Axis 坐标轴：配置X和Y轴的轴线、刻度、标签等样式外观配置";
            chart.series[0].show = false;
            chart.series[1].show = false;
            var xAxis = chart.EnsureChartComponent<XAxis>();
            var yAxis = chart.EnsureChartComponent<YAxis>();
            for (int i = 0; i < 4; i++)
            {
                xAxis.show = !xAxis.show;
                yAxis.show = !yAxis.show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            xAxis.show = true;
            yAxis.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentGrid()
        {
            chart.EnsureChartComponent<Title>().subText = "Grid 网格：调整坐标系边距和颜色等";
            var grid = chart.EnsureChartComponent<GridCoord>();
            for (int i = 0; i < 4; i++)
            {
                grid.backgroundColor = i % 2 == 0 ? Color.clear : Color.grey;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            grid.backgroundColor = Color.clear;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentSerie()
        {
            chart.EnsureChartComponent<Title>().subText = "Serie 系列：调整坐标系边距和颜色等";
            chart.series[0].show = true;
            chart.series[1].show = true;
            chart.AnimationReset();
            chart.RefreshChart();
            yield return new WaitForSeconds(1.2f);
            for (int i = 0; i < 4; i++)
            {
                chart.series[0].show = !chart.series[0].show;
                chart.series[1].show = !chart.series[1].show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.series[0].show = true;
            chart.series[1].show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentLegend()
        {
            chart.EnsureChartComponent<Title>().subText = "Legend 图例：展示不同系列的名字和颜色，可控制系列显示等";
            var legend = chart.EnsureChartComponent<Legend>();
            legend.show = true;
            var grid = chart.EnsureChartComponent<GridCoord>();
            grid.top = 80;
            legend.location.top = 50;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 4; i++)
            {
                legend.show = !legend.show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            legend.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
            chart.ClickLegendButton(0, "Line", false);
            yield return new WaitForSeconds(0.2f);
            chart.ClickLegendButton(0, "Line", true);
            yield return new WaitForSeconds(0.5f);

            chart.ClickLegendButton(1, "Bar", false);
            yield return new WaitForSeconds(0.2f);
            chart.ClickLegendButton(1, "Bar", true);
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator ComponentTheme()
        {
            chart.EnsureChartComponent<Title>().subText = "Theme 主题：可从全局上配置图表的颜色、字体等效果，支持默认主题切换";
            yield return new WaitForSeconds(1f);
            chart.EnsureChartComponent<Title>().subText = "Theme 主题：Light主题";
            chart.UpdateTheme(ThemeType.Light);
            yield return new WaitForSeconds(1f);
            chart.EnsureChartComponent<Title>().subText = "Theme 主题：Dark主题";
            chart.UpdateTheme(ThemeType.Dark);
            yield return new WaitForSeconds(1f);
            chart.EnsureChartComponent<Title>().subText = "Theme 主题：Default主题";
            chart.UpdateTheme(ThemeType.Default);
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentDataZoom()
        {
            chart.EnsureChartComponent<Title>().subText = "DataZoom 区域缩放：可通过拖、拽、缩小、放大来观察细节数据";
            var grid = chart.EnsureChartComponent<GridCoord>();
            grid.bottom = 70;

            var dataZoom = chart.EnsureChartComponent<DataZoom>();
            dataZoom.enable = true;
            dataZoom.supportInside = true;
            dataZoom.supportSlider = true;
            dataZoom.start = 0;
            dataZoom.end = 100;

            chart.RefreshChart();
            for (int i = 0; i < 4; i++)
            {
                dataZoom.supportSlider = !dataZoom.supportSlider;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            dataZoom.supportSlider = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
            while (dataZoom.start < 40)
            {
                dataZoom.start += speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (dataZoom.end > 60)
            {
                dataZoom.end -= speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (dataZoom.start > 0)
            {
                dataZoom.start -= speed * Time.deltaTime * 0.8f;
                dataZoom.end -= speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (dataZoom.end < 100)
            {
                dataZoom.start += speed * Time.deltaTime * 0.8f;
                dataZoom.end += speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (dataZoom.start > 0 || dataZoom.end < 100)
            {
                dataZoom.start -= speed * Time.deltaTime * 0.8f;
                dataZoom.end += speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
        }

        IEnumerator ComponentVisualMap()
        {
            chart.EnsureChartComponent<Title>().subText = "VisualMap 视觉映射：可从全局上配置图表的颜色、字体等效果，支持默认主题切换";

            var visualMap = chart.EnsureChartComponent<VisualMap>();
            visualMap.show = true;
            visualMap.showUI = true;
            visualMap.orient = Orient.Vertical;
            visualMap.calculable = true;
            visualMap.min = 0;
            visualMap.max = 100;
            visualMap.range[0] = 0;
            visualMap.range[1] = 100;

            var colors = new List<string>
            {
                "#313695",
                "#4575b4",
                "#74add1",
                "#abd9e9",
                "#e0f3f8",
                "#ffffbf",
                "#fee090",
                "#fdae61",
                "#f46d43",
                "#d73027",
                "#a50026"
            };
            visualMap.AddColors(colors);
            var grid = chart.EnsureChartComponent<GridCoord>();
            grid.left = 80;
            grid.bottom = 100;
            chart.RefreshChart();

            yield return new WaitForSeconds(1f);
            while (visualMap.rangeMin < 40)
            {
                visualMap.rangeMin += speed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            while (visualMap.rangeMax > 60)
            {
                visualMap.rangeMax -= speed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            while (visualMap.rangeMin > 0 || visualMap.rangeMax < 100)
            {
                visualMap.rangeMin -= speed * Time.deltaTime;
                visualMap.rangeMax += speed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
        }
    }
}