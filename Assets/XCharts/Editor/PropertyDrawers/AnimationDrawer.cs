/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(SerieAnimation), true)]
    public class AnimationDrawer : BasePropertyDrawer
    {
        public override string ClassName { get { return "Animation"; } }
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(pos, prop, label);
            if (MakeFoldout(prop, "m_Enable"))
            {
                ++EditorGUI.indentLevel;
                PropertyField(prop, "m_FadeInDuration");
                PropertyField(prop, "m_FadeInDelay");
                PropertyField(prop, "m_FadeOutDuration");
                PropertyField(prop, "m_FadeOutDelay");
                PropertyField(prop, "m_DataChangeEnable");
                PropertyField(prop, "m_DataChangeDuration");
                PropertyField(prop, "m_ActualDuration");
                PropertyField(prop, "m_AlongWithLinePath");
                --EditorGUI.indentLevel;
            }
        }
    }
}