using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example40_Radar : MonoBehaviour
    {
        private RadarChart chart;
        private Serie serie, serie1;
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
            StartCoroutine(RadarDemo());
        }

        IEnumerator RadarDemo()
        {
            StartCoroutine(RadarAdd());
            yield return new WaitForSeconds(2);
            StartCoroutine(RadarUpdate());
            yield return new WaitForSeconds(2);
            StartCoroutine(RadarAddMultiple());
            yield return new WaitForSeconds(2);
            LoopDemo();
        }

        IEnumerator RadarAdd()
        {
            chart = gameObject.GetComponent<RadarChart>();
            if (chart == null) chart = gameObject.AddComponent<RadarChart>();

            chart.RemoveChartComponents<RadarCoord>();
            chart.RemoveData();

            chart.GetChartComponent<Title>().text = "RadarChart - 雷达图";
            chart.GetChartComponent<Title>().subText = "";

            var legend = chart.GetChartComponent<Legend>();
            legend.show = true;
            legend.location.align = Location.Align.TopLeft;
            legend.location.top = 60;
            legend.location.left = 2;
            legend.itemWidth = 70;
            legend.itemHeight = 20;
            legend.orient = Orient.Vertical;

            var radarCoord = chart.AddChartComponent<RadarCoord>();
            radarCoord.shape = RadarCoord.Shape.Polygon;
            radarCoord.center[0] = 0.5f;
            radarCoord.center[1] = 0.4f;
            radarCoord.radius = 0.4f;

            radarCoord.AddIndicator("indicator1", 0, 100);
            radarCoord.AddIndicator("indicator2", 0, 100);
            radarCoord.AddIndicator("indicator3", 0, 100);
            radarCoord.AddIndicator("indicator4", 0, 100);
            radarCoord.AddIndicator("indicator5", 0, 100);

            serie = chart.AddSerie<Radar>("test");
            serie.radarIndex = 0;
            chart.AddData(0, new List<double> { 10, 20, 60, 40, 20 }, "data1");
            chart.AddData(0, new List<double> { 40, 60, 90, 80, 70 }, "data2");
            yield return new WaitForSeconds(1);
        }

        IEnumerator RadarUpdate()
        {
            var radarCoord = chart.GetChartComponent<RadarCoord>();
            radarCoord.UpdateIndicator(0, "new1", 0, 100);
            chart.UpdateData(0, 0, new List<double> { 15, 30, 50, 60, 50 });
            chart.UpdateDataName(0, 0, "new1");
            yield return new WaitForSeconds(1);
        }

        IEnumerator RadarAddMultiple()
        {
            chart.RemoveChartComponents<RadarCoord>();
            chart.RemoveData();

            chart.GetChartComponent<Title>().text = "RadarChart - 多雷达图";
            chart.GetChartComponent<Title>().subText = "";

            var legend = chart.GetChartComponent<Legend>();
            legend.show = true;
            legend.location.align = Location.Align.TopLeft;
            legend.location.top = 60;
            legend.location.left = 2;
            legend.itemWidth = 70;
            legend.itemHeight = 20;
            legend.orient = Orient.Vertical;

            var radarCoord = chart.AddChartComponent<RadarCoord>();
            radarCoord.shape = RadarCoord.Shape.Polygon;
            radarCoord.center[0] = 0.25f;
            radarCoord.center[1] = 0.4f;
            radarCoord.radius = 0.25f;
            for (int i = 1; i <= 5; i++)
            {
                radarCoord.AddIndicator("radar1" + i, 0, 100);
            }

            var radarCoord2 = chart.AddChartComponent<RadarCoord>();
            radarCoord2.shape = RadarCoord.Shape.Polygon;
            radarCoord2.center[0] = 0.75f;
            radarCoord2.center[1] = 0.4f;
            radarCoord2.radius = 0.25f;
            for (int i = 1; i <= 5; i++)
            {
                radarCoord2.AddIndicator("radar2" + i, 0, 100);
            }

            serie = chart.AddSerie<Radar>("test1");
            serie.radarIndex = 0;
            chart.AddData(0, new List<double> { 10, 20, 60, 40, 20 }, "data1");

            serie1 = chart.AddSerie<Radar>("test2");
            serie1.radarIndex = 1;
            chart.AddData(1, new List<double> { 10, 20, 60, 40, 20 }, "data2");
            yield return new WaitForSeconds(1);
        }
    }
}