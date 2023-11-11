using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XCharts.Runtime;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BaseChart))]
    public class Example01_RandomData : MonoBehaviour
    {
        public bool loopAdd = false;
        public float loopAddTime = 1f;
        public bool loopUpdate = false;
        public float loopUpadteTime = 1f;
        public int maxCache = 0;

        BaseChart chart;
        float lastAddTime;
        float lastUpdateTime;
        int dataCount;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
        }

        void Start()
        {
            if (maxCache > 0)
            {
                chart.SetMaxCache(maxCache);
            }
            dataCount = chart.GetSerie(0).dataCount;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateData();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                chart.ClearData();
            }
            lastAddTime += Time.deltaTime;
            if (loopAdd && lastAddTime >= loopAddTime)
            {
                lastAddTime = 0;
                AddData();
            }

            lastUpdateTime += Time.deltaTime;
            if (loopUpdate && lastUpdateTime >= loopUpadteTime)
            {
                lastUpdateTime = 0;
                UpdateData();
            }
        }

        void AddData()
        {
            if (chart is HeatmapChart)
            {
                var xAxis = chart.GetChartComponent<XAxis>();
                var yAxis = chart.GetChartComponent<YAxis>();
                if (xAxis != null && yAxis != null)
                {
                    chart.AddXAxisData((xAxis.data.Count + 1).ToString());
                    for (int i = 0; i < yAxis.data.Count; i++)
                    {
                        chart.AddData(0, xAxis.data.Count - 1, i, Random.Range(10, 90));
                    }
                }
                return;
            }
            else
            {
                var xAxis = chart.GetChartComponent<XAxis>();
                if (xAxis != null)
                {
                    if (xAxis.type == Axis.AxisType.Category)
                    {
                        chart.AddXAxisData("x" + (xAxis.data.Count + 1));
                    }
                }
                foreach (var serie in chart.series)
                {
                    AddSerieRandomData(serie, xAxis);
                }
            }
        }

        void UpdateData()
        {
            foreach (var serie in chart.series)
            {
                UpdateSerieRandomData(serie);
            }
        }

        void AddSerieRandomData(Serie serie, XAxis xAxis)
        {
            if (serie is Line || serie is Bar || serie is Scatter || serie is EffectScatter)
            {
                if (xAxis.type == Axis.AxisType.Category)
                {
                    chart.AddData(serie.index, Random.Range(10, 90), "data" + serie.dataCount);
                }
                else
                {
                    if (serie is Line)
                        chart.AddData(serie.index, dataCount++, Random.Range(10, 90), "data" + serie.dataCount);
                    else
                        chart.AddData(serie.index, Random.Range(10, 90), Random.Range(10, 90), "data" + serie.dataCount);
                }
            }
            else if (serie is Ring)
            {
                chart.AddData(serie.index, Random.Range(10, 90), 100, "data" + serie.dataCount);
            }
            else if (serie is Radar)
            {
                var list = new System.Collections.Generic.List<double>();
                for (int i = 0; i < 5; i++)
                    list.Add(Random.Range(10, 90));
                chart.AddData(serie.index, list, "data" + serie.dataCount);
            }
            else if (serie is Candlestick)
            {
                var open = Random.Range(20, 60);
                var close = Random.Range(40, 90);
                var lowest = Random.Range(0, 50);
                var heighest = Random.Range(50, 100);
                chart.AddData(serie.index, serie.dataCount, open, close, lowest, heighest);
            }
            else if (serie is Heatmap)
            {
                var yAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
                for (int i = 0; i < yAxis.data.Count; i++)
                {
                    chart.AddData(serie.index, xAxis.data.Count - 1, i, Random.Range(0, 150));
                }
            }
            else
            {
                chart.AddData(serie.index, Random.Range(10, 90), "data" + serie.dataCount);
            }
        }

        void UpdateSerieRandomData(Serie serie)
        {
            var index = Random.Range(0, serie.dataCount);
            if (serie is Ring)
            {
                chart.UpdateData(serie.index, index, 0, Random.Range(10, 90));
            }
            else if (serie is Radar)
            {
                var dimension = Random.Range(0, 5);
                chart.UpdateData(serie.index, index, dimension, Random.Range(10, 90));
            }
            else if (serie is Heatmap)
            {
                var xAxis = chart.GetChartComponent<XAxis>();
                var yAxis = chart.GetChartComponent<YAxis>();
                var xIndex = Random.Range(0, xAxis.data.Count);
                var yIndex = Random.Range(0, yAxis.data.Count);
                chart.UpdateData(serie.index, xIndex, yIndex, Random.Range(10, 90));
            }
            else if (serie is Candlestick)
            {
                var open = Random.Range(20, 60);
                var close = Random.Range(40, 90);
                var lowest = Random.Range(0, 50);
                var heighest = Random.Range(50, 100);
                chart.UpdateData(serie.index, index, new List<double> { open, close, lowest, heighest });
            }
            else
            {
                chart.UpdateData(serie.index, index, Random.Range(10, 90));
            }
        }
    }
}