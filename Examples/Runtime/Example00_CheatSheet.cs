/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    public class Example00_CheatSheet : MonoBehaviour
    {
        private LineChart chart;
        private float speed = 100f;

        void Awake()
        {
            LoopDemo();
        }

        private void OnEnable()
        {
            LoopDemo();
        }

        void LoopDemo()
        {
            StopAllCoroutines();
            StartCoroutine(CheatSheet());
        }

        IEnumerator CheatSheet()
        {
            StartCoroutine(InitChart());
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
            LoopDemo();
        }

        IEnumerator InitChart()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null) gameObject.AddComponent<LineChart>();

            chart.title.show = true;
            chart.title.text = "术语解析-组件";
            chart.grid.bottom = 30;
            chart.grid.right = 30;
            chart.grid.left = 50;
            chart.grid.top = 80;

            chart.dataZoom.enable = false;
            chart.visualMap.enable = false;

            chart.RemoveData();

            chart.AddSerie(SerieType.Bar, "Bar");
            chart.AddSerie(SerieType.Line, "Line");

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
            chart.title.text = "术语解析 - 组件";
            chart.title.subText = "Title 标题：可指定主标题和子标题";
            chart.xAxis0.show = true;
            chart.yAxis0.show = true;
            chart.series.list[0].show = false;
            chart.series.list[1].show = false;
            chart.legend.show = false;
            for (int i = 0; i < 4; i++)
            {
                chart.title.show = !chart.title.show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.title.show = true;
            chart.RefreshChart();
        }

        IEnumerator ComponentAxis()
        {
            chart.title.subText = "Axis 坐标轴：配置X和Y轴的轴线、刻度、标签等样式外观配置";
            chart.series.list[0].show = false;
            chart.series.list[1].show = false;
            for (int i = 0; i < 4; i++)
            {
                chart.xAxis0.show = !chart.xAxis0.show;
                chart.yAxis0.show = !chart.yAxis0.show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.xAxis0.show = true;
            chart.yAxis0.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentGrid()
        {
            chart.title.subText = "Grid 网格：调整坐标系边距和颜色等";
            for (int i = 0; i < 4; i++)
            {
                chart.grid.backgroundColor = i % 2 == 0 ? Color.clear : Color.grey;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.grid.backgroundColor = Color.clear;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentSerie()
        {
            chart.title.subText = "Serie 系列：调整坐标系边距和颜色等";
            chart.series.list[0].show = true;
            chart.series.list[1].show = true;
            chart.AnimationReset();
            chart.RefreshChart();
            yield return new WaitForSeconds(1.2f);
            for (int i = 0; i < 4; i++)
            {
                chart.series.list[0].show = !chart.series.list[0].show;
                chart.series.list[1].show = !chart.series.list[1].show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.series.list[0].show = true;
            chart.series.list[1].show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentLegend()
        {
            chart.title.subText = "Legend 图例：展示不同系列的名字和颜色，可控制系列显示等";
            chart.legend.show = true;
            chart.grid.top = 80;
            chart.legend.location.top = 50;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 4; i++)
            {
                chart.legend.show = !chart.legend.show;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.legend.show = true;
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
            chart.title.subText = "Theme 主题：可从全局上配置图表的颜色、字体等效果，支持默认主题切换";
            yield return new WaitForSeconds(1f);
            chart.title.subText = "Theme 主题：Light主题";
            chart.UpdateTheme(Theme.Light);
            yield return new WaitForSeconds(1f);
            chart.title.subText = "Theme 主题：Dark主题";
            chart.UpdateTheme(Theme.Dark);
            yield return new WaitForSeconds(1f);
            chart.title.subText = "Theme 主题：Default主题";
            chart.UpdateTheme(Theme.Default);
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ComponentDataZoom()
        {
            chart.title.subText = "DataZoom 区域缩放：可通过拖、拽、缩小、放大来观察细节数据";
            chart.grid.bottom = 70;

            chart.dataZoom.enable = true;
            chart.dataZoom.supportInside = true;
            chart.dataZoom.supportSlider = true;
            chart.dataZoom.height = 30;
            chart.dataZoom.start = 0;
            chart.dataZoom.end = 100;

            chart.RefreshChart();
            for (int i = 0; i < 4; i++)
            {
                chart.dataZoom.supportSlider = !chart.dataZoom.supportSlider;
                chart.RefreshChart();
                yield return new WaitForSeconds(0.2f);
            }
            chart.dataZoom.supportSlider = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);
            while (chart.dataZoom.start < 40)
            {
                chart.dataZoom.start += speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (chart.dataZoom.end > 60)
            {
                chart.dataZoom.end -= speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (chart.dataZoom.start > 0)
            {
                chart.dataZoom.start -= speed * Time.deltaTime * 0.8f;
                chart.dataZoom.end -= speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (chart.dataZoom.end < 100)
            {
                chart.dataZoom.start += speed * Time.deltaTime * 0.8f;
                chart.dataZoom.end += speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
            while (chart.dataZoom.start > 0 || chart.dataZoom.end < 100)
            {
                chart.dataZoom.start -= speed * Time.deltaTime * 0.8f;
                chart.dataZoom.end += speed * Time.deltaTime * 0.8f;
                chart.RefreshDataZoom();
                chart.RefreshChart();
                yield return null;
            }
        }

        IEnumerator ComponentVisualMap()
        {
            chart.title.subText = "VisualMap 视觉映射：可从全局上配置图表的颜色、字体等效果，支持默认主题切换";

            chart.visualMap.enable = true;
            chart.visualMap.show = true;
            chart.visualMap.orient = Orient.Vertical;
            chart.visualMap.calculable = true;
            chart.visualMap.min = 0;
            chart.visualMap.max = 100;
            chart.visualMap.range[0] = 0;
            chart.visualMap.range[1] = 100;

            var colors = new List<string>{"#313695", "#4575b4", "#74add1", "#abd9e9", "#e0f3f8", "#ffffbf",
                "#fee090", "#fdae61", "#f46d43", "#d73027", "#a50026"};
            chart.visualMap.inRange.Clear();
            foreach (var str in colors)
            {
                chart.visualMap.inRange.Add(ThemeInfo.GetColor(str));
            }
            chart.grid.left = 80;
            chart.grid.bottom = 100;
            chart.RefreshChart();

            yield return new WaitForSeconds(1f);
            while (chart.visualMap.rangeMin < 40)
            {
                chart.visualMap.rangeMin += speed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            while (chart.visualMap.rangeMax > 60)
            {
                chart.visualMap.rangeMax -= speed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            while (chart.visualMap.rangeMin > 0 || chart.visualMap.rangeMax < 100)
            {
                chart.visualMap.rangeMin -= speed * Time.deltaTime;
                chart.visualMap.rangeMax += speed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
        }
    }
}