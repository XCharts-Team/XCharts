using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(Emphasis), true)]
    public class EmphasisDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Emphasis"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Label");
                PropertyField(prop, "m_LabelLine");
                PropertyField(prop, "m_ItemStyle");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(EmphasisItemStyle), true)]
    public class EmphasisItemStyleDrawer : ItemStyleDrawer
    {
        public override string ClassName { get { return "EmphasisItemStyle"; } }
    }

    [CustomPropertyDrawer(typeof(EmphasisLabelStyle), true)]
    public class EmphasisLabelStyleDrawer : LabelStyleDrawer
    {
        public override string ClassName { get { return "EmphasisLabel"; } }
    }

    [CustomPropertyDrawer(typeof(EmphasisLabelLine), true)]
    public class EmphasisLabelLineDrawer : LabelLineDrawer
    {
        public override string ClassName { get { return "EmphasisLabelLine"; } }
    }
}