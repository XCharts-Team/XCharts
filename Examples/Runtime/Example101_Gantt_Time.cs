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
    public class Example101_Gantt_Time : MonoBehaviour
    {
        private GanttChart chart;
        private float updateTime;
        public int taskCount = 5;

        void Awake()
        {
            chart = gameObject.GetComponent<GanttChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<GanttChart>();
            }
            GenerateTimeData();
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
            chart.ClearData();
            for (int i = 0; i < taskCount; i++)
            {
                var taskName = "张三-任务-" + (i + 1);
                var nowTimestamp = DateTimeUtil.GetTimestamp();
                var startTimestamp = nowTimestamp + Random.Range(1, 6) * 3600 * 24;
                var endTimestamp = startTimestamp + Random.Range(1, 10) * 3600 * 24;
                chart.AddData(0, startTimestamp, endTimestamp, taskName);
            }
            chart.AddSerie(SerieType.Gantt, "李四");
            for (int i = 0; i < taskCount; i++)
            {
                var taskName = "李四-任务-" + (i + 1);
                var nowTimestamp = DateTimeUtil.GetTimestamp();
                var startTimestamp = nowTimestamp + Random.Range(1, 6) * 3600 * 24;
                var endTimestamp = startTimestamp + Random.Range(1, 10) * 3600 * 24;
                chart.AddData(1, startTimestamp, endTimestamp, taskName);
            }
        }

        void GenerateTimeData()
        {
            chart.RemoveData();

            chart.grid.left = 100;
            chart.xAxis0.type = Axis.AxisType.Time;
            chart.xAxis0.boundaryGap = false;
            chart.xAxis0.splitNumber = 5;

            chart.xAxis0.axisLabel.numericFormatter = "HH:mm:ss";
            chart.xAxis0.axisLabel.formatter = "time:{value}";

            chart.yAxis0.type = Axis.AxisType.Category;
            chart.yAxis0.boundaryGap = true;
            chart.yAxis0.splitNumber = 0;


            var serie1 = chart.AddSerie(SerieType.Gantt, "张三");
            serie1.label.show = true;
            for (int i = 0; i < taskCount; i++)
            {
                var taskName = "张三-任务-" + (i + 1);
                var nowTimestamp = DateTimeUtil.GetTimestamp();
                var startTimestamp = nowTimestamp + Random.Range(1, 6) * 3600 * 24;
                var endTimestamp = startTimestamp + Random.Range(1, 10) * 3600 * 24;
                chart.AddData(0, startTimestamp, endTimestamp, taskName);
            }
            chart.AddSerie(SerieType.Gantt, "李四");
            for (int i = 0; i < taskCount; i++)
            {
                var taskName = "李四-任务-" + (i + 1);
                var nowTimestamp = DateTimeUtil.GetTimestamp();
                var startTimestamp = nowTimestamp + Random.Range(1, 6) * 3600 * 24;
                var endTimestamp = startTimestamp + Random.Range(1, 10) * 3600 * 24;
                chart.AddData(1, startTimestamp, endTimestamp, taskName);
            }
        }
    }
}