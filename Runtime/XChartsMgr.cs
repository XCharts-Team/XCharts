

/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts
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
    public class XChartsMgr : MonoBehaviour
    {
        internal static string _version = "2.8.2";
        internal static int _versionDate = 20220815;
        public static string version { get { return _version; } }
        public static int versionDate { get { return _versionDate; } }
        public static string fullVersion { get { return version + "-" + versionDate; } }

        [SerializeField] private string m_NowVersion;
        [SerializeField] private string m_NewVersion;
        [SerializeField] private List<BaseChart> m_ChartList = new List<BaseChart>();
        [SerializeField] internal Dictionary<string, ChartTheme> m_ThemeDict = new Dictionary<string, ChartTheme>();
        [SerializeField] internal List<string> m_ThemeNames = new List<string>();
        private static XChartsMgr m_XCharts;

        public static XChartsMgr Instance
        {
            get
            {
                if (m_XCharts == null)
                {
                    m_XCharts = FindObjectOfType<XChartsMgr>();
                    if (m_XCharts == null)
                    {
                        var obj = GameObject.Find("_xcharts_");
                        if (obj == null) obj = new GameObject("_xcharts_");
                        obj.SetActive(false);
                        m_XCharts = obj.AddComponent<XChartsMgr>();
                        obj.SetActive(true);
                    }
                    m_XCharts.m_NowVersion = fullVersion;
                }
                return m_XCharts;
            }
        }

        private void Awake()
        {
            if (m_XCharts != null)
            {
                GameObject.DestroyImmediate(this);
                return;
            }
            SerieLabelPool.ClearAll();
            m_ChartList.Clear();
            XThemeMgr.ReloadThemeList();
        }

        public string changeLog { get; private set; }
        public string newVersion { get { return m_NewVersion; } }
        public string nowVersion { get { return m_NowVersion; } }
        public string desc { get; private set; }
        public string homepage { get; private set; }
        public int newDate { get; private set; }
        public int newCheckDate { get; private set; }
        public bool isCheck { get; private set; }
        public bool isNetworkError { get; private set; }
        public string networkError { get; private set; }

        public bool needUpdate
        {
            get
            {
                return !isNetworkError && newDate > versionDate;
            }
        }

        public void CheckVersion()
        {
            StartCoroutine(GetVersion());
        }

        IEnumerator GetVersion()
        {
            isCheck = true;
            isNetworkError = false;
            networkError = "";
            var url = "https://raw.githubusercontent.com/monitor1394/unity-ugui-XCharts/master/Assets/XCharts/package.json";
            var web = UnityWebRequest.Get(url);
#if UNITY_2017_3_OR_NEWER
            yield return web.SendWebRequest();
#else
            yield return web.Send();
#endif
            CheckVersionWebRequest(web);
            if (isNetworkError)
            {
                url = "https://gitee.com/monitor1394/unity-ugui-XCharts/raw/master/Assets/XCharts/package.json";
                web = UnityWebRequest.Get(url);
#if UNITY_2017_3_OR_NEWER
                yield return web.SendWebRequest();
#else
                yield return web.Send();
#endif
                CheckVersionWebRequest(web);
            }
            if (needUpdate)
            {
                url = "https://raw.githubusercontent.com/monitor1394/unity-ugui-XCharts/master/Assets/XCharts/CHANGELOG.md";
                web = UnityWebRequest.Get(url);
#if UNITY_2017_3_OR_NEWER
                yield return web.SendWebRequest();
#else
                yield return web.Send();
#endif
                if (!CheckLogWebRequest(web))
                {
                    url = "https://gitee.com/monitor1394/unity-ugui-XCharts/raw/master/Assets/XCharts/CHANGELOG.md";
                    web = UnityWebRequest.Get(url);
#if UNITY_2017_3_OR_NEWER
                    yield return web.SendWebRequest();
#else
                    yield return web.Send();
#endif
                    CheckLogWebRequest(web);
                }
            }
            isCheck = false;
        }

        private void CheckVersionWebRequest(UnityWebRequest web)
        {
            if (IsWebRequestError(web))
            {
                isNetworkError = true;
                networkError = web.error;
                m_NewVersion = "-";
            }
            else if (web.responseCode == 200)
            {
                isNetworkError = false;
                var cv = JsonUtility.FromJson<XChartsVersion>(web.downloadHandler.text);
                m_NewVersion = cv.version + "-" + cv.date;
                newDate = cv.date;
                newCheckDate = cv.checkdate;
                desc = cv.desc;
                homepage = cv.homepage;
            }
            else
            {
                isNetworkError = true;
                if (web.responseCode > 0)
                    networkError = web.responseCode.ToString();
                if (!string.IsNullOrEmpty(web.error))
                    networkError += "," + web.error;
                if (string.IsNullOrEmpty(networkError))
                {
                    networkError = "-";
                }
                m_NewVersion = "-";
            }
            web.Dispose();
        }

        private bool CheckLogWebRequest(UnityWebRequest web)
        {
            bool success = false;
            if (web.responseCode == 200)
            {
                CheckLog(web.downloadHandler.text);
                success = true;
            }
            web.Dispose();
            return success;
        }

        private void CheckLog(string text)
        {
            StringBuilder sb = new StringBuilder();
            var temp = text.Split('\n');
            var regex = new Regex(".*(\\d{4}\\.\\d{2}\\.\\d{2}).*");
            var checkDate = versionDate;
            foreach (var t in temp)
            {
                if (regex.IsMatch(t))
                {
                    var mat = regex.Match(t);
                    var date = mat.Groups[1].ToString().Replace(".", "");
                    int logDate;
                    if (int.TryParse(date, out logDate))
                    {
                        if (logDate >= checkDate)
                        {
                            sb.Append(t).Append("\n");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    sb.Append(t).Append("\n");
                }
            }
            changeLog = sb.ToString();
        }

        public bool IsWebRequestError(UnityWebRequest request)
        {
#if UNITY_5
            return request.isError && request.responseCode < 400;
#elif UNITY_2017_1
             return request.isError && !request.isHttpError;
#elif UNITY_2020_2
            return (int)request.result > 1;
#else
            return request.isNetworkError;
#endif
        }

        void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene)
        {
            SerieLabelPool.ClearAll();
        }

        public void AddChart(BaseChart chart)
        {
            var sameNameChart = GetChart(chart.chartName);
            if (sameNameChart != null)
            {
                var path = ChartHelper.GetFullName(sameNameChart.transform);
                Debug.LogError("A chart named `" + chart.chartName + "` already exists:" + path);
            }
            if (!ContainsChart(chart))
            {
                m_ChartList.Add(chart);
            }
        }

        public BaseChart GetChart(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return null;
            return m_ChartList.Find(chart => chartName.Equals(chart.chartName));
        }

        public List<BaseChart> GetCharts(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return null;
            return m_ChartList.FindAll(chart => chartName.Equals(chart.chartName));
        }

        public void RemoveChart(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return;
            m_ChartList.RemoveAll(chart => chartName.Equals(chart.chartName));
        }

        public bool ContainsChart(string chartName)
        {
            if (string.IsNullOrEmpty(chartName)) return false;
            return GetCharts(chartName) != null;
        }

        public bool ContainsChart(BaseChart chart)
        {
            return m_ChartList.Contains(chart);
        }

        public bool IsRepeatChartName(BaseChart chart, string chartName = null)
        {
            if(chartName == null)
                chartName = chart.chartName;
            if (string.IsNullOrEmpty(chartName))
                return false;
            foreach (var temp in m_ChartList)
            {
                if (temp != chart && chartName.Equals(temp.chartName))
                    return true;
            }
            return false;
        }

        public string GetRepeatChartNameInfo(BaseChart chart, string chartName)
        {
            if (string.IsNullOrEmpty(chartName))
                return string.Empty;
            string result = "";
            foreach (var temp in m_ChartList)
            {
                if (temp != chart && chartName.Equals(temp.chartName))
                    result += ChartHelper.GetFullName(temp.transform) + "\n";
            }
            return result;
        }

        public static void RemoveAllChartObject()
        {
            if (Instance.m_ChartList.Count == 0)
            {
                return;
            }
            foreach (var chart in Instance.m_ChartList)
            {
                if (chart != null)
                    chart.RemoveChartObject();
            }
        }

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
                // Search default location for development package
                if (File.Exists(packagePath + "/Assets/Packages/com.monitor1394.xcharts/package.json"))
                {
                    return packagePath + "/Assets/Packages/com.monitor1394.xcharts";
                }

                // Search for default location of normal XCharts AssetStore package
                if (File.Exists(packagePath + "/Assets/XCharts/package.json"))
                {
                    return packagePath + "/Assets/XCharts";
                }

                // Search for potential alternative locations in the user project
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

#if UNITY_EDITOR
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
            var packagePath = XChartsMgr.GetPackageFullPath();
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
