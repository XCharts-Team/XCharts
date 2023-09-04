using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(XCharts.Runtime.AnimationInfo), true)]
    public class AnimationInfoDrawer : BasePropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Enable", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Delay");
                PropertyField(prop, "m_Duration");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(XCharts.Runtime.AnimationChange), true)]
    public class AnimationChangeDrawer : BasePropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Enable", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Duration");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(XCharts.Runtime.AnimationAddition), true)]
    public class AnimationAdditionDrawer : BasePropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Enable", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Duration");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(XCharts.Runtime.AnimationInteraction), true)]
    public class AnimationInteractionDrawer : BasePropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Enable", true))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_Duration");
                PropertyField(prop, "m_Width");
                PropertyField(prop, "m_Radius");
                PropertyField(prop, "m_Offset");
                --EditorGUI.indentLevel;
            }
        }
    }

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
                PropertyField(prop, "m_UnscaledTime");
                PropertyField(prop, "m_FadeIn");
                PropertyField(prop, "m_FadeOut");
                PropertyField(prop, "m_Change");
                PropertyField(prop, "m_Addition");
                PropertyField(prop, "m_Interaction");
                --EditorGUI.indentLevel;
            }
        }
    }
}