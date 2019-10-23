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

        [MenuItem("Component/XCharts/Check For Update")]
        private static void ShowWindow()
        {
            window = GetWindow<CheckVersionEditor>();
            window.titleContent = new GUIContent("XCharts Check For Update");
            window.minSize = new Vector2(550, window.minSize.y);
            window.Show();
            XChartsMgr.Instance.CheckVersion();
        }

        private void OnGUI()
        {
            var mgr = XChartsMgr.Instance;
            GUILayout.Label("");
            GUILayout.Label("当前版本：" + mgr.nowVersion);
            if (mgr.needUpdate && !mgr.isCheck)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("最新版本：" + mgr.newVersion);
                if (mgr.isCheck) GUILayout.Label("检测中...");
                else GUILayout.Label("有更新！");
                if (GUILayout.Button("去Github主页"))
                {
                    Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts");
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("");
                if (!string.IsNullOrEmpty(mgr.desc))
                {
                    GUILayout.Label(mgr.desc);
                    GUILayout.Label("");
                }

                scrollPos = GUILayout.BeginScrollView(scrollPos);
                GUILayout.TextArea(mgr.changeLog);
                GUILayout.EndScrollView();
            }
            else
            {
                if (mgr.isCheck) GUILayout.Label("最新版本：检测中...");
                else GUILayout.Label("最新版本：" + mgr.newVersion);

                if (!mgr.needUpdate && !mgr.isCheck)
                {
                    GUILayout.Label("");
                    GUILayout.Label("已是最新版本！");
                    GUILayout.Label("");
                    if (!string.IsNullOrEmpty(mgr.desc))
                    {
                        GUILayout.Label(mgr.desc);
                        GUILayout.Label("");
                    }
                    if (GUILayout.Button("去Github主页"))
                    {
                        Application.OpenURL(mgr.homepage);
                    }
                }
            }
        }
    }
}