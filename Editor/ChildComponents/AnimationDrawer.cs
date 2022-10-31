using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(AnimationStyle), true)]
    public class AnimationDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Animation"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Enable", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Type");
                PropertyField(prop, "m_FadeInDuration");
                PropertyField(prop, "m_FadeInDelay");
                PropertyField(prop, "m_FadeOutDuration");
                PropertyField(prop, "m_FadeOutDelay");
                PropertyField(prop, "m_DataChangeEnable");
                PropertyField(prop, "m_DataChangeDuration");
                PropertyField(prop, "m_UnscaledTime");
                // using(new EditorGUI.DisabledGroupScope(true))
                // {
                //     PropertyField(prop, "m_ActualDuration");
                // }
                --EditorGUI.indentLevel;
            }
        }
    }
}