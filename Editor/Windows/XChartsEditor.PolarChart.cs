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
        [MenuItem("XCharts/PolarChart/Line", priority = 54)]
        [MenuItem("GameObject/UI/XCharts/PolarChart/Line", priority = 54)]
        public static void PolarChart()
        {
            AddChart<PolarChart>("PolarChart");
        }

        [MenuItem("XCharts/PolarChart/Radial Bar", priority = 54)]
        [MenuItem("GameObject/UI/XCharts/PolarChart/Radial Bar", priority = 54)]
        public static void PolarChart_RadialBar()
        {
            var chart = AddChart<PolarChart>("PolarChart");
            chart.DefaultRadialBarPolarChart();
        }

        [MenuItem("XCharts/PolarChart/Tangential Bar", priority = 54)]
        [MenuItem("GameObject/UI/XCharts/PolarChart/Tangential Bar", priority = 54)]
        public static void PolarChart_TangentialBar()
        {
            var chart = AddChart<PolarChart>("PolarChart");
            chart.DefaultTangentialBarPolarChart();
        }

        [MenuItem("XCharts/PolarChart/Heatmap", priority = 54)]
        [MenuItem("GameObject/UI/XCharts/PolarChart/Heatmap", priority = 54)]
        public static void PolarChart_Heatmap()
        {
            var chart = AddChart<PolarChart>("PolarChart");
            chart.DefaultHeatmapPolarChart();
        }


    }
}