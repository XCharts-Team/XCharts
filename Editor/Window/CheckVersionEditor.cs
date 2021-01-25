/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    public class CheckVersionEditor : EditorWindow
    {
        private Vector2 scrollPos;
        private static CheckVersionEditor window;

        [MenuItem("XCharts/Upgrade Check")]
        public static void ShowWindow()
        {
            window = GetWindow<CheckVersionEditor>();
            window.titleContent = new GUIContent("XCharts Upgrade Check");
            window.minSize = new Vector2(550, window.minSize.y);
            window.Show();
            XChartsMgr.Instance.CheckVersion();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            var mgr = XChartsMgr.Instance;
            GUILayout.Label("");
            GUILayout.Label("The current version: " + mgr.nowVersion);
            if (mgr.needUpdate && !mgr.isCheck)
            {
                GUILayout.Label("The remote version: " + mgr.newVersion);
                GUILayout.Label("");
                if (mgr.isCheck) GUILayout.Label("check ...");
                else if (mgr.isNetworkError) GUILayout.Label("check failed: " + mgr.networkError);
                else
                {
                    GUILayout.Label("There is a new version to upgrade!");
                }

                GUILayout.Label("");
                if (!string.IsNullOrEmpty(mgr.desc))
                {
                    GUILayout.Label(mgr.desc);
                }
                if (GUILayout.Button("Github Homepage"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts");
                }
                if (GUILayout.Button("Star Support"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/stargazers");
                }
                if (GUILayout.Button("Issues"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/issues");
                }
                if (!string.IsNullOrEmpty(mgr.changeLog))
                {
                    scrollPos = GUILayout.BeginScrollView(scrollPos);
                    GUILayout.TextArea(mgr.changeLog);
                    GUILayout.EndScrollView();
                }
            }
            else
            {
                if (mgr.isCheck) GUILayout.Label("The remote version: checking ...");
                else if (mgr.isNetworkError) GUILayout.Label("check failed: " + mgr.networkError);
                else GUILayout.Label("The remote version: " + mgr.newVersion);

                GUILayout.Label("");
                if (!mgr.isNetworkError && !mgr.needUpdate && !mgr.isCheck)
                {
                    GUILayout.Label("It is the latest version!");
                }
                GUILayout.Label("");
                if (!string.IsNullOrEmpty(mgr.desc))
                {
                    GUILayout.Label(mgr.desc);
                }
                if (GUILayout.Button("Github Homepage"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts");
                }
                if (GUILayout.Button("Star Support"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/stargazers");
                }
                if (GUILayout.Button("Issues"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/issues");
                }
                if (mgr.isNetworkError && GUILayout.Button("Check Again"))
                {
                    XChartsMgr.Instance.CheckVersion();
                }
            }
        }
    }
}