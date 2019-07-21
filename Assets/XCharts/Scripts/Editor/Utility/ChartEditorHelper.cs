using UnityEngine;
using UnityEditor;

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
        SerializedProperty prop)
    {
        SerializedProperty stringDataProp = prop.FindPropertyRelative("m_JsonData");
        SerializedProperty needParseProp = prop.FindPropertyRelative("m_DataFromJson");
        float defalutX = drawRect.x;
        drawRect.x = EditorGUIUtility.labelWidth + 15;
        drawRect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 35;
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
            drawRect.width = EditorGUIUtility.currentViewWidth - drawRect.x - 20;
            drawRect.height = EditorGUIUtility.singleLineHeight * 3;
            inputString = EditorGUI.TextArea(drawRect, inputString);
            drawRect.y += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
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
        drawRect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 70;
        if (prop != null)
        {
            EditorGUI.PropertyField(drawRect, prop, GUIContent.none);
        }
        drawRect.width = defaultWidth;
        drawRect.x = defaultX;
        return moduleToggle;
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
                EditorGUI.PropertyField(drawRect, element);
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (num >= 10)
            {
                EditorGUI.LabelField(drawRect, "...");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                element = listProp.GetArrayElementAtIndex(listSize - 1);
                EditorGUI.PropertyField(drawRect, element);
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        else
        {
            for (int i = 0; i < listProp.arraySize; i++)
            {
                SerializedProperty element = listProp.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(drawRect, element);
                drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        EditorGUI.indentLevel--;
    }
}