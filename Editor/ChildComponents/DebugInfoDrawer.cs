using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(DebugInfo), true)]
    public class DebugInfoDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Debug"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", false))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_FoldSeries");
                PropertyField(prop, "m_ShowDebugInfo");
                PropertyField(prop, "m_ShowAllChartObject");
                PropertyField(prop, "m_LabelStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}