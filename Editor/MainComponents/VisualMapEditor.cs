
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [ComponentEditor(typeof(VisualMap))]
    public class VisualMapEditor : MainComponentEditor<VisualMap>
    {
        public override void OnInspectorGUI()
        {
            ++EditorGUI.indentLevel;
            var type = (VisualMap.Type)baseProperty.FindPropertyRelative("m_Type").enumValueIndex;
            var isPiece = type == VisualMap.Type.Piecewise;
            PropertyField("m_Type");
            PropertyField("m_SerieIndex");
            PropertyField("m_AutoMinMax");
            PropertyField("m_Min");
            PropertyField("m_Max");
            PropertyField("m_SplitNumber");
            PropertyField("m_Dimension");
            PropertyField("m_WorkOnLine");
            PropertyField("m_WorkOnArea");
            PropertyField("m_ShowUI");
            if (baseProperty.FindPropertyRelative("m_ShowUI").boolValue)
            {
                PropertyField("m_SelectedMode");
                PropertyTwoFiled("m_Range");
                PropertyTwoFiled("m_Text");
                PropertyTwoFiled("m_TextGap");
                PropertyField("m_HoverLink");
                PropertyField("m_Calculable");
                PropertyField("m_ItemWidth");
                PropertyField("m_ItemHeight");
                if (isPiece) PropertyField("m_ItemGap");
                PropertyField("m_BorderWidth");
                PropertyField("m_Orient");
                PropertyField("m_Location");
            }
            PropertyListField("m_OutOfRange");
            PropertyListField(isPiece ? "m_Pieces" : "m_InRange");
            --EditorGUI.indentLevel;
        }
    }

    [CustomPropertyDrawer(typeof(VisualMapPieces), true)]
    public class PiecesDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Pieces"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, ""))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Min");
                PropertyField(prop, "m_Max");
                PropertyField(prop, "m_Label");
                PropertyField(prop, "m_Color");
                --EditorGUI.indentLevel;
            }
        }
    }
}