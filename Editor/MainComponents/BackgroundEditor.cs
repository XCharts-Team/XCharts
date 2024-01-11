using UnityEditor;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(Background))]
    internal sealed class BackgroundEditor : MainComponentEditor<Background>
    {
        public override void OnInspectorGUI()
        {

            ++EditorGUI.indentLevel;
            PropertyField("m_Image");
            PropertyField("m_ImageType");
            PropertyField("m_ImageColor");
            PropertyField("m_AutoColor");
            PropertyField("m_BorderStyle");
            --EditorGUI.indentLevel;
        }
    }
}