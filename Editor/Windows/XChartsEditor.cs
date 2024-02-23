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
    public partial class XChartsEditor : UnityEditor.Editor
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
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                    canvas.worldCamera = mainCamera == null ? null : mainCamera.GetComponent<Camera>();
                    canvasObject.AddComponent<CanvasScaler>();
                    canvasObject.AddComponent<GraphicRaycaster>();
                    if (GameObject.Find("EventSystem") == null)
                    {
                        var eventSystem = new GameObject();
                        eventSystem.name = "EventSystem";
                        eventSystem.AddComponent<EventSystem>();
                        eventSystem.AddComponent<StandaloneInputModule>();
                    }
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

        public static T AddChart<T>(string chartName, string titleName = null) where T : BaseChart
        {
            XCThemeMgr.CheckReloadTheme();
            var chart = AddGraph<T>(chartName);
            if (!string.IsNullOrEmpty(titleName))
            {
                var title = chart.GetChartComponent<Title>();
                title.text = titleName;
            }
            return chart;
        }

        public static T AddGraph<T>(string graphName) where T : Graphic
        {
            var parent = GetParent();
            if (parent == null) return null;
            XCThemeMgr.CheckReloadTheme();
            var obj = new GameObject();
            obj.name = GetName(parent, graphName);
            obj.layer = LayerMask.NameToLayer("UI");
            var t = obj.AddComponent<T>();
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            Selection.activeGameObject = obj;
            EditorUtility.SetDirty(obj);
            return t;
        }

        [MenuItem("XCharts/EmptyChart", priority = 43)]
        [MenuItem("GameObject/XCharts/EmptyChart", priority = 43)]
        public static void AddBaseChart()
        {
            AddChart<BaseChart>("EmptyChart");
        }

        [MenuItem("XCharts/RadarChart/Polygon Radar", priority = 47)]
        [MenuItem("GameObject/XCharts/RadarChart/Polygon Radar", priority = 47)]
        public static void AddRadarChart()
        {
            AddChart<RadarChart>("RadarChart");
        }

        [MenuItem("XCharts/RadarChart/Cirle Radar", priority = 47)]
        [MenuItem("GameObject/XCharts/RadarChart/Cirle Radar", priority = 47)]
        public static void AddRadarChart_CirleRadar()
        {
            var chart = AddChart<RadarChart>("RadarChart");
            chart.DefaultCircleRadarChart();
        }

        [MenuItem("XCharts/ScatterChart/Scatter", priority = 48)]
        [MenuItem("GameObject/XCharts/ScatterChart/Scatter", priority = 48)]
        public static void AddScatterChart()
        {
            AddChart<ScatterChart>("ScatterChart");
        }

        [MenuItem("XCharts/ScatterChart/Bubble", priority = 48)]
        [MenuItem("GameObject/XCharts/ScatterChart/Bubble", priority = 48)]
        public static void AddScatterChart_Bubble()
        {
            var chart = AddChart<ScatterChart>("ScatterChart");
            chart.DefaultBubbleChart();
        }

        [MenuItem("XCharts/HeatmapChart/Heatmap", priority = 49)]
        [MenuItem("GameObject/XCharts/HeatmapChart/Heatmap", priority = 49)]
        public static void AddHeatmapChart()
        {
            AddChart<HeatmapChart>("HeatmapChart");
        }

        [MenuItem("XCharts/HeatmapChart/Count Heatmap", priority = 49)]
        [MenuItem("GameObject/XCharts/HeatmapChart/Count Heatmap", priority = 49)]
        public static void AddHeatmapChart_Count()
        {
            var chart = AddChart<HeatmapChart>("HeatmapChart");
            chart.DefaultCountHeatmapChart();
        }

        [MenuItem("XCharts/RingChart/Ring", priority = 51)]
        [MenuItem("GameObject/XCharts/RingChart/Ring", priority = 51)]
        public static void AddRingChart()
        {
            AddChart<RingChart>("RingChart");
        }

        [MenuItem("XCharts/RingChart/Multiple Ring", priority = 51)]
        [MenuItem("GameObject/XCharts/RingChart/Multiple Ring", priority = 51)]
        public static void AddRingChart_MultiRing()
        {
            var chart = AddChart<RingChart>("RingChart");
            chart.DefaultMultipleRingChart();
        }

        [MenuItem("XCharts/CandlestickChart/Candlestick", priority = 54)]
        [MenuItem("GameObject/XCharts/CandlestickChart/Candlestick", priority = 54)]
        public static void CandlestickChart()
        {
            AddChart<CandlestickChart>("CandlestickChart");
        }

        [MenuItem("XCharts/ParallelChart/Parallel", priority = 55)]
        [MenuItem("GameObject/XCharts/ParallelChart/Parallel", priority = 55)]
        public static void ParallelChart()
        {
            AddChart<ParallelChart>("ParallelChart");
        }

        [MenuItem("XCharts/SimplifiedChart/Line", priority = 56)]
        [MenuItem("GameObject/XCharts/SimplifiedChart/Line", priority = 56)]
        public static void SimplifiedLineChart()
        {
            AddChart<SimplifiedLineChart>("SimplifiedLineChart");
        }

        [MenuItem("XCharts/SimplifiedChart/Bar", priority = 57)]
        [MenuItem("GameObject/XCharts/SimplifiedChart/Bar", priority = 57)]
        public static void SimplifiedBarChart()
        {
            AddChart<SimplifiedBarChart>("SimplifiedBarChart");
        }

        [MenuItem("XCharts/SimplifiedChart/Candlestick", priority = 58)]
        [MenuItem("GameObject/XCharts/SimplifiedChart/Candlestick", priority = 58)]
        public static void SimplifiedCandlestickChart()
        {
            AddChart<SimplifiedCandlestickChart>("SimplifiedCandlestickChart");
        }

        [MenuItem("XCharts/Themes Reload")]
        public static void ReloadTheme()
        {
            XCThemeMgr.ReloadThemeList();
        }

        #region Text mesh pro support
