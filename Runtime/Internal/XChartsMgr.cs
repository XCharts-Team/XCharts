using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
#if UNITY_EDITOR
using ADB = UnityEditor.AssetDatabase;
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
        public static readonly string version = "3.9.0";
        public static readonly int versionDate = 20231201;
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
            var list = GetCharts(chartName);
            return list != null && list.Count > 0;
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
            packagePath = ADB.FindAssets("t:Script")
                                              .Where(v => Path.GetFileNameWithoutExtension(ADB.GUIDToAssetPath(v)) == "XChartsMgr")
                                              .Select(id => ADB.GUIDToAssetPath(id))
                                              .FirstOrDefault();
            packagePath = Path.GetDirectoryName(packagePath);
            packagePath = packagePath.Substring(0, packagePath.LastIndexOf("Runtime"));
            return packagePath;
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
#endif
    }
}