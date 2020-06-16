/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    public class CheckVersionEditor : EditorWindow
    {
        private Vector2 scrollPos;
        private static CheckVersionEditor window;

        [MenuItem("XCharts/Check For Update")]
        public static void ShowWindow()
        {
            window = GetWindow<CheckVersionEditor>();
            window.titleContent = new GUIContent("XCharts Check For Update");
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
            GUILayout.Label("当前版本：" + mgr.nowVersion);
            if (mgr.needUpdate && !mgr.isCheck)
            {
                GUILayout.Label("最新版本：" + mgr.newVersion);
                GUILayout.Label("");
                if (mgr.isCheck) GUILayout.Label("检测中...");
                else if (mgr.isNetworkError) GUILayout.Label("检测失败：" + mgr.networkError);
                else
                {
                    GUILayout.Label("有更新！");
                }

                GUILayout.Label("");
                if (!string.IsNullOrEmpty(mgr.desc))
                {
                    GUILayout.Label(mgr.desc);
                }
                if (GUILayout.Button("去Github主页"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts");
                }
                if (GUILayout.Button("点Star支持"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/stargazers");
                }
                if (GUILayout.Button("问题反馈"))
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
                if (mgr.isCheck) GUILayout.Label("最新版本：检测中...");
                else if (mgr.isNetworkError) GUILayout.Label("检测失败：" + mgr.networkError);
                else GUILayout.Label("最新版本：" + mgr.newVersion);

                GUILayout.Label("");
                if (!mgr.isNetworkError && !mgr.needUpdate && !mgr.isCheck)
                {
                    GUILayout.Label("已是最新版本！");
                }
                GUILayout.Label("");
                if (!string.IsNullOrEmpty(mgr.desc))
                {
                    GUILayout.Label(mgr.desc);
                }
                if (GUILayout.Button("去Github主页"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts");
                }
                if (GUILayout.Button("点Star支持"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/stargazers");
                }
                if (GUILayout.Button("问题反馈"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts/issues");
                }
                if (mgr.isNetworkError && GUILayout.Button("重新检测"))
                {
                    XChartsMgr.Instance.CheckVersion();
                }
            }
        }
    }
}