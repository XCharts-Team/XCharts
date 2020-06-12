using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ChartEditorHelper
{
#if UNITY_2019_3_OR_NEWER
    public const float INDENT_WIDTH = 15;
    public const float BOOL_WIDTH = 15;
    public const float ARROW_WIDTH = 20;
    public const float GAP_WIDTH =2;
#else
    public const float INDENT_WIDTH = 15;
    public const float BOOL_WIDTH = 15;
    public const float ARROW_WIDTH = 13;
    public const float GAP_WIDTH = 0;
#endif
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

    public static void MakeTwoField(ref Rect drawRect, float rectWidth, SerializedProperty arrayProp, string name)
    {
        while (arrayProp.arraySize < 2) arrayProp.arraySize++;
        MakeTwoField(ref drawRect, rectWidth, arrayProp.GetArrayElementAtIndex(0), arrayProp.GetArrayElementAtIndex(1), name);
    }

    public static void MakeDivideList(ref Rect drawRect, float rectWidth, SerializedProperty arrayProp, string name, int showNum)
    {
        while (arrayProp.arraySize < showNum) arrayProp.arraySize++;
        EditorGUI.LabelField(drawRect, name);
#if UNITY_2019_3_OR_NEWER
        var gap = 2;
#else
        var gap = 0;
#endif
        var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + gap;
        var dataWidTotal = (rectWidth - (startX + INDENT_WIDTH + 1));
        EditorGUI.DrawRect(new Rect(startX, drawRect.y, dataWidTotal, drawRect.height), Color.grey);
        var dataWid = dataWidTotal / showNum;
        var xWid = dataWid - gap;
        for (int i = 0; i < 1; i++)
        {
            drawRect.x = startX + i * xWid;
            drawRect.width = dataWid + (EditorGUI.indentLevel - 2) * 40.5f;
            EditorGUI.PropertyField(drawRect, arrayProp.GetArrayElementAtIndex(i), GUIContent.none);
        }
        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    public static void MakeTwoField(ref Rect drawRect, float rectWidth, SerializedProperty prop1, SerializedProperty prop2, string name)
    {
        EditorGUI.LabelField(drawRect, name);
        var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + GAP_WIDTH;
        var diff = 14 + EditorGUI.indentLevel * 14;
        var offset = diff - INDENT_WIDTH;
        var tempWidth = (rectWidth - startX + diff) / 2;
        var centerXRect = new Rect(startX, drawRect.y, tempWidth, drawRect.height);
        var centerYRect = new Rect(centerXRect.x + tempWidth - offset, drawRect.y, tempWidth, drawRect.height);
        EditorGUI.PropertyField(centerXRect, prop1, GUIContent.none);
        EditorGUI.PropertyField(centerYRect, prop2, GUIContent.none);
        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    public static void MakeVector2(ref Rect drawRect, float rectWidth, SerializedProperty prop, string name)
    {
        EditorGUI.LabelField(drawRect, name);
        var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + GAP_WIDTH;
        var diff = 14 + EditorGUI.indentLevel * 14;
        var offset = diff - INDENT_WIDTH;
        var tempWidth = (rectWidth - startX + diff) / 2;
        var centerXRect = new Rect(startX, drawRect.y, tempWidth, drawRect.height);
        var centerYRect = new Rect(centerXRect.x + tempWidth - offset, drawRect.y, tempWidth, drawRect.height);
        var x = EditorGUI.FloatField(centerXRect, prop.vector3Value.x);
        var y = EditorGUI.FloatField(centerYRect, prop.vector3Value.y);
        prop.vector3Value = new Vector3(x, y);
        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    public static void MakeJsonData(ref Rect drawRect, ref bool showTextArea, ref string inputString,
        SerializedProperty prop, float currentWidth, float diff = 0)
    {
        SerializedProperty stringDataProp = prop.FindPropertyRelative("m_JsonData");
        SerializedProperty needParseProp = prop.FindPropertyRelative("m_DataFromJson");
        float defalutX = drawRect.x;
        drawRect.x = EditorGUIUtility.labelWidth + ARROW_WIDTH + diff;
        drawRect.width = currentWidth - EditorGUIUtility.labelWidth - GAP_WIDTH - diff;
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
        drawRect.width = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH;
        moduleToggle = EditorGUI.Foldout(drawRect, moduleToggle, content, bold ? foldoutStyle : EditorStyles.foldout);
        MakeBool(drawRect, prop);
        drawRect.width = defaultWidth;
        drawRect.x = defaultX;
        return moduleToggle;
    }

    public static void MakeBool(Rect drawRect, SerializedProperty boolProp, int index = 0, string name = null)
    {
        float defaultWidth = drawRect.width;
        float defaultX = drawRect.x;
        float boolWidth = index * (BOOL_WIDTH + GAP_WIDTH);
        drawRect.x = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + ARROW_WIDTH + boolWidth;
        drawRect.width = (EditorGUI.indentLevel + 1) * BOOL_WIDTH + index * 110;
        if (boolProp != null)
        {
            EditorGUI.PropertyField(drawRect, boolProp, GUIContent.none);
            if (!string.IsNullOrEmpty(name))
            {
                drawRect.x += BOOL_WIDTH;
                drawRect.width = 200;
                EditorGUI.LabelField(drawRect, name);
            }
        }
        drawRect.width = defaultWidth;
        drawRect.x = defaultX;
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
#if UNITY_2019_3_OR_NEWER
        drawRect.width = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH;
#else
        drawRect.width = EditorGUIUtility.labelWidth;
#endif
        var displayName = string.IsNullOrEmpty(moduleName) ? prop.displayName : moduleName;
        toggle = EditorGUI.Foldout(drawRect, toggle, displayName, bold ? foldoutStyle : EditorStyles.foldout);

        if (moduleToggle[key] != toggle)
        {
            moduleToggle[key] = toggle;
        }
        if (showProp != null)
        {
            drawRect.x = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + ARROW_WIDTH;
            if (showProp.propertyType == SerializedPropertyType.Boolean)
            {
                drawRect.width = (EditorGUI.indentLevel + 1) * BOOL_WIDTH;
            }
            else
            {
                drawRect.width = defaultWidth - drawRect.x + ARROW_WIDTH - GAP_WIDTH;
            }
            EditorGUI.PropertyField(drawRect, showProp, GUIContent.none);
        }
        drawRect.width = defaultWidth;
        drawRect.x = defaultX;
        return toggle;
    }

    public static void MakeList(ref Rect drawRect, ref int listSize, SerializedProperty listProp, bool showOrder = false, bool showSize = true)
    {
        EditorGUI.indentLevel++;
        listSize = listProp.arraySize;
        if (showSize)
        {
            if (showOrder)
            {
                var nameWid = 15;
                var temp = INDENT_WIDTH + GAP_WIDTH;
                var elementRect = new Rect(drawRect.x, drawRect.y, drawRect.width - nameWid - 1, drawRect.height);
                var iconRect = new Rect(drawRect.width - nameWid + temp, drawRect.y, nameWid, drawRect.height);
                if (GUI.Button(iconRect, new GUIContent("+", "add")))
                {
                    listProp.arraySize++;
                }
                listSize = listProp.arraySize;
                listSize = EditorGUI.IntField(elementRect, "Size", listSize);
            }
            else
            {
                listSize = EditorGUI.IntField(drawRect, "Size", listSize);
            }
            if (listSize < 0) listSize = 0;
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (listSize != listProp.arraySize)
            {
                while (listSize > listProp.arraySize) listProp.arraySize++;
                while (listSize < listProp.arraySize) listProp.arraySize--;
            }
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
                if (showOrder)
                {
                    var nameWid = 15;
                    var temp = INDENT_WIDTH + GAP_WIDTH;
                    var isSerie = "Serie".Equals(element.type);
                    var elementRect = isSerie ? drawRect : new Rect(drawRect.x, drawRect.y, drawRect.width - 2 * nameWid, drawRect.height);
                    EditorGUI.PropertyField(elementRect, element, new GUIContent("Element " + i));
                    var iconRect = new Rect(drawRect.width - 3 * nameWid + temp, drawRect.y, nameWid, drawRect.height);
                    if (GUI.Button(iconRect, new GUIContent("↑", "up")))
                    {
                        if (i > 0) listProp.MoveArrayElement(i, i - 1);
                    }
                    iconRect = new Rect(drawRect.width - 2 * nameWid + temp, drawRect.y, nameWid, drawRect.height);
                    if (GUI.Button(iconRect, new GUIContent("↓", "down")))
                    {
                        if (i < listProp.arraySize - 1) listProp.MoveArrayElement(i, i + 1);
                    }
                    iconRect = new Rect(drawRect.width - nameWid + temp, drawRect.y, nameWid, drawRect.height);
                    if (GUI.Button(iconRect, new GUIContent("-", "delete")))
                    {
                        if (i < listProp.arraySize && i >= 0) listProp.DeleteArrayElementAtIndex(i);
                    }
                    else
                    {
                        drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + i));
                    drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
                }
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