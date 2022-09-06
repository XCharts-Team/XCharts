using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(BaseLine), true)]
    public class BaseLineDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Line"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeComponentFoldout(prop, "m_Show", true))
            {
                ++EditorGUI.indentLevel;
                DrawExtendeds(prop);
                PropertyField(prop, "m_LineStyle");
                --EditorGUI.indentLevel;
            }
        }
    }

    [CustomPropertyDrawer(typeof(AxisLine), true)]
    public class AxisLineDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "AxisLine"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_OnZero");
            PropertyField(prop, "m_ShowArrow");
            PropertyField(prop, "m_Arrow");
        }
    }

    [CustomPropertyDrawer(typeof(AxisSplitLine), true)]
    public class AxisSplitLineDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "SplitLine"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_Interval");
            PropertyField(prop, "m_Distance");
            PropertyField(prop, "m_AutoColor");
            PropertyField(prop, "m_ShowStartLine");
            PropertyField(prop, "m_ShowEndLine");
        }
    }

    [CustomPropertyDrawer(typeof(AxisMinorSplitLine), true)]
    public class AxisMinorSplitLineDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "MinorSplitLine"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            //PropertyField(prop, "m_Distance");
            //PropertyField(prop, "m_AutoColor");
        }
    }

    [CustomPropertyDrawer(typeof(AxisTick), true)]
    public class AxisTickDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "AxisTick"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_AlignWithLabel");
            PropertyField(prop, "m_Inside");
            PropertyField(prop, "m_ShowStartTick");
            PropertyField(prop, "m_ShowEndTick");
            PropertyField(prop, "m_SplitNumber");
            PropertyField(prop, "m_Distance");
            PropertyField(prop, "m_AutoColor");
        }
    }

    [CustomPropertyDrawer(typeof(AxisMinorTick), true)]
    public class AxisMinorTickDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "MinorTick"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_SplitNumber");
            //PropertyField(prop, "m_AutoColor");
        }
    }
}