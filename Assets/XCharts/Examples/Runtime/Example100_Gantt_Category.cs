/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/


using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example100_Gantt_Category : MonoBehaviour
    {
        private GanttChart chart;
        private float updateTime;
        public int dayCount = 10;
        public int taskCount = 5;

        void Awake()
        {
            chart = gameObject.GetComponent<GanttChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<GanttChart>();
            }
            GenerateCategoryData();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
            }
        }

        void AddData()
        {
            for (int i = 0; i < taskCount; i++)
            {
                var startIndex = Random.Range(0, (int)(dayCount * 2.0f / 3));
                var endIndex = Random.Range(startIndex, dayCount);
                chart.UpdateData(0, i, 0, startIndex);
                chart.UpdateData(0, i, 1, endIndex);
            }
        }

        void GenerateCategoryData()
        {
            chart.RemoveData();

            chart.grid.left = 100;
            chart.xAxis0.type = Axis.AxisType.Category;
            chart.xAxis0.boundaryGap = false;
            chart.xAxis0.splitNumber = dayCount;

            chart.yAxis0.type = Axis.AxisType.Category;
            chart.yAxis0.boundaryGap = true;
            chart.yAxis0.splitNumber = 0;

            for (int i = 0; i < dayCount; i++)
            {
                chart.AddXAxisData("day" + (i + 1));
            }

            chart.AddSerie(SerieType.Gantt, "任务进度表");
            for (int i = 0; i < taskCount; i++)
            {
                var taskName = "task-" + (i + 1);
                var startIndex = Random.Range(0, (int)(dayCount * 2.0f / 3));
                var endIndex = Random.Range(startIndex, dayCount);
                chart.AddData(0, startIndex, endIndex, taskName);
            }
        }
    }
}