#if UNITY_2017_1_OR_NEWER
        const string SYMBOL_TMP = "dUI_TextMeshPro";
        const string ASMDEF_TMP = "Unity.TextMeshPro";

#if !dUI_TextMeshPro
        [MenuItem("XCharts/TextMeshPro Enable")]
#endif
        public static void EnableTextMeshPro()
        {
            if (!IsSpecifyAssemblyExist(ASMDEF_TMP))
            {
                Debug.LogError("TextMeshPro is not in the project, please import TextMeshPro package first.");
                return;
            }
            if (EditorUtility.DisplayDialog("TextMeshPro Enable", "TextMeshPro is disabled, do you want to enable it?", "Yes", "Cancel"))
            {
                DefineSymbolsUtil.AddGlobalDefine(SYMBOL_TMP);
                XChartsMgr.RemoveAllChartObject();
                CheckAsmdefTmpReference(true);
            }
        }

#if dUI_TextMeshPro
        [MenuItem("XCharts/TextMeshPro Disable")]
#endif
        public static void DisableTextMeshPro()
        {
            if (EditorUtility.DisplayDialog("TextMeshPro Disable", "TextMeshPro is enabled, do you want to disable it?", "Yes", "Cancel"))
            {
                CheckAsmdefTmpReference(false);
                DefineSymbolsUtil.RemoveGlobalDefine(SYMBOL_TMP);
                XChartsMgr.RemoveAllChartObject();
            }
        }

        public static void CheckAsmdefTmpReference(bool enable)
        {
            if (enable)
            {
                InsertSpecifyReferenceIntoAssembly(Platform.Editor, ASMDEF_TMP);
                InsertSpecifyReferenceIntoAssembly(Platform.Runtime, ASMDEF_TMP);
            }
            else
            {
                RemoveSpecifyReferenceFromAssembly(Platform.Editor, ASMDEF_TMP);
                RemoveSpecifyReferenceFromAssembly(Platform.Runtime, ASMDEF_TMP);
            }
        }
#endif
        #endregion

        #region InputSystem Support
#if UNITY_2019_1_OR_NEWER
        //As InputSystem is released in 2019.1+ ,when unity version is 2019.1+ , enable InputSystem Support
        const string SYMBOL_I_S = "INPUT_SYSTEM_ENABLED";
        const string ASMDEF_I_S = "Unity.InputSystem";

#if !INPUT_SYSTEM_ENABLED
        [MenuItem("XCharts/InputSystem Enable")]
#endif
        public static void EnableInputSystem()
        {
            if (!IsSpecifyAssemblyExist(ASMDEF_I_S))
            {
                Debug.LogError("InputSystem is not in the project, please import InputSystem package first.");
                return;
            }
            if (EditorUtility.DisplayDialog("InputSystem Enable", "InputSystem is disabled, do you want to enable it?", "Yes", "Cancel"))
            {
                CheckAsmdefInputSystemReference(true);
                DefineSymbolsUtil.AddGlobalDefine(SYMBOL_I_S);
            }
        }

