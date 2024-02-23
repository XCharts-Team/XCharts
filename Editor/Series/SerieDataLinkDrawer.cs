using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(SerieDataLink), true)]
    public class SerieDataLinkDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Link"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Source");
                PropertyField(prop, "m_Target");
                PropertyField(prop, "m_Value");
                --EditorGUI.indentLevel;
            }
        }
    }
}