using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(MarkArea))]
    public class MarkAreaEditor : MainComponentEditor<MarkArea>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            PropertyField("m_SerieIndex");
            PropertyField("m_Text");
            PropertyField("m_ItemStyle");
            PropertyField("m_Label");
            PropertyField("m_Start");
            PropertyField("m_End");
            --EditorGUI.indentLevel;
        }
    }

    [CustomPropertyDrawer(typeof(MarkAreaData), true)]
    public class MarkAreaDataDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "MarkAreaData"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "", true))
            {
                ++EditorGUI.indentLevel;
                var type = (MarkAreaType) (prop.FindPropertyRelative("m_Type")).enumValueIndex;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_Name");
                switch (type)
                {
                    case MarkAreaType.None:
                        PropertyField(prop, "m_XPosition");
                        PropertyField(prop, "m_YPosition");
                        PropertyField(prop, "m_XValue");
                        PropertyField(prop, "m_YValue");
                        break;
                    case MarkAreaType.Min:
                    case MarkAreaType.Max:
                    case MarkAreaType.Average:
                    case MarkAreaType.Median:
                        PropertyField(prop, "m_Dimension");
                        break;
                }
                --EditorGUI.indentLevel;
            }
        }
    }
}