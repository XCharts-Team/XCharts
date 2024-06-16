using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XCharts.Runtime;
using ADB = UnityEditor.AssetDatabase;

namespace XCharts.Editor
{
    public partial class XChartsEditor
    {
        [MenuItem("XCharts/LineChart/Basic Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Basic Line", priority = 44)]
        public static void AddLineChart()
        {
            AddChart<LineChart>("LineChart");
        }

        [MenuItem("XCharts/LineChart/Area Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Area Line", priority = 44)]
        public static void AddLineChart_Area()
        {
            var chart = AddChart<LineChart>("LineChart_Area", "Area Line");
            chart.DefaultAreaLineChart();
        }

        [MenuItem("XCharts/LineChart/Smooth Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Smooth Line", priority = 44)]
        public static void AddLineChart_Smooth()
        {
            var chart = AddChart<LineChart>("LineChart_Smooth", "Smooth Line");
            chart.DefaultSmoothLineChart();
        }

        [MenuItem("XCharts/LineChart/Smooth Area", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Smooth Area Line", priority = 44)]
        public static void AddLineChart_SmoothArea()
        {
            var chart = AddChart<LineChart>("LineChart_SmoothArea", "Smooth Area Line");
            chart.DefaultSmoothAreaLineChart();
        }

        [MenuItem("XCharts/LineChart/Stack Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Stack Line", priority = 44)]
        public static void AddLineChart_Stack()
        {
            var chart = AddChart<LineChart>("LineChart_Stack", "Stack Line");
            chart.DefaultStackLineChart();
        }

        [MenuItem("XCharts/LineChart/Stack Area Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Stack Area Line", priority = 44)]
        public static void AddLineChart_StackArea()
        {
            var chart = AddChart<LineChart>("LineChart_StackArea", "Stack Area Line");
            chart.DefaultStackAreaLineChart();
        }

        [MenuItem("XCharts/LineChart/Step Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Step Line", priority = 44)]
        public static void AddLineChart_Step()
        {
            var chart = AddChart<LineChart>("LineChart_Step", "Step Line");
            chart.DefaultStepLineChart();
        }

        [MenuItem("XCharts/LineChart/Dashed Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Dashed Line", priority = 44)]
        public static void AddLineChart_Dash()
        {
            var chart = AddChart<LineChart>("LineChart_Dashed", "Dashed Line");
            chart.DefaultDashLineChart();
        }

        [MenuItem("XCharts/LineChart/Time Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Time Line", priority = 44)]
        public static void AddLineChart_Time()
        {
            var chart = AddChart<LineChart>("LineChart_Time", "Time Line");
            chart.DefaultTimeLineChart();
        }

        [MenuItem("XCharts/LineChart/Log Line", priority = 44)]
        [MenuItem("GameObject/UI/XCharts/LineChart/Log Line", priority = 44)]
        public static void AddLineChart_Log()
        {
            var chart = AddChart<LineChart>("LineChart_Log", "Log Line");
            chart.DefaultLogLineChart();
        }
    }
}