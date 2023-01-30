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
    public class XChartsEditor : UnityEditor.Editor
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

        public static T AddChart<T>(string chartName) where T : BaseChart
        {
            XCThemeMgr.CheckReloadTheme();
            return AddGraph<T>(chartName);
        }

        public static T AddGraph<T>(string graphName) where T : BaseGraph
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
            var chart = AddChart<BaseChart>("EmptyChart");
            chart.GetChartComponent<Title>().text = "EmptyChart";
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

        [MenuItem("XCharts/RingChart", priority = 51)]
        [MenuItem("GameObject/XCharts/RingChart", priority = 51)]
        public static void AddRingChart()
        {
            AddChart<RingChart>("RingChart");
        }

        [MenuItem("XCharts/CandlestickChart", priority = 54)]
        [MenuItem("GameObject/XCharts/CandlestickChart", priority = 54)]
        public static void CandlestickChart()
        {
            AddChart<CandlestickChart>("CandlestickChart");
        }

        [MenuItem("XCharts/PolarChart", priority = 54)]
        [MenuItem("GameObject/XCharts/PolarChart", priority = 54)]
        public static void PolarChart()
        {
            AddChart<PolarChart>("PolarChart");
        }

        [MenuItem("XCharts/ParallelChart", priority = 55)]
        [MenuItem("GameObject/XCharts/ParallelChart", priority = 55)]
        public static void ParallelChart()
        {
            AddChart<ParallelChart>("ParallelChart");
        }

        [MenuItem("XCharts/SimplifiedLineChart", priority = 56)]
        [MenuItem("GameObject/XCharts/SimplifiedLineChart", priority = 56)]
        public static void SimplifiedLineChart()
        {
            AddChart<SimplifiedLineChart>("SimplifiedLineChart");
        }

        [MenuItem("XCharts/SimplifiedBarChart", priority = 57)]
        [MenuItem("GameObject/XCharts/SimplifiedBarChart", priority = 57)]
        public static void SimplifiedBarChart()
        {
            AddChart<SimplifiedBarChart>("SimplifiedBarChart");
        }

        [MenuItem("XCharts/SimplifiedCandlestickChart", priority = 58)]
        [MenuItem("GameObject/XCharts/SimplifiedCandlestickChart", priority = 58)]
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

        [MenuItem("XCharts/TextMeshPro Enable")]
        public static void EnableTextMeshPro()
        {
            if (!IsSpecifyAssemblyExist(ASMDEF_TMP))
            {
                Debug.LogError("TextMeshPro is not in the project, please import TextMeshPro package first.");
                return;
            }
            DefineSymbolsUtil.AddGlobalDefine(SYMBOL_TMP);
            XChartsMgr.RemoveAllChartObject();
            InsertSpecifyReferenceIntoAssembly(Platform.Editor, ASMDEF_TMP);
            InsertSpecifyReferenceIntoAssembly(Platform.Runtime, ASMDEF_TMP);
        }

        [MenuItem("XCharts/TextMeshPro Disable")]
        public static void DisableTextMeshPro()
        {
            RemoveSpecifyReferenceFromAssembly(Platform.Editor, ASMDEF_TMP);
            RemoveSpecifyReferenceFromAssembly(Platform.Runtime, ASMDEF_TMP);
            DefineSymbolsUtil.RemoveGlobalDefine(SYMBOL_TMP);
            XChartsMgr.RemoveAllChartObject();
        }
#endif
        #endregion

        #region InputSystem Support
#if UNITY_2019_1_OR_NEWER
        //As InputSystem is released in 2019.1+ ,when unity version is 2019.1+ , enable InputSystem Support
        const string SYMBOL_I_S = "INPUT_SYSTEM_ENABLED";
        const string ASMDEF_I_S = "Unity.InputSystem";
        [MenuItem("XCharts/InputSystem Enable")]
        public static void EnableInputSystem()
        {
            if (!IsSpecifyAssemblyExist(ASMDEF_I_S))
            {
                Debug.LogError("InputSystem is not in the project, please import InputSystem package first.");
                return;
            }
            // insert input system package into editor and runtime assembly
            InsertSpecifyReferenceIntoAssembly(Platform.Editor, ASMDEF_I_S);
            InsertSpecifyReferenceIntoAssembly(Platform.Runtime, ASMDEF_I_S);
            // add scripting define symbols
            DefineSymbolsUtil.AddGlobalDefine(SYMBOL_I_S);
        }
        [MenuItem("XCharts/InputSystem Disable")]
        public static void DisableInputSystem()
        {
            // remove input system package into editor and runtime assembly
            RemoveSpecifyReferenceFromAssembly(Platform.Editor, ASMDEF_I_S);
            RemoveSpecifyReferenceFromAssembly(Platform.Runtime, ASMDEF_I_S);
            // remove scripting define symbols
            DefineSymbolsUtil.RemoveGlobalDefine(SYMBOL_I_S);
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