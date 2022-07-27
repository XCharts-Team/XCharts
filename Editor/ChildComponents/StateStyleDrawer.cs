using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(StateStyle), true)]
    public class StateStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "StateStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                OnCustomGUI(prop);
                PropertyField(prop, "m_Symbol");
                PropertyField(prop, "m_ItemStyle");
                PropertyField(prop, "m_Label");
                PropertyField(prop, "m_LabelLine");
                PropertyField(prop, "m_LineStyle");
                PropertyField(prop, "m_AreaStyle");
                --EditorGUI.indentLevel;
            }
        }

        protected virtual void OnCustomGUI(SerializedProperty prop) { }
    }

    [CustomPropertyDrawer(typeof(EmphasisStyle), true)]
    public class EmphasisStyleDrawer : StateStyleDrawer
    {
        public override string ClassName { get { return "EmphasisStyle"; } }
        protected override void OnCustomGUI(SerializedProperty prop)
        {
            PropertyField(prop, "m_Scale");
            PropertyField(prop, "m_Focus");
            PropertyField(prop, "m_BlurScope");
        }
    }

    [CustomPropertyDrawer(typeof(BlurStyle), true)]
    public class BlurStyleDrawer : StateStyleDrawer
    {
        public override string ClassName { get { return "BlurStyle"; } }
    }

    [CustomPropertyDrawer(typeof(SelectStyle), true)]
    public class SelectStyleDrawer : StateStyleDrawer
    {
        public override string ClassName { get { return "SelectStyle"; } }
    }
}