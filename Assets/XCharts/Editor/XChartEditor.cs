/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Compilation;
using System.IO;
using System;
using System.Collections.Generic;

namespace XCharts
{
    public class XChartEditor : Editor
    {
        private static Transform GetParent()
        {
            GameObject selectObj = Selection.activeGameObject;
            if (selectObj == null)
            {
                var canvas = GameObject.FindObjectOfType<Canvas>();
                if (canvas != null) return canvas.transform;
                else
                {
                    var canvasObject = new GameObject();
                    canvasObject.name = "Canvas";
                    canvas = canvasObject.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasObject.AddComponent<CanvasScaler>();
                    canvasObject.AddComponent<GraphicRaycaster>();
                    var eventSystem = new GameObject();
                    eventSystem.name = "EventSystem";
                    eventSystem.AddComponent<EventSystem>();
                    eventSystem.AddComponent<StandaloneInputModule>();
                    return canvas.transform;
                }
            }
            else
            {
                return selectObj.transform;
            }
        }

        private static string GetName(Transform parent, string name)
        {
            if (parent.Find(name) == null) return name;
            for (int i = 1; i <= 10; i++)
            {
                var newName = string.Format("{0} ({1})", name, i);
                if (parent.Find(newName) == null)
                {
                    return newName;
                }
            }
            return name;
        }

        private static void AddChart<T>(string chartName) where T : BaseChart
        {
            var parent = GetParent();
            if (parent == null) return;
            var chart = new GameObject();
            chart.name = GetName(parent, chartName);
            chart.AddComponent<T>();
            chart.transform.SetParent(parent);
            chart.transform.localScale = Vector3.one;
            chart.transform.localPosition = Vector3.zero;
            var rect = chart.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }

        [MenuItem("XCharts/LineChart", priority = 44)]
        [MenuItem("GameObject/XCharts/LineChart", priority = 44)]
        public static void AddLineChart()
        {
            AddChart<LineChart>("LineChart");
        }

        [MenuItem("XCharts/BarChart", priority = 45)]
        [MenuItem("GameObject/XCharts/BarChart", priority = 45)]
        public static void AddBarChart()
        {
            AddChart<BarChart>("BarChart");
        }

        [MenuItem("XCharts/PieChart", priority = 46)]
        [MenuItem("GameObject/XCharts/PieChart", priority = 46)]
        public static void AddPieChart()
        {
            AddChart<PieChart>("PieChart");
        }

        [MenuItem("XCharts/RadarChart", priority = 47)]
        [MenuItem("GameObject/XCharts/RadarChart", priority = 47)]
        public static void AddRadarChart()
        {
            AddChart<RadarChart>("RadarChart");
        }

        [MenuItem("XCharts/ScatterChart", priority = 48)]
        [MenuItem("GameObject/XCharts/ScatterChart", priority = 48)]
        public static void AddScatterChart()
        {
            AddChart<ScatterChart>("ScatterChart");
        }

        [MenuItem("XCharts/HeatmapChart", priority = 49)]
        [MenuItem("GameObject/XCharts/HeatmapChart", priority = 49)]
        public static void AddHeatmapChart()
        {
            AddChart<HeatmapChart>("HeatmapChart");
        }

        [MenuItem("XCharts/GaugeChart", priority = 50)]
        [MenuItem("GameObject/XCharts/GaugeChart", priority = 50)]
        public static void AddGaugeChart()
        {
            AddChart<GaugeChart>("GaugeChart");
        }

        [MenuItem("XCharts/RingChart", priority = 51)]
        [MenuItem("GameObject/XCharts/RingChart", priority = 51)]
        public static void AddRingChart()
        {
            AddChart<RingChart>("RingChart");
        }

        [MenuItem("XCharts/PolarChart", priority = 52)]
        [MenuItem("GameObject/XCharts/PolarChart", priority = 52)]
        public static void AddPolarChart()
        {
            AddChart<PolarChart>("PolarChart");
        }

        [MenuItem("XCharts/LiquidChart", priority = 53)]
        [MenuItem("GameObject/XCharts/LiquidChart", priority = 53)]
        public static void AddLiquidChart()
        {
            AddChart<LiquidChart>("LiquidChart");
        }

        [MenuItem("XCharts/Themes Reload")]
        public static void ReloadTheme()
        {
            XChartsMgr.Instance.LoadThemesFromResources();
        }

        [MenuItem("XCharts/TextMeshPro Enable")]
        public static void EnableTextMeshPro()
        {
            if (!IsExistTMPAssembly())
            {
                Debug.LogError("TextMeshPro is not in the project, please import TextMeshPro package first.");
                return;
            }
            AddTMPRefence();
            XChartsMgr.EnableTextMeshPro();
        }

        [MenuItem("XCharts/TextMeshPro Disable")]
        public static void DisableTextMeshPro()
        {
            XChartsMgr.DisableTextMeshPro();
        }

        private static bool IsExistTMPAssembly()
        {
            foreach (var assembly in CompilationPipeline.GetAssemblies(AssembliesType.Player))
            {
                if (assembly.name.Equals("Unity.TextMeshPro")) return true;
            }
            return false;
        }

        private static bool AddTMPRefence()
        {
            var packagePath = XChartsMgr.GetPackageFullPath();
            if (!AddTMPRefence(packagePath + "/Runtime/XCharts.Runtime.asmdef")) return false;
            if (!AddTMPRefence(packagePath + "/Editor/XCharts.Editor.asmdef")) return false;
            return true;
        }

        private static bool AddTMPRefence(string asmdefPath)
        {
            if (!File.Exists(asmdefPath))
            {
                Debug.LogError("AddTMPRefence ERROR: can't find: " + asmdefPath);
                return false;
            }
            var oldText = File.ReadAllText(asmdefPath);
            try
            {
                var dest = new List<string>();
                var refs = new List<string>();
                var lines = File.ReadAllLines(asmdefPath);
                var referencesStart = false;
                var addTMP = false;
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.Contains("\"references\": ["))
                    {
                        dest.Add(line);
                        referencesStart = true;
                    }
                    else if (referencesStart)
                    {
                        if (line.Contains("],"))
                        {
                            referencesStart = false;
                            if (!refs.Contains("\"Unity.TextMeshPro\":"))
                            {
                                if (refs.Count > 0)
                                    dest[dest.Count - 1] = dest[dest.Count - 1] + ",";
                                dest.Add("        \"Unity.TextMeshPro\"");
                                dest.Add(line);
                                addTMP = true;
                            }
                            else
                            {
                                dest.Add(line);
                            }
                        }
                        else
                        {
                            dest.Add(line);
                            refs.Add(line.Trim());
                        }
                    }
                    else
                    {
                        dest.Add(line);
                    }
                }
                if (addTMP) File.WriteAllText(asmdefPath, string.Join("\n", dest));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("AddTMPRefence ERROR:" + e.Message);
                return false;
            }
        }
    }
}