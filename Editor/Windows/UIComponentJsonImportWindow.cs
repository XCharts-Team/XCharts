using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public class UIComponentJsonImportWindow : EditorWindow
    {
        private const int TEXTAREA_SAFE_CHAR_LIMIT = 8000;
        private const int LARGE_JSON_PREVIEW_CHAR_LIMIT = 4000;

        private static UIComponent s_TargetComponent;
        private string m_JsonInput = "";
        private Vector2 m_ScrollPos;
        private bool m_ShowPreview = false;
        private string m_PreviewText = "";
        private bool m_OpenFilePending = false;
        private bool m_PreviewPending = false;
        private bool m_ImportPending = false;

        public static void ShowWindow(UIComponent target)
        {
            s_TargetComponent = target;
            var window = GetWindow<UIComponentJsonImportWindow>("Import UI JSON");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }

        private void OnGUI()
        {
            if (s_TargetComponent == null)
            {
                EditorGUILayout.HelpBox("Target UI component is null. Please select a component first.", MessageType.Error);
                if (GUILayout.Button("Close")) Close();
                return;
            }

            if (m_JsonInput == null) m_JsonInput = "";
            EditorGUILayout.LabelField("Target: " + s_TargetComponent.gameObject.name + " (" + s_TargetComponent.GetType().Name + ")", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Paste JSON Data:", EditorStyles.boldLabel);

            using (var scroll = new EditorGUILayout.ScrollViewScope(m_ScrollPos, GUILayout.Height(250)))
            {
                m_ScrollPos = scroll.scrollPosition;
                if (m_JsonInput.Length <= TEXTAREA_SAFE_CHAR_LIMIT)
                {
                    m_JsonInput = EditorGUILayout.TextArea(m_JsonInput, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                }
                else
                {
                    var preview = m_JsonInput.Substring(0, LARGE_JSON_PREVIEW_CHAR_LIMIT);
                    EditorGUILayout.HelpBox("JSON content is very large. Only a preview is shown below. Import uses full content.", MessageType.Info);
                    EditorGUILayout.TextArea(preview, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                }
            }

            EditorGUILayout.HelpBox("Paste JSON directly, or click Open Json File.", MessageType.Info);
            GUILayout.Space(10);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Open Json File", GUILayout.Width(120))) RequestOpenJsonFile();
                if (GUILayout.Button("Preview", GUILayout.Width(100))) RequestPreviewJson();
            }

            if (m_ShowPreview && !string.IsNullOrEmpty(m_PreviewText))
            {
                EditorGUILayout.TextArea(m_PreviewText, GUILayout.Height(120));
            }

            GUILayout.Space(10);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Import", GUILayout.Height(40), GUILayout.Width(150))) RequestImportJson();
                if (GUILayout.Button("Cancel", GUILayout.Height(40), GUILayout.Width(150))) Close();
            }
        }

        private void RequestOpenJsonFile()
        {
            if (m_OpenFilePending) return;
            m_OpenFilePending = true;
            EditorApplication.delayCall += delegate ()
            {
                m_OpenFilePending = false;
                if (this == null) return;
                OpenJsonFile();
                Repaint();
            };
            GUIUtility.ExitGUI();
        }

        private void RequestPreviewJson()
        {
            if (m_PreviewPending) return;
            m_PreviewPending = true;
            EditorApplication.delayCall += delegate ()
            {
                m_PreviewPending = false;
                if (this == null) return;
                PreviewJson();
                Repaint();
            };
            GUIUtility.ExitGUI();
        }

        private void RequestImportJson()
        {
            if (m_ImportPending) return;
            m_ImportPending = true;
            EditorApplication.delayCall += delegate ()
            {
                m_ImportPending = false;
                if (this == null) return;
                ImportJson();
            };
            GUIUtility.ExitGUI();
        }

        private void PreviewJson()
        {
            if (string.IsNullOrEmpty(m_JsonInput))
            {
                EditorUtility.DisplayDialog("Error", "JSON input is empty.", "OK");
                return;
            }

            try
            {
                var json = JsonUtility.FromJson<UIComponentJson>(m_JsonInput);
                if (json == null)
                    m_PreviewText = "Invalid JSON or unsupported schema.";
                else
                    m_PreviewText = "Component Type: " + json.componentType + "\nSchema: " + json.schemaVersion + "\nVersion: " + json.componentVersion;
                m_ShowPreview = true;
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("Preview Error", "Invalid JSON: " + ex.Message, "OK");
            }
        }

        private void ImportJson()
        {
            if (string.IsNullOrEmpty(m_JsonInput))
            {
                EditorUtility.DisplayDialog("Error", "JSON input is empty. Please paste JSON data.", "OK");
                return;
            }
            try
            {
                Undo.RecordObject(s_TargetComponent, "Import UI Component JSON");
                s_TargetComponent.ImportFromJson(m_JsonInput);
                s_TargetComponent.RebuildChartObject();
                s_TargetComponent.RefreshAllComponent();
                s_TargetComponent.RefreshGraph();
                EditorUtility.SetDirty(s_TargetComponent);
                SceneView.RepaintAll();
                var target = s_TargetComponent;
                EditorApplication.delayCall += delegate ()
                {
                    if (target == null) return;
                    target.RefreshAllComponent();
                    target.RefreshGraph();
                    SceneView.RepaintAll();
                };
                EditorUtility.DisplayDialog("Success", "UI component imported successfully!", "OK");
                Close();
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("Import Error", "Failed to import JSON:\n" + ex.Message + "\n\n" + ex.StackTrace, "OK");
            }
        }

        private void OpenJsonFile()
        {
            var path = EditorUtility.OpenFilePanel("Open UI Component JSON", "", "json");
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                m_JsonInput = System.IO.File.ReadAllText(path);
                m_ShowPreview = false;
                m_PreviewText = "";
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("Open File Error", "Failed to read JSON file:\n" + ex.Message, "OK");
            }
        }
    }
}
