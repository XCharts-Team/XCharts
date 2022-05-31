#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class XCResourcesImporter
    {
        bool m_EssentialResourcesImported;

        public XCResourcesImporter() { }

        public void OnDestroy() { }

        public void OnGUI()
        {
            m_EssentialResourcesImported = Resources.Load<XCSettings>("XCSettings") != null;

            GUILayout.BeginVertical();
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    GUILayout.Label("XCharts Essentials", EditorStyles.boldLabel);
                    GUILayout.Label("This appears to be the first time you access XCharts, as such we need to add resources to your project that are essential for using XCharts. These new resources will be placed at the root of your project in the \"XCharts\" folder.", new GUIStyle(EditorStyles.label) { wordWrap = true });
                    GUILayout.Space(5f);

                    GUI.enabled = !m_EssentialResourcesImported;
                    GUI.enabled = true;
                    if (GUILayout.Button("Import XCharts Essentials"))
                    {
                        string packageFullPath = XChartsMgr.GetPackageFullPath();
                        if (packageFullPath != null)
                        {
                            var sourPath = Path.Combine(packageFullPath, "Resources");
                            var destPath = Path.Combine(Application.dataPath, "XCharts/Resources");
                            if (CopyFolder(sourPath, destPath))
                            {
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
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

        private static bool CopyFolder(string sourPath, string destPath)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                var files = Directory.GetFiles(sourPath);
                foreach (var file in files)
                {
                    var name = Path.GetFileName(file);
                    var path = Path.Combine(destPath, name);
                    File.Copy(file, path);
                }
                var folders = Directory.GetDirectories(sourPath);
                foreach (var folder in folders)
                {
                    var name = Path.GetFileName(folder);
                    var path = Path.Combine(destPath, name);
                    CopyFolder(folder, path);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("CopyFolder:" + e.Message);
                return false;
            }
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
    }

    public class XCResourceImporterWindow : UnityEditor.EditorWindow
    {
        [SerializeField] XCResourcesImporter m_ResourceImporter;

        static XCResourceImporterWindow m_ImporterWindow;

        public static void ShowPackageImporterWindow()
        {
            var packagePath = XChartsMgr.GetPackageFullPath();
            if (packagePath != null)
            {
                if (m_ImporterWindow == null)
                {
                    m_ImporterWindow = GetWindow<XCResourceImporterWindow>();
                    m_ImporterWindow.titleContent = new GUIContent("XCharts Importer");
                }
                m_ImporterWindow.Focus();
            }
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
        /// |</summary>
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