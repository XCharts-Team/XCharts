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
            chart.RemoveRadar();
            chart.RemoveData();

            chart.title.text = "RadarChart - 雷达图";
            chart.title.subText = "";

            chart.legend.show = true;
            chart.legend.location.align = Location.Align.TopLeft;
            chart.legend.location.top = 60;
            chart.legend.location.left = 2;
            chart.legend.itemWidth = 70;
            chart.legend.itemHeight = 20;
            chart.legend.orient = Orient.Vertical;

            chart.AddRadar(Radar.Shape.Polygon, new Vector2(0.5f, 0.4f), 0.4f);
            chart.AddIndicator(0, "indicator1", 0, 100);
            chart.AddIndicator(0, "indicator2", 0, 100);
            chart.AddIndicator(0, "indicator3", 0, 100);
            chart.AddIndicator(0, "indicator4", 0, 100);
            chart.AddIndicator(0, "indicator5", 0, 100);

            serie = chart.AddSerie(SerieType.Radar, "test");
            serie.radarIndex = 0;
            chart.AddData(0, new List<float> { 10, 20, 60, 40, 20 }, "data1");
            chart.AddData(0, new List<float> { 40, 60, 90, 80, 70 }, "data2");
            yield return new WaitForSeconds(1);
        }

        IEnumerator RadarUpdate()
        {
            chart.UpdateIndicator(0, 0, "new1", 0, 100);
            chart.UpdateData(0, 0, new List<float> { 15, 30, 50, 60, 50 });
            chart.UpdateDataName(0, 0, "new1");
            yield return new WaitForSeconds(1);
        }

        IEnumerator RadarAddMultiple()
        {
            chart.RemoveRadar();
            chart.RemoveData();

            chart.title.text = "RadarChart - 多雷达图";
            chart.title.subText = "";

            chart.legend.show = true;
            chart.legend.location.align = Location.Align.TopLeft;
            chart.legend.location.top = 60;
            chart.legend.location.left = 2;
            chart.legend.itemWidth = 70;
            chart.legend.itemHeight = 20;
            chart.legend.orient = Orient.Vertical;

            chart.AddRadar(Radar.Shape.Polygon, new Vector2(0.25f, 0.4f), 0.25f);
            for (int i = 1; i <= 5; i++)
            {
                chart.AddIndicator(0, "radar1" + i, 0, 100);
            }

            chart.AddRadar(Radar.Shape.Circle, new Vector2(0.75f, 0.4f), 0.25f);
            for (int i = 1; i <= 5; i++)
            {
                chart.AddIndicator(1, "radar2" + i, 0, 100);
            }

            serie = chart.AddSerie(SerieType.Radar, "test1");
            serie.radarIndex = 0;
            chart.AddData(0, new List<float> { 10, 20, 60, 40, 20 }, "data1");

            serie1 = chart.AddSerie(SerieType.Radar, "test2");
            serie1.radarIndex = 1;
            chart.AddData(1, new List<float> { 10, 20, 60, 40, 20 }, "data2");
            yield return new WaitForSeconds(1);
        }
    }
}