#if INPUT_SYSTEM_ENABLED
        [MenuItem("XCharts/InputSystem Disable")]
#endif
        public static void DisableInputSystem()
        {
            if (EditorUtility.DisplayDialog("InputSystem Disable", "InputSystem is enabled, do you want to disable it?", "Yes", "Cancel"))
            {
                CheckAsmdefInputSystemReference(false);
                DefineSymbolsUtil.RemoveGlobalDefine(SYMBOL_I_S);
            }
        }

        public static void CheckAsmdefInputSystemReference(bool enable)
        {
            if(enable)
            {
                InsertSpecifyReferenceIntoAssembly(Platform.Editor, ASMDEF_I_S);
                InsertSpecifyReferenceIntoAssembly(Platform.Runtime, ASMDEF_I_S);
            }
            else
            {
                RemoveSpecifyReferenceFromAssembly(Platform.Editor, ASMDEF_I_S);
                RemoveSpecifyReferenceFromAssembly(Platform.Runtime, ASMDEF_I_S);
            }
        }
#endif
        #endregion

        #region Assistant members
#if UNITY_2017_1_OR_NEWER
        // as text mesh pro is released in 2017.1, so we may use these function and types in 2017.1 or later
        private static void InsertSpecifyReferenceIntoAssembly(Platform platform, string reference)
        {
            var file = GetPackageAssemblyDefinitionPath(platform);
            var content = File.ReadAllText(file);
            var data = new AssemblyDefinitionData();
            EditorJsonUtility.FromJsonOverwrite(content, data);
            if (!data.references.Contains(reference))
            {
                data.references.Add(reference);
                var json = EditorJsonUtility.ToJson(data, true);
                File.WriteAllText(file, json);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static void RemoveSpecifyReferenceFromAssembly(Platform platform, string reference)
        {
            var file = GetPackageAssemblyDefinitionPath(platform);
            var content = File.ReadAllText(file);
            var data = new AssemblyDefinitionData();
            EditorJsonUtility.FromJsonOverwrite(content, data);
            if (data.references.Contains(reference))
            {
                data.references.Remove(reference);
                var json = EditorJsonUtility.ToJson(data, true);
                File.WriteAllText(file, json);
            }
        }

        public enum Platform { Editor, Runtime }
        public static string GetPackageAssemblyDefinitionPath(Platform platform)
        {
            var p = platform == Platform.Editor ? "Editor" : "Runtime";
            var f = "XCharts." + p + ".asmdef";
            var sub = Path.Combine(p, f);
            string packagePath = Path.GetFullPath("Packages/com.monitor1394.xcharts");
            if (!Directory.Exists(packagePath))
            {
                packagePath = ADB.FindAssets("t:Script")
                                                   .Where(v => Path.GetFileNameWithoutExtension(ADB.GUIDToAssetPath(v)) == "XChartsMgr")
                                                   .Select(id => ADB.GUIDToAssetPath(id))
                                                   .FirstOrDefault();
                packagePath = Path.GetDirectoryName(packagePath);
                packagePath = packagePath.Substring(0, packagePath.LastIndexOf("Runtime"));
            }
            return Path.Combine(packagePath, sub);
        }

        public static bool IsSpecifyAssemblyExist(string name)
        {
#if UNITY_2018_1_OR_NEWER
            foreach (var assembly in UnityEditor.Compilation.CompilationPipeline.GetAssemblies(UnityEditor.Compilation.AssembliesType.Player))
            {
                if (assembly.name.Equals(name)) return true;
            }
#elif UNITY_2017_3_OR_NEWER
            foreach (var assembly in UnityEditor.Compilation.CompilationPipeline.GetAssemblies())
            {
                if (assembly.name.Equals(name)) return true;
            }
#endif
            return false;
        }

        [Serializable]
        class AssemblyDefinitionData
        {
#pragma warning disable 649
            public string name;
            public List<string> references;
            public List<string> includePlatforms;
            public List<string> excludePlatforms;
            public bool allowUnsafeCode;
            public bool overrideReferences;
            public List<string> precompiledReferences;
            public bool autoReferenced;
            public List<string> defineConstraints;
            public List<string> versionDefines;
            public bool noEngineReferences;
#pragma warning restore 649
        }
#endif
        #endregion


    }
}