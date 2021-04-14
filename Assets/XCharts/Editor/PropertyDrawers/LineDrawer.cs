/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(BaseLine), true)]
    public class BaseLineDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Line"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Show"))
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
        }
    }

    [CustomPropertyDrawer(typeof(GaugeAxisSplitLine), true)]
    public class GaugeAxisSplitDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "Split Line"; } }
    }

    [CustomPropertyDrawer(typeof(GaugeAxisTick), true)]
    public class GaugeAxisTickDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "Axis Tick"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_SplitNumber");
        }
    }

    [CustomPropertyDrawer(typeof(GaugeAxisLine), true)]
    public class GaugeAxisLineDrawer : BaseLineDrawer
    {
        public override string ClassName { get { return "Axis Line"; } }
        protected override void DrawExtendeds(SerializedProperty prop)
        {
            base.DrawExtendeds(prop);
            PropertyField(prop, "m_BarColor");
            PropertyField(prop, "m_BarBackgroundColor");
            PropertyField(prop, "m_StageColor");
        }
    }
}