using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(MarqueeStyle), true)]
    public class MarqueeStyleDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "MarqueeStyle"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Apply");
                PropertyField(prop, "m_RealRect");
                PropertyField(prop, "m_LineStyle");
                PropertyField(prop, "m_AreaStyle");
                --EditorGUI.indentLevel;
            }
        }
    }
}