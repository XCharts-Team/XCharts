using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ChartEditorHelper
{
    public static GUIStyle headerStyle = EditorStyles.boldLabel;
    public static GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
    {
        font = headerStyle.font,
        fontStyle = headerStyle.fontStyle,
    };

    public static void SecondField(Rect drawRect, SerializedProperty prop)
    {
        RectOffset offset = new RectOffset(-(int)EditorGUIUtility.labelWidth, 0, 0, 0);
        drawRect = offset.Add(drawRect);
        EditorGUI.PropertyField(drawRect, prop, GUIContent.none);
        drawRect = offset.Remove(drawRect);
    }

    public static void MakeJsonData(ref Rect drawRect, ref bool showTextArea, ref string inputString,
        SerializedProperty prop, float currentWidth, float diff = 0)
    {
        SerializedProperty stringDataProp = prop.FindPropertyRelative("m_JsonData");
        SerializedProperty needParseProp = prop.FindPropertyRelative("m_DataFromJson");
        float defalutX = drawRect.x;
        drawRect.x = EditorGUIUtility.labelWidth + 14 + diff;
        drawRect.width = currentWidth - EditorGUIUtility.labelWidth - diff;
        if (GUI.Button(drawRect, new GUIContent("Parse JsonData", "Parse data from input json")))
        {
            showTextArea = !showTextArea;
            bool needParse = !showTextArea;
            if (needParse)
            {
                stringDataProp.stringValue = inputString;
                needParseProp.boolValue = true;
            }
        }
        drawRect.x = defalutX;
        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        if (showTextArea)
        {
            drawRect.width = currentWidth;
            drawRect.height = EditorGUIUtility.singleLineHeight * 4;
            inputString = EditorGUI.TextArea(drawRect, inputString);
            drawRect.y += EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing;
            drawRect.height = EditorGUIUtility.singleLineHeight;
        }
    }

    public static bool MakeFoldout(ref Rect drawRect, ref bool moduleToggle, string content,
        SerializedProperty prop = null, bool bold = true)
    {
        float defaultWidth = drawRect.width;
        float defaultX = drawRect.x;
        drawRect.width = EditorGUIUtility.labelWidth;
        moduleToggle = EditorGUI.Foldout(drawRect, moduleToggle, content, bold ? foldoutStyle : EditorStyles.foldout);
        drawRect.x = EditorGUIUtility.labelWidth - (EditorGUI.indentLevel - 1) * 15 - 2;
        drawRect.width = 40;
        if (prop != null)
        {
            EditorGUI.PropertyField(drawRect, prop, GUIContent.none);
        }
        drawRect.width = defaultWidth;
        drawRect.x = defaultX;
        return moduleToggle;
    }

    public static bool MakeFoldout(ref Rect drawRect, ref Dictionary<string, bool> moduleToggle, SerializedProperty prop,
        string moduleName, SerializedProperty showProp = null, bool bold = true)
    {
        var key = prop.propertyPath;
        if (!moduleToggle.ContainsKey(key))
        {
            moduleToggle.Add(key, false);
        }
        var toggle = moduleToggle[key];

        float defaultWidth = drawRect.width;
        float defaultX = drawRect.x;
        drawRect.width = EditorGUIUtility.labelWidth;
        var displayName = string.IsNullOrEmpty(moduleName) ? prop.displayName : moduleName;
        toggle = EditorGUI.Foldout(drawRect, toggle, displayName, bold ? foldoutStyle : EditorStyles.foldout);
        if (moduleToggle[key] != toggle)
        {
            moduleToggle[key] = toggle;
        }
        drawRect.x = EditorGUIUtility.labelWidth - (EditorGUI.indentLevel - 1) * 15 - 2;
        if (showProp != null)
        {
            if (showProp.propertyType == SerializedPropertyType.Boolean)
            {
                drawRect.width = 40;
            }
            else
            {
                drawRect.width = defaultWidth - drawRect.x + 15;
            }
            EditorGUI.PropertyField(drawRect, showProp, GUIContent.none);
        }
        drawRect.width = defaultWidth;
        drawRect.x = defaultX;
        return toggle;
    }

    public static void MakeList(ref Rect drawRect, ref int listSize, SerializedProperty listProp, SerializedProperty large = null)
    {
        EditorGUI.indentLevel++;
        listSize = listProp.arraySize;
        listSize = EditorGUI.IntField(drawRect, "Size", listSize);
        if (listSize < 0) listSize = 0;
        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (listSize != listProp.arraySize)
        {
            while (listSize > listProp.arraySize)
                listProp.InsertArrayElementAtIndex(listProp.arraySize);
            while (listSize < listProp.arraySize)
                listProp.DeleteArrayElementAtIndex(listProp.arraySize - 1);
        }
        if (listSize > 30)
        {
            SerializedProperty element;
            int num = listSize > 10 ? 10 : listSize;
            for (int i = 0; i < num; i++)
            {
                element = listProp.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + i));
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (num >= 10)
            {
                EditorGUI.LabelField(drawRect, "...");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                element = listProp.GetArrayElementAtIndex(listSize - 1);
                EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + (listSize - 1)));
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        else
        {
            for (int i = 0; i < listProp.arraySize; i++)
            {
                SerializedProperty element = listProp.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + i));
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        EditorGUI.indentLevel--;
    }

    public static int InitModuleToggle(ref List<bool> moduleToggle, SerializedProperty prop)
    {
        int index = 0;
        var temp = prop.displayName.Split(' ');
        if (temp == null || temp.Length < 2)
        {
            index = 0;
        }
        else
        {
            int.TryParse(temp[1], out index);
        }
        if (index >= moduleToggle.Count)
        {
            moduleToggle.Add(false);
        }
        return index;
    }

    public static int GetIndexFromPath(SerializedProperty prop)
    {
        int index = 0;
        var sindex = prop.propertyPath.LastIndexOf('[');
        var eindex = prop.propertyPath.LastIndexOf(']');
        if (sindex >= 0 && eindex >= 0)
        {
            var str = prop.propertyPath.Substring(sindex + 1, eindex - sindex - 1);
            int.TryParse(str, out index);
        }
        return index;
    }

    public static bool IsToggle(Dictionary<string, bool> toggle, SerializedProperty prop)
    {
        return toggle.ContainsKey(prop.propertyPath) && toggle[prop.propertyPath] == true;
    }
}