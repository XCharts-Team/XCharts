/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Text;

using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

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

    public class XChartsMgr : MonoBehaviour
    {
        public const string version = "1.0.5";
        public const int date = 20191122;

        [SerializeField] private string m_NowVersion;
        [SerializeField] private string m_NewVersion;

        private static XChartsMgr m_XCharts;

        public static XChartsMgr Instance
        {
            get
            {
                if (m_XCharts == null)
                {
                    var go = GameObject.Find("_xcharts_");
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = "_xcharts_";
                        if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(go);
                        }
                        m_XCharts = go.AddComponent<XChartsMgr>();
                    }
                    else
                    {
                        m_XCharts = go.GetComponent<XChartsMgr>();
                        if (m_XCharts == null)
                        {
                            m_XCharts = go.AddComponent<XChartsMgr>();
                        }
                    }
                    m_XCharts.m_NowVersion = version + " (" + date + ")";
                }
                return m_XCharts;
            }
        }

        private XChartsMgr() { }

        private void Awake()
        {
            SerieLabelPool.ClearAll();
        }

        public string changeLog { get; private set; }
        public string newVersion { get { return m_NewVersion; } }
        public string nowVersion { get { return m_NowVersion; } }
        public string desc { get; private set; }
        public string homepage { get; private set; }
        public int newDate { get; private set; }
        public int newCheckDate { get; private set; }
        public bool isCheck { get; private set; }

        public bool needUpdate
        {
            get
            {
                return date < newCheckDate;
            }
        }

        public void CheckVersion()
        {
            isCheck = true;
            StartCoroutine(GetVersion());
            if (date < newCheckDate)
            {
                StartCoroutine(GetChangeLog());
            }
        }

        IEnumerator GetVersion()
        {
            var url = "https://raw.githubusercontent.com/monitor1394/unity-ugui-XCharts/master/Assets/XCharts/version.json";
            var web = new WWW(url);
            yield return web;
            if (!string.IsNullOrEmpty(web.error))
                Debug.LogError(web.error);
            else
            {
                var cv = JsonUtility.FromJson<XChartsVersion>(web.text);
                m_NewVersion = cv.version + " (" + cv.date + ")";
                newDate = cv.date;
                newCheckDate = cv.checkdate;
                desc = cv.desc;
                homepage = cv.homepage;
                web.Dispose();
                isCheck = false;
            }
        }

        IEnumerator GetChangeLog()
        {
            isCheck = true;
            var url = "https://raw.githubusercontent.com/monitor1394/unity-ugui-XCharts/master/Assets/XCharts/CHANGELOG.md";
            var web = new WWW(url);
            yield return web;
            if (!string.IsNullOrEmpty(web.error))
                Debug.LogError(web.error);
            else
            {
                CheckLog(web.text);
                web.Dispose();
                isCheck = false;
            }
        }

        private void CheckLog(string text)
        {
            StringBuilder sb = new StringBuilder();
            var temp = text.Split('\n');
            var regex = new Regex(".*(\\d{4}\\.\\d{2}\\.\\d{2}).*");
            var checkDate = XChartsMgr.date;
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
    }
}