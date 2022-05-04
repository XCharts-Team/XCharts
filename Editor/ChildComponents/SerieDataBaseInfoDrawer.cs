
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(SerieDataBaseInfo), true)]
    public class SerieDataBaseInfoDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "BaseInfo"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", false))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Ignore");
                PropertyField(prop, "m_Selected");
                PropertyField(prop, "m_Radius");
                --EditorGUI.indentLevel;
            }
        }
    }
}