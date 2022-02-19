#if UNITY_EDITOR

using System;
using System.IO;
using UnityEngine;
using UnityEditor;


namespace XCharts.Runtime
{
    [System.Serializable]
    public class XCResourcesImporter
    {
        bool m_EssentialResourcesImported;

        public XCResourcesImporter() { }

        public void OnDestroy()
        {
        }

        public void OnGUI()
        {
            m_EssentialResourcesImported = File.Exists("Assets/XCharts/Resources/XCSettings.asset");

            GUILayout.BeginVertical();
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    GUILayout.Label("XCharts Essentials", EditorStyles.boldLabel);
                    GUILayout.Label("This appears to be the first time you access XCharts, as such we need to add resources to your project that are essential for using XCharts. These new resources will be placed at the root of your project in the \"XCharts\" folder.", new GUIStyle(EditorStyles.label) { wordWrap = true });
                    GUILayout.Space(5f);

                    GUI.enabled = !m_EssentialResourcesImported;
                    if (GUILayout.Button("Import XCharts Essentials"))
                    {
                        string packageFullPath = GetPackageFullPath();
                        var sourPath = Path.Combine(packageFullPath, "Resources");
                        var destPath = Path.Combine(Application.dataPath, "XCharts/Resources");
                        if (RuntimeUtil.CopyFolder(sourPath, destPath))
                        {
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                    GUILayout.Space(5f);
                    GUI.enabled = true;
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            GUILayout.Space(5f);
        }

        internal void RegisterResourceImportCallback()
        {
            AssetDatabase.importPackageCompleted += ImportCallback;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="packageName"></param>
        void ImportCallback(string packageName)
        {
            if (packageName == "XCharts Essential Resources")
            {
                m_EssentialResourcesImported = true;
#if UNITY_2018_3_OR_NEWER
                SettingsService.NotifySettingsProviderChanged();
#endif
            }
            Debug.Log("[" + packageName + "] have been imported.");

            AssetDatabase.importPackageCompleted -= ImportCallback;
        }

        static string GetPackageFullPath()
        {
            // Check for potential UPM package
            string packagePath = Path.GetFullPath("Packages/com.monitor1394.xcharts");
            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                // Search default location for development package
                if (Directory.Exists(packagePath + "/Assets/Packages/com.monitor1394.xcharts/Resources"))
                {
                    return packagePath + "/Assets/Packages/com.monitor1394.xcharts";
                }

                // Search for default location of normal XCharts AssetStore package
                if (Directory.Exists(packagePath + "/Assets/XCharts/Resources"))
                {
                    return packagePath + "/Assets/XCharts";
                }

                // Search for potential alternative locations in the user project
                string[] matchingPaths = Directory.GetDirectories(packagePath, "XCharts", SearchOption.AllDirectories);
                string path = ValidateLocation(matchingPaths, packagePath);
                if (path != null) return packagePath + path;
            }

            return null;
        }

        static string ValidateLocation(string[] paths, string projectPath)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                // Check if the Editor Resources folder exists.
                if (Directory.Exists(paths[i] + "/Package Resources"))
                {
                    string folderPath = paths[i].Replace(projectPath, "");
                    folderPath = folderPath.TrimStart('\\', '/');
                    return folderPath;
                }
            }

            return null;
        }
    }

    public class XChartsPackageResourceImporterWindow : UnityEditor.EditorWindow
    {
        [SerializeField]
        XCResourcesImporter m_ResourceImporter;

        static XChartsPackageResourceImporterWindow m_ImporterWindow;

        public static void ShowPackageImporterWindow()
        {
            if (m_ImporterWindow == null)
            {
                m_ImporterWindow = GetWindow<XChartsPackageResourceImporterWindow>();
                m_ImporterWindow.titleContent = new GUIContent("XCharts Importer");
            }

            m_ImporterWindow.Focus();
        }

        void OnEnable()
        {
            SetEditorWindowSize();

            if (m_ResourceImporter == null)
                m_ResourceImporter = new XCResourcesImporter();
        }

        void OnDestroy()
        {
            m_ResourceImporter.OnDestroy();
        }

        void OnGUI()
        {
            m_ResourceImporter.OnGUI();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// Limits the minimum size of the editor window.
        /// </summary>
        void SetEditorWindowSize()
        {
            EditorWindow editorWindow = this;

            Vector2 windowSize = new Vector2(640, 210);
            editorWindow.minSize = windowSize;
            editorWindow.maxSize = windowSize;
        }
    }
}

#endif
