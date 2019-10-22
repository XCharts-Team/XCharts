
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace XCharts
{
    public class XChartsMgr : MonoBehaviour
    {
        [SerializeField] private string m_Version;
        [SerializeField] private string m_Date;
        
        private XChartsMgr m_XCharts;
        public XChartsMgr Instance
        {
            get
            {
                if (m_XCharts == null)
                {
                    var go = new GameObject();
                    go.name = "_xcharts_";
                    m_XCharts = go.AddComponent<XChartsMgr>();
                    CheckVersion();
                }
                return m_XCharts;
            }
        }

        private XChartsMgr() { }

        private void Awake()
        {
            CheckVersion();
        }

        public void CheckVersion()
        {
            //StartCoroutine(GetVersion());
        }

        IEnumerator GetVersion()
        {
            var url = "https://raw.githubusercontent.com/monitor1394/unity-ugui-XCharts/master/README.md";
            var web = new UnityWebRequest(url);
            Debug.LogError(web.url);
            yield return web.SendWebRequest();
            if (web.isHttpError || web.isNetworkError)
                Debug.LogError(web.error);
            else
            {
                Debug.LogError(web.downloadedBytes);
                Debug.LogError(web.downloadHandler.text);
                web.Dispose();
            }
        }
    }
}