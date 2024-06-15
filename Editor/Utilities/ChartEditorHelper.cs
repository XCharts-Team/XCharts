using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public class HeaderCallbackContext
    {
        public int fieldCount = 0;
        public SerializedProperty serieData;
        public bool showName;
        public int index;
        public int dimension;
        public SerializedProperty listProp;
    }

    public class HeaderMenuInfo
    {
        public string name;
        public Action action;
        public bool enable = true;

        public HeaderMenuInfo() { }
        public HeaderMenuInfo(string name, Action action)
        {
            this.name = name;
            this.action = action;
        }
        public HeaderMenuInfo(string name, Action action, bool enable)
        {
            this.name = name;
            this.action = action;
            this.enable = enable;
        }
    }

    public static class ChartEditorHelper
    {
        public const float HEADER_HEIGHT = 17f;
        public const float FOLDOUT_WIDTH = 13f;
#if UNITY_2019_3_OR_NEWER
        public const float INDENT_WIDTH = 15;
        public const float BOOL_WIDTH = 15;
        public const float ARROW_WIDTH = 20;
        public const float GAP_WIDTH = 2;
        public const float DIFF_WIDTH = 0;
#else
        public const float INDENT_WIDTH = 15;
        public const float BOOL_WIDTH = 15;
        public const float ARROW_WIDTH = 14f;
        public const float GAP_WIDTH = 0;
        public const float DIFF_WIDTH = 1;
#endif
        public const float ICON_WIDHT = 10;
        public const float ICON_GAP = 0;
        static Dictionary<string, GUIContent> s_GUIContentCache;

        static ChartEditorHelper()
        {
            s_GUIContentCache = new Dictionary<string, GUIContent>();
        }

        public static void SecondField(Rect drawRect, SerializedProperty prop)
        {
            RectOffset offset = new RectOffset(-(int)EditorGUIUtility.labelWidth, 0, 0, 0);
            drawRect = offset.Add(drawRect);
            EditorGUI.PropertyField(drawRect, prop, GUIContent.none);
            drawRect = offset.Remove(drawRect);
        }

        public static void MakeTwoField(ref Rect drawRect, float rectWidth, SerializedProperty arrayProp,
            string name)
        {
            while (arrayProp.arraySize < 2) arrayProp.arraySize++;
            var prop1 = arrayProp.GetArrayElementAtIndex(0);
            var prop2 = arrayProp.GetArrayElementAtIndex(1);
            MakeTwoField(ref drawRect, rectWidth, prop1, prop2, name);
        }

        public static void MakeDivideList(ref Rect drawRect, float rectWidth, SerializedProperty arrayProp,
            string name, int showNum)
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

        public static void MakeTwoField(ref Rect drawRect, float rectWidth, SerializedProperty prop1,
            SerializedProperty prop2, string name)
        {
            EditorGUI.LabelField(drawRect, name);
            var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + GAP_WIDTH;
            var diff = 13 + EditorGUI.indentLevel * 14;
            var offset = diff - INDENT_WIDTH;
            var tempWidth = (rectWidth - startX + diff) / 2;
            var centerXRect = new Rect(startX, drawRect.y, tempWidth, drawRect.height - 1);
            var centerYRect = new Rect(centerXRect.x + tempWidth - offset, drawRect.y, tempWidth - 1, drawRect.height - 1);
            EditorGUI.PropertyField(centerXRect, prop1, GUIContent.none);
            EditorGUI.PropertyField(centerYRect, prop2, GUIContent.none);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public static float MakeThreeField(ref Rect drawRect, float rectWidth, SerializedProperty prop1,
            SerializedProperty prop2, SerializedProperty prop3, string name, bool btnSpacing = true)
        {
            EditorGUI.LabelField(drawRect, name);
            var startX = drawRect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + GAP_WIDTH;
            var diff = 13f + EditorGUI.indentLevel * 14;
            var offset = diff - INDENT_WIDTH;
            var tempWidth = (rectWidth - startX + diff - (btnSpacing ? (ICON_WIDHT + ICON_GAP) * 4 : 0)) / 3 + 8.5f;
            var centerXRect = new Rect(startX, drawRect.y, tempWidth, drawRect.height - 1);
            var centerYRect = new Rect(centerXRect.x + tempWidth - offset, drawRect.y, tempWidth - 1, drawRect.height - 1);
            var centerZRect = new Rect(centerYRect.x + tempWidth - offset, drawRect.y, tempWidth - 1, drawRect.height - 1);
            EditorGUI.PropertyField(centerXRect, prop1, GUIContent.none);
            EditorGUI.PropertyField(centerYRect, prop2, GUIContent.none);
            EditorGUI.PropertyField(centerZRect, prop3, GUIContent.none);
            var hig = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            drawRect.y += hig;
            return hig;
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

        public static bool MakeFoldout(ref Rect drawRect, ref bool moduleToggle, string content,
            SerializedProperty prop = null, bool bold = false)
        {
            float defaultWidth = drawRect.width;
            float defaultX = drawRect.x;
            var style = bold ? EditorCustomStyles.foldoutStyle : UnityEditor.EditorStyles.foldout;
            drawRect.width = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH;
            moduleToggle = EditorGUI.Foldout(drawRect, moduleToggle, content, true, style);
            MakeBool(drawRect, prop);
            drawRect.width = defaultWidth;
            drawRect.x = defaultX;
            return moduleToggle;
        }

        public static bool MakeFoldout(ref Rect drawRect, Dictionary<string, float> heights,
            Dictionary<string, bool> moduleToggle, string key, string content, SerializedProperty prop, bool bold = false)
        {
            float defaultWidth = drawRect.width;
            float defaultX = drawRect.x;
            var style = bold ? EditorCustomStyles.foldoutStyle : UnityEditor.EditorStyles.foldout;
            drawRect.width = EditorGUIUtility.labelWidth;
            moduleToggle[key] = EditorGUI.Foldout(drawRect, moduleToggle[key], content, true, style);
            if (prop != null)
            {
                if (prop.propertyType == SerializedPropertyType.Boolean)
                {
                    MakeBool(drawRect, prop);
                }
                else
                {
                    drawRect.x = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + ARROW_WIDTH;
                    drawRect.width = defaultWidth - drawRect.x + ARROW_WIDTH - 2;
                    EditorGUI.PropertyField(drawRect, prop, GUIContent.none);
                }
            }

            drawRect.width = defaultWidth;
            drawRect.x = defaultX;
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            heights[key] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return moduleToggle[key];
        }
        public static bool MakeComponentFoldout(ref Rect drawRect, Dictionary<string, float> heights,
            Dictionary<string, bool> moduleToggle, string key, string content, SerializedProperty prop,
            SerializedProperty prop2, bool propEnable, params HeaderMenuInfo[] menus)
        {
            var sourRect = drawRect;
            float defaultWidth = drawRect.width;
            float defaultX = drawRect.x;
            float headerHeight = DrawSplitterAndBackground(drawRect);

            drawRect.width = EditorGUIUtility.labelWidth;

            moduleToggle[key] = EditorGUI.Foldout(drawRect, moduleToggle[key], content, true, EditorStyles.foldout);
            if (prop != null)
            {
                if (prop.propertyType == SerializedPropertyType.Boolean)
                {
                    if (!propEnable)
                        using (new EditorGUI.DisabledScope(true))
                            MakeBool(drawRect, prop);
                    else
                        MakeBool(drawRect, prop);
                    if (prop2 != null && !moduleToggle[key])
                    {
                        drawRect.x = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + ARROW_WIDTH + BOOL_WIDTH;
                        drawRect.width = defaultWidth - drawRect.x + ARROW_WIDTH;
                        EditorGUI.PropertyField(drawRect, prop2, GUIContent.none);
                    }
                }
                else
                {
                    drawRect.x = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + ARROW_WIDTH;
                    drawRect.width = defaultWidth - drawRect.x + ARROW_WIDTH - 2;
                    EditorGUI.PropertyField(drawRect, prop, GUIContent.none);
                }
            }
            DrawMenu(sourRect, menus);
            drawRect.width = defaultWidth;
            drawRect.x = defaultX;
            drawRect.y += headerHeight;
            heights[key] += headerHeight;
            return moduleToggle[key];
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

        public static bool MakeFoldout(ref Rect drawRect, ref float height, ref Dictionary<string, bool> moduleToggle,
            SerializedProperty prop, string moduleName, string showPropName, bool bold = false)
        {
            var relativeProp = prop.FindPropertyRelative(showPropName);
            var flag = MakeFoldout(ref drawRect, ref moduleToggle, prop, moduleName, relativeProp, bold);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return flag;
        }

        public static bool MakeFoldout(ref Rect drawRect, ref Dictionary<string, bool> moduleToggle, SerializedProperty prop,
            string moduleName, SerializedProperty showProp = null, bool bold = false)
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
            var foldoutStyle = bold ? EditorCustomStyles.foldoutStyle : UnityEditor.EditorStyles.foldout;
            toggle = EditorGUI.Foldout(drawRect, toggle, displayName, true, foldoutStyle);

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

        public static bool MakeListWithFoldout(ref Rect drawRect, SerializedProperty listProp, bool foldout,
            bool showOrder, bool showSize, params HeaderMenuInfo[] menus)
        {
            var height = 0f;
            return MakeListWithFoldout(ref drawRect, ref height, listProp, foldout, showOrder, showSize, menus);
        }

        public static bool MakeListWithFoldout(ref Rect drawRect, ref float height, SerializedProperty listProp,
            bool foldout, bool showOrder, bool showSize, params HeaderMenuInfo[] menus)
        {
            var rawWidth = drawRect.width;
            var headerHeight = DrawSplitterAndBackground(drawRect);
            var foldoutRect = drawRect;
            foldoutRect.xMax -= 10;
            bool flag = EditorGUI.Foldout(foldoutRect, foldout, listProp.displayName, true);
            ChartEditorHelper.DrawMenu(drawRect, menus);
            height += headerHeight;
            drawRect.y += headerHeight;
            drawRect.width = rawWidth;
            if (flag)
            {
                MakeList(ref drawRect, ref height, listProp, showOrder, showSize);
            }
            return flag;
        }

        public static void MakeList(ref Rect drawRect, SerializedProperty listProp, bool showOrder = false,
            bool showSize = true)
        {
            var height = 0f;
            MakeList(ref drawRect, ref height, listProp, showOrder, showSize);
        }

        public static void MakeList(ref Rect drawRect, ref float height, SerializedProperty listProp,
            bool showOrder = false, bool showSize = true)
        {
            EditorGUI.indentLevel++;
            var listSize = listProp.arraySize;
            if (showSize)
            {
                var headerHeight = DrawSplitterAndBackground(drawRect);
                if (showOrder)
                {
                    var elementRect = new Rect(drawRect.x, drawRect.y, drawRect.width - ICON_WIDHT + 2, drawRect.height);
                    var oldColor = GUI.contentColor;
                    GUI.contentColor = Color.black;
                    GUI.contentColor = oldColor;
                    listSize = listProp.arraySize;
                    listSize = EditorGUI.IntField(elementRect, "Size", listSize);
                }
                else
                {
                    listSize = EditorGUI.IntField(drawRect, "Size", listSize);
                }
                if (listSize < 0) listSize = 0;
                drawRect.y += headerHeight;
                height += headerHeight;

                if (listSize != listProp.arraySize)
                {
                    while (listSize > listProp.arraySize) listProp.arraySize++;
                    while (listSize < listProp.arraySize) listProp.arraySize--;
                }
            }
            if (listSize > 30 && !XCSettings.editorShowAllListData)
            {
                SerializedProperty element;
                int num = listSize > 10 ? 10 : listSize;
                for (int i = 0; i < num; i++)
                {
                    element = listProp.GetArrayElementAtIndex(i);
                    DrawSplitterAndBackground(drawRect);
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + i));
                    drawRect.y += EditorGUI.GetPropertyHeight(element);
                    height += EditorGUI.GetPropertyHeight(element);
                }
                if (num >= 10)
                {
                    EditorGUI.LabelField(drawRect, "...");
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    element = listProp.GetArrayElementAtIndex(listSize - 1);
                    DrawSplitterAndBackground(drawRect);
                    EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + (listSize - 1)));
                    drawRect.y += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else
            {
                for (int i = 0; i < listProp.arraySize; i++)
                {
                    SerializedProperty element = listProp.GetArrayElementAtIndex(i);
                    DrawSplitterAndBackground(drawRect);
                    if (showOrder)
                    {
                        var isSerie = "Serie".Equals(element.type);
                        var elementRect = isSerie ?
                            new Rect(drawRect.x, drawRect.y, drawRect.width + INDENT_WIDTH - 2 * ICON_GAP, drawRect.height) :
                            new Rect(drawRect.x, drawRect.y, drawRect.width - 4 * ICON_WIDHT, drawRect.height);
                        EditorGUI.PropertyField(elementRect, element, new GUIContent("Element " + i));
                        UpDownAddDeleteButton(drawRect, listProp, i);
                        drawRect.y += EditorGUI.GetPropertyHeight(element);
                        height += EditorGUI.GetPropertyHeight(element);
                    }
                    else
                    {
                        EditorGUI.PropertyField(drawRect, element, new GUIContent("Element " + i));
                        drawRect.y += EditorGUI.GetPropertyHeight(element);
                        height += EditorGUI.GetPropertyHeight(element);
                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        public static void UpDownAddDeleteButton(Rect drawRect, SerializedProperty listProp, int i)
        {
            var temp = INDENT_WIDTH + GAP_WIDTH + ICON_GAP;
            var iconRect = new Rect(drawRect.width - 4 * ICON_WIDHT + temp, drawRect.y, ICON_WIDHT, drawRect.height);
            var oldColor = GUI.contentColor;
            GUI.contentColor = Color.black;
            if (GUI.Button(iconRect, EditorCustomStyles.iconUp, EditorCustomStyles.invisibleButton))
            {
                if (i > 0) listProp.MoveArrayElement(i, i - 1);
            }
            iconRect = new Rect(drawRect.width - 3 * ICON_WIDHT + temp, drawRect.y, ICON_WIDHT, drawRect.height);
            if (GUI.Button(iconRect, EditorCustomStyles.iconDown, EditorCustomStyles.invisibleButton))
            {
                if (i < listProp.arraySize - 1) listProp.MoveArrayElement(i, i + 1);
            }
            iconRect = new Rect(drawRect.width - 2 * ICON_WIDHT + temp, drawRect.y, ICON_WIDHT, drawRect.height);
            if (GUI.Button(iconRect, EditorCustomStyles.iconAdd, EditorCustomStyles.invisibleButton))
            {
                if (i < listProp.arraySize && i >= 0) listProp.InsertArrayElementAtIndex(i);
            }
            iconRect = new Rect(drawRect.width - ICON_WIDHT + temp, drawRect.y, ICON_WIDHT, drawRect.height);
            if (GUI.Button(iconRect, EditorCustomStyles.iconRemove, EditorCustomStyles.invisibleButton))
            {
                if (i < listProp.arraySize && i >= 0) listProp.DeleteArrayElementAtIndex(i);
            }
            GUI.contentColor = oldColor;
        }

        public static bool PropertyField(ref Rect drawRect, Dictionary<string, float> heights, string key,
            SerializedProperty prop)
        {
            if (prop == null) return false;
            EditorGUI.PropertyField(drawRect, prop, true);
            var hig = EditorGUI.GetPropertyHeight(prop);
            drawRect.y += hig;
            heights[key] += hig;
            return true;
        }

        public static bool PropertyFieldWithMinValue(ref Rect drawRect, Dictionary<string, float> heights, string key,
            SerializedProperty prop, float minValue)
        {
            if (prop == null) return false;
            EditorGUI.PropertyField(drawRect, prop, true);
            if (prop.propertyType == SerializedPropertyType.Float && prop.floatValue < minValue)
                prop.floatValue = minValue;
            if (prop.propertyType == SerializedPropertyType.Integer && prop.intValue < minValue)
                prop.intValue = (int)minValue;
            var hig = EditorGUI.GetPropertyHeight(prop);
            drawRect.y += hig;
            heights[key] += hig;
            return true;
        }

        public static bool PropertyFieldWithMaxValue(ref Rect drawRect, Dictionary<string, float> heights, string key,
            SerializedProperty prop, float maxValue)
        {
            if (prop == null) return false;
            EditorGUI.PropertyField(drawRect, prop, true);
            if (prop.propertyType == SerializedPropertyType.Float && prop.floatValue > maxValue)
                prop.floatValue = maxValue;
            if (prop.propertyType == SerializedPropertyType.Integer && prop.intValue > maxValue)
                prop.intValue = (int)maxValue;
            var hig = EditorGUI.GetPropertyHeight(prop);
            drawRect.y += hig;
            heights[key] += hig;
            return true;
        }

        public static bool PropertyField(ref Rect drawRect, Dictionary<string, float> heights, string key,
            SerializedProperty parentProp, string relativeName)
        {
            return PropertyField(ref drawRect, heights, key, parentProp.FindPropertyRelative(relativeName));
        }
        public static bool PropertyFieldWithMinValue(ref Rect drawRect, Dictionary<string, float> heights, string key,
            SerializedProperty parentProp, string relativeName, float minValue)
        {
            var relativeProp = parentProp.FindPropertyRelative(relativeName);
            return PropertyFieldWithMinValue(ref drawRect, heights, key, relativeProp, minValue);
        }
        public static bool PropertyFieldWithMaxValue(ref Rect drawRect, Dictionary<string, float> heights, string key,
            SerializedProperty parentProp, string relativeName, float maxValue)
        {
            var relativeProp = parentProp.FindPropertyRelative(relativeName);
            return PropertyFieldWithMaxValue(ref drawRect, heights, key, relativeProp, maxValue);
        }

        public static GUIContent GetContent(string textAndTooltip)
        {
            if (string.IsNullOrEmpty(textAndTooltip))
                return GUIContent.none;

            GUIContent content;

            if (!s_GUIContentCache.TryGetValue(textAndTooltip, out content))
            {
                var s = textAndTooltip.Split('|');
                content = new GUIContent(s[0]);

                if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
                    content.tooltip = s[1];

                s_GUIContentCache.Add(textAndTooltip, content);
            }

            return content;
        }

        public static void DrawSplitter()
        {
            var rect = GUILayoutUtility.GetRect(1f, 1f);
            rect.xMin = 0f;
            rect.width += 4f;
            DrawSplitter(rect);
        }
        public static void DrawSplitter(Rect rect)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            EditorGUI.DrawRect(rect, EditorCustomStyles.splitter);
        }

        public static float DrawSplitterAndBackground(Rect drawRect, bool drawBackground = false)
        {
            float defaultWidth = drawRect.width;
            float defaultX = drawRect.x;

            var splitRect = drawRect;
            splitRect.y = drawRect.y;
            splitRect.x = EditorGUI.indentLevel * INDENT_WIDTH + 4;
            splitRect.xMax = drawRect.xMax;
            splitRect.height = 1f;

            DrawSplitter(splitRect);

            if (drawBackground)
            {
                var bgRect = drawRect;
                bgRect.y = drawRect.y;
                bgRect.x -= 10 - EditorGUI.indentLevel * INDENT_WIDTH;
                bgRect.xMax = drawRect.xMax;
                bgRect.height = HEADER_HEIGHT + (EditorGUI.indentLevel < 1 ? 2 : 0);
                EditorGUI.DrawRect(bgRect, EditorCustomStyles.headerBackground);
            }
            return HEADER_HEIGHT;
        }

        public static bool DrawHeader(string title, bool state, bool drawBackground, SerializedProperty activeField,
            Action<Rect> drawCallback, params HeaderMenuInfo[] menus)
        {
            var rect = GUILayoutUtility.GetRect(1f, HEADER_HEIGHT);
            var labelRect = DrawHeaderInternal(rect, title, ref state, drawBackground, activeField);
            DrawMenu(rect, menus);
            if (drawCallback != null)
            {
                drawCallback(rect);
            }
            var e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (labelRect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        state = !state;
                        e.Use();
                    }
                }
            }
            return state;
        }

        public static bool DrawSerieDataHeader(string title, bool state, bool drawBackground, SerializedProperty activeField,
            HeaderCallbackContext context, Action<Rect, HeaderCallbackContext> drawCallback, params HeaderMenuInfo[] menus)
        {
            var rect = GUILayoutUtility.GetRect(1f, HEADER_HEIGHT);
            var labelRect = DrawHeaderInternal(rect, title, ref state, drawBackground, activeField);
            DrawMenu(rect, menus);
            if (drawCallback != null)
            {
                drawCallback(rect, context);
            }
            var e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (labelRect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        state = !state;
                        e.Use();
                    }
                }
            }
            return state;
        }

        internal static bool DrawHeader(string title, bool state, bool drawBackground, SerializedProperty activeField,
            Action<Rect> drawCallback, List<HeaderMenuInfo> menus)
        {
            var rect = GUILayoutUtility.GetRect(1f, HEADER_HEIGHT);
            var labelRect = DrawHeaderInternal(rect, title, ref state, drawBackground, activeField);
            DrawMenu(rect, menus);
            if (drawCallback != null)
            {
                drawCallback(rect);
            }
            var e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (labelRect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        state = !state;
                        e.Use();
                    }
                }
            }
            return state;
        }

        private static Rect DrawHeaderInternal(Rect rect, string title, ref bool state, bool drawBackground, SerializedProperty activeField)
        {
            var splitRect = rect;
            splitRect.x = EditorGUI.indentLevel * INDENT_WIDTH + 4;
            splitRect.xMax = rect.xMax;
            splitRect.height = 1f;

            var backgroundRect = rect;
            backgroundRect.x = splitRect.x;
            backgroundRect.xMax = rect.xMax;

            var labelRect = rect;
            labelRect.xMin += 0f;
            labelRect.xMax -= 35f;

            var foldoutRect = rect;
            //foldoutRect.x -= 12f - EditorGUI.indentLevel * INDENT_WIDTH ;
            foldoutRect.x = rect.x - FOLDOUT_WIDTH + EditorGUI.indentLevel * INDENT_WIDTH + DIFF_WIDTH;
            foldoutRect.y += 1f;
            foldoutRect.width = FOLDOUT_WIDTH;
            foldoutRect.height = FOLDOUT_WIDTH;

            DrawSplitter(splitRect);
            if (drawBackground)
                EditorGUI.DrawRect(backgroundRect, EditorCustomStyles.headerBackground);
            if (!string.IsNullOrEmpty(title))
                EditorGUI.LabelField(labelRect, GetContent(title));
            state = GUI.Toggle(foldoutRect, state, GUIContent.none, EditorStyles.foldout);
            if (activeField != null)
            {
                var toggleRect = backgroundRect;
                toggleRect.x = rect.x + EditorGUIUtility.labelWidth - EditorGUI.indentLevel * INDENT_WIDTH + GAP_WIDTH;
                toggleRect.y += 1f;
                toggleRect.width = 13f;
                toggleRect.height = 13f;
                activeField.boolValue = GUI.Toggle(toggleRect, activeField.boolValue, GUIContent.none);
            }
            return labelRect;
        }

        internal static bool DrawHeader(string title, SerializedProperty group, SerializedProperty activeField,
            Action resetAction, Action removeAction, Action docAction)
        {
            if (group == null) return false;
            group.isExpanded = DrawHeader(title, group.isExpanded, false, activeField, null,
                new HeaderMenuInfo("Reset", resetAction),
                new HeaderMenuInfo("Remove", removeAction),
                new HeaderMenuInfo("HelpDoc", docAction));
            return group.isExpanded;
        }

        internal static bool DrawHeader(string title, SerializedProperty group, SerializedProperty activeField,
            params HeaderMenuInfo[] menus)
        {
            group.isExpanded = DrawHeader(title, group.isExpanded, false, activeField, null, menus);
            return group.isExpanded;
        }

        internal static bool DrawHeader(string title, SerializedProperty group, SerializedProperty activeField,
            List<HeaderMenuInfo> menus)
        {
            group.isExpanded = DrawHeader(title, group.isExpanded, false, activeField, null, menus);
            return group.isExpanded;
        }

        internal static void DrawMenu(Rect parentRect, params HeaderMenuInfo[] menus)
        {
            if (menus == null || menus.Length <= 0) return;
            var menuIcon = EditorCustomStyles.paneOptionsIcon;
            var menuRect = new Rect(parentRect.xMax - menuIcon.width, parentRect.y + 2f,
                menuIcon.width, menuIcon.height);
            GUI.DrawTexture(menuRect, menuIcon);
            var e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (menuRect.Contains(e.mousePosition))
                {
                    ShowHeaderContextMenu(new Vector2(menuRect.x, menuRect.yMax), menus);
                    e.Use();
                }
                else if (parentRect.Contains(e.mousePosition))
                {
                    if (e.button != 0)
                    {
                        ShowHeaderContextMenu(e.mousePosition, menus);
                        e.Use();
                    }
                }
            }
        }

        internal static void DrawMenu(Rect parentRect, List<HeaderMenuInfo> menus)
        {
            if (menus == null || menus.Count <= 0) return;
            var menuIcon = EditorCustomStyles.paneOptionsIcon;
            var menuRect = new Rect(parentRect.xMax - menuIcon.width, parentRect.y + 2f,
                menuIcon.width, menuIcon.height);
            GUI.DrawTexture(menuRect, menuIcon);
            var e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (menuRect.Contains(e.mousePosition))
                {
                    ShowHeaderContextMenu(new Vector2(menuRect.x, menuRect.yMax), menus);
                    e.Use();
                }
                else if (parentRect.Contains(e.mousePosition))
                {
                    if (e.button != 0)
                    {
                        ShowHeaderContextMenu(e.mousePosition, menus);
                        e.Use();
                    }
                }
            }
        }

        static void ShowHeaderContextMenu(Vector2 position, params HeaderMenuInfo[] menus)
        {
            if (menus == null || menus.Length <= 0) return;
            var menu = new GenericMenu();
            foreach (var info in menus)
            {
                if (info.enable)
                    menu.AddItem(GetContent(info.name), false, () => info.action());
                else
                    menu.AddDisabledItem(GetContent(info.name));
            }
            menu.DropDown(new Rect(position, Vector2.zero));
        }
        static void ShowHeaderContextMenu(Vector2 position, List<HeaderMenuInfo> menus)
        {
            if (menus == null || menus.Count <= 0) return;
            var menu = new GenericMenu();
            foreach (var info in menus)
            {
                if (info.enable)
                    menu.AddItem(GetContent(info.name), false, () => info.action());
                else
                    menu.AddDisabledItem(GetContent(info.name));
            }
            menu.DropDown(new Rect(position, Vector2.zero));
        }
    }
}