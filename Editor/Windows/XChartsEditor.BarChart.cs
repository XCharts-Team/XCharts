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
        [MenuItem("XCharts/BarChart/Baisc Column", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Baisc Column", priority = 45)]
        public static void AddBarChart()
        {
            AddChart<BarChart>("BarChart");
        }

        [MenuItem("XCharts/BarChart/Zebra Column", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Zebra Column", priority = 45)]
        public static void AddBarChart_ZebraColumn()
        {
            var chart = AddChart<BarChart>("BarChart", "Zebra Column");
            chart.DefaultZebraColumnChart();
        }

        [MenuItem("XCharts/BarChart/Capsule Column", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Capsule Column", priority = 45)]
        public static void AddBarChart_CapsuleColumn()
        {
            var chart = AddChart<BarChart>("BarChart", "Capsule Column");
            chart.DefaultCapsuleColumnChart();
        }

        [MenuItem("XCharts/BarChart/Grouped Column", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Grouped Column", priority = 45)]
        public static void AddBarChart_GroupedColumn()
        {
            var chart = AddChart<BarChart>("BarChart", "Grouped Column");
            chart.DefaultGroupedColumnChart();
        }

        [MenuItem("XCharts/BarChart/Stacked Column", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Stacked Column", priority = 45)]
        public static void AddBarChart_StackedColumn()
        {
            var chart = AddChart<BarChart>("BarChart", "Stacked Column");
            chart.DefaultStackedColumnChart();
        }

        [MenuItem("XCharts/BarChart/Percent Column", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Percent Column", priority = 45)]
        public static void AddBarChart_PercentColumn()
        {
            var chart = AddChart<BarChart>("BarChart", "Percent Column");
            chart.DefaultPercentColumnChart();
        }

        [MenuItem("XCharts/BarChart/Baisc Bar", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Baisc Bar", priority = 45)]
        public static void AddBarChart_BasicBar()
        {
            var chart = AddChart<BarChart>("BarChart");
            chart.DefaultBarChart();
        }

        [MenuItem("XCharts/BarChart/Zebra Bar", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Zebra Bar", priority = 45)]
        public static void AddBarChart_ZebraBar()
        {
            var chart = AddChart<BarChart>("BarChart", "Zebra Bar");
            chart.DefaultZebraBarChart();
        }

        [MenuItem("XCharts/BarChart/Capsule Bar", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Capsule Bar", priority = 45)]
        public static void AddBarChart_CapsuleBar()
        {
            var chart = AddChart<BarChart>("BarChart", "Capsule Bar");
            chart.DefaultCapsuleBarChart();
        }

        [MenuItem("XCharts/BarChart/Grouped Bar", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Grouped Bar", priority = 45)]
        public static void AddBarChart_GroupedBar()
        {
            var chart = AddChart<BarChart>("BarChart", "Grouped Bar");
            chart.DefaultGroupedBarChart();
        }

        [MenuItem("XCharts/BarChart/Stacked Bar", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Stacked Bar", priority = 45)]
        public static void AddBarChart_StackedBar()
        {
            var chart = AddChart<BarChart>("BarChart", "Stacked Bar");
            chart.DefaultStackedBarChart();
        }

        [MenuItem("XCharts/BarChart/Percent Bar", priority = 45)]
        [MenuItem("GameObject/UI/XCharts/BarChart/Percent Bar", priority = 45)]
        public static void AddBarChart_PercentBar()
        {
            var chart = AddChart<BarChart>("BarChart", "Percent Bar");
            chart.DefaultPercentBarChart();
        }
    }
}