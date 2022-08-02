using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts.Runtime
{
    class XChartsVersion
    {
        public string version = "";
        public int date = 0;
        public int checkdate = 0;
        public string desc = "";
        public string homepage = "";
    }

    [ExecuteInEditMode]
    public static class XChartsMgr
    {
        public static readonly string version = "3.2.0";
        public static readonly int versionDate = 20220815;
        public static string fullVersion { get { return version + "-" + versionDate; } }

        internal static List<BaseChart> chartList = new List<BaseChart>();
        internal static Dictionary<string, Theme> themes = new Dictionary<string, Theme>();
        internal static List<string> themeNames = new List<string>();

        static XChartsMgr()
        {
            SerieLabelPool.ClearAll();
            chartList.Clear();
            if (Resources.Load<XCSettings>("XCSettings"))
                XCThemeMgr.ReloadThemeList();
            SceneManager.sceneUnloaded += OnSceneLoaded;
        }

        static void OnSceneLoaded(Scene scene)
        {
            SerieLabelPool.ClearAll();
        }

        public static void AddChart(BaseChart chart)
        {
            var sameNameChart = GetChart(chart.chartName);
            if (sameNameChart != null)
            {
                var path = ChartHelper.GetFullName(sameNameChart.transform);
                Debug.LogError("A chart named `" + chart.chartName + "` already exists:" + path);
                RemoveChart(chart.chartName);
            }
            if (!ContainsChart(chart))
            {
                chartList.Add(chart);
            }
        }

        public static BaseChart GetChart(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return null;
            return chartList.Find(chart => chartName.Equals(chart.chartName));
        }

        public static List<BaseChart> GetCharts(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return null;
            return chartList.FindAll(chart => chartName.Equals(chart.chartName));
        }

        public static void RemoveChart(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return;
            chartList.RemoveAll(chart => chartName.Equals(chart.chartName));
        }

        public static bool ContainsChart(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return false;
            return GetCharts(chartName) != null;
        }

        public static bool ContainsChart(BaseChart chart)
        {
            return chartList.Contains(chart);
        }

        public static bool IsRepeatChartName(BaseChart chart, string chartName = null)
        {
            if (chartName == null)
                chartName = chart.chartName;
            if (string.IsNullOrEmpty(chartName))
                return false;
            foreach (var temp in chartList)
            {
                if (temp != chart && chartName.Equals(temp.chartName))
                    return true;
            }
            return false;
        }

        public static string GetRepeatChartNameInfo(BaseChart chart, string chartName)
        {
            if (string.IsNullOrEmpty(chartName))
                return string.Empty;
            string result = "";
            foreach (var temp in chartList)
            {
                if (temp != chart && chartName.Equals(temp.chartName))
                    result += ChartHelper.GetFullName(temp.transform) + "\n";
            }
            return result;
        }

        public static void RemoveAllChartObject()
        {
            if (chartList.Count == 0)
            {
                return;
            }
            foreach (var chart in chartList)
            {
                if (chart != null)
                    chart.RebuildChartObject();
            }
        }

#if UNITY_EDITOR
        public static string GetPackageFullPath()
        {
            string packagePath = Path.GetFullPath("Packages/com.monitor1394.xcharts");
            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }
            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                if (File.Exists(packagePath + "/Assets/Packages/XCharts/package.json"))
                {
                    return packagePath + "/Assets/Packages/XCharts";
                }

                if (File.Exists(packagePath + "/Assets/XCharts/package.json"))
                {
                    return packagePath + "/Assets/XCharts";
                }

                string[] matchingPaths = Directory.GetDirectories(packagePath, "XCharts", SearchOption.AllDirectories);
                string path = ValidateLocation(matchingPaths, packagePath);
                if (path != null) return Path.Combine(packagePath, path);
            }
            return null;
        }

        private static string ValidateLocation(string[] paths, string projectPath)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                if (File.Exists(paths[i] + "/package.json"))
                {
                    string folderPath = paths[i].Replace(projectPath, "");
                    folderPath = folderPath.TrimStart('\\', '/');
                    return folderPath;
                }
            }
            return null;
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnEditorReload()
        {
            for (int i = chartList.Count - 1; i >= 0; i--)
            {
                var chart = chartList[i];
                if (chart == null)
                {
                    chartList.RemoveAt(i);
                }
                else
                {
                    chart.InitComponentHandlers();
                    chart.InitSerieHandlers();
                }
            }
        }

        public static void EnableTextMeshPro()
        {
            DefineSymbolsUtil.AddGlobalDefine("dUI_TextMeshPro");
            RemoveAllChartObject();
        }

        public static void DisableTextMeshPro()
        {
            DefineSymbolsUtil.RemoveGlobalDefine("dUI_TextMeshPro");
            RemoveAllChartObject();
        }

        public static bool IsExistTMPAssembly()
        {

#if UNITY_2018_1_OR_NEWER
            foreach (var assembly in UnityEditor.Compilation.CompilationPipeline.GetAssemblies(UnityEditor.Compilation.AssembliesType.Player))
            {
                if (assembly.name.Equals("Unity.TextMeshPro")) return true;
            }
#elif UNITY_2017_3_OR_NEWER
            foreach (var assembly in UnityEditor.Compilation.CompilationPipeline.GetAssemblies())
            {
                if (assembly.name.Equals("Unity.TextMeshPro")) return true;
            }
#endif
            return false;
        }

        public static bool ModifyTMPRefence(bool removeTMP = false)
        {
            var packagePath = GetPackageFullPath();
            if (!ModifyTMPRefence(packagePath + "/Runtime/XCharts.Runtime.asmdef", removeTMP)) return false;
            if (!ModifyTMPRefence(packagePath + "/Editor/XCharts.Editor.asmdef", removeTMP)) return false;
            return true;
        }

        private static bool ModifyTMPRefence(string asmdefPath, bool removeTMP = false)
        {
            if (!File.Exists(asmdefPath))
            {
                Debug.LogError("AddTMPRefence ERROR: can't find: " + asmdefPath);
                return false;
            }
            try
            {
                var dest = new List<string>();
                var refs = new List<string>();
                var lines = File.ReadAllLines(asmdefPath);
                var referencesStart = false;
                var addedTMP = false;
                var removedTMP = false;
                var tmpName = "\"Unity.TextMeshPro\"";
                var refCount = 0;
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
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
                            if (refCount > 0)
                            {
                                var old = dest[dest.Count - 1];
                                if (old.EndsWith(","))
                                    dest[dest.Count - 1] = old.Substring(0, old.Length - 1);
                            }
                            if (!removeTMP && !refs.Contains(tmpName))
                            {
                                if (refs.Count > 0)
                                    dest[dest.Count - 1] = dest[dest.Count - 1] + ",";
                                dest.Add("        " + tmpName);
                                dest.Add(line);
                                addedTMP = true;
                            }
                            else
                            {
                                dest.Add(line);
                            }
                        }
                        else
                        {
                            if (removeTMP)
                            {
                                if (!line.Contains(tmpName))
                                {
                                    dest.Add(line);
                                    refCount++;
                                }
                                else
                                {
                                    removedTMP = true;
                                }
                            }
                            else
                            {
                                dest.Add(line);
                                refs.Add(line.Trim());
                            }
                        }
                    }
                    else
                    {
                        dest.Add(line);
                    }
                }
                if (addedTMP || removedTMP)
                {
                    File.WriteAllText(asmdefPath, string.Join("\n", dest.ToArray()));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("AddTMPRefence ERROR:" + e.Message);
                return false;
            }
        }
#endif
    }
}