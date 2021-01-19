

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

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [ExecuteInEditMode]
    public class XChartsMgr : MonoBehaviour
    {
        internal static string _version = "2.0.0-preview.1";
        internal static int _versionDate = 20210119;
        public static string version { get { return _version; } }
        public static int versionDate { get { return _versionDate; } }
        public static string fullVersion { get { return version + "-" + versionDate; } }

        [SerializeField] private string m_NowVersion;
        [SerializeField] private string m_NewVersion;
        [SerializeField] private List<BaseChart> m_ChartList = new List<BaseChart>();
        [SerializeField] private Dictionary<string, ChartTheme> m_ThemeDict = new Dictionary<string, ChartTheme>();

        private List<string> m_ThemeNames = new List<string>();
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
                        GameObject obj = new GameObject("_xcharts_");
                        m_XCharts = obj.AddComponent<XChartsMgr>();
                    }
                    m_XCharts.m_NowVersion = fullVersion;
                }
                return m_XCharts;
            }
        }

        private XChartsMgr()
        {
        }

        static XChartsMgr()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += () =>
            {
                var mgr = XChartsMgr.Instance;
            };
#endif
        }

        private void Awake()
        {
            SerieLabelPool.ClearAll();
            m_ChartList.Clear();
            m_ThemeDict.Clear();
            LoadThemesFromResources();
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
                return !isNetworkError && !m_NowVersion.Equals(m_NewVersion);
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
#if UNITY_5
            yield return web.Send();
#else
            yield return web.SendWebRequest();
#endif
            CheckVersionWebRequest(web);
            if (isNetworkError)
            {
                url = "https://gitee.com/monitor1394/unity-ugui-XCharts/raw/master/Assets/XCharts/package.json";
                web = UnityWebRequest.Get(url);
#if UNITY_5
                yield return web.Send();
#else
                yield return web.SendWebRequest();
#endif
                CheckVersionWebRequest(web);
            }
            if (needUpdate)
            {
                url = "https://raw.githubusercontent.com/monitor1394/unity-ugui-XCharts/master/Assets/XCharts/CHANGELOG.md";
                web = UnityWebRequest.Get(url);
#if UNITY_5
                yield return web.Send();
#else
                yield return web.SendWebRequest();
#endif
                if (!CheckLogWebRequest(web))
                {
                    url = "https://gitee.com/monitor1394/unity-ugui-XCharts/raw/master/Assets/XCharts/CHANGELOG.md";
                    web = UnityWebRequest.Get(url);
#if UNITY_5
                    yield return web.Send();
#else
                    yield return web.SendWebRequest();
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
            return request.isError && ! request.responseCode >= 400;
#elif UNITY_2017_1
             return request.isError && ! request.isHttpError;
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

        public void LoadThemesFromResources()
        {
            //Debug.Log("LoadThemesFromResources");
            m_ThemeDict.Clear();
            AddTheme(ChartTheme.Default);
            AddTheme(ChartTheme.Light);
            AddTheme(ChartTheme.Dark);
            var list = Resources.LoadAll<ChartTheme>("");
            foreach (var theme in list)
            {
                AddTheme(theme);
            }
            //Debug.Log("LoadThemesFromResources DONE: theme count=" + m_ThemeDict.Keys.Count);
        }

        private void AddTheme(ChartTheme theme)
        {
            if (!m_ThemeDict.ContainsKey(theme.themeName))
            {
                m_ThemeDict.Add(theme.themeName, theme);
                m_ThemeNames.Add(theme.themeName);
            }
            else
            {
                Debug.LogError("Theme name is exist:" + theme.themeName);
            }
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
            return m_ChartList.FindAll(chart => chartName.Equals(chartName));
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

        public static List<string> GetAllThemeNames()
        {
            return Instance.m_ThemeNames;
        }

        public static bool ContainsTheme(string themeName)
        {
            return Instance.m_ThemeNames.Contains(themeName);
        }

        public static void SwitchTheme(BaseChart chart, string themeName)
        {
            Debug.Log("SwitchTheme:" + themeName);
            if (chart.theme.themeName.Equals(themeName))
            {
                return;
            }
#if UNITY_EDITOR
            if (Instance.m_ThemeDict.Count == 0)
            {
                Instance.LoadThemesFromResources();
            }
#endif
            if (!Instance.m_ThemeDict.ContainsKey(themeName))
            {
                Debug.LogError("SwitchTheme ERROR: not exist theme:" + themeName);
                return;
            }
            var target = Instance.m_ThemeDict[themeName];
            chart.theme.CopyTheme(target);
            chart.RefreshAllComponent();
        }

        public static string GetThemeAssetPath(string themeName)
        {
            return string.Format("Assets/XCharts/Resources/XChartsTheme-{0}.asset", themeName);
        }

        public static bool ExportTheme(ChartTheme theme, string themeNewName)
        {
#if UNITY_EDITOR
            var newtheme = ChartTheme.EmptyTheme;
            newtheme.CopyTheme(theme);
            newtheme.name = themeNewName;
            newtheme.themeName = themeNewName;

            var themeFileName = "XChartsTheme-" + newtheme.themeName;
            var assetPath = string.Format("Assets/XCharts/Resources/{0}.asset", themeFileName);
            if (Resources.Load<XChartsSettings>(themeFileName))
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
            AssetDatabase.CreateAsset(newtheme, assetPath);
            Instance.AddTheme(newtheme);
            return true;
#else
            return false;
#endif
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
    }
}