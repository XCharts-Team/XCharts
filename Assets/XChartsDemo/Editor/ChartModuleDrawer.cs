/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XChartsDemo
{
    [CustomPropertyDrawer(typeof(ChartModule), true)]
    internal class ChartModuleDrawer : PropertyDrawer
    {
        public class Styles
        {
            public static readonly GUIContent iconAdd = new GUIContent("+", "Add");
            public static readonly GUIContent iconRemove = new GUIContent("-", "Remove");
            public static readonly GUIContent iconUp = new GUIContent("↑", "Up");
            public static readonly GUIContent iconDown = new GUIContent("↓", "Down");
            public static readonly GUIStyle invisibleButton = "InvisibleButton";
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            var lastX = drawRect.x;
            var lastWid = drawRect.width;
            SerializedProperty m_Column = prop.FindPropertyRelative("m_Column");
            SerializedProperty m_Name = prop.FindPropertyRelative("m_Name");
            SerializedProperty m_SubName = prop.FindPropertyRelative("m_SubName");
            SerializedProperty m_Title = prop.FindPropertyRelative("m_Title");
            SerializedProperty m_Selected = prop.FindPropertyRelative("m_Selected");
            SerializedProperty m_Panel = prop.FindPropertyRelative("m_Panel");
            var fieldWid = EditorGUIUtility.currentViewWidth - 30 - 5 - 40 - 80 - 50 - 100 - 6 * 2 - 70;
            drawRect.width = 15;
            EditorGUI.BeginChangeCheck();
            var oldFlag = m_Selected.boolValue;
            EditorGUI.PropertyField(drawRect, m_Selected, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                var demo = prop.serializedObject.targetObject as Demo;
                var index = GetIndex(prop);
                var selectedIndex = demo.GetSelectedModule();
                if (selectedIndex != index)
                {
                    for (int i = 0; i < demo.chartModules.Count; i++)
                    {
                        demo.chartModules[i].select = i == index && m_Selected.boolValue;
                    }
                    demo.InitModuleButton();
                }
                else
                {
                    m_Selected.boolValue = oldFlag;
                }
            }
            drawRect.x += 17;
            drawRect.width = 80;
            EditorGUI.PropertyField(drawRect, m_Name, GUIContent.none);
            drawRect.x += 82;
            drawRect.width = 50;
            EditorGUI.PropertyField(drawRect, m_SubName, GUIContent.none);
            drawRect.x += 52;
            drawRect.width = fieldWid;
            EditorGUI.PropertyField(drawRect, m_Title, GUIContent.none);
            drawRect.x += fieldWid + 2;
            drawRect.width = 100;
            EditorGUI.PropertyField(drawRect, m_Panel, GUIContent.none);
            drawRect.x += 102;
            drawRect.width = 40;
            EditorGUI.PropertyField(drawRect, m_Column, GUIContent.none);

            var btnWidth = 12;
            drawRect.x += 42;
            drawRect.width = btnWidth;
            if (GUI.Button(drawRect, Styles.iconUp, Styles.invisibleButton))
            {
                var demo = prop.serializedObject.targetObject as Demo;
                var index = GetIndex(prop);
                if (index >= 0)
                {
                    Swap(demo.chartModules, index, index - 1);
                    demo.InitModuleButton();
                }
            }
            drawRect.x += btnWidth + 1;
            if (GUI.Button(drawRect, Styles.iconDown, Styles.invisibleButton))
            {
                var demo = prop.serializedObject.targetObject as Demo;
                var index = GetIndex(prop);
                if (index >= 0)
                {
                    Swap(demo.chartModules, index, index + 1);
                    demo.InitModuleButton();
                }
            }
            drawRect.x += btnWidth + 1;
            if (GUI.Button(drawRect, Styles.iconAdd, Styles.invisibleButton))
            {
                var demo = prop.serializedObject.targetObject as Demo;
                var index = GetIndex(prop);
                if (index >= 0)
                {
                    demo.chartModules.Insert(index + 1, new ChartModule());
                    demo.InitModuleButton();
                }
            }
            drawRect.x += 16;
            if (GUI.Button(drawRect, Styles.iconRemove, Styles.invisibleButton))
            {
                var demo = prop.serializedObject.targetObject as Demo;
                var index = GetIndex(prop);
                if (index >= 0)
                {
                    demo.chartModules.RemoveAt(index);
                    demo.InitModuleButton();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }

        private int GetIndex(SerializedProperty prop)
        {
            int index = -1;
            var sindex = prop.propertyPath.LastIndexOf('[');
            var eindex = prop.propertyPath.LastIndexOf(']');
            if (sindex >= 0 && eindex >= 0)
            {
                var str = prop.propertyPath.Substring(sindex + 1, eindex - sindex - 1);
                int.TryParse(str, out index);
            }
            return index;
        }

        private static bool Swap<T>(List<T> list, int index1, int index2)
        {
            if (index1 < 0 || index1 >= list.Count) return false;
            if (index2 < 0 || index2 >= list.Count) return false;
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return true;
        }


    }
}