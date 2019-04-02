using System;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class BarChartDemo : MonoBehaviour
{
    public Theme theme = Theme.Dark;

    private float time;
    private int count;
    private Theme checkTheme = Theme.Dark;

    private int dataCount = 0;
    private BarChart bigdataChart;

    void Awake()
    {
        var xchart = transform.Find("xchart");
        GridLayoutGroup grid = xchart.GetComponent<GridLayoutGroup>();
        RectTransform rect = transform.GetComponent<RectTransform>();
        var wid = rect.sizeDelta.x;
        int childNum = xchart.childCount;
        float hig = grid.padding.top + childNum * (grid.cellSize.y + grid.spacing.y);
        rect.sizeDelta = new Vector2(wid, hig);
        xchart.GetComponent<RectTransform>().sizeDelta = new Vector2(wid, hig);

        bigdataChart = xchart.Find("barchart_multidata").GetComponent<BarChart>();
        GenerateData(5000, bigdataChart);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 0)
        {
            time = 0;
            //count++;
            //bigdataChart.AddData(0, Random.Range(30, 120));
            //bigdataChart.AddXAxisCategory(count.ToString());
            //bigdataChart.InitXScale();
            //bigdataChart.RefreshChart();
        }
        if (checkTheme != theme)
        {
            checkTheme = theme;
            UpdateTheme(theme);
        }
    }

    void UpdateTheme(Theme theme)
    {
        var charts = transform.Find("xchart").GetComponentsInChildren<BaseChart>();
        foreach (var chart in charts)
        {
            chart.UpdateTheme(theme);
        }
    }

    void GenerateData(int count,BarChart chart)
    {
        var baseValue = UnityEngine.Random.Range(0,1000);
        var time = new DateTime(2011, 1, 1);
        var smallBaseValue = 0;

        for (var i = 0; i < count; i++)
        {
            chart.XAxis.AddData(time.ToString("hh:mm:ss"));

            smallBaseValue = i % 30 == 0
                ? UnityEngine.Random.Range(0, 700)
                : (smallBaseValue + UnityEngine.Random.Range(0, 500) - 250);
            baseValue += UnityEngine.Random.Range(0, 20) - 10;
            float value = Mathf.Max(
                0,
                Mathf.Round(baseValue + smallBaseValue) + 3000
            );
            //var index = i % 100;
            //var value = (Mathf.Sin(index / 5) * (index / 5 - 10) + index / 6) * 5;
            value = Mathf.Abs(value);
            chart.AddData(0, value);
            time = time.AddSeconds(1);
        }
    }
}
